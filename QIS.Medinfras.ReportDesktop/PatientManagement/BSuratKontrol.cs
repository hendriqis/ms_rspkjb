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
    public partial class BSuratKontrol : BaseRpt
    {
        public BSuratKontrol()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];
            
            ///lblHealthcareName.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName; //AD : karna ada proses generate pdf ga bisa baca session
           
            vConsultVisit10 oConsultVisit = BusinessLayer.GetvConsultVisit10List(string.Format("RegistrationID='{0}'", entity.RegistrationID)).FirstOrDefault();
            if (oConsultVisit != null) {
                vHealthcareServiceUnit oHsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID={0}", oConsultVisit.HealthcareServiceUnitID)).FirstOrDefault();
                if (oHsu != null) {
                    lblHealthcareName.Text = oHsu.HealthcareName;
                }
            }

            lblBarcode.Visible = false;
            if (!string.IsNullOrEmpty(entity.NoSuratRencanaKontrolBerikutnya))
            {
                lblNoSurkon.Text = string.Format("No. {0}", entity.NoSuratRencanaKontrolBerikutnya);
                lblBarcode.Visible = true;
                lblBarcode.Text = entity.NoSuratRencanaKontrolBerikutnya;
            }
            lblDPJP.Text = entity.ParamedicName;
            if (!string.IsNullOrEmpty(entity.SpecialtyBPJSReferenceInfo))
            {
                lblDPJPSpecialty.Text = entity.SpecialtyBPJSReferenceInfo;
            }
            else
            {
                if (!string.IsNullOrEmpty(entity.SpecialtyID))
                {
                    Specialty entitySpecialty = BusinessLayer.GetSpecialtyList(string.Format("SpecialtyID = '{0}'", entity.SpecialtyID)).FirstOrDefault();
                    if (entitySpecialty != null)
                    {
                        lblDPJPSpecialty.Text = entitySpecialty.SpecialtyName;
                    }
                    else
                    {
                        lblDPJPSpecialty.Text = "";
                    }
                }
                else
                {
                    lblDPJPSpecialty.Text = "";
                }
            }
            lblPatientName.Text = entity.PatientName;

            if (entity.kodeDPJPKonsulan != null)
            {
                lblDPJPYth.Text = entity.NamaDPJPKonsulan;
            }
            else
            {
                lblDPJPYth.Text = entity.ParamedicName;
            }
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            if (!string.IsNullOrEmpty(entity.KodeDiagnosa))
            {
                lblDiagnosa.Text = string.Format("{0} - {1}", entity.KodeDiagnosa, entity.NamaDiagnosa);
            }
            else
            {
                lblDiagnosa.Text = "";
            }
            lblRencanaKontrol.Text = string.Format("{0}", entity.TanggalRencanaKontrol.ToString(Constant.FormatString.DATE_FORMAT));

            if (entity.GCSex == Constant.Gender.MALE)
            {
                lblPatientName.Text += " (Laki-Laki)";
            }
            else if (entity.GCSex == Constant.Gender.FEMALE)
            {
                lblPatientName.Text += " (Perempuan)";
            }
             
            lblNoKartu.Text = entity.NHSRegistrationNo;

            lblCetakanKe.Text = String.Format("Tgl.Cetak: {0} {1}",DateTime.Today.ToString("MM/dd/yy") ,DateTime.Now.ToString("HH:mm:ss tt"));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");

            #region QR Codes Image
            string contents = string.Format(@"{0}", entity.NHSRegistrationNo);

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
        }
    }
}
