using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MordorServer
{
   public class ListOfDB
   {
      // returns number of all databases in Item Collection
      public static long getAllDataBase(IMongoDatabase mordor)
      {
         IMongoCollection<BsonDocument> databases = mordor.GetCollection<BsonDocument>("items");
         return databases.CountDocuments(new BsonDocument());
      }
   }
}