using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace ftp
{
    public partial class Form1 : Form
    {
        public static string error;
        public Form1()
        {
            InitializeComponent();
        }
        class ftp
        {
            private string host = null;
            private string user = null;
            private string pass = null;
            private FtpWebRequest ftpRequest = null;
            private FtpWebResponse ftpResponse = null;
            private Stream ftpStream = null;

            public ftp(string host_in, string userName, string password)
            {
                host = host_in;
                user = userName;
                pass = password;
            }

            public string[] directoryListDetailed()
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host);
                    ftpRequest.Credentials = new NetworkCredential(user, pass);
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpStream = ftpResponse.GetResponseStream();
                    StreamReader ftpReader = new StreamReader(ftpStream);
                    string directoryRaw = null;
                    try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                    catch (Exception ex) { error = ex.ToString(); }
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    ftpRequest = null;
                    try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                    catch (Exception ex) { error = ex.ToString(); }
                }
                catch (Exception ex) { error=ex.ToString(); }
                return new string[] { error };
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            string adres = textBox_adres.Text+":"+textBox_port.Text+textBox_folder.Text;
            string login = textBox_login.Text;
            string haslo = textBox_password.Text;

            ftp ftpClient = new ftp(adres, login, haslo);

            string[] detailDirectoryListing = ftpClient.directoryListDetailed();
            for (int i = 0; i < detailDirectoryListing.Count(); i++)
            {
                textBox_log.AppendText(detailDirectoryListing[i]);
                textBox_log.AppendText("\r\n");
            }
            ftpClient = null;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_log.Clear();
        }
    }
}
