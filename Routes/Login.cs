using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace MordorServer
{

   class User
   {
      public MongoDB.Bson.ObjectId _id { get; set; }
      public string username { get; set; }
      public string firstname { get; set; }
      public string lastname { get; set; }
      public string password { get; set; }
      public bool isAdmin { get; set; }
      public int __v { get; set; }
   }

   class LoginCredentials
   {
      public string username { get; set; }
   }

   class Login
   {
      private User[] DeserializeJSON(IMongoCollection<User> collection)
      {
         List<User> userCollection = collection.Find<User>(new BsonDocument()).ToList<User>();
         return userCollection.ToArray();
      }

      private void FindUser(IMongoCollection<User> collection)
      {
         User[] Users = DeserializeJSON(collection);
         for (int i = 0; i < Users.Length; i++)
         {
            Console.WriteLine(Users[i].username);
         }
      }

      public static string login(IMongoDatabase database, HttpListenerContext context)
      {
         try
         {
            IMongoCollection<User> userCollection = database.GetCollection<User>("users");
            new Login().FindUser(userCollection);
            StreamReader reader = new StreamReader(context.Request.InputStream);
            LoginCredentials body = JsonSerializer.Deserialize<LoginCredentials>(reader.ReadToEnd());
            FilterDefinition<User> item = Builders<User>.Filter.Eq("username", body.username);                        
            User collection = database.GetCollection<User>("users").Find(item).First();
            string str = JsonSerializer.Serialize(collection);
            return str;
         }
         catch (Exception e)
         {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            context.Response.StatusDescription = e.Message;
            Console.WriteLine("Here is the Error Message");
            Console.WriteLine(e.Message);
            return e.Message;
         }       
      }
   }
}