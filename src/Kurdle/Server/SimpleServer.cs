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
            private readonly HttpListener _listener;
            private readonly string _prefix;

            public XListener(string prefix)
            {
                _prefix = prefix;
                _listener = new HttpListener();
                _listener.Prefixes.Add(prefix);
            }


            public void StartListen()
            {
                if (!_listener.IsListening)
                {
                    _listener.Start();

                    Task.Factory.StartNew(async () =>
                    {
                        while (true) await Listen(_listener);
                    }, TaskCreationOptions.LongRunning);

                    Console.WriteLine("Listener started: {0}", _prefix);
                }
            }


            public void StopListen()
            {
                if (_listener.IsListening)
                {
                    _listener.Stop();
                    Console.WriteLine("Listener stopped");
                }
            }


            private async Task Listen(HttpListener l)
            {
                try
                {
                    var ctx = await l.GetContextAsync();
                    var url = ctx.Request.Url;

                    Console.WriteLine("Processing request: {0}", url);

                    var text = "Hello World";
                    var buffer = Encoding.UTF8.GetBytes(text);

                    using (var response = ctx.Response)
                    {
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unhandled exception processing request:");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
