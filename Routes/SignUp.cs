using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Collections.Generic;
using System.Net;

namespace MordorServer {
   class NewUser {
      public string username { get; set; }
      public string firstname { get; set; }
      public string lastname { get; set; }
      public bool isAdmin { get; set; } = false;
      public string password { get; set; }
   }

   class SignUp {
      private static User[] DeserializeUserJSON(IMongoCollection<User> collection) {
         List<User> userCollection = collection.Find<User>(new BsonDocument()).ToList<User>();
         return userCollection.ToArray();
      }

      public static bool FindUser(IMongoCollection<User> collection, string value) {
         User[] Users = DeserializeUserJSON(collection);
         for (int i = 0; i < Users.Length; i++) {
            if (value == Users[i].username) {
               return true;
            }
         }
         return false;
      }

      public static string SignIn(StreamReader requestBody, IMongoDatabase db, HttpListenerContext context) {
         NewUser newUser = JsonSerializer.Deserialize<NewUser>(requestBody.ReadToEnd());
         IMongoCollection<NewUser> collection = db.GetCollection<NewUser>("users");
         IMongoCollection<User> userDB = db.GetCollection<User>("users");
         if (FindUser(userDB, newUser.username)) {
            context.Response.StatusCode = (int)HttpStatusCode.Found;
            return "User already Exists";
         }
         NewUser createUser = new NewUser {
            username = newUser.username,
            firstname = newUser.firstname,
            lastname = newUser.lastname,
            password = newUser.password,
            isAdmin = newUser.isAdmin
         };
         collection.InsertOne(createUser);
         context.Response.StatusCode = (int)HttpStatusCode.Created;
         return "User Created";
      }
   }
}