using System;
using System.Text;

namespace MordorServer {
   class String {
      static string Encode(string s) {
         byte[] bytes = Encoding.UTF8.GetBytes(s);
         return Convert.ToBase64String(bytes);
      }

      static string Decode(string base64String) {
         byte[] bytes = Convert.FromBase64String(base64String);
         return Encoding.UTF8.GetString(bytes);
      }
   }
}