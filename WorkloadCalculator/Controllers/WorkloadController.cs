using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;

namespace WorkloadCalculator.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class WorkloadController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWorkloadDataManager _dataManager;

        public WorkloadController(ILogger logger, IWorkloadDataManager dataManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        [HttpGet]
        [Route("courses")]
        public ICollection<Course> GetCourses()
        {
            return _dataManager.GetAllCourses();
        }

        [HttpPost]
        [Route("calculate")]
        public Calculation Calculate(ICollection<Course> selectedCourses, DateTime startDate, DateTime endDate)
        {
            var calculation = new Calculation();

            if (startDate < DateTime.Today)
            {
                _logger.LogWarning($"Sorry! Start date can't be in the past. Calculate is cancelled.");
            }
            else if (endDate < DateTime.Today)
            {
                _logger.LogWarning($"Sorry! End date can't be in the past. Calculate is cancelled.");
            }
            else if (startDate > endDate)
            {
                _logger.LogWarning($"Sorry! Start date can't be greater than End date. Calculate is cancelled.");
            }
            else
            {
                calculation.StartDate = startDate;
                calculation.EndDate = endDate;
                calculation.ResultHours = selectedCourses.Sum(c => c.Hours);
                calculation.SelectedCourses = selectedCourses;

                double weeks = (endDate - startDate).Days/7;

                calculation.HoursPerWeek = weeks != 0 ? calculation.ResultHours/weeks : calculation.ResultHours;
                
                _dataManager.SaveCalculation(calculation);
            }

            return calculation;
        }
    }
}
