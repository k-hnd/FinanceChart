using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;
using System.Xml.Linq;

namespace Finance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // generate URL
            string brand = comboBox1.Text;
            string fromDate = comboBox2.Text + "-" + comboBox3.Text + "-" + comboBox4.Text;
            string toDate = comboBox7.Text + "-" + comboBox6.Text + "-" + comboBox5.Text;
            string url = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.historicaldata%20where%20symbol%20%3D%20%22"
                + brand + "%22%20and%20startDate%20%3D%20%22" 
                + fromDate + "%22%20and%20endDate%20%3D%20%22" 
                + toDate + "%22&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

            // get and parse XML
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string xml = wc.DownloadString(url);
            XDocument doc = XDocument.Parse(xml);

            // set chart data
            Series chartSetting = new Series();
            chartSetting.ChartType = SeriesChartType.Line;
            var quotes = doc.Descendants("quote");
            foreach (var quote in quotes)
                chartSetting.Points.AddXY(quote.Element("Date").Value, quote.Element("Close").Value);

            chart1.Series.Clear();
            chart1.Series.Add(chartSetting);
            chart1.ChartAreas[0].AxisX.IsReversed = true;

        }
    }
}
