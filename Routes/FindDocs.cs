using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace MordorServer {
     class ReqBody {
          public string itemName { get; set; }
     }

     class FindItems {
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

          public static string Find(IMongoDatabase mongodb, HttpListenerContext context) {
               if (context.Request.HttpMethod == "GET") {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return "GET Method is not Acceptable";
               }
               if (context.Request.HasEntityBody == false) {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return "Body is Required";
               }
               StreamReader reader = new StreamReader(context.Request.InputStream);               
               ReqBody body = JsonSerializer.Deserialize<ReqBody>(reader.ReadToEnd());
               if (CollectionsDB(mongodb, body.itemName)) {
                    FilterDefinition<Item> item = Builders<Item>.Filter.Eq("itemName", body.itemName);
                    Item collection = mongodb.GetCollection<Item>("items").Find(item).First();
                    string str = JsonSerializer.Serialize(collection);
                    return str;
               }
               context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
               return "Item not Found";
          }
     }
}