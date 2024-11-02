using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebInteractionLogger
{
    class Program
    {
        private static readonly string logFilePath = "file_upload_log.txt";
        private static readonly string websiteUrl = "https://purescan.godaddysites.com/";

        static async Task Main(string[] args) // Ensure this is correctly written
        {
            Console.WriteLine("Launching " + websiteUrl);
            OpenWebsite(websiteUrl);
            Console.WriteLine("Starting local server to listen for file uploads...");
            await StartLocalServer();
        }

        static void OpenWebsite(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        static async Task StartLocalServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();

            Console.WriteLine("Listening for file data on http://localhost:5000/");
            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;

                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string jsonData = await reader.ReadToEndAsync();
                    LogEvent("Received file information: " + jsonData);
                    Console.WriteLine("File info received: " + jsonData);
                }

                HttpListenerResponse response = context.Response;
                string responseString = "File information received successfully!";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }

        static void LogEvent(string message)
        {
            string logMessage = $"{DateTime.Now:G} - {message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            Console.WriteLine(logMessage);
        }
    }
}
th, logMessage + Environment.NewLine);
            Console.WriteLine(logMessage);
        }
    }
}
