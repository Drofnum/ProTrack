using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atlassian.Jira;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.BetaOpportunities;
using ProTrack.Models.Betas;

namespace ProTrack.Controllers
{
    public class BetasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private static readonly HttpClient _client = new HttpClient();

        public BetasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Applicants(int id, string accepted, string rejected)
        {

            var user = await _userManager.GetUserAsync(User);
            var userId = _userManager.GetUserId(User);

            var betasList = _context.BetaOpportunity
                .Select(l => new SelectListItem()
                {
                    Value = l.Id.ToString(),
                    Text = l.ProjectName,
                    Selected = l.Id == id ? true : false
                }).ToList();

            var applicantList = _context.BetaOptIn.Where(b => b.BetaOpportunity.Id == id)
                .Select(a => new ApplicantListingModel
                {
                    Id = a.Id,
                    Description = a.BetaOpportunity.ShortDescription,
                    ProjectName = a.BetaOpportunity.ProjectName,
                    FullName = a.User.FirstName + " " + a.User.LastName,
                    Email = a.User.Email,
                    Accepted = a.Accepted,
                    BetaOpportunityId = a.BetaOpportunity.Id
                });

            if (!string.IsNullOrEmpty(accepted) && string.IsNullOrEmpty(rejected))
            {
                applicantList = _context.BetaOptIn.Where(b => b.BetaOpportunity.Id == id && b.Accepted == 1)
                .Select(a => new ApplicantListingModel
                {
                    Id = a.Id,
                    Description = a.BetaOpportunity.ShortDescription,
                    ProjectName = a.BetaOpportunity.ProjectName,
                    FullName = a.User.FirstName + " " + a.User.LastName,
                    Email = a.User.Email,
                    Accepted = a.Accepted,
                    BetaOpportunityId = a.BetaOpportunity.Id
                });
            }
            if(!string.IsNullOrEmpty(rejected) && string.IsNullOrEmpty(accepted))
            {
                applicantList = _context.BetaOptIn.Where(b => b.BetaOpportunity.Id == id && b.Accepted == 3)
                .Select(a => new ApplicantListingModel
                {
                    Id = a.Id,
                    Description = a.BetaOpportunity.ShortDescription,
                    ProjectName = a.BetaOpportunity.ProjectName,
                    FullName = a.User.FirstName + " " + a.User.LastName,
                    Email = a.User.Email,
                    Accepted = a.Accepted,
                    BetaOpportunityId = a.BetaOpportunity.Id
                });
            }


            var model = new ApplicantIndexModel
            {
                ApplicantList = applicantList,
                BetasList = betasList,
                AcceptedChecked = !string.IsNullOrEmpty(accepted)
            };
            return View(model);
        }

        public IActionResult MyBetas()
        {
            var userId = _userManager.GetUserId(User);
            var betasList = _context.BetaOptIn.Where(u => u.User.Id == userId && u.Accepted == 1)
                .Select(b => new BetaListingModel
                {
                    Id = b.BetaOpportunity.Id,
                    ProjectName = b.BetaOpportunity.ProjectName,
                    ShortDescription = b.BetaOpportunity.ShortDescription
                });

            var model = new BetaIndexModel
            {
                BetasList = betasList
            };

            return View(model);
        }

        // GET: Betas/Details/5
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

        public async Task<IActionResult> Approve(int id)
        {

            var optInId = await _context.BetaOptIn
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            var betaOpportunityId = _context.BetaOptIn.Where(b => b.Id == id).Select(b => b.BetaOpportunity.Id).FirstOrDefault();


            var accepted = new BetaOptIn
            {
                Id = optInId.Id,
                BetaOpportunity = optInId.BetaOpportunity,
                User = optInId.User,
                Accepted = 1
            };
            _context.Update(accepted);
            await _context.SaveChangesAsync();

            /*
            await _emailSender.SendEmailAsync(
                optInId.User.Email,
                "Accepted for " + optInId.BetaOpportunity.ProjectName,
                "We will contact you shortly with more information regarding this beta opportunity");
            */

            return RedirectToAction("Applicants", "Betas", new { id = betaOpportunityId });
        }

        public async Task<IActionResult> Reject(int id)
        {
            var optInId = await _context.BetaOptIn
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            var betaOpportunityId = _context.BetaOptIn.Where(b => b.Id == id).Select(b => b.BetaOpportunity.Id).FirstOrDefault();


            var accepted = new BetaOptIn
            {
                Id = optInId.Id,
                BetaOpportunity = optInId.BetaOpportunity,
                User = optInId.User,
                Accepted = 3
            };
            _context.Update(accepted);
            await _context.SaveChangesAsync();
            return RedirectToAction("Applicants", "Betas", new { id = betaOpportunityId });
        }

        public async Task<IActionResult> ApplicantDetails(string email, int id)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var model = new ApplicantDetailListingModel
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CompanyName = user.CompanyName,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                Country = user.Country
            };

            return View(model);
        }

        public IActionResult SubmitBug(int id)
        {
            var betaProject = _context.BetaOpportunity
                .Where(b => b.Id == id).FirstOrDefault();

            var model = new SubmitBugListingModel
            {
                Id = betaProject.Id,
                ProjectName = betaProject.ProjectName,

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitBug(SubmitBugListingModel model)
        {
            var jira = Jira.CreateRestClient("http://jira.control4.com", "User", "Password");
            var issue = jira.CreateIssue("BETA");
            issue.Type = "Bug";
            issue.Summary = model.BugSummary;
            issue.Description = model.BugDescription;

            await issue.SaveChangesAsync();
            

            var getIssue = from i in jira.Issues.Queryable
                           where i.Summary == new LiteralMatch(model.BugSummary)
                           select i;

            var values = new Dictionary<string, string>
                {
                    {"title", "[" + "getIssue" + "]" + " " + model.BugSummary },
                    {"category", "8" },
                    {"raw", model.BugDescription },
                    {"api_key", "39946de7bedac9af2676dfca3be92ae943e85ea2dd9f35e12b7ecb6a732b2747" },
                    {"api_username", "amunford" }
                };
            var content = new FormUrlEncodedContent(values);

            var response = await _client.PostAsync("http://control4discourse.westus.cloudapp.azure.com/posts.json", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var bugUrl = Regex.Replace(model.BugSummary.ToLower(), @"\s", "-");

            var submittedBug = new BugSubmittedListingModel
            {
                ReferenceNumber = "123",
                ForumPostUrl = "http://control4discourse.westus.cloudapp.azure.com/t/" + "getissue" + "-" + bugUrl
            };

            return RedirectToAction("BugSubmitted", "Betas", submittedBug);
        }

        public IActionResult BugSubmitted(BugSubmittedListingModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult GetBetas(int Id, string accepted, string rejected)
        {
            return RedirectToAction("Applicants", "Betas", new { id = Id, rejected = rejected, accepted = accepted });
        }


    }
}