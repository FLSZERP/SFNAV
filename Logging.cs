using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SFAPP
{

    /// <summary>
    /// 保存日志
    /// </summary>
    public class Logging
    {
        #region Log Category
        /// <summary>
        /// Save general log
        /// </summary>
        /// <param name="message"></param>
        public static void Writelog(string message)
        {
            string logContent = string.Format("[{0}] =>{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
            SetFile(@"Log "+DateTime.Now.ToString("yyyy-MM-dd") + ".txt", logContent);
        }

        /// <summary>
        /// Save key log
        /// </summary>
        /// <param name="message"></param>
        public static void WriteKeylog(string message)
        {
            var logContent = string.Format("[{0}]=>{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
            SetFile(@"KeyLog " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", logContent);
        }

        /// <summary>
        /// Save error log
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteBuglog(Exception ex)
        {
            var logContent = string.Format("[{0}]Error source：{1}，\r\n Content：{2}",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Source, ex.Message);
            logContent += string.Format("\r\n [{0}] Tracking：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ex.StackTrace);
            SetFile(@"BugLog " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", logContent);
        }
        #endregion

        #region General Process
        /// <summary>
        /// Standardized writing operation, can be inheritance
        /// On general, file saved under debug/log
        /// </summary>
        /// <param name="filename">File Name</param>
        /// <param name="logContent">Write Msg</param>
        protected static void SetFile(string filename, string logContent)
        {
            Isexist(); // check whether exists
            string errLogFilePath = Environment.CurrentDirectory + @"\Log\" + filename.Trim();
            StreamWriter sw;
            if (!File.Exists(errLogFilePath))
            {
                FileStream fs1 = new FileStream(errLogFilePath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs1);
            }
            else
            {
                sw = new StreamWriter(errLogFilePath, true);
            }
            sw.WriteLine(logContent);
            sw.Flush();
            sw.Close();
        }

        // Check exists
        private static void Isexist()
        {
            string path = Environment.CurrentDirectory + @"\Log\";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion
    }
}