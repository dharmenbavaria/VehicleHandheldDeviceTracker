using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace DeliveryTracker.Data
{
    public class DynamoDbConfig
    {
        public string TableName { get; set; }

        public DynamoDbConfig(string tableName)
        {
            TableName = tableName;
        }

        public DynamoDBOperationConfig GetDynamoDbOperationConfig()
        {
            return new DynamoDBOperationConfig
            {
                OverrideTableName = TableName,
                ConsistentRead = false
            };
        }

    }
}
