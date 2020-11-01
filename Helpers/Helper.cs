using System;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MordorServer
{
	public class String
	{
		public static string Encode(string s) {
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			return Convert.ToBase64String(bytes);
		}

		public static string Decode(string base64String) {
			byte[] bytes = Convert.FromBase64String(base64String);
			return Encoding.UTF8.GetString(bytes);
		}
	}

	public class Tokn
	{
		public ObjectId? _id { get; set; }
		public string? username { get; set; }
		public string? password { get; set; }
		public string? Token { get; set; }
	}

	public class TOKEN
	{
		private static string Create(string header) {
			string username = header.Split(":")[0];
			string password = header.Split(":")[1];
			string seconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
			string milliSeconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
			string Token = String.Encode($"{username}_{password}_{seconds}_{milliSeconds}");
			return Token;
		}

		public static void Generate(string header) {
			string username = header.Split(":")[0];
			string password = header.Split(":")[1];
			Tokn token = null;

			FilterDefinition<Tokn> filter = Builders<Tokn>.Filter.Eq("username", username);
			string s = Mordor.DB.GetCollection<Tokn>("tokens").Find(filter).ToJson();
			Console.WriteLine(s);
		}
	}
}