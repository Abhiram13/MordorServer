using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace MordorServer {
     class User {
          public MongoDB.Bson.ObjectId _id { get; set; }
          public string username { get; set; }
          public string firstname { get; set; }
          public string lastname { get; set; }
          public string password { get; set; }
          public bool isAdmin { get; set; }
          public int __v { get; set; }
     }

     class LoginCredentials {
          public string username { get; set; }
          public string password { get; set; }
     }

     class Login {
          private static User[] DeserializeJSON(IMongoCollection<User> collection) {
               List<User> userCollection = collection.Find<User>(new BsonDocument()).ToList<User>();
               return userCollection.ToArray();
          }

          private static bool checkUser(IMongoCollection<User> collection, string password) {
               User[] Users = DeserializeJSON(collection);
               for (int i = 0; i < Users.Length; i++) {
                    if (password == Users[i].password) {
                         return true;
                    }
               }
               return false;
          }

          private static bool FindUser(IMongoCollection<User> collection, string username, string password) {
               User[] Users = DeserializeJSON(collection);
               for (int i = 0; i < Users.Length; i++) {
                    if (username == Users[i].username && password == Users[i].password) {                         
                         return true;
                    }
               }
               return false;
          }

          public static string login(IMongoDatabase database, HttpListenerContext context) {
               try {
                    IMongoCollection<User> userCollection = database.GetCollection<User>("users");
                    StreamReader reader = new StreamReader(context.Request.InputStream);
                    LoginCredentials body = JsonSerializer.Deserialize<LoginCredentials>(reader.ReadToEnd());

                    if (checkUser(userCollection, body.password) == false) {
                         context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                         return "Wrong Password";
                    }

                    if (FindUser(userCollection, body.username, body.password)) {
                         FilterDefinition<User> item = Builders<User>.Filter.Eq("username", body.username);
                         User collection = database.GetCollection<User>("users").Find(item).First();
                         context.Response.StatusCode = (int)HttpStatusCode.OK;
                         return JsonSerializer.Serialize(collection);
                    }
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return "User Not Found";
               } catch (Exception e) {
                    Console.WriteLine("Here is the Error Message");
                    Console.WriteLine(e.Message);
                    return e.Message;
               }
          }
     }
}