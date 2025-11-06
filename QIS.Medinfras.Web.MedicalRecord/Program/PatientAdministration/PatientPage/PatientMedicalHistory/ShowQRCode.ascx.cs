using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Drawing;
using System.IO;
using QIS.Medinfras.Web.Common;
using QRCoder;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ShowQRCode : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtExpiredDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            String code = "Michael Andhika Mahendra Pramata|00-45-11-20"; // HL7 Message
            string expireDate = Helper.GetDatePickerValue(txtExpiredDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112); //yyyyMMdd
            string payerCode = string.Empty;
            switch (txtPayer.Text)
            {
                case "BPJS" :
                    payerCode = txtPayer.Text;
                    break;
            }


            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            System.Web.UI.WebControls.Image imgQR = new System.Web.UI.WebControls.Image();
            imgQR.Height = 200;
            imgQR.Width = 200;

            using (Bitmap bitMap = qrCodeImage)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgQR.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                plBarCode.Controls.Add(imgQR);
            }
        }
    }
}