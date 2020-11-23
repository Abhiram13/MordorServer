using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MordorServer {
   public abstract class Token : String {
      private string Create(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         return Encode($"{username}_{password}_{DateTimeOffset.UtcNow.ToLocalTime().ToString()}");
      }

      private async void Kill(string header) {
         IMongoCollection<IToken> tokenCollection = Mongo.database.GetCollection<IToken>("tokens");
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         var filter = Builders<IToken>.Filter.Eq("username", username);
         var update = Builders<IToken>.Update.Set("Token", "");

         await System.Threading.Tasks.Task.Delay(20000);

         tokenCollection.UpdateOne(filter, update);
      }

      public virtual void Generate(string header) {
         IMongoCollection<IToken> tokenCollection = Mongo.database.GetCollection<IToken>("tokens");
         IToken[] tokens = tokenCollection.Find<IToken>(new BsonDocument()).ToList<IToken>().ToArray();
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];

         IToken token = Array.Find<IToken>(tokens, t => t.username == username);
         
         if (token == null) {
            tokenCollection.InsertOne(new IToken { username = username, password = password, Token = Create(header), _id = ObjectId.GenerateNewId() });
         } else {
            var filter = Builders<IToken>.Filter.Eq("username", username);
            var update = Builders<IToken>.Update.Set("Token", Create(header));
            UpdateResult x = tokenCollection.UpdateOne(filter, update);
         }

         Kill(header);
      }
   }
}