﻿using System;

namespace MordorServer {
   class Program {
      static void Main(string[] args) {
         Console.WriteLine(Server.Mongo.database);
      }
   }
}
