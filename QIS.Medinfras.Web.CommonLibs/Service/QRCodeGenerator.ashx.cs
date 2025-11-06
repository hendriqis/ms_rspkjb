using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for QRCodeGenerator
    /// </summary>
    public class QRCodeGenerator : IHttpHandler
    {
        private System.IO.Stream str;
        private string _strmContents;
        private int _strLen;
        protected string MessageResponseError;

        public void ProcessRequest(HttpContext context)
        {
            this._strmContents = context.Request.Params["Data"];
            this._strmContents = (this._strmContents ?? string.Empty);
            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.set_QRCodeEncodeMode(2);
            qRCodeEncoder.set_QRCodeScale(4);
            qRCodeEncoder.set_QRCodeVersion(7);
            qRCodeEncoder.set_QRCodeErrorCorrect(1);
            QRCodeEncoder qRCodeEncoder2 = qRCodeEncoder;
            context.Response.ContentType = "image/jpeg";
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            qRCodeEncoder2.Encode(this._strmContents).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] buffer = memoryStream.ToArray();
            context.Response.BinaryWrite(buffer);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}