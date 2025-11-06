using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using ThoughtWorks.QRCode.Codec;
using System.IO;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKontrolBPJSRSBL : BaseRpt
    {
        public BSuratKontrolBPJSRSBL()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];
            vAppointment entityApp = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entity.AppointmentID))[0];

            lblHealthcareName.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;

            lblControlNo.Text = entity.NoSuratRencanaKontrolBerikutnya;
            lblCardNo.Text = entity.NHSRegistrationNo;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.PatientName;
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblParamedicName.Text = entityApp.ParamedicName;
            lblControlDate.Text = entity.TanggalRencanaKontrol.ToString(Constant.FormatString.DATE_FORMAT);
            lblAppointmentNo.Text = entityApp.AppointmentNo;

            if (entity.DepartmentID == Constant.Facility.INPATIENT)
            {
                txtRemarks.Text = string.Format("POST RANAP");
            }
            else
            {
                txtRemarks.Text = "";
            }

            //logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");

            #region BarcodeAntrian
            string contents = string.Format(@"{0}", entityApp.AppointmentNo);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 0;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 400;
            //imgBarCode.Width = 400;

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion
        }
    }
}
