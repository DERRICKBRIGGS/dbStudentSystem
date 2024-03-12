using ClassSystemManager.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassSystemManager.view
{
    public partial class insert_tag : Form
    {
        private int _ID;
        public insert_tag()
        {
            InitializeComponent();
        }

        private void insert_tag_Load(object sender, EventArgs e)
        {
            show_table();
        }

        private void show_table()
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var table = from a in db.t訂餐_口味總表
                        select new
                        {
                            標籤ID = a.口味ID,
                            標籤名稱 = a.風味名稱
                        };
            dataGridView1.DataSource = table.ToList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            chick_choice();
            order_meal_systemEntities db = new order_meal_systemEntities();
            t訂餐_口味總表 storelist = db.t訂餐_口味總表.FirstOrDefault(a => a.口味ID == _ID);
            if (textBox1.Text == storelist.風味名稱)
            {
                MessageBox.Show("請輸入修改標籤名稱");
                return;
            }
            storelist.風味名稱 = textBox1.Text;
            if (!Chick_tool.chick_word_lem(textBox1.Text, 5)) 
            {
                MessageBox.Show("標籤字數不可超過五字");
                return;
            }
            if ((MessageBox.Show($"確認是否更新為  {textBox1.Text}  ", "確認", MessageBoxButtons.YesNo)) == DialogResult.Yes)
            {
                db.SaveChanges();
                show_table();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            chick_choice();
            order_meal_systemEntities db = new order_meal_systemEntities();
            t訂餐_口味總表 storelist = db.t訂餐_口味總表.FirstOrDefault(a => a.口味ID == _ID);
            List<t訂餐_店家風味表> store_style_list = db.t訂餐_店家風味表.Where(a => a.口味ID == _ID).ToList();
            foreach (var x in store_style_list)
            {
                db.t訂餐_店家風味表.Remove(x);
            }
            db.t訂餐_口味總表.Remove(storelist);
            if ((MessageBox.Show($"確認是否刪除標籤  {storelist.風味名稱}  ", "確認", MessageBoxButtons.YesNo)) == DialogResult.Yes)
            {
                db.SaveChanges();
                show_table();
            }
        }

        private void chick_choice()
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("請選擇指定標籤");
                return;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            t訂餐_口味總表 form = new t訂餐_口味總表();
            form.風味名稱 = textBox1.Text;
            if (!Chick_tool.chick_word_lem(textBox1.Text, 5))
            {
                MessageBox.Show("標籤字數不可超過五字");
                return;
            }
            if ((MessageBox.Show($"確認是否新增標籤  {form.風味名稱} ", "確認", MessageBoxButtons.YesNo)) == DialogResult.Yes)
            {
                order_meal_systemEntities db = new order_meal_systemEntities();
                db.t訂餐_口味總表.Add(form);
                db.SaveChanges();
            }
            show_table();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            order_meal_systemEntities db = new order_meal_systemEntities();
            var storelist = (from a in db.t訂餐_口味總表
                             select a).ToList();
            t訂餐_口味總表 store = storelist[e.RowIndex];
            textBox1.Text = store.風味名稱;
            _ID = db.t訂餐_口味總表.FirstOrDefault(a => a.風味名稱 == store.風味名稱).口味ID;
        }
    }
}
