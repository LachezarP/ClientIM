using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.ActionFilter
{
    public class NotifyFriendRequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Models.ClientEntities db = new Models.ClientEntities();

            int personId = Int32.Parse(filterContext.HttpContext.Session["person_id"].ToString());

            int counterFriend = Int32.Parse(filterContext.HttpContext.Session["friend_request"].ToString());

            int counterMessage = Int32.Parse(filterContext.HttpContext.Session["new_message"].ToString());
            if (counterFriend == 0)
            {
                foreach (Models.FriendLink MF in db.FriendLinks.ToList())
                {
                    if (MF.requested == personId && MF.read.Equals("Not read"))
                    {
                        counterFriend++;
                        filterContext.HttpContext.Session["friend_request"] = counterFriend;
                    }
                }
            }

            if (counterMessage == 0) { 
                foreach (Models.Message MM in db.Messages.ToList())
                {
                    if (MM.receiver == personId && MM.read.Equals("Not read"))
                    {
                        counterMessage++;
                        filterContext.HttpContext.Session["new_message"] = counterMessage;
                    }
                }
            }

            int counterLike = Int32.Parse(filterContext.HttpContext.Session["new_Likes"].ToString());

            IEnumerable<Models.Picture> yourPics = db.Pictures.Where(c => c.person_id == personId);

            if (counterLike == 0)
            {
                foreach (Models.Like ML in db.Likes.ToList())
                {
                    if (yourPics.Contains(db.Pictures.SingleOrDefault(c => c.picture_id == ML.picture_id)) && ML.read.Equals("Not read"))
                    {
                        counterLike++;
                        filterContext.HttpContext.Session["new_Likes"] = counterLike;
                    }
                }
            }

            int counterCommentLike = Int32.Parse(filterContext.HttpContext.Session["new_CommentLikes"].ToString());

            IEnumerable<Models.Comment> yourComment = db.Comments.Where(c => c.person_id == personId);

            if (counterCommentLike == 0)
            {
                foreach (Models.Comment_like ML in db.Comment_like.ToList())
                {
                    if (yourComment.Contains(db.Comments.SingleOrDefault(c => c.comment_id == ML.comment_id)) && ML.read.Equals("Not read"))
                    {
                        counterCommentLike++;
                        filterContext.HttpContext.Session["new_CommentLikes"] = counterCommentLike;
                    }
                }
            }
        }
    }
}