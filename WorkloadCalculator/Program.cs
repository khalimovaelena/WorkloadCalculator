using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using WorkloadCalculator.Controllers;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Persistence;

namespace WorkloadCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILogger _logger = new ConsoleLogger<Program>();
            string datePattern = @"^(([012]\d)|3[01])/((0\d)|(1[012]))/\d{4}$";
            Regex regExp = new Regex(datePattern, RegexOptions.IgnoreCase);

            try
            {
                IWorkloadDataManager _dataManager = new InMemoryWorkloadDataManager(_logger);
                WorkloadController _controller = new WorkloadController(_logger, _dataManager);
                var courses = _controller.GetCourses();

                _logger.LogInformation($"Hello! Now we have {courses.Count} courses for you:");
                courses
                    .Where(c => c.Name != null)
                    .ToList()
                    .ForEach(c => _logger.LogInformation($"NUMBER: {c.ID}, COURSE NAME: {c.Name}, HOURS: {c.Hours}"));

                _logger.LogInformation($"Please enter the numbers of courses which you want to learn (using delimeter comma or space) or print 'EXIT' if you want to close the Application and delete all data:");

                //TODO: cycle and method, if something entered wrong - back to start
                //TODO: check numbers
                string line = Console.ReadLine();

                if (line == null)
                {
                    _logger.LogWarning($"You didn't enter anything");
                }
                else if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("You chose to close the program. All data will be deleted! Good bye!");
                    Environment.Exit(0);
                }
                else
                {
                    ICollection<long> numbers = line
                        .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => {long.TryParse(s, out long number); return number; })
                        .Where(n => n != 0)
                        .ToList();

                    _logger.LogInformation("Thank you! You've entered:");

                    numbers.ToList().ForEach(n => _logger.LogInformation($"{n} "));

                    _logger.LogInformation($"Now please enter DATE when you want to START these courses in format DD/MM/YYYY:");

                    string startDate = Console.ReadLine();//TODO: check date not in the past
                    if (startDate == null || !regExp.IsMatch(startDate))
                    {
                        _logger.LogWarning($"Sorry! Wrong date format");
                    }
                    else
                    {
                        _logger.LogInformation($"Thank you! Now please enter DATE when you want to FINISH these courses in format DD/MM/YYYY:");

                        string endDate = Console.ReadLine();//TODO: check date more than startDate
                        if (endDate == null || !regExp.IsMatch(endDate))
                        {
                            _logger.LogWarning($"Sorry! Wrong date format");
                        }
                        else
                        {
                            _logger.LogInformation("Thank you! Now please enter Y if you want to calculate your workload or N if you want to go back and change your data:");
                            string calculate = Console.ReadLine();

                            if (calculate.Equals("N", StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogWarning($"Resetting data...");
                            }
                            else
                            {
                                _logger.LogInformation(_controller.Calculate()?.HoursPerWeek.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an ERROR during executing Main method: {ex.Message}", ex);
            }
        }
    }
}
