using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientUseDetailServiceCtl : BaseUserControlCtl
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

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}')",
                    Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT, Constant.SettingParameter.PRESCRIPTION_RETURN_ITEM, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));

                hdnLabHealthcareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue)).FirstOrDefault().HealthcareServiceUnitID.ToString();
                hdnPrescriptionReturnItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_RETURN_ITEM).ParameterValue;

                Helper.SetControlEntrySetting(txtServiceQty, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServiceItemCode, new ControlEntrySetting(true, true, true), "mpTrxService");
                Helper.SetControlEntrySetting(txtServicePhysicianCode, new ControlEntrySetting(true, true, true), "mpTrxService");
            }
        }

        private bool OnBeforeSaveRecordService(ref string errMessage)
        {
            int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            if (transactionID > 0)
            {
                string filterExpression = string.Format("TransactionID = {0} AND ItemID = {1} AND ParamedicID = {2} AND IsDeleted = 0", transactionID, hdnServiceItemID.Value, hdnServicePhysicianID.Value);
                if (hdnServiceTransactionDtID.Value.ToString() != "")
                    filterExpression += string.Format(" AND ID != {0}", hdnServiceTransactionDtID.Value);
                int count = BusinessLayer.GetPatientChargesDtRowCount(filterExpression);
                if (count > 0)
                {
                    errMessage = string.Format("Sudah Terdapat pelayanan {0} dengan dokter {1}", Request.Form[txtServiceItemName.UniqueID], Request.Form[txtServicePhysicianName.UniqueID]);
                    return false;
                }
                return true;
            }
            return true;
        }

        public void OnAddRecord()
        {
            txtServicePhysicianCode.Text = "";
            hdnHealthcareServiceUnitID.Value = DetailPage.GetHealthcareServiceUnitID().ToString();
            hdnTransactionDateServiceCtl.Value = DetailPage.GetTransactionDate();
            hdnTransactionTime.Value = DetailPage.GetTransactionTime();
            BindGridService();
            IsEditable = true;
            hdnIsEditable.Value = "1";
        }

        public void InitializeTransactionControl(bool flagHaveCharges)
        {
            int healthcareServiceUnitID = DetailPage.GetHealthcareServiceUnitID();
            if (flagHaveCharges)
            {
                BindGridService();
            }
        }

        public void SetControlProperties()
        {
        }

        protected bool IsEditable = true;
        private void BindGridService()
        {
            string GCTransactionStatus = DetailPage.GetGCTransactionStatus();
            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));
            IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            hdnIsEditable.Value = IsEditable && !entity.IsLockDown ? "1" : "0";
            IsEditable = entity.IsLockDown ? false : IsEditable;
            string filterExpression = "1 = 0";
            hdnServiceTransactionID.Value = DetailPage.GetTransactionHdID();

            hdnTransactionDateServiceCtl.Value = DetailPage.GetTransactionDate();
            hdnTransactionTime.Value = DetailPage.GetTransactionTime();

            if (hdnTransactionDateServiceCtl.Value == null || hdnTransactionDateServiceCtl.Value == "")
            {
                hdnTransactionDateServiceCtl.Value = DetailPage.GetTransactionDate2();
                hdnTransactionTime.Value = DetailPage.GetTransactionTime2();
            }

            if (hdnServiceTransactionID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 ORDER BY ID", hdnServiceTransactionID.Value, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.ItemGroupMaster.MEDICAL_CHECKUP);
            
            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            if (DetailPage.GetHealthcareServiceUnitID().ToString() == hdnLabHealthcareServiceUnitID.Value)
                IsShowParamedicTeam = false;
            else
                IsShowParamedicTeam = (DetailPage.GetDepartmentID() == Constant.Facility.DIAGNOSTIC);
            IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;
            lvwService.DataSource = lst;
            lvwService.DataBind();
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
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted)
                            {
                                decimal temp = entity.PayerAmount;
                                entity.PayerAmount = entity.PatientAmount;
                                entity.PatientAmount = temp;
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
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted)
                            {
                                decimal temp = entity.PayerAmount;
                                entity.PayerAmount = entity.PatientAmount;
                                entity.PatientAmount = temp;
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
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
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
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dihapus";
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
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
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dihapus";
                            result = false;
                        }
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
            ItemBalanceDao itemBalanceDao = new ItemBalanceDao(ctx);
            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{5}'", transactionHdID, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.TransactionStatus.VOID), ctx);
                foreach (vPatientChargesDt1 patientChargesDt in lstPatientChargesDt)
                {
                    PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                    entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                }
            }
        }

        private void ServiceControlToEntity(PatientChargesDt entity, IDbContext ctx)
        {
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

            int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            vPatientChargesHd1 entityChargesHd1 = BusinessLayer.GetvPatientChargesHd1List(string.Format("TransactionID = '{0}'", DetailPage.GetTransactionHdID())).FirstOrDefault();

            int registrationID = Convert.ToInt32(entityChargesHd1.RegistrationID);
            int visitID = Convert.ToInt32(entityChargesHd1.VisitID);

            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, entity.ChargeClassID, Convert.ToInt32(entity.ItemID), 1, DateTime.Now, ctx);
            ctx.CommandType = System.Data.CommandType.Text;
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
            entity.IsComplicationInPercentage = entity.IsComplicationInPercentage;
            entity.BaseComplicationAmount = entity.ComplicationAmount;
            entity.IsVariable = false;
            entity.IsUnbilledItem = false;

            decimal totalDiscountAmount = 0;
            decimal totalDiscountAmount1 = 0;
            decimal totalDiscountAmount2 = 0;
            decimal totalDiscountAmount3 = 0;

            decimal qty = Convert.ToDecimal(txtServiceQty.Text);
            decimal grossLineAmount = qty * price;
            decimal citoAmount =  Convert.ToDecimal(hdnCITOAmount.Value);

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

            totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (qty);

            if (grossLineAmount > 0)
            {
                if (totalDiscountAmount > grossLineAmount)
                {
                    totalDiscountAmount = grossLineAmount;
                }
            }

            #region old
            //if (isDiscountInPercentage)
            //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
            //else
            //    totalDiscountAmount = discountAmount * 1;
            //if (totalDiscountAmount > grossLineAmount)
            //    totalDiscountAmount = grossLineAmount;
            #endregion

            //decimal total = grossLineAmount - totalDiscountAmount;
            decimal total = grossLineAmount - totalDiscountAmount + citoAmount;
            decimal totalPayer = 0;
            if (isCoverageInPercentage)
                totalPayer = total * coverageAmount / 100;
            else
                totalPayer = coverageAmount * qty;
            if (totalPayer > total)
                totalPayer = total;

            entity.IsCITO = chkServiceIsCITO.Checked;
            if (chkServiceIsCITO.Checked)
            {
                entity.IsCITOInPercentage = (hdnServiceIsCITOInPercentage.Value == "1");
                entity.BaseCITOAmount = Convert.ToDecimal(hdnServiceBaseCITOAmount.Value);
                entity.CITOAmount = Convert.ToDecimal(hdnCITOAmount.Value);
            }

            entity.IsComplication = false;
            entity.ComplicationAmount = 0;
            entity.IsDiscount = false;
            entity.DiscountAmount = totalDiscountAmount;
            entity.UsedQuantity = entity.BaseQuantity = entity.ChargedQuantity = qty;
            entity.PatientAmount = total - totalPayer;
            entity.PayerAmount = totalPayer;
            entity.LineAmount = total;
        }

        private bool OnSaveEditRecordService(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
            try
            {
                PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
                if (entityPatientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnServiceTransactionDtID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        ServiceControlToEntity(entityDt, ctx);
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
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                    result = false;
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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