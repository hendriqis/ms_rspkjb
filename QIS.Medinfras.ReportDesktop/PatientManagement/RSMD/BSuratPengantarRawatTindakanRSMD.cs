using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;
namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPengantarRawatTindakanRSMD : BaseCustomDailyPotraitRpt
    {
        public BSuratPengantarRawatTindakanRSMD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            Registration entity = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit entityCv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vPatientDiagnosis entityD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", entityCv.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCv.ParamedicID)).FirstOrDefault();
            if (entityD != null)
            {
                if (entityD.DiagnoseID == null && entityD.DiagnoseID.ToString() == "")
                {
                    lblDiagnosis.Text = string.Format(".............................................................................................................................................");
                }
                else
                {
                    lblDiagnosis.Text = string.Format("{0}", entityD.DiagnosisText);
                }
            }
            else
            {
                lblDiagnosis.Text = string.Format(".............................................................................................................................................");
            }


            lbParamedicVisit.Text = string.Format("({0})", entityPM.FullName);
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));


            base.InitializeReport(param);
        }


    }
}
