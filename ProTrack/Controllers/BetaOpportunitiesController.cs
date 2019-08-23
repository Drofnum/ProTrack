using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.BetaOpportunities;

namespace ProTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BetaOpportunitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BetaOpportunitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BetaOpportunities
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = _userManager.GetUserId(User);
            var betasList = _context.BetaOpportunity
                .Select(b => new BetaListingModel
                {
                    Id = b.Id,
                    ProjectName = b.ProjectName,
                    ShortDescription = b.ShortDescription,
                    OptedIn = _context.BetaOptIn.Where(o => o.BetaOpportunity.Id == b.Id && o.User == user).Any()
                });


            var model = new BetaIndexModel
            {
                BetasList = betasList
            };

            return View(model);
        }

        // GET: BetaOpportunities/Apply/5
        public async Task<IActionResult> Apply(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var betaOpportunity = await _context.BetaOpportunity
                .FirstOrDefaultAsync(m => m.Id == id);
            var optIn = new BetaOptIn
            {
                BetaOpportunity = betaOpportunity,
                User = user
            };

            _context.Add(optIn);
            await _context.SaveChangesAsync();

            if (betaOpportunity == null)
            {
                return NotFound();
            }

            return View(betaOpportunity);
        }

        // GET: BetaOpportunities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betaOpportunity = await _context.BetaOpportunity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (betaOpportunity == null)
            {
                return NotFound();
            }

            return View(betaOpportunity);
        }

        // GET: BetaOpportunities/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: BetaOpportunities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,ShortDescription,LongDescription,DriverUrl,QuickStartGuideUrl,UserGuideUrl,FirmwareUrl")] BetaOpportunity betaOpportunity)
        {
            if (User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(betaOpportunity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(betaOpportunity);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: BetaOpportunities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var betaOpportunity = await _context.BetaOpportunity.FindAsync(id);
                if (betaOpportunity == null)
                {
                    return NotFound();
                }
                return View(betaOpportunity);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: BetaOpportunities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,ShortDescription,LongDescription,DriverUrl,QuickStartGuideUrl,UserGuideUrl,FirmwareUrl")] BetaOpportunity betaOpportunity)
        {
            if (User.IsInRole("Admin"))
            {
                if (id != betaOpportunity.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(betaOpportunity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BetaOpportunityExists(betaOpportunity.Id))
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
                return View(betaOpportunity);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: BetaOpportunities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var betaOpportunity = await _context.BetaOpportunity
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (betaOpportunity == null)
                {
                    return NotFound();
                }

                return View(betaOpportunity);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: BetaOpportunities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var betaOpportunity = await _context.BetaOpportunity.FindAsync(id);
            _context.BetaOpportunity.Remove(betaOpportunity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BetaOpportunityExists(int id)
        {
            return _context.BetaOpportunity.Any(e => e.Id == id);
        }


    }
}
