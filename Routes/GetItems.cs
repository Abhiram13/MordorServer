using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text.Json;

namespace MordorServer {
     class Item {
          public ObjectId _id { get; set; }
          public string itemName { get; set; }
          public string description { get; set; }
          public string category { get; set; }
          public string imageURL { get; set; }
          public string categoryLogo { get; set; }
          public int rating { get; set; }
          public int __v { get; set; }
     }

     class GetItems {
          public static string AllItem(IMongoDatabase mongodb) {
               IMongoCollection<Item> collection = mongodb.GetCollection<Item>("items");
               List<Item> list = collection.Find<Item>(new BsonDocument()).ToList<Item>();
               string strList = JsonSerializer.Serialize(list);
               return strList;
          }
     }
}