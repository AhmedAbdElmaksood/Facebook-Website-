using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacebookApp.Data;
using Microsoft.AspNetCore.Mvc;
using FacebookApp.Models;
using Microsoft.AspNetCore.Identity;
using FacebookApp.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FacebookApp.Controllers
{
    [Authorize(Policy = "BlockBlocked")]
    public class AdminController : Controller
    {
        ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        public AdminController(ApplicationDbContext db, UserManager<User> UserManager, RoleManager<Role> RoleManager)
        {
            context = db;
            userManager = UserManager;
            roleManager = RoleManager;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var usersVM = new List<UserViewModel>();
            var users = context.Users.ToList();
            var usersRoles = context.UserRoles.ToList();
            var roles = roleManager.Roles/*.Where(r => r.Name != "blocked")*/.ToList();
            foreach (var user in users)
            {
                var userVM = new UserViewModel()
                {
                    UserId = user.Id,
                    Nickname = user.Nickname,
                    Bio = user.Bio,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    Gender = user.Gender,
                    Image = user.Image,
                    PhoneNumber = user.PhoneNumber,
                    isBlocked = user.isBlocked
                };
                //var rolefromDB = usersRoles.FirstOrDefault(ur => ur.UserId == userManager.GetUserId(User));
                //var roleId = rolefromDB.RoleId;
                //userVM.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                usersVM.Add(userVM);
            }
            //ViewData["RoleNames"] = new SelectList(context.Roles, "Name", "Name");
            return View(usersVM);
        }
        #endregion
        
        #region Create User
        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewData["RoleNames"] = new SelectList(roleManager.Roles/*.Where(r => r.Name != "blocked")*/.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Nickname = userVM.Nickname,
                    UserName = userVM.Email,
                    Email = userVM.Email,
                    Bio = userVM.Bio,
                    Gender = userVM.Gender,
                    BirthDate = userVM.BirthDate,
                    PhoneNumber = userVM.PhoneNumber
                };
                if (user.Gender == Gender.Male)
                    user.Image = "~/Images/defaultMaleUserImage.jpg";
                else
                    user.Image = "~/Images/defaultFemaleUserImage.jpg";
                try
                {
                    var result = await userManager.CreateAsync(user, userVM.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userVM.Role);
                        await userManager.AddClaimAsync(user, new Claim("isNotBlocked", "true"));
                    }
                }
                catch
                {

                }
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Update User
        [HttpGet]
        public ActionResult UpdateUser(string id)
        {
            //ViewData["RoleNames"] = new SelectList(context.Roles, "Name", "Name");

            if (id == null)
                return RedirectToAction("Index");
            User u = context.Users.FirstOrDefault(pp => pp.Id == id);
            if (u == null)
                return RedirectToAction("Index");
            UserViewModel userVM = new UserViewModel()
            {
                UserId = u.Id,
                Nickname = u.Nickname,
                Email = u.Email,
                Bio = u.Bio,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                PhoneNumber = u.PhoneNumber,
            };
            //var usersRoles = context.UserRoles.ToList();
            //var roles = context.Roles.ToList();
            //var rolefromDB = usersRoles.FirstOrDefault(ur => userVM.UserId == ur.UserId);
            //var roleId = rolefromDB.RoleId;
            //userVM.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
            return View(userVM);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserViewModel userVM)
        {
            User userObjFromDb = context.Users.FirstOrDefault(pp => pp.Id == userVM.UserId);
            if (userObjFromDb == null)
                return RedirectToAction("Index");
            userObjFromDb.Nickname = userVM.Nickname;
            userObjFromDb.Bio = userVM.Bio;
            userObjFromDb.BirthDate = userVM.BirthDate;
            userObjFromDb.Email = userVM.Email;
            userObjFromDb.Gender = userVM.Gender;
            userObjFromDb.PhoneNumber = userVM.PhoneNumber;

            //var result = await userManager.AddToRoleAsync(userObjFromDb, userVM.Role);

            context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Get Roles
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = roleManager.Roles;
            var newRoles = roles/*.Where(r => r.Name != "blocked")*/.ToList();
            return View(newRoles);
            //return View(context.Roles.ToList());
        }
        #endregion

        #region Create Role
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                Role role1 = new Role()
                {
                    Name = role.Name,
                    Description = role.Description
                };
                Microsoft.AspNetCore.Identity.IdentityResult result = await roleManager.CreateAsync(role1);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoles");

                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                //context.Add(new Role()
                //{
                //    Name = role.Name,
                //    Description = role.Description
                //});
                //context.SaveChanges();
                //return RedirectToAction("GetRoles");
            }
            return View();
        }
        #endregion

        #region Update Role
        [HttpGet]
        public ActionResult UpdateRole(string id)
        {
            if (id == null)
                return RedirectToAction("GetRoles");
            Role role = context.Roles.FirstOrDefault(pp => pp.Id == id);
            if (role == null)
            {
                return RedirectToAction("GetRoles");
            }

            return View(role);
        }
        [HttpPost]
        public ActionResult UpdateRole(Role role, int id)
        {
            Role roleObjFromDb = context.Roles.FirstOrDefault(pp => pp.Id == role.Id);
            if (roleObjFromDb == null)
                return RedirectToAction("GetRoles");
            roleObjFromDb.Name = role.Name;
            roleObjFromDb.Description = role.Description;
            context.SaveChanges();
            return RedirectToAction("GetRoles");
        }

        #endregion

        #region Delete Role
        [HttpGet]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("GetRoles");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("GetRoles");

            //if (id == null)
            //{
            //    return RedirectToAction("GetRoles");
            //}
            //Role role = context.Roles.FirstOrDefault(pp => pp.Id == id);
            //if (role == null)
            //    return RedirectToAction("GetRoles");
            //context.Roles.Remove(role);
            //context.SaveChanges();
            //return RedirectToAction("GetRoles");
        }
        #endregion

        #region ManageUserRoles
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            ViewBag.userId = id;
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("GetRoles");
            }
            var model = new List<ViewModel.UsersRolesViewModel>();
            var roles = roleManager.Roles.ToList();
            var newRoles = roles/*.Where(r => r.Name != "blocked")*/.ToList();
            foreach (var role in newRoles)
            {
                var usersRoleViewModel = new ViewModel.UsersRolesViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    usersRoleViewModel.IsSelected = true;
                }
                else
                {
                    usersRoleViewModel.IsSelected = false;
                }
                model.Add(usersRoleViewModel);
            }
            return PartialView(model.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UsersRolesViewModel> model, string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("GetRoles");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                return View(model);
            }
            return RedirectToAction("Index", new { Id = id });
        }

        #endregion

        #region block handling
        public async Task<IActionResult> blockHandle(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            User user = context.Users.FirstOrDefault(pp => pp.Id == id);
            if (user == null)
                return RedirectToAction("Index");
            user.isBlocked = !user.isBlocked;
            context.SaveChanges();
            var claims = await userManager.GetClaimsAsync(user);
            await userManager.RemoveClaimAsync(user, claims.FirstOrDefault(i => i.Type == "isNotBlocked"));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnblockHandle(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            User user = context.Users.FirstOrDefault(pp => pp.Id == id);
            if (user == null)
                return RedirectToAction("Index");
            user.isBlocked = !user.isBlocked;
            context.SaveChanges();
            await userManager.AddClaimAsync(user, new Claim("isNotBlocked", "true"));
            return RedirectToAction("Index");
        }
        #endregion

    }
}