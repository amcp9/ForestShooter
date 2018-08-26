using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;

namespace War_Server.Servers
{
    /// <summary>
    /// 数据封装与解包的工具类
    /// </summary>
    public class Message
    {
        private int head = 8;//协议头部大小(字节)
        private byte[] data = new byte[1024];
        private int tail = 0;//尾指针

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
        public void ReadMessage(int dataLength, Action<ActionCode, string> processDataCallback)
        {
            tail += dataLength;
            while (true)
            {
                if (tail <= head) return;//接受数据太小

                //headContent解析出来的大小指的是去除整个协议头部的数据大小
                int headContent = BitConverter.ToInt32(data, 0);
                int re = BitConverter.ToInt32(data, 4);

                //判断接收到的数据去掉头部是否满足headContent
                if ((tail - head) >= headContent)
                {
                    ActionCode actionCode = (ActionCode)re;
                    string s = Encoding.UTF8.GetString(data, head, headContent);

                    //执行回调函数
                    processDataCallback(actionCode, s);

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

        /// <summary>
        /// 打包数据
        /// </summary>
        public static byte[] PackData(RequestCode request, ActionCode action,string data)
        {
            byte[] requestBytes = BitConverter.GetBytes((int)request);
            byte[] actionBytes = BitConverter.GetBytes((int)action);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;

            byte[] dataHead = BitConverter.GetBytes(dataLength);

            return dataHead.Concat(requestBytes).ToArray<byte>().
                    Concat(actionBytes).ToArray<byte>().
                    Concat(dataBytes).ToArray<byte>();
        }
    }
}
