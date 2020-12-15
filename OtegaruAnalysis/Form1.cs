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


namespace OtegaruAnalysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "hello";
            LoadJson();
        }

        private void LoadJson()
        {
            string json = "[{\"os\":\"android\",\"coin\":230,\"created_at\":1607746294014},{\"coin\":1050,\"created_at\":1608013732727,\"os\":\"android\"}]";
            List<OtegaruData> data = JsonSerializer.Deserialize<List<OtegaruData>>(json);

            textBox1.Text += data.Count().ToString();
        }

        private void ExportFile()
        {
            File.WriteAllText(@"C:\Users\ryk\Desktop\test.txt", "Good morning!");
        }
    }

    public class OtegaruData
    {
        public int coin { get; set; }
        public float created_at { get; set; }
        public string os { get; set; }
    }
}
