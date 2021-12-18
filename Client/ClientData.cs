using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class ClientData
    {
        public byte[] data;
        public Socket socket;
        public IPEndPoint iPEndPoint;
        public ClientData()
        {
            data = new byte[256];
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        }
        public ClientData(byte[] data, Socket socket, IPEndPoint iPEndPoint)
        {
            this.data = data;
            this.socket = socket;
            this.iPEndPoint = iPEndPoint;
        }
        public string GetMsg()
        {
            int bytes = 0;
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                bytes = socket.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (socket.Available > 0);
            return stringBuilder.ToString();
        }
    }
}
