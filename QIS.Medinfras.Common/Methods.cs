using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace QIS.Medinfras.Common
{
    public class CommonMethods
    {
        public static String FormatMRN(Int32 MRN)
        {
            String DefaultMRN = "00-00-00-00";
            char SplitChar = '-';
            String MedicalNo = MRN.ToString();
            int ctr = MedicalNo.Length - 1;
            for (int i = DefaultMRN.Length - 1; i >= 0; i--)
            {
                if (DefaultMRN[i] == SplitChar)
                    continue;
                else
                {
                    if (ctr >= 0)
                    {
                        DefaultMRN = DefaultMRN.Remove(i,1);
                        DefaultMRN = DefaultMRN.Insert(i, MedicalNo[ctr].ToString());
                        ctr--;
                    }
                    else
                        break;
                }
            }

            return DefaultMRN;
        }

        public static String FormatRegistrationNo(String RegistrationNo)
        {
            String DefaultRegNo = "OPR/00000000/00000";
            char SplitChar = '/';
            String RegNo = RegistrationNo.ToString();
            int ctr = RegNo.Length - 1;
            for (int i = DefaultRegNo.Length - 1; i >= 0; i--)
            {
                if (DefaultRegNo[i] == SplitChar)
                    continue;
                else
                {
                    if (ctr >= 0)
                    {
                        DefaultRegNo = DefaultRegNo.Remove(i, 1);
                        DefaultRegNo = DefaultRegNo.Insert(i, RegNo[ctr].ToString());
                        ctr--;
                    }
                    else
                        break;
                }
            }

            return DefaultRegNo;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string SendMessageToListener(string ipaddress, string port, string message)
        {
            string result = string.Format("{0}|{1}", "1", "OK");

            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipaddress), Convert.ToInt16(port));
            // Retrieve the network stream. 
            NetworkStream stream = client.GetStream();
            // Create a BinaryWriter for writing to the stream. 
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.UTF8))
            {                 
                w.Write(string.Format("{0}", message).ToCharArray());

                #region Receive ACK Response From Server
                using (BinaryReader r = new BinaryReader(stream, Encoding.GetEncoding(1252)))
                {
                    // Reads NetworkStream into a byte buffer.
                    int length = (int)client.ReceiveBufferSize;
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    stream.Read(buffer, 0, length);

                    string data = Encoding.UTF8.GetString(buffer);

                    // Find start of MLLP frame, a VT character ...
                    int start = data.IndexOf((char)0x0B);
                    if (start >= 0)
                    {
                        //Look for the end of the frame, a FS Character
                        int end = data.IndexOf((char)0x1C);
                        if (end > start)
                        {
                            string temp = data.Substring(start + 1, end - start);
                            result = ResponseToACKMessage(temp);
                        }
                    }
                }
                #endregion
            }

            return result;
        }

        private static string ResponseToACKMessage(string data)
        {
            string result = string.Empty;

            var msg = new HL7MessageText();
            msg.Parse(data);

            HL7Segment msa = msg.FindSegment("MSA");
            result = msa.Field(1) == "AA" ? string.Format("{0}|ACK: OK ({1})|", "1", msa.Field(2)) : string.Format("{0}|ACK: ERR ({1})|", "0", msa.Field(2),msa.Field(3));

            return result;
        }
    }
}
