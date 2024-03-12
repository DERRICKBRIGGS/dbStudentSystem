using ClassSystemManager.model;
using ClassSystemManager.view;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace ClassSystemManager
{
    public partial class Store_form_control : Form
    {
        /// <summary>
        /// 選取的店家對應的店家ID
        /// </summary>
        private int _ID;
        private int _Time_id;
        private bool button_close = true;
        private List<Cstore_order_table> _Order_Tables;
        private List<Cstore_feedback> _Order_feedback_Tables;
        private List<C_store> _C_store_list;
        private string 表單狀態;
        private bool feedback_star_desc = false;
        private bool time_all_day = true;
        public Store_form_control()
        {
            InitializeComponent();
        }

        private void Store_form_control_Load(object sender, EventArgs e)
        {
            show_table();
            textBox1.BackColor = Color.FromArgb(184, 184, 220);
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.Add("完成");
            toolStripComboBox1.Items.Add("進行中");
            toolStripComboBox1.Items.Add("取消");
            comboBox1.Items.Clear();
            for (int i = 1; i < 13; i++) 
            {
                string a = i.ToString("00");
                comboBox1.Items.Add(a + " 月 ");
            }
        }
        private void show_order_table() 
        {
            表單狀態 = "訂單明細";
            Cstore_order_table a = new Cstore_order_table();
            _Order_Tables = a.gettable(_ID);
            dataGridView4.DataSource = _Order_Tables;
        }
        private void show_order_feedback_table()
        {
            表單狀態 = "意見回饋";
            Cstore_feedback a = new Cstore_feedback();
            _Order_feedback_Tables=a.getdata(_ID);
            dataGridView4.DataSource = _Order_feedback_Tables;
        }
        private void show_table()
        {
            C_store result = new C_store();
            _C_store_list= result.get_table();
            dataGridView1.DataSource = _C_store_list;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            insert_store();
            show_table();
        }
        /// <summary>
        /// 透過insert_store_form視窗回傳新增店鋪資料
        /// </summary>
        private static void insert_store()
        {
            insert_store form = new insert_store();
            //C_store result = new C_store();
            //List<C_store> stores = new C_store().get_table();
            form.ShowDialog();
            if (form.isOk == DialogResult.OK)
            {
                order_meal_systemEntities db = new order_meal_systemEntities();
                t訂餐_店家資料表 a = form.data;
                var store_name_list = from aa in db.t訂餐_店家資料表
                                      select aa.店家名稱;
                //foreach (C_store store in stores) 
                //{
                //    if (store.名稱 == form.data.店家名稱)
                //    {
                //        MessageBox.Show($"新增失敗，店家名稱 {form.data.店家名稱} 已存在");
                //        return;
                //    }
                //}
                //檢查店家名稱是否重複
                foreach (string x in store_name_list)
                {
                    if (form.data.店家名稱 == x)
                    {
                        MessageBox.Show($"新增失敗，店家名稱 {form.data.店家名稱} 已存在");
                        return;
                    }
                }
                db.t訂餐_店家資料表.Add(a);
                db.SaveChanges();
                //店家口味標籤新增
                if (!String.IsNullOrEmpty(form.title))
                {
                    int id = a.店家ID;
                    var title = form.title.Split(',');
                    foreach (var t in title)
                    {
                        var x = db.t訂餐_口味總表.Where(ab => ab.風味名稱 == t);
                        foreach (var y in x)
                        {
                            t訂餐_店家風味表 store_style = new t訂餐_店家風味表();
                            store_style.店家ID = id;
                            store_style.口味ID = y.口味ID;
                            db.t訂餐_店家風味表.Add(store_style);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定店家");
                return;
            }
            delete_store();
            show_table();
        }
        /// <summary>
        /// 透過_ID選定的店家
        /// 刪除店家資料連帶先刪除子表單，將營業時間、餐點資訊表與店家風味表一併刪除
        /// </summary>
        private void delete_store()
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var remove_store = db.t訂餐_店家資料表.FirstOrDefault(a => a.店家ID == _ID);
            var remove_store_time = db.t訂餐_營業時間表.Where(a => a.店家ID == _ID);
            var remove_store_meal = db.t訂餐_餐點資訊表.Where(a => a.店家ID == _ID);
            var remove_store_style = db.t訂餐_店家風味表.Where(a => a.店家ID == _ID);
            var remove_store_order = db.t訂餐_訂單詳細資訊表.Where(a => a.店家ID == _ID);
            if (MessageBox.Show("會連帶刪除菜單與營業資料都刪除", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (MessageBox.Show("確認刪除嗎?", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //todo 寫交易包起來
                    using (var dbContext = new order_meal_systemEntities()) 
                    {
                        using (var transaction = dbContext.Database.BeginTransaction()) 
                        {
                            try 
                            {
                                foreach (var a in remove_store_time)
                                {
                                    db.t訂餐_營業時間表.Remove(a);
                                }
                                foreach (var a in remove_store_meal)
                                {
                                    db.t訂餐_餐點資訊表.Remove(a);
                                }
                                foreach (var a in remove_store_style)
                                {
                                    db.t訂餐_店家風味表.Remove(a);
                                }
                                foreach (var a in remove_store_order) 
                                {
                                    db.t訂餐_訂單詳細資訊表.Remove(a);
                                }
                                db.t訂餐_店家資料表.Remove(remove_store);
                                db.SaveChanges();
                            }
                            catch (Exception ex) 
                            {
                                transaction.Rollback();
                                MessageBox.Show(""+ex);
                            }
                        }
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定店家");
                return;
            }
            order_meal_systemEntities db = new order_meal_systemEntities();
            t訂餐_店家資料表 store = db.t訂餐_店家資料表.FirstOrDefault(a => a.店家ID == _ID);
            string result;
            List<string> list;
            update_store_original_tags(db, out result, out list); //存取原先商店的標籤資訊
            insert_store form = new insert_store();
            form.data = store;
            form.title = result;
            form.ShowDialog();
            if (form.isOk == DialogResult.OK)
            {
                store.店家名稱 = form.data.店家名稱;
                store.地址 = form.data.地址;
                store.電話 = form.data.電話;
                store.餐廳介紹 = form.data.餐廳介紹;
                store.餐廳照片 = form.data.餐廳照片;
                db.SaveChanges();
                update_store_chick_tag(db, list, form);  //檢查變更後的標籤資訊是否要刪除或新增
                db.SaveChanges();
                show_table();
            }
        }
        /// <summary>
        /// 存取原先商店的標籤資訊
        /// </summary>
        /// <param name="db">資料庫模型資料</param>
        /// <param name="result">原先有的標籤名字，放入insert_store_form中做修改呈現</param>
        /// <param name="list">將字串存成List讓後續修改後比對將有取消的從資料庫刪除</param>
        private void update_store_original_tags(order_meal_systemEntities db, out string result, out List<string> list)
        {
            result = "";
            var style = from style_01 in db.t訂餐_店家風味表
                        join b in db.t訂餐_口味總表
                        on style_01.口味ID equals b.口味ID
                        where style_01.店家ID == _ID
                        select b.風味名稱;
            foreach (var sty in style)
            {
                result += sty;
                result += ",";
            }
            list = style.ToList();
        }
        /// <summary>
        /// 檢查變更後的標籤資訊是否要刪除或新增
        /// </summary>
        /// <param name="db">資料庫模型資料</param>
        /// <param name="store"></param>
        /// <param name="list">原先店鋪的標籤資訊，修改後比對將有取消的從資料庫刪除</param>
        /// <param name="form">存取insert_store_form拋出的修改資料</param>
        private void update_store_chick_tag(order_meal_systemEntities db, List<string> list, insert_store form)
        {
            var title = form.title.Split(',');
            foreach (var t in title)
            {
                list.Remove(t);
                var x = db.t訂餐_口味總表.Where(ab => ab.風味名稱 == t);
                foreach (var y in x)
                {
                    t訂餐_店家風味表 store_style = new t訂餐_店家風味表();
                    store_style.店家ID = _ID;
                    store_style.口味ID = y.口味ID;
                    //將勾選的標籤在加入前檢查是否本來就有，有則跳出下一個，不重複加入
                    if (db.t訂餐_店家風味表.FirstOrDefault(a => a.店家ID == store_style.店家ID && a.口味ID == store_style.口味ID) != null)
                    {
                        continue;
                    }
                    db.t訂餐_店家風味表.Add(store_style);
                }
            }
            //刪除原本有但後來取消掉的標籤
            foreach (string x in list)
            {
                var x1 = db.t訂餐_口味總表.FirstOrDefault(ab => ab.風味名稱 == x);
                var style_remove = db.t訂餐_店家風味表.FirstOrDefault(a => a.店家ID == _ID && a.口味ID == x1.口味ID);
                db.t訂餐_店家風味表.Remove(style_remove);
            }
        }
        //todo RowEnter
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            order_meal_systemEntities db = new order_meal_systemEntities();
            //var storelist = (from a in db.t訂餐_店家資料表
            //                 select a).ToList();
            List<C_store> table= dataGridView1.DataSource as List<C_store>;
            C_store store = table[e.RowIndex];
            _ID = store.店家ID;
            show_data(db);
            show_data_time(db);
            show_order_table();
            chart_date_order_toal(comboBox1.Text);
        }

        private void show_data(order_meal_systemEntities db)
        {
            var mealtable = from a in db.t訂餐_餐點資訊表
                            where a.店家ID == _ID
                            select new
                            {
                                餐點ID = a.餐點ID,
                                餐點名稱 = a.餐點名稱,
                                餐點描述 = a.餐點描述,
                                餐點定價 = a.餐點定價,
                                餐點照片 = a.餐點照片
                            };
            dataGridView2.DataSource = mealtable.ToArray();
        }
        private void show_data_time(order_meal_systemEntities db)
        {
            var mealtable = from a in db.t訂餐_營業時間表
                            where a.店家ID == _ID
                            select new
                            {
                                星期 = a.星期,
                                結束營業時間 = a.結束營業時間,
                                開始營業時間 = a.開始營業時間,
                                時段 = a.時段_早_中_晚_全_,
                            };
            dataGridView3.DataSource = mealtable.ToArray();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定店家");
                return;
            }
            insert_meal();
        }

        private void insert_meal()
        {
            insert_meal form = new insert_meal();
            form.ShowDialog();
            if (form.isOK == DialogResult.OK)
            {
                order_meal_systemEntities db = new order_meal_systemEntities();
                t訂餐_餐點資訊表 m = form.meal;
                m.店家ID = _ID;
                db.t訂餐_餐點資訊表.Add(m);
                db.SaveChanges();
                show_data(db);
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定菜單");
                return;
            }
            delete_meal();
        }

        private void delete_meal()
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            int search_ID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["餐點ID"].Value);
            if (MessageBox.Show("確認刪除嗎?", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var data = db.t訂餐_餐點資訊表.FirstOrDefault(a => a.餐點ID == search_ID);
                db.t訂餐_餐點資訊表.Remove(data);
                db.SaveChanges();
                show_data(db);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定菜單");
                return;
            }
            order_meal_systemEntities db = update_meal();
            show_data(db);
        }

        private order_meal_systemEntities update_meal()
        {
            int search_ID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["餐點ID"].Value);
            order_meal_systemEntities db = new order_meal_systemEntities();
            t訂餐_餐點資訊表 update_meal = db.t訂餐_餐點資訊表.FirstOrDefault(a => a.餐點ID == search_ID);
            insert_meal form = new insert_meal();
            form.meal = update_meal;
            form.ShowDialog();
            if (form.isOK == DialogResult.OK)
            {
                update_meal.餐點描述 = form.meal.餐點描述;
                update_meal.餐點照片 = form.meal.餐點照片;
                update_meal.餐點定價 = form.meal.餐點定價;
                update_meal.餐點名稱 = form.meal.餐點名稱;
                db.SaveChanges();
            }

            return db;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            insert_tag insert_Tag = new insert_tag();
            insert_Tag.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = textBox1.Text;
            if (result != "")
            {
                var table = new C_store().search(result);
                dataGridView1.DataSource = table;
                textBox1.Text = "";
            }
            else
                show_table();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            insert_store_time form= new insert_store_time();
            form.ShowDialog();
            if (form.isOK == DialogResult.OK)
            {
                t訂餐_營業時間表 insert_time = form.time;
                insert_time.店家ID = _ID;
                order_meal_systemEntities db = new order_meal_systemEntities();
                db.t訂餐_營業時間表.Add(insert_time);
                db.SaveChanges();
                show_data_time(db);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var datarow=db.t訂餐_營業時間表.FirstOrDefault(a=>a.營業時間表ID== _Time_id);
            insert_store_time form = new insert_store_time();
            form.time = datarow;
            form.ShowDialog();
            if (form.isOK == DialogResult.OK) 
            {
                datarow.星期 = form.time.星期;
                datarow.開始營業時間= form.time.開始營業時間;
                datarow.結束營業時間 = form.time.結束營業時間;
                datarow.時段_早_中_晚_全_ = form.time.時段_早_中_晚_全_;
                db.SaveChanges();
                MessageBox.Show("時間修改完成");
                show_data_time(db);
            }
        }

        private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            order_meal_systemEntities db = new order_meal_systemEntities();
            var storelist = (from a in db.t訂餐_營業時間表
                             where a.店家ID==_ID
                             select a).ToList();
            t訂餐_營業時間表 store = storelist[e.RowIndex];
            _Time_id = store.營業時間表ID;
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var datarow = db.t訂餐_營業時間表.FirstOrDefault(a => a.營業時間表ID == _Time_id);
            db.t訂餐_營業時間表.Remove(datarow);
            if (MessageBox.Show("是否刪除營業時間", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                db.SaveChanges();
                show_data_time(db);
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (button_close == true)
            {
                toolStripButton3.Enabled = false;
                toolStripButton7.Enabled = false;
                toolStripButton6.Enabled = false;
                button_close = false;
            }
            else 
            {
                toolStripButton3.Enabled = true;
                toolStripButton7.Enabled = true;
                toolStripButton6.Enabled = true;
                button_close = true;
            }
        }
        //todo 
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            show_order_table();
            List<Cstore_order_table> order_all=dataGridView4.DataSource as List<Cstore_order_table>;
            if (order_all != null)
            {
                var order_today = order_all.Where(a => a.訂單時間.Contains(today));
                dataGridView4.DataSource = order_today.ToList();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (time_all_day == false) 
            {
                dateTimePicker1.Value = DateTime.Now;
            }
            time_all_day=true;
           // dateTimePicker1.Value = DateTime.Now;
            show_order_table();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            show_order_table();
            List<Cstore_order_table> order_type_list1 = dataGridView4.DataSource as List<Cstore_order_table>;
            if (time_all_day == true) 
            {
                var order_type1 = order_type_list1.Where(a => a.訂單狀態.Contains(toolStripComboBox1.Text));
                dataGridView4.DataSource = order_type1.ToList();
            }
            if (time_all_day == false) 
            {
                List<Cstore_order_table> order_type_list = dataGridView4.DataSource as List<Cstore_order_table>;
                var order_type = order_type_list.Where(a => a.訂單狀態.Contains(toolStripComboBox1.Text)&&a.訂單時間.Contains(dateTimePicker1.Value.ToString("yyyyMMdd")));
                dataGridView4.DataSource = order_type.ToList();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string day = dateTimePicker1.Value.ToString("yyyyMMdd");
            if (time_all_day == false)
            {
                show_order_table();
            }
            List<Cstore_order_table> order_all = dataGridView4.DataSource as List<Cstore_order_table>;
            List<Cstore_feedback> order_feedback = dataGridView4.DataSource as List<Cstore_feedback>;
            if (order_all != null)
            {
                var order_date = _Order_Tables.Where(a => a.訂單時間.Contains(day));
                dataGridView4.DataSource = order_date.ToList();
            }
            if (order_feedback != null) 
            {
                var order_date = _Order_feedback_Tables.Where(a => a.訂單時間.Contains(day));
                dataGridView4.DataSource = order_date.ToList();
            }
            time_all_day = false;
        }
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            show_order_feedback_table();
        }

        private void dataGridView4_DataSourceChanged(object sender, EventArgs e)
        {
            List<Cstore_order_table> order_all = dataGridView4.DataSource as List<Cstore_order_table>;
            List<Cstore_feedback> order_feedback = dataGridView4.DataSource as List<Cstore_feedback>;
            controler_closeOropen();
            if (order_all != null)
            {
                ordercount_and_avg(order_all);
                chart_order_analyz();
            }
            if (order_feedback != null)
            {
                feedback_count_avg(order_feedback);
            }
        }

        private void chart_date_order_toal(string mon) //mon = xx月
        {
            chart3.ChartAreas[0].AxisX.Title = "日期";
            chart3.ChartAreas[0].AxisY.Title = "銷售額";
            chart3.Titles[0].Text = $"{mon}銷售分布";
            chart3.Series["訂單總額"].IsVisibleInLegend = false;
            chart3.ChartAreas[0].AxisX.CustomLabels.Clear();
            chart3.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = from item in db.t訂餐_訂單詳細資訊表
                         join a in db.t訂餐_餐點資訊表 on item.餐點ID equals a.餐點ID
                         join b in db.t訂餐_訂單資訊表 on item.訂單ID equals b.訂單ID
                         where item.店家ID == _ID&& b.訂單狀態.Contains("完成")//選擇店家ID
                         group item.金額小記 by
                         b.訂單時間.Substring(4, 4)
                         into grouped
                         select new
                         {
                             日期 = grouped.Key,
                             訂單總額 = grouped.Sum(item => item.Value) //加總金額
                         };
            chart3.Series["訂單總額"].Points.Clear();

            foreach (var item in result)
            {
                if (item.日期.Substring(0, 2) == mon.Substring(0,2)) //判斷月份
                {
                    chart3.Series[0].Enabled=true;
                    chart3.Series["訂單總額"].Points.AddXY(Convert.ToInt32(item.日期.Substring(2, 2)), item.訂單總額); //日期做總匯出
                }
            }
            if (chart3.Series[0].Points.Count == 0)
            {
                chart3.Series[0].Enabled = false;
                label5.Visible = true;
            }
            else 
            {
                chart3.Series[0].Enabled = true;
                label5.Visible = false;
            }

        }

        /// <summary>
        /// 依照店家ID去分析每個菜單的訂單數量
        /// </summary>
        private void chart_order_analyz()
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = from item in db.t訂餐_訂單詳細資訊表
                         join a in db.t訂餐_餐點資訊表 on item.餐點ID equals a.餐點ID
                         where item.店家ID == _ID
                         group item.訂單ID by
                         a.餐點名稱 into grouped
                         select new
                         {
                             餐點名稱 = grouped.Key,
                             總餐點數量 = grouped.Count()
                         };
            chart1.Series["Series"].Points.Clear();
            foreach (var x in result)
            {
                chart1.Series["Series"].Points.AddXY(x.餐點名稱, x.總餐點數量);
            }
            chart1.Series["Series"]["PieLabelStyle"] = "Disabled";
            chart1.Legends[0].BackColor = Color.Transparent;
            chart1.Legends[0].Font = new Font("Calisto MT", 10, FontStyle.Bold);
            chart1.Series["Series"].IsValueShownAsLabel = false;
            chart1.Series["Series"].IsVisibleInLegend = true;
        }

        private void feedback_count_avg(List<Cstore_feedback> order_feedback)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("評論數");
            dt.Columns.Add("平均星數");
            DataRow dataRow = dt.NewRow();
            int start = 0;
            for (int i = 0; i < order_feedback.Count; i++)
            {
                start += Convert.ToInt32(order_feedback[i].評價星數);
            }
            dataRow["評論數"] = order_feedback.Count;
            if (order_feedback.Count == 0)
            {
                dataRow["平均星數"] = 0;
            }
            else
            {
                dataRow["平均星數"] = start / order_feedback.Count;
            }
            dt.Rows.Add(dataRow);
            dataGridView5.DataSource = dt;
        }

        private void ordercount_and_avg(List<Cstore_order_table> order_all)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("總金額");
            dt.Columns.Add("訂單總數");
            dt.Columns.Add("每筆訂單平均");
            DataRow dataRow = dt.NewRow();
            int price = 0;
            for (int i = 0; i < order_all.Count; i++)
            {
                price += Convert.ToInt32(order_all[i].金額小記);
            }
            dataRow["總金額"] = price;
            dataRow["訂單總數"] = order_all.Count;
            if (order_all.Count != 0)
                dataRow["每筆訂單平均"] = price / order_all.Count;
            else
                dataRow["每筆訂單平均"] = 0;
            dt.Rows.Add(dataRow);
            dataGridView5.DataSource = dt;
        }

        private void controler_closeOropen()
        {
            if (表單狀態 == "意見回饋")
            {
                toolStripComboBox1.Enabled = false;
                toolStripButton12.Enabled = true;
            }
            else
            {
                toolStripComboBox1.Enabled = true;
                toolStripButton12.Enabled = false;
            }
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            List<Cstore_feedback> order_feedback = dataGridView4.DataSource as List<Cstore_feedback>;
            if (order_feedback!=null)
            {
                if (feedback_star_desc == false)
                {
                    var order_star_list = order_feedback.OrderBy(a => a.評價星數);
                    dataGridView4.DataSource = order_star_list.ToList();
                    feedback_star_desc = true;
                }
                else 
                {
                    var order_star_list = order_feedback.OrderByDescending(a => a.評價星數);
                    dataGridView4.DataSource = order_star_list.ToList();
                    feedback_star_desc = false;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart_date_order_toal(comboBox1.Text);
        }
    }
}
