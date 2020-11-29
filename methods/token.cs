using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MordorServer {
   public abstract class Token : String {
      private IMongoCollection<IToken> TokenCollection() {
         return Mongo.database.GetCollection<IToken>("tokens");
      }

      private IToken[] Tokens() {
         IToken[] tokens = this.TokenCollection().Find<IToken>(new BsonDocument()).ToList<IToken>().ToArray();
         return tokens;
      }

      private string Create(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         return Encode($"{username}_{password}_{DateTimeOffset.UtcNow.ToLocalTime().ToString()}");
      }

      private async void Kill(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];

         await System.Threading.Tasks.Task.Delay(20000);

         this.TokenCollection().UpdateOne(
            Builders<IToken>.Filter.Eq("username", username),
            Builders<IToken>.Update.Set("Token", "")
         );
      }

      public virtual IToken Generate(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];

         IToken token = Array.Find<IToken>(this.Tokens(), t => t.username == username);

         if (token == null) {
            this.TokenCollection().InsertOne(new IToken { username = username, password = password, Token = Create(header), _id = ObjectId.GenerateNewId() });
         } else {
            this.TokenCollection().UpdateOne(
               Builders<IToken>.Filter.Eq("username", username),
               Builders<IToken>.Update.Set("Token", Create(header)
            ));
         }

         Kill(header);

         return Array.Find<IToken>(this.Tokens(), t => t.username == username);
      }

      public virtual void FindToken(string token) {
         IToken tokn = Array.Find<IToken>(this.Tokens(), t => t.Token == token);
      }
   }
}