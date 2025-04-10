
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

class Receiver
{
    public static void Main()
    {
        int port = 5005;
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"🟢 Receiver listening on port {port}...");

        string logDir = "data_logs";
        Directory.CreateDirectory(logDir);

        while (true)
        {
            using TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("✅ Client connected.");
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[16384];

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string raw = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                int depth = 0;
                int startIdx = -1;

                for (int i = 0; i < raw.Length; i++)
                {
                    if (raw[i] == '{')
                    {
                        if (depth == 0) startIdx = i;
                        depth++;
                    }
                    else if (raw[i] == '}')
                    {
                        depth--;
                        if (depth == 0 && startIdx >= 0)
                        {
                            string json = raw.Substring(startIdx, i - startIdx + 1);
                            try
                            {
                                string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss-fff");
                                string filepath = Path.Combine(logDir, $"state_{timestamp}.json");
                                File.WriteAllText(filepath, json);
                            }
                            catch
                            {
                                // silently ignore invalid JSON
                            }
                        }
                    }
                }
            }
        }
    }
}
