using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Util;
using Homer.Shared.Configuration;
using Homer.Shared.Entities.Contacts;
using Homer.Shared.Entities.Journal;

namespace Homer.Shared.Services
{
    public interface IDataContext
    {
        Task<TModel> GetByIdAsync<TModel>(string id) where TModel : class, new();
        Task<IEnumerable<TModel>> GetAsync<TModel>(IEnumerable<ScanCondition> conditions = null) where TModel : class, new();

        Task SaveAsync<TModel>(TModel model) where TModel : class, new();

        Task DeleteAsync<TModel>(TModel model) where TModel : class, new();
    }

    public class DynamoDbContext : IDataContext
    {
        private readonly IAmazonDynamoDB _ddbClient;
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IDynamoDBContext _ddbContext;

        public DynamoDbContext(IAmazonDynamoDB ddbClient,
            IConfigurationHelper configurationHelper)
        {
            _ddbClient = ddbClient;
            this._configurationHelper = configurationHelper;
            var config = new Amazon.DynamoDBv2.DataModel.DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
            _ddbContext = new DynamoDBContext(ddbClient, config);

            Initialize();
        }

        private void Initialize()
        {
            const string keyPrefix = "DynamoTables";
            AWSConfigsDynamoDB.Context.TypeMappings[typeof(JournalEntry)] = new TypeMapping(typeof(JournalEntry), _configurationHelper.GetValue("JournalEntryTable", keyPrefix));
            AWSConfigsDynamoDB.Context.TypeMappings[typeof(Address)] = new TypeMapping(typeof(Address), _configurationHelper.GetValue("AddressTable", keyPrefix));
        }

        public async Task<TModel> GetByIdAsync<TModel>(string id) where TModel : class, new()
        {
            return await _ddbContext.LoadAsync<TModel>(id);
        }

        public async Task<IEnumerable<TModel>> GetAsync<TModel>(IEnumerable<ScanCondition> conditions = null) where TModel : class, new()
        {            
            var search = this._ddbContext.ScanAsync<TModel>(conditions);
            var page = await search.GetNextSetAsync();

            return page;
        }

        public async Task SaveAsync<TModel>(TModel model) where TModel : class, new()
        {
            await _ddbContext.SaveAsync(model);
        }

        public async Task DeleteAsync<TModel>(TModel model) where TModel : class, new()
        {
            await _ddbContext.DeleteAsync(model);
        }
    }
}