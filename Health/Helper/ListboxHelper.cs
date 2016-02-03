using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health.EnumType;
using System.Web.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using HealthDB;
using HealthDB.Context;
using HealthDB.Model;

namespace Health.Helper
{
    public class ListboxHelper
    {
        public List<CustomListItem> GetAllPrescriptionType()
        {
            PrescriptionContext pc = new PrescriptionContext();
            IEnumerable <PrescriptionType> PrescriptionTypes = from presctype in pc.PrescriptionTypes
                                               select presctype;
            List<CustomListItem> cli = new List<CustomListItem>();
            foreach (var p in PrescriptionTypes)
            {
                CustomListItem c = new CustomListItem();
                c.text = p.Name;
                c.value = p.Id;
                cli.Add(c);
            }
            return cli;
        }

        public List<CustomListItem> GetAllGender()
        {
            List < CustomListItem > cli = new List<CustomListItem>();
            var result = new CRUD().getAllItems(new AmazonDBHelper().AmazonDBConnectionString(), "gender");
            cli = BuildCustomList(result, "id", "gender");
            return cli;
        }
        
        public List<CustomListItem> GetAllUserTypes()
        {
            List<CustomListItem> cli = new List<CustomListItem>();
            var result = new CRUD().getAllItems(new AmazonDBHelper().AmazonDBConnectionString(), "usertype");
            cli = BuildCustomList(result, "id", "usertype");
            return cli;
        }

        public List<CustomListItem> GetAllLocationType()
        {
            List<CustomListItem> cli = new List<CustomListItem>();
            var result = new CRUD().getAllItems(new AmazonDBHelper().AmazonDBConnectionString(), "locationtype");
            cli = BuildCustomList(result, "id", "locationtype");
            return cli;
        }

        public List<CustomListItem> GetTableID()
        {
            List<CustomListItem> cli = new List<CustomListItem>();
            var result = new CRUD().getAllItems(new AmazonDBHelper().AmazonDBConnectionString(), "GenerateID");
            cli = BuildCustomList(result, "ID", "TableName");
            return cli;
        }

        public class CustomListItem
        {
            public int value { get; set; }
            public string text { get; set; }
        }

        #region private
       
        private List<CustomListItem> BuildCustomList(List<Dictionary<string, AttributeValue>> result, string valueName, string textName)
        {
            List<CustomListItem> cli = new List<CustomListItem>();

            foreach (Dictionary<string, AttributeValue> item in result)
            {
                // Process the result.
                CustomListItem c = new CustomListItem();
                foreach (KeyValuePair<string, AttributeValue> kvp in item)
                {
                    string attributeName = kvp.Key;
                    AttributeValue value = kvp.Value;
                    if (attributeName == valueName)
                    {
                        c.value = Convert.ToInt32(value.N);
                    }
                    if (attributeName == textName)
                    {
                        c.text = value.S;
                    }
                }
                cli.Add(c);
            }
            return cli;
        }
        #endregion
    }
}