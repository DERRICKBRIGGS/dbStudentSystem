using ClassSystemManager.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace ClassSystemManager
{
    public partial class insert_store : Form
    {
        private DialogResult _isOk;
        private t訂餐_店家資料表 _table;
        private string _title;
        private int _count; //檢查有幾個標籤
        public string title { get { return _title; } set { _title = value; } }

        public DialogResult isOk { get { return _isOk; } }
        public insert_store()
        {
            InitializeComponent();
        }

        private void insert_store_form_Load(object sender, EventArgs e)
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var item = from a in db.t訂餐_口味總表
                       select a.風味名稱;
            int count = 0;
            int ylocation = 0;
            if (_title != null)
            {
                var style = _title.Split(',');
                foreach (string title in item)
                {
                    System.Windows.Forms.CheckBox checkBox1 = insert_tag_checkbox(ref count, title,ref ylocation);
                    if (style.Contains(checkBox1.Text))
                    {
                        checkBox1.Checked = true;
                    }
                }
            }
            else
            {
                foreach (string title in item)
                {
                    System.Windows.Forms.CheckBox checkBox1 = insert_tag_checkbox(ref count, title, ref ylocation);
                }
            }
        }
        /// <summary>
        /// 自動新增清單中的標籤名稱
        /// </summary>
        /// <param name="count">調整後續位置將位置後移幾個空間</param>
        /// <param name="title">放是否有標籤字串傳入</param>
        /// <returns></returns>
        private System.Windows.Forms.CheckBox insert_tag_checkbox(ref int count, string title, ref int ylocation)
        {
            System.Windows.Forms.CheckBox checkBox1 = new System.Windows.Forms.CheckBox();
            checkBox1.Text = title;
            checkBox1.AutoSize = false;//關閉自動大小
            checkBox1.Size = new Size(84, 16); //設置checkBox大小
            checkBox1.Location = new Point(15 + 90 * count, 430+ylocation);//將後續checkBox移動
            this.Controls.Add(checkBox1);
            count++;
            if (count >= 5)
            {
                count = 0;
                ylocation += 30;
            }
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            return checkBox1;
        }
        /// <summary>
        /// 自訂選擇事件，限制標籤選取數，不可超過五個
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e) 
        {
            System.Windows.Forms.CheckBox checkBox = sender as System.Windows.Forms.CheckBox;
            if(checkBox.Checked==true)
                _count++;
            else if(checkBox.Checked==false)
                _count--;
            if (_count > 5) 
            {
                MessageBox.Show("標簽數不可超過5個");
                checkBox.Checked=false;
            }
        }

        /// <summary>
        /// 使用視窗回傳讀取店家資料表資料
        /// </summary>
        public t訂餐_店家資料表 data
        {
            get
            {
                if (_table == null)
                    _table = new t訂餐_店家資料表();
                _table.店家名稱 = txtName.Text;
                _table.地址 = txtaddress.Text;
                _table.電話 = txtphone.Text;
                _table.餐廳介紹 = txtinformation.Text;
                _table.電子信箱 = txtemail.Text;
                _table.密碼 = txtstore_password.Text;
                return _table;
            }
            set
            {
                _table = value;
                txtName.Text = _table.店家名稱.ToString();
                txtaddress.Text = _table.地址.ToString();
                txtphone.Text = _table.電話.ToString();
                txtinformation.Text = _table.餐廳介紹.ToString();
                txtemail.Text = _table.電子信箱;
                txtstore_password.Text = _table.密碼.ToString();
                txtstore_rechick_password.Text= _table.密碼.ToString();
                if (_table.餐廳照片 != null)
                {
                    pictureBox1.Image = new Bitmap(Application.StartupPath + @"\\" + _table.餐廳照片);
                }
            }
        }
        private string CheckAllCheckBoxes()
        {
            var checkBoxes = Controls.OfType<System.Windows.Forms.CheckBox>();

            string result = "";
            foreach (var checkBox in checkBoxes)
            {
                if (checkBox.Checked)
                {
                    result += $"{checkBox.Text},";
                }
            }
            return result;
        }
        private bool CheckAllCheckBoxe_count(int count) 
        {
            var checkBoxes = Controls.OfType<System.Windows.Forms.CheckBox>();
            int count2 = 0;
            foreach (var checkBox in checkBoxes)
            {
                if (checkBox.Checked)
                {
                    count2 += 1;
                }
                if (count2 > count)
                    return false;
            }
            return true;
        }

        private void tutton1_Click(object sender, EventArgs e)
        {
            if (chick1())
            {
                _isOk = DialogResult.OK;
                _title = CheckAllCheckBoxes();
                this.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _isOk = DialogResult.No;
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "產品照片|*.jpg|產品照片|*.png|產品照片|*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //制定一個不重複的檔名
                string photo = Guid.NewGuid().ToString() + ".jpg";
                this.data.餐廳照片 = photo;
                //複製到指定位置
                File.Copy(openFileDialog1.FileName, Application.StartupPath + @"\\" + photo);
                //將顯示圖檔()內放檔案位置，故與上相同
                pictureBox1.Image = new Bitmap(Application.StartupPath + @"\\" + photo);
                //_pictuallocation = Application.StartupPath + @"\\" + photo;
            }
        }
        private bool chick1()
        {
            string result = "";
            if (string.IsNullOrEmpty(txtName.Text))
            {
                result += "店家名稱  不可空白" + "\r\n";
            }
            if (string.IsNullOrEmpty(txtphone.Text))
            {
                result += "聯絡電話  不可空白" + "\r\n";
            }
            if (string.IsNullOrEmpty(txtaddress.Text))
            {
                result += "店家地址  不可空白" + "\r\n";
            }
            if (string.IsNullOrEmpty(txtemail.Text))
            {
                result += "電子郵件  不可空白" + "\r\n";
            }
            if (string.IsNullOrEmpty(txtstore_password.Text))
            {
                result += "密碼  不可空白" + "\r\n";
            }
            if (! Chick_tool.Ispassword_len_less_ten(txtstore_password.Text, txtstore_rechick_password.Text)) 
            {
                result +="密碼格式錯誤" + "\r\n";
            }
            if (!Chick_tool.IsValidEmail(txtemail.Text)) 
            {
                result +="信箱格式錯誤" + "\r\n";
            }
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result);
            }
            return result == "";
        }
    }
}
