using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientUseDetailLogisticCtl : BaseUserControlCtl
    {
        protected bool IsShowSwitchIcon = false;
        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }
        public void InitializeTransactionControl(bool flagHaveCharges)
        {
            if (flagHaveCharges)
            {
                BindGridLogistic();
            }

            Helper.SetControlEntrySetting(txtLogisticBaseQty, new ControlEntrySetting(false, false, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticConversion, new ControlEntrySetting(false, false, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticQtyUsed, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
        }

        public void OnAddRecord()
        {
            IsEditable = true;
            BindGridLogistic();
            IsEditable = true;
            hdnIsEditable.Value = "1";
        }

        public void SetControlProperties()
        {
            BindCboLocation();
        }


        protected void cboLogisticLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            int locationID = DetailPage.GetLogisticLocationID();
            if (locationID > 0)
            {
                Location loc = BusinessLayer.GetLocation(locationID);
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                Methods.SetComboBoxField<Location>(cboLogisticLocation, lstLocation, "LocationName", "LocationID");
                cboLogisticLocation.SelectedIndex = 0;
            }
        }

        protected void cboLogisticUoM_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnLogisticItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboLogisticUoM, lst, "StandardCodeName", "StandardCodeID");
            cboLogisticUoM.SelectedIndex = -1;
        }

        protected bool IsEditable = true;
        private void BindGridLogistic()
        {
            string GCTransactionStatus = DetailPage.GetGCTransactionStatus();
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
            string transactionID = DetailPage.GetTransactionHdID();
            if (transactionID != "")
                filterExpression = string.Format("TransactionID = {0} AND UsedQuantity >= 0 AND GCItemType = '{1}' AND IsDeleted = 0", transactionID, Constant.ItemGroupMaster.LOGISTIC);
            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;
            lvwLogistic.DataSource = lst;
            lvwLogistic.DataBind();
        }

        #region Logistic
        protected void cbpLogistic_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnLogisticTransactionDtID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                    if (OnSaveEditRecordLogistic(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "approve")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnApproveChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnVoidChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteChargesDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridLogistic();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private bool OnApproveChargesDt(ref string errMessage, int ID)
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
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.IsApproved = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnVoidChargesDt(ref string errMessage, int ID)
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
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.IsDeleted = true;
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entity.GCDeleteReason = gcDeleteReason;
                            entity.DeleteReason = reason;
                            entity.DeleteDate = DateTime.Now;
                            entity.DeleteBy = AppSession.UserLogin.UserID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            
            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                List<vPatientChargesDt> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND GCItemType = '{1}' AND IsDeleted = 0", transactionHdID, Constant.ItemGroupMaster.LOGISTIC), ctx);
                foreach (vPatientChargesDt patientChargesDt in lstPatientChargesDt)
                {
                    PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                    if (entity.IsApproved)
                    {
                        entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entity.IsApproved = false;
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);
                    }
                }
            }
        }

        private void LogisticControlToEntity(PatientChargesDt entity, vPatientChargesHd1 entityHd, IDbContext ctx)
        {
            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityHd.RegistrationID, entityHd.VisitID, entity.ChargeClassID, entity.ItemID, 2, DateTime.Now, ctx);
            entity.BaseQuantity = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            entity.UsedQuantity = entity.ChargedQuantity = Convert.ToDecimal(txtLogisticQtyUsed.Text);
            entity.IsApproved = true;

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

            entity.BaseTariff = basePrice;
            entity.Tariff = price;
            entity.BaseComp1 = basePriceComp1;
            entity.BaseComp2 = basePriceComp2;
            entity.BaseComp3 = basePriceComp3;
            entity.TariffComp1 = priceComp1;
            entity.TariffComp2 = priceComp2;
            entity.TariffComp3 = priceComp3;
            entity.CostAmount = costAmount;
            ctx.CommandType = System.Data.CommandType.Text;
            ctx.Command.Parameters.Clear();
            entity.GCItemUnit = cboLogisticUoM.Value.ToString();
            entity.ParamedicID = Convert.ToInt32(DetailPage.GetRegistrationPhysicianID());
            
            decimal grossLineAmount = entity.BaseQuantity * price;

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
                            entity.DiscountPercentageComp1 = discountAmountComp1;
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
                            entity.DiscountPercentageComp2 = discountAmountComp2;
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
                            entity.DiscountPercentageComp3 = discountAmountComp3;
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
                        entity.DiscountPercentageComp1 = discountAmount;
                    }

                    if (priceComp2 > 0)
                    {
                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                        entity.DiscountPercentageComp2 = discountAmount;
                    }

                    if (priceComp3 > 0)
                    {
                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                        entity.DiscountPercentageComp3 = discountAmount;
                    }
                }

                if (entity.DiscountPercentageComp1 > 0)
                {
                    entity.IsDiscountInPercentageComp1 = true;
                }

                if (entity.DiscountPercentageComp2 > 0)
                {
                    entity.IsDiscountInPercentageComp2 = true;
                }

                if (entity.DiscountPercentageComp3 > 0)
                {
                    entity.IsDiscountInPercentageComp3 = true;
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

            totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (1);

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
                totalPayer = total * coverageAmount / 100;
            else
                totalPayer = coverageAmount * entity.BaseQuantity;
            if (total > 0 && totalPayer > total)
                totalPayer = total;

            if (hdnLogisticConversionValue.Value != "0")
            {
                entity.ConversionFactor = Convert.ToDecimal(hdnLogisticConversionValue.Value);
            }
            else
            {
                entity.ConversionFactor = 1;
            }
            entity.DiscountAmount = totalDiscountAmount;
            entity.PatientAmount = total - totalPayer;
            entity.PayerAmount = totalPayer;
            entity.LineAmount = total;
            entity.LocationID = Convert.ToInt32(cboLogisticLocation.Value);
        }

        private bool OnSaveEditRecordLogistic(ref string errMessage)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboLogisticLocation.Value.ToString(), hdnLogisticItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();

            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;

                if (qtyOnHand>= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                    try
                    {
                        int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                        if (transactionID > 0)
                        {
                            vPatientChargesHd1 entityHd = BusinessLayer.GetvPatientChargesHd1List(string.Format("TransactionID = {0}", transactionID), ctx).FirstOrDefault();
                            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnLogisticTransactionDtID.Value));
                                if (!entityDt.IsDeleted && !entityDt.IsApproved)
                                {
                                    LogisticControlToEntity(entityDt, entityHd, ctx);
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(entityDt);
                                }
                            }
                            else
                            {
                                errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                                result = false;
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtLogisticItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboLogisticUoM.Text, itemQty.ToString("N2"), cboLogisticUoM.Text);
                }   
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtLogisticItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboLogisticLocation.Text);
            }   
            return result;
        }
        #endregion
    }
}