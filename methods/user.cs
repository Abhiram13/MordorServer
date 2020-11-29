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

      private static void FetchToken(HttpListenerContext context)
      {
         
      }

      public static void Login(HttpListenerContext context)
      {
         // catch username and password
         // search database for the user
         // if exists, check is password is correct
         ///////// if wrong, user must sign in
         // else, generate token and send it to user

         if (User.FetchDetails(context) == null)
         {
            new Response<string>(context).Send("User does not Exist");
         }
      }
   }
}