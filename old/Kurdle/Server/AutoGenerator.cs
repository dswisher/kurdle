using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using Kurdle.Generation;

namespace Kurdle.Server
{

    public interface IAutoGenerator
    {
        void Watch(IProjectInfo projectInfo, bool beep);
        void Stop();
    }


    public class AutoGenerator : IAutoGenerator
    {
        private readonly IChangeMonitor _changeMonitor;
        private readonly Func<IProjectInfo, ISiteGenerator> _siteGenerator;
        private readonly Timer _timer;

        private IProjectInfo _projectInfo;
        private bool _beep;
        private DateTime _regenMoment = DateTime.MaxValue;


        public AutoGenerator(IChangeMonitor changeMonitor, Func<IProjectInfo, ISiteGenerator> siteGenerator)
        {
            _changeMonitor = changeMonitor;
            _siteGenerator = siteGenerator;

            _timer = new Timer(TimeSpan.FromMilliseconds(100).TotalMilliseconds);
            _timer.Elapsed += TimerOnElapsed;
        }



        public void Watch(IProjectInfo projectInfo, bool beep)
        {
            _projectInfo = projectInfo;
            _beep = beep;

            _changeMonitor.Start(_projectInfo, OnChange);

            try
            {
                Monitor.Enter(_timer);
                _timer.Start();
            }
            finally
            {
                Monitor.Exit(_timer);
            }
        }



        public void Stop()
        {
            try
            {
                Monitor.Enter(_timer);
                _timer.Stop();
            }
            finally
            {
                Monitor.Exit(_timer);
            }

            _changeMonitor.Stop();
        }



        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!Monitor.TryEnter(_timer))
            {
                return;
            }

            try
            {
                if (DateTime.UtcNow < _regenMoment)
                {
                    return;
                }

                Console.WriteLine("Changes detected...renerating...");
                // TODO - write out the changed files (perhaps optionally?)
                _siteGenerator(_projectInfo).Generate();

                _regenMoment = DateTime.MaxValue;

                if (_beep)
                {
                    Console.Beep(1000, 150);
                }

                Console.WriteLine("...done.");
            }
            finally
            {
                Monitor.Exit(_timer);
            }
        }



        private void OnChange(ChangeMonitor.ChangeNotification info)
        {
            // TODO - be more specific about what is generated (especially templates). For now, just regen all...
            try
            {
                Monitor.Enter(_timer);

                _regenMoment = DateTime.UtcNow.AddSeconds(1);
            }
            finally
            {
                Monitor.Exit(_timer);
            }
        }
    }
}
