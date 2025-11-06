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
    public partial class BSuratPengantarRawatInapRSP : BaseCustomDailyPotraitRpt
    {
        public BSuratPengantarRawatInapRSP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)

        {

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit cv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", entity.RegistrationID)).FirstOrDefault();
            vPatientDiagnosis pdiagnos = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID={0} AND GCDiagnoseType='{1}' AND IsDeleted=0 ORDER BY ID DESC ", cv.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();

            if (pdiagnos.DiagnoseID == null && pdiagnos.DiagnoseID.ToString() == "")
                lblDianosis.Text = string.Format("{0}", pdiagnos.DiagnosisText);
            else
                lblDianosis.Text = string.Format("{0}", pdiagnos.DiagnoseName);


            lbParamedicVisit.Text = string.Format("({0})", entity.ParamedicName);
           lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            

            base.InitializeReport(param);
        }

      
    }
}
