﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uchange.Models;

namespace uchange.Controllers
{
    public class ItemController : Controller
    {
        ItemDBContext item = new ItemDBContext();
        PersonDBContext person = new PersonDBContext();
        RequestDBContext request = new RequestDBContext();

        public ActionResult Index()
        {
            ViewBag.item_now = person.Persons.Find(User.Identity.Name).item_now;
            return View(item.Items.ToList());
        }

        public ActionResult Detail(int id)
        {
            ItemDB it = item.Items.Find(id);
            PersonDB stu=person.Persons.Find(User.Identity.Name);
            ViewBag.flag = 0;
            if (it.id == stu.item_now)
                ViewBag.flag = 1;
            else
            {
                foreach (var r in request.Requests.ToList())
                {
                    if (r.to == stu.item_now)
                    {
                        PersonDB p = person.Persons.Find(r.from);
                        if (id == p.item_now)
                        {
                            ViewBag.flag = 2;
                            break;
                        }
                    }
                }
            }
            foreach (var r in request.Requests.ToList())
            {
                if (r.from == stu.student_id && r.to == id)
                {
                    ViewBag.flag = 3;
                    break;
                }
            }
            return View(it);
        }

        public ActionResult Edit(int id)
        {
            ItemDB it = item.Items.Find(id);
            return View(it);
        }

        [HttpPost]
        public ActionResult Edit(int id, string name, string description)
        {
            ItemDB it = item.Items.Find(id);
            it.name = name;
            it.description = description;
            item.SaveChanges();
            return RedirectToAction("Detail", new { id = id });
        }
    }
}
