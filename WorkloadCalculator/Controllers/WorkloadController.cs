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
        public Calculation Calculate()
        {
            return new Calculation();//TODO
        }
    }
}
