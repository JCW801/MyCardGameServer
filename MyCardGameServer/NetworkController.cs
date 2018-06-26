using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace MyCardGameServer
{
    public static class NetworkController
    {
        private const int DEFAULT_PORT = 11000;

        /// <summary>
        /// Connect to the server via a provided hostname. 
        /// </summary>
        /// <param name="callbackFunction">a function to be called when a connection is made</param>
        /// <param name="hostname">the name of the server to connect to</param>
        /// <returns></returns>
        public static Socket ConnectToServer(Action<SocketState> callbackFunction, string hostname)
        {
            Debug.WriteLine("connecting  to " + hostname);

            // Connect to a remote device.
            try
            {

                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;
                IPAddress ipAddress = IPAddress.None;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostname);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        Debug.WriteLine("Invalid addres: " + hostname);
                        return null;
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress
                    Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostname);
                }

                // Create a TCP/IP socket.
                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 4000);

                SocketState theServer = new SocketState(socket, -1);

                theServer.CallBackFunction = callbackFunction;

                socket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectToServer, theServer);

                return socket;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return null;
            }
        }

        /// <summary>
        /// Start a TCP listener for new connections and pass the listener.
        /// </summary>
        /// <param name="callMe"></param>
        public static void ServerAwaitingClientLoop(Action<SocketState> callMe)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, DEFAULT_PORT);

            listener.Start();

            TcpState state = new TcpState(listener);

            state.CallBackFunction = callMe;

            listener.BeginAcceptSocket(AcceptNewClient, state);
        }

        /// <summary>
        /// Callback function for the BeginAcceptSocket.
        /// </summary>
        /// <param name="ar"></param>
        private static void AcceptNewClient(IAsyncResult ar)
        {
            TcpState state = (TcpState)ar.AsyncState;

            Socket s = state.TheTcpListener.EndAcceptSocket(ar);

            SocketState ss = new SocketState(s, -1);

            state.CallBackFunction(ss);

            state.TheTcpListener.BeginAcceptSocket(AcceptNewClient, state);
        }

        /// <summary>
        /// This function is called when the remote site acknowledges connect request
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectToServer(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                ss.TheSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            if (ss.CallBackFunction != null)
            {
                ss.CallBackFunction(ss);
            }
        }

        /// <summary>
        /// This function is called when new data arrives.
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            if (ss.TheSocket.Connected == false)
            {
                return;
            }

            try
            {
                int bytesRead = ss.TheSocket.EndReceive(ar);
                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(ss.MessageBuffer, 0, bytesRead);
                    ss.SB.Append(theMessage);
                    if (ss.CallBackFunction != null)
                    {
                        ss.CallBackFunction(ss);
                    }
                }
                else
                {
                    ss.ID = 0;
                }
            }
            catch (Exception e)
            {
                ss.TheSocket.Close();
                return;
            }
        }

        /// <summary>
        /// This is a helper function to get more data.
        /// </summary>
        /// <param name="state"></param>
        public static void getData(SocketState state)
        {
            try
            {
                state.TheSocket.BeginReceive(state.MessageBuffer, 0, state.MessageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to get data. Error occured: " + e);
                return;
            }
        }


        /// <summary>
        /// This function will allow a program to send data over a socket.
        /// </summary>
        public static void Send(SocketState state, String data)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            try
            {
                state.TheSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, state);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to send data. Error occured: " + e);
                return;
            }
        }

        /// <summary>
        /// A callback invoked when a send operation completes
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            ss.TheSocket.EndSend(ar);
            Debug.WriteLine("Data has been sent.");
        }


        /// <summary>
        /// This function (along with its helper 'SendCallback') will allow a program to send data over a socket with a callback function. 
        /// </summary>
        public static void Send(SocketState state, String data, Action<SocketState> callMe)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            state.CallBackFunction = callMe;
            try
            {
                state.TheSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallbackWithFunc, state);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to send data. Error occured: " + e);
                return;
            }
        }

        /// <summary>
        /// A callback invoked when a send operation completes, will invoke a callback function after send end.
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallbackWithFunc(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            ss.TheSocket.EndSend(ar);
            ss.CallBackFunction(ss);
        }


        [DllImport("Iphlpapi.dll")]
        public static extern uint SendARP(uint DestIP, uint SrcIP, ref ulong pMacAddr, ref uint PhyAddrLen);

        /// <summary>
        /// 判断IP是否可以接入
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public static bool IsAccessibleIp(string ipAddr)
        {
            String Mac = "";
            IPAddress ip;
            if (!IPAddress.TryParse(ipAddr, out ip))
            {
                return false;
            }
            uint targetIP = BitConverter.ToUInt32(ip.GetAddressBytes(), 0);
            ulong pMacAddr = 0;
            uint PhyAddrLen = 6;
            try
            {
                uint ecode = SendARP(targetIP, 0, ref pMacAddr, ref PhyAddrLen);
                byte[] ba = BitConverter.GetBytes(pMacAddr);
                Mac = BitConverter.ToString(ba, 0, 6);

            }
            catch
            {
            }

            if (Mac != "" && Mac != "00-00-00-00-00-00")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据本机IP和子网掩码算出可能出现的局域网IP地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableIp()
        {
            IPAddress ip = GetLocalIPAddress();
            IPAddress submask = GetSubnetMask(ip);
            byte[] ipBA = ip.GetAddressBytes();
            byte[] submaskBA = submask.GetAddressBytes();

            byte[] end = { 0, 0, 0, 0 };
            byte[] start = { 0, 0, 0, 0 };


            foreach (int i in Enumerable.Range(0, 4))
            {
                int interval = 0;
                if (submaskBA[i] != 255)
                {
                    interval = 255 - (int)submaskBA[i] + 1;
                }
                byte temp = ipBA[i];

                if (interval != 0)
                {
                    int result = (int)temp & ((byte)((int)ipBA[i] & (int)submaskBA[i]));
                    if (interval != 256)
                    {
                        while (result == (byte)((int)ipBA[i] & (int)submaskBA[i]))
                        {
                            temp++;
                            result = (int)temp & (int)(submaskBA[i]);
                        }

                        end[i] = (byte)((int)temp - 1);
                        start[i] = (byte)((int)end[i] - interval + 1);
                    }
                    else
                    {
                        end[i] = 254;
                        start[i] = 0;
                    }
                }
                else
                {
                    end[i] = (byte)temp;
                    start[i] = end[i];
                }
            }

            List<string> ipList = new List<string>();
            for (byte a = start[0]; a <= end[0]; a++)
            {
                for (byte b = start[1]; b <= end[1]; b++)
                {
                    for (byte c = start[2]; c <= end[2]; c++)
                    {
                        for (byte d = start[3]; d <= end[3]; d++)
                        {
                            ipList.Add(String.Format("{0}.{1}.{2}.{3}", a.ToString(), b.ToString(), c.ToString(), d.ToString()));
                        }
                    }
                }

            }
            return ipList;
        }

        /// <summary>
        /// Get local IP address
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new ArgumentException("No network adapters with an IPv4 address in the system");
        }

        /// <summary>
        /// Get submask of giving ip address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }
    }

    /// <summary>
    /// A class holds all the necessary state to handle a client connection
    /// </summary>
    public class SocketState
    {
        /// <summary>
        /// The Socket of the state.
        /// </summary>
        public Socket TheSocket;
        /// <summary>
        /// The state ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// This is the buffer where we will receive message data from the client
        /// </summary>
        public byte[] MessageBuffer = new byte[1024];

        /// <summary>
        /// This is a larger (growable) buffer, in case a single receive does not contain the full message.
        /// </summary>
        public StringBuilder SB = new StringBuilder();

        /// <summary>
        /// This function should be called after every success connection.
        /// </summary>
        public Action<SocketState> CallBackFunction;

        public SocketState(Socket s, int id)
        {
            TheSocket = s;
            ID = id;
        }
    }

    /// <summary>
    /// An object hold a TcpListener and a delegate
    /// </summary>
    public class TcpState
    {
        /// <summary>
        /// The TcpListener of the state
        /// </summary>
        public TcpListener TheTcpListener;

        /// <summary>
        /// This function should be call after every success connection.
        /// </summary>
        public Action<SocketState> CallBackFunction;

        public TcpState(TcpListener tcpListener)
        {
            TheTcpListener = tcpListener;
        }
    }
}

