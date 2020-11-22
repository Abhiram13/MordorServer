using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

namespace MordorServer {
   public class Collection<T> {
      public static string fetchAll(HttpListenerContext context, string collection_name) {
         IMongoCollection<T> collection = Mongo.database.GetCollection<T>(collection_name);

         T[] array = collection.Find<T>(new BsonDocument()).ToList<T>().ToArray();

         return JsonSerializer.Serialize<T[]>(array);
      }
   }
}