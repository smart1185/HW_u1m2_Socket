using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetProject_Server
{
    class Program
    {
        static int port = 8080;
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket socket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream,
                                        ProtocolType.Tcp);

            try
            {
                socket.Bind(endPoint);
                socket.Listen(100);    
               
                while (true)
                {
                    Console.WriteLine("Сервер запущен. Ожидание подключений...");                    
                    Socket acceptSocket = socket.Accept();

                    StringBuilder stringBuilder = new StringBuilder();

                    byte[] bytes;
                    int byteRec;

                    do
                    {
                        bytes = new byte[1024];
                        byteRec = acceptSocket.Receive(bytes);
                        stringBuilder.Append(Encoding.UTF8.GetString(bytes));
                    } while (acceptSocket.Available > 0);

                    Console.WriteLine("Message: {0}", stringBuilder.ToString());

                    string answer = string.Format("ваше сообщение {0} доставлено",
                        stringBuilder.Length);

                    byte[] reciveMsg = Encoding.UTF8.GetBytes(answer);

                    acceptSocket.Send(reciveMsg);
                    acceptSocket.Shutdown(SocketShutdown.Both);
                    acceptSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
