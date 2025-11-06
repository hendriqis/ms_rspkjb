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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplateQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnTransactionID.Value = hdnParam.Value.Split('|')[0];
            hdnHealthcareServiceUnitID.Value = hdnParam.Value.Split('|')[1];
            hdnVisitID.Value = hdnParam.Value.Split('|')[2];
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[3];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[4];

            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            hdnIsBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

            SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS);
            hdnIsOnlyBPJSItem.Value = setvar.ParameterValue;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);

            if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
            {
                List<vPatientChargesDt> lstItemID = BusinessLayer.GetvPatientChargesDtList(string.Format(
                    "TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'",
                    hdnTransactionID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vPatientChargesDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    hdnListItemBefore.Value = lstSelectedID.Substring(1);
                }
            }
            if (hdnListItemBefore.Value == "")
            {
                hdnListItemBefore.Value = "0";
            }
        }

        private void SetControlProperties()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (hdnHealthcareServiceUnitID.Value == BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue)
                {
                    hdnGCItemType.Value = Constant.ItemGroupMaster.LABORATORY;

                    SettingParameter sp = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.LB_KODE_DEFAULT_DOKTER))[0];
                    SettingParameterDt spd = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", sp.ParameterCode))[0];
                    if (spd.ParameterValue != null && spd.ParameterValue != "")
                    {
                        ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0} AND IsDeleted = 0", Convert.ToInt32(spd.ParameterValue)))[0];
                        hdnParamedicID.Value = spd.ParameterValue;
                        txtParamedicCode.Text = pm.ParamedicCode;
                        txtParamedicName.Text = pm.FullName;
                    }
                }
                else if (hdnHealthcareServiceUnitID.Value == BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue)
                {
                    hdnGCItemType.Value = Constant.ItemGroupMaster.RADIOLOGY;

                    SettingParameter sp = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.IS_KODE_DEFAULT_DOKTER))[0];
                    SettingParameterDt spd = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", sp.ParameterCode))[0];
                    if (spd.ParameterValue != null && spd.ParameterValue != "")
                    {
                        ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0} AND IsDeleted = 0", Convert.ToInt32(spd.ParameterValue)))[0];
                        hdnParamedicID.Value = spd.ParameterValue;
                        txtParamedicCode.Text = pm.ParamedicCode;
                        txtParamedicName.Text = pm.FullName;
                    }
                }
                else
                {
                    hdnParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
                    hdnGCItemType.Value = Constant.ItemGroupMaster.DIAGNOSTIC;
                }
            }
            else
            {
                hdnParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
                hdnGCItemType.Value = Constant.ItemGroupMaster.SERVICE;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TestTemplateHd entity = e.Row.DataItem as TestTemplateHd;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.TestTemplateID.ToString()))
                {
                    chkIsSelected.Checked = true;
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("IsDeleted = 0");
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetTestTemplateHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<TestTemplateHd> lstEntity = BusinessLayer.GetTestTemplateHdList(filterExpression, 10, pageIndex, "TestTemplateName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            try
            {
                #region Patient Charges Hd
                PatientChargesHd patientChargesHd = null;
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    if (hdnIsAccompany.Value == "0")
                    {
                        switch (hdnDepartmentID.Value)
                        {
                            case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                            case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                else if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                else
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                            default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                        }
                    }
                    else
                    {
                        patientChargesHd.TransactionCode = Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES;
                    }
                    patientChargesHd.TransactionDate = Helper.GetDatePickerValue(DetailPage.GetTransactionDate());
                    patientChargesHd.TransactionTime = DetailPage.GetTransactionTime();
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    hdnTransactionID.Value = patientChargesHd.TransactionID.ToString();
                }
                else
                {
                    string filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", hdnVisitID.Value, hdnTransactionID.Value);
                    patientChargesHd = BusinessLayer.GetPatientChargesHdList(filterExpression).FirstOrDefault();
                }
                retval = patientChargesHd.TransactionNo;
                #endregion

                if (hdnParamedicID.Value == "")
                {
                    hdnParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
                }

                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);

                lstSelectedMember = hdnSelectedMember.Value.Split(',');

                //before
                //string filterTemplate = string.Format("TestTemplateID IN ({0})", lstSelectedMember);
                //List<TestTemplateDt> templateDtList = BusinessLayer.GetTestTemplateDtList(filterTemplate);

                List<TestTemplateDt> templateDtList = new List<TestTemplateDt>();
                foreach (string x in lstSelectedMember)
                {
                    string filterTemplate = string.Format("TestTemplateID IN ({0})", x);
                    List<TestTemplateDt> templateDt = BusinessLayer.GetTestTemplateDtList(filterTemplate);
                    templateDtList.AddRange(templateDt);
                }

                #region Filter Item Pelayanan

                string listServiceItemIDTemp = "";
                string[] listServiceItemIDArrayTemp = null;
                string listServiceItemID = "";
                string[] listServiceItemIDArray = null;

                foreach (TestTemplateDt templateDt in templateDtList) {
                    ServiceUnitItem sui = BusinessLayer.GetServiceUnitItem(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), templateDt.ItemID);
                    if (sui != null)
                    {
                        listServiceItemIDTemp += "," + templateDt.ItemID;
                    }
                }
                if (listServiceItemIDTemp != "")
                {
                    listServiceItemIDTemp = listServiceItemIDTemp.Substring(1);
                    listServiceItemIDArrayTemp = listServiceItemIDTemp.Split(',');
                }
                if (listServiceItemIDTemp == "")
                {
                    listServiceItemIDTemp = "0";
                }

                List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format(
                        "ItemID IN ({0}) AND ItemID NOT IN ({1})",
                        listServiceItemIDTemp, hdnListItemBefore.Value));
                foreach (vItemService iservice in lstItemService)
                {
                    listServiceItemID += "," + iservice.ItemID;
                }
                if (listServiceItemID != "")
                {
                    listServiceItemID = listServiceItemID.Substring(1);
                    listServiceItemIDArray = listServiceItemID.Split(',');
                }

                #endregion

                #region Filter Item Obat, Alkes, dan Barang Umum

                string listDrugItemIDTemp = "";
                string[] listDrugItemIDArrayTemp = null;
                string listDrugItemID = "";
                string[] listDrugItemIDArray = null;
                int locationID = DetailPage.GetLocationID();

                foreach (TestTemplateDt templateDt in templateDtList)
                {
                    string filterIBL = string.Format("ItemID = {0} AND LocationID = {1} AND IsDeleted = 0", templateDt.ItemID, locationID);
                    List<ItemBalance> listIBalance = BusinessLayer.GetItemBalanceList(filterIBL);
                    if (listIBalance.Count() > 0)
                    {
                        listDrugItemIDTemp += "," + templateDt.ItemID;
                    }
                }
                if (listDrugItemIDTemp != "")
                {
                    listDrugItemIDTemp = listDrugItemIDTemp.Substring(1);
                    listDrugItemIDArrayTemp = listDrugItemIDTemp.Split(',');
                }
                if (listDrugItemIDTemp == "")
                {
                    listDrugItemIDTemp = "0";
                }

                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(string.Format(
                        "HealthcareID = '{0}' AND ItemID IN ({1}) AND ItemID NOT IN ({2}) AND IsDeleted = 0",
                        AppSession.UserLogin.HealthcareID, listDrugItemIDTemp, hdnListItemBefore.Value), ctx);
                foreach (ItemPlanning iplanning in lstItemPlanning)
                {
                    listDrugItemID += "," + iplanning.ItemID;
                }
                if (listDrugItemID != "")
                {
                    listDrugItemID = listDrugItemID.Substring(1);
                    listDrugItemIDArray = listDrugItemID.Split(',');
                }
                #endregion

                if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        if (listServiceItemID != "")
                        {
                            #region Pelayanan
                            int ct = 0;
                            if (lstItemService.Count() > 0)
                            {
                                foreach (String itemID in listServiceItemIDArray)
                                {
                                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                                    patientChargesDt.ItemID = Convert.ToInt32(itemID);
                                    patientChargesDt.ChargeClassID = DetailPage.GetClassID();

                                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 1, DateTime.Now, ctx);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID);

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
                                        GetCurrentItemTariff obj = list[0];
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

                                    patientChargesDt.BaseTariff = basePrice;
                                    patientChargesDt.Tariff = price;
                                    patientChargesDt.BaseComp1 = basePriceComp1;
                                    patientChargesDt.BaseComp2 = basePriceComp2;
                                    patientChargesDt.BaseComp3 = basePriceComp3;
                                    patientChargesDt.TariffComp1 = priceComp1;
                                    patientChargesDt.TariffComp2 = priceComp2;
                                    patientChargesDt.TariffComp3 = priceComp3;
                                    patientChargesDt.CostAmount = costAmount;
                                    patientChargesDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                                    patientChargesDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                    patientChargesDt.BaseCITOAmount = entity.CITOAmount;
                                    patientChargesDt.BaseComplicationAmount = entity.ComplicationAmount;
                                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entity.GCItemUnit;
                                    patientChargesDt.IsSubContractItem = entity.IsSubContractItem;

                                    if (Convert.ToInt32(hdnParamedicID.Value) != AppSession.RegisteredPatient.ParamedicID)
                                    {
                                        patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                                    }
                                    else
                                    {
                                        if (entity.DefaultParamedicID != null && entity.DefaultParamedicID != 0)
                                        {
                                            patientChargesDt.ParamedicID = Convert.ToInt32(entity.DefaultParamedicID);
                                        }
                                        else
                                        {
                                            patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                                        }
                                    }

                                    patientChargesDt.IsVariable = false;
                                    patientChargesDt.IsUnbilledItem = false;

                                    decimal qty = 1;
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
                                                    patientChargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                    patientChargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                    patientChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                patientChargesDt.DiscountPercentageComp1 = discountAmount;
                                            }

                                            if (priceComp2 > 0)
                                            {
                                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp2 = discountAmount;
                                            }

                                            if (priceComp3 > 0)
                                            {
                                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp3 = discountAmount;
                                            }
                                        }

                                        if (patientChargesDt.DiscountPercentageComp1 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp1 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp2 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp2 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp3 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp3 = true;
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

                                    patientChargesDt.IsCITO = false;
                                    patientChargesDt.CITOAmount = 0;
                                    patientChargesDt.IsComplication = false;
                                    patientChargesDt.ComplicationAmount = 0;

                                    patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                                    patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                    patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                    patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty;
                                    patientChargesDt.PatientAmount = total - totalPayer;
                                    patientChargesDt.PayerAmount = totalPayer;
                                    patientChargesDt.LineAmount = total;

                                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    if (patientChargesDt.RevenueSharingID == 0)
                                        patientChargesDt.RevenueSharingID = null;

                                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                    int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                    string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", patientChargesDt.ParamedicID, DateTime.Now);
                                    List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
                                    foreach (ParamedicMasterTeam pmt in pmtList)
                                    {
                                        PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                                        dtparamedic.ID = ID;
                                        dtparamedic.ItemID = patientChargesDt.ItemID;
                                        dtparamedic.ParamedicID = pmt.ParamedicID;
                                        dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                                        dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDtParamedicDao.Insert(dtparamedic);
                                    }

                                    string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                    List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                                    foreach (vItemServiceDt isd in isdList)
                                    {
                                        PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                        dtpackage.PatientChargesDtID = ID;
                                        dtpackage.ItemID = isd.DetailItemID;
                                        dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                        int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                        if (revID != 0 && revID != null)
                                        {
                                            dtpackage.RevenueSharingID = revID;
                                        }
                                        else
                                        {
                                            dtpackage.RevenueSharingID = null;
                                        }

                                        dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, visitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

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

                                        if (grossLineAmount >= 0)
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
                                        entityDtPackageDao.Insert(dtpackage);
                                    }

                                    ct++;
                                }
                            }
                            #endregion
                        }

                        if (listDrugItemID != "")
                        {
                            #region Obat, Alkes, dan Barang Umum
                            int cto = 0;
                            if (lstItemPlanning.Count() > 0)
                            {
                                foreach (String itemID in listDrugItemIDArray)
                                {
                                    decimal qty = 1;

                                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                                    patientChargesDt.ItemID = Convert.ToInt32(itemID);
                                    patientChargesDt.ChargeClassID = DetailPage.GetClassID();

                                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 2, DateTime.Now, ctx);

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
                                        GetCurrentItemTariff obj = list[0];
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

                                    patientChargesDt.BaseTariff = basePrice;
                                    patientChargesDt.Tariff = price;
                                    patientChargesDt.BaseComp1 = basePriceComp1;
                                    patientChargesDt.BaseComp2 = basePriceComp2;
                                    patientChargesDt.BaseComp3 = basePriceComp3;
                                    patientChargesDt.TariffComp1 = priceComp1;
                                    patientChargesDt.TariffComp2 = priceComp2;
                                    patientChargesDt.TariffComp3 = priceComp3;
                                    patientChargesDt.CostAmount = costAmount;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemDao.Get(Convert.ToInt32(itemID)).GCItemUnit;

                                    int defaultParamedicID = Convert.ToInt32(itemDao.Get(Convert.ToInt32(itemID)).DefaultParamedicID);
                                    if (defaultParamedicID != null && defaultParamedicID != 0)
                                    {
                                        patientChargesDt.ParamedicID = defaultParamedicID;
                                    }
                                    else
                                    {
                                        patientChargesDt.ParamedicID = Convert.ToInt32(DetailPage.GetRegistrationPhysicianID());
                                    }

                                    patientChargesDt.IsVariable = false;
                                    patientChargesDt.IsUnbilledItem = false;

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
                                                    patientChargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                    patientChargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                    patientChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                patientChargesDt.DiscountPercentageComp1 = discountAmount;
                                            }

                                            if (priceComp2 > 0)
                                            {
                                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp2 = discountAmount;
                                            }

                                            if (priceComp3 > 0)
                                            {
                                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp3 = discountAmount;
                                            }
                                        }

                                        if (patientChargesDt.DiscountPercentageComp1 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp1 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp2 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp2 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp3 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp3 = true;
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

                                    patientChargesDt.ConversionFactor = 1;
                                    patientChargesDt.AveragePrice = lstItemPlanning.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID).AveragePrice;
                                    patientChargesDt.IsCITO = false;
                                    patientChargesDt.CITOAmount = 0;
                                    patientChargesDt.IsComplication = false;
                                    patientChargesDt.ComplicationAmount = 0;

                                    patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                                    patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                    patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                    patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty;
                                    patientChargesDt.PatientAmount = total - totalPayer;
                                    patientChargesDt.PayerAmount = totalPayer;
                                    patientChargesDt.LineAmount = total;
                                    patientChargesDt.LocationID = locationID;
                                    patientChargesDt.IsApproved = true;
                                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                    patientChargesDtDao.Insert(patientChargesDt);
                                    cto++;
                                }
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
                else
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        if (listServiceItemID != "")
                        {
                            #region Pelayanan
                            int ct = 0;
                            if (lstItemService.Count() > 0)
                            {
                                foreach (String itemID in listServiceItemIDArray)
                                {
                                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                                    patientChargesDt.ItemID = Convert.ToInt32(itemID);
                                    patientChargesDt.ChargeClassID = DetailPage.GetClassID();

                                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 1, DateTime.Now, ctx);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID);

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
                                        GetCurrentItemTariff obj = list[0];
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

                                    patientChargesDt.BaseTariff = basePrice;
                                    patientChargesDt.Tariff = price;
                                    patientChargesDt.BaseComp1 = basePriceComp1;
                                    patientChargesDt.BaseComp2 = basePriceComp2;
                                    patientChargesDt.BaseComp3 = basePriceComp3;
                                    patientChargesDt.TariffComp1 = priceComp1;
                                    patientChargesDt.TariffComp2 = priceComp2;
                                    patientChargesDt.TariffComp3 = priceComp3;
                                    patientChargesDt.CostAmount = costAmount;
                                    patientChargesDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                                    patientChargesDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                    patientChargesDt.BaseCITOAmount = entity.CITOAmount;
                                    patientChargesDt.BaseComplicationAmount = entity.ComplicationAmount;
                                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entity.GCItemUnit;
                                    patientChargesDt.IsSubContractItem = entity.IsSubContractItem;

                                    if (Convert.ToInt32(hdnParamedicID.Value) != AppSession.RegisteredPatient.ParamedicID)
                                    {
                                        patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                                    }
                                    else
                                    {
                                        if (entity.DefaultParamedicID != null && entity.DefaultParamedicID != 0)
                                        {
                                            patientChargesDt.ParamedicID = Convert.ToInt32(entity.DefaultParamedicID);
                                        }
                                        else
                                        {
                                            patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                                        }
                                    }

                                    patientChargesDt.IsVariable = false;
                                    patientChargesDt.IsUnbilledItem = false;

                                    decimal qty = 1;
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
                                                    patientChargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                    patientChargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                    patientChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                patientChargesDt.DiscountPercentageComp1 = discountAmount;
                                            }

                                            if (priceComp2 > 0)
                                            {
                                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp2 = discountAmount;
                                            }

                                            if (priceComp3 > 0)
                                            {
                                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp3 = discountAmount;
                                            }
                                        }

                                        if (patientChargesDt.DiscountPercentageComp1 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp1 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp2 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp2 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp3 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp3 = true;
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

                                    patientChargesDt.IsCITO = false;
                                    patientChargesDt.CITOAmount = 0;
                                    patientChargesDt.IsComplication = false;
                                    patientChargesDt.ComplicationAmount = 0;

                                    patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                                    patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                    patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                    patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty;
                                    patientChargesDt.PatientAmount = total - totalPayer;
                                    patientChargesDt.PayerAmount = totalPayer;
                                    patientChargesDt.LineAmount = total;

                                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    if (patientChargesDt.RevenueSharingID == 0)
                                        patientChargesDt.RevenueSharingID = null;

                                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                    int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                    string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", patientChargesDt.ParamedicID, DateTime.Now);
                                    List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
                                    foreach (ParamedicMasterTeam pmt in pmtList)
                                    {
                                        PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                                        dtparamedic.ID = ID;
                                        dtparamedic.ItemID = patientChargesDt.ItemID;
                                        dtparamedic.ParamedicID = pmt.ParamedicID;
                                        dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                                        dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDtParamedicDao.Insert(dtparamedic);
                                    }

                                    string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                    List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                                    foreach (vItemServiceDt isd in isdList)
                                    {
                                        PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                        dtpackage.PatientChargesDtID = ID;
                                        dtpackage.ItemID = isd.DetailItemID;
                                        dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                        int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                        if (revID != 0 && revID != null)
                                        {
                                            dtpackage.RevenueSharingID = revID;
                                        }
                                        else
                                        {
                                            dtpackage.RevenueSharingID = null;
                                        }

                                        dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, visitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

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

                                        if (grossLineAmount >= 0)
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
                                        entityDtPackageDao.Insert(dtpackage);
                                    }

                                    ct++;
                                }
                            }
                            #endregion
                        }

                        if (listDrugItemID != "")
                        {
                            #region Obat, Alkes, dan Barang Umum
                            int cto = 0;
                            if (lstItemPlanning.Count() > 0)
                            {
                                foreach (String itemID in listDrugItemIDArray)
                                {
                                    decimal qty = 1;

                                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                                    patientChargesDt.ItemID = Convert.ToInt32(itemID);
                                    patientChargesDt.ChargeClassID = DetailPage.GetClassID();

                                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 2, DateTime.Now, ctx);

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
                                        GetCurrentItemTariff obj = list[0];
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

                                    patientChargesDt.BaseTariff = basePrice;
                                    patientChargesDt.Tariff = price;
                                    patientChargesDt.BaseComp1 = basePriceComp1;
                                    patientChargesDt.BaseComp2 = basePriceComp2;
                                    patientChargesDt.BaseComp3 = basePriceComp3;
                                    patientChargesDt.TariffComp1 = priceComp1;
                                    patientChargesDt.TariffComp2 = priceComp2;
                                    patientChargesDt.TariffComp3 = priceComp3;
                                    patientChargesDt.CostAmount = costAmount;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemDao.Get(Convert.ToInt32(itemID)).GCItemUnit;

                                    int defaultParamedicID = Convert.ToInt32(itemDao.Get(Convert.ToInt32(itemID)).DefaultParamedicID);
                                    if (defaultParamedicID != null && defaultParamedicID != 0)
                                    {
                                        patientChargesDt.ParamedicID = defaultParamedicID;
                                    }
                                    else
                                    {
                                        patientChargesDt.ParamedicID = Convert.ToInt32(DetailPage.GetRegistrationPhysicianID());
                                    }

                                    patientChargesDt.IsVariable = false;
                                    patientChargesDt.IsUnbilledItem = false;

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
                                                    patientChargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                    patientChargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                    patientChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                patientChargesDt.DiscountPercentageComp1 = discountAmount;
                                            }

                                            if (priceComp2 > 0)
                                            {
                                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp2 = discountAmount;
                                            }

                                            if (priceComp3 > 0)
                                            {
                                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                patientChargesDt.DiscountPercentageComp3 = discountAmount;
                                            }
                                        }

                                        if (patientChargesDt.DiscountPercentageComp1 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp1 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp2 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp2 = true;
                                        }

                                        if (patientChargesDt.DiscountPercentageComp3 > 0)
                                        {
                                            patientChargesDt.IsDiscountInPercentageComp3 = true;
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

                                    patientChargesDt.ConversionFactor = 1;
                                    patientChargesDt.AveragePrice = lstItemPlanning.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID).AveragePrice;
                                    patientChargesDt.IsCITO = false;
                                    patientChargesDt.CITOAmount = 0;
                                    patientChargesDt.IsComplication = false;
                                    patientChargesDt.ComplicationAmount = 0;

                                    patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                                    patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                    patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                    patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty;
                                    patientChargesDt.PatientAmount = total - totalPayer;
                                    patientChargesDt.PayerAmount = totalPayer;
                                    patientChargesDt.LineAmount = total;
                                    patientChargesDt.LocationID = locationID;
                                    patientChargesDt.IsApproved = true;
                                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                    patientChargesDtDao.Insert(patientChargesDt);
                                    cto++;
                                }
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
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
    }
}