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
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARClaim : BasePageTrx
    {
        private string oGCClaimStatus = Constant.ClaimStatus.OPEN;
        private string oDepartmentID = "%";
        private string oIsExclusion = "0";
        private string oPaymentDate = "";
        private string oGrouperAmountClaimFilter = "";
        private string oBusinessPartnerID = "0";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_AR_CLAIM;
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
            txtClaimDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstGrouperAmountClaim = new List<Variable>();
            lstGrouperAmountClaim.Add(new Variable { Code = "0", Value = "Semua" });
            lstGrouperAmountClaim.Add(new Variable { Code = "1", Value = "Nilai Nol (0)" });
            lstGrouperAmountClaim.Add(new Variable { Code = "2", Value = "Ada Nilai" });
            Methods.SetComboBoxField<Variable>(cboGrouperAmountClaimFilter, lstGrouperAmountClaim, "Value", "Code");
            cboGrouperAmountClaimFilter.Value = "1";

            List<BusinessPartners> lstBP = BusinessLayer.GetBusinessPartnersList(string.Format("IsDeleted = 0 AND IsActive = 1 AND BusinessPartnerID IN (SELECT c.BusinessPartnerID FROM Customer c WHERE c.GCCustomerType = '{0}')", Constant.CustomerType.BPJS));
            lstBP.Insert(0, new BusinessPartners { BusinessPartnerID = 0, BusinessPartnerCode = "", BusinessPartnerName = "-SEMUA-" });
            Methods.SetComboBoxField<BusinessPartners>(cboBusinessPartner, lstBP, "cfBusinessPartnerNameCode", "BusinessPartnerID");
            cboBusinessPartner.Value = "0";

            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_AR_CLAIM_BPJS).ParameterValue;

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCClaimStatus = '{0}'", Constant.ClaimStatus.OPEN);

            if (cboDepartment.Value != null)
            {
                if (chkIsExclusion.Checked)
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
            filterExpression += string.Format("PaymentDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (cboGrouperAmountClaimFilter.Value.ToString() == "1")
            {
                filterExpression += " AND GrouperAmountClaim = 0";
            }
            else if (cboGrouperAmountClaimFilter.Value.ToString() == "2")
            {
                filterExpression += " AND GrouperAmountClaim != 0";
            }

            if (cboBusinessPartner.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND BusinessPartnerID = '{0}'", cboBusinessPartner.Value.ToString());
            }

            List<vPatientPaymentBPJSClaim> lst = BusinessLayer.GetvPatientPaymentBPJSClaimList(filterExpression, int.MaxValue, 1, "NoSEP, RegistrationDate, RegistrationID");
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientPaymentBPJSClaim entity = e.Item.DataItem as vPatientPaymentBPJSClaim;

                TextBox txtGrouperCode = (TextBox)e.Item.FindControl("txtGrouperCode");
                TextBox txtPaymentAmount = (TextBox)e.Item.FindControl("txtPaymentAmount");
                CheckBox chkIsUsingCOB = (CheckBox)e.Item.FindControl("chkIsUsingCOB");

                txtGrouperCode.Text = entity.GrouperCodeClaim;

                txtPaymentAmount.Text = entity.GrouperAmountClaim.ToString();

                chkIsUsingCOB.Checked = entity.IsUsingCOB;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao paymentDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            BPJSClaimDocumentTempDao claimDocumentTempDao = new BPJSClaimDocumentTempDao(ctx);

            try
            {
                if (type == "download")
                {
                    #region Download Document

                    string reportCode = string.Format("ReportCode = '{0}'", "FN-00081");
                    ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                    StringBuilder sbResult = new StringBuilder();

                    List<dynamic> lstDynamic = null;
                    List<Variable> lstVariable = new List<Variable>();

                    if (cboDepartment.Value != null)
                    {
                        oDepartmentID = cboDepartment.Value.ToString();

                        if (chkIsExclusion.Checked)
                        {
                            oIsExclusion = "1";
                        }
                    }

                    oPaymentDate = string.Format("{0}|{1}", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

                    oGrouperAmountClaimFilter = cboGrouperAmountClaimFilter.Value.ToString();

                    oBusinessPartnerID = cboBusinessPartner.Value.ToString();

                    lstVariable.Add(new Variable { Code = "GCClaimStatus", Value = oGCClaimStatus });
                    lstVariable.Add(new Variable { Code = "DepartmentID", Value = oDepartmentID });
                    lstVariable.Add(new Variable { Code = "IsExclusion", Value = oIsExclusion });
                    lstVariable.Add(new Variable { Code = "PaymentDate", Value = oPaymentDate });
                    lstVariable.Add(new Variable { Code = "GrouperAmountClaimFilter", Value = oGrouperAmountClaimFilter });
                    //lstVariable.Add(new Variable { Code = "BusinessPartnerID", Value = oBusinessPartnerID });

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
                            else if (fieldName == "GrouperCodeClaim")
                            {
                                fieldName = "ISI_GrouperCodeClaim";
                            }
                            else if (fieldName == "GrouperAmountClaim")
                            {
                                fieldName = "ISI_GrouperAmountClaim";
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
                        List<BPJSClaimDocumentTemp> claimTempBeginList = BusinessLayer.GetBPJSClaimDocumentTempList("1=1", ctx);
                        foreach (BPJSClaimDocumentTemp claimTempBegin in claimTempBeginList)
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

                                BPJSClaimDocumentTemp oData = new BPJSClaimDocumentTemp();
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
                                oData.DepartmentID = fieldTemp[12];
                                oData.ServiceUnitCode = fieldTemp[13];
                                oData.ServiceUnitName = fieldTemp[14];
                                oData.ParamedicCode = fieldTemp[15];
                                oData.ParamedicName = fieldTemp[16];
                                oData.IsUsingCOB = Convert.ToBoolean(fieldTemp[17]);
                                oData.PaymentNo = fieldTemp[18];
                                oData.PaymentDate = fieldTemp[19];
                                oData.PaymentTime = fieldTemp[20];
                                oData.BusinessPartnerName = fieldTemp[21];
                                oData.PaymentAmount = Convert.ToDecimal(fieldTemp[22]);
                                oData.GrouperCodeClaim = fieldTemp[23];
                                oData.GrouperAmountClaim = Convert.ToDecimal(fieldTemp[24]);
                                oData.ClaimByName = fieldTemp[25];
                                //oData.ClaimDate = fieldTemp[26];
                                oData.ClaimDate = Helper.GetDatePickerValue(Request.Form[txtClaimDate.UniqueID]).ToString();
                                oData.GrouperCodeFinal = fieldTemp[27];
                                oData.GrouperAmountFinal = Convert.ToDecimal(fieldTemp[28]);
                                oData.FinalByName = fieldTemp[29];
                                oData.FinalDate = fieldTemp[30];
                                oData.CreatedBy = AppSession.UserLogin.UserID;
                                oData.CreatedDate = DateTime.Now;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                claimDocumentTempDao.Insert(oData);

                                if (oData.RegistrationID != null)
                                {
                                    RegistrationBPJS oRegistrationBPJS = registrationBPJSDao.Get(Convert.ToInt32(oData.RegistrationID));
                                    if (oRegistrationBPJS != null)
                                    {
                                        if (oRegistrationBPJS.NoSEP != oData.NoSEP)
                                        {
                                            oRegistrationBPJS.NoSEP = oData.NoSEP;
                                            if (!string.IsNullOrEmpty(oData.TanggalSEP))
                                            {
                                                oRegistrationBPJS.TanggalSEP = Convert.ToDateTime(oData.TanggalSEP);
                                            }
                                            if (!string.IsNullOrEmpty(oData.JamSEP))
                                            {
                                                oRegistrationBPJS.JamSEP = oData.JamSEP;
                                            }
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
                                        if (!string.IsNullOrEmpty(oData.TanggalSEP))
                                        {
                                            oRegistrationBPJSNew.TanggalSEP = Convert.ToDateTime(oData.TanggalSEP);
                                        }
                                        if (!string.IsNullOrEmpty(oData.JamSEP))
                                        {
                                            oRegistrationBPJSNew.JamSEP = oData.JamSEP;
                                        }
                                        if (!string.IsNullOrEmpty(oData.NoPeserta))
                                        {
                                            oRegistrationBPJSNew.NoPeserta = oData.NoPeserta;
                                        }
                                        if (!string.IsNullOrEmpty(oData.NamaPeserta))
                                        {
                                            oRegistrationBPJSNew.NamaPeserta = oData.NamaPeserta;
                                        }
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
                                        if (oPaymentDtInfo.GCClaimStatus != Constant.ClaimStatus.APPROVED)
                                        {
                                            oPaymentDtInfo.GrouperCodeClaim = oData.GrouperCodeClaim;
                                            oPaymentDtInfo.GrouperAmountClaim = oData.GrouperAmountClaim;
                                            oPaymentDtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                            //oPaymentDtInfo.ClaimDate = DateTime.Now;
                                            oPaymentDtInfo.ClaimDate = Helper.GetDatePickerValue(Request.Form[txtClaimDate.UniqueID]);
                                            oPaymentDtInfo.ClaimSelectedDate = DateTime.Now;
                                            oPaymentDtInfo.FinalSelectedDate = DateTime.Now;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            paymentDtInfoDao.Update(oPaymentDtInfo);
                                        }
                                    }
                                    else
                                    {
                                        PatientPaymentDtInfo oPaymentDtInfoNew = new PatientPaymentDtInfo();
                                        oPaymentDtInfoNew.PaymentDetailID = Convert.ToInt32(oData.PaymentDetailID);
                                        oPaymentDtInfoNew.GrouperCodeClaim = oData.GrouperCodeClaim;
                                        oPaymentDtInfoNew.GrouperAmountClaim = oData.GrouperAmountClaim;
                                        oPaymentDtInfoNew.ClaimBy = AppSession.UserLogin.UserID;
                                        //oPaymentDtInfoNew.ClaimDate = DateTime.Now;
                                        oPaymentDtInfoNew.ClaimDate = Helper.GetDatePickerValue(Request.Form[txtClaimDate.UniqueID]);
                                        oPaymentDtInfoNew.ClaimSelectedDate = DateTime.Now;
                                        oPaymentDtInfoNew.FinalSelectedDate = DateTime.Now;
                                        oPaymentDtInfoNew.GCClaimStatus = Constant.ClaimStatus.OPEN;
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
                else
                {
                    if (hdnSelectedPaymentDetailID.Value != "")
                    {
                        List<String> lstPaymentDtID = hdnSelectedPaymentDetailID.Value.Split(',').ToList();
                        List<String> lstPaymentAmount = hdnSelectedPaymentAmount.Value.Split(',').ToList();
                        List<String> lstGrouperCode = hdnSelectedGrouperCode.Value.Split(',').ToList();

                        lstPaymentDtID.RemoveAt(0);
                        lstPaymentAmount.RemoveAt(0);
                        lstGrouperCode.RemoveAt(0);

                        if (type == "save")
                        {
                            #region SAVE

                            for (int i = 0; i < lstPaymentDtID.Count(); i++)
                            {
                                PatientPaymentDtInfo entityDtInfo = paymentDtInfoDao.Get(Convert.ToInt32(lstPaymentDtID[i]));
                                if (entityDtInfo.GCClaimStatus != Constant.ClaimStatus.APPROVED)
                                {
                                    entityDtInfo.GrouperCodeClaim = lstGrouperCode[i];
                                    entityDtInfo.GrouperAmountClaim = Convert.ToDecimal(lstPaymentAmount[i]);
                                    entityDtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                    entityDtInfo.ClaimDate = Helper.GetDatePickerValue(Request.Form[txtClaimDate.UniqueID]);
                                    entityDtInfo.ClaimSelectedDate = DateTime.Now;
                                    entityDtInfo.FinalSelectedDate = DateTime.Now;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    paymentDtInfoDao.Update(entityDtInfo);
                                }
                            }

                            ctx.CommitTransaction();

                            #endregion
                        }
                        else if (type == "approve")
                        {
                            #region APPROVED

                            for (int i = 0; i < lstPaymentDtID.Count(); i++)
                            {
                                PatientPaymentDtInfo entityDtInfo = paymentDtInfoDao.Get(Convert.ToInt32(lstPaymentDtID[i]));
                                if (entityDtInfo.GCClaimStatus != Constant.ClaimStatus.APPROVED)
                                {
                                    entityDtInfo.GrouperCodeClaim = lstGrouperCode[i];
                                    entityDtInfo.GrouperAmountClaim = Convert.ToDecimal(lstPaymentAmount[i]);
                                    entityDtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                    entityDtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                    entityDtInfo.ClaimDate = Helper.GetDatePickerValue(Request.Form[txtClaimDate.UniqueID]);
                                    entityDtInfo.ClaimSelectedDate = DateTime.Now;
                                    entityDtInfo.FinalSelectedDate = DateTime.Now;

                                    if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "0")
                                    {
                                        entityDtInfo.GrouperCodeFinal = entityDtInfo.GrouperCodeClaim;
                                        entityDtInfo.GrouperAmountFinal = entityDtInfo.GrouperAmountFinal;
                                        entityDtInfo.GCFinalStatus = Constant.FinalStatus.OPEN;
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    paymentDtInfoDao.Update(entityDtInfo);

                                    PatientPaymentDt paymentDt = entityDtDao.Get(entityDtInfo.PaymentDetailID);
                                    PatientPaymentHd paymentHd = entityHdDao.Get(paymentDt.PaymentID);
                                    RegistrationBPJS regBPJS = registrationBPJSDao.Get(paymentHd.RegistrationID);
                                    if (regBPJS != null)
                                    {
                                        regBPJS.GCClaimStatus = entityDtInfo.GCClaimStatus;
                                        regBPJS.ClaimBy = entityDtInfo.ClaimBy;
                                        regBPJS.ClaimDate = entityDtInfo.ClaimDate;
                                        regBPJS.GCFinalStatus = entityDtInfo.GCFinalStatus;
                                        regBPJS.FinalBy = entityDtInfo.FinalBy;
                                        if (entityDtInfo.FinalBy != null) { regBPJS.FinalDate = entityDtInfo.FinalDate; }
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        registrationBPJSDao.Update(regBPJS);
                                    }
                                }
                            }

                            ctx.CommitTransaction();

                            #endregion
                        }
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