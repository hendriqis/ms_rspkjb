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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentManagementTransactionDetailServiceCtl : BaseUserControlCtl
    {
        protected bool IsShowParamedicTeam = false;
        protected bool IsShowSwitchIcon = false;
        List<SettingParameter> lstSettingParameter = null;

        //private BasePageTrxPatientManagement DetailPage
        //{
        //    get { return (BasePageTrxPatientManagement)Page; }
        //}

        protected string GetMainParamedicRole()
        {
            return Constant.ParamedicRole.PELAKSANA;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Registration reg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                hdnBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

                List<vSettingParameterDt> lstSP = BusinessLayer.GetvSettingParameterDtList(string.Format(
                    "ParameterCode IN ('{0}','{1}','{2}') AND HealthcareID = '{3}'",
                    Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS, AppSession.UserLogin.HealthcareID));
                hdnImagingServiceUnitID.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                hdnIsOnlyBPJS.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS).ParameterValue;

                lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}')",
                    Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT, Constant.SettingParameter.PRESCRIPTION_RETURN_ITEM, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));

                hdnLabHealthcareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue)).FirstOrDefault().HealthcareServiceUnitID.ToString();

                hdnTariffComp1Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
                hdnTariffComp2Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
                hdnTariffComp3Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;
                hdnPrescriptionReturnItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_RETURN_ITEM).ParameterValue;

                Helper.SetControlEntrySetting(txtServiceCITO, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceDiscount, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceComplication, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServicePatient, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServicePayer, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceQty, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceTariff, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceTotal, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceUnitTariff, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceItemCode, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServicePhysicianCode, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(cboServiceChargeClassID, new ControlEntrySetting(true, true, true), "mpTrxService");
            }
        }

        public void InitializeTransactionControl(bool flagHaveCharges, string HealthcareServiceUnitID, string appointmentID)
        {
            int healthcareServiceUnitID = Convert.ToInt32(HealthcareServiceUnitID);
            hdnAppointmentID.Value = appointmentID;
            if (flagHaveCharges)
            {
                BindGridService();
                if (healthcareServiceUnitID > 0)
                    hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", healthcareServiceUnitID);
                else
                    hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {{HealthcareServiceUnitID}} AND IsDeleted = 0");
            }
            else
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
                }
                else
                {
                    if (healthcareServiceUnitID > 0)
                        hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", healthcareServiceUnitID);
                    else
                        hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
                }
            }
        }

        public void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField(cboServiceChargeClassID, lstClassCare, "ClassName", "ClassID");
            cboServiceChargeClassID.SelectedIndex = 0;
        }

        public void OnAddRecord()
        {
            //txtServicePhysicianCode.Text = "";
            //hdnHealthcareServiceUnitID.Value = DetailPage.GetHealthcareServiceUnitID().ToString();
            //if (DetailPage.GetDepartmentID().ToString() == Constant.Facility.INPATIENT)
            //{
            //    hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
            //}
            //else
            //{
            //    if (Convert.ToInt32(hdnHealthcareServiceUnitID.Value) > 0)
            //        hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", DetailPage.GetHealthcareServiceUnitID());
            //    else
            //        hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
            //}
            //BindGridService();
            //IsEditable = true;
            //hdnIsEditable.Value = "1";
        }

        protected string GetTariffComponent1Text()
        {
            return hdnTariffComp1Text.Value;
        }

        protected string GetTariffComponent2Text()
        {
            return hdnTariffComp2Text.Value;
        }

        protected string GetTariffComponent3Text()
        {
            return hdnTariffComp3Text.Value;
        }

        protected bool IsEditable = true;
        private void BindGridService()
        {
            //string GCTransactionStatus = DetailPage.GetGCTransactionStatus();
            //Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));

            //IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            //hdnIsEditable.Value = IsEditable ? "1" : "0";

            //string filterExpression = "1 = 0";
            //hdnServiceTransactionID.Value = DetailPage.GetTransactionHdID();
            //if (hdnServiceTransactionID.Value != "")
            //    filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 ORDER BY ID", hdnServiceTransactionID.Value, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.ItemGroupMaster.MEDICAL_CHECKUP);
            //List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);

            //if (DetailPage.GetHealthcareServiceUnitID().ToString() == hdnLabHealthcareServiceUnitID.Value)
            //    IsShowParamedicTeam = false;
            //else
            //    IsShowParamedicTeam = (DetailPage.GetDepartmentID() == Constant.Facility.DIAGNOSTIC);
            //IsShowSwitchIcon = (DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL);
            //IsShowSwitchIcon = (DetailPage.IsPatientBillSummaryPage());
            //IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;
            //lvwService.DataSource = lst;
            //lvwService.DataBind();

            //decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            //decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            //decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            //if (lst.Count > 0)
            //{
            //    ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
            //    ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
            //    ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = totalLineAmount.ToString("N");
            //}
            //hdnServiceAllTotalPatient.Value = totalPatientAmount.ToString();
            //hdnServiceAllTotalPayer.Value = totalPayerAmount.ToString();
        }

        #region Service
        protected void cbpService_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
