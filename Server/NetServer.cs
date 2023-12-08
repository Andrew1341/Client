using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class NetServer
    {
        static void Main(string[] args)
        {
            Console.Title = "Stukan";           
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Server is starting...\nPlease wait...");

            try
            {
                Console.Write("Enter Server IP: ");
                string serverIP = Console.ReadLine();
                IPAddress ipAddr = IPAddress.Parse(serverIP);

                Console.Write("Enter Server Port: ");
                string serverPort = Console.ReadLine();
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, int.Parse(serverPort));

                using (Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    sListener.Bind(ipEndPoint);
                    sListener.Listen(10);

                    while (true)
                    {
                        Console.WriteLine("Waiting for connection on port {0}", ipEndPoint);
                        using (Socket handler = sListener.Accept())
                        {
                            string data = null;
                            byte[] bytes = new byte[1024];
                            int bytesRec = handler.Receive(bytes);
                            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                            Console.Write("Received message: " + data + "\n\n");

                            string reply = "Thank you for the reply in " + data.Length.ToString() + " symbols";
                            byte[] msg = Encoding.UTF8.GetBytes(reply);
                            handler.Send(msg);

                            if (data.IndexOf("<TheEnd>") > -1)
                            {
                                Console.WriteLine("Connection closed");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.Write("Press any key to continue . . . ");
                Console.ReadKey(true);
            }
        }
    }
}
