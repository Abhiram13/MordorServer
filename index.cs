using System;
using MongoDB.Driver;
using System.Net;
using System.IO;

namespace MordorServer {
   public class Mongo {
      private static MongoClient client = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Mordor?retryWrites=true&w=majority");
      public static IMongoDatabase database = client.GetDatabase("Mordor");
   }

   public class Server {
      public static HttpListener http = new HttpListener();
      public static void start() {
         http.Prefixes.Add("http://localhost:1995/");
         http.Start();
         Console.WriteLine("Server had Started");
         while (true) {
            HttpListenerContext context = http.GetContext();
            switch (context.Request.RawUrl) {
               case "/":
                  new Response<string>(context).Send("Hello World, this is Class Constructor");
                  break;
               case "/demo":
                  Console.WriteLine(new Auth().Headers(context.Request));
                  new Response<string>(context).Send(
                     Collection<Item>.fetchAll(context, "items")
                  );
                  break;
               case "/another":
                  new Response<string>(context).Send(
                     Collection<IUser>.fetchAll(context, "users")
                  );
                  break;
            }
         }
      }
   }
}