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

namespace ClassSystemManager.view
{
    public partial class insert_meal : Form
    {
        private DialogResult _isOk;
        private t訂餐_餐點資訊表 _meal;
        public DialogResult isOK { get { return _isOk; } }
        public insert_meal()
        {
            InitializeComponent();
        }
        public t訂餐_餐點資訊表 meal 
        {
            get 
            {
                if (_meal == null) 
                {
                    _meal=new t訂餐_餐點資訊表();
                }
                _meal.餐點名稱 = txtName.Text;
                _meal.餐點描述 = txtfeature.Text;
                if (!String.IsNullOrEmpty(priceBox.Text))
                {
                     _meal.餐點定價 = Convert.ToDecimal(priceBox.Text);
                }
                _meal.餐點名稱 = txtName.Text;
                return _meal; 
            } 

            set 
            {  
                _meal = value;
                txtName.Text= _meal.餐點名稱.ToString();
                txtfeature.Text = _meal.餐點描述.ToString() ;
                priceBox.Text= ((decimal)_meal.餐點定價).ToString("0");
                if (!String.IsNullOrEmpty(_meal.餐點照片)) 
                {
                    pictureBox1.Image = new Bitmap(Application.StartupPath + @"\\" + _meal.餐點照片);
                }
            } 
        }
        private void OK_Click(object sender, EventArgs e)
        {
            if (chick1())
            {
                _isOk = DialogResult.OK;
                this.Close();
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            _isOk= DialogResult.Cancel;
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "產品照片|*.jpg|產品照片|*.png|產品照片|*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //制定一個不重複的檔名
                string photo = Guid.NewGuid().ToString() + ".jpg";
                this.meal.餐點照片 = photo;
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
                result += "餐點名稱不可空白" + "\r\n";
            }
            if (string.IsNullOrEmpty(priceBox.Text))
            {
                result += "餐點價格不可空白" + "\r\n";
            }
            if (!Chick_tool.chickstring_only_math(priceBox.Text)) 
            {
                result += "價格欄位格式錯誤"+"\r\n";
            }
            if (txtfeature.Text.Length > 50) 
            {
                result += "餐點介紹字數不可超過50字";
            }
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result);
            }
            return result == "";
        }

        private void textfeature_MouseClick(object sender, MouseEventArgs e)
        {
            if(txtfeature.Text== "(不可大於50字)")
                txtfeature.Text = "";
        }
    }
}
