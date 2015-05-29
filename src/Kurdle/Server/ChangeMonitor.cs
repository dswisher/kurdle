using System;
using System.IO;
using Kurdle.Generation;

namespace Kurdle.Server
{
    public interface IChangeMonitor : IDisposable
    {
        void Start(IProjectInfo projectInfo, Action<ChangeMonitor.ChangeNotification> onChange);
        void Stop();
    }



    public class ChangeMonitor : IChangeMonitor
    {
        private IProjectInfo _projectInfo;
        private FileSystemWatcher _watcher;
        private Action<ChangeNotification> _onChange;


        public void Start(IProjectInfo projectInfo, Action<ChangeNotification> onChange)
        {
            if (_watcher != null)
            {
                throw new InvalidOperationException("Cannot start watching while already watching!");
            }

            _projectInfo = projectInfo;
            _onChange = onChange;

            _watcher = new FileSystemWatcher(_projectInfo.Root.FullName);

            _watcher.Changed += OnChange;
            _watcher.Created += OnChange;
            _watcher.Deleted += OnChange;
            _watcher.Renamed += OnChange;

            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }



        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;
        }



        private void OnChange(object sender, FileSystemEventArgs e)
        {
            var path = e.FullPath;

            if (_projectInfo.IsIgnored(path))
            {
                return;
            }

            var notification = new ChangeNotification { FullPath = path };

            _onChange(notification);
        }



        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Changed -= OnChange;
                _watcher.Created -= OnChange;
                _watcher.Deleted -= OnChange;
                _watcher.Renamed -= OnChange;

                _watcher.Dispose();
                _watcher = null;
            }
        }



        public class ChangeNotification
        {
            // TODO - do we need change type or anything else?
            public string FullPath { get; set; }
        }
    }
}
