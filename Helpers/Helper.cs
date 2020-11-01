using System;
using System.Text;

namespace MordorServer {
   public class String {
      public static string Encode(string s) {
         byte[] bytes = Encoding.UTF8.GetBytes(s);
         return Convert.ToBase64String(bytes);
      }

      public static string Decode(string base64String) {
         byte[] bytes = Convert.FromBase64String(base64String);
         return Encoding.UTF8.GetString(bytes);
      }
   }

   class TOKEN {
      private static string Create(string header) {
         string username = header.Split(":")[0];
         string password = header.Split(":")[1];
         string seconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
         string milliSeconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
         string Token = String.Encode($"{username}_{password}_{seconds}_{milliSeconds}");
         return Token;
      }

      public static void Generate() {
         //
		}
	}
}