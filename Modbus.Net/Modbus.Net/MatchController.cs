﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Modbus.Net
{
    /// <summary>
    ///     通讯号匹配模式的控制器
    /// </summary>
    public class MatchController : FifoController
    {
        /// <summary>
        ///     匹配字典
        /// </summary>
        protected ICollection<(int,int)>[] KeyMatches { get; }

        /// <summary>
        ///     构造器
        /// </summary>
        /// <param name="keyMatches">匹配字典，每个Collection代表一个匹配集合，每一个匹配集合中的数字代表需要匹配的位置，最后计算出来的数字是所有位置数字按照集合排序后叠放在一起</param>
        /// <param name="acquireTime">获取间隔</param>
        public MatchController(ICollection<(int,int)>[] keyMatches, int acquireTime) : base(acquireTime)
        {
            KeyMatches = keyMatches;
        }

        /// <inheritdoc />
        protected override (string,string)? GetKeyFromMessage(byte[] message)
        {
            string ans1 = "";
            string ans2 = "";
            foreach (var matchPoses in KeyMatches)
            {
                int tmpCount = 0, tmpCount2 = 0;
                foreach (var matchPos in matchPoses)
                {
                    tmpCount = tmpCount * 256 + message[matchPos.Item1];
                    tmpCount2 = tmpCount2 * 256 + message[matchPos.Item2];
                }
                ans1 += tmpCount + " ";
                ans2 += tmpCount2 + " ";
            }
            return (ans1, ans2);
        }

        /// <inheritdoc />
        protected override MessageWaitingDef GetMessageFromWaitingList(byte[] receiveMessage)
        {
            var returnKey = GetKeyFromMessage(receiveMessage);
            return WaitingMessages.FirstOrDefault(p=>returnKey.HasValue && p.Key == returnKey.Value.Item2);
        }
    }
}
