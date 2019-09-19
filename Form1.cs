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
using System.Net.Http;
using System.Diagnostics;


namespace IPTVCHECKER_C_SHARP
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if ((textBox2.Text == "") || (richTextBox1.Text == "") || (richTextBox2.Text == ""))
            {
                MessageBox.Show("Lütfen Tüm Kutucuları Doldurunuz.", "IpTVChecker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox1.Text = "";
                string user = richTextBox1.Text;
                string[] userlines = user.Split(
        new[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None
    );
                string pass = richTextBox2.Text;
                string[] passlines = pass.Split(
        new[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None
    );
                int satir = richTextBox1.Lines.Count();
                for (int i = 0; i < satir; i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(textBox2.Text + "/get.php?username=" + userlines[i] + "&password=" + passlines[i] + "&type=m3u");

                        if (response.StatusCode.ToString() == "NotFound")
                        {
                            textBox1.Text = textBox1.Text + "\r\nNot Working";
                        }
                        else
                        {
                            string m3u = await new WebClient().DownloadStringTaskAsync(textBox2.Text + "/get.php?username=" + userlines[i] + "&password=" + passlines[i] + "&type=m3u");
                            if (m3u.Contains("#EXTM3U") == true)
                            {
                                textBox1.Text = textBox1.Text + "\r\nWorking; " + userlines[i] + ":" + passlines[i];
                            }
                            else
                            {
                                textBox1.Text = textBox1.Text + "\r\nNot Working";

                            }
                        }
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/quiec");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/quiec");
        }
    }
}
