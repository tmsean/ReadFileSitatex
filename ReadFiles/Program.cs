using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ReadFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Common common = new();
            common.GetFileSita();
        }
    }
}