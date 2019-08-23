using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.ManageUser;

namespace ProTrack.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public ManageUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index(string emailSearchString, string nameSearchString)
        {

            IQueryable<ApplicationUser> users = _context.Users;
            var areNoResults = true;
            if (!String.IsNullOrEmpty(emailSearchString))
            {
                users = users.Where(u => u.Email.Contains(emailSearchString));

                if (users.Any())
                {
                    areNoResults = false;
                }
            }
            if (!String.IsNullOrEmpty(nameSearchString))
            {
                users = users.Where(u => u.FirstName.Contains(nameSearchString) || u.FirstName.Contains(nameSearchString));
                if (users.Any())
                {
                    areNoResults = false;
                }

            }
            if (String.IsNullOrEmpty(emailSearchString) && String.IsNullOrEmpty(nameSearchString))
            {
                areNoResults = false;
            }

            var userList = users.Select(u => new UserListingModel
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName,
                Email = u.Email
            });

            var model = new UserIndexModel
            {
                UserList = userList
            };

            return View(model);
        }

        // GET: ManageUser/Edit/5
        public IActionResult Edit(string id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var roles = _context.Roles.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem()
                    {
                        Value = rr.Id,
                        Text = rr.Name
                    }).ToList();

                List<string> assignedRoles = _context.UserRoles.Where(r => r.UserId == id)
                    .Select(ar => _context.Roles.Where(t => t.Id == ar.RoleId).Select(r => r.Name).FirstOrDefault()
                    ).ToList();

                //var user = await _context.Users.FindAsync(id);
                var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();

                var model = new UserListingModel
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    IsInternal = user.IsInternal,
                    Roles = roles,
                    SelectedRoles = assignedRoles
                };

                if (user == null)
                {
                    return NotFound();
                }
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: ManageUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,EmailConfirmed,IsInternal,SelectedRoles")] ApplicationUser applicationUser, List<string> SelectedRoles)
        {
            if (User.IsInRole("Admin"))
            {
                if (id != applicationUser.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {

                    try
                    {
                        var user = await _userManager.FindByIdAsync(applicationUser.Id);
                        if (user == null)
                        {
                            return NotFound($"Unable to load user with ID '{_userManager.FindByIdAsync(applicationUser.Id)}'.");
                        }

                        var email = await _userManager.GetEmailAsync(user);
                        if (applicationUser.Email != email)
                        {
                            var setEmailResult = await _userManager.SetEmailAsync(user, applicationUser.Email);
                            if (!setEmailResult.Succeeded)
                            {
                                var userId = applicationUser.Id;
                                throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                            }
                        }

                        if (applicationUser.FirstName != user.FirstName)
                        {
                            user.FirstName = applicationUser.FirstName;
                        }
                        if (applicationUser.LastName != user.LastName)
                        {
                            user.LastName = applicationUser.LastName;
                        }
                        if (applicationUser.EmailConfirmed != user.EmailConfirmed)
                        {
                            user.EmailConfirmed = applicationUser.EmailConfirmed;
                        }
                        if (applicationUser.IsInternal != user.IsInternal)
                        {
                            user.IsInternal = applicationUser.IsInternal;
                        }

                        await _userManager.UpdateAsync(user);

                        var roles = _context.Roles.Select(r => r.Id).ToList();

                        foreach (var role in roles)
                        {
                            var roleName = _context.Roles.Where(r => r.Id == role).Select(r => r.Name).FirstOrDefault();
                            if (SelectedRoles.Contains(role))
                            {
                                if (!_userManager.GetRolesAsync(user).Result.Contains(roleName))
                                {
                                    await _userManager.AddToRoleAsync(user, roleName);
                                }
                            }

                            if (!SelectedRoles.Contains(role))
                            {
                                if (_userManager.GetRolesAsync(user).Result.Contains(roleName))
                                {
                                    await _userManager.RemoveFromRoleAsync(user, roleName);
                                }
                            }
                        }
                        
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ApplicationUserExists(applicationUser.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(applicationUser);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: ManageUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                var model = new UserListingModel
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    Email = user.Email
                };

                if (user == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: BetaOpportunities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(applicationUser);
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}