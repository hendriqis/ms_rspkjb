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
    public partial class ChargesEntryQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private PatientChargesList DetailPage
        {
            get { return (PatientChargesList)Page; }
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
                                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                            AppSession.UserLogin.HealthcareID, //0
                                                                            Constant.SettingParameter.OP_LONG_CONSULTATION_MINUTES, //1
                                                                            Constant.SettingParameter.FN_DISKON_DOKTER_KOMPONEN_2, //2
                                                                            Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY, //3
                                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //4
                                                                        ));
            hdnLongConsultationMinutes.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_LONG_CONSULTATION_MINUTES).ParameterValue;
            hdnIsAllowChangeChargesQty.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY).ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            BindCboServiceUnit();
        }

        private void BindCboServiceUnit()
        {
            if (Convert.ToInt32(hdnServiceUnitID.Value) > 0)
            {
                string filterExpression = string.Format("ParamedicID = {0} AND ((DepartmentID = '{1}' AND HealthcareServiceUnitID = {2}) OR DepartmentID IN ('3'))",AppSession.UserLogin.ParamedicID,AppSession.RegisteredPatient.DepartmentID,AppSession.RegisteredPatient.HealthcareServiceUnitID,Constant.Facility.DIAGNOSTIC);
                List<vServiceUnitParamedic> lstServiceUnit = BusinessLayer.GetvServiceUnitParamedicList(filterExpression);
                Methods.SetComboBoxField<vServiceUnitParamedic>(cboPopupServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
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
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND GCItemStatus != '{3}' ", cboPopupServiceUnit.Value, hdnFilterItem.Value, hdnItemGroupID.Value, Constant.ItemStatus.IN_ACTIVE);

                List<vPhysicianItem> lstPhysicianItem = BusinessLayer.GetvPhysicianItemList(string.Format("ParamedicID = {0}", AppSession.UserLogin.ParamedicID));
                if (lstPhysicianItem.Count > 0)
                {
                    filterExpression += string.Format("AND ItemID IN (SELECT ItemID FROM vPhysicianItem WHERE ParamedicID = {0})", AppSession.UserLogin.ParamedicID);
                }

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    if (hdnLongConsultationMinutes.Value != "0")
                    {
                        //Dihitung dari dokter mulai mengisi chief complaint
                        ChiefComplaint oChiefComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}",AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                        if (oChiefComplaint != null)
                        {
                            int minute = Convert.ToInt32(oChiefComplaint.ObservationTime.Substring(3));
                            int hour = Convert.ToInt32(oChiefComplaint.ObservationTime.Substring(0, 2));
                            DateTime startServiceDateTime = new DateTime(oChiefComplaint.ObservationDate.Year, oChiefComplaint.ObservationDate.Month, oChiefComplaint.ObservationDate.Day, hour, minute, 0);
                            double totalHours = (DateTime.Now - startServiceDateTime).TotalHours;

                            double minutes = Math.Floor((totalHours) * 60);

                            if (minutes < Convert.ToDouble(hdnLongConsultationMinutes.Value))
                                filterExpression += " AND IsSpecialItem=0";
                        }

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

                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                if (entity.IsSpecialItem)
                    foreColor = System.Drawing.Color.Blue;
                e.Row.Cells[2].ForeColor = foreColor;
                
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
                    List<vPatientChargesDt> lstItemID = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value));
                    string lstSelectedID = "";
                    if (lstItemID.Count > 0)
                    {
                        foreach (vPatientChargesDt itm in lstItemID)
                        {
                            lstSelectedID += "," + itm.ItemID;
                        }
                        filterExpression += string.Format("", lstSelectedID.Substring(1));
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
            catch (Exception ex)
            {                
                throw new Exception(string.Format("An error was occured on data binding process (Datagrid Binding) /n {0}",ex.Message));
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberUnit = hdnSelectedMemberUnit.Value.Split(',');
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

                    vHealthcareServiceUnit oServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}",cboPopupServiceUnit.Value.ToString())).FirstOrDefault();
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

                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityChargesDt.ChargeClassID, entityChargesDt.ItemID,1, DateTime.Now, ctx);

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

                            entityChargesDt.BaseComp1 = entityChargesDt.BaseTariff = basePrice;
                            entityChargesDt.BaseComp2 = entityChargesDt.BaseComp3 = 0;
                            entityChargesDt.TariffComp1 = entityChargesDt.Tariff = price;
                            entityChargesDt.TariffComp2 = entityChargesDt.TariffComp3 = 0;

                            entityChargesDt.BaseQuantity = entityChargesDt.ChargedQuantity = entityChargesDt.UsedQuantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                            entityChargesDt.GCBaseUnit = entityChargesDt.GCItemUnit = lstSelectedMemberUnit[ct];
                            entityChargesDt.ConversionFactor = 1;

                            decimal qty = entityChargesDt.BaseQuantity;
                            decimal grossLineAmount = (qty * price);

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
                                            entityChargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                            entityChargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                            entityChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                        entityChargesDt.DiscountPercentageComp1 = discountAmount;
                                    }

                                    if (priceComp2 > 0)
                                    {
                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                        entityChargesDt.DiscountPercentageComp2 = discountAmount;
                                    }

                                    if (priceComp3 > 0)
                                    {
                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                        entityChargesDt.DiscountPercentageComp3 = discountAmount;
                                    }
                                }

                                if (entityChargesDt.DiscountPercentageComp1 > 0)
                                {
                                    entityChargesDt.IsDiscountInPercentageComp1 = true;
                                }

                                if (entityChargesDt.DiscountPercentageComp2 > 0)
                                {
                                    entityChargesDt.IsDiscountInPercentageComp2 = true;
                                }

                                if (entityChargesDt.DiscountPercentageComp3 > 0)
                                {
                                    entityChargesDt.IsDiscountInPercentageComp3 = true;
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

                            entityChargesDt.IsDiscount = totalDiscountAmount != 0;
                            entityChargesDt.DiscountAmount = totalDiscountAmount;
                            entityChargesDt.DiscountComp1 = totalDiscountAmount1;
                            entityChargesDt.DiscountComp2 = totalDiscountAmount2;
                            entityChargesDt.DiscountComp3 = totalDiscountAmount3;

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

                            entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;

                            //entityChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityChargesDt.ParamedicID, entityChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitID.Value), patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;

                            GetItemRevenueSharing rv = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityChargesDt.ParamedicID, entityChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitID.Value), patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault();

                            if (rv.RevenueSharingID != null && rv.RevenueSharingID != 0)
                            {
                                int revenueSharingiD = rv.RevenueSharingID;
                                if (revenueSharingiD > 0)
                                {
                                    entityChargesDt.RevenueSharingID = revenueSharingiD;
                                }
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                            #endregion
                            
                            entityChargesDt.TransactionID = transactionID;
                            entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityChargesDtDao.Insert(entityChargesDt);

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