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
    public partial class BSEP_RSRTH : BaseRpt
    {
        public BSEP_RSRTH()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];
           
            vConsultVisit10 oConsultVisit = BusinessLayer.GetvConsultVisit10List(string.Format("RegistrationID='{0}'", entity.RegistrationID)).FirstOrDefault();
            if (oConsultVisit != null) {
                vHealthcareServiceUnit oHsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID={0}", oConsultVisit.HealthcareServiceUnitID)).FirstOrDefault();
                if (oHsu != null) {
                    lblHealthcareName.Text = oHsu.HealthcareName;
                    lblKeterangan1.Text = string.Format("memberikan akses informasi medis atau riwayat pelayanan kepada dokter/tenaga medis pada {0} untuk kepentingan pemeliharaan kesehatan, pengobatan, penyembuhan, dan perawatan Pasien", oHsu.HealthcareName);

                }
                string mobilephone = string.Format("{0}", oConsultVisit.MobilePhoneNo1);

                lblPhoneNo.Text = mobilephone;
            }
            lblParamedicName.Text = entity.ParamedicName;
            lblProlarisPRB.Text = entity.ProlanisPRB;
            lblKlsHak.Text = entity.NamaKelasTanggungan;
            lblKlsRawat.Text = entity.ChargeClassName;

            StringBuilder sb = new StringBuilder();
           
            if (!string.IsNullOrEmpty(entity.VClaimTujuanKunjunganSCName)) {
                sb.Append(string.Format("- {0}\r\n", entity.VClaimTujuanKunjunganSCName));
            }
            if (!string.IsNullOrEmpty(entity.VClaimProsedurSCName))
            {
                sb.Append(string.Format("- {0}\r\n", entity.VClaimProsedurSCName));
            }
            //if (!string.IsNullOrEmpty(entity.VClaimProsedurPenunjangSCName))
            //{
            //    sb.Append(string.Format("- {0}\r\n", entity.VClaimProsedurPenunjangSCName));
            //}
            //if (!string.IsNullOrEmpty(entity.VClaimAssessmenPelayananSCName))
            //{
            //    sb.Append(string.Format("- {0}\r\n", entity.VClaimAssessmenPelayananSCName));
            //}
            lblJnsKunjungan.Text = sb.ToString(); 
            //if (entity.NoSKTM != "")
            //{
            //    lblNoSKTM.Text = string.Format("No. SKTM : {0}", entity.NoSKTM);
            //}
            //else
            //{
            //    lblNoSKTM.Text = "";
            //}
            //if (entity.ProlanisPRB != "")
            //{
            //    lblPRB.Text = string.Format("{0}", entity.ProlanisPRB);
            //}
            //else
            //{
            //    lblPRB.Text = "";
            //}

            if (entity.IsCataract == true)
            {
                lblKatarak.Text = string.Format("* PASIEN OPERASI KATARAK");
            }
            else
            {
                lblKatarak.Text = "";
            }

            lblSEPNo.Text = entity.NoSEP;
            lblSEPDate.Text = entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT);
            lblCardNo.Text = entity.NHSRegistrationNo;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.PatientName;
            lblPatientNamettd.Text = entity.PatientName; 
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            
            if(entity.GCSex == Constant.Gender.MALE)
            {
                lblGender.Text = "Laki-laki";
            }
            else if (entity.GCSex == Constant.Gender.FEMALE) {
                lblGender.Text = "Perempuan";            
            }
             
            lblPoliTujuan.Text = entity.NamaSubSpesialis;
            if (!string.IsNullOrEmpty(lblPoliTujuan.Text)) {
                lblPoliTujuan.Text = "-";
            }
            lblAsalFasKes.Text = entity.NamaRujukan;
            //if (!string.IsNullOrEmpty(entity.NamaRujukan))
            //{
            //    lblAsalFasKes.Text = entity.NamaRujukan;
            //}
            //else
            //{
            //    lblAsalFasKes.Text = string.Format("{0} ", entity.NamaPPK);
            //}
            lblDiagnose.Text = entity.NamaDiagnosa;
            lblRemarks.Text = entity.Catatan;
            lblPeserta.Text = entity.JenisPeserta;
          ////  lblCOB.Text = "";
            lblJnsRawat.Text = entity.JenisRawat;
            lblPoliTujuan.Text = entity.NamaPoliRujukanKe;
            lblPoliPerujuk.Text = entity.NamaPoliklinik;
            if (lblPoliPerujuk.Text == "|") {
                lblPoliPerujuk.Text = "-";
            }
          /////  lblKlsRawat.Text = entity.BPJSClassName;
            
            string[] AccidentPayer = entity.AccidentPayer.Split(',');
            string AccidentPayerInString = "";
            if (AccidentPayer.Length > 0)
            {
                for (int i = 0; i < AccidentPayer.Length; i++)
                {
                    if (AccidentPayer[i] == "1")
                    {
                        AccidentPayerInString += "BPJS";
                    }
                    if (AccidentPayer[i] == "2")
                    {
                        AccidentPayerInString += ", JASA RAHARJA";
                    }
                    if (AccidentPayer[i] == "3")
                    {
                        AccidentPayerInString += ", TASPEN";
                    }
                    if (AccidentPayer[i] == "4")
                    {
                        AccidentPayerInString += ", ASABRI";
                    }
                }
            }

            lblAccidentPayer.Text = AccidentPayerInString;

            lblCetakanKe.Text = String.Format("Cetakan ke {0} - {1} {2}" ,entity.PrintNumber ,DateTime.Today.ToString("MM/dd/yy") ,DateTime.Now.ToString("HH:mm:ss tt"));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");


            #region QR Codes Image
            //string contents = string.Format(@"{0}", entity.NHSRegistrationNo);

            //QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            //qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //qRCodeEncoder.QRCodeScale = 4;
            //qRCodeEncoder.QRCodeVersion = 7;
            //qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            //MemoryStream memoryStream = new MemoryStream();
            ////System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            ////imgBarCode.Height = 400;
            ////imgBarCode.Width = 400;

            //using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        //byte[] byteImage = ms.ToArray();
            //        //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            //        ////pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
            //        qrImg.Image = System.Drawing.Image.FromStream(ms, true, true);
            //    }
            //}
            #endregion
        }
    }
}
