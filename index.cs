using System;
using System.Net.Http;
using System.Net;
using System.IO;

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
            try
            {
               HttpListenerContext context = http.GetContext();
               using (Stream stream = context.Response.OutputStream)
               {
                  using (StreamWriter writer = new StreamWriter(stream))
                  {
                     writer.Write("Here is the Response Message");
                  }
               }
               Console.WriteLine("Respone Sent");
            }
            catch (Exception e)
            {
               Console.WriteLine(e);
            }
         }
      }
   }
}