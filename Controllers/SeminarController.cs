using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SeminarHub.Data;
using SeminarHub.Models;
using SeminarHub.Models.ViewModels;
using System.Security.Claims;
using SeminarHub.Common;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Linq;


namespace SeminarHub.Controllers
{
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SeminarController(SeminarHubDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddSeminarViewModel(_context.Categories.ToList()));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSeminarViewModel svm)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();

                Seminar seminar = new Seminar
                {
                    Topic = svm.Topic,
                    Lecturer = svm.Lecturer,
                    Details = svm.Details,
                    OrganiserId = userId,
                    DateAndTime = svm.DateAndTime,
                    Duration = svm.Duration,
                    CategoryId = svm.CategoryId
                };

                await _context.Seminars.AddAsync(seminar);
                await _context.SaveChangesAsync();

                return RedirectToAction("All", "Seminar");
            }

            return View(svm);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var all = await _context
                .Seminars
                .Include(s => s.Category)
                .Include(s => s.Organiser)
                .ToListAsync();

            var containing = _context
                .Seminars
                .Where(s => s
                            .SeminarsParticipants
                            .Select(sp => sp.ParticipantId)
                            .Contains(GetUserId()))
                .ToList();

            var filteredSeminars = all
                .Except(containing)
                .ToList();
                

            var seminars = filteredSeminars
                .Select(s => new AllSeminarsViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime,
                    Category = s.Category.Name,
                    Organizer = s.Organiser.UserName,
                })
                .ToList();

            return View(seminars);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            string? userId = GetUserId();

            bool alreadyAded = await _context.SeminarsParticipants
                .AnyAsync(sp => sp.ParticipantId == userId && sp.SeminarId == id);

            if(!alreadyAded)
            {
                var seminarParticipant = new SeminarParticipant
                {
                    SeminarId = id,
                    ParticipantId = userId
                };

                await _context
                    .SeminarsParticipants
                    .AddAsync(seminarParticipant);

                await _context.SaveChangesAsync();

            }
                
            return RedirectToAction("Joined", "Seminar");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Seminar? seminar = (Seminar?)await CreatorGetSeminarByIdAsync(id);

            if (seminar != null)
            {
                AddSeminarViewModel seminarView = new AddSeminarViewModel(_context.Categories.ToList())
                {
                    Topic = seminar.Topic,
                    Lecturer = seminar.Lecturer,
                    Details = seminar.Details,
                    DateAndTime = seminar.DateAndTime,
                    Duration = seminar.Duration,
                    CategoryId = seminar.CategoryId,
                };

                return View(seminarView);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddSeminarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var seminar = (Seminar?)await CreatorGetSeminarByIdAsync(id);

                if (seminar != null)
                {
                    seminar.Topic = model.Topic;
                    seminar.Lecturer = model.Lecturer;
                    seminar.Details = model.Details;
                    seminar.DateAndTime = model.DateAndTime;
                    seminar.Duration = model.Duration;
                    seminar.CategoryId = model.CategoryId;


                    await _context.SaveChangesAsync();
                    return RedirectToAction("All", "Seminar");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            DetailsViewModel? model = await _context
            .Seminars
            .Where(s => s.Id == id)
            .Select(seminar => new DetailsViewModel
            {
                 Id = seminar.Id,
                 Topic = seminar.Topic,
                 Lecturer = seminar.Lecturer,
                 DateAndTime = seminar.DateAndTime,
                 Category = seminar.Category.Name,
                 Organizer = seminar.Organiser.UserName,
                 Details = seminar.Details,
                 Duration = seminar.Duration
            })
            .FirstOrDefaultAsync();

            return View(model);

            throw new ArgumentException("Id not valid");
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var seminars = await _context
                .Seminars
                .Where(s => s
                            .SeminarsParticipants
                            .Select(sp => sp.ParticipantId)
                            .Contains(GetUserId()))
                .Select(s => new AllSeminarsViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime,
                    Category = s.Category.Name,
                    Organizer = s.Organiser.UserName,
                })
                .ToListAsync();

            return View(seminars);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Seminar? seminar = await CreatorGetSeminarByIdAsync(id);

            var model = new DeleteView
            {
                Id = seminar.Id,
                Topic = seminar.Topic,
                DateAndTime = seminar.DateAndTime
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(DeleteView model)
        {
            var realSeminar = await _context
                .Seminars
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (realSeminar != null)
            {
                _context.Seminars.Remove(realSeminar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("All", "Seminar");

        }

        [HttpPost]
        public async Task<ActionResult> Leave(int id)
        {
            var seminar = await _context
                .Seminars
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync(s => s.Id == id);

            var participantId = GetUserId();

            SeminarParticipant? sp = seminar
                .SeminarsParticipants
                .FirstOrDefault(sp => sp.ParticipantId == participantId);

            if (sp != null)
            {
                seminar.SeminarsParticipants.Remove(sp);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("All", "Seminar");

        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<Seminar?> CreatorGetSeminarByIdAsync(int id)
        {
            Seminar? seminar = await _context
                .Seminars
                .FirstOrDefaultAsync(s =>
                    s.Id == id && GetUserId() == s.OrganiserId);

            return seminar;
        }

    }
}
