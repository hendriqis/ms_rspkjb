using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRVitalSignNewMROutRSRAC : DevExpress.XtraReports.UI.XtraReport
    {
        public MRVitalSignNewMROutRSRAC()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0 ORDER BY ID DESC", VisitID, MedicalResumeID);
            vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
            vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(string.Format(filterExpression)).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();

            if (entityHd != null)
            {
                List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                var lst = (from p in lstHd
                           select new
                           {
                               ObservationDateInString = p.ObservationDateInString,
                               ObservationTime = p.cfObservationTimeRM,
                               ParamedicName = p.cfParamedicNameRM,
                               Remarks = p.Remarks,
                               VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                           }).ToList();


                if (entityVisit.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    if (entityVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                    {
                        lblVitalSignOut.Text = string.Format("{0} / {1}", entityVisit.cfDischargeDate, entityVisit.DischargeTime);
                    }
                    else
                    {
                        lblVitalSignOut.Text = "";
                    }
                }
                else
                {
                    if (entityVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    {
                        lblVitalSignOut.Text = string.Format("{0} / {1} ({2})", entityVisit.cfPhysicianDischargedDateOrderInString, entityVisit.PhysicianDischargeOrderTime, entityHd.ParamedicName);
                    }
                    else
                    {
                        lblVitalSignOut.Text = string.Format("{0} / {1}", entityVisit.cfDischargeDate, entityVisit.DischargeTime);
                    }
                }

                this.DataSource = lst;
                DetailReport.DataMember = "VitalSignDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                xrLabel6.Visible = false;
                xrLabel7.Visible = false;
                lblVitalSignOut.Visible = false;
                xrLabel9.Visible = false;
            }
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (DetailReport.RowCount == 0)
            {
                Detail.Visible = false;
            }
        }

    }
}