using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using Newtonsoft.Json;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GenerateStatusOrderGroupCtl : BaseViewPopupCtl
    {
        protected string defaultParamedicName = string.Empty;
        protected string defaultParamedicID = string.Empty;
        List<PatientChargesDt> lstEntityDt = new List<PatientChargesDt>();

        public override void InitializeDataControl(string Param)
        {
            this.PopupTitle = "Generate Order Kelompok MCU";


            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}') AND HealthcareID = '{5}'",
                                                                                                    Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                                                                                                    Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                                                                                    Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER,
                                                                                                    Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST,
                                                                                                    Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID,
                                                                                                    AppSession.UserLogin.HealthcareID));

            hdnDefaultItemIDMCUPackage.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST).ParameterValue;
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnIsUsingRegistrationParamedicID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID).ParameterValue;
            
            ///hdnRegistrationParamedicID.Value = entity.ParamedicID.ToString();
            defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
            defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;

            BindGridView();
        }

        private void BindGridView()
        {

            string filterExpression = string.Format("RequestBatchNo = '{0}' AND RegistrationID IS NOT NULL AND GCRegistrationStatus NOT IN('{1}') AND DepartmentID = '{2}' AND GCVisitStatus IN ('{3}','{4}') AND  VisitID IN (SELECT VisitID FROM ConsultVisitItemPackage WHERE GCItemDetailStatus = '{5}') ", 
                hdnBatchNo.Value, 
                Constant.VisitStatus.CANCELLED,
                Constant.Facility.MEDICAL_CHECKUP,
                Constant.VisitStatus.CHECKED_IN,
                Constant.VisitStatus.RECEIVING_TREATMENT,
                Constant.TransactionStatus.PROCESSED);
            List<vAppointmentRequest> lstEntity = BusinessLayer.GetvAppointmentRequestList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpGenerateOrderStatusView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView();
                    result = "changepage";
                }
                else if (param[0] == "GenerateUpdateOrder")
                {
                    GenerateUpdateOrder(ref result);
                }
                else // refresh
                {

                    BindGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Generate Proses
        private void InsertMCUDefaultCharges(List<ConsultVisitItemPackage> lstEntityItemPackage, ConsultVisit entityVisit, IDbContext ctx)
        {
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
            foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
            {
                ItemService entityItemServicePackage = itemServiceDao.Get(entity.ItemID);
                List<GetCurrentItemTariff> listPackage = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, Convert.ToInt32(entityVisit.ChargeClassID), entity.ItemID, 1, DateTime.Now, ctx);
                decimal itemPackagePrice = listPackage.FirstOrDefault().Price;
                decimal itemPackagePriceComp1 = listPackage.FirstOrDefault().PriceComp1;
                decimal itemPackagePriceComp2 = listPackage.FirstOrDefault().PriceComp2;
                decimal itemPackagePriceComp3 = listPackage.FirstOrDefault().PriceComp3;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterPCD = string.Format("ItemPackageID = {0} AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {1} AND GCTransactionStatus != '{2}') AND IsDeleted = 0",
                                    entity.ItemID, entityVisit.VisitID, Constant.TransactionStatus.VOID);
                List<PatientChargesDt> lstPCD = BusinessLayer.GetPatientChargesDtList(filterPCD, ctx);
                decimal totalChargesDTBaseTariff = lstPCD.Sum(a => a.BaseTariff);
                decimal totalChargesDTBaseComp1 = lstPCD.Sum(a => a.BaseComp1);
                decimal totalChargesDTBaseComp2 = lstPCD.Sum(a => a.BaseComp2);
                decimal totalChargesDTBaseComp3 = lstPCD.Sum(a => a.BaseComp3);
                decimal totalChargesDTTariff = lstPCD.Sum(a => a.Tariff);
                decimal totalChargesDTTariffComp1 = lstPCD.Sum(a => a.TariffComp1);
                decimal totalChargesDTTariffComp2 = lstPCD.Sum(a => a.TariffComp2);
                decimal totalChargesDTTariffComp3 = lstPCD.Sum(a => a.TariffComp3);

                if (!entityItemServicePackage.IsUsingAccumulatedPrice)
                {
                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                    patientChargesDt.ItemID = Convert.ToInt32(hdnDefaultItemIDMCUPackage.Value);
                    patientChargesDt.ChargeClassID = Convert.ToInt32(entityVisit.ChargeClassID);
                    patientChargesDt.ItemPackageID = entity.ItemID;
                    patientChargesDt.ReferenceDtID = entity.ItemID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);
                    decimal discountAmount = 0;
                    decimal coverageAmount = 0;
                    decimal price = 0;
                    decimal basePrice = 0;
                    bool isCoverageInPercentage = false;
                    bool isDiscountInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        discountAmount = obj.DiscountAmount;
                        coverageAmount = obj.CoverageAmount;
                        price = obj.Price;
                        basePrice = obj.BasePrice;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                    }
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", patientChargesDt.ItemID), ctx).FirstOrDefault();
                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                    patientChargesDt.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)).FirstOrDefault().RevenueSharingID;
                    if (patientChargesDt.RevenueSharingID == 0)
                        patientChargesDt.RevenueSharingID = null;
                    patientChargesDt.IsVariable = false;
                    patientChargesDt.IsUnbilledItem = false;
                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;

                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;
                    patientChargesDt.IsDiscount = false;
                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                    patientChargesDt.IsCreatedBySystem = true;
                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                    patientChargesDt.BaseTariff = itemPackagePrice - totalChargesDTBaseTariff;
                    patientChargesDt.BaseComp1 = itemPackagePriceComp1 - totalChargesDTBaseComp1;
                    patientChargesDt.BaseComp2 = itemPackagePriceComp2 - totalChargesDTBaseComp2;
                    patientChargesDt.BaseComp3 = itemPackagePriceComp3 - totalChargesDTBaseComp3;

                    patientChargesDt.Tariff = itemPackagePrice - totalChargesDTTariff;
                    patientChargesDt.TariffComp1 = itemPackagePriceComp1 - totalChargesDTTariffComp1;
                    patientChargesDt.TariffComp2 = itemPackagePriceComp2 - totalChargesDTTariffComp2;
                    patientChargesDt.TariffComp3 = itemPackagePriceComp3 - totalChargesDTTariffComp3;

                    decimal total = patientChargesDt.Tariff;
                    decimal totalPayer = 0;
                    if (isCoverageInPercentage)
                        totalPayer = total * coverageAmount / 100;
                    else
                        totalPayer = coverageAmount * 1;

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

                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;
                    patientChargesDt.LineAmount = total;

                    lstPatientChargesDt.Add(patientChargesDt);
                }
            }

            if (lstPatientChargesDt.Count > 0)
            {
                PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND IsAutoTransaction = 1 AND HealthcareServiceUnitID = {1} AND GCTransactionStatus <> '{2}'", entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                if (patientChargesHd == null)
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = entityVisit.VisitID;
                    patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                    patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES;
                    patientChargesHd.TransactionDate = DateTime.Now;
                    patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.IsAutoTransaction = true;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = 0;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                }

                foreach (PatientChargesDt entityPatientChargesDt in lstPatientChargesDt)
                {
                    entityPatientChargesDt.TransactionID = patientChargesHd.TransactionID;
                    entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    patientChargesDtDao.Insert(entityPatientChargesDt);
                }
            }
        }
       
        private void SaveTestOrder(IDbContext ctx, ConsultVisit visit, List<vItemServiceDt> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
           

            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            TestOrderHd entityHd = new TestOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.FromHealthcareServiceUnitID = visit.HealthcareServiceUnitID; //Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.TestOrderDate = dateNow;
            entityHd.TestOrderTime = timeNow;
            entityHd.ScheduledDate = dateNow;
            entityHd.ScheduledTime = timeNow;
            if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
            {
                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
            }
            else if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
            {
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            }
            else
            {
                entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
            }
            entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);

            #region Patient Charges HD
            patientChargesHd.VisitID = visit.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.TestOrderID = entityHd.TestOrderID;
            patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.IsAutoTransaction = true;
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
            #endregion

            #region Patient Charges DT
            foreach (vItemServiceDt itemServiceDt in lstItemServiceDt)
            {

                TestOrderDt entity = new TestOrderDt();
                entity.TestOrderID = entityHd.TestOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.DiagnoseID = null;
                entity.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entity.ID = BusinessLayer.GetTestOrderDtMaxID(ctx);
                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

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

                decimal totalDiscountAmount = 0;
                decimal totalDiscountAmount1 = 0;
                decimal totalDiscountAmount2 = 0;
                decimal totalDiscountAmount3 = 0;

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
              
                patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountComp1;
                patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountComp2;
                patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountComp3;

                patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountComp1;
                patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountComp2;
                patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountComp3;

                ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;
                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;
                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;

                // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

                //decimal totalDiscountAmount = 0;
                //if (isDiscountInPercentage)
                //{
                //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                //}
                //else
                //{
                //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                //}

                //if (totalDiscountAmount > grossLineAmount)
                //{
                //    totalDiscountAmount = grossLineAmount;
                //}

                //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

                //var tempDiscountTotal = totalDiscountAmount;
                //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
                //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
                //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

                //if (discountComp1 > priceComp1)
                //{
                //    discountComp1 = priceComp1;
                //}

                //if (discountComp2 > priceComp2)
                //{
                //    discountComp2 = priceComp2;
                //}

                //if (discountComp3 > priceComp3)
                //{
                //    discountComp3 = priceComp3;
                //}

                patientChargesDt.DiscountAmount = 0;
                patientChargesDt.DiscountComp1 = 0;
                patientChargesDt.DiscountComp2 = 0;
                patientChargesDt.DiscountComp3 = 0;

                decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
                decimal total = grossLineAmount - 0;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * 1;

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
                patientChargesDt.IsDiscount = false;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.IsCreatedBySystem = true;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstEntityDt.Add(patientChargesDt);
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

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

                    if (isDiscountInPercentage)
                    {
                        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
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

                        if (totalDiscountAmount1 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp3 = true;
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
                    patientChargesDtPackageDao.Insert(dtpackage);
                }

                #endregion

            }
            #endregion
        }

        private void SaveServiceOrder(IDbContext ctx, ConsultVisit visit, List<vItemServiceDt> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDao = new ServiceOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            ServiceOrderHd entityHd = new ServiceOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.ServiceOrderDate = dateNow;
            entityHd.ServiceOrderTime = timeNow;
            if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityHd.TransactionCode = Constant.TransactionCode.MCU_EMERGENCY_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
            }
            else
            {
                entityHd.TransactionCode = Constant.TransactionCode.MCU_OUTPATIENT_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
            }
            entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            entityHd.ServiceOrderID = BusinessLayer.GetServiceOrderHdMaxID(ctx);

            #region Patient Charges HD

            patientChargesHd.VisitID = visit.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.ServiceOrderID = entityHd.ServiceOrderID;
            patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.IsAutoTransaction = true;
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

            #endregion

            #region Patient Charges DT
            foreach (vItemServiceDt itemServiceDt in lstItemServiceDt)
            {
                ServiceOrderDt entity = new ServiceOrderDt();
                entity.ServiceOrderID = entityHd.ServiceOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.GCServiceOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

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
                
                patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountComp1;
                patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountComp2;
                patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountComp3;

                patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountComp1;
                patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountComp2;
                patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountComp3;

                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;

                // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

                //if (isDiscountInPercentage)
                //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                //else
                //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                //if (totalDiscountAmount > grossLineAmount)
                //    totalDiscountAmount = grossLineAmount;
                //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

                //decimal totalDiscountAmount = 0;
                //var tempDiscountTotal = totalDiscountAmount;
                //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
                //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
                //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

                //if (discountComp1 > priceComp1)
                //{
                //    discountComp1 = priceComp1;
                //}

                //if (discountComp2 > priceComp2)
                //{
                //    discountComp2 = priceComp2;
                //}

                //if (discountComp3 > priceComp3)
                //{
                //    discountComp3 = priceComp3;
                //}

                patientChargesDt.DiscountAmount = 0;
                patientChargesDt.DiscountComp1 = 0;
                patientChargesDt.DiscountComp2 = 0;
                patientChargesDt.DiscountComp3 = 0;

                decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
                decimal total = grossLineAmount - 0;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * 1;

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

                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;

                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;
                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.IsCreatedBySystem = true;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstEntityDt.Add(patientChargesDt);
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

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
                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
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

                        if (totalDiscountAmount1 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp3 = true;
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
                    patientChargesDtPackageDao.Insert(dtpackage);
                }

                #endregion

            }
            #endregion
        }
        
        private void GenerateUpdateOrder(ref string result)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
            ServiceOrderHdDao serviceOrderDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao serviceOrderDtDao = new ServiceOrderDtDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            try
            {
                
                DateTime dateNow = DateTime.Now;
                string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                //List<MCUOrder> lstMCUOrder = MCUOrderList.Where(t => t.IsConfirm).GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();

                string param = hdnSelectedAppointmentRequestID.Value;

                List<AppointmentRequest> lstAppointmentRequest = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentRequestID IN({0})", param));
                if (lstAppointmentRequest.Count > 0) {
                    string ParamVisitID = string.Empty;
                    foreach (AppointmentRequest row in lstAppointmentRequest) {
                       
                        string filterExpressionCharges = string.Format("VisitID = '{0}' AND GCTransactionStatus= '{1}'", row.VisitID, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(filterExpressionCharges, ctx);
                        if (lstPatientChargesHd.Count > 0) {
                            
                            List<PatientChargesHd> lstPatientChargesSoIDData = lstPatientChargesHd.Where(p=> p.ServiceOrderID != null).ToList();
                            if (lstPatientChargesSoIDData.Count > 0) {
                                string lstSoID = string.Join(",", lstPatientChargesSoIDData.Select(x => x.ServiceOrderID));
                                List<ServiceOrderHd> lstSoHD = BusinessLayer.GetServiceOrderHdList(string.Format("ServiceOrderID IN ({0})", lstSoID), ctx);
                                foreach (ServiceOrderHd entitySoHD in lstSoHD) {
                                    entitySoHD.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    entitySoHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entitySoHD.LastUpdatedDate = DateTime.Now;
                                    serviceOrderDao.Update(entitySoHD);
                                }

                                List<ServiceOrderDt> lstSoDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID IN ({0}) AND IsDeleted=0", lstSoID), ctx);
                                foreach (ServiceOrderDt entitySoDt in lstSoDt)
                                {
                                    entitySoDt.GCServiceOrderStatus = Constant.OrderStatus.COMPLETED;
                                    entitySoDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entitySoDt.LastUpdatedDate = DateTime.Now;
                                    serviceOrderDtDao.Update(entitySoDt);
                                }
                            }
                            List<PatientChargesHd> lstPatientChargesTestOrderIDData = lstPatientChargesHd.Where(p => p.TestOrderID != null).ToList();
                            if (lstPatientChargesTestOrderIDData.Count > 0)
                            {
                                string lstTestOrderID = string.Join(",", lstPatientChargesTestOrderIDData.Select(x => x.TestOrderID));
                                List<TestOrderHd> lstTestHD = BusinessLayer.GetTestOrderHdList(string.Format("TestOrderID IN ({0})", lstTestOrderID), ctx);
                                foreach (TestOrderHd entityTestOrderHD in lstTestHD)
                                {
                                    entityTestOrderHD.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    entityTestOrderHD.GCOrderStatus = Constant.TestOrderStatus.COMPLETED;
                                    entityTestOrderHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityTestOrderHD.LastUpdatedDate = DateTime.Now;
                                    testOrderHdDao.Update(entityTestOrderHD);
                                }

                                List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID IN ({0}) AND IsDeleted=0", lstTestOrderID), ctx);
                                foreach (TestOrderDt entitytestDt in lstTestOrderDt)
                                {
                                    entitytestDt.GCTestOrderStatus = Constant.TestOrderStatus.COMPLETED;
                                    entitytestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entitytestDt.LastUpdatedDate = DateTime.Now;
                                    testOrderDtDao.Update(entitytestDt);
                                }

                               
                            }   
                             
                        }

                        string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", row.VisitID, Constant.TransactionStatus.PROCESSED);
                        List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression, ctx);
                        foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
                        {

                            entity.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            consultVisitItemPackageDao.Update(entity);
                        }
 
                       
                    }

                  
                }

                result = "generate|success";
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("generate|fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }
        #endregion
    }
}