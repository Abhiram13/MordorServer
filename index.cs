using System;
using Types;
using MongoDB.Driver;
using System.Net;
using System.IO;

namespace MordorServer {
   class Mongo {
      private static MongoClient client = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Mordor?retryWrites=true&w=majority");
      public static IMongoDatabase database = client.GetDatabase("Mordor");
      static IMongoCollection<Item> items = database.GetCollection<Item>("items");
      static IMongoCollection<IToken> token = database.GetCollection<IToken>("tokens");
   }

   public class Server {
      public static void start() {
         HttpListener http = new HttpListener();
         http.Prefixes.Add("http://localhost:1995/");
         http.Start();
         Console.WriteLine("Server had Started");         
         while (true) {
            HttpListenerContext context = http.GetContext();            
            switch (context.Request.RawUrl) {
               case "/":
                  using (Stream stream = context.Response.OutputStream) {
                     using (StreamWriter writer = new StreamWriter(stream)) {
                        writer.Write("Hello World");
                     }
                  }
                  break;
               case "/demo":
                  using (Stream stream = context.Response.OutputStream) {
                     using (StreamWriter writer = new StreamWriter(stream)) {
                        writer.Write("Hello Demo");
                     }
                  }
                  break;
            }
         }         
      }
   }
}