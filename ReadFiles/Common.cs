using ReadFiles.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReadFiles
{
    class Common
    {
        public void GetFileSita()
        {
            string directory = "E:\\File HDQ";
            var context = new SCCContext();

            foreach (string file in Directory.EnumerateFiles(directory, "*.RCV"))
            {
                List<string> contents = File.ReadAllLines(file).ToList();
                SCC_SITATEX sCC = new();

                int checkHeader = contents.FindIndex(a => a.Contains("=HEADER"));
                sCC.Header = contents[checkHeader + 1];
                int checkPriority = contents.FindIndex(a => a.Contains("=PRIORITY"));
                sCC.Priority = contents[checkPriority + 1];
                int checkDestinations = contents.FindIndex(a => a.Contains("=DESTINATION TYPE B"));
                int checkOrigin = contents.FindIndex(a => a.Contains("=ORIGIN"));
                var sb = new System.Text.StringBuilder();
                for (int i = checkDestinations + 1; i < checkOrigin; i++)
                {
                    sb.AppendLine(contents[i]);
                }
                sCC.Destinations = sb.ToString();
                sCC.Origin = contents[checkOrigin + 1];
                int checkMessageId = contents.FindIndex(a => a.Contains("=MSGID"));
                int checkText = contents.FindIndex(a => a.Contains("=TEXT"));
                sCC.MessageId = contents[checkMessageId + 1];
                sCC.Text = String.Concat(contents[checkText + 1], "\n", contents[checkText + 2]);
                StringBuilder subMessage = new StringBuilder();
                int checkMessageEnd = contents.FindIndex(a => a.Contains("SI"));
                for (int i = checkText + 2; i < checkMessageEnd; i++)
                {
                    subMessage.AppendLine(contents[i]);
                }
                sCC.SubMessage = subMessage.ToString();
                var footer = new System.Text.StringBuilder();
                if (checkMessageEnd > 0)
                {
                    for (int i = checkMessageEnd; i < contents.Count - 1; i++)
                    {
                        Console.WriteLine(checkMessageEnd);
                        footer.AppendLine(contents[i]);
                    }
                }
                sCC.MessageEnd = footer.ToString();
                //If messageId not exist, insert, else update
                if (context.SITATEX_FILES.Where(s => s.MessageId == sCC.MessageId).FirstOrDefault<SCC_SITATEX>() is null)
                {
                    context.Add(sCC);
                }
                else
                {
                    context.Update(sCC);
                }

                GetSchedulesMessages(sCC.MessageId, sCC.SubMessage);
            }
            context.SaveChanges();
            //Console.WriteLine("=ORIGIN\nHDQONVN");
        }
        public void GetSchedulesMessages(string MessageID, string Content)
        {
            var context = new SCCContext();

            var schedulesMessage = new SchedulesMessage()
            {
                MessageID = MessageID,
                Content = Content
            };
            context.Add(schedulesMessage);
            context.SaveChanges();
        }
        public void CheckFile()
        {
            string directory = ConfigurationManager.AppSettings["directory"];
            string toDirectory = ConfigurationManager.AppSettings["toDirectory"];

            Console.WriteLine(directory);
            Console.WriteLine(toDirectory);
            Console.ReadLine();
            //foreach (string file in Directory.EnumerateFiles(directory, "*.RCV"))
            //{
            //    List<string> contents = File.ReadAllLines(file).ToList();

            //    int index = contents.FindIndex(a => a.Contains("=ORIGIN"));
            //    var checkFile = contents[index + 1];
            //    if (checkFile.Contains("HDQONVN") || checkFile.Contains("HDQTPVN") || checkFile.Contains("HDQOSVN"))
            //    {
            //        FileInfo fileToGet = new FileInfo(file);
            //        File.Copy(fileToGet.FullName, Path.Combine(toDirectory, @"\", fileToGet.Name), true);
            //    }

            //}

            foreach (string file in Directory.EnumerateFiles(directory, "*.RCV"))
            {
                //Method 1:
                //string content = File.ReadAllText(file);

                //if (content.Contains("=ORIGIN\nHDQONVN") || content.Contains("=ORIGIN\nHDQTPVN") || content.Contains("=ORIGIN\nHDQOSVN"))
                //{

                //    FileInfo fileToGet = new FileInfo(file);
                //    File.Copy(file, Path.Combine(toDirectory, fileToGet.Name), true);
                //}

                //Method 2
                int counter = 0;
                string line;

                // Read the file and display it line by line.
                System.IO.StreamReader readFile = new System.IO.StreamReader(file);
                while ((line = readFile.ReadLine()) != null)
                {
                    StringBuilder context = new StringBuilder();
                    context.Append(line + "\n");
                    if (context.ToString().Contains("=ORIGIN\nHDQONVN") || context.ToString().Contains("=ORIGIN\nHDQTPVN") || context.ToString().Contains("=ORIGIN\nHDQOSVN"))
                    {
                        FileInfo fileToGet = new FileInfo(file);
                        File.Copy(file, Path.Combine(toDirectory, fileToGet.Name), true);
                        continue;
                    }

                    counter++;
                }

                readFile.Close();
            }
            //Console.WriteLine("=ORIGIN\nHDQONVN");

            Console.ReadLine();
        }
        public void ReadFile()
        {
            //Default file. MAKE SURE TO CHANGE THIS LOCATION AND FILE PATH TO YOUR FILE   
            string textFile = @"E:\SCC\SITATEX_TEST.txt";

            if (File.Exists(textFile))
            {
                // Read entire text file content in one string    
                string text = File.ReadAllText(textFile);
                Console.WriteLine(text);
            }

            if (File.Exists(textFile))
            {
                // Read a text file line by line.  
                string[] lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                    Console.WriteLine(line);
            }

            if (File.Exists(textFile))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(textFile))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        Console.WriteLine(ln);
                        counter++;
                    }
                    file.Close();
                    Console.WriteLine($"File has {counter} lines.");
                }
            }

            Console.ReadKey();
        }

        public void TrackDirectory()
        {
            using var watcher = new FileSystemWatcher(@"E:\Test check");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
