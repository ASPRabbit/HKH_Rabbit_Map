using HKH_Rabbit_Map.Models;
using HKH_Rabbit_Map.utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HKH_Rabbit_Map.Controllers
{
    public class MapController : Controller
    {
        private static readonly Dictionary<string, double[]> dicData = new Dictionary<string, double[]>();
        // GET: Map
        public ActionResult Index()
        {
            dicData.Clear();
            InitOpcServer();
            return View();
        }

        private void InitOpcServer()
        {
            //throw new NotImplementedException();
            Task.Factory.StartNew(() => {
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
            int[] count = new int []{2,3,4,5,6,7,8,9,10 };

            foreach (var item in count)
            {
                Task.Factory.StartNew(() => {
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

            Task.Factory.StartNew(() => {
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

        private void CreateOpcServer(OpcHelper.OpcParam param, string channel)
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
                if (dicData.ContainsKey(channel))
                {
                    dicData[channel] = data;
                }
                else
                {
                    dicData.Add(channel, data);
                }
            };
        }

        public ActionResult GetData()
        {
            string name = Request.QueryString["data"];
            bool ret = Regex.IsMatch(name, "[0-9]+");
            string channel = "";
            if (ret)
            {
                channel = name + "#阀室";
                double[] rest = dicData[channel];

                return Content(JsonConvert.SerializeObject(new Vala { loggerName = channel, Data = rest[0].ToString("f2") }));
            }
            else
            {
                channel = name;
                List<string> list = new List<string>();

                foreach (double item in dicData[channel])
                {
                    list.Add(item.ToString("f2"));
                }

                return Content(JsonConvert.SerializeObject(new Station { loggerName = channel, Data = list.ToArray() }));
            }
        }
    }
}