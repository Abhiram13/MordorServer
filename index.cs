using System;
using System.Net;
using System.IO;
using MongoDB.Driver;

namespace MordorServer
{
	public class Mordor
	{
		public static HttpListener http = new HttpListener();
		public static MongoClient Mongo = new MongoClient("mongodb+srv://abhiramDB:abhiram13@myfirstdatabase.l8kvg.mongodb.net/Models?retryWrites=true&w=majority");
		public static IMongoDatabase DB = Mongo.GetDatabase("Mordor");
		static IMongoCollection<Item> Items = DB.GetCollection<Item>("items");
		static IMongoCollection<User> Users = DB.GetCollection<User>("items");
	}

	class Server
	{
		public static void Start() {
			Mordor.http.Prefixes.Add("http://localhost:1995/");
			Mordor.http.Start();
			Console.WriteLine("Server have been Started");
			while (true) {
				HttpListenerContext context = Mordor.http.GetContext();
				switch (context.Request.RawUrl) {
					case "/":
						Helper<string>.SendResponse("Here is the Resonse Message", context);
						Console.WriteLine("Respone Sent");
						break;
					case "/count/":
						Helper<long>.SendResponse(ListOfDB.getAllDataBase(Mordor.DB), context);
						Console.WriteLine("Count has Sent");
						break;
					case "/getItems/":
						Helper<string>.SendResponse(GetItems.AllItem(Mordor.DB), context);
						Console.WriteLine("Items has Sent");
						break;
					case "/Find/":
						Helper<string>.SendResponse(FindItems.Find(Mordor.DB, context), context);
						break;
					case "/FindUser/":
						Helper<string>.SendResponse(FindUser.Find(Mordor.DB, context), context);
						break;
					case "/Update/":
						Helper<string>.SendResponse(Update.UpdateDoc(Mordor.DB, context), context);
						break;
					case "/login/":
						Helper<string>.SendResponse(Login.login(Mordor.DB, context), context);
						Console.WriteLine("Response Sent");
						break;
					case "/signin/":
						System.IO.Stream stream = context.Request.InputStream;
						System.Text.Encoding encode = context.Request.ContentEncoding;
						System.IO.StreamReader s = new StreamReader(stream, encode);
						Helper<string>.SendResponse(SignUp.SignIn(s, Mordor.DB, context), context);
						break;
					case "/second/":
						Console.WriteLine(context.Request.Headers);
						//TOKEN.Generate(context.Request.Headers.auth!);
						break;
				}
			}
		}
	}
}