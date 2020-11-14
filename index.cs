using System;
using Types;
using MongoDB.Driver;
using System.Net;

namespace MordorServer {
   class Mongo {
      private static MongoClient client = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Mordor?retryWrites=true&w=majority");
      public static IMongoDatabase database = client.GetDatabase("Mordor");
      static IMongoCollection<Item> items = database.GetCollection<Item>("items");
   }

   public class HTTP {
      public static HttpListenerContext context() {
         HttpListener http = new HttpListener();
         http.Prefixes.Add("http://localhost:1995/");
         http.Start();
         Console.WriteLine("Server had Started");
         return http.GetContext();
      }
   }

   public class Server {
      public static void start() {
         HTTP.context();
      }
   }
}