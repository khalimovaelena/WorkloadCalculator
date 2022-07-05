using NUnit.Framework;
using System.Collections;
using WorkloadCalculator.Model;

namespace WorkloadCalculator.Tests
{
    internal static class TestData
    {
        public static IEnumerable TestDataForCalculation
        {
            get
            {
                yield return
                    //no courses and null dates
                    new TestCaseData(null, null, null);
                yield return
                    //1 course and correct dates
                    new TestCaseData(new List<Course>
                    {
                        new Course
                        {
                            ID = 1,
                            Name = "TestCourse1",
                            Hours = 8,
                        }
                    },
                    DateTime.Parse("01/09/2022"),
                    DateTime.Parse("01/10/2022"));
                yield return
                    //2 courses and correct dates
                    new TestCaseData(new List<Course>
                    {
                        new Course
                        {
                            ID = 1,
                            Name = "TestCourse1",
                            Hours = 8,
                        },
                        new Course
                        {
                            ID = 2,
                            Name = "TestCourse2",
                            Hours = 48,
                        }
                    },
                    DateTime.Parse("01/09/2022"),
                    DateTime.Parse("01/11/2022"));
                yield return
                    //2 courses and end date less than start date
                    new TestCaseData(new List<Course>
                    {
                        new Course
                        {
                            ID = 1,
                            Name = "TestCourse1",
                            Hours = 8,
                        },
                        new Course
                        {
                            ID = 2,
                            Name = "TestCourse2",
                            Hours = 48,
                        }
                    },
                    DateTime.Parse("01/09/2022"),
                    DateTime.Parse("01/08/2022"));
                yield return
                    //2 courses and start date in the past
                    new TestCaseData(new List<Course>
                    {
                        new Course
                        {
                            ID = 1,
                            Name = "TestCourse1",
                            Hours = 8,
                        },
                        new Course
                        {
                            ID = 2,
                            Name = "TestCourse2",
                            Hours = 48,
                        }
                    },
                    DateTime.Parse("01/09/2020"),
                    DateTime.Parse("01/11/2022"));
                yield return
                    //2 courses and less than 7 days between dates
                    new TestCaseData(new List<Course>
                    {
                        new Course
                        {
                            ID = 1,
                            Name = "TestCourse1",
                            Hours = 8,
                        },
                        new Course
                        {
                            ID = 2,
                            Name = "TestCourse2",
                            Hours = 48,
                        }
                    },
                    DateTime.Parse("01/09/2022"),
                    DateTime.Parse("04/09/2022"));
            }
        }
        public static IEnumerable TestDataForHistory
        {
            get
            {
                yield return
                    //empty history
                    new TestCaseData(null);
                yield return
                    //1 calculation in the history
                    new TestCaseData(new List<Calculation>
                    {
                        new Calculation
                        {
                            Id = 1,
                            StartDate = DateTime.Parse("01/09/2022"),
                            EndDate = DateTime.Parse("01/10/2022"),
                            ResultHours = 40,
                            HoursPerWeek = 10,
                            SelectedCourses = new List<Course>
                            {
                                new Course
                                {
                                    ID = 1,
                                    Name = "TestCourse1",
                                    Hours = 8,
                                },
                                new Course
                                {
                                    ID = 2,
                                    Name = "TestCourse2",
                                    Hours = 32,
                                }
                            }
                        }
                    });
                yield return
                    //2 calculations in the history
                    new TestCaseData(new List<Calculation>
                    {
                        new Calculation
                        {
                            Id = 1,
                            StartDate = DateTime.Parse("01/09/2022"),
                            EndDate = DateTime.Parse("01/10/2022"),
                            ResultHours = 40,
                            HoursPerWeek = 10,
                            SelectedCourses = new List<Course>
                            {
                                new Course
                                {
                                    ID = 1,
                                    Name = "TestCourse1",
                                    Hours = 8,
                                },
                                new Course
                                {
                                    ID = 2,
                                    Name = "TestCourse2",
                                    Hours = 32,
                                }
                            }
                        },
                        new Calculation
                        {
                            Id = 2,
                            StartDate = DateTime.Parse("01/09/2022"),
                            EndDate = DateTime.Parse("01/12/2022"),
                            ResultHours = 80,
                            HoursPerWeek = 5,
                            SelectedCourses = new List<Course>
                            {
                                new Course
                                {
                                    ID = 3,
                                    Name = "TestCourse3",
                                    Hours = 40,
                                },
                                new Course
                                {
                                    ID = 4,
                                    Name = "TestCourse4",
                                    Hours = 40,
                                }
                            }
                        }
                    });
            }
        }
    }
}
