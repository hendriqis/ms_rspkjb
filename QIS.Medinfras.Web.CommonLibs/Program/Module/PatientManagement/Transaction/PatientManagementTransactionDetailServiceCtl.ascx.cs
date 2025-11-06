using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class PatientManagementTransactionDetailServiceCtl : BaseUserControlCtl
    {
        protected bool IsShowParamedicTeam = false;
        protected bool IsShowSwitchIcon = false;
        List<SettingParameter> lstSettingParameter = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        protected string GetMainParamedicRole()
        {
            return Constant.ParamedicRole.PELAKSANA;
        }

        protected string SA0198()
        {
            return AppSession.SA0198 ? "1" : "0";
        }

        protected string MRN()
        {
            return AppSession.RegisteredPatient.MRN.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Registration reg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                hdnBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";
                hdnBusinessPartnerIDServiceCtl.Value = reg.BusinessPartnerID.ToString();

                hdnVisitIDCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();
                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID.ToString();

                List<vSettingParameterDt> lstSP = BusinessLayer.GetvSettingParameterDtList(string.Format(
                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //1
                                                            Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //2
                                                            Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS, //3
                                                            Constant.SettingParameter.IS_PEMERIKSAAN_RADIOLOGI_HANYA_BPJS, //4
                                                            Constant.SettingParameter.MD_PEMERIKSAAN_PENUNJANG_MEDIS_HANYA_BPJS, //5
                                                            Constant.SettingParameter.SA0199, //6
                                                            Constant.SettingParameter.SA0205, //7
                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //8
                                                            Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT, //9
                                                            Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE //10
                                                        ));

                hdnImagingServiceUnitID.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                hdnIsOnlyBPJSLab.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS).ParameterValue;
                hdnIsOnlyBPJSRad.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PEMERIKSAAN_RADIOLOGI_HANYA_BPJS).ParameterValue;
                hdnIsOnlyBPJSOth.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_PEMERIKSAAN_PENUNJANG_MEDIS_HANYA_BPJS).ParameterValue;
                hdnDefaultExpiredDateSetvar.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0199).ParameterValue;
                hdnDefaultExpiredDateAgeUnit.Value = lstSP.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0205).ParameterValue;

                hdnIsEndingAmountRoundingTo1.Value = lstSP.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
                hdnIsUsingValidateDigitDecimal.Value = lstSP.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT).FirstOrDefault().ParameterValue;

                lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format(
                                                        "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}')",
                                                        Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, //0
                                                        Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, //1
                                                        Constant.SettingParameter.TARIFF_COMPONENT3_TEXT, //2
                                                        Constant.SettingParameter.PRESCRIPTION_RETURN_ITEM, //3
                                                        Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID //4
                                                    ));

                hdnLabHealthcareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue)).FirstOrDefault().HealthcareServiceUnitID.ToString();

                hdnTariffComp1Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
                hdnTariffComp2Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
                hdnTariffComp3Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;
                hdnPrescriptionReturnItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_RETURN_ITEM).ParameterValue;

                int ageUnitExpiredDate = 0;
                switch (hdnDefaultExpiredDateAgeUnit.Value)
                {
                    case Constant.AgeUnit.HARI:
                        ageUnitExpiredDate = 1;
                        break;
                    case Constant.AgeUnit.MINGGU:
                        ageUnitExpiredDate = 7;
                        break;
                    case Constant.AgeUnit.BULAN:
                        ageUnitExpiredDate = 30;
                        break;
                    case Constant.AgeUnit.TAHUN:
                        ageUnitExpiredDate = 365;
                        break;
                    default:
                        ageUnitExpiredDate = 7;
                        break;
                }

                if (hdnDefaultExpiredDateSetvar.Value != "0")
                {
                    int totalDays = Convert.ToInt32(hdnDefaultExpiredDateSetvar.Value) * ageUnitExpiredDate;
                    txtExpiredDate.Text = hdnDefaultExpiredDate.Value = DateTime.Now.AddDays(totalDays).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    txtExpiredDate.Text = hdnDefaultExpiredDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                string filterHSULB = string.Format("HealthcareServiceUnitID = {0}", DetailPage.GetHealthcareServiceUnitID());
                vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(filterHSULB).FirstOrDefault();
                if (vsu.IsLaboratoryUnit)
                {
                    hdnIsLaboratoryUnit.Value = "1";
                }
                else
                {
                    hdnIsLaboratoryUnit.Value = "0";
                }

                Helper.SetControlEntrySetting(txtServiceCITO, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceDiscount, new ControlEntrySetting(false, false, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceComplication, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServicePatient, new ControlEntrySetting(true, true, true), "mpTrxService");
                if (hdnBusinessPartnerIDServiceCtl.Value == "1")
                {
                    Helper.SetControlEntrySetting(txtServicePayer, new ControlEntrySetting(false, false, true), "mpTrxService");
                }
                else
                {
                    Helper.SetControlEntrySetting(txtServicePayer, new ControlEntrySetting(true, true, true), "mpTrxService");
                }
                Helper.SetControlEntrySetting(txtServiceQty, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceTariff, new ControlEntrySetting(false, false, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceTotal, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceUnitTariff, new ControlEntrySetting(false, false, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceItemCode, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServicePhysicianCode, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(cboServiceChargeClassID, new ControlEntrySetting(true, true, true), "mpTrxService");
            }
        }

        public void InitializeTransactionControl(bool flagHaveCharges, string isServiceUnitMultiVisitScheduleCtl = "0")
        {
            int healthcareServiceUnitID = DetailPage.GetHealthcareServiceUnitID();

            if (flagHaveCharges)
            {
                BindGridService(isServiceUnitMultiVisitScheduleCtl);
                if (healthcareServiceUnitID > 0)
                    hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", DetailPage.GetHealthcareServiceUnitID());
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
                        hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", DetailPage.GetHealthcareServiceUnitID());
                    else
                        hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
                }
            }

            if (AppSession.SA0198)
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT || AppSession.RegisteredPatient.DepartmentID == Constant.Facility.DIAGNOSTIC)
                {
                    if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() != hdnLaboratoryServiceUnitID.Value && AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() != hdnImagingServiceUnitID.Value)
                    {
                        hdnServiceItemFilterExpression.Value += " AND (IsPackageBalanceItem = 1 OR IsPackageBalanceItem = 0 OR IsPackageBalanceItem IS NULL)";
                    }
                    else
                    {
                        hdnServiceItemFilterExpression.Value += " AND (IsPackageBalanceItem = 0 OR IsPackageBalanceItem IS NULL)";
                    }
                }
            }
            else
            {
                hdnServiceItemFilterExpression.Value += " AND (IsPackageBalanceItem = 0 OR IsPackageBalanceItem IS NULL)";
            }
        }

        public void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField(cboServiceChargeClassID, lstClassCare, "ClassName", "ClassID");
            Methods.SetComboBoxField(cboEditAIOChargeClass, lstClassCare, "ClassName", "ClassID");

            string filterSC = string.Format("IsDeleted = 0 AND IsActive = 1 AND ParentID IN ('{0}')", Constant.StandardCode.DISCOUNT_REASON_CHARGESDT);
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterSC);
            Methods.SetComboBoxField(cboGCDiscountReasonChargesDt, lstSC.Where(a => a.ParentID == Constant.StandardCode.DISCOUNT_REASON_CHARGESDT).ToList(), "StandardCodeName", "StandardCodeID");
        }

        public void OnAddRecord()
        {
            txtServicePhysicianCode.Text = "";
            hdnHealthcareServiceUnitIDServiceCtl.Value = DetailPage.GetHealthcareServiceUnitID().ToString();
            hdnTransactionDateServiceCtl.Value = DetailPage.GetTransactionDate();
            hdnTransactionTime.Value = DetailPage.GetTransactionTime();

            if (DetailPage.GetDepartmentID().ToString() == Constant.Facility.INPATIENT)
            {
                hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
            }
            else
            {
                if (Convert.ToInt32(hdnHealthcareServiceUnitIDServiceCtl.Value) > 0)
                    hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", DetailPage.GetHealthcareServiceUnitID());
                else
                    hdnServiceItemFilterExpression.Value = "HealthcareServiceUnitID = {HealthcareServiceUnitID} AND IsDeleted = 0";
            }

            if (AppSession.SA0198)
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT || AppSession.RegisteredPatient.DepartmentID == Constant.Facility.DIAGNOSTIC)
                {
                    if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() != hdnLaboratoryServiceUnitID.Value && AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() != hdnImagingServiceUnitID.Value)
                    {
                        hdnServiceItemFilterExpression.Value += " AND (IsPackageBalanceItem = 1 OR IsPackageBalanceItem = 0)";
                    }
                    else
                    {
                        hdnServiceItemFilterExpression.Value += " AND IsPackageBalanceItem = 0";
                    }
                }
            }
            else
            {
                hdnServiceItemFilterExpression.Value += " AND IsPackageBalanceItem = 0";
            }

            BindGridService();
            IsEditable = true;
            hdnIsEditable.Value = "1";
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
        private void BindGridService(string isServiceUnitMultiVisitScheduleCtl = "0")
        {
            string GCTransactionStatus = DetailPage.GetGCTransactionStatus();
            hdnHealthcareServiceUnitIDServiceCtl.Value = DetailPage.GetHealthcareServiceUnitID().ToString();

            hdnTransactionDateServiceCtl.Value = DetailPage.GetTransactionDate();
            hdnTransactionTime.Value = DetailPage.GetTransactionTime();

            if (hdnTransactionDateServiceCtl.Value == null || hdnTransactionDateServiceCtl.Value == "")
            {
                hdnTransactionDateServiceCtl.Value = DetailPage.GetTransactionDate2();
                hdnTransactionTime.Value = DetailPage.GetTransactionTime2();
            }

            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN || GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            }
            hdnIsEditable.Value = IsEditable && !entity.IsLockDown ? "1" : "0";
            IsEditable = entity.IsLockDown ? false : IsEditable;

            string filterExpression = "1 = 0";
            hdnServiceTransactionID.Value = DetailPage.GetTransactionHdID();
            if (hdnServiceTransactionID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 ORDER BY ID", hdnServiceTransactionID.Value, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.ItemGroupMaster.MEDICAL_CHECKUP);
            }

            if (DetailPage.GetHealthcareServiceUnitID().ToString() == hdnLabHealthcareServiceUnitID.Value)
                IsShowParamedicTeam = false;
            else
                IsShowParamedicTeam = (DetailPage.GetDepartmentID() == Constant.Facility.DIAGNOSTIC);

            IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;

            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            lvwService.DataSource = lst;
            lvwService.DataBind();

            if (hdnServiceTransactionID.Value != "")
            {
                if (isServiceUnitMultiVisitScheduleCtl == "1")
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitIDServiceCtl.Value)).FirstOrDefault();
                    if (hsu != null)
                    {
                        if (hsu.IsAllowMultiVisitSchedule)
                        {
                            if (lst.Where(w => w.DiagnosticVisitScheduleID > 0).Count() > 0)
                            {
                                divListAddButton.Style.Add("display", "none");
                            }
                            else
                            {
                                divListAddButton.Style.Remove("display");
                            }
                        }
                        else
                        {
                            divListAddButton.Style.Remove("display");
                        }
                    }
                    else
                    {
                        divListAddButton.Style.Remove("display");
                    }
                }
                else
                {
                    divListAddButton.Style.Remove("display");
                }
            }
            else
            {
                divListAddButton.Style.Remove("display");
            }

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnServiceAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnServiceAllTotalPayer.Value = totalPayerAmount.ToString();

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", Convert.ToInt32(DetailPage.GetTransactionHdID()))).FirstOrDefault();
            if (entityHd != null)
            {
                if (entityHd.IsAIOTransaction)
                {
                    hdnIsAIOTransactionServiceCtl.Value = "1";
                }
                else
                {
                    hdnIsAIOTransactionServiceCtl.Value = "0";
                }

                if (entityHd.ConsultVisitItemPackageID != null && entityHd.ConsultVisitItemPackageID != 0)
                {
                    hdnIsChargesGenerateMCUServiceCtl.Value = "1";
                }
                else
                {
                    hdnIsChargesGenerateMCUServiceCtl.Value = "0";
                }
            }
            else
            {
                hdnIsAIOTransactionServiceCtl.Value = "0";
                hdnIsChargesGenerateMCUServiceCtl.Value = "0";
            }

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
                transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
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
                        transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                        result += string.Format("fail|{0}", errMessage);
                    }
                    DetailPage.SetTransactionHdID(transactionID.ToString());
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
            if (param[0] == "editAIO")
            {
                transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (hdnServiceTransactionDtID.Value.ToString() != "")
                {
                    if (OnBeforeSaveRecordService(ref errMessage))
                    {
                        if (OnSaveEditRecordServiceAIO(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            BindGridService();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private bool OnSwitchChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnDeleteChargesDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            DiagnosticVisitScheduleDao diagVisitSchDao = new DiagnosticVisitScheduleDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);

            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);

                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dihapus lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dihapus lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            entity.GCDeleteReason = gcDeleteReason;
                            entity.DeleteReason = reason;
                            entity.DeleteDate = DateTime.Now;
                            entity.DeleteBy = AppSession.UserLogin.UserID;
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityDtDao.Update(entity);

                            if (entityHd.TestOrderID != null && entityHd.TestOrderID != 0)
                            {
                                TestOrderHd entityOrderHd = orderHdDao.Get(Convert.ToInt32(entityHd.TestOrderID));

                                string filterTestOrderDt = string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityOrderHd.TestOrderID, entity.ItemID);
                                List<TestOrderDt> orderDtList = BusinessLayer.GetTestOrderDtList(filterTestOrderDt, ctx);
                                foreach (TestOrderDt entityOrderDt in orderDtList)
                                {
                                    if ((!entityOrderDt.IsCreatedFromOrder) || (entityOrderDt.IsCreatedFromOrder && entityOrderHd.GCOrderStatus == Constant.OrderStatus.COMPLETED))
                                    {
                                        entityOrderDt.GCTestOrderStatus = Constant.OrderStatus.CANCELLED;
                                        entityOrderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        orderDtDao.Update(entityOrderDt);
                                    }
                                }
                            }

                            string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", entity.ID);
                            List<PatientChargesDtPackage> entityPackageLst = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                            foreach (PatientChargesDtPackage entityPackage in entityPackageLst)
                            {
                                entityPackage.IsDeleted = true;
                                entityPackage.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtPackageDao.Update(entityPackage);
                            }

                            if (entity.DiagnosticVisitScheduleID > 0)
                            {
                                DiagnosticVisitSchedule diagVisitSch = diagVisitSchDao.Get(Convert.ToInt32(entity.DiagnosticVisitScheduleID));
                                if (diagVisitSch != null)
                                {
                                    diagVisitSch.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.OPEN;
                                    diagVisitSch.ScheduledDate = new DateTime(1900, 1, 1);
                                    diagVisitSch.RealDate = new DateTime(1900, 1, 1);
                                    diagVisitSch.AppointmentID = null;
                                    diagVisitSch.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    diagVisitSch.LastUpdatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    diagVisitSchDao.Update(diagVisitSch);
                                }
                            }
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        public void OnVoidAllChargesDt(IDbContext ctx, int transactionHdID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemBalanceDao itemBalanceDao = new ItemBalanceDao(ctx);
            DiagnosticVisitScheduleDao diagVisitSchDao = new DiagnosticVisitScheduleDao(ctx);

            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            bool isAllowSaveDt = false;
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isAllowSaveDt = true;
                }
            }
            else
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    isAllowSaveDt = true;
                }
            }

            if (isAllowSaveDt)
            {
                List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{5}'", transactionHdID, Constant.ItemType.PELAYANAN, Constant.ItemType.LABORATORIUM, Constant.ItemType.RADIOLOGI, Constant.ItemType.PENUNJANG_MEDIS, Constant.TransactionStatus.VOID), ctx);
                foreach (vPatientChargesDt1 patientChargesDt in lstPatientChargesDt)
                {
                    PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                    entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                    entity.IsApproved = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityDtDao.Update(entity);

                    string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", entity.ID);
                    List<PatientChargesDtPackage> entityPackageLst = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                    foreach (PatientChargesDtPackage entityPackage in entityPackageLst)
                    {
                        entityPackage.IsDeleted = true;
                        entityPackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtPackageDao.Update(entityPackage);
                    }

                    if (entity.DiagnosticVisitScheduleID > 0)
                    {
                        DiagnosticVisitSchedule diagVisitSch = BusinessLayer.GetDiagnosticVisitSchedule(Convert.ToInt32(entity.DiagnosticVisitScheduleID));
                        if (diagVisitSch != null)
                        {
                            diagVisitSch.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.OPEN;
                            diagVisitSch.ScheduledDate = new DateTime(1900, 1, 1);
                            diagVisitSch.RealDate = new DateTime(1900, 1, 1);
                            diagVisitSch.AppointmentID = null;
                            diagVisitSch.LastUpdatedBy = AppSession.UserLogin.UserID;
                            diagVisitSch.LastUpdatedDate = DateTime.Now;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            diagVisitSchDao.Update(diagVisitSch);
                        }
                    }
                }
            }
        }

        private void ServiceControlToEntity(PatientChargesDt entity)
        {
            entity.RevenueSharingID = Convert.ToInt32(hdnServiceRevenueSharingID.Value);
            if (entity.RevenueSharingID == 0)
            {
                entity.RevenueSharingID = null;
            }

            entity.ParamedicID = Convert.ToInt32(hdnServicePhysicianID.Value);

            if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
            }
            else
            {
                entity.BusinessPartnerID = null;
            }

            entity.IsSubContractItem = hdnIsSubContractItem.Value == "1" ? true : false;

            if (hdnIsAIOTransactionServiceCtl.Value == "0")
            {
                entity.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);
                entity.UsedQuantity = entity.BaseQuantity = entity.ChargedQuantity = Convert.ToDecimal(txtServiceQty.Text);
            }
            else
            {
                entity.ChargeClassID = Convert.ToInt32(cboEditAIOChargeClass.Value);
                entity.UsedQuantity = entity.BaseQuantity = entity.ChargedQuantity = Convert.ToDecimal(txtAIOQty.Text);
            }

            entity.IsVariable = chkServiceIsVariable.Checked;
            if (entity.IsVariable)
            {
                entity.ItemName = txtItemNameVariable.Text;
            }

            entity.IsUnbilledItem = chkServiceIsUnbilledItem.Checked;

            entity.IsCITO = chkServiceIsCITO.Checked;
            entity.IsCITOInPercentage = (hdnServiceIsCITOInPercentage.Value == "1");
            entity.BaseCITOAmount = Convert.ToDecimal(hdnServiceBaseCITOAmount.Value);
            if (entity.IsCITO)
            {
                entity.CITOAmount = Convert.ToDecimal(Request.Form[txtServiceCITO.UniqueID]);
                entity.CITODiscount = Convert.ToDecimal(Request.Form[txtServiceCITODisc.UniqueID]);
            }
            else
            {
                entity.CITOAmount = 0;
            }

            entity.IsComplication = chkServiceIsComplication.Checked;
            if (entity.IsComplication)
            {
                entity.IsComplicationInPercentage = (hdnServiceIsComplicationInPercentage.Value == "1");
                entity.BaseComplicationAmount = Convert.ToDecimal(hdnServiceBaseComplicationAmount.Value);
                entity.ComplicationAmount = Convert.ToDecimal(Request.Form[txtServiceComplication.UniqueID]);
            }

            entity.PatientAmount = Convert.ToDecimal(Request.Form[txtServicePatient.UniqueID]);
            entity.PayerAmount = Convert.ToDecimal(Request.Form[txtServicePayer.UniqueID]);
            entity.LineAmount = Convert.ToDecimal(Request.Form[txtServiceTotal.UniqueID]);

            decimal oPatientAmount = entity.PatientAmount;
            decimal oPayerAmount = entity.PayerAmount;
            decimal oLineAmount = entity.LineAmount;

            if (hdnIsEndingAmountRoundingTo1.Value == "1")
            {
                decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                {
                    oPatientAmount = Math.Floor(oPatientAmount);
                }
                else
                {
                    oPatientAmount = Math.Ceiling(oPatientAmount);
                }

                decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                {
                    oPayerAmount = Math.Floor(oPayerAmount);
                }
                else
                {
                    oPayerAmount = Math.Ceiling(oPayerAmount);
                }

                oLineAmount = oPatientAmount + oPayerAmount;
            }

            entity.PatientAmount = oPatientAmount;
            entity.PayerAmount = oPayerAmount;
            entity.LineAmount = oLineAmount;

            entity.Tariff = Convert.ToDecimal(Request.Form[txtServiceUnitTariff.UniqueID]);
            entity.TariffComp1 = Convert.ToDecimal(Request.Form[txtServiceTariffComp1.UniqueID]);
            entity.TariffComp2 = Convert.ToDecimal(Request.Form[txtServiceTariffComp2.UniqueID]);
            entity.TariffComp3 = Convert.ToDecimal(Request.Form[txtServiceTariffComp3.UniqueID]);

            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtServiceDiscount.UniqueID]);
            entity.DiscountComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscComp1.UniqueID]);
            entity.DiscountComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscComp2.UniqueID]);
            entity.DiscountComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscComp3.UniqueID]);

            entity.IsDiscount = entity.DiscountAmount != 0 ? true : false;

            if (entity.IsDiscount)
            {
                if (cboGCDiscountReasonChargesDt.Value != null)
                {
                    entity.GCDiscountReason = cboGCDiscountReasonChargesDt.Value.ToString();

                    if (entity.GCDiscountReason == Constant.DiscountReasonChargesDt.LAIN_LAIN)
                    {
                        entity.DiscountReason = txtDiscountReasonChargesDt.Text;
                    }
                    else
                    {
                        entity.DiscountReason = null;
                    }
                }
                else
                {
                    entity.GCDiscountReason = null;
                    entity.DiscountReason = null;
                }
            }

            decimal discPercentComp1 = 0;
            decimal discPercentComp2 = 0;
            decimal discPercentComp3 = 0;

            if (Request.Form[txtServiceDiscPercentComp1.UniqueID] != "")
            {
                discPercentComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp1.UniqueID]);
            }
            if (Request.Form[txtServiceDiscPercentComp2.UniqueID] != "")
            {
                discPercentComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp2.UniqueID]);
            }
            if (Request.Form[txtServiceDiscPercentComp3.UniqueID] != "")
            {
                discPercentComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp3.UniqueID]);
            }

            if (discPercentComp1 > 0)
            {
                entity.IsDiscountInPercentageComp1 = true;
                entity.DiscountPercentageComp1 = discPercentComp1;
            }
            else
            {
                entity.IsDiscountInPercentageComp1 = false;
                entity.DiscountPercentageComp1 = 0;
            }

            if (discPercentComp2 > 0)
            {
                entity.IsDiscountInPercentageComp2 = true;
                entity.DiscountPercentageComp2 = discPercentComp2;
            }
            else
            {
                entity.IsDiscountInPercentageComp2 = false;
                entity.DiscountPercentageComp2 = 0;
            }

            if (discPercentComp3 > 0)
            {
                entity.IsDiscountInPercentageComp3 = true;
                entity.DiscountPercentageComp3 = discPercentComp3;
            }
            else
            {
                entity.IsDiscountInPercentageComp3 = false;
                entity.DiscountPercentageComp3 = 0;
            }

            entity.CostAmount = Convert.ToDecimal(Request.Form[txtServiceCostAmount.UniqueID]);
        }

        private bool OnBeforeSaveRecordService(ref string errMessage)
        {
            int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            if (transactionID > 0)
            {
                string filterExpression = string.Format("TransactionID = {0} AND ItemID = {1} AND ParamedicID = {2} AND IsDeleted = 0", transactionID, hdnServiceItemID.Value, hdnServicePhysicianID.Value);
                if (hdnServiceTransactionDtID.Value.ToString() != "")
                {
                    filterExpression += string.Format(" AND ID != {0}", hdnServiceTransactionDtID.Value);
                }
                int count = BusinessLayer.GetPatientChargesDtRowCountByFieldName(filterExpression);
                if (count > 0)
                {
                    errMessage = string.Format("Sudah terdapat pelayanan {0} dengan dokter {1}", Request.Form[txtServiceItemName.UniqueID], Request.Form[txtServicePhysicianName.UniqueID]);
                    return false;
                }

                return true;
            }
            return true;
        }

        private bool OnSaveAddRecordService(ref string errMessage, ref int transactionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            PatientChargesDtInfoDao entityInfoDao = new PatientChargesDtInfoDao(ctx);
            try
            {
                DetailPage.SaveTransactionHeader(ctx, ref transactionID);
                PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", transactionID), ctx).FirstOrDefault();
                bool isAllowSaveDt = true;
                if (transactionID > 0)
                {
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityPatientChargesHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityPatientChargesHd.TransactionNo);
                            result = false;
                        }
                    }
                }

                if (isAllowSaveDt)
                {
                    PatientChargesDt entityDt = new PatientChargesDt();
                    ServiceControlToEntity(entityDt);
                    entityDt.ItemID = Convert.ToInt32(hdnServiceItemID.Value);

                    ItemService its = itemServiceDao.Get(entityDt.ItemID);

                    entityDt.ItemName = txtItemNameVariable.Text;
                    if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
                    {
                        TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
                        if (entityTestOrderDt != null)
                        {
                            if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
                            {
                                entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
                            }
                            else entityTestOrderDt.BusinessPartnerID = null;
                            entityTestOrderDtDao.Update(entityTestOrderDt);
                        }
                    }
                    entityDt.BaseTariff = Convert.ToDecimal(hdnServiceBaseTariff.Value);
                    entityDt.BaseComp1 = Convert.ToDecimal(hdnServiceBasePriceComp1.Value);
                    entityDt.BaseComp2 = Convert.ToDecimal(hdnServiceBasePriceComp2.Value);
                    entityDt.BaseComp3 = Convert.ToDecimal(hdnServiceBasePriceComp3.Value);
                    entityDt.GCBaseUnit = entityDt.GCItemUnit = hdnServiceItemUnit.Value;
                    entityDt.TransactionID = transactionID;
                    entityDt.GCTransactionDetailStatus = entityPatientChargesHd.GCTransactionStatus;
                    //entityDt.CreatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID; -> comment by RN - 20181119
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    int oChargesDtID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                    PatientChargesDtInfo info = entityInfoDao.Get(oChargesDtID);
                    //info.Remarks = txtRemarks.Text;
                    info.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (!string.IsNullOrEmpty(hdnIsPackageBalanceItem.Value) && Convert.ToBoolean(hdnIsPackageBalanceItem.Value))
                    {
                        info.IsPackageBalance = true;
                        info.IsFirstPackageBalance = true;
                        info.PackageExpiredDate = Helper.GetDatePickerValue(txtExpiredDate);

                        if (!String.IsNullOrEmpty(txtPackageQtyTaken.Text))
                        {
                            if (Convert.ToDecimal(txtPackageQtyTaken.Text) > 0)
                            {
                                info.TakenNumber = 1;
                                info.PackageBalanceQtyTaken = Convert.ToDecimal(txtPackageQtyTaken.Text);
                            }
                        }
                    }

                    //#region alasan diskon
                    //if (chkServiceIsDiscount.Checked)
                    //{
                    //    if (cboDiscReason.Value != null)
                    //    {
                    //        info.GCDiscountReason = cboDiscReason.Value.ToString();
                    //        if (cboDiscReason.Value.ToString() == Constant.DiscountReason.LAIN_LAIN)
                    //        {
                    //            info.OtherDiscountReason = txtOtherReason.Text;
                    //        }
                    //    }

                    //}
                    //#endregion

                    //#region alat
                    //if (!string.IsNullOrEmpty(hdnFixedAssetID.Value) || hdnFixedAssetID.Value != "0")
                    //{
                    //    info.FAItemID = Convert.ToInt32(hdnFixedAssetID.Value);
                    //}
                    //else {
                    //    info.FAItemID = null;
                    //}
                    //#endregion

                    entityInfoDao.Update(info);

                    string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", entityDt.ParamedicID, DateTime.Now);
                    List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
                    foreach (ParamedicMasterTeam pmt in pmtList)
                    {
                        PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                        dtparamedic.ID = oChargesDtID;
                        dtparamedic.ItemID = entityDt.ItemID;
                        dtparamedic.ParamedicID = pmt.ParamedicID;
                        dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                        dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtParamedicDao.Insert(dtparamedic);
                    }

                    int countInventoryItemDetail = 0;

                    decimal oAccumulatedDiscountAmount = 0;
                    decimal oAccumulatedDiscountAmountComp1 = 0;
                    decimal oAccumulatedDiscountAmountComp2 = 0;
                    decimal oAccumulatedDiscountAmountComp3 = 0;

                    bool isChargePackageDt = true;
                    //if (Convert.ToBoolean(hdnIsPackageBalanceItem.Value) && Convert.ToDecimal(txtPackageQtyTaken.Text) <= 0)
                    //{
                    //    isChargePackageDt = false;
                    //}
                    //else if (Convert.ToBoolean(hdnIsPackageBalanceItem.Value) && Convert.ToDecimal(txtPackageQtyTaken.Text) > 0)
                    //{
                    //    qtyCharges = Convert.ToDecimal(txtPackageQtyTaken.Text);
                    //}

                    List<PatientChargesDtPackage> lstDtPackage = new List<PatientChargesDtPackage>();
                    string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.ItemID);
                    List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);

                    foreach (vItemServiceDt isd in isdList)
                    {
                        decimal qtyCharges = isd.IsPackageBalanceItem ? !string.IsNullOrEmpty(txtPackageQtyTaken.Text) ? Convert.ToDecimal(txtPackageQtyTaken.Text) : 1 : 1;
                        if (isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS || isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                        {
                            countInventoryItemDetail += 1;
                        }

                        PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                        dtpackage.PatientChargesDtID = oChargesDtID;
                        dtpackage.ItemID = isd.DetailItemID;
                        dtpackage.ParamedicID = entityDt.ParamedicID;

                        int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, entityDt.ParamedicID, entityDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitIDCtl.Value), entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, entityPatientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                        if (revID != 0 && revID != null)
                        {
                            dtpackage.RevenueSharingID = revID;
                        }
                        else
                        {
                            dtpackage.RevenueSharingID = null;
                        }

                        dtpackage.ChargedQuantity = (isd.Quantity * entityDt.ChargedQuantity) * qtyCharges;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        int itemType = isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                        GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, entityPatientChargesHd.VisitID, entityDt.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

                        decimal basePrice = 0;
                        decimal basePriceComp1 = 0;
                        decimal basePriceComp2 = 0;
                        decimal basePriceComp3 = 0;
                        decimal price = 0;
                        decimal priceComp1 = 0;
                        decimal priceComp2 = 0;
                        decimal priceComp3 = 0;
                        bool isDiscountUsedComp = false;
                        decimal discountAmount = 0;
                        decimal discountAmountComp1 = 0;
                        decimal discountAmountComp2 = 0;
                        decimal discountAmountComp3 = 0;
                        decimal coverageAmount = 0;
                        bool isDiscountInPercentage = false;
                        bool isDiscountInPercentageComp1 = false;
                        bool isDiscountInPercentageComp2 = false;
                        bool isDiscountInPercentageComp3 = false;
                        bool isCoverageInPercentage = false;
                        decimal costAmount = 0;
                        decimal grossLineAmount = 0;

                        basePrice = tariff.BasePrice;
                        basePriceComp1 = tariff.BasePriceComp1;
                        basePriceComp2 = tariff.BasePriceComp2;
                        basePriceComp3 = tariff.BasePriceComp3;
                        price = tariff.Price;
                        priceComp1 = tariff.PriceComp1;
                        priceComp2 = tariff.PriceComp2;
                        priceComp3 = tariff.PriceComp3;
                        isDiscountUsedComp = tariff.IsDiscountUsedComp;
                        discountAmount = tariff.DiscountAmount;
                        discountAmountComp1 = tariff.DiscountAmountComp1;
                        discountAmountComp2 = tariff.DiscountAmountComp2;
                        discountAmountComp3 = tariff.DiscountAmountComp3;
                        coverageAmount = tariff.CoverageAmount;
                        isDiscountInPercentage = tariff.IsDiscountInPercentage;
                        isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                        isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                        isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                        isCoverageInPercentage = tariff.IsCoverageInPercentage;
                        costAmount = tariff.CostAmount;
                        grossLineAmount = dtpackage.ChargedQuantity * price;

                        dtpackage.BaseTariff = tariff.BasePrice;
                        dtpackage.BaseComp1 = tariff.BasePriceComp1;
                        dtpackage.BaseComp2 = tariff.BasePriceComp2;
                        dtpackage.BaseComp3 = tariff.BasePriceComp3;
                        dtpackage.Tariff = tariff.Price;
                        dtpackage.TariffComp1 = tariff.PriceComp1;
                        dtpackage.TariffComp2 = tariff.PriceComp2;
                        dtpackage.TariffComp3 = tariff.PriceComp3;
                        dtpackage.CostAmount = tariff.CostAmount;

                        decimal totalDiscountAmount = 0;
                        decimal totalDiscountAmount1 = 0;
                        decimal totalDiscountAmount2 = 0;
                        decimal totalDiscountAmount3 = 0;

                        if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                        {
                            if (isDiscountUsedComp)
                            {
                                if (priceComp1 > 0)
                                {
                                    if (isDiscountInPercentageComp1)
                                    {
                                        totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                        dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                                    }
                                    else
                                    {
                                        totalDiscountAmount1 = discountAmountComp1;
                                    }
                                }

                                if (priceComp2 > 0)
                                {
                                    if (isDiscountInPercentageComp2)
                                    {
                                        totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                        dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                                    }
                                    else
                                    {
                                        totalDiscountAmount2 = discountAmountComp2;
                                    }
                                }

                                if (priceComp3 > 0)
                                {
                                    if (isDiscountInPercentageComp3)
                                    {
                                        totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                        dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                                    }
                                    else
                                    {
                                        totalDiscountAmount3 = discountAmountComp3;
                                    }
                                }
                            }
                            else
                            {
                                if (priceComp1 > 0)
                                {
                                    totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                    dtpackage.DiscountPercentageComp1 = discountAmount;
                                }

                                if (priceComp2 > 0)
                                {
                                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                    dtpackage.DiscountPercentageComp2 = discountAmount;
                                }

                                if (priceComp3 > 0)
                                {
                                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                    dtpackage.DiscountPercentageComp3 = discountAmount;
                                }
                            }

                            if (dtpackage.DiscountPercentageComp1 > 0)
                            {
                                dtpackage.IsDiscountInPercentageComp1 = true;
                            }

                            if (dtpackage.DiscountPercentageComp2 > 0)
                            {
                                dtpackage.IsDiscountInPercentageComp2 = true;
                            }

                            if (dtpackage.DiscountPercentageComp3 > 0)
                            {
                                dtpackage.IsDiscountInPercentageComp3 = true;
                            }
                        }
                        else
                        {
                            if (isDiscountUsedComp)
                            {
                                if (priceComp1 > 0)
                                    totalDiscountAmount1 = discountAmountComp1;
                                if (priceComp2 > 0)
                                    totalDiscountAmount2 = discountAmountComp2;
                                if (priceComp3 > 0)
                                    totalDiscountAmount3 = discountAmountComp3;
                            }
                            else
                            {
                                if (priceComp1 > 0)
                                    totalDiscountAmount1 = discountAmount;
                                if (priceComp2 > 0)
                                    totalDiscountAmount2 = discountAmount;
                                if (priceComp3 > 0)
                                    totalDiscountAmount3 = discountAmount;
                            }
                        }

                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                        if (grossLineAmount > 0)
                        {
                            if (totalDiscountAmount > grossLineAmount)
                            {
                                totalDiscountAmount = grossLineAmount;
                            }
                        }

                        dtpackage.DiscountAmount = totalDiscountAmount;
                        dtpackage.DiscountComp1 = totalDiscountAmount1;
                        dtpackage.DiscountComp2 = totalDiscountAmount2;
                        dtpackage.DiscountComp3 = totalDiscountAmount3;

                        if (isd.IsPackageItem && isd.IsUsingAccumulatedPrice)
                        {
                            oAccumulatedDiscountAmount += dtpackage.DiscountAmount;
                            oAccumulatedDiscountAmountComp1 += dtpackage.DiscountComp1;
                            oAccumulatedDiscountAmountComp2 += dtpackage.DiscountComp2;
                            oAccumulatedDiscountAmountComp3 += dtpackage.DiscountComp3;
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                        List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                        if (iplan.Count() > 0)
                        {
                            dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                        }
                        else
                        {
                            dtpackage.AveragePrice = 0;
                        }

                        dtpackage.CreatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        //if (isChargePackageDt)
                        //{
                        //    entityDtPackageDao.Insert(dtpackage);
                        //}

                        if (its.IsUsingAccumulatedPrice)
                        {
                            int idPackage = entityDtPackageDao.InsertReturnPrimaryKeyID(dtpackage);

                            dtpackage.ID = idPackage;
                        }
                        lstDtPackage.Add(dtpackage);
                    }


                    if (countInventoryItemDetail > 0)
                    {
                        HealthcareServiceUnit chargesHSU = chargesHSUDao.Get(Convert.ToInt32(hdnHealthcareServiceUnitIDServiceCtl.Value));

                        PatientChargesDt pcdt = entityDtDao.Get(oChargesDtID);
                        pcdt.LocationID = chargesHSU.LocationID;
                        pcdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(pcdt);
                    }

                    if (oAccumulatedDiscountAmount != 0)
                    {
                        PatientChargesDt pcdt = entityDtDao.Get(oChargesDtID);
                        pcdt.IsDiscount = true;
                        pcdt.DiscountAmount = oAccumulatedDiscountAmount;
                        pcdt.DiscountComp1 = oAccumulatedDiscountAmountComp1;
                        pcdt.DiscountComp2 = oAccumulatedDiscountAmountComp2;
                        pcdt.DiscountComp3 = oAccumulatedDiscountAmountComp3;
                        pcdt.DiscountPercentageComp1 = 0;
                        pcdt.DiscountPercentageComp2 = 0;
                        pcdt.DiscountPercentageComp3 = 0;
                        pcdt.IsDiscountInPercentageComp1 = false;
                        pcdt.IsDiscountInPercentageComp2 = false;
                        pcdt.IsDiscountInPercentageComp3 = false;
                        pcdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(pcdt);
                    }

                    if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                    {
                        PatientChargesDt pcdt = entityDtDao.Get(oChargesDtID);

                        decimal BaseTariff = 0;
                        decimal BaseComp1 = 0;
                        decimal BaseComp2 = 0;
                        decimal BaseComp3 = 0;
                        decimal Tariff = 0;
                        decimal TariffComp1 = 0;
                        decimal TariffComp2 = 0;
                        decimal TariffComp3 = 0;
                        decimal DiscountAmount = 0;
                        decimal DiscountComp1 = 0;
                        decimal DiscountComp2 = 0;
                        decimal DiscountComp3 = 0;
                        foreach (PatientChargesDtPackage e in lstDtPackage)
                        {
                            BaseTariff += e.BaseTariff * e.ChargedQuantity;
                            BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                            BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                            BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                            Tariff += e.Tariff * e.ChargedQuantity;
                            TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                            TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                            TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                            DiscountAmount += e.DiscountAmount;
                            DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                            DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                            DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;
                        }

                        pcdt.BaseTariff = BaseTariff / entityDt.ChargedQuantity;
                        pcdt.BaseComp1 = BaseComp1 / entityDt.ChargedQuantity;
                        pcdt.BaseComp2 = BaseComp2 / entityDt.ChargedQuantity;
                        pcdt.BaseComp3 = BaseComp3 / entityDt.ChargedQuantity;
                        pcdt.Tariff = Tariff / entityDt.ChargedQuantity;
                        pcdt.TariffComp1 = TariffComp1 / entityDt.ChargedQuantity;
                        pcdt.TariffComp2 = TariffComp2 / entityDt.ChargedQuantity;
                        pcdt.TariffComp3 = TariffComp3 / entityDt.ChargedQuantity;
                        pcdt.DiscountAmount = DiscountAmount;
                        pcdt.DiscountComp1 = DiscountComp1 / entityDt.ChargedQuantity;
                        pcdt.DiscountComp2 = DiscountComp2 / entityDt.ChargedQuantity;
                        pcdt.DiscountComp3 = DiscountComp3 / entityDt.ChargedQuantity;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitIDCtl.Value), pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                        decimal coverageAmount = 0;
                        bool isCoverageInPercentage = false;
                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff obj = list[0];
                            coverageAmount = obj.CoverageAmount;
                            isCoverageInPercentage = obj.IsCoverageInPercentage;
                        }

                        decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                        decimal totalDiscountAmount = pcdt.DiscountAmount;
                        if (grossLineAmount > 0)
                        {
                            if (totalDiscountAmount > grossLineAmount)
                            {
                                totalDiscountAmount = grossLineAmount;
                            }
                        }

                        decimal total = grossLineAmount - totalDiscountAmount;
                        decimal totalPayer = 0;
                        if (isCoverageInPercentage)
                        {
                            totalPayer = total * coverageAmount / 100;
                        }
                        else
                        {
                            totalPayer = coverageAmount * pcdt.ChargedQuantity;
                        }

                        if (total == 0)
                        {
                            totalPayer = total;
                        }
                        else
                        {
                            if (totalPayer < 0 && totalPayer < total)
                            {
                                totalPayer = total;
                            }
                            else if (totalPayer > 0 & totalPayer > total)
                            {
                                totalPayer = total;
                            }
                        }

                        decimal oPatientAmount = total - totalPayer;
                        decimal oPayerAmount = totalPayer;
                        decimal oLineAmount = total;

                        if (hdnIsEndingAmountRoundingTo1.Value == "1")
                        {
                            decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                            decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                            if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                            {
                                oPatientAmount = Math.Floor(oPatientAmount);
                            }
                            else
                            {
                                oPatientAmount = Math.Ceiling(oPatientAmount);
                            }

                            decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                            decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                            if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                            {
                                oPayerAmount = Math.Floor(oPayerAmount);
                            }
                            else
                            {
                                oPayerAmount = Math.Ceiling(oPayerAmount);
                            }

                            oLineAmount = oPatientAmount + oPayerAmount;
                        }

                        pcdt.PatientAmount = oPatientAmount;
                        pcdt.PayerAmount = oPayerAmount;
                        pcdt.LineAmount = oLineAmount;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(pcdt);
                    }
                    else if (!its.IsUsingAccumulatedPrice && its.IsPackageItem)
                    {
                        foreach (PatientChargesDtPackage e in lstDtPackage)
                        {
                            //ctx.CommandType = CommandType.Text;
                            //ctx.Command.Parameters.Clear();
                            //PatientChargesDtPackage packageDt = entityDtPackageDao.Get(e.ID);
                            PatientChargesDtPackage packageDt = e;
                            if (packageDt.TariffComp1 != 0)
                            {
                                packageDt.TariffComp1 = ((packageDt.BaseComp1 / lstDtPackage.Sum(t => t.BaseTariff)) * entityDt.Tariff);
                            }
                            else
                            {
                                packageDt.TariffComp1 = 0;
                            }

                            if (packageDt.TariffComp2 != 0)
                            {
                                packageDt.TariffComp2 = ((packageDt.BaseComp2 / lstDtPackage.Sum(t => t.BaseTariff)) * entityDt.Tariff);
                            }
                            else
                            {
                                packageDt.TariffComp2 = 0;
                            }
                            if (packageDt.TariffComp3 != 0)
                            {
                                packageDt.TariffComp3 = ((packageDt.BaseComp3 / lstDtPackage.Sum(t => t.BaseTariff)) * entityDt.Tariff);
                            }
                            else
                            {
                                packageDt.TariffComp3 = 0;
                            }

                            packageDt.Tariff = packageDt.TariffComp1 + packageDt.TariffComp2 + packageDt.TariffComp3;

                            if (packageDt.DiscountComp1 != 0)
                            {
                                packageDt.DiscountComp1 = ((packageDt.Tariff / entityDt.Tariff) * entityDt.DiscountComp1);
                            }
                            else
                            {
                                packageDt.DiscountComp1 = 0;
                            }

                            if (packageDt.DiscountComp2 != 0)
                            {
                                packageDt.DiscountComp2 = ((packageDt.Tariff / entityDt.Tariff) * entityDt.DiscountComp2);
                            }
                            else
                            {
                                packageDt.DiscountComp2 = 0;
                            }

                            if (packageDt.DiscountComp3 != 0)
                            {
                                packageDt.DiscountComp3 = ((packageDt.Tariff / entityDt.Tariff) * entityDt.DiscountComp3);
                            }
                            else
                            {
                                packageDt.DiscountComp3 = 0;
                            }

                            packageDt.DiscountAmount = ((packageDt.DiscountComp1 + packageDt.DiscountComp2 + packageDt.DiscountComp3) * packageDt.ChargedQuantity);

                            //packageDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtPackageDao.InsertReturnPrimaryKeyID(packageDt);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnSaveEditRecordService(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            VisitPackageBalanceHdDao balanceHdDao = new VisitPackageBalanceHdDao(ctx);
            PatientChargesDtInfoDao entityInfoDao = new PatientChargesDtInfoDao(ctx);

            int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            int oldParamedicID = 0;
            int newParamedicID = 0;

            try
            {
                bool isAllowSaveDt = false;
                PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
                if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityPatientChargesHd.TransactionNo);
                        result = false;
                    }
                }
                else
                {
                    if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityPatientChargesHd.TransactionNo);
                        result = false;
                    }
                }

                if (isAllowSaveDt)
                {
                    PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnServiceTransactionDtID.Value));
                    ItemService its = itemServiceDao.Get(entityDt.ItemID);
                    if (!entityDt.IsDeleted)
                    {
                        oldParamedicID = entityDt.ParamedicID;
                        ServiceControlToEntity(entityDt);
                        if (entityPatientChargesHd.TestOrderID != 0 && entityPatientChargesHd.TestOrderID != null)
                        {
                            TestOrderDt entityTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityPatientChargesHd.TestOrderID, entityDt.ItemID)).FirstOrDefault();
                            if (entityTestOrderDt != null)
                            {
                                if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
                                {
                                    entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
                                }
                                else entityTestOrderDt.BusinessPartnerID = null;
                                entityTestOrderDtDao.Update(entityTestOrderDt);
                            }
                        }

                        newParamedicID = entityDt.ParamedicID;

                        if (oldParamedicID != newParamedicID)
                        {
                            string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", newParamedicID, DateTime.Now);
                            List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
                            foreach (ParamedicMasterTeam pmt in pmtList)
                            {
                                PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                                dtparamedic.ID = entityDt.ID;
                                dtparamedic.ItemID = entityDt.ItemID;
                                dtparamedic.ParamedicID = pmt.ParamedicID;
                                dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                                dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtParamedicDao.Insert(dtparamedic);
                            }
                        }

                        decimal oAccumulatedDiscountAmount = 0;
                        decimal oAccumulatedDiscountAmountComp1 = 0;
                        decimal oAccumulatedDiscountAmountComp2 = 0;
                        decimal oAccumulatedDiscountAmountComp3 = 0;

                        bool isChargePackageDt = true;
                        List<PatientChargesDtPackage> lstDtPackage = new List<PatientChargesDtPackage>();
                        string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", hdnServiceTransactionDtID.Value);
                        List<PatientChargesDtPackage> entityPackageLst = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                        foreach (PatientChargesDtPackage dtpackage in entityPackageLst)
                        {
                            ItemMaster im = itemMasterDao.Get(dtpackage.ItemID);
                            ItemService isd = itemServiceDao.Get(dtpackage.ItemID);

                            decimal qtyCharges = 1;

                            if (isd != null)
                            {
                                qtyCharges = isd.IsPackageBalanceItem ? !string.IsNullOrEmpty(txtPackageQtyTaken.Text) ? Convert.ToDecimal(txtPackageQtyTaken.Text) : 1 : 1;
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            decimal serviceDtQty = 0;
                            string filterServiceDt = string.Format("ItemID = {0} AND DetailItemID = {1} AND IsDeleted = 0", entityDt.ItemID, dtpackage.ItemID);
                            List<ItemServiceDt> lstServiceDt = BusinessLayer.GetItemServiceDtList(filterServiceDt, ctx);
                            if (lstServiceDt.Count() > 0)
                            {
                                serviceDtQty = lstServiceDt.FirstOrDefault().Quantity;
                            }

                            dtpackage.ChargedQuantity = (serviceDtQty * entityDt.ChargedQuantity) * qtyCharges;

                            //if (its.IsPackageItem && !its.IsUsingAccumulatedPrice)
                            //{
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            int itemType = im.GCItemType == Constant.ItemType.OBAT_OBATAN || im.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : im.GCItemType == Constant.ItemType.BARANG_UMUM || im.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                            GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, entityPatientChargesHd.VisitID, entityDt.ChargeClassID, dtpackage.ItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

                            decimal basePrice = 0;
                            decimal basePriceComp1 = 0;
                            decimal basePriceComp2 = 0;
                            decimal basePriceComp3 = 0;
                            decimal price = 0;
                            decimal priceComp1 = 0;
                            decimal priceComp2 = 0;
                            decimal priceComp3 = 0;
                            bool isDiscountUsedComp = false;
                            decimal discountAmount = 0;
                            decimal discountAmountComp1 = 0;
                            decimal discountAmountComp2 = 0;
                            decimal discountAmountComp3 = 0;
                            decimal coverageAmount = 0;
                            bool isDiscountInPercentage = false;
                            bool isDiscountInPercentageComp1 = false;
                            bool isDiscountInPercentageComp2 = false;
                            bool isDiscountInPercentageComp3 = false;
                            bool isCoverageInPercentage = false;
                            decimal costAmount = 0;
                            decimal grossLineAmount = 0;

                            basePrice = tariff.BasePrice;
                            basePriceComp1 = tariff.BasePriceComp1;
                            basePriceComp2 = tariff.BasePriceComp2;
                            basePriceComp3 = tariff.BasePriceComp3;
                            price = tariff.Price;
                            priceComp1 = tariff.PriceComp1;
                            priceComp2 = tariff.PriceComp2;
                            priceComp3 = tariff.PriceComp3;
                            isDiscountUsedComp = tariff.IsDiscountUsedComp;
                            discountAmount = tariff.DiscountAmount;
                            discountAmountComp1 = tariff.DiscountAmountComp1;
                            discountAmountComp2 = tariff.DiscountAmountComp2;
                            discountAmountComp3 = tariff.DiscountAmountComp3;
                            coverageAmount = tariff.CoverageAmount;
                            isDiscountInPercentage = tariff.IsDiscountInPercentage;
                            isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                            isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                            isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                            isCoverageInPercentage = tariff.IsCoverageInPercentage;
                            costAmount = tariff.CostAmount;
                            grossLineAmount = dtpackage.ChargedQuantity * price;

                            dtpackage.BaseTariff = tariff.BasePrice;
                            dtpackage.BaseComp1 = tariff.BasePriceComp1;
                            dtpackage.BaseComp2 = tariff.BasePriceComp2;
                            dtpackage.BaseComp3 = tariff.BasePriceComp3;
                            dtpackage.Tariff = tariff.Price;
                            dtpackage.TariffComp1 = tariff.PriceComp1;
                            dtpackage.TariffComp2 = tariff.PriceComp2;
                            dtpackage.TariffComp3 = tariff.PriceComp3;
                            dtpackage.CostAmount = tariff.CostAmount;

                            decimal totalDiscountAmount = 0;
                            decimal totalDiscountAmount1 = 0;
                            decimal totalDiscountAmount2 = 0;
                            decimal totalDiscountAmount3 = 0;

                            if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                            {
                                if (isDiscountUsedComp)
                                {
                                    if (priceComp1 > 0)
                                    {
                                        if (isDiscountInPercentageComp1)
                                        {
                                            totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                            dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                                        }
                                        else
                                        {
                                            totalDiscountAmount1 = discountAmountComp1;
                                            dtpackage.DiscountPercentageComp1 = 0;
                                        }
                                    }

                                    if (priceComp2 > 0)
                                    {
                                        if (isDiscountInPercentageComp2)
                                        {
                                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                            dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                                        }
                                        else
                                        {
                                            totalDiscountAmount2 = discountAmountComp2;
                                            dtpackage.DiscountPercentageComp2 = 0;
                                        }
                                    }

                                    if (priceComp3 > 0)
                                    {
                                        if (isDiscountInPercentageComp3)
                                        {
                                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                            dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                                        }
                                        else
                                        {
                                            totalDiscountAmount3 = discountAmountComp3;
                                            dtpackage.DiscountPercentageComp3 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    if (priceComp1 > 0)
                                    {
                                        totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                        dtpackage.DiscountPercentageComp1 = discountAmount;
                                    }

                                    if (priceComp2 > 0)
                                    {
                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                        dtpackage.DiscountPercentageComp2 = discountAmount;
                                    }

                                    if (priceComp3 > 0)
                                    {
                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                        dtpackage.DiscountPercentageComp3 = discountAmount;
                                    }
                                }

                                if (dtpackage.DiscountPercentageComp1 > 0)
                                {
                                    dtpackage.IsDiscountInPercentageComp1 = true;
                                }

                                if (dtpackage.DiscountPercentageComp2 > 0)
                                {
                                    dtpackage.IsDiscountInPercentageComp2 = true;
                                }

                                if (dtpackage.DiscountPercentageComp3 > 0)
                                {
                                    dtpackage.IsDiscountInPercentageComp3 = true;
                                }
                            }
                            else
                            {
                                dtpackage.DiscountPercentageComp1 = 0;
                                dtpackage.DiscountPercentageComp2 = 0;
                                dtpackage.DiscountPercentageComp3 = 0;

                                dtpackage.IsDiscountInPercentageComp1 = false;
                                dtpackage.IsDiscountInPercentageComp2 = false;
                                dtpackage.IsDiscountInPercentageComp3 = false;

                                if (isDiscountUsedComp)
                                {
                                    if (priceComp1 > 0)
                                        totalDiscountAmount1 = discountAmountComp1;
                                    if (priceComp2 > 0)
                                        totalDiscountAmount2 = discountAmountComp2;
                                    if (priceComp3 > 0)
                                        totalDiscountAmount3 = discountAmountComp3;
                                }
                                else
                                {
                                    if (priceComp1 > 0)
                                        totalDiscountAmount1 = discountAmount;
                                    if (priceComp2 > 0)
                                        totalDiscountAmount2 = discountAmount;
                                    if (priceComp3 > 0)
                                        totalDiscountAmount3 = discountAmount;
                                }
                            }

                            totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                            if (grossLineAmount > 0)
                            {
                                if (totalDiscountAmount > grossLineAmount)
                                {
                                    totalDiscountAmount = grossLineAmount;
                                }
                            }


                            dtpackage.DiscountAmount = totalDiscountAmount;
                            dtpackage.DiscountComp1 = totalDiscountAmount1;
                            dtpackage.DiscountComp2 = totalDiscountAmount2;
                            dtpackage.DiscountComp3 = totalDiscountAmount3;
                            //}

                            if (its != null)
                            {
                                if (its.IsPackageItem && its.IsUsingAccumulatedPrice)
                                {
                                    oAccumulatedDiscountAmount += dtpackage.DiscountAmount;
                                    oAccumulatedDiscountAmountComp1 += dtpackage.DiscountComp1;
                                    oAccumulatedDiscountAmountComp2 += dtpackage.DiscountComp2;
                                    oAccumulatedDiscountAmountComp3 += dtpackage.DiscountComp3;
                                }
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", dtpackage.ItemID);
                            List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                            if (iplan.Count() > 0)
                            {
                                dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                            }
                            else
                            {
                                dtpackage.AveragePrice = 0;
                            }

                            if (oldParamedicID != newParamedicID)
                            {
                                dtpackage.ParamedicID = newParamedicID;
                            }

                            dtpackage.LastUpdatedBy = AppSession.UserLogin.UserID;

                            if (its.IsUsingAccumulatedPrice)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtPackageDao.Update(dtpackage);
                            }

                            lstDtPackage.Add(dtpackage);
                        }

                        if (oAccumulatedDiscountAmount != 0)
                        {
                            entityDt.IsDiscount = true;
                            entityDt.DiscountAmount = oAccumulatedDiscountAmount;
                            entityDt.DiscountComp1 = oAccumulatedDiscountAmountComp1;
                            entityDt.DiscountComp2 = oAccumulatedDiscountAmountComp2;
                            entityDt.DiscountComp3 = oAccumulatedDiscountAmountComp3;
                            entityDt.DiscountPercentageComp1 = 0;
                            entityDt.DiscountPercentageComp2 = 0;
                            entityDt.DiscountPercentageComp3 = 0;
                            entityDt.IsDiscountInPercentageComp1 = false;
                            entityDt.IsDiscountInPercentageComp2 = false;
                            entityDt.IsDiscountInPercentageComp3 = false;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        }
                        else
                        {
                            if (its.IsPackageItem && its.IsUsingAccumulatedPrice)
                            {
                                entityDt.DiscountAmount = 0;
                                entityDt.DiscountComp1 = 0;
                                entityDt.DiscountComp2 = 0;
                                entityDt.DiscountComp3 = 0;
                                entityDt.DiscountPercentageComp1 = 0;
                                entityDt.DiscountPercentageComp2 = 0;
                                entityDt.DiscountPercentageComp3 = 0;
                                entityDt.IsDiscountInPercentageComp1 = false;
                                entityDt.IsDiscountInPercentageComp2 = false;
                                entityDt.IsDiscountInPercentageComp3 = false;
                                entityDt.IsDiscount = false;
                            }
                        }

                        if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                        {
                            decimal BaseTariff = 0;
                            decimal BaseComp1 = 0;
                            decimal BaseComp2 = 0;
                            decimal BaseComp3 = 0;
                            decimal Tariff = 0;
                            decimal TariffComp1 = 0;
                            decimal TariffComp2 = 0;
                            decimal TariffComp3 = 0;
                            decimal DiscountAmount = 0;
                            decimal DiscountComp1 = 0;
                            decimal DiscountComp2 = 0;
                            decimal DiscountComp3 = 0;
                            foreach (PatientChargesDtPackage e in entityPackageLst)
                            {
                                BaseTariff += e.BaseTariff * e.ChargedQuantity;
                                BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                                BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                                BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                                Tariff += e.Tariff * e.ChargedQuantity;
                                TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                                TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                                TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                                DiscountAmount += e.DiscountAmount;
                                DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                                DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                                DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;

                            }

                            entityDt.BaseTariff = BaseTariff / entityDt.ChargedQuantity;
                            entityDt.BaseComp1 = BaseComp1 / entityDt.ChargedQuantity;
                            entityDt.BaseComp2 = BaseComp2 / entityDt.ChargedQuantity;
                            entityDt.BaseComp3 = BaseComp3 / entityDt.ChargedQuantity;
                            entityDt.Tariff = Tariff / entityDt.ChargedQuantity;
                            entityDt.TariffComp1 = TariffComp1 / entityDt.ChargedQuantity;
                            entityDt.TariffComp2 = TariffComp2 / entityDt.ChargedQuantity;
                            entityDt.TariffComp3 = TariffComp3 / entityDt.ChargedQuantity;
                            entityDt.DiscountAmount = DiscountAmount;
                            entityDt.DiscountComp1 = DiscountComp1 / entityDt.ChargedQuantity;
                            entityDt.DiscountComp2 = DiscountComp2 / entityDt.ChargedQuantity;
                            entityDt.DiscountComp3 = DiscountComp3 / entityDt.ChargedQuantity;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitIDCtl.Value), entityDt.ChargeClassID, entityDt.ItemID, 1, entityDt.CreatedDate, ctx);

                            decimal coverageAmount = 0;
                            bool isCoverageInPercentage = false;
                            if (list.Count > 0)
                            {
                                GetCurrentItemTariff obj = list[0];
                                coverageAmount = obj.CoverageAmount;
                                isCoverageInPercentage = obj.IsCoverageInPercentage;
                            }

                            if (entityDt.IsCITO)
                            {
                                if (entityDt.IsCITOInPercentage)
                                {
                                    decimal tariff = (entityDt.Tariff * entityDt.ChargedQuantity);
                                    entityDt.CITOAmount = ((entityDt.BaseCITOAmount / 100) * tariff);
                                }
                                else
                                {
                                    entityDt.CITOAmount = entityDt.BaseCITOAmount * entityDt.ChargedQuantity;
                                }
                            }
                            else
                            {
                                entityDt.CITOAmount = 0;
                            }

                            decimal grossLineAmount = (entityDt.Tariff * entityDt.ChargedQuantity) + (entityDt.CITOAmount - entityDt.CITODiscount);
                            decimal totalDiscountAmount = entityDt.DiscountAmount;
                            if (grossLineAmount > 0)
                            {
                                if (totalDiscountAmount > grossLineAmount)
                                {
                                    totalDiscountAmount = grossLineAmount;
                                }
                            }

                            decimal total = grossLineAmount - totalDiscountAmount;
                            decimal totalPayer = 0;
                            if (isCoverageInPercentage)
                            {
                                totalPayer = total * coverageAmount / 100;
                            }
                            else
                            {
                                totalPayer = coverageAmount * entityDt.ChargedQuantity;
                            }

                            if (total == 0)
                            {
                                totalPayer = total;
                            }
                            else
                            {
                                if (totalPayer < 0 && totalPayer < total)
                                {
                                    totalPayer = total;
                                }
                                else if (totalPayer > 0 & totalPayer > total)
                                {
                                    totalPayer = total;
                                }
                            }

                            decimal oPatientAmount = total - totalPayer;
                            decimal oPayerAmount = totalPayer;
                            decimal oLineAmount = total;

                            if (hdnIsEndingAmountRoundingTo1.Value == "1")
                            {
                                decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPatientAmount = Math.Floor(oPatientAmount);
                                }
                                else
                                {
                                    oPatientAmount = Math.Ceiling(oPatientAmount);
                                }

                                decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPayerAmount = Math.Floor(oPayerAmount);
                                }
                                else
                                {
                                    oPayerAmount = Math.Ceiling(oPayerAmount);
                                }

                                oLineAmount = oPatientAmount + oPayerAmount;
                            }

                            entityDt.PatientAmount = oPatientAmount;
                            entityDt.PayerAmount = oPayerAmount;
                            entityDt.LineAmount = oLineAmount;
                        }
                        else if (!its.IsUsingAccumulatedPrice && its.IsPackageItem)
                        {
                            foreach (PatientChargesDtPackage e in lstDtPackage)
                            {
                                //ctx.CommandType = CommandType.Text;
                                //ctx.Command.Parameters.Clear();
                                PatientChargesDtPackage packageDt = e;

                                if (entityDt.TariffComp1 != 0)
                                {
                                    packageDt.TariffComp1 = ((packageDt.BaseComp1 / lstDtPackage.Sum(t => t.BaseTariff)) * entityDt.Tariff);
                                }
                                else
                                {
                                    packageDt.TariffComp1 = 0;
                                }

                                if (entityDt.TariffComp2 != 0)
                                {
                                    packageDt.TariffComp2 = ((packageDt.BaseComp2 / lstDtPackage.Sum(t => t.BaseTariff)) * entityDt.Tariff);
                                }
                                else
                                {
                                    packageDt.TariffComp2 = 0;
                                }

                                if (entityDt.TariffComp3 != 0)
                                {
                                    packageDt.TariffComp3 = ((packageDt.BaseComp3 / lstDtPackage.Sum(t => t.BaseTariff)) * entityDt.Tariff);
                                }
                                else
                                {
                                    packageDt.TariffComp3 = 0;
                                }

                                packageDt.Tariff = packageDt.TariffComp1 + packageDt.TariffComp2 + packageDt.TariffComp3;

                                if (entityDt.DiscountComp1 != 0)
                                {
                                    packageDt.DiscountComp1 = ((packageDt.Tariff / entityDt.Tariff) * entityDt.DiscountComp1);
                                }
                                else
                                {
                                    packageDt.DiscountComp1 = 0;
                                }

                                if (entityDt.DiscountComp2 != 0)
                                {
                                    packageDt.DiscountComp2 = ((packageDt.Tariff / entityDt.Tariff) * entityDt.DiscountComp2);
                                }
                                else
                                {
                                    packageDt.DiscountComp2 = 0;
                                }

                                if (entityDt.DiscountComp3 != 0)
                                {
                                    packageDt.DiscountComp3 = ((packageDt.Tariff / entityDt.Tariff) * entityDt.DiscountComp3);
                                }
                                else
                                {
                                    packageDt.DiscountComp3 = 0;
                                }

                                packageDt.DiscountAmount = (packageDt.DiscountComp1 + packageDt.DiscountComp2 + packageDt.DiscountComp3) * packageDt.ChargedQuantity;

                                //packageDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtPackageDao.Update(packageDt);
                            }
                        }

                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(entityDt);

                        PatientChargesDtInfo info = entityInfoDao.Get(entityDt.ID);
                        //info.Remarks = txtRemarks.Text;
                        info.LastUpdatedBy = AppSession.UserLogin.UserID;

                        //if (info.PromotionSchemeDetailID != null && info.PromotionSchemeDetailID != 0)
                        //{
                        //    result = false;
                        //    if (!String.IsNullOrEmpty(errMessage))
                        //    {
                        //        errMessage += string.Format("Harap Batalkan Pemberian Diskon Detail Untuk Item Ini Terlebih Dahulu");
                        //    }
                        //    else
                        //    {
                        //        errMessage = string.Format("Harap Batalkan Pemberian Diskon Detail Untuk Item Ini Terlebih Dahulu");
                        //    }
                        //}

                        if (!string.IsNullOrEmpty(hdnIsPackageBalanceItem.Value) && Convert.ToBoolean(hdnIsPackageBalanceItem.Value))
                        {
                            info.IsPackageBalance = true;
                            if (!String.IsNullOrEmpty(txtPackageQtyTaken.Text))
                            {
                                info.PackageBalanceQtyTaken = Convert.ToDecimal(txtPackageQtyTaken.Text);

                                if (info.TakenNumber == null)
                                {
                                    info.TakenNumber = 1;
                                }
                            }

                            if (info.VisitPackageBalanceTransactionID != null)
                            {
                                VisitPackageBalanceHd balanceHD = balanceHdDao.Get(Convert.ToInt32(info.VisitPackageBalanceTransactionID));
                                Decimal qtySisa = balanceHD.Quantity - info.PackageBalanceQtyTaken;
                                if (qtySisa < 0)
                                {
                                    result = false;

                                    ItemMaster item = itemMasterDao.Get(entityDt.ItemID);
                                    if (!String.IsNullOrEmpty(errMessage))
                                    {
                                        errMessage += string.Format("Sisa paket untuk item {0} tidak mencukupi", item.ItemName1);
                                    }
                                    else
                                    {
                                        errMessage = string.Format("Sisa paket untuk item {0} tidak mencukupi", item.ItemName1);
                                    }
                                }
                            }
                            else
                            {
                                info.PackageExpiredDate = Helper.GetDatePickerValue(txtExpiredDate);
                            }
                        }

                        //#region alat
                        //if (!string.IsNullOrEmpty(hdnFixedAssetID.Value) || hdnFixedAssetID.Value != "0")
                        //{
                        //    info.FAItemID = Convert.ToInt32(hdnFixedAssetID.Value);
                        //}
                        //else
                        //{
                        //    info.FAItemID = null;
                        //}
                        //#endregion

                        //#region alasan diskon
                        //if (chkServiceIsDiscount.Checked)
                        //{
                        //    if (!string.IsNullOrEmpty(cboDiscReason.Value.ToString()))
                        //    {

                        //        info.GCDiscountReason = cboDiscReason.Value.ToString();
                        //        if (cboDiscReason.Value.ToString() == Constant.DiscountReason.LAIN_LAIN)
                        //        {
                        //            info.OtherDiscountReason = txtOtherReason.Text;
                        //        }
                        //        else
                        //        {
                        //            info.OtherDiscountReason = "";
                        //        }
                        //    }

                        //}
                        //#endregion

                        entityInfoDao.Update(info);
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnSaveEditRecordServiceAIO(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            VisitPackageBalanceHdDao balanceHdDao = new VisitPackageBalanceHdDao(ctx);
            PatientChargesDtInfoDao entityInfoDao = new PatientChargesDtInfoDao(ctx);

            int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            int oldParamedicID = 0;
            int newParamedicID = 0;

            try
            {
                bool isAllowSaveDt = false;
                PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
                if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityPatientChargesHd.TransactionNo);
                        result = false;
                    }
                }
                else
                {
                    if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityPatientChargesHd.TransactionNo);
                        result = false;
                    }
                }

                if (isAllowSaveDt)
                {
                    PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnServiceTransactionDtID.Value));
                    vItemService its = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entityDt.ItemID), ctx).FirstOrDefault();
                    if (!entityDt.IsDeleted)
                    {
                        ServiceControlToEntity(entityDt);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        List<GetCurrentItemTariffAIO> list = BusinessLayer.GetCurrentItemTariffAIO(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitIDCtl.Value), entityDt.ChargeClassID, entityDt.ItemID, 1, DateTime.Now, ctx);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        decimal basePrice = 0;
                        decimal basePriceComp1 = 0;
                        decimal basePriceComp2 = 0;
                        decimal basePriceComp3 = 0;
                        decimal price = 0;
                        decimal priceComp1 = 0;
                        decimal priceComp2 = 0;
                        decimal priceComp3 = 0;
                        bool isDiscountUsedComp = false;
                        decimal discountAmount = 0;
                        decimal discountAmountComp1 = 0;
                        decimal discountAmountComp2 = 0;
                        decimal discountAmountComp3 = 0;
                        decimal coverageAmount = 0;
                        bool isDiscountInPercentage = false;
                        bool isDiscountInPercentageComp1 = false;
                        bool isDiscountInPercentageComp2 = false;
                        bool isDiscountInPercentageComp3 = false;
                        bool isCoverageInPercentage = false;
                        decimal costAmount = 0;

                        if (list.Count > 0)
                        {
                            GetCurrentItemTariffAIO obj = list[0];
                            basePrice = obj.BasePrice;
                            basePriceComp1 = obj.BasePriceComp1;
                            basePriceComp2 = obj.BasePriceComp2;
                            basePriceComp3 = obj.BasePriceComp3;
                            price = obj.Price;
                            priceComp1 = obj.PriceComp1;
                            priceComp2 = obj.PriceComp2;
                            priceComp3 = obj.PriceComp3;
                            isDiscountUsedComp = obj.IsDiscountUsedComp;
                            discountAmount = obj.DiscountAmount;
                            discountAmountComp1 = obj.DiscountAmountComp1;
                            discountAmountComp2 = obj.DiscountAmountComp2;
                            discountAmountComp3 = obj.DiscountAmountComp3;
                            coverageAmount = obj.CoverageAmount;
                            isDiscountInPercentage = obj.IsDiscountInPercentage;
                            isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                            isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                            isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                            isCoverageInPercentage = obj.IsCoverageInPercentage;
                            costAmount = obj.CostAmount;
                        }

                        entityDt.BaseTariff = basePrice;
                        entityDt.Tariff = price;
                        entityDt.BaseComp1 = basePriceComp1;
                        entityDt.BaseComp2 = basePriceComp2;
                        entityDt.BaseComp3 = basePriceComp3;
                        entityDt.TariffComp1 = priceComp1;
                        entityDt.TariffComp2 = priceComp2;
                        entityDt.TariffComp3 = priceComp3;
                        entityDt.CostAmount = costAmount;

                        entityDt.GCBaseUnit = entityDt.GCItemUnit = its.GCItemUnit;
                        entityDt.IsSubContractItem = its.IsSubContractItem;

                        entityDt.IsVariable = false;
                        entityDt.IsUnbilledItem = false;

                        decimal qty = Convert.ToDecimal(txtAIOQty.Text);
                        decimal grossLineAmount = qty * price;

                        decimal totalDiscountAmount = 0;
                        decimal totalDiscountAmount1 = 0;
                        decimal totalDiscountAmount2 = 0;
                        decimal totalDiscountAmount3 = 0;

                        if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                        {
                            if (isDiscountUsedComp)
                            {
                                if (priceComp1 > 0)
                                {
                                    if (isDiscountInPercentageComp1)
                                    {
                                        totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                        entityDt.DiscountPercentageComp1 = discountAmountComp1;
                                    }
                                    else
                                    {
                                        totalDiscountAmount1 = discountAmountComp1;
                                    }
                                }

                                if (priceComp2 > 0)
                                {
                                    if (isDiscountInPercentageComp2)
                                    {
                                        totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                        entityDt.DiscountPercentageComp2 = discountAmountComp2;
                                    }
                                    else
                                    {
                                        totalDiscountAmount2 = discountAmountComp2;
                                    }
                                }

                                if (priceComp3 > 0)
                                {
                                    if (isDiscountInPercentageComp3)
                                    {
                                        totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                        entityDt.DiscountPercentageComp3 = discountAmountComp3;
                                    }
                                    else
                                    {
                                        totalDiscountAmount3 = discountAmountComp3;
                                    }
                                }
                            }
                            else
                            {
                                if (priceComp1 > 0)
                                {
                                    totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                    entityDt.DiscountPercentageComp1 = discountAmount;
                                }

                                if (priceComp2 > 0)
                                {
                                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                    entityDt.DiscountPercentageComp2 = discountAmount;
                                }

                                if (priceComp3 > 0)
                                {
                                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                    entityDt.DiscountPercentageComp3 = discountAmount;
                                }
                            }

                            if (entityDt.DiscountPercentageComp1 > 0)
                            {
                                entityDt.IsDiscountInPercentageComp1 = true;
                            }

                            if (entityDt.DiscountPercentageComp2 > 0)
                            {
                                entityDt.IsDiscountInPercentageComp2 = true;
                            }

                            if (entityDt.DiscountPercentageComp3 > 0)
                            {
                                entityDt.IsDiscountInPercentageComp3 = true;
                            }
                        }
                        else
                        {
                            if (isDiscountUsedComp)
                            {
                                if (priceComp1 > 0)
                                    totalDiscountAmount1 = discountAmountComp1;
                                if (priceComp2 > 0)
                                    totalDiscountAmount2 = discountAmountComp2;
                                if (priceComp3 > 0)
                                    totalDiscountAmount3 = discountAmountComp3;
                            }
                            else
                            {
                                if (priceComp1 > 0)
                                    totalDiscountAmount1 = discountAmount;
                                if (priceComp2 > 0)
                                    totalDiscountAmount2 = discountAmount;
                                if (priceComp3 > 0)
                                    totalDiscountAmount3 = discountAmount;
                            }
                        }

                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (qty);

                        if (grossLineAmount > 0)
                        {
                            if (totalDiscountAmount > grossLineAmount)
                            {
                                totalDiscountAmount = grossLineAmount;
                            }
                        }

                        decimal total = grossLineAmount - totalDiscountAmount;
                        decimal totalPayer = 0;
                        if (isCoverageInPercentage)
                        {
                            totalPayer = total * coverageAmount / 100;
                        }
                        else
                        {
                            totalPayer = coverageAmount * qty;
                        }

                        if (total == 0)
                        {
                            totalPayer = total;
                        }
                        else
                        {
                            if (totalPayer < 0 && totalPayer < total)
                            {
                                totalPayer = total;
                            }
                            else if (totalPayer > 0 & totalPayer > total)
                            {
                                totalPayer = total;
                            }
                        }

                        entityDt.IsCITO = false;
                        entityDt.IsCITOInPercentage = its.IsCITOInPercentage;
                        entityDt.BaseCITOAmount = its.CITOAmount;
                        entityDt.CITOAmount = 0;

                        entityDt.IsComplication = false;
                        entityDt.IsComplicationInPercentage = false;
                        entityDt.BaseComplicationAmount = 0;
                        entityDt.ComplicationAmount = 0;

                        entityDt.IsDiscount = totalDiscountAmount != 0;
                        entityDt.DiscountAmount = totalDiscountAmount;
                        entityDt.DiscountComp1 = totalDiscountAmount1;
                        entityDt.DiscountComp2 = totalDiscountAmount2;
                        entityDt.DiscountComp3 = totalDiscountAmount3;

                        entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;

                        decimal oPatientAmount = total - totalPayer;
                        decimal oPayerAmount = totalPayer;
                        decimal oLineAmount = total;

                        if (hdnIsEndingAmountRoundingTo1.Value == "1")
                        {
                            decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                            decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                            if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                            {
                                oPatientAmount = Math.Floor(oPatientAmount);
                            }
                            else
                            {
                                oPatientAmount = Math.Ceiling(oPatientAmount);
                            }

                            decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                            decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                            if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                            {
                                oPayerAmount = Math.Floor(oPayerAmount);
                            }
                            else
                            {
                                oPayerAmount = Math.Ceiling(oPayerAmount);
                            }

                            oLineAmount = oPatientAmount + oPayerAmount;
                        }

                        entityDt.PatientAmount = oPatientAmount;
                        entityDt.PayerAmount = oPayerAmount;
                        entityDt.LineAmount = oLineAmount;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(its.ItemCode, entityDt.ParamedicID, entityDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitIDCtl.Value), entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, entityPatientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        if (entityDt.RevenueSharingID == 0)
                            entityDt.RevenueSharingID = null;

                        entityDtDao.Update(entityDt);
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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
        #endregion
    }
}