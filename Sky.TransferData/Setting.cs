using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky.TransferData
{
    public class Setting
    {
        /// <summary>
        /// 源数据库连接字符串
        /// </summary>
        public string OriginConnStr { get; set; }

        /// <summary>
        /// 目标数据库连接字符串
        /// </summary>
        public string TargetConnStr { get; set; }      

        /// <summary>
        /// 多少条数据开启一个任务 为0则不开启
        /// </summary>
        public int OpenTaskByDataCount { get; set; } = -1;

        public List<string> TableNameList { get; set; }
    }
}
