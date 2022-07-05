using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;

namespace WorkloadCalculator.Controllers
{
    /// <summary>
    /// Controller to calculate workload
    /// </summary>
    [ApiController, Route("api/[controller]")]
    public class WorkloadController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWorkloadDataManager _dataManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="dataManager">DataManager to store results</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WorkloadController(ILogger logger, IWorkloadDataManager dataManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        /// <summary>
        /// Get all courses which can be learned
        /// </summary>
        /// <returns>Collection of <see cref="Course"/></returns>
        [HttpGet]
        [Route("courses")]
        public ICollection<Course> GetCourses()
        {
            return _dataManager.GetAllCourses();
        }

        /// <summary>
        /// Get history of all previous calculations
        /// </summary>
        /// <returns>Collection of <see cref="Calculation"/></returns>
        [HttpGet]
        [Route("history")]
        public ICollection<Calculation> GetHistory()
        {
           return _dataManager.CalculationHistory();
        }

        /// <summary>
        /// Calculate new workload for input data
        /// </summary>
        /// <param name="selectedCourses">Courses which user want to learn</param>
        /// <param name="startDate">Date when user wants to start learning</param>
        /// <param name="endDate">Date when user wants to finish learning</param>
        /// <returns><see cref="Calculation"/></returns>
        [HttpPost]
        [Route("calculate")]
        public Calculation? Calculate(ICollection<Course> selectedCourses, DateTime startDate, DateTime endDate)
        {
            if (startDate < DateTime.Today)
            {
                _logger.LogWarning($"Sorry! Start date can't be in the past. Calculate is cancelled.");
                return null;
            }
            else if (endDate < DateTime.Today)
            {
                _logger.LogWarning($"Sorry! End date can't be in the past. Calculate is cancelled.");
                return null;
            }
            else if (startDate > endDate)
            {
                _logger.LogWarning($"Sorry! Start date can't be greater than End date. Calculate is cancelled.");
                return null;
            }
            else
            {
                var calculation = new Calculation();
                calculation.StartDate = startDate;
                calculation.EndDate = endDate;
                calculation.ResultHours = selectedCourses.Sum(c => c.Hours);
                calculation.SelectedCourses = selectedCourses;

                double weeks = (endDate - startDate).Days/7;

                calculation.HoursPerWeek = weeks != 0 ? calculation.ResultHours/weeks : calculation.ResultHours;
                
                _dataManager.SaveCalculation(calculation);

                return calculation;
            }
        }
    }
}
