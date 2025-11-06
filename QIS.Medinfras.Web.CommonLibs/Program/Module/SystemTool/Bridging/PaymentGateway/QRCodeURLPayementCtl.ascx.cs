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
    public partial class QRCodeURLPayementCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            //vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit=1", AppSession.RegisteredPatient.RegistrationID)).First();
            hdnUrlPayment.Value = param;
            if (!string.IsNullOrEmpty(hdnUrlPayment.Value))
            {
                //string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                //    oVisit.MedicalNo, oVisit.RegistrationNo, oVisit.FirstName, oVisit.MiddleName, oVisit.LastName,oVisit.cfPatientLocation);
                string contents = hdnUrlPayment.Value;
                QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qRCodeEncoder.QRCodeScale = 4;
                qRCodeEncoder.QRCodeVersion = 7;
                qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                imgBarCode.Height = 400;
                imgBarCode.Width = 400;

                using (Bitmap bitMap = qRCodeEncoder.Encode(contents,System.Text.Encoding.UTF8))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    }
                    plBarCode.Controls.Add(imgBarCode);
                }              
            }
        }
    }
}

