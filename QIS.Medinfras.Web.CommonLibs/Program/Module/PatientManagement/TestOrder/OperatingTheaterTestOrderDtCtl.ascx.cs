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
    public partial class OperatingTheaterTestOrderDtCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private decimal controlTariff = 0;

        public override void InitializeDataControl(string param)
        {
            hdnTestOrderID.Value = param;

            vTestOrderHdVisit orderHd = BusinessLayer.GetvTestOrderHdVisitList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value)).FirstOrDefault();
            hdnProcedureGroupID.Value = orderHd.ProcedureGroupID.ToString();
            hdnHealthcareServiceUnitID.Value = orderHd.HealthcareServiceUnitID.ToString();
            hdnRegistrationID.Value = orderHd.RegistrationID.ToString();
            hdnVisitID.Value = orderHd.VisitID.ToString();
            hdnChargesClassID.Value = orderHd.ChargeClassID.ToString();

            txtTransactionNo.Text = orderHd.TestOrderNo;
            txtTestOrderDate.Text = orderHd.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTestOrderTime.Text = orderHd.TestOrderTime;

            txtOrderUser.Text = orderHd.CreatedBy;
            txtOrderPhysicianName.Text = string.Format("{0} ({1})", orderHd.ParamedicName, orderHd.ParamedicCode);
            txtNotes.Text = orderHd.Remarks;

            txtRealizationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRealizationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            string filterCharges = string.Format("HealthcareServiceUnitID = {0} AND TestorderID = {1} AND GCTransactionStatus = '{2}'", hdnHealthcareServiceUnitID.Value, hdnTestOrderID.Value, Constant.TransactionStatus.OPEN);
            PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges).FirstOrDefault();
            if (patientChargesHd != null)
            {
                hdnTransactionID.Value = patientChargesHd.TransactionID.ToString();
            }

            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

            filterExpression = string.Format("ProcedurePanelID = {0} AND IsDeleted = 0", hdnProcedurePanelID.Value);

            filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND IsTestItem = 1)", hdnHealthcareServiceUnitID.Value, hdnFilterItem.Value);

            filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM ItemMaster WHERE GCItemStatus != '{0}')", Constant.ItemStatus.IN_ACTIVE);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
            {
                List<vTestOrderDt> lstItemID = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value));
                if (lstItemID.Count > 0)
                {
                    string lstSelectedID = "";
                    foreach (vTestOrderDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvProcedurePanelDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }
            lstSelectedMember = hdnSelectedMemberItemID.Value.Split(',');

            List<vProcedurePanelDt> lstEntity = BusinessLayer.GetvProcedurePanelDtList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "DisplayOrder ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vProcedurePanelDt entity = e.Row.DataItem as vProcedurePanelDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                {
                    chkIsSelected.Checked = true;
                }

                vProcedurePanelDt item = (vProcedurePanelDt)e.Row.DataItem;
                Label txtItemTariff = (Label)e.Row.FindControl("txtItemTariff");
                Label txtFormulaPercent = (Label)e.Row.FindControl("txtFormulaPercent");
                Label txtItemEndTariff = (Label)e.Row.FindControl("txtItemEndTariff");

                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                int chargeClassID = Convert.ToInt32(hdnChargesClassID.Value);

                if (item != null && txtItemTariff != null)
                {
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, chargeClassID, item.ItemID, 1, DateTime.Now);
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        txtItemTariff.Text = obj.Price.ToString(Constant.FormatString.NUMERIC_2);

                        if (item.IsControlItem)
                        {
                            controlTariff = obj.Price;
                        }
                    }

                    txtFormulaPercent.Text = item.FormulaPercentage.ToString(Constant.FormatString.NUMERIC_2);

                    decimal endTariff = controlTariff * item.FormulaPercentage / 100;
                    txtItemEndTariff.Text = endTariff.ToString(Constant.FormatString.NUMERIC_2);

                    if (item.IsControlItem)
                    {
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                    }
                }

            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);

            lstSelectedMember = hdnSelectedMemberItemID.Value.Split(',');
            string[] lstSelectedMemberTariff = hdnSelectedMemberItemTariff.Value.Split(',');
            string[] lstSelectedMemberIsControlItem = hdnSelectedMemberIsControlItem.Value.Split(',');

            try
            {
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                int chargeClassID = Convert.ToInt32(hdnChargesClassID.Value);

                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                int ct = 0;

                foreach (String itemID in lstSelectedMember)
                {
                    PatientChargesDt patientChargesDt = new PatientChargesDt();

                    patientChargesDt.ItemID = Convert.ToInt32(itemID);
                    patientChargesDt.ChargeClassID = Convert.ToInt32(hdnChargesClassID.Value);

                    decimal discountAmount = 0;
                    decimal coverageAmount = 0;
                    bool isCoverageInPercentage = false;
                    bool isDiscountInPercentage = false;
                    decimal basePrice = 0;
                    decimal baseComp1 = 0;
                    decimal baseComp2 = 0;
                    decimal baseComp3 = 0;
                    decimal price = 0;
                    decimal tariffComp1 = 0;
                    decimal tariffComp2 = 0;
                    decimal tariffComp3 = 0;

                    if (lstSelectedMemberIsControlItem[ct] == "True")
                    {
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, chargeClassID, Convert.ToInt32(itemID), 1, DateTime.Now, ctx);

                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff obj = list[0];
                            discountAmount = obj.DiscountAmount;
                            coverageAmount = obj.CoverageAmount;
                            basePrice = obj.BasePrice;
                            baseComp1 = obj.BasePriceComp1;
                            baseComp2 = obj.BasePriceComp2;
                            baseComp3 = obj.BasePriceComp3;
                            price = obj.Price;
                            tariffComp1 = obj.PriceComp1;
                            tariffComp2 = obj.PriceComp2;
                            tariffComp3 = obj.PriceComp3;
                            isCoverageInPercentage = obj.IsCoverageInPercentage;
                            isDiscountInPercentage = obj.IsDiscountInPercentage;
                        }
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                    }
                    else
                    {
                        basePrice = Convert.ToDecimal(lstSelectedMemberTariff[ct]);
                        baseComp2 = Convert.ToDecimal(lstSelectedMemberTariff[ct]);
                        price = Convert.ToDecimal(lstSelectedMemberTariff[ct]);
                        tariffComp2 = Convert.ToDecimal(lstSelectedMemberTariff[ct]);
                    }

                    patientChargesDt.BaseTariff = basePrice;
                    patientChargesDt.BaseComp1 = baseComp1;
                    patientChargesDt.BaseComp2 = baseComp2;
                    patientChargesDt.BaseComp3 = baseComp3;
                    patientChargesDt.Tariff = price;
                    patientChargesDt.TariffComp1 = tariffComp1;
                    patientChargesDt.TariffComp2 = tariffComp2;
                    patientChargesDt.TariffComp3 = tariffComp3;

                    vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", itemID), ctx).FirstOrDefault();
                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;

                    patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianIDOT.Value);

                    if (patientChargesDt.BusinessPartnerID != null)
                    {
                        patientChargesDt.IsSubContractItem = true;
                    }
                    else
                    {
                        patientChargesDt.IsSubContractItem = false;
                    }

                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Helper.GetDatePickerValue(txtRealizationDate.Text), txtRealizationTime.Text).FirstOrDefault().RevenueSharingID;
                    if (patientChargesDt.RevenueSharingID == 0)
                        patientChargesDt.RevenueSharingID = null;
                    patientChargesDt.IsVariable = false;
                    patientChargesDt.IsUnbilledItem = false;

                    decimal totalDiscountAmount = 0;
                    decimal grossLineAmount = 1 * price;

                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;
                    patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                    patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;


                    if (isDiscountInPercentage)
                    {
                        totalDiscountAmount = grossLineAmount * discountAmount / 100;
                    }
                    else
                    {
                        totalDiscountAmount = discountAmount * 1;
                    }

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
                        totalPayer = coverageAmount * 1;
                    }

                    if (totalPayer > total)
                    {
                        totalPayer = total;
                    }


                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;
                    patientChargesDt.IsDiscount = false;
                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;
                    patientChargesDt.LineAmount = total;
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    lstPatientChargesDt.Add(patientChargesDt);

                    ct++;
                }

                if (result)
                {
                    #region Patient Charges
                    PatientChargesHd patientChargesHd = null;
                    if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0" || patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                    {
                        patientChargesHd = new PatientChargesHd();
                        patientChargesHd.VisitID = visitID;
                        patientChargesHd.LinkedChargesID = null;
                        patientChargesHd.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                        patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        patientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtRealizationDate.Text);
                        patientChargesHd.TransactionTime = txtRealizationTime.Text;
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    }
                    else
                    {
                        patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    }
                    retval = patientChargesHd.TransactionNo;
                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                        patientChargesDtDao.Insert(patientChargesDt);
                    }
                    #endregion

                    int testOrderDtCount = BusinessLayer.GetTestOrderDtRowCount(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}' AND IsDeleted = 0", hdnTestOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                    if (testOrderDtCount < 1)
                    {
                        TestOrderHd testOrderHd = testOrderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                        testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        testOrderHdDao.Update(testOrderHd);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
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