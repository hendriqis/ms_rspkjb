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
    public partial class LogisticsQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}', '{3}', '{4}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //2
                                                        Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_ALKES_N_BARANGUMUM_QUICKPICK, //3
                                                        Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT //4
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            hdnParam.Value = param;
            hdnTransactionID.Value = hdnParam.Value.Split('|')[0];
            hdnLocationID.Value = hdnParam.Value.Split('|')[1];
            hdnVisitIDCtl.Value = hdnParam.Value.Split('|')[2];
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[3];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[5];
            hdnHealthcareServiceUnitID.Value = hdnParam.Value.Split('|')[6];
            hdnIsAccompany.Value = hdnParam.Value.Split('|')[7];

            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            hdnIsBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

            ConsultVisit visit = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitIDCtl.Value));
            hdnChargesClassID.Value = visit.ChargeClassID.ToString();
            hdnIsDisplayPrice.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_ALKES_N_BARANGUMUM_QUICKPICK).ParameterValue;
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

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField(cboDrugChargeClassIDCtl, lstClassCare, "ClassName", "ClassID");
            if (AppSession.RegisteredPatient.ChargeClassID != 0)
            {
                cboDrugChargeClassIDCtl.Value = Convert.ToString(AppSession.RegisteredPatient.ChargeClassID);
            }
            
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            //List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
            //hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            //hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID.ToString();
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID.ToString();

            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (Convert.ToInt32(hdnLocationID.Value) > 0)
            {
                Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                List<Location> lstLocation = null;
                List<Location> lstLocationSelected = new List<Location>();

                if (loc.IsHeader)
                {
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0} AND IsDeleted = 0", loc.LocationID));

                    List<GetLocationUserList> lstLocationByUser = BusinessLayer.GetLocationUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Empty, string.Empty);
                    if (lstLocationByUser.Count > 0)
                    {
                        foreach (GetLocationUserList locUser in lstLocationByUser)
                        {
                            foreach (Location lc in lstLocation)
                            {
                                if (locUser.LocationID == lc.LocationID)
                                {
                                    lstLocationSelected.Add(lc);
                                }
                            }
                        }

                        Methods.SetComboBoxField<Location>(cboLocation, lstLocationSelected, "LocationName", "LocationID");
                    }
                    else
                    {
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0} AND IsDeleted = 0", loc.LocationID));
                        Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    }

                    cboLocation.SelectedIndex = 0;
                }
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                }
                hdnIsAllowOverissued.Value = loc.IsAllowOverIssued ? "1" : "0";
            }
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
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

            if (hdnItemGroupDrugLogisticID.Value == "")
            {
                filterExpression += string.Format("LocationID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%'", cboLocation.Value, hdnFilterItem.Value);
            }
            else
            {
                filterExpression += string.Format("LocationID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%')", cboLocation.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
            }
            
            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalanceQuickPick entity = e.Row.DataItem as vItemBalanceQuickPick;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                if (entity.QuantityEND == 0)
                    foreColor = System.Drawing.Color.Red;
                e.Row.Cells[2].ForeColor = foreColor;
                e.Row.Cells[3].ForeColor = foreColor;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            string filterExpressionPatientChargesDt = " 1=0";

            if (rblFilterItemType.SelectedValue == "nutrition")
            {
                filterExpression += string.Format(" AND GCItemType = '{0}' AND IsDeleted = 0 AND IsChargeToPatient = 1", Constant.ItemType.BAHAN_MAKANAN);
            }
            else
            {
                filterExpression += string.Format(" AND GCItemType = '{0}' AND IsDeleted = 0 AND IsChargeToPatient = 1", Constant.ItemType.BARANG_UMUM);
            }

            filterExpressionPatientChargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value);

            if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
            {
                List<vPatientChargesDt1> lstItemID = BusinessLayer.GetvPatientChargesDt1List(filterExpressionPatientChargesDt);
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vPatientChargesDt1 itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }

            if (hdnIsAllowOverissued.Value == "0")
            {
                filterExpression += " AND QuantityEND > 0";
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceQuickPickRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemBalanceQuickPick> lstEntity = BusinessLayer.GetvItemBalanceQuickPickList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);

            try
            {
                int registrationID = Convert.ToInt32(hdnParam.Value.Split('|')[3]);
                int visitID = Convert.ToInt32(hdnParam.Value.Split('|')[2]);
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');

                #region Patient Charges Hd
                PatientChargesHd patientChargesHd = null;
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = visitID;
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    if (hdnIsAccompany.Value == "0")
                    {
                        switch (hdnDepartmentID.Value)
                        {
                            case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                            case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
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
                    //to make sure, transaction id is belong to selected patient (visit id)
                    string filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, hdnTransactionID.Value);
                    patientChargesHd = BusinessLayer.GetPatientChargesHdList(filterExpression).FirstOrDefault();
                    //patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
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
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        #region Patient Charges Dt
                        decimal qty = Convert.ToDecimal(lstSelectedMemberQty[ct]);

                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = Convert.ToInt32(itemID);
                        // patientChargesDt.ChargeClassID = DetailPage.GetClassID();
                        patientChargesDt.ChargeClassID = Convert.ToInt32(cboDrugChargeClassIDCtl.Value);

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
                        patientChargesDt.ParamedicID = Convert.ToInt32(DetailPage.GetRegistrationPhysicianID());

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

                        if (totalDiscountAmount > grossLineAmount)
                        {
                            totalDiscountAmount = grossLineAmount;
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

                        ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, patientChargesDt.ItemID), ctx).FirstOrDefault();
                        patientChargesDt.AveragePrice = iPlanning.AveragePrice;
                        patientChargesDt.CostAmount = iPlanning.UnitPrice;

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

                        if (hdnIsEndingAmountRoundingTo100.Value == "1")
                        {
                            oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                            oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                            oLineAmount = oPatientAmount + oPayerAmount;
                        }

                        patientChargesDt.PatientAmount = oPatientAmount;
                        patientChargesDt.PayerAmount = oPayerAmount;
                        patientChargesDt.LineAmount = oLineAmount;

                        patientChargesDt.LocationID = Convert.ToInt32(cboLocation.Value);
                        patientChargesDt.IsApproved = true;
                        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                        patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                        patientChargesDtDao.Insert(patientChargesDt);
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
            return (result);
        }
    }
}