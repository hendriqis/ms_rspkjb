using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace QIS.Medinfras.DesktopTools.Notification
{
    public class Listener
    {
        // A flag used to indicate whether the server is shutting down. 
        private static bool _isListening = false;
        public bool IsListening
        {
            get { return _isListening; }
            set { _isListening = value; }
        }

        #region A variable to track the identity of each client connection.
        private static int _clientNumber = 0;
        public static int ClientNumber
        {
            get { return _clientNumber; }
            set { _clientNumber = value; }
        }
        #endregion

        #region A single TcpListener will accept all incoming client connections.
        private static TcpListener _server = null;
        private static string _ipAddress;
        public string IpAddress
        {
            get { return Listener._ipAddress; }
            set { Listener._ipAddress = value; }
        }
        private static int _port;
        public int Port
        {
            get { return Listener._port; }
            set { Listener._port = value; }
        }
        #endregion

        #region Notify Icon
        private static NotifyIcon _notifyInfo;

        public NotifyIcon NotifyInfo
        {
            get { return Listener._notifyInfo; }
            set { Listener._notifyInfo = value; }
        } 
        #endregion

        public Listener()
        {

        }

        public void StartListener(string ipAddress, int port)
        {
            try
            {
                // Create a TcpListener that will accept incoming client 
                // connections on endpoint which is defined in system configuration
                _server = new TcpListener(IPAddress.Parse(ipAddress), Convert.ToInt16(port));
                //AddEventLog("Starting listener...");
                _server.Start();
                //btnStart.Enabled = false;
                //btnStop.Enabled = true;
                //txtIPAddress.Enabled = false;
                //txtPort.Enabled = false;
                _isListening = true;

                // Begin asynchronously listening for client connections. When a 
                // new connection is established, call the ConnectionHandler 
                // method to process the new connection.
                _server.BeginAcceptTcpClient(ConnectionHandler, null);

                // Keep the server active until the user presses Stop
                //AddEventLog("Awaiting for a connection...");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void StopListener()
        {
            _server.Stop();
            _isListening = false;
        }

        // A method to handle the callback when a connection is established 
        // from a client. This is a simple way to implement a dispatcher 
        // but lacks the control and scalability required when implementing 
        // full-blown asynchronous server applications. 
        private static void ConnectionHandler(IAsyncResult result)
        {
            TcpClient client = null;
            // Always end the asynchronous operation to avoid leaks. 
            try
            {
                // Get the TcpClient that represents the new client connection. 
                client = _server.EndAcceptTcpClient(result);
            }
            catch (ObjectDisposedException)
            {
                // Server is shutting down and the outstanding asynchronous 
                // request calls the completion method with this exception. 
                // The exception is thrown when EndAcceptTcpClient is called. 
                // Do nothing and return. 
                return;
            }

            // Begin asynchronously listening for the next client 
            // connection. 
            _server.BeginAcceptTcpClient(ConnectionHandler, null);

            if (client != null)
            {
                // Determine the identifier for the new client connection. - Get Client IP Address
                string clientID = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                // Create a new ClientHandler to handle this connection.
                new ClientHandler(client, clientID, _notifyInfo);
            }
        }
    }


}
