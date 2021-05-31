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
        //Define command levels to define whether should get Flight Number, IssueDate, Config, ORI/DEST, FROMTIME/TOTIME
        private readonly Dictionary<string, int> commandvalues = new Dictionary<string, int>();
        public void GetFileSita()
        {
            string directory = "E:\\File HDQ";
            var context = new SCCContext();

            foreach (string file in Directory.EnumerateFiles(directory, "*.RCV"))
            {
                List<string> contents = File.ReadAllLines(file).ToList();
                SITATEX sCC = new();
                FileInfo getFile = new(file);
                //Get file name
                sCC.FileName = getFile.Name;

                int checkHeader = contents.FindIndex(a => a.Contains("=HEADER"));
                sCC.Header = contents[checkHeader + 1];
                int checkPriority = contents.FindIndex(a => a.Contains("=PRIORITY"));
                sCC.Priority = contents[checkPriority + 1];
                int checkDestinations = contents.FindIndex(a => a.Contains("=DESTINATION TYPE B"));
                int checkOrigin = contents.FindIndex(a => a.Contains("=ORIGIN"));
                //Get list of destinations
                var sb = new System.Text.StringBuilder();
                for (int i = checkDestinations + 1; i < checkOrigin; i++)
                {
                    sb.AppendLine(contents[i]);
                }
                sCC.Destinations = sb.ToString();
                sCC.Origin = contents[checkOrigin + 1];
                //Get messageID, smi, text
                int checkMessageId = contents.FindIndex(a => a.Contains("=MSGID"));
                sCC.MessageId = contents[checkMessageId + 1];
                int checkSMI = contents.FindIndex(a => a.Contains("=SMI"));
                sCC.SMI = contents[checkSMI + 1];
                int checkText = contents.FindIndex(a => a.Contains("=TEXT"));
                sCC.Text = String.Concat(contents[checkText + 1], "\n", contents[checkText + 2]);
                //Get sub messages
                StringBuilder subMessageContent = new StringBuilder();
                int checkMessageEnd = contents.FindIndex(a => a.Contains("SI") || a.Contains("DC THEO SLOT") || a.Contains("BRGDS/") || a.Contains("BRGDS") || a.Contains("RSN:")); //Text for footer
                for (int i = checkText + 3; i < checkMessageEnd; i++)
                {
                    subMessageContent.AppendLine(contents[i]);
                }

                sCC.SubMessages = new List<SubMessage>();
                List<string> messages = subMessageContent.ToString().Split("//").ToList();
                foreach (string message in messages)
                {
                    //Console.WriteLine(message);
                    if (!String.IsNullOrEmpty(message))
                    {
                        string message_RemoveEmptyLines = Regex.Replace(message, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                        var SCMessage = new SubMessage()
                        {
                            SITATEXID = sCC.ID,
                            MessageID = sCC.MessageId,
                            Content = message_RemoveEmptyLines
                        };
                        sCC.SubMessages.Add(SCMessage);
                    }
                }
                //Get footer content
                StringBuilder footer = new System.Text.StringBuilder();
                if (checkMessageEnd > 0)
                {
                    for (int i = checkMessageEnd; i < contents.Count; i++)
                    {
                        //Console.WriteLine(checkMessageEnd);
                        footer.AppendLine(contents[i]);
                    }
                }
                sCC.MessageEnd = footer.ToString();
                //If file name not exist, insert, else update
                if (!context.SITATEXes.Any(s => s.FileName == sCC.FileName))
                {
                    context.Add(sCC);
                }
                else
                {
                    context.Update(sCC);
                }
                //Get sub messages
                context.SaveChanges();
            }
            context.SaveChanges();
            //Console.WriteLine("=ORIGIN\nHDQONVN");
        }
        public void HandleSubMessage(string content)
        {
            string[] lines = Regex.Split(content, @"\r?\n|\r");
            string[] firstLine = lines[0].Split();
            if (!string.IsNullOrEmpty(firstLine[1]))
            {
                string ChangeReason = Regex.Replace(firstLine[1], @"\r?\n|\r", "");
                Console.WriteLine(string.Format("SC Reason is: {0}", ChangeReason));
            }
            Console.WriteLine(string.Format(""));
            //Decide level of this submessage to get command details
            if (firstLine[0].Contains("/"))
            {
                List<string> commands = firstLine[0].Split("/").ToList();
                int maxLevel = 1;

                Console.WriteLine(string.Format("List of commands:"));
                foreach (var c in commands)
                {
                    if (commandvalues[c] > maxLevel) maxLevel = commandvalues[c];
                    Console.WriteLine(c);
                }
                CheckCommandASM(maxLevel, lines);
            }
            else //1 command only
            {
                Console.WriteLine(string.Format("Command is: {0}", firstLine[0]));
                CheckCommandASM(commandvalues[firstLine[0]], lines);
            }
            Console.WriteLine();
        }
        public void CheckCommandASM(int level, string[] lines)
        {
            //Get flight number, Issue date from second line
            string FlightNumber = lines[1].Split("/")[0];
            string IssueDate = lines[1].Split("/")[1];
            Console.WriteLine(string.Format("Flight number: {0}", FlightNumber));
            Console.WriteLine(string.Format("Issue date: {0}", IssueDate));
            if (level == 4)
            {
                //GetFullContent
                //Get config from third line
                string Config = lines[2];
                //Get origin/destination, fromtime/totime from fourth line
                string Origin = lines[3].Split()[0].Substring(0, 3);
                string FromTime = lines[3].Split()[0].Substring(2, 4);
                string Destination = lines[3].Split()[1].Substring(0, 3);
                string ToTime = lines[3].Split()[1].Substring(2, 4);
                Console.WriteLine(string.Format("Config: {0}", Config));
                Console.WriteLine(string.Format("Orgin/Destination {0}/{1}", Origin, Destination));
                Console.WriteLine(string.Format("From Time/To Time: {0}/{1}", FromTime, ToTime));

            }
            else if (level == 3)
            {
                //GetChangeTime
                //Get origin/destination, fromtime/totime from third line
                string Origin = lines[2].Split()[0].Substring(0, 3);
                string FromTime = lines[2].Split()[0].Substring(2, 4);
                string Destination = lines[2].Split()[1].Substring(0, 3);
                string ToTime = lines[2].Split()[1].Substring(2, 4);
                Console.WriteLine(string.Format("Orgin/Destination {0}/{1}", Origin, Destination));
                Console.WriteLine(string.Format("From Time/To Time: {0}/{1}", FromTime, ToTime));
            }
            else if (level == 2)
            {
                //GetChangeConfig
                //Get config from third line
                string Config = lines[2];
                Console.WriteLine(string.Format("Config: {0}", Config));
            }
            else if (level == 1)
            {
                //GetFlightNoIssDateOnly
            }
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
                string contents = File.ReadAllText(file);
                if (contents.Contains("=ORIGIN\nHDQONVN") || contents.Contains("=ORIGIN\nHDQTPVN") || contents.Contains("=ORIGIN\nHDQOSVN"))
                {

                    FileInfo filetoget = new FileInfo(file);
                    Console.WriteLine(string.Format("File found: {0}", filetoget.Name));
                    File.Copy(file, Path.Combine(toDirectory, filetoget.Name), true);
                    Console.WriteLine(Path.Combine(toDirectory, filetoget.Name));
                }

                //Method 2
                //int counter = 0;
                //string line;

                //// Read the file and display it line by line.
                //System.IO.StreamReader readFile = new System.IO.StreamReader(file);
                //while ((line = readFile.ReadLine()) != null)
                //{
                //    StringBuilder context = new StringBuilder();
                //    context.Append(line + "\n");
                //    if (context.ToString().Contains("=ORIGIN\nHDQONVN") || context.ToString().Contains("=ORIGIN\nHDQTPVN") || context.ToString().Contains("=ORIGIN\nHDQOSVN"))
                //    {
                //        FileInfo fileToGet = new FileInfo(file);
                //        File.Copy(file, Path.Combine(toDirectory, fileToGet.Name), true);
                //        continue;
                //    }

                //    counter++;
                //}

                //readFile.Close();
            }

            Console.ReadLine();
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

        #region Constructor
        public Common()
        {
            commandvalues.Add("NEW", 4);
            commandvalues.Add("RPL", 4);
            commandvalues.Add("TIM", 3);
            commandvalues.Add("EQT", 2);
            commandvalues.Add("CON", 2);
            commandvalues.Add("CNL", 1);
        }
        #endregion
    }
}
