using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacebookApp.Data;
using FacebookApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FacebookApp.Controllers
{
    [Authorize(Policy = "BlockBlocked")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly Post Post;

        public CommentsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserCommentsOnPosts.Include(u => u.Post).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCommentsOnPost = await _context.UserCommentsOnPosts
                .Include(u => u.Post)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (userCommentsOnPost == null)
            {
                return NotFound();
            }

            return View(userCommentsOnPost);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }


        #region Old Create
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CommentId,UserId,PostId,CommentDate,Content,IsDeleted")] UserCommentsOnPost userCommentsOnPost)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        userCommentsOnPost.UserId = _userManager.GetUserId(User);
        //        userCommentsOnPost.CommentDate = DateTime.Now;

        //        _context.Add(userCommentsOnPost);
        //        await _context.SaveChangesAsync();
        //        //return RedirectToAction(nameof(Index));
        //    }
        //    //ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", userCommentsOnPost.PostId);
        //    //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCommentsOnPost.UserId);
        //    return RedirectToAction("Index", "Home");
        //} 
        #endregion

        #region New Create
        [HttpPost]
        public JsonResult Create(string Content, int postID)
        {
            UserCommentsOnPost userCommentsOnPost = new UserCommentsOnPost();
            userCommentsOnPost.UserId = _userManager.GetUserId(User);
            userCommentsOnPost.CommentDate = DateTime.Now;
            userCommentsOnPost.Content = Content;
            userCommentsOnPost.PostId = postID;
            _context.UserCommentsOnPosts.Add(userCommentsOnPost);
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index));

            //ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", userCommentsOnPost.PostId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCommentsOnPost.UserId);
            return Json("");
        } 
        #endregion

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCommentsOnPost = await _context.UserCommentsOnPosts.FindAsync(id);
            if (userCommentsOnPost == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", userCommentsOnPost.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCommentsOnPost.UserId);
            //return View(userCommentsOnPost);
            return PartialView(userCommentsOnPost);
        }


        #region old Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CommentId,UserId,PostId,CommentDate,Content,IsDeleted")] UserCommentsOnPost userCommentsOnPost)
        //{
        //    if (id != userCommentsOnPost.CommentId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(userCommentsOnPost);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserCommentsOnPostExists(userCommentsOnPost.CommentId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        //return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", userCommentsOnPost.PostId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCommentsOnPost.UserId);
        //    //return View(userCommentsOnPost);
        //    return RedirectToAction(actionName: "Index", controllerName: "Home");
        //} 
        #endregion

        #region New Edit
        [HttpPost]
        public JsonResult Edit(int CommentId , string UserId , int PostId , DateTime CommentDate , string Content , bool IsDeleted)
        {
            UserCommentsOnPost userCommentsOnPost = new UserCommentsOnPost();

            userCommentsOnPost.CommentId = CommentId;
            userCommentsOnPost.UserId = UserId;
            userCommentsOnPost.PostId = PostId;
            userCommentsOnPost.CommentDate = CommentDate;
            userCommentsOnPost.Content = Content;
            userCommentsOnPost.IsDeleted = IsDeleted;
            

            //if (id != userCommentsOnPost.CommentId)
            //{
            //    return NotFound();
            //}

           
                try
                {
                    _context.Update(userCommentsOnPost);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCommentsOnPostExists(userCommentsOnPost.CommentId))
                    {
                        return Json("Not Found");
                    }
                    else
                    {
                        throw;
                    }
                }
            
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", userCommentsOnPost.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCommentsOnPost.UserId);
            //return View(userCommentsOnPost);
            return Json("");
        }
        #endregion

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCommentsOnPost = await _context.UserCommentsOnPosts
                .Include(u => u.Post)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (userCommentsOnPost == null)
            {
                return NotFound();
            }

            //return View(userCommentsOnPost);
            return PartialView(userCommentsOnPost);

        }

        // POST: Comments/Delete/5
        #region old Delete
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var userCommentsOnPost = await _context.UserCommentsOnPosts.FindAsync(id);
        //    _context.UserCommentsOnPosts.Remove(userCommentsOnPost);
        //    await _context.SaveChangesAsync();
        //    //return RedirectToAction(nameof(Index));
        //    return RedirectToAction(actionName: "Index", controllerName: "Home");
        //} 
        #endregion

        #region new Delete
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int CommentId)
        {
            var userCommentsOnPost =  _context.UserCommentsOnPosts.Find(CommentId);
            _context.UserCommentsOnPosts.Remove(userCommentsOnPost);
             _context.SaveChanges();
            //return RedirectToAction(nameof(Index));
            return Json("");
        }
        #endregion

        private bool UserCommentsOnPostExists(int id)
        {
            return _context.UserCommentsOnPosts.Any(e => e.CommentId == id);
        }
    }

}
