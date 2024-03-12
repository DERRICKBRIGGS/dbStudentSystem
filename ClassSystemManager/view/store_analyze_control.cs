using ClassSystemManager.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClassSystemManager.view
{
    public partial class store_analyze_control : Form
    {
        public store_analyze_control()
        {
            InitializeComponent();
        }

        private void store_analyze_control_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Title = "月份";
            chart1.ChartAreas[0].AxisY.Title = "銷售";
            chart1.ChartAreas[0].AxisX.CustomLabels.Clear();
            //chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Maximum = 32;
            //chart1.Update();
            //chart1.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            //for (int i = 1; i <= 31; i++)
            //{
            //    chart1.ChartAreas[0].AxisX.CustomLabels.Add(
            //        i - 0.5, // 標籤的位置（在刻度中心）
            //        i + 0.5, // 標籤的結束位置
            //        i.ToString()); // 標籤的文字
            //}
            chart1.Series["商店"].Points.Clear();
            chart1.Series["商店"].Points.AddXY(12, 15);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = from item in db.t訂餐_訂單詳細資訊表
                         join a in db.t訂餐_餐點資訊表 on item.餐點ID equals a.餐點ID
                         join b in db.t訂餐_訂單資訊表 on item.訂單ID equals b.訂單ID
                         where item.店家ID == 10 //選擇店家ID
                         group item.金額小記 by
                         b.訂單時間.Substring(4, 4) into grouped
                         select new
                         {
                             日期 = grouped.Key,
                             訂單總額 = grouped.Sum(item => item.Value) //加總金額
                         };
            chart1.Series["商店"].Points.Clear();
            foreach (var item in result)
            {
                if (item.日期.Substring(0, 2) == "05") //判斷月份
                {
                    chart1.Series["商店"].Points.AddXY(Convert.ToInt32(item.日期.Substring(2, 2)), item.訂單總額); //日期做總匯出
                }
            }
            label1.Text=(chart1.Series[0].Points.Count).ToString();
            //chart1.Update();
            dataGridView1.DataSource = result.ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = from item in db.t訂餐_訂單詳細資訊表
                         join a in db.t訂餐_餐點資訊表 on item.餐點ID equals a.餐點ID
                         join b in db.t訂餐_訂單資訊表 on item.訂單ID equals b.訂單ID
                         where item.店家ID == 10 //選擇店家ID
                         group item.金額小記 by
                         b.訂單時間.Substring (4,4)
                         into grouped
                         select new
                         {
                             日期 = grouped.Key,
                             訂單總額 = grouped.Sum(item=>item.Value) //加總金額
                         };
            chart1.Series["商店"].Points.Clear();
            foreach (var item in result) 
            {
                if (item.日期.Substring(0, 2) == "12") //判斷月份
                {
                    chart1.Series["商店"].Points.AddXY(Convert.ToInt32(item.日期.Substring(2,2)), item.訂單總額); //日期做總匯出
                }
            }
            //chart1.Update();
            dataGridView1.DataSource = result.ToArray();
        }
    }
}
