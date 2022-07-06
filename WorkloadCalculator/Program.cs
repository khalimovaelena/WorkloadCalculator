using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using WorkloadCalculator.Controllers;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;
using WorkloadCalculator.Persistence;

namespace WorkloadCalculator
{
    internal class Program
    {
        private static ICollection<Course> _selectedCourses;
        private static ICollection<Course> _courses;

        private static ILogger _logger = new ConsoleLogger<Program>();
        private static string _datePattern = @"^(([012]\d)|3[01])/((0\d)|(1[012]))/\d{4}$";
        private static WorkloadController _controller;

        static void Main(string[] args)
        {
            using (IWorkloadDataManager _dataManager = new InMemoryWorkloadDataManager(_logger))
            {
                try
                {
                    _controller = new WorkloadController(_logger, _dataManager);
                    _courses = _controller.GetCourses();

                    _logger.LogInformation($"Hello! Now we have {_courses.Count} courses for you:");
                    _courses
                        .Where(c => c.Name != null)
                        .ToList()
                        .ForEach(c => _logger.LogInformation($"NUMBER: {c.ID}, COURSE NAME: {c.Name}, HOURS: {c.Hours}"));

                    _logger.LogInformation("");//Empty Line

                    var readValue = "";
                    while (!readValue.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($"Please enter 'C' if you want to calculate new workload and 'H' if you want to see the history of previous calculations. To exit from the program enter 'EXIT'");
                        readValue = Console.ReadLine();

                        if (readValue.Equals("C", StringComparison.OrdinalIgnoreCase))
                        { Calculate(); }
                        else if (readValue.Equals("H", StringComparison.OrdinalIgnoreCase))
                        { PrintHistory(); }
                        else { _logger.LogWarning("Wrong input data!"); }

                        _logger.LogInformation("");
                    }

                    _logger.LogInformation("You chose to close the program. All data will be deleted! Good bye!");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"There is an ERROR during executing Main method: {ex.Message}", ex);
                }
            }
        }

        private static ICollection<Course>? ReadSelectedCourses()
        {
            ICollection<Course> selectedCourses;
            string line = Console.ReadLine();

            if (line == null)
            {
                _logger.LogWarning($"You didn't enter anything.");
                return null;
            }
            else if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("You chose to close the program. All data will be deleted! Good bye!");
                Environment.Exit(0);
            }

            var numbers = line
                        .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => { long.TryParse(s, out long number); return number; })
                        .Where(n => n != 0)
                        .ToList();

            _logger.LogInformation("Thank you! You've entered:");

            numbers.ToList().ForEach(n => _logger.LogInformation($"{n} "));

            selectedCourses = _courses.Where(c => numbers.Contains(c.ID)).ToList();
            return selectedCourses;
        }

        private static DateTime? ReadDate()
        {
            Regex regExp = new Regex(_datePattern, RegexOptions.IgnoreCase);
            string dateStr = Console.ReadLine();

            if (dateStr.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("You chose to close the program. All data will be deleted! Good bye!");
                Environment.Exit(0);
            } else if (dateStr == null || !regExp.IsMatch(dateStr))
            {
                _logger.LogWarning($"Sorry! Wrong date format");
                return null;
            }

            var result = DateTime.TryParse(dateStr, out var dateResult);
            if (!result)
            {
                _logger.LogError($"Sorry! Wrong date format!");
                return null;
            }

            return dateResult;
        }

        private static void Calculate()
        {
            _logger.LogInformation($"Please enter the numbers of courses which you want to learn (using delimeter comma or space) or print 'EXIT' if you want to close the Application and delete all data:");
            _selectedCourses = ReadSelectedCourses();

            while (_selectedCourses == null)
            {
                _selectedCourses = ReadSelectedCourses();
            }

            _logger.LogInformation($"Now please enter DATE when you want to START these courses in format DD/MM/YYYY:");
            DateTime? startDate = ReadDate();

            while (startDate == null)
            {
                _logger.LogWarning($"Please enter valid Start Date");
                startDate = ReadDate();
            }

            _logger.LogInformation($"Thank you! Now please enter DATE when you want to FINISH these courses in format DD/MM/YYYY:");
            DateTime? endDate = ReadDate();

            while (endDate == null)
            {
                _logger.LogWarning($"Please enter valid End Date");
                endDate = ReadDate();
            }

            _logger.LogInformation("Thank you! Now please enter Y if you want to confirm your workload or N if you want to go back and change your data:");

            string confirm = Console.ReadLine();

            if (confirm.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Resetting data...");
                return;
            }

            var calculation = _controller.Calculate(_selectedCourses, (DateTime)startDate, (DateTime)endDate);
            _logger.LogInformation("Calculated values:");
            _logger.LogInformation($"ResultHours={calculation?.ResultHours}, HoursPerWeek={calculation?.HoursPerWeek}");
            return;
        }

        private static void PrintHistory()
        {
            _logger.LogInformation("You have already calculated:");
            var history = _controller.GetHistory();
            foreach(var calculation in history)
            {
                _logger.LogInformation($"Number: {calculation.Id}, StartDate: {calculation.StartDate.ToString("dd/MM/yyyy")}, EndDate: {calculation.EndDate.Date.ToString("dd/MM/yyyy")}, TotalHours: {calculation.ResultHours}, HoursPerWeek: {calculation.HoursPerWeek:N2}, Courses: [{String.Join(", ", calculation.SelectedCourses.Select(sc => sc.Name))}]");
            }
        }
    }
}
