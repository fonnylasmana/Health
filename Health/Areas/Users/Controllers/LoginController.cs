using System;
using System.Web.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Health.Areas.Users.Models;
using Health.Helper;
using System.Collections.Generic;
using System.Configuration;
using HealthDB;

namespace Health.Areas.Users.Controllers
{
    public class LoginController : Controller
    {
        // GET: Users/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login l)
        {
            //var client = new Connection().ConnectAmazonDynamoDB(new AmazonDBHelper().AmazonDBConnectionString());
            //var result = new CRUD().get2Item(new AmazonDBHelper().AmazonDBConnectionString(), "login", l.Email, ":v_email", "email", l.Password, ":v_password", "password");
            //if (result.Count >= 1)
            //    l.Result = "Successful";
            //else
            //    l.Result = "Failed";
            return View(l);
        }
    }
}