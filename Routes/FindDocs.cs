using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Text.Json;

namespace MordorServer
{
   class FindItems
   {
      public static void Find(IMongoDatabase mongodb, HttpListenerContext context)
      {
         FilterDefinition<Item> item = Builders<Item>.Filter.Eq("itemName", "Blue Milk");
         Item collection = mongodb.GetCollection<Item>("items").Find(item).First();
         string str = JsonSerializer.Serialize(collection);
         Console.WriteLine(str);
      }
   }
}