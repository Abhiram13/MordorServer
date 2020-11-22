using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MordorServer {
   public abstract class String {
      public virtual string Encode(string str) {
         var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
         return System.Convert.ToBase64String(plainTextBytes);
      }

      public virtual string Decode(string str) {
         var base64EncodedBytes = System.Convert.FromBase64String(str);
         return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
      }
   }

   public abstract class Token : String {
      private static List<IToken> tokenList = Mongo.database.GetCollection<IToken>("tokens").Find<IToken>(new BsonDocument()).ToList<IToken>();
      private static IToken[] tokens = tokenList.ToArray();
      private static string s = DateTimeOffset.UtcNow.ToLocalTime().ToString();
      
      public virtual string CreateToken(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         return Encode($"{username}_{password}_{s}");
      }
   }

   public class Auth {
      public static void Headers(HttpListenerRequest request) {
         foreach (string key in request.Headers.AllKeys) {
            string[] values = request.Headers.GetValues(key);

            foreach (string value in values) {
               if (key == "Token") {}
            }
         }
      }
   }
}