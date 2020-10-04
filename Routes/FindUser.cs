using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace MordorServer {
     class RequestBody {
          public string username { get; set; }
     }

     class FindUser {
          private static bool CollectionsDB(IMongoDatabase mongo, string username) {
               IMongoCollection<User> collection = mongo.GetCollection<User>("users");
               List<User> list = collection.Find<User>(new BsonDocument()).ToList<User>();
               foreach (User name in list.ToArray()) {
                    if (name.username == username) {
                         return true;
                    }
               }
               return false;
          }

          public static string Find(IMongoDatabase mongodb, HttpListenerContext context) {
               if (context.Request.HttpMethod == "GET") {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    return "GET Method is not Acceptable";
               }
               if (context.Request.HasEntityBody == false) {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return "Body is Required";
               }
               StreamReader reader = new StreamReader(context.Request.InputStream);
               RequestBody body = JsonSerializer.Deserialize<RequestBody>(reader.ReadToEnd());
               if (CollectionsDB(mongodb, body.username)) {
                    FilterDefinition<User> item = Builders<User>.Filter.Eq("username", body.username);
                    User collection = mongodb.GetCollection<User>("users").Find(item).First();
                    string str = JsonSerializer.Serialize(collection);
                    return str;
               }
               context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
               return "Item not Found";
          }
     }
}