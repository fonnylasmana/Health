using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using System.Configuration;

namespace HealthDB
{
    public class Connection
    {
        public AmazonDynamoDBClient ConnectAmazonDynamoDB(string ConnectionString)
        {
            var config = new AmazonDynamoDBConfig();
            config.ServiceURL = ConnectionString;
            var client = new AmazonDynamoDBClient(config);
            return client;
        }
    }
}