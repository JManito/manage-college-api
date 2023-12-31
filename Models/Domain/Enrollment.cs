﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManageCollege.Models.Domain
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; } = 0;
        [ForeignKey("Courses")]
        public int CourseId { get; set; } = 0;
        public int StudentId { get; set; } = 0;
    }
}
