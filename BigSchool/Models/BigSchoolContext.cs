using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BigSchool.Models
{
    public partial class BigSchoolContext : DbContext
    {
        public BigSchoolContext()
            : base("name=BigSchoolContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Following> Followings { get; set; }


        //public object Attendances { get; internal set; } //API gọi

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
