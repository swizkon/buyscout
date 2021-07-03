using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BuyScout.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BuyScout.Common.Persistence
{
    public class MongoRepository : IRepository
    {
        private readonly DatabaseConfiguration _databaseConfiguration;
        private readonly MongoClient _client;

        public MongoRepository(IOptions<DatabaseConfiguration> databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration.Value;
            _client = new MongoClient(databaseConfiguration.Value.ConnectionString);
        }

        public async Task<T> StoreAsync<T>(T entity) where T : class
        {
            var collection = GetCollection<T>();
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> filterPredicate) where T : class
        {
            var collection = GetCollection<T>();
            var result = await collection.FindAsync(filterPredicate);
            return result.ToList();
        }

        public async Task<T> FirstAsync<T>(Expression<Func<T, bool>> filterPredicate) where T : class 
            => (await QueryAsync(filterPredicate)).FirstOrDefault();

        public async Task<IEnumerable<T>> RemoveAsync<T>(Expression<Func<T, bool>> filterPredicate) where T : class
        {
            var result = await QueryAsync(filterPredicate);
            await GetCollection<T>().DeleteManyAsync(filterPredicate);

            return result;
        }

        private IMongoCollection<T> GetCollection<T>() =>
            _client.GetDatabase(_databaseConfiguration.DatabaseName)
                .GetCollection<T>($"{typeof(T).Name}Collection");
    }
}