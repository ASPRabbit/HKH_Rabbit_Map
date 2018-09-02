using HKH_Rabbit_Map.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace HKH_Rabbit_Map.Controllers
{
    public class MapController : Controller
    {
        private OpcHelper.OpcClient opcClient = new OpcHelper.OpcClient();
        private Dictionary<string,OpcHelper.OpcClient> dicClient = new Dictionary<string, OpcHelper.OpcClient>();
        // GET: Map
        public ActionResult Index()
        {
            InitOpcServer();
            return View();
        }

        private void InitOpcServer()
        {
            //throw new NotImplementedException();

            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "输卤车间.RTU.AI1", "输卤车间.RTU.AI2", "输卤车间.RTU.AI3" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("输卤车间", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "2#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("2#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "3#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("3#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "4#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("4#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "5#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("5#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "6#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("6#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "7#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("7#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "8#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("8#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "9#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("9#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "10#阀室.RTU.AI1" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("10#阀室", opcClient);
            opcClient.ConnectServer(new OpcHelper.OpcParam
            {
                RemoteServerName = "KEPware.KEPServerEx.V6",
                RemoteServerIp = "",
                GrpIsActive = true,
                GrpDeadBand = 0,
                UpdateRate = 500,//更新频率 0.5秒一次
                IsActive = true,
                IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                Point = new string[] { "五通厂区.RTU.AI1","五通厂区.RTU.AI2" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                GroupName = "OPCDOTNETGROUP"
            });
            dicClient.Add("五通厂区", opcClient);
        }

        public ActionResult GetData()
        {
            string name = Request.QueryString["data"];
            var ret = Regex.IsMatch(name, "[0-9]+");
            var channel = "";
            if (ret)
            {
                channel = name + "#阀室";
                
                
                var rest = opcClient.ReadValue("AI1").ToString();
                return Content(JsonConvert.SerializeObject(rest));
            }
            else
            {
                channel = name;
                opcClient.ConnectServer(new OpcHelper.OpcParam
                {
                    RemoteServerName = "KEPware.KEPServerEx.V6",
                    RemoteServerIp = "",
                    GrpIsActive = true,
                    GrpDeadBand = 0,
                    UpdateRate = 500,//更新频率 0.5秒一次
                    IsActive = true,
                    IsSubScribed = true,//使用订阅功能，即可以异步，默认false
                    Point = new string[] { channel + ".RTU.AI1", channel + ".RTU.AI2", channel + ".RTU.AI3", channel + ".RTU.AI4" },//data7、8 柴油流量 data9、10 DN159流量  data 5 6 DN219流量
                    GroupName = "OPCDOTNETGROUP"
                });
                var list = new List<string>();
                var retst1 = opcClient.ReadValue("AI1").ToString();
                var retst2 = opcClient.ReadValue("AI2").ToString();
                var retst3 = opcClient.ReadValue("AI3").ToString();
                var retst4 = opcClient.ReadValue("AI4").ToString();
                list.Add(retst1);
                list.Add(retst2);
                list.Add(retst3);
                list.Add(retst4);
                return Content(JsonConvert.SerializeObject(list));
            }
            
        }
    }
}