using System;
using System.IO;
using System.Net;

namespace MordorServer {
     class Helper<Type> {
          public static void SendResponse(Type responseMessage, HttpListenerContext context) {
               try {
                    foreach (string key in context.Request.Headers.AllKeys) {
                         string[] values = context.Request.Headers.GetValues(key);
                         foreach (string name in values) {
                              Console.WriteLine(name);
                         }
                    }
                    using (Stream str = context.Response.OutputStream) {
                         using (StreamWriter writer = new StreamWriter(str)) {
                              writer.Write(responseMessage);
                         }
                    }
               } catch (ArgumentException error) {
                    context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                    context.Response.StatusDescription = error.Message;
                    Console.WriteLine(error.Message);
               }
          }
     }
}