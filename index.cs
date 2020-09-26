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
                     Helper<string>.SendResponse("Here is the Resonse Message", context);
                     Console.WriteLine("Respone Sent");
                     break;
                  case "/count/":
                     if (context.Request.HttpMethod == "POST")
                     {
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        Helper<string>.SendResponse("The Method is Wrong", context);
                        http.Stop();
                        break;
                     }
                     Helper<long>.SendResponse(ListOfDB.getAllDataBase(mordorDataBase), context);
                     Console.WriteLine("Count has Sent");
                     break;
                  case "/getItems/":
                     Helper<string>.SendResponse(GetItems.AllItem(mordorDataBase), context);
                     Console.WriteLine("Items has Sent");
                     break;
                  case "/Find/":
                     FindItems.Find(mordorDataBase, context);
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