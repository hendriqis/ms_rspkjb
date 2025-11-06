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
    public partial class OrderRealizationMedicalSupport : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.BILL_SUMMARY_ORDER_REALIZATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                AppSession.UserLogin.HealthcareID,
                                                                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                                                                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                                                Constant.SettingParameter.LB_KODE_DEFAULT_DOKTER
                                                            ));
            hdnImagingServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnDefaultLabParamedicID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<vTestOrderHdCustom1> lst = BusinessLayer.GetvTestOrderHdCustom1List(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            int visitID = Convert.ToInt32(hdnVisitID.Value);

            IDbContext ctx = DbFactory.Configure(true);
            HealthcareServiceUnitDao hsuDao = new HealthcareServiceUnitDao(ctx);
            ServiceUnitMasterDao srvUnitDao = new ServiceUnitMasterDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            try
            {
                if (type == "process")
                {
                    string[] listParam = hdnParam.Value.Split('|');
                    //string[] listParamedic = hdnParamedic.Value.Split('|');
                    //int ct = 0;
                    foreach (string param in listParam)
                    {
                        //string paramedicID = listParamedic[ct];
                        string paramedicID = "0";
                        string filterExpression = string.Format("TestOrderID = '{0}'", param);
                        TestOrderHd entityTestOrderHd = testOrderHdDao.Get(Convert.ToInt32(param));
                        if (entityTestOrderHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                        {
                            paramedicID = hdnDefaultLabParamedicID.Value;
                        }
                        else
                        {
                            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", entityTestOrderHd.HealthcareServiceUnitID));
                            if (lstParamedic.Count == 1)
                            {
                                ParamedicMaster paramedic = lstParamedic.FirstOrDefault();
                                paramedicID = paramedic.ParamedicID.ToString();
                            }
                            else
                            {
                                paramedicID = entityTestOrderHd.ParamedicID.ToString();
                            }
                        }
                        List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(filterExpression, ctx);
                        List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                        if (lstTestOrderDt.Count > 0)
                        {
                            PatientChargesHd patientChargesHd = new PatientChargesHd();
                            foreach (TestOrderDt entity in lstTestOrderDt)
                            {
                                if (entity.GCTestOrderStatus == Constant.TestOrderStatus.CANCELLED || entity.GCTestOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                                {
                                    result = false;
                                    errMessage += "Order sudah diproses, silahkan merefresh halaman ini";
                                    break;
                                }
                                entity.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                testOrderDtDao.Update(entity);

                                PatientChargesDt patientChargesDt = new PatientChargesDt();
                                patientChargesDt.ItemID = entity.ItemID;
                                patientChargesDt.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                                patientChargesDt.ReferenceDtID = entity.ID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

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

                                patientChargesDt.BaseTariff = patientChargesDt.BaseComp1 = basePrice;
                                patientChargesDt.Tariff = patientChargesDt.TariffComp1 = price;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID), ctx).FirstOrDefault();
                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                                patientChargesDt.ParamedicID = Convert.ToInt32(paramedicID);
                                patientChargesDt.BusinessPartnerID = null;

                                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, entityTestOrderHd.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ctx).FirstOrDefault().RevenueSharingID;
                                if (patientChargesDt.RevenueSharingID == 0) patientChargesDt.RevenueSharingID = null;
                                
                                patientChargesDt.IsVariable = false;
                                patientChargesDt.IsUnbilledItem = false;

                                decimal grossLineAmount = entity.ItemQty * price;
                                patientChargesDt.IsCITO = entityTestOrderHd.IsCITO;
                                patientChargesDt.CITOAmount = 0;
                                patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                                patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;

                                if (patientChargesDt.IsCITO)
                                {
                                    if (entityItemMaster.IsCITOInPercentage)
                                        patientChargesDt.CITOAmount = (entityItemMaster.CITOAmount * grossLineAmount) / 100;
                                    else
                                        patientChargesDt.CITOAmount = entityItemMaster.CITOAmount;
                                    grossLineAmount += patientChargesDt.CITOAmount;
                                }

                                decimal totalDiscountAmount = 0;
                                decimal totalDiscountAmount1 = 0;
                                decimal totalDiscountAmount2 = 0;
                                decimal totalDiscountAmount3 = 0;

                                if (isDiscountInPercentage)
                                {
                                    //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                                    if (isDiscountUsedComp)
                                    {
                                        if (priceComp1 > 0)
                                        {
                                            totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                            patientChargesDt.DiscountPercentageComp1 = discountAmountComp1;
                                        }

                                        if (priceComp2 > 0)
                                        {
                                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                            patientChargesDt.DiscountPercentageComp2 = discountAmountComp2;
                                        }

                                        if (priceComp3 > 0)
                                        {
                                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                            patientChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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

                                    if (totalDiscountAmount1 > 0)
                                    {
                                        patientChargesDt.IsDiscountInPercentageComp1 = true;
                                    }

                                    if (totalDiscountAmount2 > 0)
                                    {
                                        patientChargesDt.IsDiscountInPercentageComp2 = true;
                                    }

                                    if (totalDiscountAmount3 > 0)
                                    {
                                        patientChargesDt.IsDiscountInPercentageComp3 = true;
                                    }
                                }
                                else
                                {
                                    //totalDiscountAmount = discountAmount * 1;

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

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (entity.ItemQty);

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
                                    totalPayer = coverageAmount * 1;
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

                                patientChargesDt.IsComplication = false;
                                patientChargesDt.ComplicationAmount = 0;

                                patientChargesDt.IsDiscount = totalDiscountAmount > 0;
                                patientChargesDt.DiscountAmount = totalDiscountAmount;
                                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;
                                patientChargesDt.PatientAmount = total - totalPayer;
                                patientChargesDt.PayerAmount = totalPayer;
                                patientChargesDt.LineAmount = total;
                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                                lstPatientChargesDt.Add(patientChargesDt);
                            }

                            if (result)
                            {
                                patientChargesHd = new PatientChargesHd();
                                patientChargesHd.VisitID = visitID;
                                patientChargesHd.LinkedChargesID = null;
                                patientChargesHd.TestOrderID = entityTestOrderHd.TestOrderID;
                                patientChargesHd.HealthcareServiceUnitID = entityTestOrderHd.HealthcareServiceUnitID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                HealthcareServiceUnit hsuCharges = hsuDao.Get(entityTestOrderHd.HealthcareServiceUnitID);
                                ServiceUnitMaster sums = srvUnitDao.Get(hsuCharges.ServiceUnitID);

                                if (entityTestOrderHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                else if (sums.IsLaboratoryUnit)
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                else
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;

                                patientChargesHd.TransactionDate = DateTime.Now;
                                patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
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

                                foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                {
                                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                    patientChargesDtDao.Insert(patientChargesDt);
                                }

                                entityTestOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                entityTestOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                testOrderHdDao.Update(entityTestOrderHd);
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                ctx.RollBackTransaction();
                            }
                        }
                    }
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