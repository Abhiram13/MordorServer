using System;
using System.Net;
using System.IO;
using MongoDB.Driver;

namespace MordorServer {
     class Server {
          public static void Start() {
               HttpListener http = new HttpListener();
               http.Prefixes.Add("http://localhost:1995/");
               http.Start();
               Console.WriteLine("Server has Started");
               while (true) {
                    HttpListenerContext context = http.GetContext();
                    MongoClient mongoDB = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Models?retryWrites=true&w=majority");
                    IMongoDatabase mordorDataBase = mongoDB.GetDatabase("Mordor");
                    IMongoCollection<Item> collection = mordorDataBase.GetCollection<Item>("items");
                    switch (context.Request.RawUrl) {
                         case "/":
                              Helper<string>.SendResponse("Here is the Resonse Message", context);
                              Console.WriteLine("Respone Sent");
                              break;
                         case "/count/":
                              if (context.Request.HttpMethod == "POST") {
                                   context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
                              Helper<string>.SendResponse(FindItems.Find(mordorDataBase, context), context);
                              break;
                         case "/Update/":
                              Helper<string>.SendResponse(Update.UpdateDoc(mordorDataBase, context), context);
                              break;
                         case "/login/":
                              Helper<string>.SendResponse(Login.login(mordorDataBase, context), context);
                              Console.WriteLine("Response Sent");
                              break;
                         case "/signin/":
                              System.IO.Stream stream = context.Request.InputStream;
                              System.Text.Encoding encode = context.Request.ContentEncoding;
                              System.IO.StreamReader s = new StreamReader(stream, encode);
                              Helper<string>.SendResponse(SignUp.SignIn(s, mordorDataBase), context);
                              break;
                    }
               }
          }
     }
}