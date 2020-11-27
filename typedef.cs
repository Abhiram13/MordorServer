using MongoDB.Bson;

namespace MordorServer {   
   public class Item {
      public ObjectId? _id { get; set; }
      public string itemName { get; set; }
      public string description { get; set; }
      public string category { get; set; }
      public string imageURL { get; set; }
      public string categoryLogo { get; set; }
      public int rating { get; set; }
      public int? __v { get; set; }
   }

   public class LoginRequest {
      public string username { get; set; }
      public string password { get; set; }
   }

   public class IToken {
      public ObjectId? _id { get; set; }
      public string password { get; set; }
      public string username { get; set; }
      public string Token { get; set; }
   }

   public class IUser {
      public ObjectId? _id { get; set; }
      public string username { get; set; }
      public string firstname { get; set; }
      public string lastname { get; set; }
      public string password { get; set; }
      public bool isAdmin { get; set; }
      public int? __v { get; set; }
   }

   public class ILogin {
      public IUser user { get; set; }
      public string token { get; set; }
   }
}