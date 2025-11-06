using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Drawing;
using System.IO;
using QIS.Medinfras.Data.Service;
using ThoughtWorks.QRCode.Codec;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewDigitalSignatureCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            string imageStream = string.Empty;

            switch (paramInfo[4])
            {
                case "1" :
                    imageStream = paramInfo[5];
                    break;
                case "2":
                    imageStream = paramInfo[6];
                    break;
                case "3":
                    imageStream = paramInfo[7];
                    break;
                case "4":
                    imageStream = paramInfo[8];
                    break;
                default:
                    break;
            }

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            System.Web.UI.WebControls.Image imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = imageStream;
            plImage.Controls.Add(imgSignature);
        }
    }
}