using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WorkloadCalculator.Controllers;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;

namespace WorkloadCalculator.Tests
{
    [TestFixture]
    public class WorkloadControllerUnitTests
    {
        private Mock<IWorkloadDataManager> _dataManager;
        private Mock<ILogger> _logger;
        private WorkloadController _controller;

        [SetUp]
        public void SetUp()
        {
            _dataManager = new Mock<IWorkloadDataManager>();
            _logger = new Mock<ILogger>();
            _controller = new WorkloadController(_logger.Object, _dataManager.Object);
        }

        [Test]
        [TestCaseSource(
            typeof(TestData),
            nameof(TestData.TestDataForCalculation))]
        public void GetCourses(ICollection<Course> selectedCourses, DateTime startDate, DateTime endDate)
        {
            _dataManager.Setup(m => m.GetAllCourses()).Returns(selectedCourses);
            var result = _controller.GetCourses();
            if (selectedCourses != null)
            {
                Assert.NotNull(result);
                Assert.True(result.Count == selectedCourses.Count);
                foreach (var course in result)
                {
                    Assert.True(selectedCourses.Contains(course));
                }
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Test]
        [TestCaseSource(
            typeof(TestData),
            nameof(TestData.TestDataForHistory))]
        public void GetHistory(ICollection<Calculation> calculations)
        {
            _dataManager.Setup(m => m.CalculationHistory()).Returns(calculations);
            var result = _controller.GetHistory();
            if (calculations != null)
            {
                Assert.NotNull(result);
                Assert.True(result.Count == calculations.Count);
                foreach(Calculation calculation in result)
                {
                    Assert.True(calculations.Contains(calculation));
                }
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [Test]
        [TestCaseSource(
            typeof(TestData),
            nameof(TestData.TestDataForCalculation))]
        public void Calculate(ICollection<Course> selectedCourses, DateTime startDate, DateTime endDate)
        {
            var result = _controller.Calculate(selectedCourses, startDate, endDate);
            if (startDate < DateTime.Today || endDate < DateTime.Today || startDate > endDate)
            {
                Assert.IsNull(result); 
            }
            else
            {
                Assert.True(result.StartDate.Equals(startDate));
                Assert.True(result.EndDate.Equals(endDate));
                
                foreach(Course course in result.SelectedCourses)
                {
                    Assert.True(selectedCourses.Contains(course));
                }
                
                Assert.True(result.ResultHours == selectedCourses.Sum(c => c.Hours));
                double weeks = (endDate - startDate).Days / 7;
                Assert.True(result.HoursPerWeek == (weeks != 0 ? result.ResultHours / weeks : result.ResultHours));
            }
        }
    }
}
