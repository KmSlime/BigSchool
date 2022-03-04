namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        public int CourseId { get; set; }

        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required(ErrorMessage = "Nhập sai!")]
        [StringLength(255)]
        public string Place { get; set; }

        [Required(ErrorMessage = "Sai định dạng ngày!")]
        public DateTime? DateTime { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        //public string LecturerName { get; internal set; }

        //public int Id { get; set; }

        //public string Attendee { get; set; }

        //add list category
        public List<Category> ListCategory = new List<Category>();      
        public string Name;
        //public string LecturerName;

        public bool isLogin = false;
        public bool isShowGoing = false;
        public bool isShowFollow = false;


    }
}
