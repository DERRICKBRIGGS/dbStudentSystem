using ClassSystemManager.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassSystemManager.view
{
    public partial class order_form_control : Form
    {
        private bool _open_panel = false;
        private int _ID;
        private 總訂單資料 _Order_information = new 總訂單資料();
        private 子訂單詳細資料 _Order_detail_information= new 子訂單詳細資料();
        private 評論排序資料 _Order_feedback_start=new 評論排序資料();
        public order_form_control()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 開關收合案紐
        /// </summary>

        private void order_form_control_Load(object sender, EventArgs e)
        {
            panel1.Visible = _open_panel;
            combox_load();
            show_ordertable();
            dataGridView4.DataSource = _Order_feedback_start.gettable_feedback();
        }
        /// <summary>
        /// 讓combox做初始資料新增
        /// </summary>
        private void combox_load()
        {
            toolStripComboBox2.Items.Clear();
            toolStripComboBox2.Items.Add("取消");
            toolStripComboBox2.Items.Add("進行中");
            toolStripComboBox2.Items.Add("完成");
            toolStripComboBox1.Items.Clear();
            for (int i = 1; i < 6; i++)
            {
                toolStripComboBox1.Items.Add(i.ToString());
            }
            comboBox1.Items.Clear();
            comboBox1.Items.Add("全部");
            comboBox1.Items.Add("取消");
            comboBox1.Items.Add("進行中");
            comboBox1.Items.Add("完成");
        }

        /// <summary>
        /// 呈現訂單主要表
        /// </summary>
        private void show_ordertable()
        {
            List<總訂單資料> datatable= _Order_information.gettable();
            dataGridView1.DataSource = datatable;
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            order_meal_systemEntities db = new order_meal_systemEntities();
            List<總訂單資料> datatable = dataGridView1.DataSource as List<總訂單資料>;
            int id = datatable[e.RowIndex].訂單ID;
            _ID = id;
            control_feedback_table(db);
            show_orderdeatal(_ID);
            // todo 店家評論平均
            var store_information = new 子訂單商家資料().gettable(_ID);
            dataGridView3.DataSource = store_information.ToList();
        }

        private void control_feedback_table(order_meal_systemEntities db)
        {
            var feedback_control = db.t訂餐_訂單資訊表.FirstOrDefault(a => a.訂單ID == _ID);
            if (feedback_control.訂單狀態.ToString().Trim() != "完成")
            {
                toolStripButton5.Enabled = false;
                toolStripButton1.Enabled = false;
                toolStripButton6.Enabled = false;
                feedback_clear();
            }
            else
            {
                chick_onlyone_feedback(db);
            }
            if (feedback_control.訂單狀態.ToString().Trim() == "完成")
            {
                toolStripButton2.Enabled = false;
            }
            else
            {
                toolStripButton2.Enabled = true;
            }
        }

        /// <summary>
        /// 確認只有一筆大訂單只有一個評論
        /// </summary>
        private void chick_onlyone_feedback(order_meal_systemEntities db)
        {
            var feedback = db.t訂餐_評論表.FirstOrDefault(a => a.訂單ID == _ID);
            if (feedback != null)
            {
                show_order_feedback(feedback);
            }
            else
            {
                toolStripButton5.Enabled = true;
                feedback_clear();
            }
        }
        /// <summary>
        /// 已經有評論下關閉新增，只打開編輯與刪除
        /// </summary>
        private void show_order_feedback(t訂餐_評論表 feedback)
        {
            toolStripButton5.Enabled = false;
            toolStripButton1.Enabled = true;
            toolStripButton6.Enabled = true;
            textBox1.Text = feedback.評論;
            toolStripComboBox1.Text = feedback.滿意度_星數_;
            progressBar1.Value = Convert.ToInt32(feedback.滿意度_星數_);
        }

        private void feedback_clear()
        {
            textBox1.Text = "(此處寫意見回饋)";
            toolStripComboBox1.Text = "滿意度(1~5)";
            progressBar1.Value = 0;
        }

        /// <summary>
        /// 呈現詳細訂單的資訊與加總
        /// </summary>
        /// <param name="db">資料庫名稱</param>
        private void show_orderdeatal(int ID)
        {
            var result = _Order_detail_information.gettable(ID);
            dataGridView2.DataSource = result;
        }

        private void Orderdelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定訂單");
                return;
            }
            order_meal_systemEntities db = new order_meal_systemEntities();
            var remove_order = db.t訂餐_訂單資訊表.FirstOrDefault(a => a.訂單ID == _ID);
            var remove_order_deatial = db.t訂餐_訂單詳細資訊表.Where(a => a.訂單ID == _ID);// list
            var remove_order_feedback = db.t訂餐_評論表.FirstOrDefault(a => a.訂單ID == _ID);
            if (MessageBox.Show("會連帶刪除詳細訂單與該筆評論都刪除", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (MessageBox.Show("確認刪除嗎?", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (var a in remove_order_deatial)
                    {
                        db.t訂餐_訂單詳細資訊表.Remove(a);
                    }
                    if (remove_order_feedback != null){ db.t訂餐_評論表.Remove(remove_order_feedback); }
                    db.t訂餐_訂單資訊表.Remove(remove_order);
                    db.SaveChanges();
                }
            }
            show_ordertable();
            feedback_clear();
        }
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = Convert.ToInt32(toolStripComboBox1.Text);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (Chick_tool.chickstring_only_math(toolStripComboBox1.Text))
            {
                if (textBox1.Text == "(此處寫意見回饋)") { textBox1.Text = null; }
                order_meal_systemEntities db = new order_meal_systemEntities();
                t訂餐_評論表 feedback = new t訂餐_評論表();
                feedback.訂單ID = _ID;
                feedback.評論 = textBox1.Text;
                feedback.滿意度_星數_ = toolStripComboBox1.Text;
                db.t訂餐_評論表.Add(feedback);
                db.SaveChanges();
                toolStripButton5.Enabled = false;
                toolStripButton1.Enabled = true;
                toolStripButton6.Enabled = true;
            }
            else { MessageBox.Show("記得選取滿意度"); }
        }

        private void writefeedback_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "(此處寫意見回饋)") { textBox1.Text = ""; }
        }

        private void updatefeedbackButton6_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var feedback = db.t訂餐_評論表.FirstOrDefault(a => a.訂單ID == _ID);
            if (MessageBox.Show("是否重新編輯評論", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                feedback.評論 = textBox1.Text;
                feedback.滿意度_星數_ = toolStripComboBox1.Text;
                db.SaveChanges();
            }
            show_order_feedback(feedback);
        }

        private void detailfeedbackButton1_Click_1(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var feedback = db.t訂餐_評論表.FirstOrDefault(a => a.訂單ID == _ID);
            if (MessageBox.Show("是否刪除此評論", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.t訂餐_評論表.Remove(feedback);
                db.SaveChanges();
                toolStripButton5.Enabled = true;
                toolStripButton1.Enabled = false;
                toolStripButton6.Enabled = false;
                feedback_clear();
            }
            else { show_order_feedback(feedback); }
        }

        private void update_order_typeButton2_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var order = db.t訂餐_訂單資訊表.FirstOrDefault(a => a.訂單ID == _ID);
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定訂單");
                return;
            }
            order.訂單狀態 = toolStripComboBox2.Text;
            db.SaveChanges();
            show_ordertable();
            toolStripComboBox2.Text = "";
        }

        private void insert_order_and_orderdeatil_Button4_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            insert_order insert_Order = new insert_order();
            insert_Order.ShowDialog();
            if (insert_Order.isOK == DialogResult.OK)
            {
                t訂餐_訂單資訊表 table_order = insert_Order.c_Order;
                db.t訂餐_訂單資訊表.Add(table_order);
                db.SaveChanges();
                List<t訂餐_訂單詳細資訊表> orderslist = insert_Order.c_Order_deatal;
                foreach (t訂餐_訂單詳細資訊表 order in orderslist)
                {
                    order.訂單ID = table_order.訂單ID;
                    db.t訂餐_訂單詳細資訊表.Add(order);
                    db.SaveChanges();
                }
            }
            show_ordertable();
            show_orderdeatal(_ID);
        }
        private void search_all_Click(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            List<總訂單資料> datatable = _Order_information.gettable();
            string timeday = dateTimePicker1.Value.ToString("yyyyMMdd");          
            search_function(sender, e, datatable, timeday);
        }

        private void search_function(object sender, EventArgs e, List<總訂單資料> datatable, string timeday)
        {
            if (timeday == "19950123") //不指定日期
            {
                if (comboBox1.Text == "全部" && textBox2.Text == "") //沒有指定資料 
                {
                    clear_search_Click(sender, e);
                }
                else
                {
                    if (textBox2.Text == "") //不指定學員
                    {
                        var search01 = datatable.Where(a => a.訂單狀態.Contains(comboBox1.Text));
                        dataGridView1.DataSource = search01.ToList();
                    }
                    else //指定學員
                    {
                        if (comboBox1.Text == "全部") //看指定日期的指定學員
                        {
                            var search01 = datatable.Where(a => a.學員ID == Convert.ToInt32(textBox2.Text));
                            dataGridView1.DataSource = search01.ToList();
                        }
                        else 
                        {
                            var search01 = datatable.Where(a => a.訂單狀態.Contains(comboBox1.Text) && a.學員ID == Convert.ToInt32(textBox2.Text));
                            dataGridView1.DataSource = search01.ToList();
                        }
                    }
                }
            }
            else  //指定日期
            {
                if (comboBox1.Text == "全部" && textBox2.Text == "") //沒有指定資料 
                {
                    var search01 = datatable.Where(a => a.訂單時間.Contains(timeday));
                    dataGridView1.DataSource = search01.ToList();
                }
                else if (comboBox1.Text == "全部" && textBox2.Text != "") 
                {
                    var search01 = datatable.Where(a => a.訂單時間.Contains(timeday)&&a.學員ID== Convert.ToInt32(textBox2.Text));
                    dataGridView1.DataSource = search01.ToList();
                }
                else
                {
                    if (textBox2.Text == "") //不指定學員
                    {
                        var search01 = datatable.Where(a => a.訂單狀態.Contains(comboBox1.Text) && a.訂單時間.Contains(timeday));
                        dataGridView1.DataSource = search01.ToList();
                    }
                    else
                    {
                        var search01 = datatable.Where(a => a.訂單狀態.Contains(comboBox1.Text) && a.學員ID == Convert.ToInt32(textBox2.Text) && a.訂單時間.Contains(timeday));
                        dataGridView1.DataSource = search01.ToList();
                    }
                }
            }
        }

        private void clear_search_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = true;
            dateTimePicker1.Value = DateTime.Now;
            show_ordertable();
        }
        private void dateday_ValueChanged(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            string dateday= dateTimePicker1.Value.ToString("yyyyMMdd");
            List<總訂單資料> datatable = _Order_information.gettable();
            var select01 = datatable.Where(a => a.訂單時間.Contains(dateday));
            dataGridView1.DataSource = select01.ToList();
            comboBox1.Text = "全部";
        }

        private void close_day_search_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled=false;
            dateTimePicker1.Value = DateTime.Parse("1995/01/23");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<總訂單資料> dataTable = dataGridView1.DataSource as List<總訂單資料>;
            List<總訂單資料> sorted= dataTable.OrderByDescending(a=>a.總額).ToList();
            dataGridView1.DataSource=sorted;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<總訂單資料> dataTable = dataGridView1.DataSource as List<總訂單資料>;
            List<總訂單資料> sorted = dataTable.OrderBy(a => a.訂單時間).ToList();
            dataGridView1.DataSource = sorted;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_open_panel == false)
            {
                _open_panel = true;
                panel1.Visible = true;
            }
            else
            {
                _open_panel = false;
                panel1.Visible = false;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            string dateday = dateTimePicker2.Value.ToString("yyyyMMdd");
            List<評論排序資料> alist = _Order_feedback_start.gettable_feedback();
            var datelist = alist.Where(a => a.訂單時間.Contains(dateday));
            dataGridView4.DataSource=datelist.ToList();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            List<評論排序資料> alist = _Order_feedback_start.gettable_feedback();
            dataGridView4.DataSource = alist;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            List<評論排序資料> alist = dataGridView4.DataSource as List<評論排序資料>;
            var listsort = alist.OrderBy(a => a.評價星數);
            dataGridView4.DataSource= listsort.ToList();
        }

        private void dataGridView4_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            order_meal_systemEntities db = new order_meal_systemEntities();
            List<評論排序資料> alist = dataGridView4.DataSource as List<評論排序資料>;
            int id = alist[e.RowIndex].訂單ID;
            _ID = id;
            show_orderdeatal(_ID);
            var store_information = new 子訂單商家資料().gettable(_ID);
            dataGridView3.DataSource = store_information.ToList();
            control_feedback_table(db);
        }

        private void toolStripButton9_Click_1(object sender, EventArgs e)
        {
            string search = toolStripTextBox1.Text;
            if (!Chick_tool.chickstring_only_math(search))
            {
                toolStripTextBox1.Text = "";
                MessageBox.Show("編號格式錯誤");
                show_ordertable();
                return;
            }
            if (toolStripTextBox1.Text != "")
            {
                order_meal_systemEntities db = new order_meal_systemEntities();
                List<總訂單資料> datatable = _Order_information.gettable();
                var searchdatarow = datatable.Where(a => a.訂單ID == Convert.ToInt32(search));
                dataGridView1.DataSource = searchdatarow.ToList();
            }
            else
                show_ordertable();
            toolStripTextBox1.Text = "";
        }
    }
}
