using System;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;

namespace WorkloadCalculator.Persistence
{
    public class InMemoryWorkloadDataManager : IWorkloadDataManager
    {

        public InMemoryWorkloadDataManager()
        {
            //TODO: create in-memory DataBase
        }

        public ICollection<Calculation> CalculationHistory()
        {
            throw new NotImplementedException();
        }

        public ICollection<Course> GetAllCourses()
        {
            throw new NotImplementedException();
        }

        public void SaveCalculation(Calculation calculation)
        {
            throw new NotImplementedException();
        }
    }
}

