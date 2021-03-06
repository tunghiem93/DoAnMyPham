using CMS_DTO.CMSContact;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.Utilities
{
    public class CommonHelper
    {
        public static bool SendContentMail(string EmailTo, string Content, string Name, string Subject, string imgUrl = "", string attachment = "")
        {
            bool isOk = false;
            try
            {

                string email = Commons.EmailSend;
                string passWord = Commons.PassEmailSend;
                string emailReceive = Commons.EmailReceive;
                string smtpServer = "smtp.gmail.com";
                if (email != "" && passWord != "")
                {
                    MailMessage mail = new MailMessage(email, EmailTo);
                    mail.Subject = Subject;
                    mail.Body = Content;
                    mail.IsBodyHtml = true;
                    if (!string.IsNullOrEmpty(imgUrl))
                        mail.Body = string.Format("<div><img src='{0}'/><div>", imgUrl);

                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(email, passWord);
                    client.Host = smtpServer;
                    client.Timeout = 10000;
                    client.EnableSsl = true;
                    if (!string.IsNullOrEmpty(attachment))
                        mail.Attachments.Add(new System.Net.Mail.Attachment(attachment));
                    client.Send(mail);
                    isOk = true;
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Send Mail Error", ex);
            }
            return isOk;
        }

        public static List<string> GenerateCode(int number, List<string> listCodeInDB, int length = 10)
        {
            List<string> listCode = new List<string>();
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                string code = "";
                do
                {
                    code = new string(Enumerable.Repeat(Commons.PasswordChar, length).Select(s => s[random.Next(s.Length)]).ToArray());
                } while (listCodeInDB.Contains(code) || listCode.Contains(code));
                listCode.Add(code);
            }
            return listCode;
        }

        public static bool ContactAdmin(CMS_ContactModels _ctInfo)
        {
            var ret = false;
            try
            {
                string lamodMail = ConfigurationManager.AppSettings["LamodeMail"];

                /* send mail to lamode */
                SendContentMail(lamodMail, _ctInfo.GetContactInfo(), _ctInfo.Name, "[Customer Contact]" + _ctInfo.Subject);

                /* send mail to customer */
                if (!string.IsNullOrEmpty(_ctInfo.Email))
                    SendContentMail(_ctInfo.Email, _ctInfo.GetContactInfo(), _ctInfo.Name, "[Lamode Home]" + " thanks for contacting us " + _ctInfo.Subject);

                ret = true;
            }
            catch (Exception ex) { }
            return ret;
        }


        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob00900X";
        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static string RandomNumberOrder()
        {
            Random R = new Random();
            return "NO_" + ((long)R.Next(0, 100000) * (long)R.Next(0, 100000)).ToString().PadLeft(10, '0');
        }

        public static string GetDurationPostFromNow(DateTime? dateUpdate)
        {
            var ret = "";
            try
            {
                var span = (TimeSpan)(DateTime.Now - dateUpdate);
                /* get total day */
                int totalDay = span.Days;

                /* get total year */
                int years = totalDay / 365;
                totalDay -= (years * 365);

                /* get total months */
                int months = totalDay / 30;
                totalDay -= (months * 30);

                /* string format */
                //string formatted = string.Format("{0}{1}{2}{3}{4}{5}",
                //                    years > 0 ? string.Format("{0:0}y ", years) : string.Empty,
                //                    months > 0 ? string.Format("{0:0}m ", months) : string.Empty,
                //                    years > 0 ? "" : totalDay > 0 ? string.Format("{0:0}d ", totalDay) : string.Empty,
                //                    months > 0 ? "" : span.Hours > 0 ? string.Format("{0:0}h ", span.Hours) : string.Empty,
                //                    totalDay < 0 ? "" : span.Minutes > 0 ? string.Format("{0:0}min ", span.Minutes) : string.Empty,
                //                    totalDay < 0 ? "" : span.Seconds > 0 ? string.Format("{0:0}s", span.Seconds) : string.Empty
                //                    );
                string formatted = string.Format("{0}{1}{2}{3}",
                                    years > 0 ? string.Format("{0:0}y ", years) : string.Empty,
                                    months > 0 ? string.Format("{0:0}m ", months) : string.Empty,
                                    years > 0 ? "" : totalDay > 0 ? string.Format("{0:0}d ", totalDay) : string.Empty,
                                    months > 0 ? "" : span.Hours > 0 ? string.Format("{0:0}h ", span.Hours) : string.Empty
                                    );

                ret += formatted;
                ret += " ago";
            }
            catch (Exception ex) { };
            return ret;
        }

        public string CouponGenerator(int length)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));

                sb.Append(ch);
            }

            return sb.ToString();
        }
        private static readonly Random _random = new Random();
    }
}
