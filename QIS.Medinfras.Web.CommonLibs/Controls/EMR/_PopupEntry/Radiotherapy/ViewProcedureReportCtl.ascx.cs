using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewProcedureReportCtl : BaseViewPopupCtl
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _reportID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnPopupMRN.Value = paramInfo[3];
            hdnPopupMedicalNo.Value = paramInfo[4];
            hdnPopupPatientName.Value = paramInfo[5];
            hdnPopupParamedicID.Value = paramInfo[6];
            hdnTotalFraction.Value = paramInfo[7];

            txtTotalFraction.Text = paramInfo[7];

            SetControlProperties();

            string filterExpression = string.Format("BrachytherapyProcedureReportID = {0}", hdnID.Value);
            vBrachytherapyProcedureReport entity = BusinessLayer.GetvBrachytherapyProcedureReportList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;

            string filterExpression = string.Format("IsDeleted = 0", paramedicID);


            //if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            //{
            //    filterExpression = string.Format(
            //                                        "GCParamedicMasterType IN ('{0}')",
            //                                        Constant.ParamedicType.Physician, paramedicID);
            //}

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = paramedicID.ToString();

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                    Constant.StandardCode.SURGERY_TEAM_ROLE,
                    Constant.StandardCode.APPLICATOR_TYPE,
                    Constant.StandardCode.INTRAUTERINE_LENGTH,
                    Constant.StandardCode.INTRAUTERINE_CORNER,
                    Constant.StandardCode.CYLINDER,
                    Constant.StandardCode.HEMORRHAGE));

            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.APPLICATOR_TYPE).ToList();
            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.INTRAUTERINE_LENGTH).ToList();
            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.INTRAUTERINE_CORNER).ToList();
            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.CYLINDER).ToList();
            List<StandardCode> lstCode6 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.HEMORRHAGE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorType, lstCode2, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIntrauterineLength, lstCode3, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIntrauterineCorner, lstCode4, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCCylinder, lstCode5, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCHemorrhage1, lstCode6, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorReleaseHemorrhage, lstCode6, "StandardCodeName", "StandardCodeID");

            VisitInfoForRadiotherapyProgram oInfo = BusinessLayer.GetVisitInfoForRadiotherapyProgram(Convert.ToInt32(hdnPopupVisitID.Value)).FirstOrDefault();
            if (oInfo != null)
            {
                txtDiagnosisInfo.Text = oInfo.PatientDiagnosis.Replace("+", Environment.NewLine);
            }

            cboParamedicID.Enabled = false;
        }

        private void EntityToControl(vBrachytherapyProcedureReport entity)
        {
            _reportID = entity.BrachytherapyProcedureReportID.ToString();
            txtReportDate.Text = entity.ReportDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReportTime.Text = entity.ReportTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtFractionNo.Text = entity.FractionNo.ToString();
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;
            txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndTime.Text = entity.EndTime;
            txtDuration.Text = entity.Duration.ToString();
            if (!string.IsNullOrEmpty(entity.GCApplicatorType))
            {
                cboGCApplicatorType.Value = entity.GCApplicatorType;
            }
            if (!string.IsNullOrEmpty(entity.GCIntrauterineLength))
            {
                cboGCIntrauterineLength.Value = entity.GCIntrauterineLength;
            }
            if (!string.IsNullOrEmpty(entity.GCIntrauterineCorner))
            {
                cboGCIntrauterineCorner.Value = entity.GCIntrauterineCorner;
            }
            if (!string.IsNullOrEmpty(entity.GCCylinder))
            {
                cboGCCylinder.Value = entity.GCCylinder;
            }

            txtNeddleDepth.Text = entity.NeedleDepth.ToString();
            txtTotalNeddle.Text =  entity.TotalNeedle.ToString();
            chkIsNeedleLocation1.Checked = entity.IsNeedleLocation1;
            chkIsNeedleLocation2.Checked = entity.IsNeedleLocation2;
            chkIsNeedleLocation3.Checked = entity.IsNeedleLocation3;
            chkIsNeedleLocation4.Checked = entity.IsNeedleLocation4;
            chkIsNeedleLocation5.Checked = entity.IsNeedleLocation5;
            chkIsNeedleLocation6.Checked = entity.IsNeedleLocation6;
            chkIsNeedleLocation7.Checked = entity.IsNeedleLocation7;
            chkIsNeedleLocation8.Checked = entity.IsNeedleLocation8;
            chkIsNeedleLocation9.Checked = entity.IsNeedleLocation9;
            chkIsNeedleLocation10.Checked = entity.IsNeedleLocation10;
            chkIsNeedleLocation11.Checked = entity.IsNeedleLocation11;
            chkIsNeedleLocation12.Checked = entity.IsNeedleLocation12;

            txtTotalDosage.Text = entity.TotalDosage.ToString();
            txtBladderDosageLimitation.Text = entity.BladderDosageLimitation.ToString();
            txtRectumDosageLimitation.Text = entity.RectumDosageLimitation.ToString();
            txtSigmoidDosageLimitation.Text = entity.SigmoidDosageLimitation.ToString();
            txtBowelDosageLimitation.Text = entity.BowelDosageLimitation.ToString();

            txtProcedureRemarks.Text = entity.ProcedureRemarks;

            chkIsHasProcedureComplication.Checked = entity.IsHasProcedureComplication;
            chkIsHasProcedureHemorrhage.Checked = entity.IsHasProcedureHemorrhage;
            if (entity.IsHasProcedureHemorrhage)
            {
                cboGCHemorrhage1.Value = entity.GCProcedureHemorrhage;
            }
            txtProcedurePainIndex.Text = entity.ProcedurePainIndex.ToString();
            txtAnesthesiaComplicationRemarks.Text = entity.AnesthesiaComplicationRemarks;
            txtProcedureComplicationRemarks.Text = entity.ProcedureComplicationRemarks;

            chkIsHasApplicatorReleaseComplication.Checked = entity.IsHasApplicatorReleaseComplication;
            chkIsHasApplicatorReleaseHemorrhage.Enabled = entity.IsHasApplicatorReleaseComplication;
            chkIsHasApplicatorReleaseHemorrhage.Checked = entity.IsHasApplicatorReleaseHemorrhage;
            if (entity.IsHasApplicatorReleaseHemorrhage)
            {
                cboGCApplicatorReleaseHemorrhage.Value = entity.GCApplicatorReleaseHemorrhage;
            }
            txtPainIndex.Text = entity.PainIndex.ToString();
            txtApplicatorReleaseComplicationRemarks.Text = entity.ApplicatorReleaseComplicationRemarks;

            BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
        }

        private void ControlToEntity(BrachytherapyProcedureReport entityHd)
        {
            entityHd.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entityHd.MRN = Convert.ToInt32(hdnPopupMRN.Value);
            entityHd.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.ReportDate = Helper.GetDatePickerValue(txtReportDate.Text);
            entityHd.ReportTime = txtReportTime.Text;
            entityHd.FractionNo = Convert.ToInt16(txtFractionNo.Text);
            entityHd.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entityHd.StartTime = txtStartTime.Text;
            entityHd.EndDate = Helper.GetDatePickerValue(txtEndDate.Text);
            entityHd.EndTime = txtEndTime.Text;
            entityHd.Duration = Convert.ToDecimal(txtDuration.Text);
            entityHd.GCApplicatorType = cboGCApplicatorType.Value.ToString();
            entityHd.GCIntrauterineLength = cboGCIntrauterineLength.Value.ToString();
            entityHd.GCIntrauterineCorner = cboGCIntrauterineCorner.Value.ToString();
            entityHd.GCCylinder = cboGCCylinder.Value.ToString();
            entityHd.NeedleDepth = Convert.ToInt32(txtNeddleDepth.Text);
            entityHd.TotalNeedle = Convert.ToInt32(txtTotalNeddle.Text);
            entityHd.IsNeedleLocation1 = chkIsNeedleLocation1.Checked;
            entityHd.IsNeedleLocation2 = chkIsNeedleLocation2.Checked;
            entityHd.IsNeedleLocation3 = chkIsNeedleLocation3.Checked;
            entityHd.IsNeedleLocation4 = chkIsNeedleLocation4.Checked;
            entityHd.IsNeedleLocation5 = chkIsNeedleLocation5.Checked;
            entityHd.IsNeedleLocation6 = chkIsNeedleLocation6.Checked;
            entityHd.IsNeedleLocation7 = chkIsNeedleLocation7.Checked;
            entityHd.IsNeedleLocation8 = chkIsNeedleLocation8.Checked;
            entityHd.IsNeedleLocation9 = chkIsNeedleLocation9.Checked;
            entityHd.IsNeedleLocation10 = chkIsNeedleLocation10.Checked;
            entityHd.IsNeedleLocation11 = chkIsNeedleLocation11.Checked;
            entityHd.IsNeedleLocation12 = chkIsNeedleLocation12.Checked;
            entityHd.TotalDosage = Convert.ToDecimal(txtTotalDosage.Text);
            if (!string.IsNullOrEmpty(txtBladderDosageLimitation.Text) && txtBladderDosageLimitation.Text != "0")
            {
                entityHd.BladderDosageLimitation = Convert.ToDecimal(txtBladderDosageLimitation.Text); 
            }
            if (!string.IsNullOrEmpty(txtRectumDosageLimitation.Text) && txtRectumDosageLimitation.Text != "0")
            {
                entityHd.RectumDosageLimitation = Convert.ToDecimal(txtRectumDosageLimitation.Text); 
            }
            if (!string.IsNullOrEmpty(txtSigmoidDosageLimitation.Text) && txtSigmoidDosageLimitation.Text != "0")
            {
                entityHd.SigmoidDosageLimitation = Convert.ToDecimal(txtSigmoidDosageLimitation.Text); 
            }
            if (!string.IsNullOrEmpty(txtBowelDosageLimitation.Text) && txtBowelDosageLimitation.Text != "0")
            {
                entityHd.BowelDosageLimitation = Convert.ToDecimal(txtBowelDosageLimitation.Text);
            }
            entityHd.ProcedureRemarks = txtProcedureRemarks.Text;
            entityHd.IsHasProcedureComplication = chkIsHasProcedureComplication.Checked;
            if (chkIsHasProcedureComplication.Checked)
            {
                entityHd.IsHasProcedureHemorrhage = chkIsHasProcedureHemorrhage.Checked;
                if (cboGCHemorrhage1.Value != null)
                {
                    entityHd.GCProcedureHemorrhage = cboGCHemorrhage1.Value.ToString(); 
                }
                entityHd.ProcedurePainIndex = Convert.ToInt16(txtProcedurePainIndex.Text);
                entityHd.AnesthesiaComplicationRemarks = txtAnesthesiaComplicationRemarks.Text;
                entityHd.ProcedureComplicationRemarks = txtProcedureRemarks.Text;
            }
            entityHd.IsHasApplicatorReleaseComplication = chkIsHasApplicatorReleaseComplication.Checked;
            if (chkIsHasApplicatorReleaseComplication.Checked)
            {
                entityHd.IsHasApplicatorReleaseHemorrhage = chkIsHasApplicatorReleaseHemorrhage.Checked;
                if (cboGCApplicatorReleaseHemorrhage.Value != null)
                {
                    entityHd.GCApplicatorReleaseHemorrhage = cboGCApplicatorReleaseHemorrhage.Value.ToString(); 
                }
                entityHd.PainIndex = Convert.ToInt16(txtPainIndex.Text);
                entityHd.ApplicatorReleaseComplicationRemarks = txtApplicatorReleaseComplicationRemarks.Text;
            }
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Paramedic Team
        private void BindGridViewParamedicTeam(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _reportID != "0")
            {
                hdnID.Value = _reportID;
            }

            List<vBrachytherapyProcedureTeam> lstEntity = new List<vBrachytherapyProcedureTeam>();
            if (hdnID.Value != "0")
            {
                string filterExpression = string.Format("BrachytherapyProcedureReportID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, _reportID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvBrachytherapyProcedureTeamRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvBrachytherapyProcedureTeamList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);

            }

            grdParamedicTeamView.DataSource = lstEntity;
            grdParamedicTeamView.DataBind();
        }
        protected void cbpParamedicTeamView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewParamedicTeam(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewParamedicTeam(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion
    }
}