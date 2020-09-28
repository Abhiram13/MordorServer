using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;
using System.IO;

namespace MordorServer
{
   class NewUser
   {
      public string username { get; set; }
      public string firstname { get; set; }
      public string lastname { get; set; }
      public string isAdmin { get; set; }
      public string password { get; set; }
   }

   class SignUp
   {
      public static string SignIn(StreamReader requestBody, IMongoDatabase db)
      {
         NewUser newUser = JsonSerializer.Deserialize<NewUser>(requestBody.ReadToEnd());
         BsonDocument doc = new BsonDocument
         {
            {"username", newUser.username},
            {"firstname", newUser.firstname},
            {"lastname", newUser.lastname},
            {"isAdmin", newUser.isAdmin},
            {"password", newUser.password}
         };

         IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("users");
         collection.InsertOne(doc);
         return "User Created";
      }
   }
}