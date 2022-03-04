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
    public class FollowingController : ApiController
    {
        BigSchoolContext context = new BigSchoolContext();

        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            //user login là người theo dõi, follow.FolloweeId là người được theo dõi
            var userID = User.Identity.GetUserId();
            if (userID == null) return BadRequest("Bạn cần đăng nhập để thực hiện!");
            if (userID == follow.FolloweeId) return BadRequest("Không thể theo dõi chính bạn!");
           
            //kiểm tra 
            Following find = context.Followings
                .FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId);
            if (find != null) //return BadRequest("Đã theo dõi người hướng dẫn này!");
            {
                context.Followings.Remove(context.Followings.SingleOrDefault(p =>
                                p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                                context.SaveChanges();
                                return Ok("cancel");
            }
            //thực hiện thao tác theo dõi
            follow.FollowerId = userID;
            context.Followings.Add(follow);
            context.SaveChanges();
            return Ok();
        }
    }
}
