using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
namespace Play.Catalog.Service.Repositories
{

    public class MongoRepository<T> : IRepository<T> where T: IEntity
    {
        private readonly IMongoCollection<T> dbCollection;
        //build the filters to query for items in MongoDB
        private readonly FilterDefinitionBuilder<T> filterBuider = Builders<T>.Filter;
        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            //now we declare the constructor of this repository 
            //we need to start by using a mongo client to connect to the database 
            dbCollection = database.GetCollection<T>(collectionName);
        }
        // return all the items in the database
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuider.Empty).ToListAsync();
        }
        // return a searched item
        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuider.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        // create an item in the database
        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }
        //update an existing item in the database 
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<T> filter = filterBuider.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        //delete from database
        public async Task DeleteAsync(T entity)
        {
            FilterDefinition<T> filter = filterBuider.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}