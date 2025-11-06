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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ChargesEntryQuickPicksCtl1 : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private PatientChargesList DetailPage
        {
            get { return (PatientChargesList)Page; }
        }

        public String IsAllowPreviewTariffQPCtl()
        {
            return hdnIsAllowPreviewTariffQPCtl.Value;
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnServiceUnitID.Value = temp[1];
            hdnParamedicID.Value = temp[2];
            hdnRegistrationID.Value = temp[3];
            hdnVisitID.Value = temp[4];
            hdnChargeClassID.Value = temp[5];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                                        AppSession.UserLogin.HealthcareID, //0
                                        Constant.SettingParameter.OP_LONG_CONSULTATION_MINUTES, //1
                                        Constant.SettingParameter.FN_DISKON_DOKTER_KOMPONEN_2, //2
                                        Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY, //3
                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //4
                                        Constant.SettingParameter.EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF //5
                                    ));
            hdnLongConsultationMinutes.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_LONG_CONSULTATION_MINUTES).ParameterValue;
            hdnIsDiscountApplyToTariffComp2Only.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_DISKON_DOKTER_KOMPONEN_2).ParameterValue;
            hdnIsAllowChangeChargesQty.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY).ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnIsAllowPreviewTariffQPCtl.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF).FirstOrDefault().ParameterValue;

            BindCboServiceUnit();
            BindGridView(1, true, ref PageCount);
        }

        private void BindCboServiceUnit()
        {
            if (Convert.ToInt32(hdnServiceUnitID.Value) > 0)
            {
                string filterExpression = string.Format("((DepartmentID = '{1}' AND HealthcareServiceUnitID = {2}) OR DepartmentID IN ('3'))", AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.DepartmentID, AppSession.RegisteredPatient.HealthcareServiceUnitID, Constant.Facility.DIAGNOSTIC);
                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboPopupServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboPopupServiceUnit.SelectedIndex = 0;
                BindCboLocation();
            }
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
            hdnLocationID.Value = cboLocation.Value.ToString();
        }

        private void BindCboLocation()
        {
            if (cboPopupServiceUnit.Value != null)
            {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0}) OR LocationID IN (SELECT LogisticLocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboPopupServiceUnit.Value)).FirstOrDefault();

                if (location != null)
                {
                    int locationID = location.LocationID;
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                }
            }
        }

        protected void cboPopupServiceUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboServiceUnit();
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            string itemType = rblItemType.SelectedValue;

            if (itemType == "1")
            {
                if (hdnItemGroupID.Value == "")
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND GCItemStatus != '{2}' ", cboPopupServiceUnit.Value, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);
                else
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND GCItemStatus != '{3}' ", cboPopupServiceUnit.Value, hdnFilterItem.Value, hdnItemGroupID.Value, Constant.ItemStatus.IN_ACTIVE);

                List<PhysicianItem> lstPhysicianItem = BusinessLayer.GetPhysicianItemList(string.Format("ParamedicID = {0}", AppSession.UserLogin.ParamedicID));
                if (lstPhysicianItem.Count > 0)
                {
                    //Item Khusus Dokter
                    filterExpression += string.Format("AND ItemID IN (SELECT ItemID FROM PhysicianItem  WITH (NOLOCK) WHERE ParamedicID = {0})", AppSession.UserLogin.ParamedicID);
                }

                //Restriksi Item
                filterExpression += string.Format("AND ItemID NOT IN (SELECT ItemID FROM ExclusionPhysicianItem WITH (NOLOCK) WHERE ParamedicID = {0})", AppSession.UserLogin.ParamedicID);

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    if (hdnLongConsultationMinutes.Value != "0")
                    {
                        int minute = Convert.ToInt32(AppSession.RegisteredPatient.StartServiceTime.Substring(3));
                        int hour = Convert.ToInt32(AppSession.RegisteredPatient.StartServiceTime.Substring(0, 2));
                        DateTime startServiceDateTime = new DateTime(AppSession.RegisteredPatient.StartServiceDate.Year, AppSession.RegisteredPatient.StartServiceDate.Month, AppSession.RegisteredPatient.StartServiceDate.Day, hour, minute, 0);
                        double totalHours = (DateTime.Now - startServiceDateTime).TotalHours;

                        double minutes = Math.Floor((totalHours) * 60);

                        if (minutes < Convert.ToDouble(hdnLongConsultationMinutes.Value))
                            filterExpression += " AND IsSpecialItem=0";
                    }
                }
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
                    chkIsSelected.Checked = true;

                vServiceUnitItem item = (vServiceUnitItem)e.Row.DataItem;
                Label txtUnitPrice = (Label)e.Row.FindControl("txtItemTariff");
                Label txtItemDiscount = (Label)e.Row.FindControl("txtItemDiscount");
                if (item != null && txtUnitPrice != null)
                {
                    txtUnitPrice.Enabled = item.IsAllowVariable;

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID, item.ItemID, 1, DateTime.Now);

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

                    decimal discPreview = discountAmount;

                    if (isDiscountInPercentageComp2)
                    {
                        if (isDiscountUsedComp)
                        {
                            if (priceComp2 > 0)
                            {
                                discPreview = discountAmountComp2;
                            }
                        }
                    }

                    txtUnitPrice.Text = price.ToString("N2");
                    txtItemDiscount.Text = discPreview.ToString("N2");
                }

                System.Drawing.Color specialItemColor = System.Drawing.Color.Black;
                System.Drawing.Color allowVariableColor = System.Drawing.Color.Black;
                if (entity.IsSpecialItem)
                    specialItemColor = System.Drawing.Color.Blue;
                if (!entity.IsAllowVariable)
                    allowVariableColor = System.Drawing.Color.Red;
                e.Row.Cells[2].ForeColor = specialItemColor;
                e.Row.Cells[3].ForeColor = allowVariableColor;

                if (hdnIsAllowChangeChargesQty.Value == "1")
                {
                    if (entity.IsQtyAllowChangeForDoctor)
                    {
                        txtQty.Disabled = false;
                    }
                    else
                    {
                        txtQty.Disabled = true;
                    }
                }
                else
                {
                    txtQty.Disabled = true;
                }

                if (hdnIsAllowPreviewTariffQPCtl.Value == "0")
                {
                    txtUnitPrice.Attributes.Add("style", "display:none");
                    txtItemDiscount.Attributes.Add("style", "display:none");
                }
                else
                {
                    txtUnitPrice.Attributes.Remove("style");
                    txtItemDiscount.Attributes.Remove("style");
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            try
            {
                string itemType = rblItemType.SelectedValue;
                string filterExpression = GetFilterExpression();
                if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
                {
                    List<PatientChargesDt> lstItemID = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value));
                    string lstSelectedID = "";
                    if (lstItemID.Count > 0)
                    {
                        foreach (PatientChargesDt itm in lstItemID)
                        {
                            lstSelectedID += "," + itm.ItemID;
                        }
                        filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                    }
                }

                //filterExpression += string.Format(" AND ParamedicID = {0} ", AppSession.UserLogin.ParamedicID);

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
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error was occured on data binding process (Datagrid Binding) /n {0}", ex.Message));
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao patientChargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberUnit = hdnSelectedMemberUnit.Value.Split(',');
                string[] lstSelectedMemberIsDiscount = hdnSelectedMemberIsDiscount.Value.Split(',');
                string[] lstSelectedMemberDiscount = hdnSelectedMemberDiscount.Value.Split(',');
                string[] lstSelectedMemberIsVariable = hdnSelectedMemberIsVariable.Value.Split(',');
                string[] lstSelectedMemberUnitPrice = hdnSelectedMemberUnitPrice.Value.Split(',');

                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                bool isNewHeader = false;

                PatientChargesHd patientChargesHd = null;

                if (transactionID > 0)
                {
                    patientChargesHd = patientChargesHdDao.Get(transactionID);
                    if (patientChargesHd.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                        isNewHeader = true;
                }
                else
                {
                    isNewHeader = true;
                }

                if (isNewHeader)
                {
                    //Transaction is processed --> generate new header

                    patientChargesHd = new PatientChargesHd();

                    #region new PatientChargesHd
                    patientChargesHd = new PatientChargesHd();

                    patientChargesHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    patientChargesHd.TransactionDate = DateTime.Now.Date;
                    patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(cboPopupServiceUnit.Value.ToString());
                    patientChargesHd.IsEntryByPhysician = true;

                    vHealthcareServiceUnit oServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", cboPopupServiceUnit.Value.ToString())).FirstOrDefault();
                    switch (oServiceUnit.DepartmentID)
                    {
                        case Constant.Facility.EMERGENCY:
                            patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
                            break;
                        case Constant.Facility.OUTPATIENT:
                            patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
                            break;
                        case Constant.Facility.INPATIENT:
                            patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES;
                            break;
                        case Constant.Facility.PHARMACY:
                            patientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                            break;
                        case Constant.Facility.MEDICAL_CHECKUP:
                            patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES;
                            break;
                        case Constant.Facility.DIAGNOSTIC:
                            patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                            break;
                    }
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    transactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    #endregion
                }

                if (patientChargesHd != null)
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                        int ct = 0;
                        foreach (String itemID in lstSelectedMember)
                        {
                            #region PatientChargesDt

                            PatientChargesDt entityChargesDt = new PatientChargesDt();
                            entityChargesDt.ItemID = Convert.ToInt32(itemID);
                            entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                            entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);

                            vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == entityChargesDt.ItemID);

                            decimal entryAmount = Convert.ToDecimal(lstSelectedMemberUnitPrice[ct]);

                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityChargesDt.ChargeClassID, entityChargesDt.ItemID, 1, DateTime.Now, ctx);

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

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            if (lstSelectedMemberIsVariable[ct] == "1")
                            {
                                decimal factor = 0;

                                if (price != 0)
                                {
                                    factor = Math.Round(entryAmount / price, 2);
                                }
                                else
                                {
                                    factor = Math.Round(entryAmount, 2);
                                }

                                entityChargesDt.IsVariable = true;
                                entityChargesDt.BaseTariff = basePrice;
                                entityChargesDt.BaseComp1 = factor * basePriceComp1;
                                entityChargesDt.BaseComp2 = factor * basePriceComp2;
                                entityChargesDt.BaseComp3 = factor * basePriceComp3;
                                entityChargesDt.Tariff = entryAmount;
                                entityChargesDt.TariffComp1 = factor * priceComp1;
                                entityChargesDt.TariffComp2 = factor * priceComp2;
                                entityChargesDt.TariffComp3 = factor * priceComp3;

                                price = entryAmount;
                            }
                            else
                            {
                                entityChargesDt.IsVariable = false;
                                entityChargesDt.BaseTariff = basePrice;
                                entityChargesDt.BaseComp1 = basePriceComp1;
                                entityChargesDt.BaseComp2 = basePriceComp2;
                                entityChargesDt.BaseComp3 = basePriceComp3;
                                entityChargesDt.Tariff = price;
                                entityChargesDt.TariffComp1 = priceComp1;
                                entityChargesDt.TariffComp2 = priceComp2;
                                entityChargesDt.TariffComp3 = priceComp3;
                            }

                            entityChargesDt.BaseQuantity = entityChargesDt.ChargedQuantity = entityChargesDt.UsedQuantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                            entityChargesDt.GCBaseUnit = entityChargesDt.GCItemUnit = lstSelectedMemberUnit[ct];
                            entityChargesDt.ConversionFactor = 1;

                            decimal qty = entityChargesDt.ChargedQuantity;
                            decimal grossLineAmount = (qty * price);

                            if (lstSelectedMemberIsDiscount[ct] == "1")
                            {
                                isDiscountInPercentage = true;
                                entityChargesDt.IsDiscount = true;
                                discountAmount = Convert.ToDecimal(lstSelectedMemberDiscount[ct]);
                            }

                            if (entityChargesDt.TariffComp1 > 0)
                            {
                                if (isDiscountUsedComp)
                                {
                                    if (isDiscountInPercentageComp1)
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp1 = true;
                                        entityChargesDt.DiscountPercentageComp1 = discountAmountComp1;
                                        entityChargesDt.DiscountComp1 = Math.Round((entityChargesDt.TariffComp1 * discountAmountComp1 / 100), 2);
                                    }
                                    else
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp1 = false;
                                        entityChargesDt.DiscountPercentageComp1 = 0;
                                        entityChargesDt.DiscountComp1 = discountAmountComp1;
                                    }
                                }
                                else
                                {
                                    entityChargesDt.DiscountComp1 = Math.Round((entityChargesDt.TariffComp1 * discountAmount / 100), 2);

                                    if (isDiscountInPercentage)
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp1 = true;
                                        entityChargesDt.DiscountPercentageComp1 = discountAmount;
                                    }
                                }
                            }

                            if (entityChargesDt.TariffComp2 > 0)
                            {
                                if (isDiscountUsedComp)
                                {
                                    if (isDiscountInPercentageComp2)
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp2 = true;
                                        entityChargesDt.DiscountPercentageComp2 = discountAmountComp2;
                                        entityChargesDt.DiscountComp2 = Math.Round((entityChargesDt.TariffComp2 * discountAmountComp2 / 100), 2);
                                    }
                                    else
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp2 = false;
                                        entityChargesDt.DiscountPercentageComp2 = 0;
                                        entityChargesDt.DiscountComp2 = discountAmountComp2;
                                    }
                                }
                                else
                                {
                                    entityChargesDt.DiscountComp2 = Math.Round((entityChargesDt.TariffComp2 * discountAmount / 100), 2);

                                    if (isDiscountInPercentage)
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp2 = true;
                                        entityChargesDt.DiscountPercentageComp2 = discountAmount;
                                    }
                                }
                            }

                            if (entityChargesDt.TariffComp3 > 0)
                            {
                                if (isDiscountUsedComp)
                                {
                                    if (isDiscountInPercentageComp3)
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp3 = true;
                                        entityChargesDt.DiscountPercentageComp3 = discountAmountComp3;
                                        entityChargesDt.DiscountComp3 = Math.Round((entityChargesDt.TariffComp3 * discountAmountComp3 / 100), 2);
                                    }
                                    else
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp3 = false;
                                        entityChargesDt.DiscountPercentageComp3 = 0;
                                        entityChargesDt.DiscountComp3 = discountAmountComp3;
                                    }
                                }
                                else
                                {
                                    entityChargesDt.DiscountComp3 = Math.Round((entityChargesDt.TariffComp3 * discountAmount / 100), 2);

                                    if (isDiscountInPercentage)
                                    {
                                        entityChargesDt.IsDiscountInPercentageComp3 = true;
                                        entityChargesDt.DiscountPercentageComp3 = discountAmount;
                                    }
                                }
                            }

                            if (hdnIsDiscountApplyToTariffComp2Only.Value == "1")
                            {
                                entityChargesDt.DiscountComp1 = 0;
                                entityChargesDt.DiscountPercentageComp1 = 0;
                                entityChargesDt.DiscountComp3 = 0;
                                entityChargesDt.DiscountPercentageComp3 = 0;
                            }

                            entityChargesDt.DiscountAmount = (entityChargesDt.DiscountComp1 + entityChargesDt.DiscountComp2 + entityChargesDt.DiscountComp3) * entityChargesDt.ChargedQuantity;

                            decimal total = grossLineAmount - entityChargesDt.DiscountAmount;

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

                            entityChargesDt.PatientAmount = oPatientAmount;
                            entityChargesDt.PayerAmount = oPayerAmount;
                            entityChargesDt.LineAmount = oLineAmount;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityChargesDt.ParamedicID, entityChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitID.Value), patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;

                            if (entityChargesDt.RevenueSharingID == 0)
                                entityChargesDt.RevenueSharingID = null;

                            entityChargesDt.TransactionID = transactionID;
                            entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                            entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            int oChargesDtID = entityChargesDtDao.InsertReturnPrimaryKeyID(entityChargesDt);

                            string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", entityChargesDt.ParamedicID, DateTime.Now);
                            List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam, ctx);
                            foreach (ParamedicMasterTeam pmt in pmtList)
                            {
                                PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                                dtparamedic.ID = oChargesDtID;
                                dtparamedic.ItemID = entityChargesDt.ItemID;
                                dtparamedic.ParamedicID = pmt.ParamedicID;
                                dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                                dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                patientChargesDtParamedicDao.Insert(dtparamedic);
                            }

                            decimal totalDiscountAmount = 0;
                            decimal totalDiscountAmount1 = 0;
                            decimal totalDiscountAmount2 = 0;
                            decimal totalDiscountAmount3 = 0;

                            int countInventoryItemDetail = 0;
                            List<PatientChargesDtPackage> lstDtPackage = new List<PatientChargesDtPackage>();
                            string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", entityChargesDt.ItemID);
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
                                dtpackage.ParamedicID = entityChargesDt.ParamedicID;

                                int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, entityChargesDt.ParamedicID, entityChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitID.Value), patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                if (revID != 0 && revID != null)
                                {
                                    dtpackage.RevenueSharingID = revID;
                                }
                                else
                                {
                                    dtpackage.RevenueSharingID = null;
                                }

                                dtpackage.ChargedQuantity = (isd.Quantity * entityChargesDt.ChargedQuantity);

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

                                int itemType = isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                                GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, entityChargesDt.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

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
                                dtpackage.ID = entityDtPackageDao.InsertReturnPrimaryKeyID(dtpackage);

                                lstDtPackage.Add(dtpackage);
                            }

                            if (countInventoryItemDetail > 0)
                            {
                                HealthcareServiceUnit chargesHSU = chargesHSUDao.Get(patientChargesHd.HealthcareServiceUnitID);

                                PatientChargesDt pcdt = entityChargesDtDao.Get(oChargesDtID);
                                pcdt.LocationID = chargesHSU.LocationID;
                                pcdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityChargesDtDao.Update(pcdt);
                            }

                            if (entity.IsUsingAccumulatedPrice && entity.IsPackageItem)
                            {
                                PatientChargesDt pcdt = entityChargesDtDao.Get(oChargesDtID);

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
                                entityChargesDtDao.Update(pcdt);
                            }
                            else if (!entity.IsUsingAccumulatedPrice && entity.IsPackageItem)
                            {
                                foreach (PatientChargesDtPackage e in lstDtPackage)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    PatientChargesDtPackage packageDt = entityDtPackageDao.Get(e.ID);
                                    if (e.TariffComp1 != 0)
                                    {
                                        packageDt.TariffComp1 = ((packageDt.BaseComp1 / lstDtPackage.Sum(t => t.BaseTariff)) * entityChargesDt.Tariff);
                                        packageDt.DiscountComp1 = ((e.Tariff / entityChargesDt.Tariff) * entityChargesDt.DiscountComp1);
                                    }
                                    if (e.TariffComp2 != 0)
                                    {
                                        packageDt.TariffComp2 = ((packageDt.BaseComp2 / lstDtPackage.Sum(t => t.BaseTariff)) * entityChargesDt.Tariff);
                                        packageDt.DiscountComp2 = ((e.Tariff / entityChargesDt.Tariff) * entityChargesDt.DiscountComp2);
                                    }
                                    if (e.TariffComp3 != 0)
                                    {
                                        packageDt.TariffComp3 = ((packageDt.BaseComp3 / lstDtPackage.Sum(t => t.BaseTariff)) * entityChargesDt.Tariff);
                                        packageDt.DiscountComp3 = ((e.Tariff / entityChargesDt.Tariff) * entityChargesDt.DiscountComp3);
                                    }

                                    packageDt.Tariff = packageDt.TariffComp1 + packageDt.TariffComp2 + packageDt.TariffComp3;
                                    packageDt.DiscountAmount = ((packageDt.DiscountComp1 + packageDt.DiscountComp2 + packageDt.DiscountComp3) * packageDt.ChargedQuantity);
                                    packageDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityDtPackageDao.Update(packageDt);
                                }
                            }

                            #endregion

                            ct++;
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                        result = false;
                    }
                }
                retval = transactionID.ToString();
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