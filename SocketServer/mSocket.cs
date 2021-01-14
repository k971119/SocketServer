
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer
{
    class mSocket
    {
        private Socket _WorkSocket = null;

        private IPAddress ipAddress = null;
        private int port = 0;

        DataBaseManager.DBManagement db = new DataBaseManager.DBManagement("192.168.0.116", "Chat", "sa", "admin123!@#");

        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br><br></br>
        /// 소켓 서버 생성자
        /// </summary>
        public mSocket()
        {
            XMLCommand.Command cmd = new XMLCommand.Command(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ServerConfig.xml");
            Console.WriteLine(cmd._sPath);
            ipAddress = IPAddress.Parse(cmd.Read("serverIP"));
            port = Convert.ToInt32(cmd.Read("port"));
            _WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// 클라이언트 연결을 위한 서버 대기 시작
        /// </summary>
        public void StartServer()
        {
            IPEndPoint serverEP = new IPEndPoint(ipAddress, port);
            _WorkSocket.Bind(serverEP);
            _WorkSocket.Listen(10);
            try
            {
                _WorkSocket.BeginAccept(AcceptCallback, null);          //비동기 클라이언트 연결요청 대기
            }
            catch
            {
                Console.WriteLine(string.Format("{0} 클라이언트 종료됨",_WorkSocket.RemoteEndPoint.ToString()));
            }
            Console.WriteLine(string.Format("클라이언트 연결대기 ---- IP : {0}   -----port : {1}", ipAddress, port));
        }

        List<Socket> clients = new List<Socket>();

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// 비동기 클라이언트 연결 요청 승인
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket client = _WorkSocket.EndAccept(ar);              //클라이언트 연결요청 수락

            _WorkSocket.BeginAccept(AcceptCallback, null);          //신규클라이언트 연결요청 대기

            AsyncObject obj = new AsyncObject(8192);              //버퍼 사이즈 지정
            obj.WorkingSocket = client;                                 //연결된 클라이언트 저장

            clients.Add(client);        //클라이언트 연결리스트에 추가

            client.BeginReceive(obj.Buffer, 0, 8192, 0, DataReceived, obj);

            Console.WriteLine(string.Format(@"클라이언트 {0}이(가) 연결되었습니다.", client.RemoteEndPoint));
            db.InsertDB(string.Format("INSERT INTO CHAT(LOG_ADDRESS, LOG_CONNECTION, LOG_DATETIME) VALUES('{0}',{1},GETDATE())",obj.WorkingSocket.RemoteEndPoint.ToString(),1));
        }

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// 클라이언트에서 데이터를 수신받아 연결된 나머지 클라이언트에 송신
        /// </summary>
        /// <param name="ar"></param>
        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;                   //데이터 가공

            try
            {
                int receiver = obj.WorkingSocket.EndReceive(ar);                //클라이언트 수신종료

                if (receiver <= 0)
                {
                    obj.WorkingSocket.Close();
                }

                string text = Encoding.UTF8.GetString(obj.Buffer);
                string[] tokens = text.Split('\x01');
                string user = tokens[0];
                string msg = tokens[1];

                Console.WriteLine(string.Format("{0} :  {1}", obj.WorkingSocket.RemoteEndPoint.ToString(), msg));          //서버에 메세지 출력

                int index = 0;
                foreach (Socket client in clients)
                {
                    if (client != obj.WorkingSocket)             //메세지를 보낸 클라이언트를 제외한 모든 클라이언트에 메세지를 보냄
                    {
                        try
                        {
                            client.Send(obj.Buffer);
                            Console.WriteLine(string.Format("{0}에게 전달완료", client.RemoteEndPoint.ToString()));
                        }
                        catch       //전송이 실패할경우 연결이 끊어짐으로 인식해 전송을 취소하고 연결리스트에서 제거한다.
                        {
                            try { client.Dispose(); } catch { }
                            clients.RemoveAt(index);
                        }
                        finally
                        {
                            index++;
                        }
                    }
                }

                obj.ClearBuffer();                                                                        //수신 버퍼를 비운후 재대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 8192, 0, DataReceived, obj);      //클라이언트 수신 대기
            }
            catch (SocketException se)                                                                  //클라이언트 강제 끊김 예외처리
            {
                Console.WriteLine(string.Format("<{0}> 클라이언트 종료 ", obj.WorkingSocket.RemoteEndPoint.ToString()));
                db.InsertDB(string.Format("INSERT INTO CHAT(LOG_ADDRESS, LOG_CONNECTION, LOG_DATETIME) VALUES('{0}',{1},GETDATE())", obj.WorkingSocket.RemoteEndPoint.ToString(), 0));
                obj.WorkingSocket.Close();
            }
        }
    }

    /// <summary>
    /// 비동기 소켓을 위한 오브젝트
    /// </summary>
    class AsyncObject
    {
        public byte[] Buffer;
        public Socket WorkingSocket;
        public readonly int BufferSize;

        /// <summary>
        /// 버퍼 초기화
        /// </summary>
        /// <param name="bufferSize"></param>
        public AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }

        /// <summary>
        /// 버퍼 피우기
        /// </summary>
        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }
    }
}
