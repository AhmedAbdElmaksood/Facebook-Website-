using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FacebookApp.Models;
using FacebookApp.Data;
using Microsoft.AspNetCore.Authorization;
using FacebookApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FacebookApp.Controllers
{
    [Authorize(Policy = "BlockBlocked")]
    //[Authorize(Roles ="member,admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<User> UserManager, RoleManager<Role> RoleManager)
        {
            _logger = logger;
            context = db;
            this._userManager = UserManager;
            this._roleManager = RoleManager;


        }

        //#region Timeline
        //[Authorize(Roles = "member")]
        //public IActionResult Index(int flag)
        //{
        //    ViewBag.flag = flag;
        //    //return View(context.Posts.OrderByDescending(p => p.PostingDate).ToList());

        //    var id = _userManager.GetUserId(User);

        //    var userFromDb = context.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
        //    var userVM = new UserViewModel()
        //    {
        //        Nickname = userFromDb.Nickname,
        //        Bio = userFromDb.Bio,
        //        BirthDate = userFromDb.BirthDate,
        //        Gender = userFromDb.Gender,
        //        Image = userFromDb.Image,
        //        Email = userFromDb.Email,
        //        UserId = userFromDb.Id,
        //    };

        //    IEnumerable<Post> postsList = context.Posts.Where(p => p.IsDeleted == false);
        //    var postVMLst = new List<PostsViewModel>();
        //    foreach (var post in postsList)
        //    {
        //        var currentUserId = _userManager.GetUserId(User);
        //        var isAFriend = context.UserHasFriends.FirstOrDefault(u => (u.UserId == post.UserId || u.FriendId == post.UserId) && (u.UserId == currentUserId || u.FriendId == currentUserId) && u.Status == FriendRequestStatus.Friend);
        //        if (isAFriend != null || post.UserId == currentUserId)
        //        {
        //            var postVM = new PostsViewModel();
        //            postVM.Content = post.Content;
        //            postVM.IsDeleted = post.IsDeleted;
        //            postVM.PostId = post.Id;
        //            postVM.UserId = post.UserId;
        //            postVM.PostingDate = post.PostingDate;

        //            IEnumerable<UserLikesPost> likesList = context.UserLikesPosts.Where(l => l.PostId == post.Id && l.IsLiked == true);
        //            IEnumerable<UserCommentsOnPost> commentsList = context.UserCommentsOnPosts.Where(c => c.PostId == post.Id && c.IsDeleted == false);

        //            var likesVMLst = new List<LikesViewModel>();
        //            var commentsVMLst = new List<CommentsViewModel>();

        //            foreach (var like in likesList)
        //            {
        //                var likeVM = new LikesViewModel();
        //                likeVM.UserId = like.UserId;
        //                likeVM.PostId = like.PostId;
        //                likeVM.IsLiked = like.IsLiked;
        //                likesVMLst.Add(likeVM);
        //            }
        //            postVM.likesVMList = likesVMLst;

        //            foreach (var comment in commentsList)
        //            {
        //                var commentVM = new CommentsViewModel();
        //                commentVM.Content = comment.Content;
        //                commentVM.UserId = comment.UserId;
        //                commentVM.IsDeleted = comment.IsDeleted;
        //                commentVM.PostId = comment.PostId;
        //                commentVM.CommentId = comment.CommentId;
        //                commentsVMLst.Add(commentVM);
        //            }
        //            postVM.commentsVMList = commentsVMLst;


        //            postVMLst.Add(postVM);
        //        }
        //    }

        //    userVM.Posts = postVMLst;

        //    //postVMLst.OrderByDescending(p => p.PostingDate)
        //    return View(userVM);
        //}
        //#endregion

        #region Timeline


        public IActionResult Index(int flag)
        {
            ViewBag.flag = flag;
            int FriendsCount = 0;

            var id = _userManager.GetUserId(User);

            var userFromDb = context.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
            var userVM = new UserViewModel()
            {
                Nickname = userFromDb.Nickname,
                Bio = userFromDb.Bio,
                BirthDate = userFromDb.BirthDate,
                Gender = userFromDb.Gender,
                Image = userFromDb.Image,
                Email = userFromDb.Email,
                UserId = userFromDb.Id,
            };

            IEnumerable<Post> postsList = context.Posts.Where(p => p.IsDeleted == false);
            var postVMLst = new List<PostsViewModel>();
            foreach (var post in postsList)
            {
                var currentUserId = _userManager.GetUserId(User);
                var isAFriend = context.UserHasFriends.FirstOrDefault(u => (u.UserId == post.UserId || u.FriendId == post.UserId) && (u.UserId == currentUserId || u.FriendId == currentUserId) && u.Status == FriendRequestStatus.Friend);
                if (isAFriend != null || post.UserId == currentUserId)
                {
                    var postVM = new PostsViewModel();
                    postVM.Content = post.Content;
                    postVM.IsDeleted = post.IsDeleted;
                    postVM.PostId = post.Id;
                    postVM.UserId = post.UserId;
                    postVM.PostingDate = post.PostingDate;
                    postVM.Image = post.Image;

                    IEnumerable<UserLikesPost> likesList = context.UserLikesPosts.Where(l => l.PostId == post.Id && l.IsLiked == true);
                    IEnumerable<UserCommentsOnPost> commentsList = context.UserCommentsOnPosts.Where(c => c.PostId == post.Id && c.IsDeleted == false);

                    var likesVMLst = new List<LikesViewModel>();
                    var commentsVMLst = new List<CommentsViewModel>();

                    foreach (var like in likesList)
                    {
                        var likeVM = new LikesViewModel();
                        likeVM.UserId = like.UserId;
                        likeVM.PostId = like.PostId;
                        likeVM.IsLiked = like.IsLiked;
                        likesVMLst.Add(likeVM);
                    }
                    postVM.likesVMList = likesVMLst;

                    foreach (var comment in commentsList)
                    {
                        var commentVM = new CommentsViewModel();
                        commentVM.Content = comment.Content;
                        commentVM.UserId = comment.UserId;
                        commentVM.IsDeleted = comment.IsDeleted;
                        commentVM.PostId = comment.PostId;
                        commentVM.CommentId = comment.CommentId;
                        commentsVMLst.Add(commentVM);
                    }
                    postVM.commentsVMList = commentsVMLst;


                    postVMLst.Add(postVM);
                }
            }

            userVM.Posts = postVMLst;

            //Friends
            FriendsCount = context.UserHasFriends.Where(u => (u.UserId == _userManager.GetUserId(User) || u.FriendId == _userManager.GetUserId(User)) && u.Status == FriendRequestStatus.Friend).Count();
            //foreach (var friend in friendsList)
            //{
            //    var pendingFriends = context.UserHasFriends.FirstOrDefault(f => (f.UserId == _userManager.GetUserId(User) || f.FriendId == _userManager.GetUserId(User)) && f.Status == FriendRequestStatus.Pending);
            //    var friendId = (pendingFriends.FriendId != _userManager.GetUserId(User)) ? pendingFriends.FriendId : pendingFriends.UserId;
            //    friendsVMList.Add(new FriendsViewModel()
            //    {
            //        Email = context.Users.FirstOrDefault(f => f.Id == friendId).Email,
            //        //TODO: Image
            //        Image = context.Users.FirstOrDefault(f => f.Id == friendId).Image,
            //    });
            //}
            ViewBag.FriendsCount = FriendsCount;

            //return View(postVMLst.OrderByDescending(p => p.PostingDate));
            return View(userVM);

        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
