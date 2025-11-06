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
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BAmplopCoverHasilRSSM : BaseRpt
    {
        public BAmplopCoverHasilRSSM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientChargesHd entityHd = BusinessLayer.GetvPatientChargesHdList(param[0]).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entityHd.VisitID)).FirstOrDefault();

            lblMedicalNo.Text = entityHd.MedicalNo;
            lblPatientName.Text = string.Format("{0} / ({1} th.)", entityCV.PatientName , entityCV.AgeInYear);
            lblTransactionDate.Text = entityHd.TransactionDateInString;

            lblRegistrationNo.Text = entityCV.RegistrationNo;

            if (entityHd.TestOrderPhysician == null || entityHd.TestOrderPhysician == "")
            {
                lblParamedicName.Text = entityCV.ParamedicName;
            }
            else
            {
                lblParamedicName.Text = entityHd.TestOrderPhysician;
            }
        }
    }
}
