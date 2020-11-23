using System;
using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MordorServer
{
   public abstract class String
   {
      public virtual string Encode(string str)
      {
         var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
         return System.Convert.ToBase64String(plainTextBytes);
      }

      public virtual string Decode(string str)
      {
         var base64EncodedBytes = System.Convert.FromBase64String(str);
         return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
      }
   }

   public class Auth : Token
   {
      public void Headers(HttpListenerRequest request)
      {
         string[] values = request.Headers.GetValues("Token");
         string tokenHeader = values[0];
         Generate(tokenHeader);
      }
   }
}