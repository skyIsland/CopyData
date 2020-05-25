using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Sky.TransferData
{
    public static class Helper
    {
        #region 获取配置文件
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns></returns>
        public static Setting GetSetting(string filePath)
        {
            string result;
            if (!File.Exists(filePath))
            {
                Console.WriteLine("配置文件不存在,自动生成....");
                var fs = File.Create(filePath);
                fs.Close();
                var obj = new Setting();
                var jsonStr = Encode(obj);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(jsonStr);
                }
                Console.WriteLine("自动生成配置文件完成...");
            }
            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }
            return Decode<Setting>(result);
        }
        #endregion

        #region JsonHelper
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Decode<T>(string input)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(input);
        }
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Encode(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(obj);
        }
        #endregion
    }
}
