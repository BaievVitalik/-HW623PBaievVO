using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;


namespace ReactorPatternDemo
{

    public class Reactor
    {
        private List<Socket> _socketsToMonitor = new List<Socket>();
        private bool _isRunning = false;

        public void AddSocket(Socket socket)
        {
            _socketsToMonitor.Add(socket);
            Console.WriteLine($"[Reactor] Socket {socket.RemoteEndPoint} додано для моніторингу.");
        }

        public void Start()
        {
            _isRunning = true;
            Console.WriteLine("[Reactor] Запущено. Очікування на події...");

            while (_isRunning)
            {
                if (_socketsToMonitor.Count == 0)
                {
                    Thread.Sleep(100); 
                    continue;
                }

                var checkRead = new List<Socket>(_socketsToMonitor);
                var checkError = new List<Socket>(_socketsToMonitor);

                try
                {
                    Socket.Select(checkRead, null, checkError, 1000000);

                    foreach (var socket in checkRead)
                    {
                        HandleRead(socket);
                    }

                    foreach (var socket in checkError)
                    {
                        HandleError(socket);
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"[Reactor] Помилка сокета: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void HandleRead(Socket socket)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int received = socket.Receive(buffer, SocketFlags.None);
                if (received > 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"[EventHandler] Отримано від {socket.RemoteEndPoint}: {data.Trim()}");

                    byte[] response = Encoding.UTF8.GetBytes("Повідомлення отримано\n");
                    socket.Send(response);
                }
                else 
                {
                    Console.WriteLine($"[EventHandler] Клієнт {socket.RemoteEndPoint} від'єднався.");
                    socket.Close();
                    _socketsToMonitor.Remove(socket);
                }
            }
            catch (SocketException)
            {
                HandleError(socket);
            }
        }

        private void HandleError(Socket socket)
        {
            Console.WriteLine($"[EventHandler] Помилка на сокеті {socket.RemoteEndPoint}. Закриття з'єднання.");
            socket.Close();
            _socketsToMonitor.Remove(socket);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Демонстрація шаблону Reactor ---");

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Сервер слухає на порту 8888...");
            Console.WriteLine("Підключіться до сервера за допомогою telnet: 'telnet 127.0.0.1 8888'");

            var reactor = new Reactor();
            var reactorThread = new Thread(reactor.Start);
            reactorThread.Start();

            try
            {
                while (true)
                {
                    Socket clientSocket = listener.Accept();
                    Console.WriteLine($"Прийнято нове з'єднання від {clientSocket.RemoteEndPoint}");

                    reactor.AddSocket(clientSocket);
                }
            }
            finally
            {
                reactor.Stop();
                reactorThread.Join();
                listener.Close();
            }
        }
    }
}
