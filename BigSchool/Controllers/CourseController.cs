using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        BigSchoolContext context = new BigSchoolContext();
        public ActionResult CreateCourse()
        {
            //lấy danh sách catalog
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(Course objCourse)
        {
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("CreateCourse", objCourse);
            }
            //lấy id user
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            //add vào db
            context.Courses.Add(objCourse);
            context.SaveChanges();
            // hoàn thanh quay về trang home
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext
                .Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance item in listAttendances)
            {
                Course objCourse = item.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext()
               .GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext
               .Current.User.Identity.GetUserId());
            var courses = context.Courses.Where(c=>c.LecturerId == currentUser.Id /*&& c.DateTime > DateTime.Now*/)
                .OrderByDescending(c=>c.DateTime).ToList();
            foreach (Course item in courses)
            {
                item.Name = currentUser.Name; //item name là tên khóa học, current user name là cột được thêm vào bảng aspnetuser
            }
            return View(courses);
        }

        public ActionResult LectureImGoing()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext()
               .GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext
               .Current.User.Identity.GetUserId());
            //danh sách người hướng dẫn được theo dõi (bởi user login) hiện tại
            var listFollowee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();

            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id 
                                    && p.Course.DateTime == DateTime.Now)
                .ToList();

            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollowee)
                {
                    if(item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objcourse = course.Course;
                        objcourse.Name = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(objcourse.LecturerId).Name;
                        courses.Add(objcourse);

                    }
                }
            }
            return View(courses);
        }
        
    }
}