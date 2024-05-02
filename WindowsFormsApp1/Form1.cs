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

namespace WindowsFormsApp1
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
            
        }
        private void import_data_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ecgdata.Clear();
                gsrdata.Clear();
                comboBox1.Items.Clear();

                String ecgline;
                String gsrline;
                String[] ecgnumber;
                String[] gsrnumber;
                String[] datanum;
                int[] ecgInts;
                int[] gsrInts;
                String filepath = folderBrowserDialog1.SelectedPath;

                String s1 = "Sensor Data:";
                String s2 = "GSR Data:";

                datanum = filepath.Split('\\');
                FileName = datanum[datanum.Length - 1];
                label4.Text = FileName;
                

                if (File.Exists(filepath + "\\ECG.txt"))
                {
                    StreamReader ecgrdata = new StreamReader(filepath + "\\ECG.txt");
                    StreamReader gsrrdata = new StreamReader(filepath + "\\GSR.txt");

                    ecgline = ecgrdata.ReadLine();
                    gsrline = gsrrdata.ReadLine();

                    if(ecgline.Contains(s1) || gsrline.Contains(s2))
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

                    /*for (int i = 0; i < gsrInts.Length; i++)
                    {
                        chart2.Series[0].Points.Add(gsrInts[i]);
                    }*/

                    while (ecgline != null)
                    {
                        ecgline = ecgrdata.ReadLine();
                        if (ecgline != null)
                        {
                            ecgnumber = ecgline.Remove(0, 13).TrimEnd(' ').Split(new char[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
                        }
                        ecgInts = Array.ConvertAll(ecgnumber, int.Parse);
                        ecgdata.Add(ecgInts.ToList());
                    }
                    while (gsrline != null)
                    {
                        gsrline = gsrrdata.ReadLine();
                        if (gsrline != null)
                        {
                            gsrnumber = gsrline.Remove(0, 9).TrimEnd(' ').Split(new char[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
                        }
                        gsrInts = Array.ConvertAll(gsrnumber, int.Parse);
                        gsrdata.Add(gsrInts.ToList());
                    }

                    ecgrdata.Close();
                    gsrrdata.Close();

                }
                else if (File.Exists(filepath + "\\ECG.csv"))
                {
                    StreamReader ecgrdata = new StreamReader(filepath + "\\ECG.csv");
                    StreamReader gsrrdata = new StreamReader(filepath + "\\GSR.csv");

                    ecgline = ecgrdata.ReadLine();
                    gsrline = gsrrdata.ReadLine();

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

                    /*for (int i = 0; i < gsrInts.Length; i++)
                    {
                        chart2.Series[0].Points.Add(gsrInts[i]);
                    }*/

                    while (ecgline != null)
                    {
                        ecgline = ecgrdata.ReadLine();
                        if (ecgline != null)
                        {
                            ecgnumber = ecgline.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
                        }
                        ecgInts = Array.ConvertAll(ecgnumber, int.Parse);
                        ecgdata.Add(ecgInts.ToList());
                    }
                    while (gsrline != null)
                    {
                        gsrline = gsrrdata.ReadLine();
                        if (gsrline != null)
                        {
                            gsrnumber = gsrline.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
                        }
                        gsrInts = Array.ConvertAll(gsrnumber, int.Parse);
                        gsrdata.Add(gsrInts.ToList());
                    }

                    ecgrdata.Close();
                    gsrrdata.Close();
                }

                else if(File.Exists(filepath + "\\ECG.csv") && File.Exists(filepath + "\\ECG.txt"))
                {
                    StreamReader ecgrdata = new StreamReader(filepath + "\\ECG.csv");
                    StreamReader gsrrdata = new StreamReader(filepath + "\\GSR.csv");

                    ecgline = ecgrdata.ReadLine();
                    gsrline = gsrrdata.ReadLine();

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

                    /*for (int i = 0; i < gsrInts.Length; i++)
                    {
                        chart2.Series[0].Points.Add(gsrInts[i]);
                    }*/

                    while (ecgline != null)
                    {
                        ecgline = ecgrdata.ReadLine();
                        if (ecgline != null)
                        {
                            ecgnumber = ecgline.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
                        }
                        ecgInts = Array.ConvertAll(ecgnumber, int.Parse);
                        ecgdata.Add(ecgInts.ToList());
                    }
                    while (gsrline != null)
                    {
                        gsrline = gsrrdata.ReadLine();
                        if (gsrline != null)
                        {
                            gsrnumber = gsrline.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
                        }
                        gsrInts = Array.ConvertAll(gsrnumber, int.Parse);
                        gsrdata.Add(gsrInts.ToList());
                    }

                    ecgrdata.Close();
                    gsrrdata.Close();
                }

                for (int i = 0; i < ecgdata.Count; i++)
                {
                    comboBox1.Items.Add(i);
                }
                output_csv.Enabled = true;
            }

        }
        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void output_csv_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(FileName) == false)
            {
                Directory.CreateDirectory(FileName);
            }
            String ecgFilePath = FileName + "\\ECG.csv";
            String gsrFilePath = FileName + "\\GSR.csv";

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

            //MessageBox.Show("待更新", "提示", MessageBoxButtons.OK);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Int32.Parse(comboBox1.Text);
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();

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