//                transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (hdnServiceTransactionDtID.Value.ToString() != "")
                {
                    if (OnBeforeSaveRecordService(ref errMessage))
                    {
                        if (OnSaveEditRecordService(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnBeforeSaveRecordService(ref errMessage))
                    {
                        if (OnSaveAddRecordService(ref errMessage, ref transactionID))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
//                        transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                        result += string.Format("fail|{0}", errMessage);
                    }
//                    DetailPage.SetTransactionHdID(transactionID.ToString());
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteChargesDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridService();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private bool OnSwitchChargesDt(ref string errMessage, int ID)
        {
            return true;
//            bool result = true;
//            IDbContext ctx = DbFactory.Configure(true);
//            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
//            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
//            try
//            {
////                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
//                if (transactionID > 0)
//                {
//                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
//                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
//                    {
//                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
//                        {
//                            PatientChargesDt entity = entityDtDao.Get(ID);
//                            if (!entity.IsDeleted)
//                            {
//                                decimal temp = entity.PayerAmount;
//                                entity.PayerAmount = entity.PatientAmount;
//                                entity.PatientAmount = temp;
//                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
//                                entityDtDao.Update(entity);
//                            }
//                        }
//                        else
//                        {
//                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
//                            result = false;
//                        }
//                    }
//                    else
//                    {
//                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
//                        {
//                            PatientChargesDt entity = entityDtDao.Get(ID);
//                            if (!entity.IsDeleted)
//                            {
//                                decimal temp = entity.PayerAmount;
//                                entity.PayerAmount = entity.PatientAmount;
//                                entity.PatientAmount = temp;
//                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
//                                entityDtDao.Update(entity);
//                            }
//                        }
//                        else
//                        {
//                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
//                            result = false;
//                        }
//                    }
//                }
//                ctx.CommitTransaction();
//            }
//            catch (Exception ex)
//            {
//                result = false;
//                errMessage = ex.Message;
//                Helper.InsertErrorLog(ex);
//                ctx.RollBackTransaction();
//            }
//            finally
//            {
//                ctx.Close();
//            }
//            return result;
        }

        private bool OnDeleteChargesDt(ref string errMessage, string param)
        {
            return true;
            //bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            //PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            //string[] paramDelete = param.Split(';');
            //int ID = Convert.ToInt32(paramDelete[0]);
            //string gcDeleteReason = paramDelete[1];
            //string reason = paramDelete[2];
            //try
            //{
            //    int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            //    if (transactionID > 0)
            //    {
            //        PatientChargesHd entityHd = entityHdDao.Get(transactionID);
            //        if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            //        {
            //            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            //            {
            //                PatientChargesDt entity = entityDtDao.Get(ID);
            //                if (!entity.IsDeleted)
            //                {
            //                    entity.GCDeleteReason = gcDeleteReason;
            //                    entity.DeleteReason = reason;
            //                    entity.DeleteDate = DateTime.Now;
            //                    entity.DeleteBy = AppSession.UserLogin.UserID;
            //                    entity.IsDeleted = true;
            //                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //                    entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
            //                    entityDtDao.Update(entity);
            //                }
            //            }
            //            else
            //            {
            //                errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dihapus";
            //                result = false;
            //            }
            //        }
            //        else
            //        {
            //            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            //            {
            //                PatientChargesDt entity = entityDtDao.Get(ID);
            //                if (!entity.IsDeleted)
            //                {
            //                    entity.GCDeleteReason = gcDeleteReason;
            //                    entity.DeleteReason = reason;
            //                    entity.DeleteDate = DateTime.Now;
            //                    entity.DeleteBy = AppSession.UserLogin.UserID;
            //                    entity.IsDeleted = true;
            //                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //                    entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
            //                    entityDtDao.Update(entity);
            //                }
            //            }
            //            else
            //            {
            //                errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dihapus";
            //                result = false;
            //            }
            //        }
            //    }
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    errMessage = ex.Message;
            //    Helper.InsertErrorLog(ex);
            //    ctx.RollBackTransaction();
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            //return result;
        }

        public void OnVoidAllChargesDt(IDbContext ctx, int transactionHdID)
        {
            string a = "";
            //PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            //PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            //ItemBalanceDao itemBalanceDao = new ItemBalanceDao(ctx);
            //PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            //if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            //{
            //    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            //    {
            //        List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{5}'", transactionHdID, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.TransactionStatus.VOID), ctx);
            //        foreach (vPatientChargesDt1 patientChargesDt in lstPatientChargesDt)
            //        {
            //            PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
            //            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
            //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //            entityDtDao.Update(entity);
            //        }
            //    }
            //}
            //else
            //{
            //    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            //    {
            //        List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{5}'", transactionHdID, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.TransactionStatus.VOID), ctx);
            //        foreach (vPatientChargesDt1 patientChargesDt in lstPatientChargesDt)
            //        {
            //            PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
            //            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
            //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //            entityDtDao.Update(entity);
            //        }
            //    }
            //}
        }

        private void ServiceControlToEntity(PatientChargesDt entity)
        {
            entity.RevenueSharingID = Convert.ToInt32(hdnServiceRevenueSharingID.Value);
            if (entity.RevenueSharingID == 0)
                entity.RevenueSharingID = null;
            entity.ParamedicID = Convert.ToInt32(hdnServicePhysicianID.Value);
            if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
                entity.IsSubContractItem = true;
            }
            else
            {
                entity.BusinessPartnerID = null;
                entity.IsSubContractItem = false;
            }
            entity.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);
            entity.IsVariable = chkServiceIsVariable.Checked;
            entity.IsUnbilledItem = chkServiceIsUnbilledItem.Checked;
            entity.Tariff = Convert.ToDecimal(Request.Form[txtServiceUnitTariff.UniqueID]);
            entity.IsCITO = chkServiceIsCITO.Checked;
            entity.CITOAmount = Convert.ToDecimal(Request.Form[txtServiceCITO.UniqueID]);
            entity.IsComplication = chkServiceIsComplication.Checked;
            entity.ComplicationAmount = Convert.ToDecimal(Request.Form[txtServiceComplication.UniqueID]);
            entity.IsDiscount = chkServiceIsDiscount.Checked;
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtServiceDiscount.UniqueID]);
            entity.UsedQuantity = entity.BaseQuantity = entity.ChargedQuantity = Convert.ToDecimal(txtServiceQty.Text);
            entity.PatientAmount = Convert.ToDecimal(Request.Form[txtServicePatient.UniqueID]);
            entity.PayerAmount = Convert.ToDecimal(Request.Form[txtServicePayer.UniqueID]);
            entity.LineAmount = Convert.ToDecimal(Request.Form[txtServiceTotal.UniqueID]);
            entity.TariffComp1 = Convert.ToDecimal(Request.Form[txtServiceTariffComp1.UniqueID]);
            entity.TariffComp2 = Convert.ToDecimal(Request.Form[txtServiceTariffComp2.UniqueID]);
            entity.TariffComp3 = Convert.ToDecimal(Request.Form[txtServiceTariffComp3.UniqueID]);
            entity.DiscountComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscComp1.UniqueID]);
            entity.DiscountComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscComp2.UniqueID]);
            entity.DiscountComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscComp3.UniqueID]);
            entity.CITODiscount = Convert.ToDecimal(Request.Form[txtServiceCITODisc.UniqueID]);
            entity.CostAmount = Convert.ToDecimal(Request.Form[txtServiceCostAmount.UniqueID]);
        }

        private bool OnBeforeSaveRecordService(ref string errMessage)
        {
            return true;
            //int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            //if (transactionID > 0)
            //{
            //    string filterExpression = string.Format("TransactionID = {0} AND ItemID = {1} AND ParamedicID = {2} AND IsDeleted = 0", transactionID, hdnServiceItemID.Value, hdnServicePhysicianID.Value);
            //    if (hdnServiceTransactionDtID.Value.ToString() != "")
            //        filterExpression += string.Format(" AND ID != {0}", hdnServiceTransactionDtID.Value);
            //    int count = BusinessLayer.GetPatientChargesDtRowCount(filterExpression);
            //    if (count > 0)
            //    {
            //        errMessage = string.Format("Sudah Terdapat pelayanan {0} dengan dokter {1}", Request.Form[txtServiceItemName.UniqueID], Request.Form[txtServicePhysicianName.UniqueID]);
            //        return false;
            //    }

            //    //PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHd(transactionID);
            //    //if (entityHd.GCTransactionStatus != Constant.TransactionStatus.OPEN && entityHd.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            //    //{
            //    //    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
            //    //    return false;
            //    //}
            //    return true;
            //}
            //return true;
        }

        private bool OnSaveAddRecordService(ref string errMessage, ref int transactionID)
        {
            return true;
            //bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            //TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            //try
            //{
            //    DetailPage.SaveTransactionHeader(ctx, ref transactionID);
            //    PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", transactionID), ctx).FirstOrDefault();
            //    if (transactionID > 0)
            //    {
            //        if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            //        {
            //            if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            //            {
            //                PatientChargesDt entityDt = new PatientChargesDt();
            //                ServiceControlToEntity(entityDt);
            //                entityDt.ItemID = Convert.ToInt32(hdnServiceItemID.Value);
            //                if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
            //                {
            //                    TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
            //                    if (entityTestOrderDt != null)
            //                    {
            //                        if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            //                        {
            //                            entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
            //                        }
            //                        else entityTestOrderDt.BusinessPartnerID = null;
            //                        entityTestOrderDtDao.Update(entityTestOrderDt);
            //                    }
            //                }
            //                entityDt.BaseTariff = Convert.ToDecimal(hdnServiceBaseTariff.Value);
            //                entityDt.BaseComp1 = Convert.ToDecimal(hdnServiceBasePriceComp1.Value);
            //                entityDt.BaseComp2 = Convert.ToDecimal(hdnServiceBasePriceComp2.Value);
            //                entityDt.BaseComp3 = Convert.ToDecimal(hdnServiceBasePriceComp3.Value);
            //                entityDt.GCBaseUnit = entityDt.GCItemUnit = hdnServiceItemUnit.Value;
            //                entityDt.IsCITOInPercentage = (hdnServiceIsCITOInPercentage.Value == "1");
            //                entityDt.IsComplicationInPercentage = (hdnServiceIsComplicationInPercentage.Value == "1");
            //                entityDt.BaseCITOAmount = Convert.ToDecimal(hdnServiceBaseCITOAmount.Value);
            //                entityDt.BaseComplicationAmount = Convert.ToDecimal(hdnServiceBaseComplicationAmount.Value);
            //                entityDt.TransactionID = transactionID;
            //                entityDt.CreatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            //                entityDt.GCTransactionDetailStatus = entityPatientChargesHd.GCTransactionStatus;
            //                entityDtDao.Insert(entityDt);
            //            }
            //            else
            //            {
            //                errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
            //                result = false;
            //            }
            //        }
            //        else
            //        {
            //            if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            //            {
            //                PatientChargesDt entityDt = new PatientChargesDt();
            //                ServiceControlToEntity(entityDt);
            //                entityDt.ItemID = Convert.ToInt32(hdnServiceItemID.Value);
            //                if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
            //                {
            //                    TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
            //                    if (entityTestOrderDt != null)
            //                    {
            //                        if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            //                        {
            //                            entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
            //                        }
            //                        else entityTestOrderDt.BusinessPartnerID = null;
            //                        entityTestOrderDtDao.Update(entityTestOrderDt);
            //                    }
            //                }
            //                entityDt.BaseTariff = Convert.ToDecimal(hdnServiceBaseTariff.Value);
            //                entityDt.BaseComp1 = Convert.ToDecimal(hdnServiceBasePriceComp1.Value);
            //                entityDt.BaseComp2 = Convert.ToDecimal(hdnServiceBasePriceComp2.Value);
            //                entityDt.BaseComp3 = Convert.ToDecimal(hdnServiceBasePriceComp3.Value);
            //                entityDt.GCBaseUnit = entityDt.GCItemUnit = hdnServiceItemUnit.Value;
            //                entityDt.IsCITOInPercentage = (hdnServiceIsCITOInPercentage.Value == "1");
            //                entityDt.IsComplicationInPercentage = (hdnServiceIsComplicationInPercentage.Value == "1");
            //                entityDt.BaseCITOAmount = Convert.ToDecimal(hdnServiceBaseCITOAmount.Value);
            //                entityDt.BaseComplicationAmount = Convert.ToDecimal(hdnServiceBaseComplicationAmount.Value);
            //                entityDt.TransactionID = transactionID;
            //                entityDt.CreatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            //                entityDt.GCTransactionDetailStatus = entityPatientChargesHd.GCTransactionStatus;
            //                entityDtDao.Insert(entityDt);
            //            }
            //            else
            //            {
            //                errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
            //                result = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        PatientChargesDt entityDt = new PatientChargesDt();
            //        ServiceControlToEntity(entityDt);
            //        entityDt.ItemID = Convert.ToInt32(hdnServiceItemID.Value);
            //        if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
            //        {
            //            TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
            //            if (entityTestOrderDt != null)
            //            {
            //                if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            //                {
            //                    entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
            //                }
            //                else entityTestOrderDt.BusinessPartnerID = null;
            //                entityTestOrderDtDao.Update(entityTestOrderDt);
            //            }
            //        }
            //        entityDt.BaseTariff = Convert.ToDecimal(hdnServiceBaseTariff.Value);
            //        entityDt.BaseComp1 = Convert.ToDecimal(hdnServiceBasePriceComp1.Value);
            //        entityDt.BaseComp2 = Convert.ToDecimal(hdnServiceBasePriceComp2.Value);
            //        entityDt.BaseComp3 = Convert.ToDecimal(hdnServiceBasePriceComp3.Value);
            //        entityDt.GCBaseUnit = entityDt.GCItemUnit = hdnServiceItemUnit.Value;
            //        entityDt.IsCITOInPercentage = (hdnServiceIsCITOInPercentage.Value == "1");
            //        entityDt.IsComplicationInPercentage = (hdnServiceIsComplicationInPercentage.Value == "1");
            //        entityDt.BaseCITOAmount = Convert.ToDecimal(hdnServiceBaseCITOAmount.Value);
            //        entityDt.BaseComplicationAmount = Convert.ToDecimal(hdnServiceBaseComplicationAmount.Value);
            //        entityDt.TransactionID = transactionID;
            //        entityDt.GCTransactionDetailStatus = entityPatientChargesHd.GCTransactionStatus;
            //        entityDt.CreatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            //        entityDtDao.Insert(entityDt);
            //    }
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    errMessage = ex.Message;
            //    Helper.InsertErrorLog(ex);
            //    ctx.RollBackTransaction();
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            //return result;
        }

        private bool OnSaveEditRecordService(ref string errMessage)
        {
            return true;
            //bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            //TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            //int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            //try
            //{
            //    PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
            //    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            //    {
            //        if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            //        {
            //            PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnServiceTransactionDtID.Value));
            //            if (!entityDt.IsDeleted)
            //            {
            //                ServiceControlToEntity(entityDt);
            //                if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
            //                {
            //                    TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
            //                    if (entityTestOrderDt != null)
            //                    {
            //                        if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            //                        {
            //                            entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
            //                        }
            //                        else entityTestOrderDt.BusinessPartnerID = null;
            //                        entityTestOrderDtDao.Update(entityTestOrderDt);
            //                    }
            //                }
            //                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            //                entityDtDao.Update(entityDt);
            //            }
            //        }
            //        else
            //        {
            //            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
            //            result = false;
            //        }
            //    }
            //    else
            //    {
            //        if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            //        {
            //            PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnServiceTransactionDtID.Value));
            //            if (!entityDt.IsDeleted)
            //            {
            //                ServiceControlToEntity(entityDt);
            //                if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
            //                {
            //                    TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
            //                    if (entityTestOrderDt != null)
            //                    {
            //                        if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            //                        {
            //                            entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
            //                        }
            //                        else entityTestOrderDt.BusinessPartnerID = null;
            //                        entityTestOrderDtDao.Update(entityTestOrderDt);
            //                    }
            //                }
            //                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            //                entityDtDao.Update(entityDt);
            //            }
            //        }
            //        else
            //        {
            //            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
            //            result = false;
            //        }
            //    }
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    errMessage = ex.Message;
            //    Helper.InsertErrorLog(ex);
            //    ctx.RollBackTransaction();
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            //return result;
        }
        #endregion
    }
}