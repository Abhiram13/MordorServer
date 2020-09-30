using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace MordorServer {
     class Update {
          private static bool CollectionsDB(IMongoDatabase mongo, string itemName) {
               IMongoCollection<Item> collection = mongo.GetCollection<Item>("items");
               List<Item> list = collection.Find<Item>(new BsonDocument()).ToList<Item>();
               foreach (Item name in list.ToArray()) {
                    if (name.itemName == itemName) {
                         return true;
                    }
               }
               return false;
          }

          class UpdateReqBody {
               public string itemName { get; set; }
               public string updateName { get; set; }
          }

          public static string UpdateDoc(IMongoDatabase database, HttpListenerContext context) {
               if (context.Request.HttpMethod == "GET") {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return "GET Method is not Acceptable";
               }
               if (context.Request.HasEntityBody == false) {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return "Body is Required";
               }
               StreamReader reader = new StreamReader(context.Request.InputStream);
               UpdateReqBody body = JsonSerializer.Deserialize<UpdateReqBody>(reader.ReadToEnd());
               FilterDefinition<Item> item = Builders<Item>.Filter.Eq("itemName", body.itemName);
               UpdateDefinition<Item> updateItem = Builders<Item>.Update.Set("itemName", body.updateName);
               UpdateResult result = database.GetCollection<Item>("items").UpdateOne(item, updateItem);
               if (result.IsAcknowledged) {
                    FilterDefinition<Item> filteredItem = Builders<Item>.Filter.Eq("itemName", body.updateName);
                    Item collect = database.GetCollection<Item>("items").Find(filteredItem).First();
                    string stg = JsonSerializer.Serialize(collect);
                    return stg;
               }
               return "Nothing Found";
          }
     }
}