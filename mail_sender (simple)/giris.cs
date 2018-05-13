using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace mail_sender__simple_
{
    public partial class giris : Form
    {
        public giris()
        {
            InitializeComponent();
        }

        public static string mail;
        public static string sifre;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                mail = bunifuMaterialTextbox1.Text;
                sifre = bunifuMaterialTextbox2.Text;
                MailMessage msg = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(mail, sifre);
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                Form1 form1 = new Form1(mail, sifre);
                form1.Show();
                this.Hide();

            }

            catch
            {
                MessageBox.Show("Bir sorunla karşılaşıldı. Lütfen programı kapatıp açmayı deneyin ve internet bağlantınızı kontrol edin.");
            }
            
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void giris_Load(object sender, EventArgs e)
        {
            
        }
    }
}
