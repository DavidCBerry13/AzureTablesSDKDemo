using Azure;
using Azure.Data.Tables;
using AzureTablesDemoApplication.Entities;
using AzureTablesDemoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Services
{
    public class TablesService
    {

        public TablesService(TableClient tableClient)
        {
            _tableClient = tableClient;
        }

        private TableClient _tableClient;

        public string[] EXCLUDE_TABLE_ENTITY_KEYS = { "PartitionKey", "RowKey", "odata.etag", "Timestamp" };



        public IEnumerable<WeatherDataModel> GetAllRows()
        {
            List<WeatherDataModel> weatherData = new List<WeatherDataModel>();
            
            var entities = _tableClient.Query<TableEntity>();

            foreach (var entity in entities)
            {
                WeatherDataModel observation = new WeatherDataModel();
                observation.StationName = entity.PartitionKey;
                observation.ObservationDate = entity.RowKey;

                var measurements = entity.Keys.Where(key => !EXCLUDE_TABLE_ENTITY_KEYS.Contains(key));
                foreach(var key in measurements)
                {
                    observation[key] = entity[key];
                }
                weatherData.Add(observation);
            }

            return weatherData;
        }


        public void InsertTableEntity(WeatherInputModel model)
        {
            TableEntity entity = new TableEntity();
            entity.PartitionKey = model.StationName;
            entity.RowKey = $"{model.ObservationDate} {model.ObservationTime}";

            // The other values are added like a items to a dictionary
            entity["Temperature"] = model.Temperature;
            entity["Humidity"] = model.Humidity;
            entity["Barometer"] = model.Barometer;
            entity["WindDirection"] = model.WindDirection;
            entity["WindSpeed"] = model.WindSpeed;
            entity["Precipitation"] = model.Precipitation;

            _tableClient.AddEntity(entity);
        }


        public void UpsertTableEntity(WeatherInputModel model)
        {
            TableEntity entity = new TableEntity();
            entity.PartitionKey = model.StationName;
            entity.RowKey = $"{model.ObservationDate} {model.ObservationTime}";

            // The other values are added like a items to a dictionary
            entity["Temperature"] = model.Temperature;
            entity["Humidity"] = model.Humidity;
            entity["Barometer"] = model.Barometer;
            entity["WindDirection"] = model.WindDirection;
            entity["WindSpeed"] = model.WindSpeed;
            entity["Precipitation"] = model.Precipitation;

            _tableClient.UpsertEntity(entity);
        }


        public void InsertCustomEntity(WeatherInputModel model)
        {
            WeatherDataEntity customEntity = new WeatherDataEntity();
            customEntity.PartitionKey = model.StationName;
            customEntity.RowKey = $"{model.ObservationDate} {model.ObservationTime}";

            // The remaining values are strongly typed properties on the custom entity type
            customEntity.Temperature = model.Temperature;
            customEntity.Humidity = model.Humidity;
            customEntity.Barometer = model.Barometer;
            customEntity.WindDirection = model.WindDirection;
            customEntity.WindSpeed = model.WindSpeed;
            customEntity.Precipitation = model.Precipitation;

            _tableClient.AddEntity(customEntity);
        }


        public void UpsertCustomEntity(WeatherInputModel model)
        {
            WeatherDataEntity customEntity = new WeatherDataEntity();
            customEntity.PartitionKey = model.StationName;
            customEntity.RowKey = $"{model.ObservationDate} {model.ObservationTime}";

            // The remaining values are strongly typed properties on the custom entity type
            customEntity.Temperature = model.Temperature;
            customEntity.Humidity = model.Humidity;
            customEntity.Barometer = model.Barometer;
            customEntity.WindDirection = model.WindDirection;
            customEntity.WindSpeed = model.WindSpeed;
            customEntity.Precipitation = model.Precipitation;

            _tableClient.UpsertEntity(customEntity);
        }


        public void InsertExpandableData(string partitionKey, string rowKey, IDictionary<string, string> fields)
        {
            TableEntity entity = new TableEntity();
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;

            foreach (string fieldName in fields.Keys)
            {
                var value = fields[fieldName];
                
                if (Double.TryParse(value, out double number))
                    entity[fieldName] = number;
                else
                    entity[fieldName] = value;
            }
            _tableClient.AddEntity(entity);
        }


        public void UpsertExpandableData(string partitionKey, string rowKey, IDictionary<string, string> fields)
        {
            TableEntity entity = new TableEntity();
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;

            foreach (string fieldName in fields.Keys)
            {
                var value = fields[fieldName];

                if (Double.TryParse(value, out double number))
                    entity[fieldName] = number;
                else
                    entity[fieldName] = value;
            }
            _tableClient.UpsertEntity(entity);
        }

        public void RemoveEntity(string partitionKey, string rowKey)
        {
            _tableClient.DeleteEntity(partitionKey, rowKey);           
        }


        public void InsertBulkData(IEnumerable<TableEntity> items)
        {
            var transactionActions = items.Select(item => new TableTransactionAction(TableTransactionActionType.Add, item));

            _tableClient.SubmitTransaction(transactionActions);
        }


        public void UpdateEntity(string partitionKey, string rowKey, IDictionary<string, string> fields)
        {
            // Use the partition key and row key to get the entity
            TableEntity entity = _tableClient.GetEntity<TableEntity>(partitionKey, rowKey).Value;

            foreach (string fieldName in fields.Keys)
            {
                var value = fields[fieldName];

                if (Double.TryParse(value, out double number))
                    entity[fieldName] = number;
                else
                    entity[fieldName] = value;
            }

            _tableClient.UpdateEntity(entity, ETag.All);
        }

    }
}
