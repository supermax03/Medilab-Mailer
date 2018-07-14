using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MedilabMailer
{
    public static class Library
    {
        public static void WriteErrorLog(String msg)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ":" + msg);
                sw.Flush();
                sw.Close();

            }
            catch
            { }

        }


    }
}
