using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace HKH_Rabbit_Map.utility
{
    public class OpcHelper
    {
        public class OpcClient
        {
            private double[] _tagValue;
            private OPCServer _kepServer = new OPCServer();
            private OPCGroups _kepGroups;
            public OPCGroup KepGroup;
            private OPCItems _kepItems;
            private OPCItem _kepItem;
            private bool _opcConnected;
            private readonly Dictionary<string, int> _opcItemDic = new Dictionary<string, int>();

            #region 定义事件委托

            public delegate void OpcDataChange(double[] datenum); //定义委托
            public event OpcDataChange DataChange; //定义事件

            #endregion
            public List<string> ServerNames = new List<string>();
            public IList<string> ItemsNames = new List<string>();
            public List<string> Tags = new List<string>();
            public Dictionary<string, IList<string>> dicItems = new Dictionary<string, IList<string>>();
            /// <summary>
            /// OPC SERVER
            /// </summary>
            public void GetOpcServers()
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Environment.MachineName);
                //Console.WriteLine("MAC Address:");
                foreach (IPAddress ip in ipHost.AddressList)
                {
                    Console.WriteLine(ip.ToString());
                }
                //Console.WriteLine("Please Enter IPHOST");
                const string strHostIp = "localhost"; //Console.ReadLine();
                IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostIp);
                try
                {
                    object serverList = _kepServer.GetOPCServers(ipHostEntry.HostName);
                    foreach (string serverName in (Array)serverList)
                    {
                        ServerNames.Add(serverName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            /// <summary>
            /// 初始化OPC SERVER
            /// </summary>
            /// <param name="param"></param>
            public void ConnectServer(OpcParam param)
            {
                try
                {
                    _kepServer.Connect(param.RemoteServerName, param.RemoteServerIp);
                    if (_kepServer.ServerState == (int)OPCServerState.OPCRunning)
                    {
                        _opcConnected = true;
                        CreateGroup(param.GrpIsActive, param.GrpDeadBand, param.UpdateRate, param.IsActive, param.IsSubScribed, param.GroupName);
                        CreateItems(param.Point);
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                    LogHelper.WriteLog(typeof(OpcClient), ex.Message);
                    //return null;
                }
            }

            /// <summary>
            /// 创建组
            /// </summary>
            private void CreateGroup(bool grpIsActive, int grpDeadBand, int updateRate, bool isActive, bool isSubscribed, string groupName)
            {
                try
                {
                    _kepGroups = _kepServer.OPCGroups;
                    KepGroup = _kepGroups.Add(groupName);

                    _kepServer.OPCGroups.DefaultGroupIsActive = grpIsActive;
                    _kepServer.OPCGroups.DefaultGroupDeadband = grpDeadBand;
                    KepGroup.UpdateRate = updateRate;
                    KepGroup.IsActive = isActive;
                    KepGroup.IsSubscribed = isSubscribed;

                    KepGroup.DataChange += KepGroup_DataChange;
                    KepGroup.AsyncWriteComplete += KepGroup_AsyncWriteComplete;
                    _kepItems = KepGroup.OPCItems;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(OpcClient), ex.Message);
                }
            }


            //异步写入完成
            private void KepGroup_AsyncWriteComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array Errors)
            {

            }

            //创建项
            private void CreateItems(string[] point)
            {
                _tagValue = new double[point.Length];
                int i;
                for (i = 0; i < point.Length; i++)
                {
                    try
                    {
                        _kepItem = _kepItems.AddItem(point[i], i);
                        _opcItemDic.Add(point[i].Substring(point[i].LastIndexOf('.') + 1, point[i].Length - point[i].LastIndexOf('.') - 1), _kepItem.ServerHandle);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(typeof(OpcClient), ex.Message);
                    }
                }
            }

            //数据项内容改变事件
            private void KepGroup_DataChange(int transactionId, int numItems, ref Array clientHandles, ref Array itemValues, ref Array qualities, ref Array timeStamps)
            {
                try
                {
                    for (int i = 1; i <= numItems; i++)
                    {
                        int id = (int)clientHandles.GetValue(i);
                        _tagValue[id] = Convert.ToDouble(itemValues.GetValue(i));
                    }
                    DataChange?.Invoke(_tagValue);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(OpcClient), ex.Message);
                }
            }

            //向指定项写入内容
            public void WriteValue(string tagName, object value, string key)
            {
                lock ("write")
                {
                    try
                    {
                        OPCItem bItem = _kepItems.GetOPCItem(_opcItemDic[tagName]);
                        int[] temp = { 0, bItem.ServerHandle };
                        Array serverHandles = temp;
                        object[] valueTemp = { "", value };
                        Array values = valueTemp;
                        KepGroup.AsyncWrite(1, ref serverHandles, ref values, out Array errors, 2009, out int cancelId);
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(typeof(OpcClient), ex.Message);
                    }
                    //Thread.Sleep(100);//暂停100毫秒
                }
            }

            public object ReadValue(string tagName)
            {
                Thread.Sleep(200);
                try
                {
                    return _kepItem.Value;
                }
                catch
                {
                    return null;
                }
            }

            public void ReadValue(string tagName, bool wtf)
            {
                try
                {
                    OPCItem bItem = _kepItems.GetOPCItem(_opcItemDic[tagName]);
                    int[] temp = { 0, bItem.ServerHandle };
                    Array serverHandles = temp;
                    KepGroup.AsyncRead(1, ref serverHandles, out Array errors, 2009, out int cancel);
                    GC.Collect();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            #region
            public void GetItemsNames(string RemoteServerName, string RemoteServerIp, string pass, string pattern, string patternRe)
            {
                try
                {
                    _kepServer.Connect(RemoteServerName, RemoteServerIp);
                    OPCBrowser browser = _kepServer.CreateBrowser();
                    //browser.Filter = "[\u4e00-\u9fa5]+.PLC.[\u4e00-\u9fa5]+[A|B|C]";
                    browser.ShowBranches();
                    browser.ShowLeafs(true);

                    dicItems.Clear();

                    //string pattern = @"[\u4e00-\u9fa5]+\d+\-" + pass + @".PLC.[\u4e00-\u9fa5]+[A-Z]+\d+";//匹配 设备001.PLC.配置参数A1

                    //string patternRe = @".PLC.[\u4e00-\u9fa5]+[A-Z]+\d+";

                    //string pattern = "[A-Z|a-z]+.PLC." + @"[AI|BI]+\d";
                    //string patternRe = @".PLC.[AI|BI]+\d";

                    foreach (object item in browser)
                    {
                        if (Regex.IsMatch(item.ToString(), pattern, RegexOptions.IgnoreCase))
                        {
                            ItemsNames.Add(item.ToString());
                            Regex rgx = new Regex(patternRe);//获取GroupName(组名)
                            string key = rgx.Replace(item.ToString(), " ");
                            IList<string> items = new List<string>
                    {
                        item.ToString()
                    };
                            if (!dicItems.ContainsKey(key))
                            {
                                dicItems.Add(key, items);
                            }
                            else
                            {
                                dicItems[key].Add(item.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(OpcClient), ex.Message);
                }
            }

            /// <summary>断开OPC
            /// 断开与服务器的连接
            /// </summary>
            public void KepDisconnect()
            {
                try
                {
                    if (!_opcConnected)
                    {
                        return;
                    }
                    if (KepGroup != null)
                    {
                        KepGroup.DataChange -= KepGroup_DataChange;
                    }

                    if (_kepServer != null)
                    {
                        _kepServer.Disconnect();
                        _kepServer = null;
                    }
                    _opcConnected = false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion
        }

        public class OpcParam
        {
            public string RemoteServerIp { get; set; }

            public string RemoteServerName { get; set; }

            public string GroupName { get; set; }

            public bool GrpIsActive { get; set; }

            public int GrpDeadBand { get; set; }

            public int UpdateRate { get; set; }

            public bool IsActive { get; set; }

            public bool IsSubScribed { get; set; }

            public string[] Point { get; set; }
        }

    }



}