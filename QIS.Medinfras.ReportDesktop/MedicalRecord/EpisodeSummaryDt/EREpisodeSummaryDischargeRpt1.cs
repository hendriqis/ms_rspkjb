using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Collections.Generic;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EREpisodeSummaryDischargeRpt1 : DevExpress.XtraReports.UI.XtraReport
    {
        public EREpisodeSummaryDischargeRpt1()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            List<vConsultVisit5> lstEntity = BusinessLayer.GetvConsultVisit5List(String.Format("VisitID = {0} AND IsMainVisit = 1", VisitID));
            this.DataSource = lstEntity;


            if (lstEntity[0].GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_WARD || lstEntity[0].GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER || lstEntity[0].GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
            {
                lblRefferal.Visible = lblReferralColon.Visible = lblReferralInfo.Visible = true;
                lblReferralReason.Visible = lblReferralReasonColon.Visible = lblReferralReasonInfo.Visible = true;

                if (lstEntity[0].GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_WARD )
                {
                    lblRefferal.Text = "Reffered Physician";
                    lblReferralReason.Visible = lblReferralReasonColon.Visible = lblReferralReasonInfo.Visible = false;
                }
            }
            else
            {
                lblRefferal.Visible = lblReferralColon.Visible = lblReferralInfo.Visible = false;
                lblReferralReason.Visible = lblReferralReasonColon.Visible = lblReferralReasonInfo.Visible = false;
            }

            if (lstEntity[0].DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                lblSignDate.Text = string.Format("{0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            }
            else
            {
                lblSignDate.Text = lstEntity[0].cfDischargeDateInString2;
            }
            lblSignParamedicName.Text = lstEntity[0].PhysicianName;
        }
    }
}
