using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode.DataAccessLayer;

namespace Sky.TransferData
{
    public class Setting
    {
        /*
         *  //
    // 摘要:
    //     数据库类型
    public enum DatabaseType
    {
        //
        // 摘要:
        //     无效值
        [Description("无效值")]
        None = 0,
        //
        // 摘要:
        //     MS的Access文件数据库
        [Description("Access文件数据库")]
        Access = 1,
        //
        // 摘要:
        //     MS的SqlServer数据库
        [Description("SqlServer数据库")]
        SqlServer = 2,
        //
        // 摘要:
        //     Oracle数据库
        [Description("Oracle数据库")]
        Oracle = 3,
        //
        // 摘要:
        //     MySql数据库
        [Description("MySql数据库")]
        MySql = 4,
        //
        // 摘要:
        //     SqlCe数据库
        [Description("SqlCe数据库")]
        SqlCe = 5,
        //
        // 摘要:
        //     SQLite数据库
        [Description("SQLite数据库")]
        SQLite = 6,
        //
        // 摘要:
        //     SqlCe数据库
        [Description("PostgreSQL数据库")]
        PostgreSQL = 8,
        //
        // 摘要:
        //     网络虚拟数据库
        [Description("网络虚拟数据库")]
        Network = 100
    }
         */
        public string OriginDbType { get; set; } = DatabaseType.Oracle.ToString();

        /// <summary>
        /// 源数据库连接字符串
        /// </summary>
        public string OriginConnStr { get; set; }

        /// <summary>
        /// 目标数据库连接字符串
        /// </summary>
        public string TargetConnStr { get; set; }

        public string TargetDbType { get; set; } = DatabaseType.Oracle.ToString();


        /// <summary>
        /// 多少条数据开启一个任务 为0则不开启
        /// </summary>
        public int OpenTaskByDataCount { get; set; } = -1;

        public List<string> TableNameList { get; set; } = new List<string> { };

    }
}
