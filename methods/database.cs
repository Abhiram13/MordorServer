using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

namespace MordorServer {
   public class Collection<T> {
      IMongoCollection<T> collection;
      T[] arrayOfCollection;

      public Collection(string collection_name) {
         collection = Mongo.database.GetCollection<T>(collection_name);
         arrayOfCollection = this.collection.Find<T>(new BsonDocument()).ToList<T>().ToArray();
      }

      public string fetchAll(HttpListenerContext context) {
         return JsonSerializer.Serialize<T[]>(this.arrayOfCollection);
      }

      public T find(HttpListenerContext context, Predicate<T> func) {
         T item = Array.Find<T>(this.arrayOfCollection, func);
         return item;
      }
   }
}