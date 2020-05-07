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
            
            ("*********初始化配置参数********").WriteInfo();
            _Setting = Helper.GetSetting("Setting.json");

            #endregion

            if (_Setting.OriginConnStr.IsNullOrEmpty() || _Setting.TargetConnStr.IsNullOrEmpty())
            {
                ("源数据库连接字符串或者目标数据库连接字符串不能为空!").WriteError();
            }
            else
            {
                DAL.AddConnStr("originConnStr", _Setting.OriginConnStr, null, "oracle");
                DAL.AddConnStr("targetConnStr", _Setting.TargetConnStr, null, "oracle");
              
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    CopyData("originConnStr", "targetConnStr", _Setting.OpenTaskByDataCount,_Setting.TableNameList);
                    sw.Stop();
                    ("数据库转移完成!耗时:" + sw.Elapsed).WriteInfo();

                }
                catch (Exception ex)
                {
                    ("发生错误:" + ex.Message).WriteError();
                }
            }
            Console.ReadKey();
        }
        /// <summary>
        /// 拷贝数据库
        /// </summary>
        /// <param name="originConn">源数据库</param>
        /// <param name="desConn">目标数据库</param>
        /// <param name="openTaskByDataCount">开启线程数</param>
        private static  void CopyData(string originConn, string desConn, int openTaskByDataCount,List<string> tableNameList)
        {
            DAL dal = DAL.Create(originConn);

            // 获取源数据库的架构信息
            List<IDataTable> tableList = dal.Tables;
            if (tableList.Count == 0)
            {
               ("数据表数量为0,复制数据任务结束!").WriteInfo();
                return;
            }

            // 过滤掉视图
            tableList.RemoveAll(t => t.IsView);

            if(tableNameList != null && tableNameList.Any())
            {
                tableList = tableList.Where(p => tableNameList.Contains(p.TableName)).ToList();
            }
            else
            {
                // 必须指定表
                return;
            }

            // 首先拷贝数据库架构            
            DAL desDal = DAL.Create(desConn);   
            
            // 要在配置文件中启用数据库架构才行 
            desDal.Db.CreateMetaData().SetTables(new Migration(), tableList.ToArray());

            // 然后依次拷贝每个表中的数据
            foreach (var item in tableList)
            {
                (item.DisplayName + "开始转移").WriteInfo();

                var Factory = EntityFactory.CreateOperate(item.GetType());
                //IEntityOperate Factory = dal.CreateOperate(item.Name);

                Factory.Session.Truncate();

                // 分页获取数据，并更新到新的数据库，通过更改数据库连接来完成
                var allCount = (int)Factory.FindCount();
                if (openTaskByDataCount < 0) openTaskByDataCount = GetDataRowsPerConvert(allCount);
                int pages = (int)Math.Ceiling(((double)allCount / openTaskByDataCount));
                for (int i = 0; i < pages; i++)
                {
                    Factory.ConnName = originConn;
                    var modelList = Factory.FindAll(string.Empty, Factory.Unique, string.Empty, i * openTaskByDataCount, openTaskByDataCount);
                    Factory.ConnName = desConn;
                    modelList.Insert(true);
                }
                ////首先根据表名称获取当前表的实体操作接口
                //IEntityOperate factory = dal.CreateOperate(item.Name);
                ////总数量
                //int allCount = (int)factory.FindCount();
                //var perCount = GetDataRowsPerConvert(allCount);
                //var pages = (int)Math.Ceiling((double)allCount / perCount);
                //if (openTaskByDataCount != 0)
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

    static class ConsoleExe
    {
        #region 控制台输出信息
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="str"></param>
        public static void WriteError(this string str)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ForegroundColor = oldColor;
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="str"></param>
        public static void WriteInfo(this string str)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(str);
            Console.ForegroundColor = oldColor;
        }
        #endregion
    }
}
