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
    public partial class MRReviewOfSystemNewOutRSRA : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReviewOfSystemNewOutRSRA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0 ORDER BY ID DESC", VisitID, MedicalResumeID);
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();
            vReviewOfSystemHd entityHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).FirstOrDefault();
            vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression).FirstOrDefault();

            if (entityMR.ObjectiveResumeText != null && entityMR.ObjectiveResumeText != "")
            {
                xrLabel2.Text = string.Format("Catatan Tambahan Pemeriksaan Fisik :");
                lblRemarks.Text = string.Format("{0}", entityMR.ObjectiveResumeText);
            }
            else
            {
                xrLabel2.Visible = false;
                lblRemarks.Visible = false;
            }

            if (filterExpression != null)
            {
                lblRemarks.Text = string.Format("{0}", entityMR.ObjectiveResumeText);
                List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression);
                List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
                var lst = (from p in lstHd
                           select new
                           {
                               ObservationDateInString = p.ObservationDateInString,
                               ObservationTime = p.ObservationTime,
                               ParamedicName = p.ParamedicName,
                               ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                           }).ToList();

                if (entityHd != null)
                {
                    if (lstDt.Count > 0)
                    {
                        if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                        {
                            if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                            {
                                lblReviewOfSystem.Text = string.Format("{0} / {1} ({2})", entityCV.cfDischargeDate, entityCV.DischargeTime, entityHd.ParamedicName);
                            }
                            else
                            {
                                lblReviewOfSystem.Text = "";
                            }
                        }
                        else
                        {
                            if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                            {
                                lblReviewOfSystem.Text = string.Format("{0} / {1} ({2})", entityCV.cfPhysicianDischargedDateOrderInString, entityCV.PhysicianDischargeOrderTime, entityHd.ParamedicName);
                            }
                            else
                            {
                                lblReviewOfSystem.Text = string.Format("{0} / {1} ({2})", entityCV.cfDischargeDate, entityCV.DischargeTime, entityHd.ParamedicName);
                            }
                        }
                    }
                    else
                    {
                        xrLabel6.Visible = false;
                        lblReviewOfSystem.Visible = false;
                    }
                }
                else
                {
                    xrLabel6.Visible = false;
                    lblReviewOfSystem.Visible = false;
                }
                this.DataSource = lst;
                DetailReport.DataMember = "ReviewOfSystemDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                xrLabel5.Visible = false;
                xrLabel6.Visible = false;
                lblReviewOfSystem.Visible = false;
                lblRemarks.Visible = false;
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
