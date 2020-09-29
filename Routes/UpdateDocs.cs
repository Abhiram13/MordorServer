using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;
using System.IO;
using System.Text.Json;

namespace MordorServer {
     class Update {
          public static string UpdateDoc(IMongoDatabase database, HttpListenerContext context) {
               StreamReader reader = new StreamReader(context.Request.InputStream);
               ReqBody body = JsonSerializer.Deserialize<ReqBody>(reader.ReadToEnd());
               FilterDefinition<Item> item = Builders<Item>.Filter.Eq("itemName", body.itemName);
               UpdateDefinition<Item> updateItem = Builders<Item>.Update.Set("itemName", "Second Milk");
               UpdateResult result = database.GetCollection<Item>("items").UpdateOne(item, updateItem);
               if (result.IsAcknowledged) {
                    FilterDefinition<Item> filteredItem = Builders<Item>.Filter.Eq("itemName", "Second Milk");
                    Item collect = database.GetCollection<Item>("items").Find(filteredItem).First();
                    string stg = JsonSerializer.Serialize(collect);
                    return stg;
               }
               return "Nothing Found";
          }
     }
}