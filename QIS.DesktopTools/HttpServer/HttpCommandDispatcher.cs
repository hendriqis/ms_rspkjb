// MedinfrasDesktopTools
//
// by Mark Gladding
// Copyright 2009 Tumbywood Software
// http://www.text2go.com
//
// You are free to reuse this code in any commercial or non-commercial work.
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using QIS.DesktopTools;

namespace HttpServer
{
    public class HttpCommandDispatcher
    {
        public HttpCommandDispatcher(System.Drawing.Image dummyImage)
        {
            MemoryStream stream = new MemoryStream();
            dummyImage.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
            mDummyGif = stream.GetBuffer();
        }

        public event RequestReceivedHandler RequestReceived;

        public void Start(string url)
        {
            try
            {
                if (!HttpListener.IsSupported)
                {
                    MessageBox.Show("Windows XP SP2 or Server 2003 is required.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                mListener = new HttpListener();
                // Add the prefix.
                mListener.Prefixes.Add(url);
                mListener.Start();
                mListener.BeginGetContext(new AsyncCallback(this.ProcessRequest), null);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Unable to start the service.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void End()
        {
            if (mListener != null)
            {
                HttpListener listener = mListener;
                mListener = null;
                listener.Close();
            }
        }
        public void AddCommand(HttpCommand command)
        {
            Debug.Assert(!mCommands.ContainsKey(command.Name));
            mCommands[command.Name] = command;
        }

        public void AddResourceLocator(ResourceLocator locator)
        {
            foreach (string extension in locator.Extensions)
            {
                Debug.Assert(!mResourceLocators.ContainsKey(extension));
                mResourceLocators[extension] = locator;
            }
        }

        public void ProcessRequest(IAsyncResult result)
        {
            if (mListener == null)
            {
                return; // Listener is has  been closed
            }
            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context = mListener.EndGetContext(result);
            try
            {
                HttpListenerRequest request = context.Request;
                string queryString = string.Empty;
                int urlEndPos = request.RawUrl.IndexOf('?');
                if (request.HttpMethod == "GET")
                {
                    if (urlEndPos != -1 && urlEndPos < request.RawUrl.Length - 1)
                    {
                        queryString = request.RawUrl.Substring(urlEndPos + 1);
                    }
                }
                else
                {
                    queryString = GetQueryStringFromPostData(request);
                }
                NameValueCollection queryCollection = SplitNameValuePairs(queryString);

                string type = queryCollection["type"];

                Process process = new Process();
                if (type == "report")
                {
                    string healthcareID = queryCollection["healthcareid"];
                    string userID = queryCollection["userid"];
                    string userName = queryCollection["username"];
                    string userFullName = queryCollection["userfullname"];

                    string reportID = queryCollection["reportid"];
                    string param = queryCollection["param"];
                    //string fileName = ConfigurationManager.AppSettings["ReportViewerApp"];
                    //string fileName = string.Format(@"{0}\Viewer\QISReportViewer.exe", Application.StartupPath);

                    string[] args = { healthcareID, userID, userName, userFullName, reportID, param };


                    //ProcessStartInfo processInfo = new ProcessStartInfo();
                    //processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //processInfo.FileName = "cmd.exe";
                    //processInfo.Arguments = string.Format("/c START \"\" \"{0}\" \"{1}\"", fileName, String.Join("\" \"", args));
                    //Process.Start(processInfo);

                    ReportViewer viewer = new ReportViewer(args);
                    viewer.ShowDialog();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Command failed.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mListener.BeginGetContext(new AsyncCallback(this.ProcessRequest), null);
            }
        }

        private static NameValueCollection SplitNameValuePairs(string queryString)
        {
            NameValueCollection queryCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] args = queryString.Split(new char[] { '&' });
                for (int i = 0; i < args.Length; i++)
                {
                    int separatorPos = args[i].IndexOf('=');
                    if (separatorPos > 0 && separatorPos < args[i].Length)
                    {
                        queryCollection[args[i].Substring(0, separatorPos)] = System.Web.HttpUtility.UrlDecode(args[i].Substring(separatorPos + 1), System.Text.Encoding.UTF8);
                    }
                }
            }
            return queryCollection;
        }

        private static string GetQueryStringFromPostData(HttpListenerRequest request)
        {
            string queryString = string.Empty;
            if (request.HasEntityBody)
            {
                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                queryString = reader.ReadToEnd();
                body.Close();
                reader.Close();
            }
            return queryString;
        }

        private void OnRequestReceived(string request)
        {
            if (RequestReceived != null)
            {
                RequestReceived(this, new RequestEventArgs(request));
            }
        }

        public delegate void RequestReceivedHandler(object source, RequestEventArgs e);

        private HttpListener mListener;
        private Dictionary<string, HttpCommand> mCommands = new Dictionary<string, HttpCommand>();
        private Dictionary<string, ResourceLocator> mResourceLocators = new Dictionary<string, ResourceLocator>();
        private byte[] mDummyGif;
    }
}
