using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassSystemManager.view
{
    public partial class insert_store_time : Form
    {
        private DialogResult _isOk;
        private t訂餐_營業時間表 _time;
        public DialogResult isOK { get { return _isOk; } }
        public t訂餐_營業時間表 time
        {
            get
            {
                if (_time == null)
                {
                    _time = new t訂餐_營業時間表();
                }
                _time.星期 = chick_week();
                _time.時段_早_中_晚_全_ = chick_time();
                _time.開始營業時間 = comboBox1.Text + textBox1.Text;
                _time.結束營業時間 = comboBox2.Text + textBox2.Text;
                return _time;
            }
            set 
            {
                _time=value;
                comboBox1.Text = _time.開始營業時間.Substring(0, 2);
                comboBox2.Text = _time.結束營業時間.Substring(0, 2);
                textBox1.Text = _time.開始營業時間.Substring(2, _time.開始營業時間.Length - 2);
                textBox2.Text = _time.結束營業時間.Substring(2, _time.結束營業時間.Length - 2);
                chickbox_open(_time);
            }
        }

        public insert_store_time()
        {
            InitializeComponent();
        }

        private void insert_store_time_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("上午");
            comboBox1.Items.Add("下午");
            comboBox2.Items.Add("上午");
            comboBox2.Items.Add("下午");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _isOk = DialogResult.OK;
            //MessageBox.Show(chick_week());
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _isOk = DialogResult.Cancel;
            this.Close();
        }
        private string chick_week() 
        {
            string week = "";
            if (checkBox1.Checked) 
            {
                week += "一";
                week += ",";
            }
            if (checkBox2.Checked)
            {
                week += "二";
                week += ",";
            }
            if (checkBox3.Checked)
            {
                week += "三";
                week += ",";
            }
            if (checkBox6.Checked)
            {
                week += "四";
                week += ",";
            }
            if (checkBox5.Checked)
            {
                week += "五";
                week += ",";
            }
            if (checkBox4.Checked)
            {
                week += "六";
                week += ",";
            }
            if (checkBox7.Checked)
            {
                week += "日";
                week += ",";
            }
            if (week != "") 
            {
                week=week.Substring(0, week.Length - 1);
            }
            return week;
        }
        private string chick_time()
        {
            string time = "";
            if (checkBox8.Checked)
            {
                time += "早";
                time += ",";
            }
            if (checkBox9.Checked)
            {
                time += "中";
                time += ",";
            }
            if (checkBox10.Checked)
            {
                time += "晚";
                time += ",";
            }
            if (time != "")
            {
                time = time.Substring(0, time.Length - 1);
            }
            return time;
        }
        public void chickbox_open(t訂餐_營業時間表 a) 
        {
            string time = a.時段_早_中_晚_全_;
            if (a.時段_早_中_晚_全_ == null) { return; }
            if (time.Contains("早")) 
            {
                checkBox8.Checked = true;
            }
            if (time.Contains("中"))
            {
                checkBox9.Checked = true;
            }
            if (time.Contains("晚"))
            {
                checkBox10.Checked = true;
            }
            string week = a.星期;
            if (week.Contains("一"))
            {
                checkBox1.Checked = true;
            }
            if (week.Contains("二"))
            {
                checkBox2.Checked = true;
            }
            if (week.Contains("三"))
            {
                checkBox3.Checked = true;
            }
            if (week.Contains("四"))
            {
                checkBox6.Checked = true;
            }
            if (week.Contains("五"))
            {
                checkBox5.Checked = true;
            }
            if (week.Contains("六"))
            {
                checkBox4.Checked = true;
            }
            if (week.Contains("日"))
            {
                checkBox7.Checked = true;
            }
        }
    }
}
