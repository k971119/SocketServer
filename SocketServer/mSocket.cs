
using System;

namespace SocketServer
{
    class mSocket
    {
        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br><br></br>
        /// 소켓 초기 설정
        /// </summary>
        public mSocket()
        {
            XMLCommand.Command cmd = new XMLCommand.Command(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ServerConfig.xml");
            cmd.Write("SERVER_IP", "192.168.0.116");
            cmd.Write("PORT", "9999");
        }

        public bool StandBy()
        {
            return false;
        }
    }
}
