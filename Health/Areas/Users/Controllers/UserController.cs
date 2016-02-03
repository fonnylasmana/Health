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
    public class UserController : Controller
    {
        // GET: Users/User
        public ActionResult Index()
        {
            UserProfile model = new UserProfile();
            ListboxHelper g = new ListboxHelper();
            model.getAllGender = g.GetAllGender();
            model.getAllUserTypes = g.GetAllUserTypes();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(UserProfile model)
        {
            var client = new Connection().ConnectAmazonDynamoDB(new AmazonDBHelper().AmazonDBConnectionString());

            var table = Table.LoadTable(client, "user");
            var item = new Document();

            item["email"] = model.Email;
            item["password"] = model.Password;
            item["dob"] = model.DOB;
            item["firstname"] = model.FirstName;
            item["lastname"] = model.LastName;
            if (model.MiddleName != null && model.MiddleName != "")
                item["middlename"] = model.MiddleName;
            item["gender"] = model.GenderID;
            item["mobilephone"] = model.MobilePhone;
            item["usertype"] = model.UserTypeID;

            var licenses = new Document();
            if (model.LicenseExpireDate != null && model.LicenseExpireDate != "")
                licenses.Add("licenseexpiredate", model.LicenseExpireDate);
            if (model.LicenseIssueDate != null && model.LicenseIssueDate != "")
                licenses.Add("licenseissuedate", model.LicenseIssueDate);
            if (model.LicenseIssuePlace != null && model.LicenseIssuePlace != "")
                licenses.Add("licenseissueplace", model.LicenseIssuePlace);
            if (model.LicenseNumber != null && model.LicenseNumber != "")
                licenses.Add("licensenumber", model.LicenseNumber);
            item.Add("license", licenses);

            table.PutItem(item);

            ListboxHelper g = new ListboxHelper();
            model.getAllGender = g.GetAllGender();
            model.getAllUserTypes = g.GetAllUserTypes();
            return RedirectToAction("AddLocation", "User", new {Email = model.Email });
        }

        public ActionResult AddLocation(string Email)
        {
            ListboxHelper g = new ListboxHelper();
            Location l = new Location();
            l.getAllLocationTypes = g.GetAllLocationType();

            return View(l);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save(Location l) {
            AddLocation(l);
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "AddAnother")]
        public ActionResult AddAnother(Location l){
            AddLocation(l);
            return RedirectToAction("AddLocation", "User", new { Email = l.Email });
        }

        [HttpPost]
        public JsonResult CheckEmail(string Email)
        {
            var client = new Connection().ConnectAmazonDynamoDB(new AmazonDBHelper().AmazonDBConnectionString());
            var result = new CRUD().getItem(new AmazonDBHelper().AmazonDBConnectionString(), "user", Email, ":v_email", "email");
            if (result.Count >= 1)
                return Json("false");
            else
                return Json("true");
        }

        #region private function
        private bool AddLocation(Location l)
        {
            int ID = 1;
            bool result = false;
            try
            {
                var client = new Connection().ConnectAmazonDynamoDB(new AmazonDBHelper().AmazonDBConnectionString());

                //Get ID
                List<ListboxHelper.CustomListItem> cli = new ListboxHelper().GetTableID();
                foreach (var li in cli)
                {
                    if (li.text == "Location")
                    {
                        ID = li.value;
                        ID += 1;
                    }
                }

                var table = Table.LoadTable(client, "location");
                var item = new Document();
                item["id"] = ID;

                item["city"] = l.City;
                item["postalcode"] = l.PostalCode;
                item["province"] = l.Province;
                item["street1"] = l.Street1;
                if (l.Street2 != null && l.Street2 != "")
                    item["street2"] = l.Street2;

                item["email"] = l.Email;
                item["locationtypeid"] = l.LocationTypeID;
                item["locname"] = l.LocName;


                if (l.LocFax != null && l.LocFax != "")
                    item["locfax"] = l.LocFax;
                item["locphone"] = l.LocPhoneMain;
              
                table.PutItem(item);

                new CRUD().UpdateItem(new AmazonDBHelper().AmazonDBConnectionString(), "GenerateID", "location", ":i", "ID", "#I", (ID + 1).ToString());
            }
            catch (Exception)
            {

            }
            return result;

        }
        
        #endregion
    }
}