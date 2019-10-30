using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace K164037_Q5_2
{
    public partial class Service1 : ServiceBase
    {
        private readonly Timer _timer;
        public Service1()
        {
            InitializeComponent();
            _timer = new Timer(900000) { AutoReset = true };
            _timer.Elapsed += sendMail;
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
            } while (Console.Read()!= 1);
        }
        private void sendMail(object sender, ElapsedEventArgs e)
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

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var user = ConfigurationManager.AppSettings["smtpUser"];
            var password = ConfigurationManager.AppSettings["smtpPass"];
            var to = ConfigurationManager.AppSettings["to"];
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(user);
                mail.To.Add(to);
                mail.Subject = "Folder Activity Notification";
                mail.Body = "File Name : "+e.Name+" File Size : "+e.Name.Length;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Credentials = new System.Net.NetworkCredential(user, password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("mail Send");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("File Name : " + e.Name + " File Size : " + e.Name.Length);
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
