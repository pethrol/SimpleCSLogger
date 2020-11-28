using Newtonsoft.Json;
using System;
using System.IO;

//Simple C# logger made by procq 28.11.2020

namespace App {

    class Logger {

        class LogMessage {
            public string Time { get; set; } // I picked string instead of DateTime becouse I ToLongTimeString() delivers time without miliseconds.
            public string Summary { get; set; }
        }

        private static string filename;
        private static string path;

        private static void CreateFileIfNotExists() { // Also creating directory
            filename = DateTime.UtcNow.Date.ToShortDateString();
            path = "logs/" + filename + ".log";

            if (!File.Exists(path)) {
                Directory.CreateDirectory("logs");
                File.Create(path).Close();
            }
        }
        public static void Log(string text) {
            CreateFileIfNotExists();

            DateTime timeNow = DateTime.UtcNow;
            string timeString = timeNow.ToLongTimeString() + ":" + timeNow.Millisecond.ToString();

            LogMessage log = new LogMessage {
                Time = timeString,
                Summary = text
            };

            string jsonString = JsonConvert.SerializeObject(log);

            jsonString = jsonString.Replace(@"\r\n", ""); // getting rid those new line characters

            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonString);

            try {
                FileStream stream = new FileStream(path, FileMode.Append);
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            catch (Exception) {
                Console.WriteLine("Failed to open log stream.");
            }
        }
    }
}
