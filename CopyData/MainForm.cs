using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCode;
using XCode.DataAccessLayer;

namespace CopyData
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var exportConnStr = this.txtExportConnStr.Text.Trim();
            var importConnStr = this.txtImportConnStr.Text.Trim();          
            var isOpenTask = this.rdbtnOpenTask.Checked;
            if (exportConnStr.IsNullOrEmpty())
            {
                MessageBox.Show("源数据库连接字符串不能为空", "错误");
                return;
            }
            if (importConnStr.IsNullOrEmpty())
            {
                MessageBox.Show("目标数据库连接字符串不能为空","错误");
                return;
            }          
            DAL.AddConnStr("exportConnStr", exportConnStr,null, "mssql");
            DAL.AddConnStr("importConnStr", importConnStr, null, "mssql");
            try
            {
                Stopwatch sw =new Stopwatch();
                sw.Start();
                CopyData("exportConnStr", "importConnStr", isOpenTask);
                sw.Stop();
                txtLog.AppendText("数据库复制完成!耗时:"+sw.Elapsed);
                
            }
            catch (Exception ex)
            {
               txtLog.AppendText("发生错误:"+ex.Message);
            }

        }
        /// <summary>
        /// 拷贝数据库
        /// </summary>
        /// <param name="originConn">源数据库</param>
        /// <param name="desConn">目标数据库</param>
        /// <param name="isOpenTask">是否开启线程</param>
        private void CopyData(string originConn, string desConn,bool isOpenTask)
        {
            DAL dal = DAL.Create(originConn);
            List<IDataTable> tableList = dal.Tables;//获取源数据库的架构信息
            if (tableList.Count == 0)
            {
                txtLog.AppendText("数据表数量为0,复制数据任务结束!");
                return;
            }
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
                IEntityOperate factory = dal.CreateOperate(item.Name);
                //总数量
                int allCount = (int)factory.FindCount();
                var perCount = GetDataRowsPerConvert(allCount);
                var pages = (int)Math.Ceiling((double)allCount / perCount);
                if (isOpenTask)
                {                  
                    Task[] tasks=new Task[pages];
                    for (int i = 0; i < pages; i++)
                    {
                        var modelList = factory.FindAll(null, factory.Unique + " asc", null, i * perCount, perCount);
                        factory.ConnName = desConn;
                        tasks[i]=Task.Factory.StartNew(()=>
                        {
                            txtLog.BeginInvoke(new Action(() =>
                            {
                                txtLog.AppendText($"创建复制{factory.TableName}数据任务!");
                            }));
                            modelList.Insert(true);
                        });
                    }
                    Task.WaitAll(tasks);
                }
                else
                {                 
                    for (int i = 0; i < pages; i++)
                    {
                        factory.ConnName = originConn;
                        var modelList = factory.FindAll(string.Empty, string.Empty, string.Empty, i * perCount, perCount);
                        factory.ConnName = desConn;
                        modelList.Insert(true);
                    }
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
