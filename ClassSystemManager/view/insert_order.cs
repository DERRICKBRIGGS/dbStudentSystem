using ClassSystemManager.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassSystemManager.view
{
    public partial class insert_order : Form
    {
        //private DataTable _Order_little_table;
        private DataTable _Order_toal_table;
        private t訂餐_訂單資訊表 _C_Order;
        private List<t訂餐_訂單詳細資訊表> _C_Order_deatal;
        private DialogResult _isOk;
        public DialogResult isOK { get { return _isOk; } }
        public string pro_means_transaction { get { return txtmeans_of_transaction.Text; } } //傳出訂單交易方式字串
        public List<t訂餐_訂單詳細資訊表> c_Order_deatal { get { return _C_Order_deatal; } }
        public t訂餐_訂單資訊表 c_Order { get { return _C_Order; } }
        public insert_order()
        {
            InitializeComponent();
        }

        private void insert_order_Load(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var student = from a in db.t會員_學生
                          select a;
            foreach (var item in student)
            {
                comstudentID.Items.Add(item.學生ID);
            }
            var store = from b in db.t訂餐_店家資料表
                        select b.店家名稱;
            foreach (string a in store)
            {
                txtstore.Items.Add(a);
            }
            for (int i = 1; i < 10; i++)
            {
                txtcount.Items.Add(i);
            }
            txtmeans_of_transaction.Items.Add("付現交易");
            txtmeans_of_transaction.Items.Add("刷卡交易");
            txtmeans_of_transaction.Items.Add("第三方平台");
        }
        /// <summary>
        /// 計算加總菜單價錢與數量
        /// </summary>
        private void order_toal_table() 
        {
            if (_C_Order_deatal.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("總價");
                dt.Columns.Add("餐點總數");
                _Order_toal_table = dt;
                DataRow dataRow = _Order_toal_table.NewRow();
                int price = 0;
                int count = 0;
                for (int i = 0; i < _C_Order_deatal.Count; i++) 
                {
                   price += Convert.ToInt32(_C_Order_deatal[i].金額小記);
                   count += Convert.ToInt32(_C_Order_deatal[i].餐點數量);
                }
                dataRow["總價"]=price.ToString();
                dataRow["餐點總數"]=count.ToString();
                _Order_toal_table.Rows.Add(dataRow);
                totalView2.DataSource = _Order_toal_table;
            }
        }

        private void comstudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(comstudentID.Text);
            order_meal_systemEntities db = new order_meal_systemEntities();
            var student = (from a in db.t會員_學生
                          where a.學生ID== id
                           select a).FirstOrDefault();
            txtstudentName.Text = student.姓名;
        }

        private void comstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            string storename = txtstore.Text;
            order_meal_systemEntities db = new order_meal_systemEntities();
            var meals= from a in db.t訂餐_餐點資訊表
                      join b in db.t訂餐_店家資料表 on a.店家ID equals b.店家ID
                      where b.店家名稱== storename
                      select a.餐點名稱;
            txtMealName.Items.Clear();
            foreach (string meal in meals) 
            {
                txtMealName.Items.Add(meal);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!chick())
            {
                MessageBox.Show("訂單新增失敗，資料要都寫喔");
                return;
            }
            if (_C_Order_deatal == null)
            {
                _C_Order_deatal = new List<t訂餐_訂單詳細資訊表>();
            }
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = db.t訂餐_餐點資訊表.FirstOrDefault(a => a.餐點名稱 == txtMealName.Text).餐點定價;
            int count = Convert.ToInt32(txtcount.Text);
            int price = Convert.ToInt32(result);
            t訂餐_訂單詳細資訊表 orderdeatal = new t訂餐_訂單詳細資訊表();
            orderdeatal.金額小記 = price * count;
            orderdeatal.餐點ID = convert_mealname_to_ID(txtMealName.Text);
            orderdeatal.餐點數量 = count;
            orderdeatal.店家ID = convert_storename_to_ID(txtstore.Text);
            _C_Order_deatal.Add(orderdeatal);
            show_order_deatal();
            order_toal_table();
            control_clear();
            comstudentID.Enabled = false;
            txtstudentName.Enabled = false;
        }

        private void show_order_deatal()
        {
            var datatable = from a in _C_Order_deatal
                            select new
                            {
                                店家姓名 = convert_storeID_to_name(a.店家ID),
                                餐點名稱 = convert_mealID_to_name(a.餐點ID),
                                餐點數量 = a.餐點數量,
                                小計 = a.金額小記
                            };
            dataGridView1.DataSource = datatable.ToList();
        }

        private string convert_mealID_to_name(int ID)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = db.t訂餐_餐點資訊表.Where(a => a.餐點ID == ID).FirstOrDefault();
            return result.餐點名稱;
        }
        private int convert_mealname_to_ID(string name)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = db.t訂餐_餐點資訊表.Where(a => a.餐點名稱 == name).FirstOrDefault();
            return result.餐點ID;
        }
        private string convert_storeID_to_name(int ID)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = db.t訂餐_店家資料表.Where(a => a.店家ID == ID).FirstOrDefault();
            return result.店家名稱;
        }
        private int convert_storename_to_ID(string name)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = db.t訂餐_店家資料表.Where(a => a.店家名稱 == name).FirstOrDefault();
            return result.店家ID;
        }

        private void control_clear()
        {
            txtstore.Text = null;
            txtMealName.Items.Clear();
            txtcount.Text = null;
        }

        //todo 日期修改
        private void button3_Click(object sender, EventArgs e)
        {
            if (_C_Order == null) 
            {
                _C_Order=new t訂餐_訂單資訊表();
            }
            if (MessageBox.Show("是否確認付款完成訂餐", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                if (!String.IsNullOrEmpty(txtmeans_of_transaction.Text))
                {
                    _isOk = DialogResult.OK;
                    _C_Order.訂單時間 = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    _C_Order.學員ID = Convert.ToInt32(comstudentID.Text);
                    _C_Order.支付方式 = txtmeans_of_transaction.Text;
                    _C_Order.訂單狀態 = "進行中";
                    this.Close();
                }
                else { MessageBox.Show("付款方式未填寫"); }
            }
        }
        // todo 刪除功能
        private void button4_Click(object sender, EventArgs e)
        {
            if (_C_Order_deatal == null) { return; }
            if (_C_Order_deatal.Count != 0)
            {
                if (!dataGridView())
                    return;
                if (MessageBox.Show("確認刪除嗎?", "確認", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                _C_Order_deatal.RemoveAt(dataGridView1.SelectedRows[0].Index);
                // dataTable.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                show_order_deatal();
                order_toal_table();
            }
        }
        private bool dataGridView()
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("目前沒有資料");
                return false;
            }
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("選擇指定資料");
                return false;
            }
            return true;
        }
        private bool chick() 
        {
            if (String.IsNullOrEmpty(comstudentID.Text)) { return false; }
            if (String.IsNullOrEmpty(txtstudentName.Text)) { return false; }
            if (String.IsNullOrEmpty(txtstore.Text)) { return false; }
            if (String.IsNullOrEmpty(txtMealName.Text)) { return false; }
            if (String.IsNullOrEmpty(txtcount.Text)) { return false; }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _isOk = DialogResult.No;
            this.Close();
        }

        private void txtMealName_SelectedIndexChanged(object sender, EventArgs e)
        {
            label10.Text = "售價 : ";
            order_meal_systemEntities db = new order_meal_systemEntities();
            var choice_price =( from a in db.t訂餐_餐點資訊表
                        where a.餐點名稱== txtMealName.Text
                        select a.餐點定價).FirstOrDefault();
            //int price = Convert.ToInt32(choice_price);
            string price= Convert.ToInt32(choice_price).ToString();
            label10.Text += "$"+ price;
        }
    }
}
