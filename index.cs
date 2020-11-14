using System;
using Types;
using MongoDB.Driver;

namespace Server {
   public class Mongo {
      private static MongoClient client = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Mordor?retryWrites=true&w=majority");
      public static IMongoDatabase database = client.GetDatabase("Mordor");
      static IMongoCollection<Item> items = database.GetCollection<Item>("items");
   }
}