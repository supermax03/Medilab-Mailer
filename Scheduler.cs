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
using System.Configuration;
using System.Net.Mail;


namespace MedilabMailer
{
    public partial class Scheduler : ServiceBase
    {
        private Timer timer1 = null;
        MedicinaEntities db;

        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["Time"]);
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            this.timer1.Enabled = true;
            Library.WriteErrorLog("Mailer comenzando...");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            
            try
            {
                procesarNovedades();          
                
            }
            catch (Exception exc)
            {
                Library.WriteErrorLog("Error" + exc.InnerException.Message);
                Library.WriteErrorLog("Error" + exc.Message);
            }
        }
        private void procesarNovedades()
        {
            try
            {
                db = new MedicinaEntities();
                List<Novedad> novedades = db.Novedad.Take(10).ToList();
                foreach (Novedad novedad in novedades)
                {
                    string body = db.Template.Where(s => s.Id.Equals(novedad.Template.Id)).Select(s => s.Content).First();
                    body = body.Replace("{UserName}", novedad.Usuario.Username); 
                    body = body.Replace("{Title}", novedad.Template.Title);
                    body = body.Replace("{message}", novedad.Template.Msg.Replace("{password}",novedad.Usuario.Password));
                    Mailer.Mailer.sendEmail(novedad.Usuario.Email, "Medilab Informa", body);
                    db.Novedad.Remove(novedad);
                    db.SaveChanges();
                }
            }
            catch(Exception exc)
            {
                Library.WriteErrorLog("Error" + exc.InnerException.Message);
                Library.WriteErrorLog("Error" + exc.Message);

            }


        }



        protected override void OnStop()
        {
            timer1.Enabled = false;
            Library.WriteErrorLog("Desbloqueo de usuarios detenido...");
        }

    }
}
