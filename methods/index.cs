using System;
using MongoDB.Driver;
using System.Net;
using System.IO;

namespace MordorServer
{
   public class Mongo
   {
      private static string url = $"mongodb+srv://{Config.username}:{Config.password}@myfirstdatabase.l8kvg.mongodb.net/{Config.cluster}?retryWrites=true&w=majority";
      private static MongoClient client = new MongoClient(url);
      public static IMongoDatabase database = client.GetDatabase(Config.dbname);
   }

   public class Server
   {
      public static HttpListener http = new HttpListener();
      public static void start()
      {
         http.Prefixes.Add("http://localhost:1995/");
         http.Start();
         Console.WriteLine("Server had Started");
         while (true)
         {
            HttpListenerContext context = http.GetContext();
            switch (context.Request.RawUrl)
            {
               case "/":
                  new Response<string>(context).Send("Hello World");
                  break;
               case "/login":
                  User.Login(context);
                  break;
               case "/signin":
                  User.SignIn(context);
                  break;
               case "/another":
                  new Response<string>(context).Send(
                     new Collection<IUser>("users").fetchAll(context)
                  );
                  break;
            }
         }
      }
   }
}