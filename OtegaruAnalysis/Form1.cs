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

        public Form1()
        {
            InitializeComponent();
            label1.Text = "open json file!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadJson();
            label1.Text = GetUnixTime(DateTime.Now).ToString();
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
            List<OtegaruData> data = JsonSerializer.Deserialize<List<OtegaruData>>(json);

            textBox1.Text += data.Count().ToString();
        }

        private void ExportFile()
        {
            File.WriteAllText(@"C:\Users\ryk\Desktop\test.txt", "Good morning!");
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

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
    }

    public class OtegaruData
    {
        public int coin { get; set; }
        public float created_at { get; set; }
        public string os { get; set; }
    }

    public class AnalysisData
    {
        int day;
        int android, ios, sumcoin;

        public void Analysis(int d)
        {
            day = d;
            android = 0;
            ios = 0;
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

        public void AddCoin(int c)
        {
            sumcoin += c;
        }

        public int GetAverageCoin()
        {
            return sumcoin / (android + ios);
        }
    }
}
