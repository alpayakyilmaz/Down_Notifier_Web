using Down_Notifier_Web.Data;
using Down_Notifier_Web.Models;
using Down_Notifier_Web.Util;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Down_Notifier_Web.Controllers
{
    [Authorize]
    public class HealthCheckController : Controller
    {
        //private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public HealthCheckController(ApplicationDbContext dbContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailService;
        }
        public IActionResult Index()
        {           
            try
            {
                var listModel = _dbContext.HealthChecks.Where(x => x.IsDeleted == false);
                return View(listModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something unexpected happened, please try again.");
                HttpContext.RaiseError(ex);
            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(HealthCheckModel model)
        {
            try
            {
                _dbContext.Add(model);
                _dbContext.SaveChanges();
                ServiceBuilder.CreateNewCheck(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something unexpected happened in created, please try again.");
                HttpContext.RaiseError(ex);
            }
            return BadRequest();
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.HealthChecks.FirstOrDefault(x => x.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(HealthCheckModel model)
        {
            try
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();
                ServiceBuilder.UpdateCheck(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something unexpected happened in updated, please try again.");

              
                HttpContext.RaiseError(ex);
            }

            return BadRequest();
        }

        public IActionResult Delete(int id)
        {
            var model = _dbContext.HealthChecks.FirstOrDefault(x => x.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var model = _dbContext.HealthChecks.FirstOrDefault(x => x.Id == id);
                model.IsDeleted = true;
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();
                ServiceBuilder.DeleteCheck(model);
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something unexpected happened in deleted, please try again.");
                HttpContext.RaiseError(ex);
            }

            return BadRequest();
        }
    }
}
