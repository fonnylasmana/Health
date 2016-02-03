using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Health.Areas.Prescription.Models;
using  Health.Helper;
using Amazon.DynamoDBv2.DocumentModel;
using HealthDB;
using Amazon.DynamoDBv2.Model;
using Health.Areas.Users.Models;
using HealthDB.Context;
using HealthDB.Model;
using System.Data.Linq;

namespace Health.Areas.Prescription.Controllers
{
    public class PrescriptionController : Controller
    {
        // GET: Prescription/Prescription
        public ActionResult Add(int PatientID, int DoctorID)
        {
            Models.Prescription model = new Models.Prescription();
            model = GetPrescription(PatientID, DoctorID, DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + "_" + PatientID + "_" + DoctorID, model);

            return View(model);
        }

        public ActionResult Delete (int id, int PatientID, int DoctorID)
        {
            PrescriptionContext pc = new PrescriptionContext();
            HealthDB.Model.Prescription p = (from Prescriptions in pc.Prescriptions
                                            where Prescriptions.Id == id
                                            select Prescriptions).FirstOrDefault();
            pc.Prescriptions.DeleteOnSubmit(p);
            pc.SubmitChanges();
            return RedirectToAction("Add", "Prescription", new { PatientID = PatientID, DoctorID = DoctorID});

        }

        [HttpPost]
        public ActionResult Add(Health.Areas.Prescription.Models.Prescription model)
        {
            PrescriptionContext pc = new PrescriptionContext();
            HealthDB.Model.Prescription p = new HealthDB.Model.Prescription();
            p.DoctorId = model.PrescribedByUserId;
            p.Dosage = model.Strength;
            p.DrugName = model.DrugName;
            if (model.GroupName != null)
                p.GroupName = model.GroupName;
            p.Instruction = model.Direction;
            p.Invoice = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + "_" + model.PatientUserId + "_" + model.PrescribedByUserId;
            p.PatientId = model.PatientUserId;
            p.PharmacistId = model.PharmacistUserId;
            p.PrescriptionTypeId = model.PrescriptionTypeId;
            p.Quantity = model.Quantity;
            p.PrescriptionStatusId = 1;
            pc.Prescriptions.InsertOnSubmit(p);

            IEnumerable<DrugName> DrugNames = from drugs in pc.DrugNames
                                              where drugs.Name == model.DrugName
                                              select drugs;
            if (DrugNames == null)
            {
                DrugName dn = new DrugName();
                dn.Name = model.DrugName;
                pc.DrugNames.InsertOnSubmit(dn);
            }

            pc.SubmitChanges();
            model = GetPrescription(model.PatientUserId, model.PrescribedByUserId, DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + "_" + model.PatientUserId + "_" + model.PrescribedByUserId, model);

            return View(model);
        }

