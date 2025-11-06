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
    public partial class ARFinalClaimv2New : BasePageTrx
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
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_AR_FINAL_CLAIM_V2_NEW;
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

            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageCount = 1;
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

        protected void cbpProcessDetailV2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "save")
                {
                    OnSaveAddRecord(e.Parameter, ref errMessage);
                    result = "save|" + errMessage;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
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

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientPaymentBPJSFinalClaimRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_TEMP_MAX_FOR_ORDERING);
            }

            List<vPatientPaymentBPJSFinalClaim> lst = BusinessLayer.GetvPatientPaymentBPJSFinalClaimList(filterExpression, Constant.GridViewPageSize.GRID_TEMP_MAX_FOR_ORDERING, pageIndex, "NoSEP, RegistrationDate, RegistrationID");
            lvwView.DataSource = lst;
            lvwView.DataBind();

            hdnPageCount.Value = PageCount.ToString();

            divTotalRecord.InnerHtml = hdnTotalRecord.Value = lst.Count.ToString();
            List<vPatientPaymentBPJSFinalClaim> lstSEP = lst.GroupBy(test => test.NoSEP).Select(grp => grp.First()).ToList().OrderBy(x => x.NoSEP).ToList();
            divNoSEP.InnerHtml = hdnTotalSEP.Value = lstSEP.Count().ToString();
            divGrouperAmount.InnerHtml = hdnTotalGrouper.Value = lst.Sum(t => t.GrouperAmountFinal).ToString(Constant.FormatString.NUMERIC_2);
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientPaymentBPJSFinalClaim entity = e.Item.DataItem as vPatientPaymentBPJSFinalClaim;

                TextBox txtGrouperCodeFinal = (TextBox)e.Item.FindControl("txtGrouperCodeFinal");
                TextBox txtGrouperAmountFinal = (TextBox)e.Item.FindControl("txtGrouperAmountFinal");
                CheckBox chkIsUsingCOB = (CheckBox)e.Item.FindControl("chkIsUsingCOB");
                ASPxComboBox cboFeedbackStatus = e.Item.FindControl("cboFeedbackStatus") as ASPxComboBox;
                cboFeedbackStatus.ClientInstanceName = string.Format("cboFeedbackStatus{0}", e.Item.DataItemIndex);

                List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.FINAL_CLAIM_FEEDBACK_STATUS));
                Methods.SetComboBoxField<StandardCode>(cboFeedbackStatus, lstSC, "StandardCodeName", "StandardCodeID");

                HtmlInputButton btnSave = e.Item.FindControl("btnSave") as HtmlInputButton;
                //btnSave.Attributes.Add("enabled", "false");

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

                chkIsUsingCOB.Checked = entity.IsUsingCOB;

                string status = "";
                if (cboFeedbackStatusFilter.Value != null)
                {
                    if (cboFeedbackStatusFilter.Value.ToString() != "")
                    {
                        status = cboFeedbackStatusFilter.Value.ToString();
                    }
                }
            }
        }

        protected bool OnSaveAddRecord(string data, ref string errMessage)
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
                string[] param = data.Split('|');
                PatientPaymentDtInfo entityDtInfo = paymentDtInfoDao.Get(Convert.ToInt32(param[1]));
                if (entityDtInfo.GCFinalStatus != Constant.FinalStatus.APPROVED)
                {
                    entityDtInfo.GrouperCodeFinal = param[3];
                    entityDtInfo.GrouperAmountFinal = Convert.ToDecimal(param[4]);
                    entityDtInfo.FinalBy = AppSession.UserLogin.UserID;
                    entityDtInfo.FinalDate = DateTime.Now;
                    entityDtInfo.GCFinalClaimFeedbackStatus = param[2];
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    paymentDtInfoDao.Update(entityDtInfo);

                    ctx.CommitTransaction();
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
                if (type == "download")
                {
                    #region Download Document

                    string reportCode = string.Format("ReportCode = '{0}'", "FN-00090");
                    ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                    StringBuilder sbResult = new StringBuilder();

                    List<dynamic> lstDynamic = null;
                    List<Variable> lstVariable = new List<Variable>();

                    if (cboDepartment.Value != null)
                    {
                        oDepartmentID = cboDepartment.Value.ToString();

                        if (chkIsExclusionDepartment.Checked)
                        {
                            oIsExclusion = "1";
                        }
                    }
                    oPaymentDate = string.Format("{0}|{1}", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

                    oGrouperAmountFinalFilter = cboGrouperAmountFinalFilter.Value.ToString();

                    oGCFeedbackStatus = "%%";
                    if (cboFeedbackStatusFilter.Value != null)
                    {
                        oGCFeedbackStatus = cboFeedbackStatusFilter.Value.ToString();

                        if (chkIsExclusion.Checked)
                        {
                            oIsExclusionFeedback = "1";
                        }
                    }

                    oDownloadType = cboDownloadType.Value.ToString();
                    oBusinessPartnerID = cboBusinessPartner.Value.ToString();
                    oDateType = cboDateType.Value.ToString();

                    lstVariable.Add(new Variable { Code = "GCClaimFinalStatus", Value = oGCClaimFinalStatus });
                    lstVariable.Add(new Variable { Code = "DepartmentID", Value = oDepartmentID });
                    lstVariable.Add(new Variable { Code = "IsExclusion", Value = oIsExclusion });
                    lstVariable.Add(new Variable { Code = "DateType", Value = oDateType });
                    lstVariable.Add(new Variable { Code = "PaymentDate", Value = oPaymentDate });
                    lstVariable.Add(new Variable { Code = "GrouperAmountFinalFilter", Value = oGrouperAmountFinalFilter });
                    lstVariable.Add(new Variable { Code = "GCFeedbackStatus", Value = oGCFeedbackStatus });
                    lstVariable.Add(new Variable { Code = "IsExclusionFeedback", Value = oIsExclusionFeedback });
                    lstVariable.Add(new Variable { Code = "DownloadType", Value = oDownloadType });
                    lstVariable.Add(new Variable { Code = "BusinessPartnerID", Value = oBusinessPartnerID });

                    lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                    if (lstDynamic.Count() != 0)
                    {
                        dynamic fields = lstDynamic[0];

                        foreach (var prop in fields.GetType().GetProperties())
                        {
                            string fieldName = prop.Name;

                            if (fieldName == "NoSEP")
                            {
                                fieldName = "ISI_NoSEP";
                            }
                            else if (fieldName == "GrouperCodeFinal")
                            {
                                fieldName = "ISI_GrouperCodeFinal";
                            }
                            else if (fieldName == "GrouperAmountFinal")
                            {
                                fieldName = "ISI_GrouperAmountFinal";
                            }
                            else if (fieldName == "GCFinalClaimFeedbackStatus")
                            {
                                fieldName = "ISI_GCFinalClaimFeedbackStatus";
                            }

                            sbResult.Append(fieldName);
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");

                        for (int i = 0; i < lstDynamic.Count; ++i)
                        {
                            dynamic entity = lstDynamic[i];

                            foreach (var prop in entity.GetType().GetProperties())
                            {
                                sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                                sbResult.Append(",");
                            }

                            sbResult.Append("\r\n");
                        }

                        retval = sbResult.ToString();

                    }
                    else
                    {
                        retval = "Download Failed";
                    }

                    ctx.RollBackTransaction();

                    #endregion

                }
                else if (type == "upload")
                {
                    #region Upload Document

                    string fileUpload = hdnBPJSUploadedFile.Value;
                    if (fileUpload != "")
                    {
                        string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                        fileUpload = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        Directory.Delete(path, true);
                        Directory.CreateDirectory(path);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<BPJSFinalClaimDocumentTemp> claimTempBeginList = BusinessLayer.GetBPJSFinalClaimDocumentTempList("1=1", ctx);
                        foreach (BPJSFinalClaimDocumentTemp claimTempBegin in claimTempBeginList)
                        {
                            claimDocumentTempDao.Delete(claimTempBegin.ID);
                        }
                    }

                    FileStream fs = new FileStream(string.Format("{0}{1}", path, hdnFileName.Value), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    byte[] data = Convert.FromBase64String(fileUpload);
                    bw.Write(data);
                    bw.Close();

                    string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, hdnFileName.Value));

                    int rowCount = 0;
                    foreach (string temp in lstTemp)
                    {
                        if (rowCount != 0)
                        {
                            if (temp.Contains(','))
                            {
                                List<String> fieldTemp = temp.Split(',').ToList();

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                BPJSFinalClaimDocumentTemp oData = new BPJSFinalClaimDocumentTemp();
                                if (fieldTemp[0] != null && fieldTemp[0] != "" && fieldTemp[0] != "0")
                                {
                                    oData.RegistrationID = Convert.ToInt32(fieldTemp[0]);
                                }
                                else
                                {
                                    oData.RegistrationID = null;
                                }
                                if (fieldTemp[1] != null && fieldTemp[1] != "" && fieldTemp[1] != "0")
                                {
                                    oData.PaymentDetailID = Convert.ToInt32(fieldTemp[1]);
                                }
                                else
                                {
                                    oData.PaymentDetailID = null;
                                }
                                oData.RegistrationNo = fieldTemp[2];
                                oData.RegistrationDate = fieldTemp[3];
                                oData.RegistrationTime = fieldTemp[4];
                                oData.MedicalNo = fieldTemp[5];
                                oData.PatientName = fieldTemp[6];
                                oData.NoPeserta = fieldTemp[7];
                                oData.NamaPeserta = fieldTemp[8];
                                oData.NoSEP = fieldTemp[9];
                                oData.TanggalSEP = fieldTemp[10];
                                oData.JamSEP = fieldTemp[11];
                                oData.DepartmentID = fieldTemp[13];
                                oData.ServiceUnitCode = fieldTemp[14];
                                oData.ServiceUnitName = fieldTemp[15];
                                oData.ParamedicCode = fieldTemp[16];
                                oData.ParamedicName = fieldTemp[17];
                                oData.NamaKelasDitempati = fieldTemp[18];
                                oData.NamaKelasTanggungan = fieldTemp[19];
                                oData.IsUsingCOB = Convert.ToBoolean(fieldTemp[20]);
                                oData.PaymentNo = fieldTemp[22];
                                oData.PaymentDate = fieldTemp[23];
                                oData.PaymentTime = fieldTemp[24];
                                oData.BusinessPartnerName = fieldTemp[25];
                                oData.TotalChargesAmount = Convert.ToDecimal(fieldTemp[26]);
                                oData.PaymentAmount = Convert.ToDecimal(fieldTemp[27]);
                                oData.GrouperCodeClaim = fieldTemp[28];
                                oData.GrouperAmountClaim = Convert.ToDecimal(fieldTemp[29]);
                                oData.ClaimByName = fieldTemp[30];
                                oData.ClaimDate = fieldTemp[31];
                                oData.GCFinalClaimFeedbackStatus = fieldTemp[32];
                                oData.FinalClaimFeedbackStatus = fieldTemp[33];
                                oData.GrouperCodeFinal = fieldTemp[34];
                                oData.GrouperAmountFinal = Convert.ToDecimal(fieldTemp[35]);
                                oData.FinalByName = fieldTemp[36];
                                oData.FinalDate = fieldTemp[37];
                                oData.TotalPatientBillAmount = Convert.ToDecimal(fieldTemp[38]);
                                oData.TotalPayerBillAmount = Convert.ToDecimal(fieldTemp[39]);
                                oData.TotalPayerCOB = Convert.ToDecimal(fieldTemp[40]);
                                oData.CreatedBy = AppSession.UserLogin.UserID;
                                oData.CreatedDate = DateTime.Now;

                                if (oData.GrouperAmountFinal != 0 && !String.IsNullOrEmpty(oData.NoSEP) && oData.NoSEP != "")
                                {
                                    if (String.IsNullOrEmpty(oData.GCFinalClaimFeedbackStatus) || oData.GCFinalClaimFeedbackStatus == "")
                                    {
                                        oData.GCFinalClaimFeedbackStatus = Constant.FinalClaimFeedbackStatus.TAGIHAN_DISETUJUI;
                                    }
                                }

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                claimDocumentTempDao.Insert(oData);

                                if (oData.RegistrationID != null)
                                {
                                    RegistrationBPJS oRegistrationBPJS = registrationBPJSDao.Get(Convert.ToInt32(oData.RegistrationID));
                                    if (oRegistrationBPJS != null)
                                    {
                                        if (oRegistrationBPJS.IsManualSEP && oRegistrationBPJS.NoSEP != oData.NoSEP)
                                        {
                                            oRegistrationBPJS.NoSEP = oData.NoSEP;
                                            oRegistrationBPJS.IsManualSEP = true;
                                        }
                                        oRegistrationBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        oRegistrationBPJS.LastUpdatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        registrationBPJSDao.Update(oRegistrationBPJS);
                                    }
                                    else
                                    {
                                        RegistrationBPJS oRegistrationBPJSNew = new RegistrationBPJS();
                                        oRegistrationBPJSNew.RegistrationID = Convert.ToInt32(oData.RegistrationID);
                                        oRegistrationBPJSNew.NoSEP = oData.NoSEP;
                                        oRegistrationBPJSNew.IsManualSEP = true;
                                        oRegistrationBPJSNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        oRegistrationBPJSNew.LastUpdatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        registrationBPJSDao.Insert(oRegistrationBPJSNew);
                                    }
                                }

                                if (oData.PaymentDetailID != null)
                                {
                                    PatientPaymentDtInfo oPaymentDtInfo = paymentDtInfoDao.Get(Convert.ToInt32(oData.PaymentDetailID));
                                    if (oPaymentDtInfo != null)
                                    {
                                        if (oPaymentDtInfo.GCFinalStatus != Constant.FinalStatus.APPROVED)
                                        {
                                            oPaymentDtInfo.GrouperCodeFinal = oData.GrouperCodeFinal;
                                            oPaymentDtInfo.GrouperAmountFinal = oData.GrouperAmountFinal;
                                            oPaymentDtInfo.FinalBy = AppSession.UserLogin.UserID;
                                            oPaymentDtInfo.FinalDate = DateTime.Now;
                                            oPaymentDtInfo.GCFinalClaimFeedbackStatus = oData.GCFinalClaimFeedbackStatus;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            paymentDtInfoDao.Update(oPaymentDtInfo);
                                        }
                                    }
                                    else
                                    {
                                        PatientPaymentDtInfo oPaymentDtInfoNew = new PatientPaymentDtInfo();
                                        oPaymentDtInfoNew.PaymentDetailID = Convert.ToInt32(oData.PaymentDetailID);
                                        oPaymentDtInfoNew.GrouperCodeFinal = oData.GrouperCodeClaim;
                                        oPaymentDtInfoNew.GrouperAmountFinal = oData.GrouperAmountClaim;
                                        oPaymentDtInfoNew.FinalBy = AppSession.UserLogin.UserID;
                                        oPaymentDtInfoNew.FinalDate = DateTime.Now;
                                        oPaymentDtInfoNew.GCFinalClaimFeedbackStatus = oData.GCFinalClaimFeedbackStatus;
                                        oPaymentDtInfoNew.GCFinalStatus = Constant.FinalStatus.OPEN;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        paymentDtInfoDao.Insert(oPaymentDtInfoNew);
                                    }
                                }
                            }
                        }
                        rowCount += 1;
                    }

                    ctx.CommitTransaction();

                    #endregion

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