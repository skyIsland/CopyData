using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;
using XCode.DataAccessLayer;

namespace CopyData
{
    public class ExportData
    {
        /// <summary>
        /// 拷贝数据库,只需要数据库连接字符串和源数据库即可
        /// </summary>
        /// <param name="originConn">源数据库连接字符串配置名</param>
        /// <param name="desConn">目标数据库连接字符串配置名</param>
        /// <param name="perCount">每次获取的记录数目，如果默认-1则会自动调用函数计算一个合理值<</param>
        public static void CopyData(string originConn, string desConn, int perCount = -1)
        {
            DAL dal = DAL.Create(originConn);
            List<IDataTable> tableList = dal.Tables;//获取源数据库的架构信息
            tableList.RemoveAll(t => t.IsView);//过滤掉视图
            //首先拷贝数据库架构            
            DAL desDal = DAL.Create(desConn);
            var ss = desDal.Tables ?? new List<IDataTable>();
            //要在配置文件中启用数据库架构才行 
            desDal.Db.CreateMetaData().SetTables(new NegativeSetting(), tableList.ToArray());
            //然后依次拷贝每个表中的数据
            foreach (var item in ss)
            {
                //首先根据表名称获取当前表的实体操作接口
                IEntityOperate Factory = dal.CreateOperate(item.Name);
                //分页获取数据，并更新到新的数据库，通过更改数据库连接来完成
                int allCount = (int)Factory.FindCount();
                if (perCount < 0) perCount = GetDataRowsPerConvert(allCount);
                int pages = (int)Math.Ceiling((double)allCount / perCount);
                for (int i = 0; i < pages; i++)
                {
                    Factory.ConnName = originConn;
                    var modelList = Factory.FindAll(string.Empty, string.Empty, string.Empty, i * perCount, perCount);
                    Factory.ConnName = desConn;
                    modelList.Insert(true);
                }
            }
        }
        /// <summary>
        /// 根据数据表的记录总数来设置一个合理的每次转换数目。数据量大，一次性导出导入不合理
        /// </summary>
        /// <param name="allCount">数据表记录总数</param>
        /// <returns>每次转换的记录数</returns>
        private static int GetDataRowsPerConvert(int allCount)
        {
            if (allCount < 1000) return 200;
            if (allCount < 5000) return 500;
            return allCount < 50000 ? 1000 : 1500;
        }
    }
}
