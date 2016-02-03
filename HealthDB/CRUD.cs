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
    public class CRUD
    {
        public List<Dictionary<string,AttributeValue>> getAllItems(string ConnectionString, string tableName)
        {

            Connection c = new Connection();
            var client = c.ConnectAmazonDynamoDB(ConnectionString);
            var request = new ScanRequest
            {
                TableName = tableName,
            };

            var response = client.Scan(request);
            var result = response.Items;
            return result;
        }

        public List<Dictionary<string, AttributeValue>> getItem(string ConnectionString, string tableName, string inputparam, string expressionText, string columnName)
        {

            Connection c = new Connection();
            var client = c.ConnectAmazonDynamoDB(ConnectionString);

            var table = Table.LoadTable(client, tableName);

            var request = new ScanRequest
            {
                TableName = tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        {expressionText, new AttributeValue { S = inputparam}}
                    },
                FilterExpression = columnName + "=" + expressionText
            };

            var response = client.Scan(request);
            var result = response.Items;
            return result;
        }

        public List<Dictionary<string, AttributeValue>> getItemContain(string ConnectionString, string tableName, string inputparam, string columnName)
        {

            Connection c = new Connection();
            var client = c.ConnectAmazonDynamoDB(ConnectionString);

            var table = Table.LoadTable(client, tableName);


            // Define scan conditions
            Dictionary<string, Condition> conditions = new Dictionary<string, Condition>();

            // Title attribute should contain the string "Adventures"
            Condition titleCondition = new Condition();
            titleCondition.ComparisonOperator = "BEGINS_WITH";
            titleCondition.AttributeValueList.Add(new AttributeValue { S = inputparam });
            conditions[columnName] = titleCondition;


            var request = new ScanRequest
            {
                TableName = tableName,
                ScanFilter = conditions
            };

            var response = client.Scan(request);
            var result = response.Items;
            return result;
        }

        public List<Dictionary<string, AttributeValue>> getItemWithFilters(string ConnectionString, string tableName, List<string> inputparam, List<string> expressionText, List<string> columnName)
        {
            Connection c = new Connection();
            var client = c.ConnectAmazonDynamoDB(ConnectionString);

            var table = Table.LoadTable(client, tableName);

            var strFilterExpression = columnName[0] + " = " + expressionText[0];
            var newDictionary = new Dictionary<string, AttributeValue>
            {
                {expressionText[0], new AttributeValue { S = inputparam[0]}}
            };

            if (columnName.Count() > 1)
            {
                for (int i=1; i < columnName.Count(); i++)
                {
                    strFilterExpression += " and " + columnName[i] + " = " + expressionText[i];
                    newDictionary.Add(expressionText[i], new AttributeValue { S = inputparam[i] });
                }
            }

            var request = new ScanRequest
            {
                TableName = tableName,
                ExpressionAttributeValues = newDictionary,
                FilterExpression = strFilterExpression
            };

            var response = client.Scan(request);
            var result = response.Items;
            return result;
        }

        public void InsertItem(string ConnectionString, string tableName, List<string> columnName, List<string> types, List<string> value)
        {
            Connection c = new Connection();
            var client = c.ConnectAmazonDynamoDB(ConnectionString);

            var table = Table.LoadTable(client, tableName);
            var item = new Document();
            for (int i = 0; i < columnName.Count(); i++)
            {
                if (types[i].ToLower() == "int")
                    item[columnName[i]] = Convert.ToInt32(value[i]);
                else
                    item[columnName[i]] = value[i];
            }
            table.PutItem(item);
        }

        public void UpdateItem(string ConnectionString, string tableName, string whereparam, string expressionText, string columnName, string abbvrName, string updatedValue)
        {
            Connection c = new Connection();
            var client = c.ConnectAmazonDynamoDB(ConnectionString);

            var request = new UpdateItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>() { { "TableName", new AttributeValue { S = whereparam } } },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {abbvrName, columnName}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {expressionText,new AttributeValue {N = updatedValue}},
                },

                // This expression does the following:
                // 1) Adds two new authors to the list
                // 2) Reduces the price
                // 3) Adds a new attribute to the item
                // 4) Removes the ISBN attribute from the item
                UpdateExpression = "SET " + expressionText +  "=" + expressionText + "-" +  expressionText
            };
        }
    }
}
