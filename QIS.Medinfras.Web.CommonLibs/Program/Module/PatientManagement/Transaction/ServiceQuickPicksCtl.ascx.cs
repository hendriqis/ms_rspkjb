using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            hdnParam.Value = param;
            hdnTransactionID.Value = hdnParam.Value.Split('|')[0];
            hdnHealthcareServiceUnitID.Value = hdnParam.Value.Split('|')[1];
            hdnVisitIDCtl.Value = hdnParam.Value.Split('|')[2];
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[3];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[4];
            hdnIsAccompany.Value = hdnParam.Value.Split('|')[5];
            hdnTransactionDateQuickPicksCtl.Value = hdnParam.Value.Split('|')[6];

            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            hdnIsBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

            ConsultVisit visit = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitIDCtl.Value));
            hdnChargesClassID.Value = visit.ChargeClassID.ToString();

            vHealthcareServiceUnitCustom visitHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", visit.HealthcareServiceUnitID)).FirstOrDefault();

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField(cboServiceChargeClassIDCtl, lstClassCare, "ClassName", "ClassID");

            if (visit.ChargeClassID != 0)
            {
                cboServiceChargeClassIDCtl.Value = Convert.ToString(visit.ChargeClassID);
            }

            if (visitHSU.DepartmentID == Constant.Facility.INPATIENT)
            {
                cboServiceChargeClassIDCtl.Enabled = true;
            }
            else
            {
                cboServiceChargeClassIDCtl.Enabled = false;
            }

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //1
                                                        Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //2
                                                        Constant.SettingParameter.SA0199, //3
                                                        Constant.SettingParameter.SA0205, //4
                                                        Constant.SettingParameter.MD_PEMERIKSAAN_PENUNJANG_MEDIS_HANYA_BPJS, //5
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //6
                                                        Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_PELAYANAN_QUICKPICK, //7
                                                        Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT //8
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnImagingServiceUnitID.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnDefaultExpiredDateByWeek.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0199).ParameterValue;
            hdnDefaultExpiredDateAgeUnit.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0205).ParameterValue;

            hdnIsOnlyBPJSItem.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.MD_PEMERIKSAAN_PENUNJANG_MEDIS_HANYA_BPJS).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnIsDisplayPrice.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_PELAYANAN_QUICKPICK).ParameterValue;
            hdnIsUsingValidateDigitDecimal.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT).FirstOrDefault().ParameterValue;

            if (hdnIsDisplayPrice.Value == "1")
            {
                trCalculate.Style.Remove("display");
                thHarga.Style.Remove("display");
            }
            else
            {
                trCalculate.Style.Add("display", "none");
                thHarga.Style.Add("display", "none");
            }

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            if (hdnParam.Value.Split('|')[4] == Constant.Facility.DIAGNOSTIC)
            {
                if (hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
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
                else if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
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
                    hdnGCItemType.Value = Constant.ItemGroupMaster.DIAGNOSTIC;
                }
            }
            else
            {
                hdnGCItemType.Value = Constant.ItemGroupMaster.SERVICE;
            }

            hdnTransactionDateQuickPicksCtl.Value = Helper.GetDatePickerValue(DetailPage.GetTransactionDate()).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string medicSupport = "";
            medicSupport += hdnParam.Value.Split('|')[1];

            if (hdnItemGroupID.Value == "")
                filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR ItemCode LIKE '%{1}%')", medicSupport, hdnFilterItem.Value);
            else
                filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR ItemCode LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%')", medicSupport, hdnFilterItem.Value, hdnItemGroupID.Value);

            if (hdnRegistrationID.Value == "1")
            {
                if (hdnIsOnlyBPJSItem.Value == "1")
                {
                    filterExpression += " AND IsBPJS = 1";
                }
            }

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);
            filterExpression += string.Format(" AND IsPackageAllInOne = 0");

            if (AppSession.SA0198)
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT || AppSession.RegisteredPatient.DepartmentID == Constant.Facility.DIAGNOSTIC)
                {
                    if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() != hdnLaboratoryServiceUnitID.Value && AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() != hdnImagingServiceUnitID.Value)
                    {
                        filterExpression += " AND (IsPackageBalanceItem = 1 OR IsPackageBalanceItem = 0 OR IsPackageBalanceItem IS NULL)";
                    }
                    else
                    {
                        filterExpression += " AND (IsPackageBalanceItem = 0 OR IsPackageBalanceItem IS NULL)";
                    }
                }
            }
            else
            {
                filterExpression += " AND (IsPackageBalanceItem = 0 OR IsPackageBalanceItem IS NULL)";
            }

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vServiceUnitItem entity = e.Row.DataItem as vServiceUnitItem;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                {
                    chkIsSelected.Checked = true;
                }

                if (entity.IsAllowChangeQty)
                {
                    txtQty.Disabled = false;
                }
                else
                {
                    txtQty.Disabled = true;
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
            {
                List<vPatientChargesDt> lstItemID = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}','{4}') AND IsDeleted = 0", hdnTransactionID.Value, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vPatientChargesDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vServiceUnitItem> lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao patientChargesHdInfoDao = new PatientChargesHdInfoDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtInfoDao entityInfoDao = new PatientChargesDtInfoDao(ctx);
            PatientChargesDtParamedicDao patientChargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);

            try
            {
                #region Patient Charges Hd

                PatientChargesHd patientChargesHd = null;
                int visitID = Convert.ToInt32(hdnParam.Value.Split('|')[2]);
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = visitID;
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnParam.Value.Split('|')[1]);
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
                                else if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(!string.IsNullOrEmpty(AppSession.RT0001) ? AppSession.RT0001 : "0"))
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_CHARGES;
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
                    patientChargesHd.Remarks = DetailPage.GetRemarksHd();
                    patientChargesHd.ReferenceNo = DetailPage.GetReferenceNoHd();
                    patientChargesHd.IsCorrectionTransaction = DetailPage.GetIsCorrectionTransactionHd() != null ? DetailPage.GetIsCorrectionTransactionHd() : false;
                    patientChargesHd.IsAIOTransaction = DetailPage.GetIsAIOTransactionHd() != null ? DetailPage.GetIsAIOTransactionHd() : false;
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
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    string filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, hdnTransactionID.Value);
                    patientChargesHd = BusinessLayer.GetPatientChargesHdList(filterExpression, ctx).FirstOrDefault();
                }

                retval = patientChargesHd.TransactionNo;

                #endregion

                bool isAllowSaveDt = false;
                if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat ditambah detail item lagi.", patientChargesHd.TransactionNo);
                        result = false;
                    }
                }
                else
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat ditambah detail item lagi.", patientChargesHd.TransactionNo);
                        result = false;
                    }
                }

                if (isAllowSaveDt)
                {
                    int registrationID = Convert.ToInt32(hdnParam.Value.Split('|')[3]);
                    lstSelectedMember = hdnSelectedMember.Value.Split(',');
                    string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');

                    List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);

                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        #region Patient Charges Dt

                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = Convert.ToInt32(itemID);
                        patientChargesDt.ChargeClassID = Convert.ToInt32(cboServiceChargeClassIDCtl.Value);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
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

                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entity.GCItemUnit;
                        patientChargesDt.IsSubContractItem = entity.IsSubContractItem;

                        patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);

                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal qty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
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
                        patientChargesDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                        patientChargesDt.BaseCITOAmount = entity.CITOAmount;
                        patientChargesDt.CITOAmount = 0;

                        patientChargesDt.IsComplication = false;
                        patientChargesDt.IsComplicationInPercentage = false;
                        patientChargesDt.BaseComplicationAmount = 0;
                        patientChargesDt.ComplicationAmount = 0;

                        patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                        patientChargesDt.DiscountAmount = totalDiscountAmount;
                        patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                        patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                        patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty;

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

                        patientChargesDt.PatientAmount = oPatientAmount;
                        patientChargesDt.PayerAmount = oPayerAmount;
                        patientChargesDt.LineAmount = oLineAmount;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;

                        if (patientChargesDt.RevenueSharingID == 0)
                            patientChargesDt.RevenueSharingID = null;

                        patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                        patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                        patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        int oChargesDtID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                        bool isChargePackageDt = true;
                        PatientChargesDtInfo info = entityInfoDao.Get(oChargesDtID);
                        if (entity.IsPackageBalanceItem)
                        {
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

                            info.IsPackageBalance = true;
                            info.IsFirstPackageBalance = true;
                            info.PackageExpiredDate = DateTime.Now.AddDays(Convert.ToInt32(hdnDefaultExpiredDateByWeek.Value) * ageUnitExpiredDate);
                            info.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityInfoDao.Update(info);

                            isChargePackageDt = false;
                        }

                        string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", patientChargesDt.ParamedicID, DateTime.Now);
                        List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam, ctx);
                        foreach (ParamedicMasterTeam pmt in pmtList)
                        {
                            PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                            dtparamedic.ID = oChargesDtID;
                            dtparamedic.ItemID = patientChargesDt.ItemID;
                            dtparamedic.ParamedicID = pmt.ParamedicID;
                            dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                            dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            patientChargesDtParamedicDao.Insert(dtparamedic);
                        }

                        int countInventoryItemDetail = 0;


                        List<PatientChargesDtPackage> lstDtPackage = new List<PatientChargesDtPackage>();
                        string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                        List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
                        foreach (vItemServiceDt isd in isdList)
                        {
                            if (isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS || isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                            {
                                countInventoryItemDetail += 1;
                            }

                            PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                            dtpackage.PatientChargesDtID = oChargesDtID;
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

                            basePrice = 0;
                            basePriceComp1 = 0;
                            basePriceComp2 = 0;
                            basePriceComp3 = 0;
                            price = 0;
                            priceComp1 = 0;
                            priceComp2 = 0;
                            priceComp3 = 0;
                            isDiscountUsedComp = false;
                            discountAmount = 0;
                            discountAmountComp1 = 0;
                            discountAmountComp2 = 0;
                            discountAmountComp3 = 0;
                            coverageAmount = 0;
                            isDiscountInPercentage = false;
                            isDiscountInPercentageComp1 = false;
                            isDiscountInPercentageComp2 = false;
                            isDiscountInPercentageComp3 = false;
                            isCoverageInPercentage = false;
                            costAmount = 0;

                            totalDiscountAmount = 0;
                            totalDiscountAmount1 = 0;
                            totalDiscountAmount2 = 0;
                            totalDiscountAmount3 = 0;

                            int itemType = isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                            GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

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

                            if (entity.IsUsingAccumulatedPrice)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                dtpackage.ID = entityDtPackageDao.InsertReturnPrimaryKeyID(dtpackage);
                            }

                            lstDtPackage.Add(dtpackage);
                        }

                        if (countInventoryItemDetail > 0)
                        {
                            HealthcareServiceUnit chargesHSU = chargesHSUDao.Get(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));

                            PatientChargesDt pcdt = patientChargesDtDao.Get(oChargesDtID);
                            pcdt.LocationID = chargesHSU.LocationID;
                            pcdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            patientChargesDtDao.Update(pcdt);
                        }

                        if (entity.IsUsingAccumulatedPrice && entity.IsPackageItem)
                        {
                            PatientChargesDt pcdt = patientChargesDtDao.Get(oChargesDtID);

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

                            pcdt.BaseTariff = BaseTariff / pcdt.ChargedQuantity;
                            pcdt.BaseComp1 = BaseComp1 / pcdt.ChargedQuantity;
                            pcdt.BaseComp2 = BaseComp2 / pcdt.ChargedQuantity;
                            pcdt.BaseComp3 = BaseComp3 / pcdt.ChargedQuantity;
                            pcdt.Tariff = Tariff / pcdt.ChargedQuantity;
                            pcdt.TariffComp1 = TariffComp1 / pcdt.ChargedQuantity;
                            pcdt.TariffComp2 = TariffComp2 / pcdt.ChargedQuantity;
                            pcdt.TariffComp3 = TariffComp3 / pcdt.ChargedQuantity;
                            pcdt.DiscountAmount = DiscountAmount;
                            pcdt.DiscountComp1 = DiscountComp1 / pcdt.ChargedQuantity;
                            pcdt.DiscountComp2 = DiscountComp2 / pcdt.ChargedQuantity;
                            pcdt.DiscountComp3 = DiscountComp3 / pcdt.ChargedQuantity;

                            grossLineAmount = pcdt.Tariff * pcdt.ChargedQuantity;
                            totalDiscountAmount = pcdt.DiscountAmount;
                            if (grossLineAmount > 0)
                            {
                                if (totalDiscountAmount > grossLineAmount)
                                {
                                    totalDiscountAmount = grossLineAmount;
                                }
                            }

                            total = grossLineAmount - totalDiscountAmount;
                            totalPayer = 0;
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

                            oPatientAmount = total - totalPayer;
                            oPayerAmount = totalPayer;
                            oLineAmount = total;

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
                            patientChargesDtDao.Update(pcdt);
                        }
                        else if (!entity.IsUsingAccumulatedPrice && entity.IsPackageItem)
                        {
                            foreach (PatientChargesDtPackage e in lstDtPackage)
                            {
                                //ctx.CommandType = CommandType.Text;
                                //ctx.Command.Parameters.Clear();
                                //PatientChargesDtPackage packageDt = entityDtPackageDao.Get(e.ID);
                                PatientChargesDtPackage packageDt = e;
                                if (e.TariffComp1 != 0)
                                {
                                    packageDt.TariffComp1 = ((packageDt.BaseComp1 / lstDtPackage.Sum(t => t.BaseTariff)) * patientChargesDt.Tariff);                                    
                                }
                                if (e.TariffComp2 != 0)
                                {
                                    packageDt.TariffComp2 = ((packageDt.BaseComp2 / lstDtPackage.Sum(t => t.BaseTariff)) * patientChargesDt.Tariff);                                    
                                }
                                if (e.TariffComp3 != 0)
                                {
                                    packageDt.TariffComp3 = ((packageDt.BaseComp3 / lstDtPackage.Sum(t => t.BaseTariff)) * patientChargesDt.Tariff);

                                }

                                packageDt.Tariff = packageDt.TariffComp1 + packageDt.TariffComp2 + packageDt.TariffComp3;

                                if (packageDt.DiscountComp1 != 0)
                                {
                                    packageDt.DiscountComp1 = ((e.Tariff / patientChargesDt.Tariff) * patientChargesDt.DiscountComp1);
                                }
                                else
                                {
                                    packageDt.DiscountComp1 = 0;
                                }

                                if (packageDt.DiscountComp2 != 0)
                                {
                                    packageDt.DiscountComp3 = ((e.Tariff / patientChargesDt.Tariff) * patientChargesDt.DiscountComp2);
                                }
                                else
                                {
                                    packageDt.DiscountComp2 = 0;
                                }

                                if (packageDt.DiscountComp3 != 0)
                                {
                                    packageDt.DiscountComp3 = ((e.Tariff / patientChargesDt.Tariff) * patientChargesDt.DiscountComp3);
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

                        ct++;

                        #endregion
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
    }
}