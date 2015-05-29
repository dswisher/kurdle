using System;
using System.IO;
using Kurdle.Generation;

namespace Kurdle.Server
{
    public interface IChangeMonitor : IDisposable
    {
        void Start(IProjectInfo projectInfo);
        void Stop();
    }



    public class ChangeMonitor : IChangeMonitor
    {
        private IProjectInfo _projectInfo;
        private FileSystemWatcher _watcher;


        public void Start(IProjectInfo projectInfo)
        {
            if (_watcher != null)
            {
                throw new InvalidOperationException("Cannot start watching while already watching!");
            }

            _projectInfo = projectInfo;

            _watcher = new FileSystemWatcher(_projectInfo.Root.FullName);

            _watcher.IncludeSubdirectories = true;

            _watcher.Changed += OnChange;
            _watcher.Created += OnChange;
            _watcher.Deleted += OnChange;
            _watcher.Renamed += OnChange;

            // watcher.EnableRaisingEvents = true;

            // TODO - need to ignore files in output dir!
        }



        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;
        }



        private void OnChange(object sender, FileSystemEventArgs e)
        {
            // TODO
            Console.WriteLine("-> {0} {1}", e.ChangeType, e.FullPath);
        }



        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }
        }
    }
}
