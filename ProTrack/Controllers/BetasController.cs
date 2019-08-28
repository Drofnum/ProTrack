using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atlassian.Jira;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.BetaOpportunities;
using ProTrack.Models.Betas;

namespace ProTrack.Controllers
{
    [Authorize(Roles = "Admin")]
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
            if (ModelState.IsValid)
            {
                var jiraResponse = BuildJiraIssue(model);            

                var values = new Dictionary<string, string>
                {
                    {"title", "[" + jiraResponse + "]" + " " + model.BugSummary },
                    {"category", "8" },
                    {"raw", model.BugDescription + Environment.NewLine + model.BuildNumber },
                    {"api_key", "39946de7bedac9af2676dfca3be92ae943e85ea2dd9f35e12b7ecb6a732b2747" },
                    {"api_username", "amunford" }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await _client.PostAsync("http://control4discourse.westus.cloudapp.azure.com/posts.json", content);

                var responseString = await response.Content.ReadAsStringAsync();
                var bugUrl = "";

                if (!string.IsNullOrEmpty(jiraResponse))
                {
                    var bugSummary = Regex.Replace(model.BugSummary.ToLower(), @"\s-", "");
                    bugUrl = "http://control4discourse.westus.cloudapp.azure.com/t/" + jiraResponse + "-" + Regex.Replace(bugSummary, @"\s", "-");
                }
                else
                {
                    var bugSummary = Regex.Replace(model.BugSummary.ToLower(), @"\s-", "");
                    bugUrl = "http://control4discourse.westus.cloudapp.azure.com/t/" + Regex.Replace(bugSummary, @"\s", "-");
                }
                    
                

                var submittedBug = new BugSubmittedListingModel
                {
                    ReferenceNumber = jiraResponse,
                    ForumPostUrl = bugUrl
                };

                return RedirectToAction("BugSubmitted", "Betas", submittedBug);
            }
            return View(model);
        }

        public IActionResult SubmitFeedback(int id)
        {
            var betaProject = _context.BetaOpportunity
                .Where(b => b.Id == id).FirstOrDefault();

            var model = new SubmitFeedbackListingModel
            {
                Id = betaProject.Id,
                ProjectName = betaProject.ProjectName + " Feedback"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(SubmitFeedbackListingModel model)
        {
            if (ModelState.IsValid)
            {
                
                await _emailSender.SendEmailAsync("amunford@control4.com", "Feedback for " + model.ProjectName,
                        model.Summary);

                return RedirectToAction("FeedbackSubmitted", "Betas");
            }
            return View(model);
        }

        private string BuildJiraIssue(SubmitBugListingModel model)
        {
            var jiraJson = new
            {
                fields = new Dictionary<string, object> {
                {"project", new {id = "11600"} },
                {"summary", model.BugSummary },
                {"description", model.BugDescription + Environment.NewLine + model.BuildNumber },
                {"issuetype", new {id = "10004"} }
                }
            };

            string url = "https://jira-stage.control4.com/rest/api/2/issue/";
            string user = "apiaccess";
            string password = "api access for bear";


            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            string credentials = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(user + ":" + password));
                request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Basic {0}", credentials);

            string data = JsonConvert.SerializeObject(jiraJson);

            using (var webStream = request.GetRequestStream())
                using (var requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(data);
            }

            try
            {
                var webResponse = request.GetResponse();
                using (var responseReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string response = responseReader.ReadToEnd();
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                    var key = values["key"];

                    return key;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult BugSubmitted(BugSubmittedListingModel model)
        {
            return View(model);
        }

        public IActionResult FeedbackSubmitted(SubmitFeedbackListingModel model)
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