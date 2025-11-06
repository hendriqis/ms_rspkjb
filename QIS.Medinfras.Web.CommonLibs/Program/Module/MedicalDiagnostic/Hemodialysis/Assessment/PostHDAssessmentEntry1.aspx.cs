using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using System.IO;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PostHDAssessmentEntry1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int gridVitalSignPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalDiagnostic.MD035124;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Page.Request.QueryString["id"] != null)
            {
                hdnID.Value = string.IsNullOrEmpty(Page.Request.QueryString["id"]) ? "0" : Page.Request.QueryString["id"];
            }

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnEndTime1.Value = txtEndTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
            hdnEndTime2.Value = txtEndTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);

            hdnIsChanged.Value = "0";
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Empty;

            vPreHDAssessment entity = BusinessLayer.GetvPreHDAssessmentList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, hdnID.Value)).FirstOrDefault();

            if (entity != null)
            {
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "0";
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            filterExpression = string.Empty;
            if (paramedicID != 0)
                filterExpression = string.Format("ParamedicID = {0}", paramedicID);
            else
                filterExpression = string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            bool isEnabled = true;
            if (entity != null)
            {
                isEnabled = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
            }

            Helper.SetControlEntrySetting(txtPostHDSymptoms, new ControlEntrySetting(isEnabled, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(txtStartTime1, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(txtStartTime2, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(txtEndDate, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(txtEndTime1, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(txtEndTime2, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, false, true), "mpEntry");
            Helper.SetControlEntrySetting(txtFinalUFG, new ControlEntrySetting(true, true, true), "mpEntry");

            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                {
                    if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                        cboParamedicID.Value = Convert.ToInt32(hdnAssessmentParamedicID.Value);
                    else
                        cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }

            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
        }

        private void EntityToControl(vPreHDAssessment entity)
        {
            hdnAssessmentParamedicID.Value = entity.PostHDParamedicID.ToString();

            if (entity.ID == 0)
            {
                hdnID.Value = "";
            }
            else
            {
                hdnID.Value = entity.ID.ToString();
            }

            if (!string.IsNullOrEmpty(entity.StartTime))
            {
                txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStartTime1.Text = entity.StartTime.Substring(0, 2);
                txtStartTime2.Text = entity.StartTime.Substring(3, 2);
            }

            if (!string.IsNullOrEmpty(entity.EndTime))
            {
                txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEndTime1.Text = entity.EndTime.Substring(0, 2);
                txtEndTime2.Text = entity.EndTime.Substring(3, 2); 
            }

            #region Post-HD Information
            txtFinalUFG.Text = entity.FinalUFG.ToString();
            txtTotalOutput.Text = entity.TotalOutput.ToString();
            txtPrimingBalance.Text = entity.PrimingBalance.ToString();
            txtWashOut.Text = entity.WashOut.ToString();
            txtTotalIntake.Text = entity.TotalIntake.ToString();
            txtPostHDSymptoms.Text = entity.PostHDAnamnese;
            cboParamedicID.Value = entity.PostHDParamedicID.ToString();
            #endregion

            #region Intake-Output Balance
            string filterExpression = string.Format("VisitID = {0} AND PreHDAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);
            List<vHSUFluidBalance> lstEntity = BusinessLayer.GetvHSUFluidBalanceList(filterExpression);
            decimal totalIntake1 = 0;
            decimal totalOutput1 = 0;

            foreach (vHSUFluidBalance item in lstEntity)
            {
                if (item.GCFluidGroup == Constant.FluidBalanceGroup.Intake)
                {
                    totalIntake1 += item.FluidAmount;
                }
                else
                {
                    totalOutput1 += item.FluidAmount;
                }
            }

            if (entity.TotalIntake == 0)
            {
                txtTotalIntake.Text = totalIntake1.ToString();
            }
            else
            if (entity.TotalOutput == 0)
            {
                txtTotalOutput.Text = totalOutput1.ToString(); 
            }

            decimal totalCalculatedOutput = entity.FinalUFG + Convert.ToDecimal(txtTotalOutput.Text);
            decimal totalCalculatedIntake = entity.PrimingBalance + entity.WashOut + Convert.ToDecimal(txtTotalIntake.Text);
            txtCalculatedTotalOutput.Text = totalCalculatedOutput.ToString();
            txtCalculatedTotalIntake.Text = totalCalculatedIntake.ToString();
            txtTotalUF.Text = (totalCalculatedOutput - totalCalculatedIntake).ToString();
            #endregion

            hdnIsChanged.Value = "0";
        }
        protected override bool OnCustomButtonClick(string type, ref string messages)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PreHDAssessmentDao assesmentDao = new PreHDAssessmentDao(ctx);
                bool isAllowSave = true;
                try
                {
                    if (isAllowSave)
                    {
                        if (IsValidated(ref messages))
                        {
                            PreHDAssessment obj = BusinessLayer.GetPreHDAssessmentList(string.Format("VisitID = {0} AND ID = '{1}'", AppSession.RegisteredPatient.VisitID, hdnID.Value), ctx).FirstOrDefault();
                            bool isNewRecord = false;
                            if (obj == null)
                            {
                                isNewRecord = true;
                                obj = new PreHDAssessment();
                            }
                            ControlToEntity(obj);

                            if (isNewRecord)
                            {
                                obj.CreatedBy = AppSession.UserLogin.UserID;
                                hdnID.Value = assesmentDao.InsertReturnPrimaryKeyID(obj).ToString();
                            }
                            else
                            {
                                obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                assesmentDao.Update(obj);
                            }

                            ctx.CommitTransaction();

                            hdnIsSaved.Value = "1";
                            hdnIsChanged.Value = "0";

                            messages = hdnID.Value;

                        }
                        else
                        {
                            hdnIsChanged.Value = "0";

                            ctx.RollBackTransaction();
                            result = false;
                        }
                    }
                    else
                    {
                        messages = "Maaf, Perubahan Pengkajian Pasien hanya bisa dilakukan oleh Perawat yang melakukan pengkajian pertama kali";
                        hdnIsChanged.Value = "0";

                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    messages = ex.Message;
                    hdnIsSaved.Value = "0";
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        private bool IsValidated(ref string messages)
        {
            DateTime startDate;
            DateTime endDate;
            string format = Constant.FormatString.DATE_TIME_FORMAT_4;
            try
            {                
                startDate = DateTime.ParseExact(string.Format("{0} {1}:{2}",txtStartDate.Text,txtStartTime1.Text,txtStartTime2.Text), format, CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(string.Format("{0} {1}:{2}", txtEndDate.Text, txtEndTime1.Text, txtEndTime2.Text), format, CultureInfo.InvariantCulture);

                if (DateTime.Compare(startDate, DateTime.Now) > 0)
                {
                    messages = "Tanggal mulai harus lebih kecil dari atau sama dengan tanggal sekarang";
                    return false;
                }
                else if (DateTime.Compare(endDate, DateTime.Now) > 0)
                {
                    messages = "Tanggal selesai harus lebih kecil dari atau sama dengan tanggal sekarang";
                    return false;
                }
                if (DateTime.Compare(endDate, startDate) < 0)
                {
                    messages = "Tanggal selesai harus lebih besar dari tanggal mulai";
                    return false;
                }

                if (string.IsNullOrEmpty(txtFinalUFG.Text) || !Methods.IsNumeric(txtFinalUFG.Text))
                {
                    messages = "Nilai UFG Akhir HD harus diisi atau sama dengan 0";
                    return false;
                }

                if (string.IsNullOrEmpty(txtPrimingBalance.Text) || !Methods.IsNumeric(txtPrimingBalance.Text))
                {
                    messages = "Nilai Sisa Priming harus diisi atau sama dengan 0";
                    return false;
                }

                if (string.IsNullOrEmpty(txtWashOut.Text) || !Methods.IsNumeric(txtWashOut.Text))
                {
                    messages = "Nilai Wash Out harus diisi atau sama dengan 0";
                    return false;
                }

                return true;
            }
            catch (FormatException)
            {
                messages = "Format tanggal tidak sesuai dengan (dd-MM-yyyy HH:mm)";
                return false;
            }
        }

        private void ControlToEntity(PreHDAssessment obj)
        {
            obj.VisitID = AppSession.RegisteredPatient.VisitID;
            obj.StartDate = Helper.GetDatePickerValue(txtStartDate);
            obj.StartTime = string.Format("{0}:{1}", txtStartTime1.Text, txtStartTime2.Text);
            obj.EndDate = Helper.GetDatePickerValue(txtEndDate);
            obj.EndTime = string.Format("{0}:{1}", txtEndTime1.Text, txtEndTime2.Text);
            obj.PostHDParamedicID = Convert.ToInt32(cboParamedicID.Value);
            obj.FinalUFG = Convert.ToDecimal(txtFinalUFG.Text);
            obj.TotalOutput = Convert.ToDecimal(txtTotalOutput.Text);
            obj.PrimingBalance = Convert.ToDecimal(txtPrimingBalance.Text);
            obj.WashOut = Convert.ToDecimal(txtWashOut.Text);
            obj.TotalIntake = Convert.ToDecimal(txtTotalIntake.Text);
            //obj.TotalUF = Convert.ToDecimal(txtTotalUF.Text);
            decimal totalCalculatedOutput = obj.FinalUFG + obj.TotalOutput;
            decimal totalCalculatedIntake = obj.PrimingBalance + obj.WashOut + obj.TotalIntake;
            decimal total = (totalCalculatedOutput - totalCalculatedIntake);

            obj.TotalUF = Convert.ToDecimal(total);
            obj.PostHDAnamnese = txtPostHDSymptoms.Text;
        }

        protected void cbpCalculateBalanceSummary_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string value = "0;0;0;0";
            string[] paramInfo = e.Parameter.Split('|');
            try
            {
                string filterExpression = string.Format("VisitID = {0} AND PreHDAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

                List<vHSUFluidBalance> lstEntity = BusinessLayer.GetvHSUFluidBalanceList(filterExpression);
                decimal totalIntake1 = 0;
                decimal totalOutput1 = 0;

                foreach (vHSUFluidBalance item in lstEntity)
                {
                    if (item.GCFluidGroup == Constant.FluidBalanceGroup.Intake)
                    {
                        totalIntake1 += item.FluidAmount;
                    }
                    else
                    {
                        totalOutput1 += item.FluidAmount;
                    }
                }
                value = string.Format("{0};{1}", totalIntake1, totalOutput1);
                result += string.Format("success|{0}", value);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            filterExpression += string.Format("VisitID = {0} AND PreHDAssessmentID = {1} AND IsPostAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND PreHDAssessmentID = {1} AND IsPostAssessment = 1 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, hdnID.Value));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}