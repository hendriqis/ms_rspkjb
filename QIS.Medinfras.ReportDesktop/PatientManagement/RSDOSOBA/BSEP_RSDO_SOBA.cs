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
using System.Linq;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSEP_RSDO_SOBA : BaseRpt
    {
        public BSEP_RSDO_SOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJSInfo2 entity = BusinessLayer.GetvRegistrationBPJSInfo2List(String.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vRegistrationBPJS entityBPJS = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(String.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

            lblHealthcare.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;
            if (entity.ProlanisPRB == null || entity.ProlanisPRB == "")
            {
                lblPRB.Visible = false;
            }
            else
            {
                lblPRB.Text = entity.ProlanisPRB;
            }
            lblKatarak.Text = entity.IsCataract;
            lblSEPNo.Text = entity.NoSEP;
            if (entity.TanggalSEP == null || entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT) == "01-Jan-1900")
                lblSEPDate.Text = "";
            else
                lblSEPDate.Text = entity.TanggalSEPInString;

            lblCardNo.Text = entity.NoPeserta;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.FullName;
            lblDOB.Text = entity.DateOfBirthInString;
            if (entity.Gender == "L")
                lblGender.Text = "Laki-Laki";
            else if (entity.Gender == "P")
                lblGender.Text = "Perempuan";

            string[] namaPoliklinik = entity.NamaPoliklinik.Split('|');
            if (entity.NamaPoliklinik == "" || entity.NamaPoliklinik == null)
            {
                lblPoliTujuan.Text = "";
                lblPoliTujuan.Text = "";
            }
            else
            {
                lblPoliTujuan.Text = namaPoliklinik[1];
                lblPoliTujuan.Text = string.Format("{0} | {1}", entity.NamaPoliklinik, entity.NamaSubSpesialis);
            }
            lblAsalFasKes.Text = entity.NamaRujukan;
            lblDiagnose.Text = entity.NamaDiagnosa;
            tbdpjp.Text = entity.ParamedicName;
            lblPeserta.Text = entity.JenisPeserta;
            lblCOB.Text = "-";
            lblJnsRawat.Text = entity.JenisRawat;
            lblHakKelas.Text = entity.NamaKelasTanggungan;

            lblNoSKDP.Text = entity.NoSuratKontrol;
            lblNoRujukan.Text = entity.NoRujukan;
            if (entity.KodeRujukan != BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.BPJS_CODE).ParameterValue)
            {
                lblTglRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT);
                lblTglPulang.Text = entity.TanggalPulang.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                lblTglRujukan.Text = "-";
                lblTglPulang.Text = " ";
            }
            
            lblCetakanKe.Text = String.Format("Cetakan ke {0} - {1} {2}", entity.PrintNumber, DateTime.Today.ToString("MM/dd/yy"), DateTime.Now.ToString("HH:mm:ss tt"));
            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
            //lblHealthcareNameCell.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;

            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(String.Format("RegistrationID = {0} ORDER BY VisitID ASC", entity.RegistrationID)).LastOrDefault();
            lblQueueNo.Text = entityCV.QueueNo.ToString();
            //lblUserName.Text = AppSession.UserLogin.UserFullName;


            #region QR Codes Image
            string contents = string.Format(@"{0}", entityBPJS.NHSRegistrationNo);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 7;
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
                    ////pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                    qrImg.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion
            base.InitializeReport(param);
        }
    }
}
