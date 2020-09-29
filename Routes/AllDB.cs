using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MordorServer {
     public class ListOfDB {
          // returns number of all databases in Item Collection
          public static long getAllDataBase(IMongoDatabase mordor) {
               IMongoCollection<Item> databases = mordor.GetCollection<Item>("items");
               return databases.CountDocuments(new BsonDocument());
          }
     }
}