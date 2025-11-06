using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPasianMeninggalDetail : BaseCustomDailyLandscapeRpt
    {
        public LPasianMeninggalDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[1].Split(';');
            List<vConsultVisitDeath> lstConsultVisitDeathBefore48 = BusinessLayer.GetvConsultVisitDeathList(string.Format("DischargeDate BETWEEN '{0}' AND '{1}' AND GCDischargeCondition = '{2}'", temp[0], temp[1], Constant.PatientOutcome.DEAD_BEFORE_48));
            List<vConsultVisitDeath> lstConsultVisitDeathAfter48 = BusinessLayer.GetvConsultVisitDeathList(string.Format("DischargeDate BETWEEN '{0}' AND '{1}' AND GCDischargeCondition = '{2}'", temp[0], temp[1], Constant.PatientOutcome.DEAD_AFTER_48));
            List<vConsultVisitDeath> lstConsultVisitDeathAll = BusinessLayer.GetvConsultVisitDeathList(string.Format("DischargeDate BETWEEN '{0}' AND '{1}'", temp[0], temp[1]));
            List<vConsultVisit> lstConsulVisit = BusinessLayer.GetvConsultVisitList(string.Format("DischargeDate BETWEEN '{0}' AND '{1}' AND IsMainVisit = 1 AND GCDischargeCondition NOT IN ('{2}','{3}')", temp[0], temp[1], Constant.PatientOutcome.DEAD_BEFORE_48, Constant.PatientOutcome.DEAD_AFTER_48));


            int DeadBefore48 = lstConsultVisitDeathBefore48.Count;
            int DeadAfter48 = lstConsultVisitDeathAfter48.Count;
            int PatientOutDead = lstConsultVisitDeathAll.Count;
            int PatientOut = lstConsulVisit.Count;

            if (DeadAfter48 == 0)
            {
                lblNDR.Text = string.Format("0 %");
            }
            else
            {
                decimal NDR = ((Decimal)DeadAfter48 / (Decimal)PatientOut) * 1000;
                lblNDR.Text = string.Format("{0} %", NDR.ToString("N2"));
            }

            if (PatientOut == 0)
            {
                lblGDR.Text = string.Format("0 %");
            }
            else
            {
                decimal GDR = ((Decimal)PatientOutDead / (Decimal)PatientOut) * 100;
                lblGDR.Text = string.Format("{0} %", GDR.ToString("N2"));
            }

            lblPatientOut.Text = string.Format("{0}", PatientOut);
            lbl48jam.Text = string.Format("{0}", DeadBefore48);
            base.InitializeReport(param);
        }
  
    }
}
