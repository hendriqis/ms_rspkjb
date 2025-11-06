using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TemporaryClaim : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            switch (hdnRequestID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Finance.BPJS_TEMPORARY_CLAIM_INPATIENT;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Finance.BPJS_TEMPORARY_CLAIM_OUTPATIENT;
                default: return Constant.MenuCode.Finance.BPJS_TEMPORARY_CLAIM_INPATIENT;
            }
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
            {
                return GetLabel(menu.MenuCaption);
            }
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnRequestID.Value = Page.Request.QueryString["id"];
            }
            else
            {
                hdnRequestID.Value = "ALL";
            }

            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            txtSearchRegistrationDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtSearchRegistrationDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstOrderStatus = new List<Variable>();
            lstOrderStatus.Add(new Variable { Code = "0", Value = "Semua" });
            lstOrderStatus.Add(new Variable { Code = "1", Value = "Ada Order Coding" });
            lstOrderStatus.Add(new Variable { Code = "2", Value = "Tidak Ada Order Coding" });
            Methods.SetComboBoxField<Variable>(cboOrderStatus, lstOrderStatus, "Value", "Code");
            cboOrderStatus.Value = "0";

            List<Variable> lstCustomerType = new List<Variable>();
            lstCustomerType.Add(new Variable { Code = "0", Value = "Semua" });
            lstCustomerType.Add(new Variable { Code = "1", Value = "BPJS" });
            lstCustomerType.Add(new Variable { Code = "2", Value = "Non BPJS" });
            Methods.SetComboBoxField<Variable>(cboCustomerType, lstCustomerType, "Value", "Code");
            cboCustomerType.Value = "1";

            BindGridView(1, true, ref PageCount);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            string url = "";
            if (type == "upload")
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnID.Value))[0];
                SetSessionRegisteredPatient(entity);
                url = string.Format("~/Program/BPJS/TemporaryClaim/UploadDocument/BPJSDocument.aspx?id={0}|TC|", hdnID.Value);
                
                retval = url;
            }
            return true;
        }

        private void SetSessionRegisteredPatient(vConsultVisit entity)
        {
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.DateOfBirth = entity.DateOfBirth;
            AppSession.RegisteredPatient = pt;
        }

        #region Header Klaim
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationDate BETWEEN '{0}' AND '{1}'",
                                                        Helper.GetDatePickerValue(txtSearchRegistrationDateFrom.Text),
                                                        Helper.GetDatePickerValue(txtSearchRegistrationDateTo.Text));

            if (hdnRequestID.Value == Constant.Facility.INPATIENT)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            }
            else
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("DepartmentID != '{0}'", Constant.Facility.INPATIENT);
            }

            if (cboCustomerType.Value.ToString() == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCCustomerType = '{0}'", Constant.CustomerType.BPJS);
            }
            else if (cboCustomerType.Value.ToString() == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCCustomerType != '{0}'", Constant.CustomerType.BPJS);
            }

            if (cboOrderStatus.Value.ToString() == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += "OrderCodingBy IS NOT NULL";
            }
            else if (cboOrderStatus.Value.ToString() == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += "OrderCodingBy IS NULL";
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitCasemixRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vConsultVisitCasemix> lstEntity = BusinessLayer.GetvConsultVisitCasemixList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "RegistrationID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewTemp_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail Klaim
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnID.Value);

            if (cboCustomerType.Value.ToString() == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCCustomerType = '{0}'", Constant.CustomerType.BPJS);
            }
            else if (cboCustomerType.Value.ToString() == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCCustomerType != '{0}'", Constant.CustomerType.BPJS);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationClaimHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vRegistrationClaimHistory> lstEntity = BusinessLayer.GetvRegistrationClaimHistoryList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "RegistrationID DESC, SortID DESC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vRegistrationClaimHistory entity = e.Row.DataItem as vRegistrationClaimHistory;

                HtmlGenericControl lblINACBGMaster = (HtmlGenericControl)e.Row.FindControl("lblINACBGMaster");
                HtmlInputText txtGrouperCodeClaim = (HtmlInputText)e.Row.FindControl("txtGrouperCodeClaim");
                HtmlInputText txtGrouperTypeClaim = (HtmlInputText)e.Row.FindControl("txtGrouperTypeClaim");
                HtmlInputText txtRealCostAmount = (HtmlInputText)e.Row.FindControl("txtRealCostAmount");
                HtmlInputText txtCoverageAmount = (HtmlInputText)e.Row.FindControl("txtCoverageAmount");
                HtmlInputText txtOccupiedAmount = (HtmlInputText)e.Row.FindControl("txtOccupiedAmount");
                HtmlInputText txtPatientAmount = (HtmlInputText)e.Row.FindControl("txtPatientAmount");
                HtmlInputText txtDifferenceAmount = (HtmlInputText)e.Row.FindControl("txtDifferenceAmount");
                HtmlInputButton btnSave = e.Row.FindControl("btnSave") as HtmlInputButton;

                if (entity.HistoryID == 0)
                {
                    lblINACBGMaster.Attributes.Remove("style");
                    txtGrouperCodeClaim.Attributes.Add("readonly", "readonly");
                    txtGrouperTypeClaim.Attributes.Add("readonly", "readonly");
                    txtRealCostAmount.Attributes.Add("readonly", "readonly");
                    txtCoverageAmount.Attributes.Remove("readonly");
                    txtOccupiedAmount.Attributes.Remove("readonly");
                    txtPatientAmount.Attributes.Remove("readonly");
                    txtDifferenceAmount.Attributes.Add("readonly", "readonly");
                    btnSave.Style.Remove("display");
                }
                else
                {
                    lblINACBGMaster.Attributes.Add("style", "display:none");
                    txtGrouperCodeClaim.Attributes.Add("readonly", "readonly");
                    txtGrouperTypeClaim.Attributes.Add("readonly", "readonly");
                    txtRealCostAmount.Attributes.Add("readonly", "readonly");
                    txtCoverageAmount.Attributes.Add("readonly", "readonly");
                    txtOccupiedAmount.Attributes.Add("readonly", "readonly");
                    txtPatientAmount.Attributes.Add("readonly", "readonly");
                    txtDifferenceAmount.Attributes.Add("readonly", "readonly");
                    btnSave.Style.Add("display", "none");
                }
            }
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "save")
                {
                    if (OnSaveClaim(param, ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else // refresh
                {
                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveClaim(string[] param, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao regBPJSDao = new RegistrationBPJSDao(ctx);

            try
            {
                RegistrationBPJS regBPJS = regBPJSDao.Get(Convert.ToInt32(hdnID.Value));
                regBPJS.CodingBy = AppSession.UserLogin.UserID;
                regBPJS.CodingDate = DateTime.Now;
                regBPJS.GrouperCodeClaim = param[2];
                regBPJS.GrouperTypeClaim = param[3];
                regBPJS.RealCostAmount = Convert.ToDecimal(param[4]);
                regBPJS.INAHakPasien = regBPJS.CoverageAmount = regBPJS.GrouperAmountClaim = Convert.ToDecimal(param[5]);
                regBPJS.INADitempati = regBPJS.OccupiedAmount = Convert.ToDecimal(param[6]);
                regBPJS.PatientAmount = Convert.ToDecimal(param[7]);
                regBPJS.DifferenceAmount = Convert.ToDecimal(param[8]);

                regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                regBPJSDao.Update(regBPJS);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}