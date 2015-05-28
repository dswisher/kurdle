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

            _listener = new XListener(url, ProcessRequest);
            _listener.StartListen();

            Console.WriteLine("Listening on port {0}...", port);
        }



        public void Stop()
        {
            Console.WriteLine("Shutting down server...");
            _listener.StopListen();
        }



        private void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            var uriPath = request.Url.LocalPath.Substring(1);

            // TODO - better default document handling!
            if (string.IsNullOrEmpty(uriPath))
            {
                uriPath = "index.html";
            }

            var localPath = Path.Combine(_rootDir.FullName, uriPath);

            Console.WriteLine("Processing {0}, local path: {1}", request.Url, localPath);

            var info = new FileInfo(localPath);

            byte[] buffer;
            if (!info.Exists)
            {
                var text = string.Format("Could not find file: {0}", info.FullName);
                buffer = Encoding.UTF8.GetBytes(text);
                response.StatusCode = 404;
            }
            else
            {
                // TODO - how to determine content encoding...do we care?

                response.ContentType = MimeTypeMap.GetMimeType(info.Extension);

                buffer = File.ReadAllBytes(info.FullName);
            }

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }



        // See http://stackoverflow.com/questions/16946389/stopping-and-restarting-httplistener
        public class XListener
        {
            private readonly HttpListener _listener;
            private readonly string _prefix;
            private readonly Action<HttpListenerRequest, HttpListenerResponse> _handler;

            public XListener(string prefix, Action<HttpListenerRequest, HttpListenerResponse> handler)
            {
                _prefix = prefix;
                _handler = handler;
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
// ReSharper disable once FunctionNeverReturns
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

                    var request = ctx.Request;
                    using (var response = ctx.Response)
                    {
                        _handler(request, response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unhandled exception processing request:");
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
