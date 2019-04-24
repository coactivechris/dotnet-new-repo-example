using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Util;
using Microsoft.Extensions.Logging;
using Repositories.XRay;
using DynamoDBContextConfig = Amazon.DynamoDBv2.DataModel.DynamoDBContextConfig;

namespace DefaultNamespace.Repositories.ModelName
{
    public class ModelNameRepository : IModelNameRepository
    {
        private readonly IDynamoDBContext _ddbContext;
        private readonly IXRayManager _xRayManager;
        private readonly ILogger<ModelNameRepository> _logger;

        public ModelNameRepository(
            IAmazonDynamoDB dynamoDb,
            IXRayManager xRayManager,
            ILogger<ModelNameRepository> logger)
        {
            _logger = logger;
            _xRayManager = xRayManager;
            var tableNameEnvironmentVariableLookup = typeof(Models.ModelName).Name + "Table";
            var tableName = Environment.GetEnvironmentVariable(tableNameEnvironmentVariableLookup);
            if (!string.IsNullOrEmpty(tableName))
                AWSConfigsDynamoDB.Context.TypeMappings[typeof(Models.ModelName)] = new TypeMapping(typeof(Models.ModelName), tableName);

            var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
            var client = dynamoDb;
            _ddbContext = new DynamoDBContext(client, config);
        }

        public async Task<Models.ModelName> SelectAsync(dynamic id)
        {
            try
            {
                _xRayManager.BeginXRaySegment($"{typeof(Models.ModelName).Name}-GetModel");
                _logger.LogDebug($"Getting model {typeof(Models.ModelName).Name} {id}");
                var model = await _ddbContext.LoadAsync<Models.ModelName>((string)id);
                _logger.LogDebug($"Found model: {model != null}");
                return model;
            }
            finally
            {
                _xRayManager.EndXRaySegment();
            }
        }


        public async Task UpdateAsync(Models.ModelName model)
        {
            try
            {
                _xRayManager.BeginXRaySegment($"{typeof(Models.ModelName).Name}-UpdateModel");
                _logger.LogDebug($"Updating {typeof(Models.ModelName).Name} with primaryKeyName {model.Id}");
                await _ddbContext.SaveAsync(model);
            }
            finally
            {
                _xRayManager.EndXRaySegment();
            }
        }

        public async Task<Models.ModelName> InsertAsync(Models.ModelName model)
        {

            _logger.LogDebug($"Saving {typeof(Models.ModelName).Name} with primaryKeyName {model.Id}");
            try
            {
                _xRayManager.BeginXRaySegment($"{typeof(Models.ModelName).Name}-AddModel");
                await _ddbContext.SaveAsync(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _xRayManager.EndXRaySegment();
            }
            var regrabbedModel = await _ddbContext.LoadAsync<Models.ModelName>(model.Id);
            return regrabbedModel;
        }

        public async Task DeleteAsync(dynamic primaryKeyName)
        {
            try
            {
                _xRayManager.BeginXRaySegment($"{typeof(Models.ModelName).Name}-DeleteModel");
                _logger.LogDebug($"Deleting blog with primaryKeyName {primaryKeyName}");
                await _ddbContext.DeleteAsync<Models.ModelName>((string)primaryKeyName);
            }
            finally
            {
                _xRayManager.EndXRaySegment();
            }
        }

    }
}