using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers.API
{
    public class AttendanceController : ApiController
    {
        BigSchoolContext context = new BigSchoolContext();
        [Authorize]
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDTO)
        {
            var userID = User.Identity.GetUserId();
            if (context.Attendance.Any(p => p.Attendee == userID && p.CourseId == attendanceDTO.CourseId))
            {
                //return BadRequest("The attendance already exists!");
                // xóa thông tin khóa học đã đăng kí tham gia trong bảng attendances
                context.Attendance.Remove(context.Attendance
                    .SingleOrDefault(p=>p.Attendee == userID && p.CourseId == attendanceDTO.CourseId));
                context.SaveChanges();
                return Ok("Cancel");
            }
            var attendance = new Attendance()
            {
                CourseId = attendanceDTO.CourseId,
                Attendee = User.Identity.GetUserId()
            };
            context.Attendance.Add(attendance);
            context.SaveChanges();
            return Ok();
        }
    }
}
