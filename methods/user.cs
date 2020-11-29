using System;
using System.Net;
using System.Text.Json;

namespace MordorServer {
   class User {
      private static IUser FetchDetails(HttpListenerContext context) {
         System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.InputStream);
         LoginRequest body = JsonSerializer.Deserialize<LoginRequest>(reader.ReadToEnd());
         Console.WriteLine(body.username);
         return new Collection<IUser>("users").find(context, t => t.username == body.username);
      }

      private static void FetchToken(HttpListenerContext context) { 

      }

      public static void Login(HttpListenerContext context) {
         IUser res = User.FetchDetails(context);

         if (res == null) {
            new Response<string>(context).Send("User does not Exist");
         } else {
            string str = $"{res.username}:{res.password}";
            Console.WriteLine();

            LoginResponse response = new LoginResponse() {
               Token = new Auth().Generate(str).Token,
               User = res
            };

            new Response<string>(context).Send(JsonSerializer.Serialize<LoginResponse>(response));
         }
      }
   }
}