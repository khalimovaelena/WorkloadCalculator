using System;

namespace WorkloadCalculator.Model
{
    /// <summary>
    /// Object to store the Course
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Course Internal Id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Course Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Course Duration (in Hours)
        /// </summary>
        public long Hours { get; set; }
    }
}

