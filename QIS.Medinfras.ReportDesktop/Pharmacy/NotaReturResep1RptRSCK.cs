using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class NotaReturResep1RptRSCK : BaseReceipt1Rpt
    {
        public NotaReturResep1RptRSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHdList(param[0])[0];
            lblReportSubTitle.Text = string.Format("No. : {0}",entity.TransactionNo);

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", entity.VisitID))[0];
            lblPatientInfo.Text = entityConsultVisit.PatientName;
            lblRegistrationInfo.Text = string.Format("{0} / {1}", entityConsultVisit.RegistrationNo, entityConsultVisit.MedicalNo);
            lblPhysicianName.Text = entityConsultVisit.ParamedicName;
            lblPatientLocation.Text = string.Format("{0} {1}", entityConsultVisit.ServiceUnitName, entityConsultVisit.BedCode);

            lblUserName.Text = entity.ChargesUserName;

            if (entity.LastUpdatedBy != 0 && entity.LastUpdatedBy != null)
            {
                lblLastUpdatedBy.Text = AppSession.UserLogin.UserFullName;
            }
            else
            {
                lblLastUpdatedBy.Text = AppSession.UserLogin.UserFullName;
            }
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.cfDateForSignInString);
        }

        protected void lblIsRFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean isRFlag = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag"));
            if (!isRFlag) e.Cancel = true;
        }
    }
}
