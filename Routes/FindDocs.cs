using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Text.Json;
using System.IO;

namespace MordorServer
{
   class ReqBody
   {
      public string itemName { get; set; }
   }
   class FindItems
   {
      public static string Find(IMongoDatabase mongodb, HttpListenerContext context)
      {
         StreamReader reader = new StreamReader(context.Request.InputStream);
         ReqBody body = JsonSerializer.Deserialize<ReqBody>(reader.ReadToEnd());
         FilterDefinition<Item> item = Builders<Item>.Filter.Eq("itemName", body.itemName);
         Item collection = mongodb.GetCollection<Item>("items").Find(item).First();
         string str = JsonSerializer.Serialize(collection);
         return str;
      }
   }
}