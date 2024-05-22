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
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.Security.Cryptography;

namespace AUH_Data_viewing_tools
{
    public partial class Form1 : Form
    {
        static List<List<int>> ecgdata = new List<List<int>>();
        static List<List<int>> gsrdata = new List<List<int>>();
        String FileName;
        public Form1()
        {
            InitializeComponent();

            // 設定ChartArea
            ChartArea chtArea = new ChartArea("ViewArea");
            chtArea.AxisX.Minimum = 0; //X軸數值從0開始
            chtArea.AxisX.ScaleView.Size = 1000; //設定視窗範圍內一開始顯示多少點
            chtArea.AxisX.Interval = 100;
            chtArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chtArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All; //設定scrollbar
            chart1.ChartAreas[0] = chtArea; // chart new 出來時就有內建第一個chartarea
            //chart2.ChartAreas[0] = chtArea;

        }

        private void enable_element()
        {
            comboBox1.Enabled = true;

            for (int i = 0; i < ecgdata.Count; i++)
            {
                comboBox1.Items.Add(i);
            }

            output_csv.Enabled = true;
        }

        public void import_data1(string filepath, string datatype)
        {
            String s1 = "Sensor Data:";
            String s2 = "GSR Data:";
            String ecgline;
            String gsrline;
            int[] ecgInts;
            int[] gsrInts;
            String[] ecgnumber;
            String[] gsrnumber;

            StreamReader ecgrdata = new StreamReader(filepath + "\\ECG" + datatype);
            StreamReader gsrrdata = new StreamReader(filepath + "\\GSR" + datatype);

            while (ecgrdata.ReadLine() != null)
            {
                ecgline = ecgrdata.ReadLine();
                gsrline = gsrrdata.ReadLine();

                if (ecgline != null)
                {
                    if (ecgline.Contains(s1) || gsrline.Contains(s2))
                    {
                        ecgnumber = ecgline.Remove(0, 13).TrimEnd(' ').Split(new char[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
                        gsrnumber = gsrline.Remove(0, 9).TrimEnd(' ').Split(new char[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        ecgnumber = ecgline.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
                        gsrnumber = gsrline.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
                    }
                  
                    ecgInts = Array.ConvertAll(ecgnumber, int.Parse);
                    gsrInts = Array.ConvertAll(gsrnumber, int.Parse);
                    
                    ecgdata.Add(ecgInts.ToList());
                    gsrdata.Add(gsrInts.ToList());
                }
                
            }
            ecgrdata.Close();
            gsrrdata.Close();
        }
        private void import_data_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ecgdata.Clear();
                gsrdata.Clear();
                comboBox1.Items.Clear();
                String[] datanum;
                String filepath = folderBrowserDialog1.SelectedPath;

                datanum = filepath.Split('\\');
                FileName = datanum[datanum.Length - 1];
                label4.Text = FileName;
                
                if (File.Exists(filepath + "\\ECG.txt"))
                {
                    import_data1(filepath, ".txt");
                    enable_element();

                }
                else if (File.Exists(filepath + "\\ECG.csv"))
                {
                    import_data1(filepath, ".csv");
                    enable_element();
                }

                else if(File.Exists(filepath + "\\ECG.csv") && File.Exists(filepath + "\\ECG.txt"))
                {
                    import_data1(filepath, ".csv");
                    enable_element(); 
                }
                else
                {
                    MessageBox.Show("無法讀取到檔案!!", "提示", MessageBoxButtons.OK);
                }
            }
        }
        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void output_csv_Click(object sender, EventArgs e)
        {
            String savepath = "";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                savepath = folderBrowserDialog1.SelectedPath;

                if (Directory.Exists(savepath + "\\" +FileName) == false)
                {
                    Directory.CreateDirectory(savepath + "\\" + FileName);
                }
                String ecgFilePath = savepath + "\\" + FileName + "\\ECG.csv";
                String gsrFilePath = savepath + "\\" + FileName + "\\GSR.csv";

                StreamWriter writer = new StreamWriter(ecgFilePath, false, Encoding.UTF8);
                foreach (var line in ecgdata)
                {
                    string lines = String.Join(",", line);
                    writer.WriteLine(lines);
                }
                writer.Close();

                StreamWriter writer1 = new StreamWriter(gsrFilePath, false, Encoding.UTF8);
                foreach (var line in gsrdata)
                {
                    string lines = String.Join(",", line);
                    writer1.WriteLine(lines);
                }
                writer1.Close();
                MessageBox.Show("匯出成功", "提示", MessageBoxButtons.OK);
            }

            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = savepath;
            prc.Start();

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Int32.Parse(comboBox1.Text);
           
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();

            chart1.ChartAreas[0].AxisY.Minimum = ecgdata[index].Min();
            chart1.ChartAreas[0].AxisY.Maximum = ecgdata[index].Max()+10;

            chart2.ChartAreas[0].AxisY.Minimum = gsrdata[index].Min();
            chart2.ChartAreas[0].AxisY.Maximum = gsrdata[index].Max()+10; 

            for (int i = 0; i < ecgdata[index].Count; i++)
            {
                
                chart1.Series[0].Points.Add(ecgdata[index][i]);
            }
            for (int i = 0; i < gsrdata[index].Count; i++)
            {
                chart2.Series[0].Points.Add(gsrdata[index][i]);
            }
        }
        
    }
}
