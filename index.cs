using System;
using System.Net.Http;
using System.Net;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MordorServer
{
   class Server
   {
      public static void Start()
      {
         HttpListener http = new HttpListener();
         http.Prefixes.Add("http://localhost:1995/");
         http.Start();
         Console.WriteLine("Server has Started");
         MongoClient mongoDB = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Models?retryWrites=true&w=majority");
         IMongoDatabase mordorDataBase = mongoDB.GetDatabase("Mordor");
         IMongoCollection<BsonDocument> collection = mordorDataBase.GetCollection<BsonDocument>("items");
         Console.WriteLine(collection.CountDocuments(new BsonDocument()));

         while (true)
         {
            try
            {               
               HttpListenerContext context = http.GetContext();
               using (Stream stream = context.Response.OutputStream)
               {
                  using (StreamWriter writer = new StreamWriter(stream))
                  {
                     writer.Write("Here is the Response Message");
                  }
               }
               Console.WriteLine("Respone Sent");
            }
            catch (Exception e)
            {
               Console.WriteLine(e);
            }
         }
      }
   }
}