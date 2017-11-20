using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using XCode.DataAccessLayer;
using System.Diagnostics;
using XCode;

namespace Sky.TransferData
{
    class Program
    {
        public static Setting _Setting;
        static void Main(string[] args)
        {
            #region 配置
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*********初始化配置参数********");
            _Setting = Helper.GetSetting("Setting.json");
            #endregion
            #region 验证参数
            if (_Setting.OriginConnStr.IsNullOrEmpty()|| _Setting.TargetConnStr.IsNullOrEmpty())
            {
                Console.WriteLine("源数据库连接字符串或者目标数据库连接字符串不能为空!");               
            }
            else
            {
                DAL.AddConnStr("originConnStr", _Setting.OriginConnStr, null, "mssql");
                DAL.AddConnStr("targetConnStr", _Setting.TargetConnStr, null, "mssql");
                #endregion
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    CopyData("originConnStr", "targetConnStr", _Setting.OpenTaskByDataCount);
                    sw.Stop();
                    Console.WriteLine("数据库转移完成!耗时:" + sw.Elapsed);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("发生错误:" + ex.Message);
                }
            }
           
            Console.ReadKey();
        }
        /// <summary>
        /// 拷贝数据库
        /// </summary>
        /// <param name="originConn">源数据库</param>
        /// <param name="desConn">目标数据库</param>
        /// <param name="OpenTaskByDataCount">开启线程数</param>
        private static  void CopyData(string originConn, string desConn, int OpenTaskByDataCount)
        {
            DAL dal = DAL.Create(originConn);
            List<IDataTable> tableList = dal.Tables;//获取源数据库的架构信息
            if (tableList.Count == 0)
            {
                Console.WriteLine("数据表数量为0,复制数据任务结束!");
                return;
            }
            tableList.RemoveAll(t => t.IsView);//过滤掉视图
            //首先拷贝数据库架构            
            DAL desDal = DAL.Create(desConn);           
            //要在配置文件中启用数据库架构才行 
            desDal.Db.CreateMetaData().SetTables(Migration.Full);
            //然后依次拷贝每个表中的数据
            foreach (var item in tableList)
            {
                IEntityOperate Factory = dal.CreateOperate(item.Name);
                //分页获取数据，并更新到新的数据库，通过更改数据库连接来完成
                var allCount = (int)Factory.FindCount();
                if (OpenTaskByDataCount < 0) OpenTaskByDataCount = GetDataRowsPerConvert(allCount);
                int pages = (int)Math.Ceiling(((double)allCount / OpenTaskByDataCount));
                for (int i = 0; i < pages; i++)
                {
                    Factory.ConnName = originConn;
                    var modelList = Factory.FindAll(string.Empty, string.Empty, string.Empty, i * OpenTaskByDataCount, OpenTaskByDataCount);
                    Factory.ConnName = desConn;
                    modelList.Insert(true);
                }
                ////首先根据表名称获取当前表的实体操作接口
                //IEntityOperate factory = dal.CreateOperate(item.Name);
                ////总数量
                //int allCount = (int)factory.FindCount();
                //var perCount = GetDataRowsPerConvert(allCount);
                //var pages = (int)Math.Ceiling((double)allCount / perCount);
                //if (OpenTaskByDataCount != 0)
                //{
                //    Task[] tasks = new Task[pages];
                //    for (int i = 0; i < pages; i++)
                //    {
                //        var modelList = factory.FindAll(null, factory.Unique + " asc", null, i * perCount, perCount);
                //        factory.ConnName = desConn;
                //        tasks[i] = Task.Factory.StartNew(() =>
                //        {
                //            Console.WriteLine($"创建复制{factory.TableName}数据任务!");                           
                //            modelList.Insert(true);
                //        });
                //    }
                //    Task.WaitAll(tasks);
                //}
                //else
                //{
                //    for (int i = 0; i < pages; i++)
                //    {
                //        factory.ConnName = originConn;
                //        var modelList = factory.FindAll(string.Empty, string.Empty, string.Empty, i * perCount, perCount);
                //        factory.ConnName = desConn;
                //        modelList.Insert(true);
                //    }
                //}

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
