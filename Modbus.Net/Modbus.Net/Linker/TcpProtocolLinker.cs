﻿using System.Configuration;

namespace Modbus.Net
{
    /// <summary>
    ///     Tcp连接对象
    /// </summary>
    public abstract class TcpProtocolLinker : ProtocolLinker
    {
        /// <summary>
        ///     构造器
        /// </summary>
        protected TcpProtocolLinker(int port)
            : this(ConfigurationManager.AppSettings["IP"], port)
        {
        }

        /// <summary>
        ///     构造器
        /// </summary>
        /// <param name="ip">Ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="isFullDuplex">是否为全双工</param>
        protected TcpProtocolLinker(string ip, int port, bool isFullDuplex = true)
            : this(ip, port, int.Parse(ConfigurationManager.AppSettings["IPConnectionTimeout"] ?? "-1"), isFullDuplex)
        {
        }

        /// <summary>
        ///     构造器
        /// </summary>
        /// <param name="ip">Ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="connectionTimeout">超时时间</param>
        /// <param name="isFullDuplex">是否为全双工</param>
        protected TcpProtocolLinker(string ip, int port, int connectionTimeout, bool isFullDuplex = true)
        {
            if (connectionTimeout == -1)
            {
                //初始化连接对象
                BaseConnector = new TcpConnector(ip, port, isFullDuplex:isFullDuplex);
            }
            else
            {
                //初始化连接对象
                BaseConnector = new TcpConnector(ip, port, connectionTimeout, isFullDuplex:isFullDuplex);
            }            
        }
    }
}