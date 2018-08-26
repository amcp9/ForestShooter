using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace War_Server.Servers
{
    class Message
    {
        private int head = 12;//协议头部大小(字节)
        private byte[] data = new byte[1024];
        private int tail = 0;//尾指针

        //public void AddCount(int count)
        //{
        //    tail += count;
        //}
        public byte[] Data
        {
            get { return data; }
        }
        public int StartIndex
        {
            get { return tail; }
        }
        public int RemainSize
        {
            get { return data.Length - tail; }
        }
        /// <summary>
        /// 解析数据或者叫做读取数据
        /// </summary>
        public void ReadMessage(int dataLength,Action<RequestCode,ActionCode,string> processDataCallback)
        {
            tail += dataLength;
            while (true)
            {
                if (tail <= head) return;//接受数据太小

                //headContent解析出来的大小指的是去除整个协议头部的数据大小
                int headContent = BitConverter.ToInt32(data, 0);
                Console.WriteLine("headContent:" + headContent);
                int re = BitConverter.ToInt32(data, 4);
                Console.WriteLine("re:" + re);
                int ac = BitConverter.ToInt32(data, 8);
                Console.WriteLine("ac:" + ac);

                //判断接收到的数据去掉头部是否满足headContent
                if ((tail - head) >= headContent)
                {
                    RequestCode request = (RequestCode)re;
                    Console.WriteLine("request:" + request.ToString());
                    ActionCode action = (ActionCode)ac;
                    Console.WriteLine("action:" + action.ToString());
                    string s = Encoding.UTF8.GetString(data, head, headContent);
                    Console.WriteLine("s:" + s);

                    //执行回调函数
                    processDataCallback(request, action, s);
                    Console.WriteLine("回调函数已执行！");

                    Array.Copy(data, headContent + head, data, 0, tail - head - headContent);
                    tail -= (headContent + head);
                }
                //此处条件为发生了分包
                else
                {
                    break;
                }
            }
        }

        public static byte[] PackData(ActionCode actionCode,string data)
        {
            byte[] requestBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;
            byte[] dataHead = BitConverter.GetBytes(dataLength);

            return dataHead.Concat(requestBytes).ToArray().Concat(dataBytes).ToArray();
        }
    }
}
