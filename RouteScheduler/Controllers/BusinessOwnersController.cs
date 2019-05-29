﻿using Microsoft.AspNet.Identity;
using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Controllers
{
    public class BusinessOwnersController : Controller
    {
        private APIKeys aPIKeys = new APIKeys();
        private ApplicationDbContext db = new ApplicationDbContext();
        private GoogleLogic gl = new GoogleLogic();
        private SchedulingLogic sl = new SchedulingLogic();

        // GET: BusinessOwner
        public ActionResult Index()
        {
            ServiceRequested service = new ServiceRequested();

            var UserId = User.Identity.GetUserId();
            double lat = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault().Latitude;
            double lng = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault().Longitude;
            string ApiIs = ($"https://www.google.com/maps/embed/v1/view?zoom=10&center={lat},{lng}&key=" + aPIKeys.ApiKey);
            ViewData["ApiKey"] = ApiIs;

            ViewData["NameIs"] = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault().FirstName;
            var eventsAre = db.Events.Where(e => e.Customer.ApplicationId == UserId);
            return View(eventsAre);
        }

        public ActionResult TodaysRoute()
        {
            var currentPerson = User.Identity.GetUserId();
            var Longitude = db.BusinessOwners.Where(c => c.ApplicationId == currentPerson).FirstOrDefault().Longitude;
            var Latitude = db.BusinessOwners.Where(c => c.ApplicationId == currentPerson).FirstOrDefault().Latitude;
            string DisplayIs = ($"https://www.google.com/maps/embed/v1/view?zoom=16&center={Latitude},{Longitude}&key=" + aPIKeys.ApiKey);
            ViewData["DisplayIs"] = DisplayIs;
            return View();
        }

        public ActionResult Calendar()
        {
            
            return View();
        }

        public ActionResult AssignToSchedule(int? id)
        {
            Event newEvent = new Event();
            var Customer = db.Customers.Where(b => b.CustomerId == id).FirstOrDefault();
            newEvent.CustomerId = Customer.CustomerId;
            return View(newEvent);
        }

        [HttpPost]
        public ActionResult AssignToSchedule([Bind(Include = "CustomerId,Latitude,Longitude,StartDate,EndDate")] Event @event)
        {

            try
            {
                var UserId = User.Identity.GetUserId();
                var business = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault();
                @event.BusinessId = business.BusinessId;
                if (ModelState.IsValid)
                {

                    db.Events.Add(@event);
                    db.SaveChanges();
                }
                return RedirectToAction("Calendar");
            }
            catch
            {
                return View(@event);
            }
            
        }

        public async Task<ActionResult> ScheduleeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequested serviceRequested = await db.ServiceRequests.FindAsync(id);
            if (serviceRequested == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequested);
        }

        public ActionResult ViewServiceRequests()
        {
            var currentPerson = User.Identity.GetUserId();
            var serviceRequests = db.ServiceRequests.Where(e => e.BusinessTemplate.BusinessOwner.ApplicationId == currentPerson);
            return View(serviceRequests);
        }




        public ActionResult DisplayRoute()
        {
            return View();
        }

        // GET: BusinessOwner/Details/5
        public ActionResult Details()
        {
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.BusinessOwners.Where(x => currentPerson == x.ApplicationId).FirstOrDefault();
            return View(currentUser);
        }

        // GET: BusinessOwner/Create
        public ActionResult Create()
        {
            BusinessOwner owner = new BusinessOwner();
            return View(owner);
        }

        // POST: BusinessOwner/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "BusinessId,FirstName, LastName, Address, City, State, Zipcode")] BusinessOwner businessOwner)
        {
            var Geocode = gl.GeocodeAddress(businessOwner.Address, businessOwner.City, businessOwner.State);
            

            try
            {
                businessOwner.ApplicationId = User.Identity.GetUserId();
                businessOwner.Latitude = Geocode[0];
                businessOwner.Longitude = Geocode[1];
                if (ModelState.IsValid)
                {

                    db.BusinessOwners.Add(businessOwner);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(businessOwner);
            }
        }


        // GET: BusinessOwner/Edit/5
        public ActionResult Edit(int? id)
        {
            var businessIs = db.BusinessOwners.Where(b => b.BusinessId == id).FirstOrDefault();
            return View(businessIs);
        }

        // POST: BusinessOwner/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "BusinessId, FirstName, LastName, Address, City, State, Zipcode")] BusinessOwner businessOwner)
        {
            try
            {
                db.Entry(businessOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}