using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modbus.Net
{
    /// <summary>
    ///		���豸���������
    /// </summary>
    public sealed class MultipleMachinesJobScheduler
    {
        private static int _machineCount = 0;

        /// <summary>
        ///     �����豸������
        /// </summary>
        /// <param name="machines">�豸�ļ���</param>
        /// <param name="machineJobTemplate">�豸������ģ��</param>
        /// <param name="count">�ظ�����������Ϊ����ѭ����0Ϊִ��һ��</param>
        /// <param name="intervalSecond">�������</param>
        /// <returns></returns>
        public static ParallelLoopResult RunScheduler<TKey>(IEnumerable<IMachine<TKey>> machines, Func<IMachine<TKey>, MachineGetJobScheduler, Task> machineJobTemplate, int count = 0, int intervalSecond = 1) where TKey : IEquatable<TKey>
        {
            _machineCount = machines.Count();
            return Parallel.ForEach(machines, (machine, state, index) =>
            {
                Task.Factory.StartNew(async () =>
                {
                    Thread.Sleep((int)(intervalSecond * 1000.0 / _machineCount * index));
                    var getJobScheduler = await MachineJobSchedulerCreator.CreateScheduler("Trigger" + index, count, intervalSecond);
                    await machineJobTemplate(machine, getJobScheduler);
                });
            });
        }

        /// <summary>
        ///     �����豸������
        /// </summary>
        /// <param name="machines">�豸�ļ���</param>
        /// <param name="machineJobTemplate">�豸������ģ��</param>
        /// <param name="count">�ظ�����������Ϊ����ѭ����0Ϊִ��һ��</param>
        /// <param name="intervalSecond">�������</param>
        /// <returns></returns>
        public static ParallelLoopResult RunScheduler(IEnumerable<IMachine<string>> machines, Func<IMachine<string>, MachineGetJobScheduler, Task> machineJobTemplate, int count = 0, int intervalSecond = 1)
        {
            return RunScheduler<string>(machines, machineJobTemplate, count, intervalSecond);
        }

        /// <summary>
        ///		ȡ������
        /// </summary>
        /// <returns></returns>
        public static ParallelLoopResult CancelJob()
        {
            return Parallel.For(0, _machineCount, async index =>
            {
                await MachineJobSchedulerCreator.CancelJob("Trigger" + index);
            });
        }
    }
}
