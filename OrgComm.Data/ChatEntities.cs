using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OrgComm.Data
{
    public class ChatEntities : IDisposable
    {

        // Flag: Has Dispose already been called? 
        private bool _disposed = false;

        public ChatEntities()
        {

        }

        public void test()
        {
            IMongoClient _client = new MongoClient("mongodb://192.168.1.108:27017/test");
            IMongoDatabase _database = _client.GetDatabase("test");

            var document = new BsonDocument
            {
                { "address" , new BsonDocument
                    {
                        { "street", "2 Avenue" },
                        { "zipcode", "10075" },
                        { "building", "1480" },
                        { "coord", new BsonArray { 73.9557413, 40.7720266 } }
                    }
                },
                { "borough", "Manhattan" },
                { "cuisine", "Italian" },
                { "grades", new BsonArray
                    {
                        new BsonDocument
                        {
                            { "date", new DateTime(2014, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
                            { "grade", "A" },
                            { "score", 11 }
                        },
                        new BsonDocument
                        {
                            { "date", new DateTime(2014, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
                            { "grade", "B" },
                            { "score", 17 }
                        }
                    }
                },
                { "name", "Vella" },
                { "restaurant_id", "41704620" }
            };

            var collection = _database.GetCollection<BsonDocument>("restuarants");

            collection.InsertOneAsync(document).Wait();

            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = collection.FindAsync(filter).Result)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    var batch = cursor.Current;
                    foreach (var doc in batch)
                    {
                        // process document
                        count++;
                    }
                }
            }

            string c = count.ToString();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ChatEntities()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free managed resources
            }

            // free native resources if there are any.

            _disposed = true;
        }
    }
}
