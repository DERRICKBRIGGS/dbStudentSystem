using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class Chick_tool
    {
        /// <summary>
        /// 檢查字串是否只包含數字
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool chickstring_only_math(string a)
        {
            try
            {
                Convert.ToDouble(a);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 檢查信箱是否有效
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool  IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim(); //移除頭尾空白

            if (trimmedEmail.EndsWith(".")) //判斷結尾是否有"."
            {
                return false; 
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email); //嘗試創建一個電子郵件
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false; 
            }
        }
        public static bool Ispassword_len_less_ten(string password, string passwordagain) 
        {
            if (password != passwordagain) { return false; }
            if (password.Length > 10 || passwordagain.Length > 10) { return false; }
            if (Chick_tool.chickstring_only_math(password)) { return false; }
            return true;
        }
        public static bool chick_word_lem(string sentence, int word) 
        {
            if (sentence.Length > word)
                return false;
            else
                return true;
        }

    }
}
