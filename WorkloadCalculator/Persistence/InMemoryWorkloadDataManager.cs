using System;
using WorkloadCalculator.Interfaces;
using WorkloadCalculator.Model;
using System.Data.SQLite;
using System.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace WorkloadCalculator.Persistence
{
    public class InMemoryWorkloadDataManager : IWorkloadDataManager, IDisposable
    {
        private string _connectionStringName = "workload_db";
        private SQLiteConnection _sqlConnection;
        private ILogger _logger;

        public InMemoryWorkloadDataManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName]?.ConnectionString ?? throw new ArgumentNullException("ConnectionString");

            _sqlConnection = new SQLiteConnection(connectionString);
            _sqlConnection.Open();
            CreateTables();
            InitialFilling();
        }

        private void CreateTables()
        {
            try
            {
                var sqlTableCourse = "create table course (id int primary key, name varchar, hours int)";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlTableCourse, _sqlConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                var sqlTableCalculation = "create table calculation (id int primary key, startdate int, enddate int, resulthours int, hoursperworkweek int, hoursperweek int)";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlTableCalculation, _sqlConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                var sqlTableSelectedCourses = "create table selectedcourses (id int primary key, workloadid int, courseid int)";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlTableSelectedCourses, _sqlConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                _logger.LogError($"There is an ERROR during executing query:{ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"There is an error during connecting to the database:{ex.Message}", ex);
            }
        }

        private void InitialFilling()
        {
            var initialCourses = new List<Course>();
            initialCourses.Add(new Course()
            {
                ID = 1,
                Name = "Blockchain and HR",
                Hours = 8
            }) ;
            initialCourses.Add(new Course()
            {
                ID = 2,
                Name = "Compensation & Benefits",
                Hours = 32
            });
            initialCourses.Add(new Course()
            {
                ID = 3,
                Name = "Digital HR",
                Hours = 40
            });
            initialCourses.Add(new Course()
            {
                ID = 4,
                Name = "Digital HR Strategy",
                Hours = 10
            });
            initialCourses.Add(new Course()
            {
                ID = 5,
                Name = "Digital HR Transformation",
                Hours = 8
            });
            initialCourses.Add(new Course()
            {
                ID = 6,
                Name = "Diversity & Inclusion",
                Hours = 20
            });
            initialCourses.Add(new Course()
            {
                ID = 7,
                Name = "Employee Experience & Design Thinking",
                Hours = 12
            });
            initialCourses.Add(new Course()
            {
                ID = 8,
                Name = "Employer Branding",
                Hours = 6
            });
            initialCourses.Add(new Course()
            {
                ID = 9,
                Name = "Global Data Integrity",
                Hours = 12
            });
            initialCourses.Add(new Course()
            {
                ID = 10,
                Name = "Hiring & Recruitment Strategy",
                Hours = 15
            });
            initialCourses.Add(new Course()
            {
                ID = 11,
                Name = "HR Analytics Leader",
                Hours = 21
            });
            initialCourses.Add(new Course()
            {
                ID = 12,
                Name = "HR Business Partner 2.0",
                Hours = 40
            });
            initialCourses.Add(new Course()
            {
                ID = 13,
                Name = "HR Data Analyst",
                Hours = 18
            });
            initialCourses.Add(new Course()
            {
                ID = 14,
                Name = "HR Data Science in R",
                Hours = 12
            });
            initialCourses.Add(new Course()
            {
                ID = 15,
                Name = "HR Data Visualization",
                Hours = 12
            });
            initialCourses.Add(new Course()
            {
                ID = 16,
                Name = "HR Metrics & Reporting",
                Hours = 40
            });
            initialCourses.Add(new Course()
            {
                ID = 17,
                Name = "Learning & Development",
                Hours = 30
            });
            initialCourses.Add(new Course()
            {
                ID = 18,
                Name = "Organizational Development",
                Hours = 30
            });
            initialCourses.Add(new Course()
            {
                ID = 19,
                Name = "People Analytics",
                Hours = 40
            });
            initialCourses.Add(new Course()
            {
                ID = 20,
                Name = "Statistics in HR",
                Hours = 15
            });
            initialCourses.Add(new Course()
            {
                ID = 21,
                Name = "Strategic HR Leadership",
                Hours = 34
            });
            initialCourses.Add(new Course()
            {
                ID = 22,
                Name = "BStrategic HR Metrics",
                Hours = 17
            });
            initialCourses.Add(new Course()
            {
                ID = 23,
                Name = "Talent Acquisition",
                Hours = 40
            });

            FillCourses(initialCourses);
        }

        public void FillCourses(ICollection<Course> courses)
        {
            try
            {
                using (SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO course (Id, name, Hours) VALUES (@Id, @Name, @Hours)", _sqlConnection))
                {
                    foreach (Course course in courses)
                    {
                        insertSQL.CommandType = CommandType.Text;
                        insertSQL.Parameters.AddWithValue("Id", course.ID);
                        insertSQL.Parameters.AddWithValue("Name", course.Name);
                        insertSQL.Parameters.AddWithValue("Hours", course.Hours);
                        try
                        {
                            insertSQL.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"There is an ERROR during executing INSERT query:{ex.Message}", ex);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _logger.LogError($"There is an ERROR during executing INSERT query:{ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"There is an error during connecting to the database:{ex.Message}", ex);
            }
        }

        public ICollection<Calculation> CalculationHistory()
        {
            throw new NotImplementedException();
        }

        public ICollection<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            try
            {
                using (SQLiteCommand selectCmd = _sqlConnection.CreateCommand())
                {
                    selectCmd.CommandText = @"SELECT id, name, hours FROM course";
                    selectCmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = selectCmd.ExecuteReader();
                    while (r.Read())
                    {
                        courses.Add(new Course() 
                        { 
                            ID = Convert.ToInt32(r["id"]),
                            Name = Convert.ToString(r["name"]),
                            Hours = Convert.ToInt32(r["hours"]),
                        });
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _logger.LogError($"There is an ERROR during executing SELECT query:{ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"There is an error during connecting to the database:{ex.Message}", ex);
            }
            return courses;
        }

        public void SaveCalculation(Calculation calculation)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}

