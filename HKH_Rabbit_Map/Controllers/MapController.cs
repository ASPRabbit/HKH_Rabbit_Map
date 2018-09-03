using HKH_Rabbit_Map.DataCollection.Utility;
using HKH_Rabbit_Map.Models;
using HKH_Rabbit_Map.utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace HKH_Rabbit_Map.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            try
            {
                string name = Request.QueryString["data"];
                bool ret = Regex.IsMatch(name, "[0-9]+");
                string channel = "";
                if (ret)
                {
                    channel = name + "#阀室";
                    using (ServiceStack.Redis.IRedisClient redis = RedisHelper.CreateRedisPool().GetClient())
                    {
                        double[] rest = JsonConvert.DeserializeObject<double[]>(redis.Get<string>(channel));
                        return Content(JsonConvert.SerializeObject(new Vala { loggerName = channel, Data = rest[0].ToString("f2") }));
                    }
                }
                else
                {
                    channel = name;
                    List<string> list = new List<string>();
                    using (ServiceStack.Redis.IRedisClient redis = RedisHelper.CreateRedisPool().GetClient())
                    {
                        double[] rest = JsonConvert.DeserializeObject<double[]>(redis.Get<string>(channel));
                        foreach (double item in rest)
                        {
                            list.Add(item.ToString("f2"));
                        }
                        return Content(JsonConvert.SerializeObject(new Station { loggerName = channel, Data = list.ToArray() }));
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(MapController), ex.StackTrace + ex.Message);
                return null;
            }
        }
    }
}