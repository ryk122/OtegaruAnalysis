using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Globalization;


namespace OtegaruAnalysis
{
    public partial class Form1 : Form
    {
        private string jsonfile;
        private AnalysisData[] analysisdatas;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "open json file!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadJson();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (analysisdatas != null)
            {
                textBox1.Text = "Exporting csv.";

                string csv = ",android,ios,sum\r\n";

                for (int i = 0; i < analysisdatas.Length; i++)
                {
                    csv += analysisdatas[i].ToString() + "\r\n";
                }

                ExportFile(csv);
                textBox1.Text += "\r\n done.\r\n";
            }
            else
            {
                textBox1.Text = "Load Json!";
            }
        }

        private void OpenFile()
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
                jsonfile = openFileDialog1.FileName;
            }
        }

        private void LoadJson()
        {
            //string json = "[{\"os\":\"android\",\"coin\":230,\"created_at\":1607746294014},{\"coin\":1050,\"created_at\":1608013732727,\"os\":\"android\"}]";

            if (jsonfile==null)
            {
                OpenFile();
                return;
            }
            //string FilePath = @"C:\Users\ryk\Desktop\get_active_logs.json"; //ファイルパス
            string FilePath = jsonfile; //ファイルパス
            var json = File.ReadAllText(FilePath); // ファイル内容をjson変数に格納
            List<OtegaruData> datas = JsonSerializer.Deserialize<List<OtegaruData>>(json);

            AnalyzeJson(datas);
        }

        private void AnalyzeJson(List<OtegaruData> datas)
        {
            analysisdatas = new AnalysisData[32];

            for (int i = 0; i < analysisdatas.Length; i++)
            {
                analysisdatas[i] = new AnalysisData(i);
            }

            textBox1.Text += "There are " + datas.Count().ToString() + "datas.\r\n";

            foreach (OtegaruData d in datas)
            {
                DateTime t = DateTimeOffset.FromUnixTimeSeconds(d.created_at/1000).LocalDateTime;
                //textBox1.Text += " " + t.Day;
                int day = t.Day;

                if (d.os.Equals("android"))
                {
                    analysisdatas[day].AddAndroidCount();
                }
                else if (d.os.Equals("ios"))
                {
                    analysisdatas[day].AddiOSCount();
                }
                else
                {
                    analysisdatas[day].AddUnknownCount();
                }

                analysisdatas[day].AddCoin(d.coin);
            }

            for(int i=0; i<analysisdatas.Length; i++)
            {
                textBox1.Text += analysisdatas[i].ToString() + "\r\n";
            }
        }

        private void ExportFile(string str)
        {
            File.WriteAllText(@"C:\Users\ryk\Desktop\test.csv", str);
        }

        // UNIXエポックを表すDateTimeオブジェクトを取得
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static long GetUnixTime(DateTime targetTime)
        {
            // UTC時間に変換
            targetTime = targetTime.ToUniversalTime();

            // UNIXエポックからの経過時間を取得
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

            // 経過秒数に変換
            return (long)elapsedTime.TotalSeconds;
        }
    }

    public class OtegaruData
    {
        public int coin { get; set; }
        public long created_at { get; set; }
        public string os { get; set; }
    }

    public class AnalysisData
    {
        int day;
        int android, ios, unknown, sumcoin;

        public AnalysisData(int d)
        {
            day = d;
            android = 0;
            ios = 0;
            unknown = 0;
            sumcoin = 0;
        }

        public void AddAndroidCount()
        {
            android++;
        }

        public void AddiOSCount()
        {
            ios++;
        }

        public void AddUnknownCount()
        {
            unknown++;
        }

        public void AddCoin(int c)
        {
            sumcoin += c;
        }

        public int GetAverageCoin()
        {
            return sumcoin / (android + ios);
        }

        public override string ToString()
        {
            return day.ToString() + "," + android.ToString() + "," + ios.ToString() + "," + (android + ios).ToString();
        }
    }
}
