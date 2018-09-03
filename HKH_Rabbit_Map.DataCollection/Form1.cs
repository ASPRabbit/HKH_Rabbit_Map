using HKH_Rabbit_Map.DataCollection.Utility;
using HKH_Rabbit_Map.utility;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HKH_Rabbit_Map.DataCollection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitOpcServer();
        }

        private void InitOpcServer()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    CreateOpcServer(new OpcHelper.OpcParam
                    {
                        RemoteServerName = "KEPware.KEPServerEx.V6",
                        RemoteServerIp = "",
                        GrpIsActive = true,
                        GrpDeadBand = 0,
                        UpdateRate = 500,//更新频率 0.5秒一次
                        IsActive = true,
                        IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                        Point = new string[] { "输卤车间.RTU.AI1", "输卤车间.RTU.AI2", "输卤车间.RTU.AI3" },
                        GroupName = "OPCDOTNETGROUP"
                    }, "输卤车间");
                });
                int[] count = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                foreach (int item in count)
                {
                    Task.Factory.StartNew(() =>
                    {
                        CreateOpcServer(new OpcHelper.OpcParam
                        {
                            RemoteServerName = "KEPware.KEPServerEx.V6",
                            RemoteServerIp = "",
                            GrpIsActive = true,
                            GrpDeadBand = 0,
                            UpdateRate = 500,//更新频率 0.5秒一次
                            IsActive = true,
                            IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                            Point = new string[] { item + "#阀室.RTU.AI1" },
                            GroupName = "OPCDOTNETGROUP"
                        }, item + "#阀室");
                    });
                }

                Task.Factory.StartNew(() =>
                {
                    CreateOpcServer(new OpcHelper.OpcParam
                    {
                        RemoteServerName = "KEPware.KEPServerEx.V6",
                        RemoteServerIp = "",
                        GrpIsActive = true,
                        GrpDeadBand = 0,
                        UpdateRate = 500,//更新频率 0.5秒一次
                        IsActive = true,
                        IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                        Point = new string[] { "五通厂区.RTU.AI1", "五通厂区.RTU.AI2" },
                        GroupName = "OPCDOTNETGROUP"
                    }, "五通厂区");
                });
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Form1), ex.StackTrace + ex.Message);
            }
        }

        private void CreateOpcServer(OpcHelper.OpcParam param, string channel)
        {
            try
            {
                OpcHelper.OpcClient client = new OpcHelper.OpcClient();
                client.ConnectServer(new OpcHelper.OpcParam
                {
                    RemoteServerName = param.RemoteServerName,
                    RemoteServerIp = param.RemoteServerIp,
                    GrpIsActive = param.GrpIsActive,
                    GrpDeadBand = param.GrpDeadBand,
                    UpdateRate = param.UpdateRate,//更新频率 0.5秒一次
                    IsActive = param.IsActive,
                    IsSubScribed = param.IsSubScribed,//使用订阅功能，即可以异步，默认false
                    Point = param.Point,
                    GroupName = param.GroupName
                });
                client.DataChange += (data) =>
                {
                    using (var redis = RedisHelper.CreateRedisPool().GetClient())
                    {
                        redis.Set(channel, JsonConvert.SerializeObject(data));
                    }
                };
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Form1), ex.StackTrace + ex.Message);
            }
        }
    }
}
