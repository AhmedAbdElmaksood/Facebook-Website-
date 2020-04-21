using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacebookApp.Data;
using FacebookApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.MemoryMappedFiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting.Internal;
using FacebookApp.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace FacebookApp.Controllers
{
    [Authorize(Policy = "BlockBlocked")]
    public class PostsController : Controller
    {
        private readonly IHostingEnvironment _he;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        List<Post> posts = new List<Post>();
        public PostsController(ApplicationDbContext context, IHostingEnvironment he, UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _context = context;
            _he = he;
            _userManager = UserManager;
            _signInManager = SignInManager;
            //List<Post> posts = new List<Post>();
        }

        #region commented
        //// GET: Posts
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Posts.Include(p => p.User);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        //// GET: Posts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .Include(p => p.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        //// GET: Posts/Create
        //public IActionResult Create()
        //{
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}
        #endregion

        #region commented Old Create 

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Content,PostingDate,IsDeleted,UserId")] Post post, IFormFile pic)
        //{
        //    //posts.Add(post);
        //    int flag = 0;
        //    if (ModelState.IsValid && post.Content != null)
        //    {
        //        flag = 0;
        //        post.UserId = _userManager.GetUserId(User);
        //        post.PostingDate = DateTime.Now;
        //        _context.Add(post);
        //        await _context.SaveChangesAsync();
        //        //return RedirectToAction(nameof(Index));
        //        //return View(posts);
        //    }
        //    else if (pic != null)
        //    {
        //        flag = 1;

        //        var picName = Path.GetFileName(pic.FileName);
        //        var picLocation = Path.Combine(_he.WebRootPath, picName);
        //        pic.CopyTo(new FileStream(picLocation, FileMode.Create));


        //        //Other code
        //        //using (var stream = System.IO.File.Open(@"D:\ITI\ASP.NET MVC\Project\MVCProject\FacebookApp\wwwroot", FileMode.Open))
        //        //{
        //        //    // Use stream
        //        //}

        //        var picPath = "/" + Path.GetFileName(pic.FileName);
        //        post.UserId = _userManager.GetUserId(User);
        //        post.PostingDate = DateTime.Now;
        //        post.Content = picPath;
        //        _context.Add(post);
        //        await _context.SaveChangesAsync();
        //    }

        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
        //    //return RedirectToAction("Index", "Timeline", new { flag });
        //    return RedirectToAction("Index", "Home", new { flag });
        //    //return RedirectToAction("Index", new { flag });
        //}

        #endregion

        #region New Create
        [HttpPost]
        //public /*async*/ /*Task<IActionResult>*/ JsonResult Create(/*[Bind("Id,Content,PostingDate,IsDeleted,UserId")] Post post , IFormFile pic*/ string Content)
        public JsonResult Create(IFormFile ImageFile, string Content)
        //public /*async*/ /*Task<IActionResult>*/ JsonResult Create([Bind("Id,Content,PostingDate,IsDeleted,UserId, ImageFile")] PostsViewModel model)
        {
            #region ajax
            ////posts.Add(post);
            //Post post = new Post();
            //int flag = 0;

            //flag = 0;
            //post.UserId = _userManager.GetUserId(User);
            //post.PostingDate = DateTime.Now;
            //post.Content = Content;
            //_context.Posts.Add(post);
            //_context.SaveChanges();
            #endregion
            #region with photo
            //posts.Add(post);
            int flag = 0;
            if (ModelState.IsValid)
            {
                flag = 0;
                var post = new Post()
                {
                    Content = Content
                };
                if (ImageFile != null)
                {
                    flag = 1;
                    string uniqueFileName = UploadedFile(ImageFile);
                    var userid = _userManager.GetUserId(User);
                    post.Image = "~/Images/PostsImages/" + uniqueFileName;
                }
                post.UserId = _userManager.GetUserId(User);
                post.PostingDate = DateTime.Now;
                post.IsDeleted = false;
                _context.Add(post);
                _context.SaveChanges();

                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
            }
            #endregion

            return Json("");
        }
        private string UploadedFile(IFormFile formFile)
        {
            string uniqueFileName = null;

            string s = Path.Combine("Images", "PostsImages");
            string uploadsFolder = Path.Combine(_he.WebRootPath, s);
            uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
        #endregion

        #region Edit
        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
            return PartialView(post);
        }

        #region old Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Content,PostingDate,IsDeleted,UserId")] Post post)
        //{
        //    //if (id != post.Id)
        //    //{
        //    //    return NotFound();
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(post);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PostExists(post.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        //return RedirectToAction(actionName:"Index",controllerName:"Timeline");
        //        return RedirectToAction(actionName: "Index", controllerName: "Home");
        //    }
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
        //    //return RedirectToAction(actionName: "Index", controllerName: "Timeline");
        //    return RedirectToAction(actionName: "Index", controllerName: "Home");
        //} 
        #endregion

        #region New Edit
        [HttpPost]
        public JsonResult Edit(int Id, string Content, string UserId, IFormFile ImageFile, string OldImage)
        {
            #region old before ajax
            if (ModelState.IsValid)
            {
                var post = _context.Posts.Find(Id);
                //post.Id = Id;
                post.Content = Content;
                //post.UserId = UserId;
                try
                {
                    if (ImageFile != null)
                    {
                        string uniqueFileName = UploadedFile(ImageFile);
                        post.Image = "~/Images/PostsImages/" + uniqueFileName;
                    }
                    else if(OldImage != null)
                    {
                        post.Image = OldImage;
                    }
                    //post.PostingDate = _context.Posts.Find(Id).PostingDate;
                    //UserId = _userManager.GetUserId(User);
                    //PostingDate = DateTime.Now;
                    //var post2 = _context.Posts.Find(id);
                    //post2.Content = Content;
                    //post2.Image = Image;
                    _context.Update(post);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        Json("Not Found");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Json("");

            #endregion

            #region after ajax
            //Post post = new Post();
            //post.Id = Id;
            //post.Content = Content;
            //post.UserId = UserId;
            //try
            //{
            //    _context.Update(post);
            //        _context.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PostExists(post.Id))
            //    {
            //    return Json("Not Found");
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            //return Json("");
            #endregion
        }
        #endregion

        #endregion

        #region Delete
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            #region for alert
            //if (id == null)
            //{
            //    return NotFound();
            //}

            ////var post = await _context.Posts
            ////    .Include(p => p.User)
            ////    .FirstOrDefaultAsync(m => m.Id == id);
            //var post = await _context.Posts.FindAsync(id);

            //if (post == null)
            //{
            //    return NotFound();
            //}

            ////return View(post);
            //post.IsDeleted = true;
            ////_context.Posts.Remove(post);
            //await _context.SaveChangesAsync();
            ////return RedirectToAction("Index", "Timeline");
            //return RedirectToAction("Index", "Home");
            #endregion

            if (id == null)
            {
                return BadRequest();
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return PartialView(post);
        }

        // POST: Posts/Delete/5
        #region old Delete
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var post = await _context.Posts.FindAsync(id);
        //    post.IsDeleted = true;
        //    //_context.Posts.Remove(post);
        //    await _context.SaveChangesAsync();
        //    //return RedirectToAction(actionName: "Index", controllerName: "Timeline");
        //    return RedirectToAction(actionName: "Index", controllerName: "Home");
        //}

        #endregion

        #region New Delete
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            var post =  _context.Posts.Find(id);
            post.IsDeleted = true;
            //_context.Posts.Remove(post);
             _context.SaveChanges();
            //return RedirectToAction(actionName: "Index", controllerName: "Timeline");
            return Json("");
        }
        #endregion
        #endregion

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
        #region old ajax details
        //////////////////////////////////
        //[HttpGet]
        //public IActionResult Details(int flag)
        //{
        //    #region Old View Model
        //    //    ////TODO : Return the friends' posts only
        //    //    ViewBag.flag = flag;
        //    //    // get the posts from DB
        //    //    var postsList = _context.Posts.OrderByDescending(p => p.PostingDate).ToList();
        //    //    // use the object to be added in the new view model posts list
        //    //    var postVMLst = new List<PostsViewModel>();


        //    //    // new comments list
        //    //    var commentsVMLst = new List<CommentsViewModel>();

        //    //    // get the posts from the DB
        //    //    foreach (var post in postsList)
        //    //    {
        //    //        // the new view model posts list
        //    //        var postVM = new PostsViewModel();

        //    //        postVM.Content = post.Content;
        //    //        postVM.IsDeleted = post.IsDeleted;
        //    //        postVM.PostId = post.Id;
        //    //        postVM.UserId = post.UserId;
        //    //        postVM.PostingDate = post.PostingDate;

        //    //        // get the comments from DB
        //    //        IEnumerable<UserCommentsOnPost> commentsList = _context.UserCommentsOnPosts.Where(c => c.PostId == post.Id);

        //    //        foreach (var comment in commentsList)
        //    //        {
        //    //            // use the comments object to be added to the new commments list
        //    //            var commentVM = new CommentsViewModel();
        //    //            commentVM.Content = comment.Content;
        //    //            commentVM.UserId = comment.UserId;
        //    //            commentVM.IsDeleted = comment.IsDeleted;
        //    //            commentVM.PostId = comment.PostId;
        //    //            commentVM.CommentId = comment.CommentId;

        //    //            commentsVMLst.Add(commentVM);
        //    //        }

        //    //        postVM.commentsVMList = commentsVMLst;

        //    //        postVMLst.Add(postVM);

        //    //    } 
        //    #endregion

        //    ViewBag.flag = flag;
        //    int FriendsCount = 0;

        //    var id = _userManager.GetUserId(User);

        //    var userFromDb = _context.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
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

        //    IEnumerable<Post> postsList = _context.Posts.Where(p => p.IsDeleted == false);
        //    var postVMLst = new List<PostsViewModel>();
        //    foreach (var post in postsList)
        //    {
        //        var currentUserId = _userManager.GetUserId(User);
        //        var isAFriend = _context.UserHasFriends.FirstOrDefault(u => (u.UserId == post.UserId || u.FriendId == post.UserId) && (u.UserId == currentUserId || u.FriendId == currentUserId) && u.Status == FriendRequestStatus.Friend);
        //        if (isAFriend != null || post.UserId == currentUserId)
        //        {
        //            var postVM = new PostsViewModel();
        //            postVM.Content = post.Content;
        //            postVM.IsDeleted = post.IsDeleted;
        //            postVM.PostId = post.Id;
        //            postVM.UserId = post.UserId;
        //            postVM.PostingDate = post.PostingDate;

        //            IEnumerable<UserLikesPost> likesList = _context.UserLikesPosts.Where(l => l.PostId == post.Id && l.IsLiked == true);
        //            IEnumerable<UserCommentsOnPost> commentsList = _context.UserCommentsOnPosts.Where(c => c.PostId == post.Id && c.IsDeleted == false);

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

        //    //Friends
        //    FriendsCount = _context.UserHasFriends.Where(u => (u.UserId == _userManager.GetUserId(User) || u.FriendId == _userManager.GetUserId(User)) && u.Status == FriendRequestStatus.Friend).Count();
        //    //foreach (var friend in friendsList)
        //    //{
        //    //    var pendingFriends = context.UserHasFriends.FirstOrDefault(f => (f.UserId == _userManager.GetUserId(User) || f.FriendId == _userManager.GetUserId(User)) && f.Status == FriendRequestStatus.Pending);
        //    //    var friendId = (pendingFriends.FriendId != _userManager.GetUserId(User)) ? pendingFriends.FriendId : pendingFriends.UserId;
        //    //    friendsVMList.Add(new FriendsViewModel()
        //    //    {
        //    //        Email = context.Users.FirstOrDefault(f => f.Id == friendId).Email,
        //    //        //TODO: Image
        //    //        Image = context.Users.FirstOrDefault(f => f.Id == friendId).Image,
        //    //    });
        //    //}
        //    ViewBag.FriendsCount = FriendsCount;


        //    return PartialView(userVM);


        //}
        #endregion

        [HttpGet]
        public IActionResult HomeDetails()
        {
            #region before
            //int FriendsCount = 0;

            //var id = _userManager.GetUserId(User);

            //var userFromDb = _context.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
            //var userVM = new UserViewModel()
            //{
            //    Nickname = userFromDb.Nickname,
            //    Bio = userFromDb.Bio,
            //    BirthDate = userFromDb.BirthDate,
            //    Gender = userFromDb.Gender,
            //    Image = userFromDb.Image,
            //    Email = userFromDb.Email,
            //    UserId = userFromDb.Id,
            //};

            //IEnumerable<Post> postsList = _context.Posts.Where(p => p.IsDeleted == false);
            //var postVMLst = new List<PostsViewModel>();
            //foreach (var post in postsList)
            //{
            //    var currentUserId = _userManager.GetUserId(User);
            //    var isAFriend = _context.UserHasFriends.FirstOrDefault(u => (u.UserId == post.UserId || u.FriendId == post.UserId) && (u.UserId == currentUserId || u.FriendId == currentUserId) && u.Status == FriendRequestStatus.Friend);
            //    if (isAFriend != null || post.UserId == currentUserId)
            //    {
            //        var postVM = new PostsViewModel();
            //        postVM.Content = post.Content;
            //        postVM.IsDeleted = post.IsDeleted;
            //        postVM.PostId = post.Id;
            //        postVM.UserId = post.UserId;
            //        postVM.PostingDate = post.PostingDate;

            //        IEnumerable<UserLikesPost> likesList = _context.UserLikesPosts.Where(l => l.PostId == post.Id && l.IsLiked == true);
            //        IEnumerable<UserCommentsOnPost> commentsList = _context.UserCommentsOnPosts.Where(c => c.PostId == post.Id && c.IsDeleted == false);

            //        var likesVMLst = new List<LikesViewModel>();
            //        var commentsVMLst = new List<CommentsViewModel>();

            //        foreach (var like in likesList)
            //        {
            //            var likeVM = new LikesViewModel();
            //            likeVM.UserId = like.UserId;
            //            likeVM.PostId = like.PostId;
            //            likeVM.IsLiked = like.IsLiked;
            //            likesVMLst.Add(likeVM);
            //        }
            //        postVM.likesVMList = likesVMLst;

            //        foreach (var comment in commentsList)
            //        {
            //            var commentVM = new CommentsViewModel();
            //            commentVM.Content = comment.Content;
            //            commentVM.UserId = comment.UserId;
            //            commentVM.IsDeleted = comment.IsDeleted;
            //            commentVM.PostId = comment.PostId;
            //            commentVM.CommentId = comment.CommentId;
            //            commentsVMLst.Add(commentVM);
            //        }
            //        postVM.commentsVMList = commentsVMLst;


            //        postVMLst.Add(postVM);
            //    }
            //}

            //userVM.Posts = postVMLst;

            ////Friends
            //FriendsCount = _context.UserHasFriends.Where(u => (u.UserId == _userManager.GetUserId(User) || u.FriendId == _userManager.GetUserId(User)) && u.Status == FriendRequestStatus.Friend).Count();
            //ViewBag.FriendsCount = FriendsCount;
            #endregion

            int FriendsCount = 0;

            var id = _userManager.GetUserId(User);

            var userFromDb = _context.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
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

            IEnumerable<Post> postsList = _context.Posts.Where(p => p.IsDeleted == false);
            var postVMLst = new List<PostsViewModel>();
            foreach (var post in postsList)
            {
                var currentUserId = _userManager.GetUserId(User);
                var isAFriend = _context.UserHasFriends.FirstOrDefault(u => (u.UserId == post.UserId || u.FriendId == post.UserId) && (u.UserId == currentUserId || u.FriendId == currentUserId) && u.Status == FriendRequestStatus.Friend);
                if (isAFriend != null || post.UserId == currentUserId)
                {
                    var postVM = new PostsViewModel();
                    postVM.Content = post.Content;
                    postVM.IsDeleted = post.IsDeleted;
                    postVM.PostId = post.Id;
                    postVM.UserId = post.UserId;
                    postVM.PostingDate = post.PostingDate;
                    postVM.Image = post.Image;

                    IEnumerable<UserLikesPost> likesList = _context.UserLikesPosts.Where(l => l.PostId == post.Id && l.IsLiked == true);
                    IEnumerable<UserCommentsOnPost> commentsList = _context.UserCommentsOnPosts.Where(c => c.PostId == post.Id && c.IsDeleted == false);

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
            FriendsCount = _context.UserHasFriends.Where(u => (u.UserId == _userManager.GetUserId(User) || u.FriendId == _userManager.GetUserId(User)) && u.Status == FriendRequestStatus.Friend).Count();
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


            return PartialView("Details", userVM);

        }

        [HttpGet]
        public IActionResult ProfileDetails(string id)
        {

            #region Profile info
            if (id == null)
                id = _userManager.GetUserId(User);
            var userFromDb = _context.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
            var userVM = new UserViewModel()
            {
                Nickname = userFromDb.Nickname,
                Bio = userFromDb.Bio,
                BirthDate = userFromDb.BirthDate,
                Gender = userFromDb.Gender,
                Image = userFromDb.Image,
                Email = userFromDb.Email,
                UserId = userFromDb.Id,
                isBlocked = userFromDb.isBlocked,
                PhoneNumber = userFromDb.PhoneNumber
            };
            #endregion

            IEnumerable<Post> postsList = _context.Posts.Where(p => p.IsDeleted == false);
            var postVMLst = new List<PostsViewModel>();
            #region Getting all the posts with their likes and comments
            if (postsList != null)
                foreach (var post in postsList)
                {
                    //var currentUserId = _userManager.GetUserId(User);
                    if (post.UserId == id)
                    {
                        //Posts
                        var postVM = new PostsViewModel()
                        {
                            Content = post.Content,
                            IsDeleted = post.IsDeleted,
                            PostId = post.Id,
                            UserId = post.UserId,
                            PostingDate = post.PostingDate,
                            Image = post.Image
                        };
                        IEnumerable<UserLikesPost> likesList = _context.UserLikesPosts.Where(l => l.PostId == post.Id && l.IsLiked == true);
                        IEnumerable<UserCommentsOnPost> commentsList = _context.UserCommentsOnPosts.Where(c => c.PostId == post.Id && c.IsDeleted == false);
                        var likesVMLst = new List<LikesViewModel>();
                        var commentsVMLst = new List<CommentsViewModel>();
                        //Likes
                        foreach (var like in likesList)
                        {
                            var likeVM = new LikesViewModel();
                            likeVM.UserId = like.UserId;
                            likeVM.PostId = like.PostId;
                            likeVM.IsLiked = like.IsLiked;
                            likesVMLst.Add(likeVM);
                        }
                        postVM.likesVMList = likesVMLst;
                        //Comments
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
            userVM.Posts = postVMLst.OrderByDescending(p => p.PostingDate).ToList();
            #endregion

            #region Getting friends of the current user
            if (id == _userManager.GetUserId(User)) //Friends in the current user profile
            {
                var friendships = _context.UserHasFriends.Where(f => f.UserId == _userManager.GetUserId(User) || f.FriendId == _userManager.GetUserId(User) && f.Status != FriendRequestStatus.NotFriend).ToList();
                if (friendships.Count > 0)
                {
                    foreach (var friendship in friendships)
                    {
                        var trueFriendId = string.Empty; //reciever user id
                        var friendshipStatus = friendship.Status;

                        if (friendship.FriendId == _userManager.GetUserId(User)) trueFriendId = friendship.UserId;
                        else if (friendship.UserId == _userManager.GetUserId(User)) trueFriendId = friendship.FriendId;

                        var friendFromDb = _context.Users.Find(trueFriendId); // Friend that I have sent to him or recieved from him (Not current user)
                        if (friendFromDb != null)
                            userVM.Friends.Add(new FriendsViewModel()
                            {
                                FriendId = friendFromDb.Id,
                                Nickname = friendFromDb.Nickname,
                                Email = friendFromDb.Email,
                                Bio = friendFromDb.Bio,
                                Gender = friendFromDb.Gender,
                                Image = friendFromDb.Image,
                                Status = friendshipStatus,
                                SenderId = friendship.UserId,
                                RecieverId = friendship.FriendId
                            });

                        userVM.SenderId = friendship.UserId;
                        userVM.RecieverId = friendship.FriendId;
                    }
                }
            }
            else //friendship between the current user and the profile owner
            {
                var friendship = _context.UserHasFriends.FirstOrDefault(f => f.UserId == id || f.FriendId == id && f.Status != FriendRequestStatus.NotFriend);
                if (friendship != null) //there is a friendship between current user and the profile owner
                {
                    var trueFriendId = string.Empty; //reciever user id
                    var friendshipStatus = friendship.Status;

                    if (friendship.FriendId == _userManager.GetUserId(User)) trueFriendId = friendship.UserId;
                    else if (friendship.UserId == _userManager.GetUserId(User)) trueFriendId = friendship.FriendId;

                    var friendFromDb = _context.Users.Find(trueFriendId); // Friend that I have sent to him or recieved from him (Not current user)
                    if (friendFromDb != null)
                        userVM.Friends.Add(new FriendsViewModel()
                        {
                            FriendId = friendFromDb.Id,
                            Nickname = friendFromDb.Nickname,
                            Email = friendFromDb.Email,
                            Bio = friendFromDb.Bio,
                            Gender = friendFromDb.Gender,
                            Image = friendFromDb.Image,
                            Status = friendshipStatus,
                            SenderId = friendship.UserId,
                            RecieverId = friendship.FriendId
                        });

                    userVM.SenderId = friendship.UserId;
                    userVM.RecieverId = friendship.FriendId;
                }
            }
            #endregion

            return PartialView("Details", userVM);


        }

    }
}
