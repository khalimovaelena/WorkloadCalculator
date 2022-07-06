using WorkloadCalculator.Model;

namespace WorkloadCalculator.Interfaces
{
    /// <summary>
    /// Interface to connect to the DataBase which stores Workload data
    /// </summary>
    public interface IWorkloadDataManager : IDisposable
    {
        /// <summary>
        /// Save collection with courses to database
        /// </summary>
        /// <param name="courses"></param>
        void FillCourses(ICollection<Course> courses);

        /// <summary>
        /// Gets all courses from the DataBase
        /// </summary>
        /// <returns>Collection of Courses</returns>
        ICollection<Course> GetAllCourses();

        /// <summary>
        /// Save Calculate to the DataBase
        /// </summary>
        /// <param name="calculation">Workload Calculate</param>
        void SaveCalculation(Calculation calculation);

        /// <summary>
        /// Gets history of all calculations
        /// </summary>
        /// <returns>Collection of Calculations</returns>
        ICollection<Calculation> CalculationHistory();
    }
}

