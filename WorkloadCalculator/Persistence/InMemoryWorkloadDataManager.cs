using System;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;
using System.Data.SQLite;
using System.Configuration;

namespace WorkloadCalculator.Persistence
{
    public class InMemoryWorkloadDataManager : IWorkloadDataManager
    {
        private string _connectionStringName = "workload_db";
        private SQLiteConnection _cnx;

        public InMemoryWorkloadDataManager()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            _cnx = new SQLiteConnection(connectionString);
            CreateTables();
        }

        private void CreateTables()
        {
            _cnx.Open();

            var sqlTableCourse = "create table course (name varchar, hours int)";
            using (SQLiteCommand cmd = new SQLiteCommand(sqlTableCourse, _cnx))
            {
                cmd.ExecuteNonQuery();
            }

            var sqlTableCalculation = "create table calculation (startdate int, enddate int, resulthours int, hoursperworkweek int, hoursperweek int)";
            using (SQLiteCommand cmd = new SQLiteCommand(sqlTableCalculation, _cnx))
            {
                cmd.ExecuteNonQuery();
            }

            var sqlTableSelectedCourses = "create table selectedcourses (workloadid int, courseid int)";
            using (SQLiteCommand cmd = new SQLiteCommand(sqlTableSelectedCourses, _cnx))
            {
                cmd.ExecuteNonQuery();
            }

            _cnx.Close();
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

