using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using missing_people.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NpgsqlTypes;
using Microsoft.AspNetCore.Authorization;

namespace missing_people.Controllers
{
    public class MissingPersonController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MissingPersonController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int gender)
        {
            List<MissingPerson> missingPersonList;

            if (gender == 1)
            {
                missingPersonList = await _context.MissingPeople
                    .Where(x => x.Gender == "Male")
                    .ToListAsync();
            }
            else if (gender == 2)
            {
                missingPersonList = await _context.MissingPeople
                    .Where(x => x.Gender == "Female")
                    .ToListAsync();
            }
            else missingPersonList = await _context.MissingPeople.ToListAsync();

            return View(missingPersonList);
        }


        public async Task<IActionResult> Details(int id)
        {
            var missingPerson = await _context.MissingPeople.FindAsync(id);
            return View(missingPerson);
        }

        [Authorize(Roles = "admin,user")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Description,ImageUrl,DateLastSeen,LocationLastSeen,ContactName,ContactEmail,UserId,Gender")] MissingPerson person)
        {
            if (person.Gender == "0")
            {
                person.Gender = "Male";
            }
            else { person.Gender = "Female"; }

            person.DateLastSeen = person.DateLastSeen.ToUniversalTime();


            _context.MissingPeople.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var missingPerson = await _context.MissingPeople.FindAsync(id);
            return View(missingPerson);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,FirstName,LastName,Description,ImageUrl,DateLastSeen,LocationLastSeen,ContactName,ContactEmail,UserId,Gender")] MissingPerson person)
        {
            var editedMissingPerson = new MissingPerson
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Description = person.Description,
                ImageUrl = person.ImageUrl,
                DateLastSeen = person.DateLastSeen.ToUniversalTime(),
                LocationLastSeen = person.LocationLastSeen,
                ContactName = person.ContactName,
                ContactEmail = person.ContactEmail,
                Gender = person.Gender
            };
            if (person.Gender == "0")
            {
                editedMissingPerson.Gender = "Male";
            }
            else editedMissingPerson.Gender = "Female";

            _context.Update(editedMissingPerson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var missingPerson = await _context.MissingPeople.FindAsync(id);
            return View(missingPerson);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.MissingPeople.FindAsync(id);
            _context.MissingPeople.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MissingPersonExists(Guid id)
        {
            return _context.MissingPeople.Any();
        }
    }
}
