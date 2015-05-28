﻿using System;
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
            var url = request.Url;

            Console.WriteLine("Processing request: {0}", url);

            var text = "Hello World";
            var buffer = Encoding.UTF8.GetBytes(text);

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
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
