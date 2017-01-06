using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("输入要Ping的次数");
            int a = 0;
            int b = 0;
            string times = Console.ReadLine();
            while (int.TryParse(times, out a) == false)
            {
                Console.WriteLine("输入的不是数字，请重新输入");
                times = Console.ReadLine();
            }
            Console.WriteLine("请输入要每次Ping的间隔时间(毫秒)");
            string intervals = Console.ReadLine();
            while (int.TryParse(intervals, out b) == false)
            {
                Console.WriteLine("输入的不是数字，请重新输入");
                intervals = Console.ReadLine();
            }
            Ping p1 = new Ping();
            string IP = ConfigurationManager.AppSettings["PingAddress"].ToString();
            string[] IPList = IP.Split(',');
            int timeOut = int.Parse(ConfigurationManager.AppSettings["TimeOut"]);
            string filePath = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < IPList.Length; j++)
                {
                    string path = filePath + "_" + "PING IP地址(" + IPList[j] + ")超时记录";
                    PingReply reply = p1.Send(IPList[j], timeOut);
                    var ssss = reply.RoundtripTime;
                    if (reply.Status == IPStatus.TimedOut)
                    {
                        using (FileStream fs1 = new FileStream(path, FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(fs1))
                            {
                                sw.WriteLine(string.Format("PING超时：Ping的IP：{0}，Ping的时间：{1}", IPList[j], DateTime.Now));
                            }
                        }
                    }
                }

                Thread.Sleep(b);
            }
            Console.WriteLine("Ping完成，具体请看该目录下文件：" + filePath);

            Console.ReadKey();
        }
    }
}