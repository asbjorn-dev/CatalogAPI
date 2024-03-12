using Catalog.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public class MongoDBVareRepository : IVareRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "varene";
        private readonly IMongoCollection<Vare> vareCollection;
        // injection af mongoDB vha. nuget package i klasse constructoren
        public MongoDBVareRepository(IMongoClient mongoClient) 
        {
            //reference til mongodb.
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            // reference til collection.
            vareCollection = database.GetCollection<Vare>(collectionName);
        }
        public async Task CreateVareAsync(Vare vare)
        {
            await vareCollection.InsertOneAsync(vare);
        }

        public async Task DeleteVareAsync(Guid id)
        {
            // note: skal laves om til BsonDocument da DeleteOne() kun kan fjerne 1 dokument og ik objekt
            var VareDerSkalSlettes = vareCollection.Find(vare => vare.Id == id).FirstOrDefault().ToBsonDocument();
            await vareCollection.DeleteOneAsync(VareDerSkalSlettes);
        }

        public async Task<Vare> GetEnkeltVareAsync(Guid id)
        {
             return await vareCollection.Find(vare => vare.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vare>> GetVareAsync()
        {
            return await vareCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateVareAsync(Vare vare)
        {
          // Opret et filter baseret på varens Id. Bruger Builder fra mongodb biblio. Eq står for equals og matcher id'erne med dem man taster ind fra parameteren.
          var filter = Builders<Vare>.Filter.Eq(x => x.Id, vare.Id);
          // Laver en opdatering baseret på de nye værdier af varen og "replacer" dem.
          var update = Builders<Vare>.Update
             .Set(x => x.Name, vare.Name)
             .Set(x => x.Price, vare.Price);

            // erstatter den gamle med det nye man har valgt (price, name).
           await vareCollection.ReplaceOneAsync(filter, vare);
        }
    }
}