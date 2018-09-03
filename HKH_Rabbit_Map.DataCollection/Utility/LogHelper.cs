using System;
using System.IO;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace HKH_Rabbit_Map.utility
{
    public class LogHelper
    {
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        #region static void WriteLog(Type t, Exception ex)

        public static void WriteLog(Type t, Exception exstr)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                log4net.ILog log = log4net.LogManager.GetLogger(t);
                //log.Info("info", exstr);
                log.Info(exstr.Message, exstr);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\log\error.txt", ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        #region static void WriteLog(Type t, string msg) 
        public static void WriteLog(Type t, string msg)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                log4net.ILog log = log4net.LogManager.GetLogger(t);
                log.Info(msg);
            }
            catch (Exception ex)
            {
                //throw;
                //throw new Exception(ex.Message);
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\log\error.txt", ex.Message);
            }
        }
        #endregion
    }
}