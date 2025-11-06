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
    public partial class BSPRI : BaseRpt
    {
        public BSPRI()
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

            lblNoSurkon.Text = string.Format("No. {0}", entity.NoSPRI);
            lblPatientName.Text = entity.PatientName;
            lblDPJP.Text = entity.ParamedicName;
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);

            lblRencanaKontrol.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            
            if(entity.GCSex == Constant.Gender.MALE)
            {
                lblPatientName.Text += " (Laki-Laki)";
            }
            else if (entity.GCSex == Constant.Gender.FEMALE) {
                lblPatientName.Text += " (Perempuan)";            
            }
            if (!string.IsNullOrEmpty(entity.KodeDiagnosa))
            {
                lblDiagnosa.Text = string.Format("{0} - {1}", entity.KodeDiagnosa, entity.NamaDiagnosa);
            }
            else
            {
                lblDiagnosa.Text = "";
            }
             
            lblNoKartu.Text = entity.NHSRegistrationNo;

            lblCetakanKe.Text = String.Format("Tgl.Entri: {0} | Tgl.Cetak: {0} {1}",DateTime.Today.ToString("MM/dd/yy") ,DateTime.Now.ToString("HH:mm:ss tt"));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
        }
    }
}
