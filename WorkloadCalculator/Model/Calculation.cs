using System;
using System.Collections.Generic;

namespace WorkloadCalculator.Model
{
    /// <summary>
    /// Object to store Workload Calculation
    /// </summary>
    public class Calculation
    {
        /// <summary>
        /// Internal Object Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Student learning Start Date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Student learning End Date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Sum of hours that is needed to finish all selected courses
        /// </summary>
        public long ResultHours { get; set; }

        /// <summary>
        /// Sum of hours in a work week (Mon-Fri) that is needed to finish all selected courses
        /// </summary>
        public long HoursPerWorkWeek { get; set; }

        /// <summary>
        /// Sum of hours in a whole week (Mon-Sun) that is needed to finish all selected courses
        /// </summary>
        public long HoursPerWeek { get; set; }

        /// <summary>
        /// Enumeration of Selected Courses
        /// </summary>
        public ICollection<Course> SelectedCourses { get; set; }
    }
}

