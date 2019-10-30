using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace K164037_Q5
{
    public partial class Service1 : ServiceBase
    {
        public  Timer _timer;
        public Service1()
        {
            InitializeComponent();
            _timer = new Timer(5000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;

        }
        public void onDeubg()
        {
           
        }

        public void test()
        {
            do
            {
                FileSystemWatcher Folder = new FileSystemWatcher();
                Folder.Path = ConfigurationManager.AppSettings["path"];
                Folder.NotifyFilter = NotifyFilters.LastWrite;
                Folder.Filter = "*.*";
                Folder.Changed += new FileSystemEventHandler(OnChanged);
                Folder.Created += new FileSystemEventHandler(OnChanged);
                Folder.Deleted += new FileSystemEventHandler(OnChanged);
                // Folder.Renamed += new FileSystemEventHandler(OnChanged);
                Folder.EnableRaisingEvents = true;
            } while (Console.Read() != 1);
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            FileSystemWatcher Folder = new FileSystemWatcher();
            Folder.Path = ConfigurationManager.AppSettings["path"];
            Folder.NotifyFilter = NotifyFilters.LastWrite;
            Folder.Filter = "*.*";
            Folder.Changed += new FileSystemEventHandler(OnChanged);
            Folder.Created += new FileSystemEventHandler(OnChanged);
            Folder.Deleted += new FileSystemEventHandler(OnChanged);
            // Folder.Renamed += new FileSystemEventHandler(OnChanged);
            Folder.EnableRaisingEvents = true;

        }

        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.Name);
            File.Copy(ConfigurationManager.AppSettings["path"]+ e.Name, ConfigurationManager.AppSettings["path2"]+ e.Name);
            _timer = new Timer(5000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        protected override void OnStart(string[] args)
        {
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }
    }
}
