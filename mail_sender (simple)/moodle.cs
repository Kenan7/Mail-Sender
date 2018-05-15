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
using System.Data.SQLite;

namespace mail_sender__simple_
{
    public partial class moodle : Form
    {
        public moodle()
        {
            InitializeComponent();
        }

        string haberr;
        string tarihh;
        string link;
        bool coklumail = false;
        string name = @"mailsender.db";
        string getOgrenci = "SELECT * FROM Ogrenciler";
        string create = "CREATE TABLE IF NOT EXISTS Ogrenciler(email TEXT)";
        

        void run_cmd()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo(@"parse_moodle\parse_moodle.exe");
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
        }

        private void moodle_Load(object sender, EventArgs e)
        {

            SQLiteConnection conn = new SQLiteConnection("Data Source=" + name + ";");
            conn.Open();

            SQLiteCommand cmd = conn.CreateCommand();

            cmd.CommandText = create;
            cmd.ExecuteNonQuery();

            ogr.Visible = false;
            bunifuMaterialTextbox1.Visible = false;
            button1.Visible = false;
            button5.Visible = false;

            cmd.CommandText = getOgrenci;
            cmd.ExecuteNonQuery();
            SQLiteDataReader dr = cmd.ExecuteReader();
            ogr.Items.Clear();
            while (dr.Read())
            {
                ogr.Items.Add(dr["email"]);
            }
            conn.Close();
            run_cmd();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sonuncuyuAl = "SELECT * FROM haber ORDER BY haber_kimligi ASC LIMIT 1;";
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + name + ";");
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sonuncuyuAl;
            cmd.ExecuteNonQuery();

            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                haberr = dr["baslik"].ToString();
                tarihh = dr["tarih"].ToString();
                link = dr["link"].ToString();
                
            }

            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("", "");
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            if (coklumail)
            {
                try
                {
                    foreach (string item in ogr.CheckedItems)
                    {
                        msg.To.Add(item);
                        msg.From = new MailAddress("");
                        msg.Subject = "Moodleda En Son Haber";
                        msg.Body = "En sonuncu haber:   " + haberr + Environment.NewLine + "Tarih:   " + tarihh + Environment.NewLine + "Link:   " + link;
                        client.EnableSsl = true;
                        client.Send(msg);
                    }
                }

                catch
                {
                    MessageBox.Show("Lütfen mesaj gönderebileceğimiz bir email adresi yazınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            else if (coklumail == false)
            {
                string to = (tbox.Text).ToString();
                msg.To.Add(to);
                msg.From = new MailAddress("");
                msg.Subject = "Moodleda En Son Haber";
                msg.Body = "En sonuncu haber:   " + haberr + Environment.NewLine + "Tarih:   " + tarihh + Environment.NewLine + "Link:   " + link;
                client.EnableSsl = true;
                client.Send(msg);
            }
            conn.Close();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ogrencimaili = null;

            try
            {
                ogrencimaili = bunifuMaterialTextbox1.Text;
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

        private void button4_Click(object sender, EventArgs e)
        {
            acilis acilis = new acilis();
            acilis.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            coklumail = true;
            ogr.Visible = true;
            bunifuMaterialTextbox1.Visible = true;
            button1.Visible = true;
            button5.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button3.Visible = true;
            coklumail = false;
            ogr.Visible = false;
            bunifuMaterialTextbox1.Visible = false;
            button1.Visible = false;
            button5.Visible = false;
        }
    }
}
