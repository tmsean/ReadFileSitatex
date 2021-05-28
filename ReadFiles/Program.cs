using ReadFiles.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ReadFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Common common = new();
            //common.CheckFile();
            common.GetFileSita();

            //var context = new SCCContext();
            //var SubMessages = context.SCMessages.Where(s => s.MessageID == "201747 201016");
            //foreach (var sub in SubMessages)
            //{
            //    //Console.WriteLine(sub.Content);
            //    common.HandleSubMessage(sub.Content);
            //}
        }
    }
}