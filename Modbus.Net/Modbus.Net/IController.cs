namespace Modbus.Net
{
    /// <summary>
    ///     ����������ӿ�
    /// </summary>
    public interface IController
    {
        /// <summary>
        ///     ������Ϣ
        /// </summary>
        /// <param name="sendMessage">��Ҫ���͵���Ϣ</param>
        /// <returns></returns>
        MessageWaitingDef AddMessage(byte[] sendMessage);

        /// <summary>
        ///     ������������߳�
        /// </summary>
        void SendStart();

        /// <summary>
        ///     �رմ�������߳�
        /// </summary>
        void SendStop();

        /// <summary>
        ///     ������д����͵���Ϣ
        /// </summary>
        void Clear();

        /// <summary>
        ///     �����ص���Ϣ�󶨵����͵���Ϣ�ϣ�������Ϣ����ȷ��
        /// </summary>
        /// <param name="receiveMessage">���ص���Ϣ</param>
        /// <returns>�Ƿ�����ȷ��</returns>
        bool ConfirmMessage(byte[] receiveMessage);

        /// <summary>
        ///     û���κη���ʱǿ��ɾ���ȴ������ϵ���Ϣ
        /// </summary>
        /// <param name="def">��Ҫǿ��ɾ������Ϣ</param>
        void ForceRemoveWaitingMessage(MessageWaitingDef def);
    }
}
