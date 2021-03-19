using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Magician.Common.CustomControl;

namespace WLUtility
{
    public partial class FrmMain : MagicianForm
    {
        uint _processId;
        private bool _isRunning;
        private Thread _threadForward;
        private Socket _socketServer;
        private int _num;
        private readonly Dictionary<int, SocketPair> _dicSocketPairs = new Dictionary<int, SocketPair>();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                MessageBox.Show("运行中");
                return;
            }
            Process[] processes = Process.GetProcessesByName("nwlvipcn");
            if(processes.Length == 0)
            {
                processes = Process.GetProcessesByName("wlviptw");
            }
            if (processes.Length > 0)
            {
                _processId = (uint)processes[0].Id;

                DllHelper.SetTargetPid(_processId);
                var result = DllInjector.GetInstance.Inject(_processId, "WLHook.dll");
                if (result == DllInjectionResult.Success)
                {
                    _isRunning = true;
                    _threadForward = new Thread(SocketForward);
                    _threadForward.Start();
                    MessageBox.Show("注入成功");
                }
            }
        }

        private class SocketPair
        {
            public readonly Socket LocalSocket;
            public readonly Socket RemoteSocket;
            public bool IsReceived;
            public readonly object ObjLocker;
            public readonly List<byte> ListBuffer;

            public SocketPair(Socket localSocket, Socket remoteSocket)
            {
                LocalSocket = localSocket;
                RemoteSocket = remoteSocket;
                IsReceived = false;
                ObjLocker = new object();
                ListBuffer = new List<byte>();
            }
        }

        private void SocketForward()
        {
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socketServer.Bind(new IPEndPoint(IPAddress.Any, 6414));
            _socketServer.Listen(int.MaxValue);
            try
            {
                while (_isRunning)
                {
                    var curNum = Interlocked.Increment(ref _num);
                    var localSocket = _socketServer.Accept();
                    ThreadPool.QueueUserWorkItem(SendHandler, curNum);

                    var remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    remoteSocket.Connect(new IPEndPoint(IPAddress.Parse(textBox1.Text), 6414));
                    ThreadPool.QueueUserWorkItem(ReceiveHandler, curNum);

                    _dicSocketPairs.Add(curNum, new SocketPair(localSocket, remoteSocket));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void StopSocketPair(int socketId)
        {
            if (_dicSocketPairs.ContainsKey(socketId))
            {
                var socketPair = _dicSocketPairs[socketId];
                if (socketPair.LocalSocket.Connected)
                {
                    //socketPair.LocalSocket.Disconnect(false);
                    socketPair.LocalSocket.Close();
                    socketPair.LocalSocket.Dispose();
                }
                if (socketPair.RemoteSocket.Connected)
                {
                    //socketPair.RemoteSocket.Disconnect(false);
                    socketPair.RemoteSocket.Close();
                    socketPair.RemoteSocket.Dispose();
                }
                _dicSocketPairs.Remove(socketId);
            }
        }

        public void SendHandler(object obj)
        {
            var socketId = (int)obj;
            while (_isRunning && !_dicSocketPairs.ContainsKey(socketId))
            {
                Thread.Sleep(50);
            }
            if(!_isRunning)return;
            var socketPair = _dicSocketPairs[socketId];
            while (!socketPair.IsReceived)
            {
                Thread.Sleep(50);
            }
            while (socketPair.LocalSocket.Connected && socketPair.RemoteSocket.Connected)
            {
                try
                {
                    var data = new byte[1024];
                    var read = socketPair.LocalSocket.Receive(data);
                    if (read > 0)
                        socketPair.RemoteSocket.Send(data.Take(read).ToArray());
                    else
                        break;
                }
                catch
                {
                    break;
                }
            }
            StopSocketPair(socketId);
        }

        public void ReceiveHandler(object obj)
        {
            var socketId = (int)obj;
            while (_isRunning && !_dicSocketPairs.ContainsKey(socketId))
            {
                Thread.Sleep(50);
            }
            if (!_isRunning) return;
            var socketPair = _dicSocketPairs[socketId];
            while (socketPair.LocalSocket.Connected && socketPair.RemoteSocket.Connected)
            {
                try
                {
                    var data = new byte[1024];
                    var read = socketPair.RemoteSocket.Receive(data);
                    socketPair.IsReceived = true;
                    if (read > 0)
                    {
                        AnalyzePacket(data.Take(read), socketId);
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }
            StopSocketPair(socketId);
        }

        private int SendPacket(SocketPair socketPair, int len, bool isSkip = false)
        {
            len = Math.Min(socketPair.ListBuffer.Count, len);
            if (!isSkip)
                socketPair.LocalSocket.Send(socketPair.ListBuffer.Take(len).ToArray());
            socketPair.ListBuffer.RemoveRange(0, len);
            return len;
        }

        private void AnalyzePacket(IEnumerable<byte> data, int socketId)
        {
            while (true)
            {
                var socketPair = _dicSocketPairs[socketId];
                var isFull = false;
                lock (socketPair.ObjLocker)
                {
                    if (data != null) socketPair.ListBuffer.AddRange(data);

                    var totalLen = socketPair.ListBuffer.LongCount();
                    if (totalLen >= 5)
                    {
                        if ((socketPair.ListBuffer[0] == 0x10 && socketPair.ListBuffer[1] == 0x6B) ||
                            (socketPair.ListBuffer[0] == 0x59 && socketPair.ListBuffer[1] == 0xE9))
                        {
                            int len = (socketPair.ListBuffer[2] ^ 0xAD) + (socketPair.ListBuffer[3] ^ 0xAD) * 0x100 + 4;
                            if (totalLen >= len)
                            {
                                bool isSkip = false;
                                isFull = true;
                                if (len >= 6)
                                {
                                    byte typeA = (byte)(socketPair.ListBuffer[4] ^ 0xAD);
                                    byte typeB = (byte)(socketPair.ListBuffer[5] ^ 0xAD);
                                    if (typeA == 4)
                                    {
                                        len -= 3;
                                        socketPair.ListBuffer.RemoveRange(len, 3);
                                        //isSkip = true;
                                    }
                                    else if(typeA == 1 && typeB >= 30)
                                    {
                                        isSkip = true;
                                    }
                                    else if (typeA == 5 && typeB == 3)
                                    {
                                        len -= 8;
                                        socketPair.ListBuffer.RemoveRange(66, 8);
                                    }
                                    else if(typeA == 11 && typeB == 250)
                                    {
                                        if (typeB == 5)
                                        {
                                            var removeLen = len - 4 - 36;
                                            len -= removeLen;
                                            socketPair.ListBuffer.RemoveRange(41, removeLen - 1);
                                            socketPair.ListBuffer.RemoveRange(6, 1);
                                        }
                                        else if(typeB == 250)
                                        {
                                            var currentPos = 8;
                                            var count = socketPair.ListBuffer[currentPos] ^ 0xAD;
                                            socketPair.ListBuffer.RemoveRange(currentPos, 1);
                                            var removeLen = 1;
                                            for (var i = 0; i < count; i++)
                                            {
                                                currentPos += 30;
                                                var nameLen = socketPair.ListBuffer[currentPos + 14] ^ 0xAD;
                                                socketPair.ListBuffer.RemoveRange(currentPos, 15 + nameLen + 12);
                                                removeLen += 15 + nameLen + 12;
                                            }
                                            len -= removeLen;
                                        }
                                    }
                                    else if (typeA == 14)
                                    {
                                        if(typeB == 5)
                                        {
                                            var removeLen = 0;
                                            var currentPos = 6;
                                            while(currentPos + removeLen < len)
                                            {
                                                currentPos += 4;
                                                var nameLen = socketPair.ListBuffer[currentPos] ^ 0xAD;
                                                currentPos += 1 + nameLen + 14;

                                                //NickName
                                                nameLen = socketPair.ListBuffer[currentPos] ^ 0xAD;
                                                currentPos += 1 + nameLen;

                                                //OrgName
                                                nameLen = socketPair.ListBuffer[currentPos] ^ 0xAD;
                                                currentPos += 1 + nameLen + 4;

                                                //此处缺数据，不确定是Byte(StrLen)+Str还是Byte，暂时认为Byte(StrLen)+Str
                                                nameLen = socketPair.ListBuffer[currentPos] ^ 0xAD;

                                                socketPair.ListBuffer.RemoveRange(currentPos, 1 + nameLen);
                                                removeLen += 1 + nameLen;
                                            }
                                            len -= removeLen;
                                        }
                                    }
                                    else if(typeA == 15 && typeB == 8)
                                    {
                                        var removeLen = 0;
                                        var currentPos = 6;
                                        var count = len / 183;
                                        for (var i = 0; i < count; i++)
                                        {
                                            currentPos += 29;
                                            var nameLen = socketPair.ListBuffer[currentPos] ^ 0xAD;
                                            currentPos += nameLen + 151;
                                            socketPair.ListBuffer.RemoveRange(currentPos + 1, 2);
                                            removeLen += 2;
                                        }
                                        len -= removeLen;


                                    }
                                    byte[] byteLen = BitConverter.GetBytes((short)(len - 4));
                                    socketPair.ListBuffer[2] = (byte)(byteLen[0] ^ 0xAD);
                                    socketPair.ListBuffer[3] = (byte)(byteLen[1] ^ 0xAD);
                                }
                                while (len > 1024)
                                {
                                    len -= SendPacket(socketPair, 1024, isSkip);
                                }

                                SendPacket(socketPair, len, isSkip);
                            }
                        }
                        else
                        {
                            Console.WriteLine(socketPair.ListBuffer[0]);
                            Console.WriteLine(socketPair.ListBuffer[1]);
                        }
                    }
                }

                if (isFull)
                {
                    data = null;
                    continue;
                }

                break;
            }
        }

        private void StopForward()
        {
            _isRunning = false;
            _socketServer?.Close();
            _socketServer = null;
            _threadForward?.Abort();
            _threadForward = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DllInjector.GetInstance.UnInject(_processId, "WLHook.dll") == DllInjectionResult.Success)
            {
                MessageBox.Show("解除注入成功");
            }
            StopForward();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopForward();
        }
    }
}
