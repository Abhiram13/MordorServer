using System;
using System.Net;
using System.Text.Json;

namespace MordorServer
{
   class User
   {
      private static IUser FetchDetails(HttpListenerContext context)
      {
         System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.InputStream);
         LoginRequest body = JsonSerializer.Deserialize<LoginRequest>(reader.ReadToEnd());
         return new Collection<IUser>("users").find(context, t => t.username == body.username);
      }

      public static void Login(HttpListenerContext context)
      {
         IUser user = User.FetchDetails(context);

         if (user == null)
         {
            new Response<string>(context).Send("User does not Exist");
         }
         else
         {
            string str = $"{user.username}:{user.password}";

            LoginResponse response = new LoginResponse()
            {
               Token = new Auth().Generate(str).Token,
               User = user
            };

            new Response<string>(context).Send(JsonSerializer.Serialize<LoginResponse>(response));
         }
      }

      public static void SignIn(HttpListenerContext context)
      {
         IUser user = User.FetchDetails(context);

         Console.WriteLine(user.username);

         if (user != null)
         {
            new Response<string>(context).Send("User had already Exist");
         }
         else
         {
            //
         }
      }
   }
}