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
         while (true)
         {
            MongoClient mongoDB = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Models?retryWrites=true&w=majority");
            IMongoDatabase mordorDataBase = mongoDB.GetDatabase("Mordor");
            IMongoCollection<BsonDocument> collection = mordorDataBase.GetCollection<BsonDocument>("items");
            try
            {
               HttpListenerContext context = http.GetContext();               
               switch (context.Request.RawUrl)
               {
                  case "/":
                     using (Stream stream = context.Response.OutputStream)
                     {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                           writer.Write("Here is the Response Message");
                        }
                     }
                     Console.WriteLine("Respone Sent");
                     break;
                  case "/count/":
                     if (context.Request.HttpMethod == "POST")
                     {
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        using (Stream str = context.Response.OutputStream)
                        {
                           using (StreamWriter writer = new StreamWriter(str))
                           {
                              writer.Write("The Method is wrong");
                           }
                        }
                        http.Stop();
                        break;
                     }
                     using (Stream stream = context.Response.OutputStream)
                     {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                           writer.Write(ListOfDB.getAllDataBase(mordorDataBase));
                        }
                     }
                     Console.WriteLine("Count has Sent");
                     break;
                  case "/allItems/":
                     using (Stream stream = context.Response.OutputStream)
                     {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                           writer.Write(GetItems.AllItem(mordorDataBase));
                        }
                     }
                     Console.WriteLine("Items has Sent");
                     break;
               }
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
         }
      }
   }
}