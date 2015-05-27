using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kurdle.Server
{
    public interface ISimpleServer
    {
        void Start(DirectoryInfo rootDir, int port);
        void Stop();
    }


    public class SimpleServer : ISimpleServer
    {
        private XListener _listener;
        private DirectoryInfo _rootDir;


        public void Start(DirectoryInfo rootDir, int port)
        {
            _rootDir = rootDir;

            string url = "http://localhost:" + port + "/";

            _listener = new XListener(url);
            _listener.StartListen();

            Console.WriteLine("Listening on port {0}...", port);
        }



        public void Stop()
        {
            Console.WriteLine("Shutting down server...");
            _listener.StopListen();
        }



        // See http://stackoverflow.com/questions/16946389/stopping-and-restarting-httplistener
        public class XListener
        {
            HttpListener listener;

            public XListener(string prefix)
            {
                listener = new HttpListener();
                listener.Prefixes.Add(prefix);
            }

            public void StartListen()
            {
                if (!listener.IsListening)
                {
                    listener.Start();

                    Task.Factory.StartNew(async () =>
                    {
                        while (true) await Listen(listener);
                    }, TaskCreationOptions.LongRunning);

                    Console.WriteLine("Listener started");
                }
            }

            public void StopListen()
            {
                if (listener.IsListening)
                {
                    listener.Stop();
                    Console.WriteLine("Listener stopped");
                }
            }

            private async Task Listen(HttpListener l)
            {
                try
                {
                    var ctx = await l.GetContextAsync();

                    var text = "Hello World";
                    var buffer = Encoding.UTF8.GetBytes(text);

                    using (var response = ctx.Response)
                    {
                        ctx.Response.ContentLength64 = buffer.Length;
                        ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                }
                catch (HttpListenerException)
                {
                    Console.WriteLine("screw you guys, I'm going home!");
                }
            }
        }
    }
}
