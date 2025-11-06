using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARFinalClaimv2Approval : BasePageTrx
    {
        private string oGCClaimFinalStatus = Constant.FinalStatus.OPEN;
        private string oDepartmentID = "%";
        private string oIsExclusion = "0";
        private string oDateType = "0";
        private string oPaymentDate = "";
        private string oGrouperAmountFinalFilter = "";
        private string oGCFeedbackStatus = "%%";
        private string oIsExclusionFeedback = "0";
        private string oDownloadType = "1";
        private string oBusinessPartnerID = "0";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_AR_FINAL_CLAIM_V2_APPROVAL;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            hdnIsUsedClaimFinal.Value = AppSession.IsUsedClaimFinal ? "1" : "0";
            hdnIsFinalisasiKlaimAfterARInvoice.Value = AppSession.IsClaimFinalAfterARInvoice ? "1" : "0";

            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_AR_FINAL_CLAIM_BPJS).ParameterValue;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowApprove.Value = CRUDMode.Contains("A") ? "1" : "0";

            if (hdnIsAllowApprove.Value == "1")
            {
                btnApproved.Attributes.Remove("style");
            }
            else
            {
                btnApproved.Attributes.Add("style", "display:none");
            }

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstGrouperAmountFinal = new List<Variable>();
            lstGrouperAmountFinal.Add(new Variable { Code = "0", Value = "" });
            lstGrouperAmountFinal.Add(new Variable { Code = "1", Value = "Nilai Nol (0)" });
            lstGrouperAmountFinal.Add(new Variable { Code = "2", Value = "Ada Nilai" });
            Methods.SetComboBoxField<Variable>(cboGrouperAmountFinalFilter, lstGrouperAmountFinal, "Value", "Code");
            cboGrouperAmountFinalFilter.Value = "1";

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.FINAL_CLAIM_FEEDBACK_STATUS));
            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFeedbackStatusFilter, lstSC, "StandardCodeName", "StandardCodeID");
            cboFeedbackStatusFilter.Value = Constant.FinalClaimFeedbackStatus.MENUNGGU_UMPAN_BALIK;

            List<Variable> lstDownloadType = new List<Variable>();
            lstDownloadType.Add(new Variable { Code = "1", Value = "Download Template" });
            lstDownloadType.Add(new Variable { Code = "2", Value = "Download Keseluruhan" });
            lstDownloadType.Add(new Variable { Code = "3", Value = "Download Approved" });
            lstDownloadType.Add(new Variable { Code = "4", Value = "Download Sesuai Dengan Filter Pada Layar" });
            Methods.SetComboBoxField<Variable>(cboDownloadType, lstDownloadType, "Value", "Code");
            cboDownloadType.Value = "1";

            List<Variable> lstDateFilter = new List<Variable>();
            lstDateFilter.Add(new Variable { Code = "0", Value = "Tanggal Pengakuan Piutang" });
            lstDateFilter.Add(new Variable { Code = "1", Value = "Tanggal Pembuatan Piutang" });
            Methods.SetComboBoxField<Variable>(cboDateType, lstDateFilter, "Value", "Code");
            cboDateType.Value = "0";

            List<BusinessPartners> lstBP = BusinessLayer.GetBusinessPartnersList(string.Format("IsDeleted = 0 AND IsActive = 1 AND BusinessPartnerID IN (SELECT c.BusinessPartnerID FROM Customer c WHERE c.GCCustomerType = '{0}')", Constant.CustomerType.BPJS));
            lstBP.Insert(0, new BusinessPartners { BusinessPartnerID = 0, BusinessPartnerCode = "", BusinessPartnerName = "" });
            Methods.SetComboBoxField<BusinessPartners>(cboBusinessPartner, lstBP, "cfBusinessPartnerNameCode", "BusinessPartnerID");
            cboBusinessPartner.Value = "0";

            BindGridView();
        }

        protected void cbpProcessDetailV2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = e.Parameter;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCClaimStatus = '{0}' AND ClaimBy IS NOT NULL AND GCFinalStatus = '{1}' AND ARInvoiceID IS NULL", Constant.ClaimStatus.APPROVED, Constant.FinalStatus.OPEN);

            if (cboDepartment.Value != null)
            {
                if (chkIsExclusionDepartment.Checked)
                {
                    filterExpression += " AND ";
                    filterExpression += string.Format("DepartmentID NOT IN ('{0}')", cboDepartment.Value.ToString());
                }
                else
                {
                    filterExpression += " AND ";
                    filterExpression += string.Format("DepartmentID IN ('{0}')", cboDepartment.Value.ToString());
                }
            }

            filterExpression += " AND ";
            if (cboDateType.Value.ToString() == "0")
            {
                filterExpression += string.Format("PaymentDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else
            {
                filterExpression += string.Format("PaymentCreatedDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboGrouperAmountFinalFilter.Value.ToString() == "1")
            {
                filterExpression += " AND GrouperAmountFinal = 0";
            }
            else if (cboGrouperAmountFinalFilter.Value.ToString() == "2")
            {
                filterExpression += " AND GrouperAmountFinal != 0";
            }

            if (cboFeedbackStatusFilter.Value != null)
            {
                if (cboFeedbackStatusFilter.Value.ToString() != "")
                {
                    if (chkIsExclusion.Checked)
                    {
                        filterExpression += " AND ISNULL(GCFinalClaimFeedbackStatus,'') != '" + cboFeedbackStatusFilter.Value.ToString() + "'";
                    }
                    else
                    {
                        filterExpression += " AND ISNULL(GCFinalClaimFeedbackStatus,'') = '" + cboFeedbackStatusFilter.Value.ToString() + "'";
                    }
                }
                else
                {
                    if (chkIsExclusion.Checked)
                    {
                        filterExpression += " AND ISNULL(GCFinalClaimFeedbackStatus,'') != ''";
                    }
                }
            }
            else
            {
                if (chkIsExclusion.Checked)
                {
                    filterExpression += " AND ISNULL(GCFinalClaimFeedbackStatus,'') != ''";
                }
            }

            if (cboBusinessPartner.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND BusinessPartnerID = '{0}'", cboBusinessPartner.Value.ToString());
            }

            List<vPatientPaymentBPJSFinalClaim1> lst = BusinessLayer.GetvPatientPaymentBPJSFinalClaim1List(filterExpression, int.MaxValue, 1, "NoSEP, RegistrationDate, RegistrationID");
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientPaymentBPJSFinalClaim1 entity = e.Item.DataItem as vPatientPaymentBPJSFinalClaim1;

                TextBox txtGrouperCodeFinal = (TextBox)e.Item.FindControl("txtGrouperCodeFinal");
                TextBox txtGrouperAmountFinal = (TextBox)e.Item.FindControl("txtGrouperAmountFinal");
                ASPxComboBox cboFeedbackStatus = e.Item.FindControl("cboFeedbackStatus") as ASPxComboBox;
                cboFeedbackStatus.ClientInstanceName = string.Format("cboFeedbackStatus{0}", e.Item.DataItemIndex);

                List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.FINAL_CLAIM_FEEDBACK_STATUS));
                Methods.SetComboBoxField<StandardCode>(cboFeedbackStatus, lstSC, "StandardCodeName", "StandardCodeID");

                if (entity.GCFinalClaimFeedbackStatus != null)
                {
                    cboFeedbackStatus.Value = entity.GCFinalClaimFeedbackStatus;
                }
                else
                {
                    cboFeedbackStatus.SelectedIndex = 0;
                }

                txtGrouperCodeFinal.Text = entity.GrouperCodeFinal != null ? entity.GrouperCodeFinal : "";

                txtGrouperAmountFinal.Text = entity.GrouperAmountFinal.ToString();

                CheckBox chkSelectAll = (CheckBox)e.Item.FindControl("chkSelectAll");
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");

                string status = "";
                if (cboFeedbackStatusFilter.Value != null)
                {
                    if (cboFeedbackStatusFilter.Value.ToString() != "")
                    {
                        status = cboFeedbackStatusFilter.Value.ToString();
                    }
                }

                bool isFilterStatus = chkAllThisStatus.Checked;

                if (isFilterStatus)
                {
                    if (!String.IsNullOrEmpty(status))
                    {
                        if (entity.GCFinalClaimFeedbackStatus == status)
                        {
                            chkIsSelected.Checked = true;
                        }
                    }
                    else
                    {
                        chkIsSelected.Checked = true;
                    }
                }
                else
                {
                    chkIsSelected.Checked = false;
                }

                if (!string.IsNullOrEmpty(hdnlstPaymentDetailID.Value))
                {
                    hdnlstPaymentDetailID.Value += ",";
                }
                hdnlstPaymentDetailID.Value += entity.PaymentDetailID;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao paymentHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao paymentDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao paymentDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            BPJSFinalClaimDocumentTempDao claimDocumentTempDao = new BPJSFinalClaimDocumentTempDao(ctx);

            try
            {
                if (type == "approve")
                {
                    if (hdnSelectedPaymentDetailID.Value != "")
                    {
                        #region APPROVED

                        List<String> lstPaymentDtID = hdnSelectedPaymentDetailID.Value.Split(',').ToList();
                        lstPaymentDtID.RemoveAt(0);

                        for (int i = 0; i < lstPaymentDtID.Count(); i++)
                        {
                            PatientPaymentDtInfo entityDtInfo = paymentDtInfoDao.Get(Convert.ToInt32(lstPaymentDtID[i]));
                            if (entityDtInfo.GCFinalStatus != Constant.FinalStatus.APPROVED)
                            {
                                decimal GrouperAmountFinalBefore = entityDtInfo.GrouperAmountFinal;
                                //entityDtInfo.GrouperCodeFinal = lstGrouperCode[i];
                                //entityDtInfo.GrouperAmountFinal = Convert.ToDecimal(lstPaymentAmount[i]);
                                entityDtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                entityDtInfo.FinalBy = AppSession.UserLogin.UserID;
                                entityDtInfo.FinalDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                paymentDtInfoDao.Update(entityDtInfo);

                                PatientPaymentDt entityDt = paymentDtDao.Get(entityDtInfo.PaymentDetailID);
                                PatientPaymentHd entityHd = paymentHdDao.Get(entityDt.PaymentID);

                                if (entityHd.RegistrationID != null && entityHd.RegistrationID != 0)
                                {
                                    Registration oRegistration = registrationDao.Get(Convert.ToInt32(entityHd.RegistrationID));
                                    oRegistration.IsBPJSFinal = true;
                                    oRegistration.GrouperAmountFinal += (entityDtInfo.GrouperAmountFinal - GrouperAmountFinalBefore);
                                    oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    registrationDao.Update(oRegistration);

                                    RegistrationBPJS oRegistrationBPJS = registrationBPJSDao.Get(Convert.ToInt32(entityHd.RegistrationID));
                                    if (oRegistrationBPJS != null)
                                    {
                                        oRegistrationBPJS.GrouperCodeFinal = entityDtInfo.GrouperCodeFinal;
                                        oRegistrationBPJS.GrouperAmountFinal += (entityDtInfo.GrouperAmountFinal - GrouperAmountFinalBefore);
                                        oRegistrationBPJS.GCFinalStatus = entityDtInfo.GCFinalStatus;
                                        oRegistrationBPJS.GCFinalClaimFeedbackStatus = entityDtInfo.GCFinalClaimFeedbackStatus;
                                        oRegistrationBPJS.FinalBy = AppSession.UserLogin.UserID;
                                        oRegistrationBPJS.FinalDate = DateTime.Now;
                                        oRegistrationBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        oRegistrationBPJS.LastUpdatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        registrationBPJSDao.Update(oRegistrationBPJS);
                                    }
                                }
                            }
                        }
                        ctx.CommitTransaction();

                        #endregion
                    }
                    else
                    {
                        result = false;
                        errMessage = GetErrorMsgSelectTransactionFirst();
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else if (type == "approveall")
                {
                    #region APPROVED ALL

                    if (!string.IsNullOrEmpty(hdnlstPaymentDetailID.Value))
                    {
                        string filterExpression = string.Format("PaymentDetailID IN ({0})", hdnlstPaymentDetailID.Value);
                        List<PatientPaymentDtInfo> lstInfoList = BusinessLayer.GetPatientPaymentDtInfoList(filterExpression, ctx);
                        foreach (PatientPaymentDtInfo entityDtInfo in lstInfoList)
                        {
                            if (entityDtInfo.GCFinalStatus != Constant.FinalStatus.APPROVED)
                            {
                                decimal GrouperAmountFinalBefore = entityDtInfo.GrouperAmountFinal;
                                //entityDtInfo.GrouperCodeFinal = lstGrouperCode[i];
                                //entityDtInfo.GrouperAmountFinal = Convert.ToDecimal(lstPaymentAmount[i]);
                                entityDtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                entityDtInfo.FinalBy = AppSession.UserLogin.UserID;
                                entityDtInfo.FinalDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                paymentDtInfoDao.Update(entityDtInfo);

                                PatientPaymentDt entityDt = paymentDtDao.Get(entityDtInfo.PaymentDetailID);
                                PatientPaymentHd entityHd = paymentHdDao.Get(entityDt.PaymentID);

                                if (entityHd.RegistrationID != null && entityHd.RegistrationID != 0)
                                {
                                    Registration oRegistration = registrationDao.Get(Convert.ToInt32(entityHd.RegistrationID));
                                    oRegistration.IsBPJSFinal = true;
                                    oRegistration.GrouperAmountFinal += (entityDtInfo.GrouperAmountFinal - GrouperAmountFinalBefore);
                                    oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    registrationDao.Update(oRegistration);

                                    RegistrationBPJS oRegistrationBPJS = registrationBPJSDao.Get(Convert.ToInt32(entityHd.RegistrationID));
                                    if (oRegistrationBPJS != null)
                                    {
                                        oRegistrationBPJS.GrouperCodeFinal = entityDtInfo.GrouperCodeFinal;
                                        oRegistrationBPJS.GrouperAmountFinal += (entityDtInfo.GrouperAmountFinal - GrouperAmountFinalBefore);
                                        oRegistrationBPJS.GCFinalStatus = entityDtInfo.GCFinalStatus;
                                        oRegistrationBPJS.GCFinalClaimFeedbackStatus = entityDtInfo.GCFinalClaimFeedbackStatus;
                                        oRegistrationBPJS.FinalBy = AppSession.UserLogin.UserID;
                                        oRegistrationBPJS.FinalDate = DateTime.Now;
                                        oRegistrationBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        oRegistrationBPJS.LastUpdatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        registrationBPJSDao.Update(oRegistrationBPJS);
                                    }
                                }
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Data tidak ditemukan.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    errMessage = "Method tidak ditemukan.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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
    }
}