        public ActionResult Index()
        {
            SearchUser model = new SearchUser();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(SearchUser model)
        {
            model = SearchUser(model);

            return View(model);
        }

        public ActionResult Select(int PatientID)
        {
            SearchUser model = new SearchUser();
            model.PatientID = PatientID;
            return View(model);
        }
        [HttpPost]
        public ActionResult Select(SearchUser model)
        {
            UserContext UserContext = new UserContext();
            int UserTypeId = (from usertype in UserContext.UserTypes
                          where usertype.Name == "Dokter Umum"
                          select usertype.Id).FirstOrDefault();
            model.UserTypeId = UserTypeId;
            model = SearchUser(model);
            return View(model);
        }

        public bool InsertDrugName(string DrugName)
        {
            var result = new CRUD().getItem(new AmazonDBHelper().AmazonDBConnectionString(), "drugname", DrugName, ":v_drugname", "drugname");
            if (result.Count == 0)
            {
                int ID = 1;
                //Get ID
                List<ListboxHelper.CustomListItem> cli = new ListboxHelper().GetTableID();
                foreach (var li in cli)
                {
                    if (li.text == "drugname")
                    {
                        ID = li.value;
                        ID += 1;
                    }
                }
                List<string> columnName = new List<string>();
                columnName.Add("id");
                columnName.Add("drugname");
                List<string> types = new List<string>();
                types.Add("int");
                types.Add("string");
                List<string> value = new List<string>();
                value.Add(ID.ToString());
                value.Add(DrugName.ToLower());
                new CRUD().InsertItem(new AmazonDBHelper().AmazonDBConnectionString(), "drugname", columnName, types, value);
            }
            return true;
        }
        [HttpPost]
        public JsonResult AutoCompleteDrugName(string DrugName)
        {
            List<string> DrugNameList = new List<string>();
            if (DrugName.ToString().Length >= 3)
            {
                PrescriptionContext PrescriptionContext = new PrescriptionContext();
                DrugNameList = (from drugs in PrescriptionContext.DrugNames
                                                  where drugs.Name.Contains(DrugName)
                                                  select drugs.Name).ToList();
            }
            

           return Json(DrugNameList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Order(int PatientID, int DoctorID, string InvoiceId)
        {
            PrescriptionContext pc = new PrescriptionContext();
            IEnumerable<HealthDB.Model.Prescription> ps = from Prescriptions in pc.Prescriptions
                                                          where Prescriptions.Invoice == InvoiceId
                                                          select Prescriptions;
            foreach (var p in ps)
            {
                p.PrescriptionStatusId = 2;
            }
            pc.SubmitChanges();
            Models.Prescription model = new Models.Prescription();
            model = GetPrescription(PatientID, DoctorID, InvoiceId, model);

            return View(model);
        }

        #region private methods
        private Models.Prescription GetPrescription(int PatientID, int DoctorID, string InvoiceId, Models.Prescription model)
        {
            ListboxHelper g = new ListboxHelper();
            model.getAllPrescriptionTypes = g.GetAllPrescriptionType();
            model.Patient = GetUser(PatientID);
            model.Doctor = GetUser(DoctorID);
            model.PatientUserId = PatientID;
            model.PrescribedByUserId = DoctorID;
            model.InvoiceId = InvoiceId;
            List<Drug> Drugs = new List<Drug>();

            PrescriptionContext pc = new PrescriptionContext();
            IEnumerable<HealthDB.Model.Prescription> ps = from Prescriptions in pc.Prescriptions
                                                          where Prescriptions.Invoice == InvoiceId
                                                          select Prescriptions;

            foreach (var p in ps)
            {
                Drug d = new Drug();
                d.Id = p.Id;
                d.Direction = p.Instruction;
                d.DrugName = p.DrugName;
                d.GroupName = p.GroupName;
                d.PrescriptionType = (from PrescriptionTypes in pc.PrescriptionTypes
                                      where PrescriptionTypes.Id == p.PrescriptionTypeId
                                      select PrescriptionTypes.Name).FirstOrDefault();
                d.Quantity = p.Quantity;
                d.Strength = p.Dosage;
                Drugs.Add(d);
            }
            model.Drugs = Drugs.OrderBy(ds => ds.PrescriptionType + ds.GroupName + ds.DrugName).ToList();
            return model;
        }

        private User GetUser (int ID)
        {
            UserContext uc = new UserContext();
            User u = (from users in uc.Users
                      where users.Id == ID
                      select users).FirstOrDefault();
            return u;
        }
        private SearchUser SearchUser(SearchUser model)
        {
            List<User> Users = new List<User>();
            UserContext UserContext = new UserContext();
            if (model.Email != null)
            {
                IEnumerable<User> us = from user in UserContext.Users
                                       where user.Email == model.Email
                                       && (user.UsertypeId == model.UserTypeId || model.UserTypeId == null)
                                       select user;

                model.Users = us.ToList();
            }
            else
            {
                IEnumerable<User> us = from user in UserContext.Users
                                       where (user.FirstName == model.FirstName || model.FirstName == null)
                                        && (user.LastName == model.LastName || model.LastName == null)
                                        && (user.DOB == Convert.ToDateTime(model.DOB) || model.DOB == null)
                                        && (user.UsertypeId == model.UserTypeId || model.UserTypeId == null)
                                       select user;

                model.Users = us.ToList();
            }

            

            //var client = new Connection().ConnectAmazonDynamoDB(new AmazonDBHelper().AmazonDBConnectionString());
            //List<int> userids = new List<int>();
            //List<UserProfile> UserProfileList = new List<UserProfile>();

            //if (model.Email != null)
            //{
            //    var result = new CRUD().getItem(new AmazonDBHelper().AmazonDBConnectionString(), "login", model.Email, ":v_email", "email");
            //    //Get all matching user Ids
            //    foreach (Dictionary<string, AttributeValue> item in result)
            //    {
            //        foreach (KeyValuePair<string, AttributeValue> kvp in item)
            //        {
            //            string attributeName = kvp.Key;
            //            AttributeValue value = kvp.Value;
            //            if (attributeName == "userid")
            //            {
            //                userids.Add(Convert.ToInt32(value.S));
            //            }
            //        }
            //    }

            //    if (userids != null && userids.Count() > 0)
            //    {


            //        foreach (var u in userids)
            //        {
            //            var PatientResult = new CRUD().getItem(new AmazonDBHelper().AmazonDBConnectionString(), "user", u.ToString(), ":v_id", "id");
            //            foreach (Dictionary<string, AttributeValue> itemP in PatientResult)
            //            {
            //                UserProfile UserProfile = new UserProfile();
            //                UserProfile.Email = model.Email;
            //                foreach (KeyValuePair<string, AttributeValue> kvpP in itemP)
            //                {
            //                    string attributeName = kvpP.Key;
            //                    AttributeValue value = kvpP.Value;
            //                    if (attributeName == "id")
            //                    {
            //                        UserProfile.Id = Convert.ToInt32(value.S);
            //                    }
            //                    if (attributeName == "firstname")
            //                    {
            //                        UserProfile.FirstName = value.S;
            //                    }
            //                    if (attributeName == "lastname")
            //                    {
            //                        UserProfile.LastName = value.S;
            //                    }
            //                    if (attributeName == "dob")
            //                    {
            //                        UserProfile.DOB = value.S;
            //                    }
            //                    if (attributeName == "usertypeid")
            //                    {
            //                        UserProfile.UserTypeID = Convert.ToInt32(value.N);
            //                    }
            //                }
            //                UserProfileList.Add(UserProfile);
            //            }
            //        }
            //    }
            //    model.Users = UserProfileList;
            //}
            //else
            //{
            //    List<string> inputparam = new List<string>();
            //    List<string> expressionText = new List<string>();
            //    List<string> columnName = new List<string>();
            //    if (model.FirstName != null)
            //    {
            //        inputparam.Add(model.FirstName);
            //        expressionText.Add(":v_firstname");
            //        columnName.Add("firstname");
            //    }
            //    if (model.LastName != null)
            //    {
            //        inputparam.Add(model.LastName);
            //        expressionText.Add(":v_lastname");
            //        columnName.Add("lastname");
            //    }
            //    if (model.DOB != null)
            //    {
            //        inputparam.Add(model.DOB);
            //        expressionText.Add(":v_dob");
            //        columnName.Add("dob");
            //    }

            //    var result = new CRUD().getItemWithFilters(new AmazonDBHelper().AmazonDBConnectionString(), "user", inputparam, expressionText, columnName);

            //    //Get all matching user Ids
            //    foreach (Dictionary<string, AttributeValue> itemP in result)
            //    {
            //        UserProfile UserProfile = new UserProfile();
            //        UserProfile.Email = model.Email;
            //        foreach (KeyValuePair<string, AttributeValue> kvpP in itemP)
            //        {
            //            string attributeName = kvpP.Key;
            //            AttributeValue value = kvpP.Value;
            //            if (attributeName == "id")
            //            {
            //                UserProfile.Id = Convert.ToInt32(value.S);
            //                userids.Add(Convert.ToInt32(value.S));
            //            }
            //            if (attributeName == "firstname")
            //            {
            //                UserProfile.FirstName = value.S;
            //            }
            //            if (attributeName == "lastname")
            //            {
            //                UserProfile.LastName = value.S;
            //            }
            //            if (attributeName == "dob")
            //            {
            //                UserProfile.DOB = value.S;
            //            }
            //            if (attributeName == "usertypeid")
            //            {
            //                UserProfile.UserTypeID = Convert.ToInt32(value.N);
            //            }
            //        }
            //        UserProfileList.Add(UserProfile);
            //    }

            //    foreach (var u in UserProfileList)
            //    {
            //        var EmailResult = new CRUD().getItem(new AmazonDBHelper().AmazonDBConnectionString(), "login", u.Id.ToString(), ":v_userid", "userid");
            //        //Get all matching Emails
            //        foreach (Dictionary<string, AttributeValue> item in EmailResult)
            //        {
            //            foreach (KeyValuePair<string, AttributeValue> kvp in item)
            //            {
            //                string attributeName = kvp.Key;
            //                AttributeValue value = kvp.Value;
            //                if (attributeName == "email")
            //                {
            //                    u.Email = value.S;
            //                }
            //            }
            //        }
            //    }
            //    model.Users = UserProfileList;
            //}
            return model;
        }
        #endregion
    }
}
