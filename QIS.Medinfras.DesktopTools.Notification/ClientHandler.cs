using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Media;

namespace QIS.Medinfras.DesktopTools.Notification
{
    // A class that encapsulates the logic to handle a client connection. 
    public class ClientHandler
    {
        // The TcpClient that represents the connection to the client. 
        private TcpClient client;
        // An ID that uniquely identifies this ClientHandler. 
        private string ID;
        private NotifyIcon ni;

        internal ClientHandler(TcpClient client, string ID, NotifyIcon ni)
        {
            this.client = client;
            this.ID = ID;
            this.ni = ni;
            // Create a new background thread to handle the client connection 
            // so that we do not consume a thread-pool thread for a long time 
            // and also so that it will be terminated when the main thread ends. 
            Thread thread = new Thread(ProcessConnection);
            thread.IsBackground = true;
            thread.Start();
        }

        private void ProcessConnection()
        {
            using (client)
            {

                // Uses the GetStream public method to return the NetworkStream.
                NetworkStream netStream = client.GetStream();
                
                // Create a BinaryReader to receive messages from the client. At 
                // the end of the using block, it will close both the BinaryReader 
                // and the underlying NetworkStream. 
                using (BinaryReader reader = new BinaryReader(netStream,Encoding.GetEncoding(1252)))
                {
                    // Reads NetworkStream into a byte buffer.
                    int length = (int)client.ReceiveBufferSize;
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    // Read can return anything from 0 to numBytesToRead. 
                    // This method blocks until at least one byte is read.
                    int count;
                    int sum = 0;
                    while ((count = netStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;
                    }

                     //Returns the data received from the host to the console.
                    string message = Encoding.UTF8.GetString(buffer);

                    if (!string.IsNullOrEmpty(message))
                    {
                        SoundPlayer player = new SoundPlayer(string.Format(@"{0}\assets\sounds\order.wav",Application.StartupPath));

                        string msgText = FormatMessage(message);
                        ni.BalloonTipIcon = ToolTipIcon.Info;
                        ni.BalloonTipTitle = string.Format("{0} {1} {2}", this.ID, DateTime.Now.Date.ToString("yyyy-mm-dd"),DateTime.Now.ToString("HH:mm:ss"));
                        ni.BalloonTipText = string.Format(@"{0}", msgText);
                        ni.ShowBalloonTip(5000);

                        player.Play();

                        MessageData messageInfo = new MessageData()
                        {
                            Sender = this.ID,
                            MessageDate = DateTime.Now.ToString("yyyy-mm-dd"),
                            MessageTime = DateTime.Now.ToString("hh:mm:ss"),
                            MessageType = "Plain Text",
                            MessageOriginalText = message,
                            MessageText = string.Format(@"{0}", msgText)
                        };
                        Global.LastMessage = messageInfo;
                        Global.MessageList.Add(messageInfo);
                        
                    }
                }
            }
        }

        private string FormatMessage(string message)
        {
            string result = message.Replace("\\v", "");
            return result;
        }
    }
}
