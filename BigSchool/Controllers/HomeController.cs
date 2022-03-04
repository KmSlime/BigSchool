using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        BigSchoolContext context = new BigSchoolContext();
        public ActionResult Index()
        {
            //Hiển thị khóa học
            var upcommingCourse = context.Courses//.Include(x=>x.Category).Where(x => x.DateTime > DateTime.Now)
                .OrderByDescending(x => x.DateTime).ToList();
            //Lấy id user login hiện tại
            var userID = User.Identity.GetUserId();
            foreach (Course i in upcommingCourse)
            {
                //Lấy name của user từ lecturerID
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
                //i.Category = (Category)context.Categories.Select(x => x.CategoryName);
                
                //Lấy danh sách tham gia khóa học
                if(userID != null)  //nếu user đã đăng nhập
                {
                    i.isLogin = true;
                    //kiểm tra user đã tham gia khóa học hay chưa
                    Attendance find = context.Attendance
                        .FirstOrDefault(p => p.CourseId == i.CourseId && p.Attendee == userID);
                    if (find == null) i.isShowGoing = true;

                    //kiểm tra user đã theo dõi giảng viên của khóa học hay chưa
                    Following findFollow = context.Followings
                        .FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == i.LecturerId);
                    if (findFollow == null) i.isShowFollow = true;
                }
            }
            return View(upcommingCourse);
        }
        public ActionResult Buy(int id)
        {
            var b = context.Courses.SingleOrDefault(s => s.CourseId == id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}