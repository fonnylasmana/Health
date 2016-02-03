using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using System.Configuration;

namespace Health.Helper
{
    public class AmazonDBHelper
    {
        public string AmazonDBConnectionString()
        {
            return ConfigurationManager.AppSettings["AmazonDynamoDB"].ToString();
        }
    }
}