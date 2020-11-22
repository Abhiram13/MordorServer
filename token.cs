using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MordorServer {
   public abstract class Token : String {
      private static IMongoCollection<IToken> tokenCollection = Mongo.database.GetCollection<IToken>("tokens");
      private static List<IToken> tokenList = tokenCollection.Find<IToken>(new BsonDocument()).ToList<IToken>();
      private static IToken[] tokens = tokenList.ToArray();
      private static string s = DateTimeOffset.UtcNow.ToLocalTime().ToString();

      private string CreateToken(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         return Encode($"{username}_{password}_{s}");
      }

      public virtual string Generate(string header) {
         IToken token = null;
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];

         for (int i = 0; i < tokens.Length; i++) {
            if (tokens[i].username == username) {
               token = tokens[i];
            }
         }

         return token.username;
      }
   }
}