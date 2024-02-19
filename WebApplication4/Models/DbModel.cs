using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required, StringLength(50)]
        public string CourseName { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
    public class Student
    {
        public int StudentId { get; set; }
        [Required, StringLength(50)]
        public string StudentName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? JoinDate { get; set; }
        [Required, StringLength(30)]
        public string Picture { get; set; }
        [Required, ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
    public class CourseDbContext : DbContext
    {
        public CourseDbContext()
        {
            Database.SetInitializer(new DbInitializer());
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<CourseDbContext>
    {
        protected override void Seed(CourseDbContext context)
        {
            Course d = new Course { CourseName = "IT" };
            d.Students.Add(new Student { StudentName = "S1", JoinDate = DateTime.Parse("2023-02-12"), Picture = "s1.jpeg" });
            d.Students.Add(new Student { StudentName = "S2", JoinDate = DateTime.Parse("2023-07-12"), Picture = "s2.jpeg" });
            context.Courses.Add(d);
            context.SaveChanges();
        }
    }
}