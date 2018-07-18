using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSSelfRunning.DataAccess;
using ITSSelfRunning.DataAccess.Models;
using ITSSelfRunning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ITSSelfRunning.Controllers
{
    public class RunController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RunnerRepository _runnerRepository;
        private readonly ActivityRepository _activityRepository;
        private readonly TelemetryRepository _telemetryRepository;

        public RunController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _runnerRepository = new RunnerRepository(_configuration["ConnectionStrings:DB"]);
            _activityRepository = new ActivityRepository(_configuration["ConnectionStrings:DB"]);
            _telemetryRepository = new TelemetryRepository(_configuration["ConnectionStrings:DB"]);
            
        }

        // GET: Run
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Running(int IdActivity, bool isOpen)
        {
            if (isOpen == false)
            {
                _activityRepository.StartTraining(IdActivity); //TODO: cambio lo stato dell'attività da 0 a 1
            }

            return View(IdActivity);
        }

        [Authorize]
        public ActionResult EndRunning(int IdActivity)
        {
            _activityRepository.EndTraining(IdActivity); //cambio lo stato a 2 per terminare

            return RedirectToAction("Index", "Activity", IdActivity);
        }

        [Authorize]
        public ActionResult RunDetails(int IdActivity)
        {
            var activity = _activityRepository.Get(IdActivity);

            var listTelemetry = _telemetryRepository.GetTelemetries(IdActivity);

            var model = new ActivityDetailsViewModel()
            {
                IdActivity = IdActivity,
                ActivityName = activity.ActivityName,
                Status = activity.Status,
                CreationDate = activity.CreationDate,
                Location = activity.Location,
                ActivityType = activity.ActivityType,
                UriGara = activity.UriGara,
                IdRunner = activity.Runner_Id,
                arrayTelemetry = listTelemetry
            };

            return View(model);
        }

        // GET: Run/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Run/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Run/Create
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

        // GET: Run/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Run/Edit/5
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

        // GET: Run/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Run/Delete/5
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

        //Run/SendTelemetry
        [HttpPost]
        [Authorize]
         public async Task<IActionResult> SendTelemetry([FromBody] Telemetry t)
        {
            var json = JsonConvert.SerializeObject(t);

            var bytes = Encoding.UTF8.GetBytes(json);

            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=aletomeaworksasync;AccountKey=gS/ED+jA1gVXVVSrhVBWwL9sm/caRl9K8R7V+EzEyADfZYjY258NfwVhDvw+8xBHogIGwOB/0IjQ5cF2FCtbxw==;EndpointSuffix=core.windows.net");

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("telemetryqueue");

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();

            CloudQueueMessage message = new CloudQueueMessage(json);
            await queue.AddMessageAsync(message);

            return Ok(new { message = "success", result = true });
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateTraining()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTraining(ActivityViewModel avm) //passo l'oggetto dentro la viem model
        {

            var user = await _runnerRepository.GetRunner(_userManager.GetUserName(User));

            Activity a = new Activity //creo un nuovo oggetto activity
            {
                ActivityName = avm.ActivityName,
                Location = avm.Location,
                CreationDate = DateTime.Now,
                Status = 0, //0= To be started, 1=in progress, 2=Closed
                ActivityType = 1, //1=training, 2=gara
                UriGara = "",
                Runner_Id = user.IdRunner

            };

            await _activityRepository.InsertActivity(a);
            return RedirectToAction("Index", "Activity");
        }


    }
}