using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MordorServer {
   public abstract class Token : String {
      private string CreateToken(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         return Encode($"{username}_{password}_{DateTimeOffset.UtcNow.ToLocalTime().ToString()}");
      }

      public virtual void Generate(string header) {
         IMongoCollection<IToken> tokenCollection = Mongo.database.GetCollection<IToken>("tokens");
         IToken[] tokens = tokenCollection.Find<IToken>(new BsonDocument()).ToList<IToken>().ToArray();
         IToken token = null;
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];

         for (int i = 0; i < tokens.Length; i++) {
            if (tokens[i].username == username) {
               token = tokens[i];
            }
         }

         // token will be null, when no user is found
         // then new token object will be created in Database
         // else if user is found, token value will be updated
         if (token == null) {
            tokenCollection.InsertOne(new IToken { username = username, password = password, Token = CreateToken(header), _id = ObjectId.GenerateNewId() });
         } else {
            Console.WriteLine(username);
            Console.WriteLine(CreateToken(header));
            var filter = Builders<IToken>.Filter.Eq("username", username);
            var update = Builders<IToken>.Update.Set("Token", CreateToken(header));
            UpdateResult x = tokenCollection.UpdateOne(filter, update);
            Console.WriteLine(x.ModifiedCount);
         }
      }
   }
}