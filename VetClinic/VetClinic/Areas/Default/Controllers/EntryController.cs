﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VetClinic.Controllers;
using VetClinic.Models;
using VetClinic.Models.ViewModels;

namespace VetClinic.Areas.Default.Controllers
{
    public class EntryController : BaseController
    {
        //
        // GET: /Default/Entry/

            [HttpGet]
        public ActionResult Index()
        {
            ScheduleView newSchedule = new ScheduleView();
            newSchedule.DoctorList = _repository.GetDoctors().ToList();
            newSchedule.PetList = _repository.GetPets().ToList();
            return View(newSchedule);
        }
        [HttpPost]
        public ActionResult Index(ScheduleView newSchedule)
        {
            bool anyPetName = _repository.GetSchedules().Any(p => string.Compare(p.Pet.ToString(), newSchedule.Pet.ToString()) == 0);
            bool anyPetMaster = _repository.GetSchedules().Any(p => string.Compare(p.Date.ToString(), newSchedule.date.ToString()) == 0);
            if (anyPetName && anyPetMaster)
            {
                ModelState.AddModelError("Title", "Питомец уже записан на выбранную дату");
            }

            if (ModelState.IsValid)
            {
                var currentSchedule = (Schedule)_mapper.Map(newSchedule, typeof(ScheduleView), typeof(Schedule));
                this.SavePet(currentSchedule);
                return RedirectToAction("Index", "Success");

            }
            newSchedule.DoctorList = _repository.GetDoctors().ToList();
            newSchedule.PetList = _repository.GetPets().ToList();
            return View(newSchedule);
        }

        private void SavePet(Schedule currentSchedule)
        {
            _repository.AddSchedule(currentSchedule);
        }
    }
}