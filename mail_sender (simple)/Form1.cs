using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Net.Mail;

namespace mail_sender__simple_
{
    public partial class Form1 : Form
    {
        string mail;
        string sifre;
        string name = @"mailsender.db";
        string getOgrenci = "SELECT * FROM Ogrenciler";
        string create = "CREATE TABLE IF NOT EXISTS Ogrenciler(email TEXT)";

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string mail, string sifre)
        {
            InitializeComponent();
            this.mail = mail;
            this.sifre = sifre;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SQLiteConnection conn = new SQLiteConnection("Data Source=" + name + ";");
            conn.Open();

            SQLiteCommand cmd = conn.CreateCommand();

            cmd.CommandText = create;
            cmd.ExecuteNonQuery();

            cmd.CommandText = getOgrenci;
            cmd.ExecuteNonQuery();
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ogr.Items.Add(dr["email"]);
            }
            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ogrencimaili = null;

            try
            {
                ogrencimaili = bunifuMaterialTextbox2.Text;
            }
            catch
            {
                MessageBox.Show("Veriyi alamadık.");
            }

            if (ogrencimaili != null && ogrencimaili != "")
            {
                string writeToOgrenci = "INSERT INTO Ogrenciler (email) values ('" + ogrencimaili + "')";

                SQLiteConnection conn = new SQLiteConnection("Data Source=" + name + ";");
                conn.Open();

                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = writeToOgrenci;
                cmd.ExecuteNonQuery();

                cmd.CommandText = getOgrenci;
                cmd.ExecuteNonQuery();
                SQLiteDataReader dr = cmd.ExecuteReader();
                ogr.Items.Clear();
                while (dr.Read())
                {
                    ogr.Items.Add(dr["email"]);
                }
                conn.Close();

            }

            else
            {
                ;
            } 
        }

        giris giris = new giris();
        Attachment filename;
        bool dosya = false;

        private void button2_Click(object sender, EventArgs e)
        {
            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(mail, sifre);
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            string subj = bunifuMaterialTextbox1.Text;
            string msj = bunifuMetroTextbox1.Text;

            try
            {
                
                foreach (string item in ogr.CheckedItems)
                {
                    msg.To.Add(item);

                    msg.From = new MailAddress(mail);
                    msg.Subject = subj;
                    msg.Body = msj;
                    if (dosya)
                    {
                        msg.Attachments.Add(filename);
                    }
                    client.EnableSsl = true;
                    client.Send(msg);
                }
            }

            catch
            {
                MessageBox.Show("Lütfen mesaj gönderebileceğimiz bir email adresi yazınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult c = openFileDialog1.ShowDialog();
            if (c == DialogResult.OK)
            {
                filename = new Attachment(openFileDialog1.FileName);
                dosya = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            acilis acilis = new acilis();
            acilis.Show();
            this.Hide();
        }
    }
}
