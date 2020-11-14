using MongoDB.Driver;
using MongoDB.Bson;

namespace Types {
   class Item {
      public ObjectId _id { get; }
      public string itemName { get; set; }
      public string description { get; set; }
      public string category { get; set; }
      public string imageURL { get; set; }
      public string categoryLogo { get; set; }
      public int rating { get; set; }
      public int __v { get; }
   }
}