using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITSSelfRunning.DataAccess;
using ITSSelfRunning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ITSSelfRunning.Controllers
{
    public class ActivityController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RunnerRepository _runnerRepository;
        private readonly ActivityRepository _activityRepository;

        public ActivityController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _runnerRepository = new RunnerRepository(_configuration["ConnectionStrings:DB"]);
            _activityRepository = new ActivityRepository(_configuration["ConnectionStrings:DB"]);

        }

        [Authorize]
        // GET: Activity
        public async Task<ActionResult> Index()
        {
            var user = await _runnerRepository.GetRunner(_userManager.GetUserName(User));

            var list = await _activityRepository.GetTrainings(user.IdRunner);

            return View(list);
        }

        // GET: Activity/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Activity/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Activity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Activity/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Activity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Activity/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Activity/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}