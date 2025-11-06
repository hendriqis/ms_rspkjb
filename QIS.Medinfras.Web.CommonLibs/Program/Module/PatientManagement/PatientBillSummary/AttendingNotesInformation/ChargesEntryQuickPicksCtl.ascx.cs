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
    public partial class ChargesEntryQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private AttendingNotesInformation DetailPage
        {
            get { return (AttendingNotesInformation)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnHealthcareServiceUnitIDCtl.Value = temp[1];
            hdnParamedicIDCtl.Value = temp[2];
            hdnRegistrationIDCtl.Value = temp[3];
            hdnVisitIDCtl.Value = temp[4];
            hdnChargeClassIDCtl.Value = temp[5];
            hdnNoteDateCtl.Value = temp[6];

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            BindCboServiceUnit();
        }

        private void BindCboServiceUnit()
        {
            if (Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value) > 0)
            {
                string filterExpression = string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND IsDeleted = 0 AND IsUsingRegistration = 1", hdnParamedicIDCtl.Value, hdnHealthcareServiceUnitIDCtl.Value);
                List<vServiceUnitParamedic> lstServiceUnit = BusinessLayer.GetvServiceUnitParamedicList(filterExpression);
                Methods.SetComboBoxField<vServiceUnitParamedic>(cboPopupServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboPopupServiceUnit.SelectedIndex = 0;
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
            string filterExpression = string.Format("IsDeleted = 0 AND GCItemStatus = '{0}' AND ItemID IN (SELECT ItemID FROM ItemMaster WHERE IsDeleted = 0 AND GCItemStatus = '{0}' AND GCItemType = '{1}') AND HealthcareServiceUnitID = {2}", Constant.ItemStatus.ACTIVE, Constant.ItemType.PELAYANAN, hdnHealthcareServiceUnitIDCtl.Value);

            List<vPhysicianItem> lstPhysicianItem = BusinessLayer.GetvPhysicianItemList(string.Format("ParamedicID = {0}", hdnParamedicIDCtl.Value));
            if (lstPhysicianItem.Count > 0)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("ItemID IN (SELECT ItemID FROM vPhysicianItem WHERE ParamedicID = {0})", hdnParamedicIDCtl.Value);
            }

            filterExpression += string.Format(" AND ItemGroupID = (SELECT ParameterValue FROM SettingParameterDt WHERE ParameterCode = '{0}')", Constant.SettingParameter.IP_ITEM_GROUP_ID_HONOR_VISITE);
            
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

                        txtUnitPrice.Text = obj.Price.ToString("N2");
                        txtItemDiscount.Text = obj.DiscountAmount.ToString("N2");

                    }
                }

                System.Drawing.Color specialItemColor = System.Drawing.Color.Black;
                System.Drawing.Color allowVariableColor = System.Drawing.Color.Black;
                if (entity.IsSpecialItem)
                    specialItemColor = System.Drawing.Color.Blue;
                if (!entity.IsAllowVariable)
                    allowVariableColor = System.Drawing.Color.Red;
                e.Row.Cells[2].ForeColor = specialItemColor;
                e.Row.Cells[3].ForeColor = allowVariableColor;

            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            try
            {
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
                        filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                    }
                }

                //filterExpression += string.Format(" AND ParamedicID = {0} ", AppSession.UserLogin.ParamedicID);

                filterExpression += string.Format(" AND ItemName1 LIKE '%{0}%'", hdnFilterItem.Value);

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

                    patientChargesHd.VisitID = Convert.ToInt32(hdnVisitIDCtl.Value);
                    patientChargesHd.TransactionDate = Helper.GetDatePickerValue(hdnNoteDateCtl.Value);
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
                            entityChargesDt.ItemID = Convert.ToInt16(itemID);
                            entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                            entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassIDCtl.Value);

                            vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == entityChargesDt.ItemID);

                            decimal entryAmount = Convert.ToDecimal(lstSelectedMemberUnitPrice[ct]);

                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationIDCtl.Value), Convert.ToInt32(hdnVisitIDCtl.Value), entityChargesDt.ChargeClassID, entityChargesDt.ItemID, 1, DateTime.Now, ctx);

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
                                entityChargesDt.BaseComp1 = basePriceComp1;
                                entityChargesDt.BaseComp2 = basePriceComp2;
                                entityChargesDt.BaseComp3 = basePriceComp3;
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

                            decimal totalDiscountAmount = 0;
                            decimal qty = entityChargesDt.BaseQuantity;
                            decimal grossLineAmount = (qty * price);
                            if (lstSelectedMemberIsDiscount[ct] == "1")
                            {
                                isDiscountInPercentage = true;
                                entityChargesDt.IsDiscount = true;
                                discountAmount = Convert.ToDecimal(lstSelectedMemberDiscount[ct]);
                            }

                            if (isDiscountInPercentage)
                                totalDiscountAmount = grossLineAmount * discountAmount / 100;
                            else
                                totalDiscountAmount = discountAmount * 1;

                            totalDiscountAmount = totalDiscountAmount * qty;

                            if (totalDiscountAmount > grossLineAmount)
                            {
                                if (grossLineAmount > 0)
                                {
                                    totalDiscountAmount = grossLineAmount;
                                }
                            }

                            entityChargesDt.DiscountAmount = totalDiscountAmount;
                            entityChargesDt.DiscountComp1 = 0;
                            entityChargesDt.DiscountComp2 = totalDiscountAmount;
                            entityChargesDt.DiscountComp3 = 0;

                            decimal total = grossLineAmount - totalDiscountAmount;
                            decimal totalPayer = 0;

                            if (isCoverageInPercentage)
                                totalPayer = total * coverageAmount / 100;
                            else
                                totalPayer = coverageAmount * qty;
                            if (total > 0 && totalPayer > total)
                                totalPayer = total;

                            entityChargesDt.PatientAmount = total - totalPayer;
                            entityChargesDt.PayerAmount = totalPayer;
                            entityChargesDt.LineAmount = total;
                            entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;

                            GetItemRevenueSharing rv = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityChargesDt.ParamedicID, entityChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(hdnVisitIDCtl.Value), patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault();

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