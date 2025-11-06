using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class NotaReturResepA6Rpt : BaseA6Rpt
    {
        public NotaReturResepA6Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHdList(param[0])[0];
            lblReportTitle.Text = string.Format("RETUR RESEP");
            lblsubTitle.Text = string.Format("{0}", entity.cfTransactionNoDisplay);

            vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", entity.VisitID))[0];
            lblPatientInfo.Text = entityConsultVisit.PatientName;
            lblRegistrationInfo.Text = string.Format("{0} / {1}", entityConsultVisit.RegistrationNo, entityConsultVisit.MedicalNo);
            lblPhysicianName.Text = entityConsultVisit.ParamedicName;
            lblPatientLocation.Text = string.Format("{0} {1}", entityConsultVisit.ServiceUnitName, entityConsultVisit.BedCode);

            lblUserName.Text = entity.ChargesUserName;
            
        }

        protected void lblIsRFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean isRFlag = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag"));
            if (!isRFlag) e.Cancel = true;
        }
    }
}
