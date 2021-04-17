using System;
using System.IO;
using System.Net;

namespace MordorServer
{
   public class Response<T>
   {
      public HttpListenerContext context;
      public Response(HttpListenerContext Context)
      {
         context = Context;
      }

      public void Send(T response)
      {
         using (Stream stream = this.context.Response.OutputStream)
         {
            using (StreamWriter writer = new StreamWriter(stream))
            {
               writer.Write(response);
            }
         }
      }
   }
}