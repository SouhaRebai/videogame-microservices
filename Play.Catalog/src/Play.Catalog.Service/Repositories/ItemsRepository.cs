using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollection;
        //build the filters to query for items in MongoDB
        private readonly FilterDefinitionBuilder<Item> filterBuider = Builders<Item>.Filter;
        public ItemsRepository()
        {
            //now we declare the constructor of this repository 
            //we need to start by using a mongo client to connect to the database 
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(collectionName);
        }
        // return all the items in the database
        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuider.Empty).ToListAsync();
        }
        // return a searched item
        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuider.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        // create an item in the database
        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }
        //update an existing item in the database 
        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<Item> filter = filterBuider.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        //delete from database
        public async Task DeleteAsync(Item entity)
        {
            FilterDefinition<Item> filter = filterBuider.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}