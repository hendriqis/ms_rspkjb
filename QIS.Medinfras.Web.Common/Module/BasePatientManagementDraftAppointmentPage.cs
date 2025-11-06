using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Common
{
    public abstract class BasePatientManagementDraftAppointmentPage : BaseEntryPopupCtl
    {
        public ItemTarrifResult GetItemTariff(int registrationID, int visitID, int classID, int itemID, DateTime transactionDate, IDbContext ctx, int testPartnerID = 0)
        {
            string GCItemType = BusinessLayer.GetItemMaster(itemID).GCItemType;
            if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
            {
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, classID, itemID, 1, transactionDate, ctx, testPartnerID);
                ctx.CommandType = CommandType.Text;
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
                vItemService entity = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", itemID)).FirstOrDefault();

                ItemTarrifResult result = new ItemTarrifResult();
                result.CoverageAmount = coverageAmount;
                result.Price = price;
                result.BasePrice = basePrice;
                result.BasePriceComp1 = basePriceComp1;
                result.BasePriceComp2 = basePriceComp2;
                result.BasePriceComp3 = basePriceComp3;
                result.PriceComp1 = priceComp1;
                result.PriceComp2 = priceComp2;
                result.PriceComp3 = priceComp3;
                result.IsCoverageInPercentage = isCoverageInPercentage;
                result.IsAllowCito = entity.IsAllowCito;
                result.IsAllowComplication = entity.IsAllowComplication;

                //result.IsAllowDiscount = entity.IsAllowDiscount;
                //result.IsDiscountInPercentage = isDiscountInPercentage;
                //result.DiscountAmount = discountAmount;

                decimal totalDiscountAmount = 0;
                decimal totalDiscountAmount1 = 0;
                decimal totalDiscountAmount2 = 0;
                decimal totalDiscountAmount3 = 0;

                if (isDiscountInPercentage)
                {
                    result.IsDiscountInPercentage = true;

                    //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                    if (isDiscountUsedComp)
                    {
                        if (priceComp1 > 0)
                        {
                            totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                            result.DiscountPercentageComp1 = discountAmountComp1;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                            result.DiscountPercentageComp2 = discountAmountComp2;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                            result.DiscountPercentageComp3 = discountAmountComp3;
                        }
                    }
                    else
                    {
                        if (priceComp1 > 0)
                        {
                            totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                            result.DiscountPercentageComp1 = discountAmount;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                            result.DiscountPercentageComp2 = discountAmount;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                            result.DiscountPercentageComp3 = discountAmount;
                        }
                    }

                    if (totalDiscountAmount1 > 0)
                    {
                        result.IsDiscountInPercentageComp1 = true;
                    }

                    if (totalDiscountAmount2 > 0)
                    {
                        result.IsDiscountInPercentageComp2 = true;
                    }

                    if (totalDiscountAmount3 > 0)
                    {
                        result.IsDiscountInPercentageComp3 = true;
                    }
                }
                else
                {
                    result.IsDiscountInPercentage = false;

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

                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (1);

                result.IsDiscount = totalDiscountAmount != 0;
                result.DiscountAmount = totalDiscountAmount;
                result.DiscountAmountComp1 = totalDiscountAmount1;
                result.DiscountAmountComp2 = totalDiscountAmount2;
                result.DiscountAmountComp3 = totalDiscountAmount3;

                result.IsAllowVariable = entity.IsAllowVariable;
                result.IsUnbilledItem = entity.IsUnbilledItem;
                result.GCItemUnit = entity.GCItemUnit;
                result.ItemUnit = entity.ItemUnit;
                result.IsCITOInPercentage = entity.IsCITOInPercentage;
                result.CITOAmount = entity.CITOAmount;
                result.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                result.ComplicationAmount = entity.ComplicationAmount;
                result.DefaultTariffComp = entity.DefaultTariffComp;
                result.CostAmount = costAmount;

                return result;
            }
            else
            {
                vItemMaster entity = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", itemID)).FirstOrDefault();
                int type = 2;
                if (entity.GCItemType == Constant.ItemGroupMaster.LOGISTIC)
                    type = 3;
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, classID, itemID, type, transactionDate, ctx);
                ctx.CommandType = CommandType.Text;
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

                ItemTarrifResult result = new ItemTarrifResult();
                result.CoverageAmount = coverageAmount;
                result.Price = price;
                result.BasePrice = basePrice;
                result.IsCoverageInPercentage = isCoverageInPercentage;
                result.IsDiscountInPercentage = isDiscountInPercentage;
                result.GCItemUnit = entity.GCItemUnit;
                result.ItemUnit = entity.ItemUnit;
                result.CostAmount = costAmount;

                decimal totalDiscountAmount = 0;
                decimal totalDiscountAmount1 = 0;
                decimal totalDiscountAmount2 = 0;
                decimal totalDiscountAmount3 = 0;

                if (isDiscountInPercentage)
                {
                    result.IsDiscountInPercentage = true;

                    //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                    if (isDiscountUsedComp)
                    {
                        if (priceComp1 > 0)
                        {
                            totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                            result.DiscountPercentageComp1 = discountAmountComp1;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                            result.DiscountPercentageComp2 = discountAmountComp2;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                            result.DiscountPercentageComp3 = discountAmountComp3;
                        }
                    }
                    else
                    {
                        if (priceComp1 > 0)
                        {
                            totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                            result.DiscountPercentageComp1 = discountAmount;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                            result.DiscountPercentageComp2 = discountAmount;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                            result.DiscountPercentageComp3 = discountAmount;
                        }
                    }

                    if (totalDiscountAmount1 > 0)
                    {
                        result.IsDiscountInPercentageComp1 = true;
                    }

                    if (totalDiscountAmount2 > 0)
                    {
                        result.IsDiscountInPercentageComp2 = true;
                    }

                    if (totalDiscountAmount3 > 0)
                    {
                        result.IsDiscountInPercentageComp3 = true;
                    }
                }
                else
                {
                    result.IsDiscountInPercentage = false;

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

                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (1);

                result.IsDiscount = totalDiscountAmount != 0;
                result.DiscountAmount = totalDiscountAmount;
                result.DiscountAmountComp1 = totalDiscountAmount1;
                result.DiscountAmountComp2 = totalDiscountAmount2;
                result.DiscountAmountComp3 = totalDiscountAmount3;

                return result;
            }
        }

        public class ItemTarrifResult
        {
            public Decimal CoverageAmount { get; set; }
            public Decimal Price { get; set; }
            public Decimal BasePrice { get; set; }
            public Decimal BasePriceComp1 { get; set; }
            public Decimal BasePriceComp2 { get; set; }
            public Decimal BasePriceComp3 { get; set; }
            public Decimal PriceComp1 { get; set; }
            public Decimal PriceComp2 { get; set; }
            public Decimal PriceComp3 { get; set; }
            public bool IsCoverageInPercentage { get; set; }
            public bool IsAllowCito { get; set; }
            public bool IsAllowComplication { get; set; }
            public bool IsAllowDiscount { get; set; }
            public bool IsDiscountInPercentage { get; set; }
            public bool IsDiscountInPercentageComp1 { get; set; }
            public bool IsDiscountInPercentageComp2 { get; set; }
            public bool IsDiscountInPercentageComp3 { get; set; }
            public bool IsDiscount { get; set; }
            public Decimal DiscountAmount { get; set; }
            public Decimal DiscountAmountComp1 { get; set; }
            public Decimal DiscountAmountComp2 { get; set; }
            public Decimal DiscountAmountComp3 { get; set; }
            public Decimal DiscountPercentageComp1 { get; set; }
            public Decimal DiscountPercentageComp2 { get; set; }
            public Decimal DiscountPercentageComp3 { get; set; }
            public bool IsAllowVariable { get; set; }
            public bool IsUnbilledItem { get; set; }
            public String GCItemUnit { get; set; }
            public String ItemUnit { get; set; }
            public bool IsCITOInPercentage { get; set; }
            public Decimal CITOAmount { get; set; }
            public bool IsComplicationInPercentage { get; set; }
            public Decimal ComplicationAmount { get; set; }
            public Int16 DefaultTariffComp { get; set; }
            public Decimal CostAmount { get; set; }
        }

        protected bool OnSaveRecord(ref string errMessage, ref string retval, int registrationID, string paramCharges, string paramTestOrder, string paramPrescriptionOrder, string healthcareServiceUnitIDVisit)
        {
            bool result = true;
            Registration registration = BusinessLayer.GetRegistration(registrationID);
            vConsultVisit4 visit = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = {0}", registration.RegistrationID)).FirstOrDefault();

            List<DraftPatientChargesDt> lstDraftChargesDt = new List<DraftPatientChargesDt>();
            List<vDraftTestOrderDt> lstvDraftTestOrderDt = new List<vDraftTestOrderDt>();
            List<DraftPrescriptionOrderDt> lstDraftPrescriptionOrderDt = new List<DraftPrescriptionOrderDt>();

            string[] paramChargesSplit = paramCharges.Split('|');
            string[] paramTestOrderSplit = paramTestOrder.Split('|');
            string[] paramPrescriptionOrderSplit = paramPrescriptionOrder.Split('|');

            if (paramChargesSplit.Count() > 0)
            {
                foreach (string chargesID in paramChargesSplit)
                {
                    if (chargesID != "")
                    {
                        DraftPatientChargesDt draftChargesDt = BusinessLayer.GetDraftPatientChargesDt(Convert.ToInt32(chargesID));
                        if (draftChargesDt != null)
                        {
                            lstDraftChargesDt.Add(draftChargesDt);
                        }
                    }
                }
            }

            if (paramTestOrderSplit.Count() > 0)
            {
                foreach (string testOrderID in paramTestOrderSplit)
                {
                    if (testOrderID != "")
                    {
                        vDraftTestOrderDt draftOrderDt = BusinessLayer.GetvDraftTestOrderDtList(string.Format("ID = {0}", testOrderID)).FirstOrDefault();
                        if (draftOrderDt != null)
                        {

                            lstvDraftTestOrderDt.Add(draftOrderDt);
                        }
                    }
                }
            }

            if (paramPrescriptionOrderSplit.Count() > 0)
            {
                foreach (string prescriptionOrderID in paramPrescriptionOrderSplit)
                {
                    if (prescriptionOrderID != "")
                    {
                        DraftPrescriptionOrderDt draftPrescriptionOrderDt = BusinessLayer.GetDraftPrescriptionOrderDt(Convert.ToInt32(prescriptionOrderID));
                        if (draftPrescriptionOrderDt != null)
                        {
                            lstDraftPrescriptionOrderDt.Add(draftPrescriptionOrderDt);
                        }
                    }
                }
            }

            #region save PatientCharges
            if (lstDraftChargesDt.Count > 0)
            {
                onSavePatientCharges(registration, visit, lstDraftChargesDt);
            }
            #endregion

            #region save TestOrder
            if (lstvDraftTestOrderDt.Count > 0)
            {
                onSaveTestOrder(registration, visit, lstvDraftTestOrderDt, healthcareServiceUnitIDVisit);
            }
            #endregion

            #region save PrescriptionOrder
            if (lstDraftPrescriptionOrderDt.Count > 0)
            {
                onSavePrescriptionOrder(registration, visit, lstDraftPrescriptionOrderDt);
            }
            #endregion

            return result;
        }

        protected bool onSavePatientCharges(Registration registration, vConsultVisit4 visit, List<DraftPatientChargesDt> lstDraftChargesDt)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            IDbContext ctx2 = DbFactory.Configure(true);
            DraftPatientChargesHdDao draftEntityChargesHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao draftEntityChargesDtDao = new DraftPatientChargesDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao entityChargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemMasterDao entityMasterDao = new ItemMasterDao(ctx);
            HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar, ctx);

            string oIsEndingAmountRoundingTo100 = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).ToList().FirstOrDefault().ParameterValue;
            string oIsEndingAmountRoundingTo1 = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).ToList().FirstOrDefault().ParameterValue;

            #region save PatientCharges
            string remark = "";
            var lstEntityDraftHd = lstDraftChargesDt.GroupBy(test => test.TransactionID).Select(grp => grp.First()).ToList().OrderBy(x => x.TransactionID);
            foreach (DraftPatientChargesDt df in lstEntityDraftHd)
            {
                DraftPatientChargesHd entityDraftHd = BusinessLayer.GetDraftPatientChargesHd(df.TransactionID);
                if (String.IsNullOrEmpty(remark))
                {
                    remark = entityDraftHd.DraftTransactionNo;
                }
                else
                {
                    remark = remark + " , " + entityDraftHd.DraftTransactionNo;
                }
            }

            #region Patient Charges Hd

            PatientChargesHd entityHd = new PatientChargesHd();
            int transactionID;
            entityHd.VisitID = visit.VisitID;
            entityHd.HealthcareServiceUnitID = visit.HealthcareServiceUnitID;

            switch (visit.DepartmentID)
            {
                case Constant.Facility.INPATIENT: entityHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                case Constant.Facility.EMERGENCY: entityHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                case Constant.Facility.DIAGNOSTIC:
                    if (Page.Request.QueryString["id"] == "is")
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                    else
                    {
                        if (visit.DepartmentID == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                    }; break;
                default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
            }
            entityHd.TransactionDate = DateTime.Now;
            entityHd.TransactionTime = DateTime.Now.ToString("HH:mm");
            entityHd.IsCorrectionTransaction = false;
            entityHd.PatientBillingID = null;
            entityHd.Remarks = string.Format("From Draft Charges ({0})", remark);
            entityHd.ReferenceNo = "";
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            entityHd.GCVoidReason = null;
            entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityHd);

            #endregion

            foreach (DraftPatientChargesDt e in lstDraftChargesDt)
            {
                PatientChargesDt patientChargesDt = new PatientChargesDt();

                ItemMaster entityIM = entityMasterDao.Get(e.ItemID);
                string GCItemType = entityIM.GCItemType;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", entityIM.ItemID), ctx);

                if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
                {
                    #region Patient Charges Dt

                    patientChargesDt.ItemID = e.ItemID;
                    patientChargesDt.ChargeClassID = e.ChargeClassID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, patientChargesDt.ItemID, 1, DateTime.Now, ctx);

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

                    patientChargesDt.GCBaseUnit = e.GCItemUnit;
                    patientChargesDt.GCItemUnit = e.GCItemUnit;
                    patientChargesDt.IsSubContractItem = entity.IsSubContractItem;

                    patientChargesDt.ParamedicID = e.ParamedicID;

                    patientChargesDt.IsVariable = e.IsVariable;
                    patientChargesDt.IsUnbilledItem = e.IsUnbilledItem;

                    decimal qty = e.ChargedQuantity;
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

                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;
                    patientChargesDt.LineAmount = total;

                    decimal oPatientAmount = patientChargesDt.PatientAmount;
                    decimal oPayerAmount = patientChargesDt.PayerAmount;
                    decimal oLineAmount = patientChargesDt.LineAmount;

                    if (oIsEndingAmountRoundingTo1 == "1")
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

                    if (GCItemType == Constant.ItemType.OBAT_OBATAN || GCItemType == Constant.ItemType.BARANG_MEDIS || GCItemType == Constant.ItemType.BARANG_UMUM || GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                    {
                        if (oIsEndingAmountRoundingTo100 == "1")
                        {
                            oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                            oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                            oLineAmount = oPatientAmount + oPayerAmount;
                        }
                    }

                    patientChargesDt.PatientAmount = oPatientAmount;
                    patientChargesDt.PayerAmount = oPayerAmount;
                    patientChargesDt.LineAmount = oLineAmount;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, entityHd.HealthcareServiceUnitID, entityHd.TransactionDate, entityHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;

                    if (patientChargesDt.RevenueSharingID == 0)
                        patientChargesDt.RevenueSharingID = null;

                    patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    patientChargesDt.TransactionID = transactionID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int oChargesDtID = entityChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

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
                        entityChargesDtParamedicDao.Insert(dtparamedic);
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

                        int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, entityHd.HealthcareServiceUnitID, entityHd.TransactionDate, entityHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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
                        GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, entityHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

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
                        entityChargesDtPackageDao.Insert(dtpackage);

                        lstDtPackage.Add(dtpackage);
                    }

                    if (countInventoryItemDetail > 0)
                    {
                        HealthcareServiceUnit chargesHSU = chargesHSUDao.Get(visit.HealthcareServiceUnitID);

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
                        foreach (PatientChargesDtPackage pck in lstDtPackage)
                        {
                            BaseTariff += pck.BaseTariff * pck.ChargedQuantity;
                            BaseComp1 += pck.BaseComp1 * pck.ChargedQuantity;
                            BaseComp2 += pck.BaseComp2 * pck.ChargedQuantity;
                            BaseComp3 += pck.BaseComp3 * pck.ChargedQuantity;
                            Tariff += pck.Tariff * pck.ChargedQuantity;
                            TariffComp1 += pck.TariffComp1 * pck.ChargedQuantity;
                            TariffComp2 += pck.TariffComp2 * pck.ChargedQuantity;
                            TariffComp3 += pck.TariffComp3 * pck.ChargedQuantity;
                            DiscountAmount += pck.DiscountAmount;
                            DiscountComp1 += pck.DiscountComp1 * pck.ChargedQuantity;
                            DiscountComp2 += pck.DiscountComp2 * pck.ChargedQuantity;
                            DiscountComp3 += pck.DiscountComp3 * pck.ChargedQuantity;

                        }

                        pcdt.BaseTariff = BaseTariff / pcdt.ChargedQuantity;
                        pcdt.BaseComp1 = BaseComp1 / pcdt.ChargedQuantity;
                        pcdt.BaseComp2 = BaseComp2 / pcdt.ChargedQuantity;
                        pcdt.BaseComp3 = BaseComp3 / pcdt.ChargedQuantity;
                        pcdt.Tariff = Tariff / pcdt.ChargedQuantity;
                        pcdt.TariffComp1 = TariffComp1 / pcdt.ChargedQuantity;
                        pcdt.TariffComp2 = TariffComp2 / pcdt.ChargedQuantity;
                        pcdt.TariffComp3 = TariffComp3 / pcdt.ChargedQuantity;
                        pcdt.DiscountAmount = DiscountAmount / pcdt.ChargedQuantity;
                        pcdt.DiscountComp1 = DiscountComp1 / pcdt.ChargedQuantity;
                        pcdt.DiscountComp2 = DiscountComp2 / pcdt.ChargedQuantity;
                        pcdt.DiscountComp3 = DiscountComp3 / pcdt.ChargedQuantity;

                        grossLineAmount = pcdt.Tariff * pcdt.ChargedQuantity;
                        totalDiscountAmount = pcdt.DiscountAmount * pcdt.ChargedQuantity;
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

                        pcdt.PatientAmount = total - totalPayer;
                        pcdt.PayerAmount = totalPayer;
                        pcdt.LineAmount = total;

                        oPatientAmount = pcdt.PatientAmount;
                        oPayerAmount = pcdt.PayerAmount;
                        oLineAmount = pcdt.LineAmount;

                        if (oIsEndingAmountRoundingTo1 == "1")
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

                        if (GCItemType == Constant.ItemType.OBAT_OBATAN || GCItemType == Constant.ItemType.BARANG_MEDIS || GCItemType == Constant.ItemType.BARANG_UMUM || GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                        {
                            if (oIsEndingAmountRoundingTo100 == "1")
                            {
                                oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                oLineAmount = oPatientAmount + oPayerAmount;
                            }
                        }

                        pcdt.PatientAmount = oPatientAmount;
                        pcdt.PayerAmount = oPayerAmount;
                        pcdt.LineAmount = oLineAmount;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityChargesDtDao.Update(pcdt);
                    }

                    #endregion
                }
                else
                {
                    #region Patient Charges Dt
                    decimal qty = e.ChargedQuantity;

                    patientChargesDt.ItemID = e.ItemID;
                    patientChargesDt.ChargeClassID = e.ChargeClassID;

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, e.ItemID, 2, DateTime.Now, ctx);

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

                    patientChargesDt.GCBaseUnit = entityIM.GCItemUnit;
                    patientChargesDt.GCItemUnit = e.GCItemUnit;
                    patientChargesDt.ParamedicID = e.ParamedicID;

                    patientChargesDt.IsVariable = e.IsVariable;
                    patientChargesDt.IsUnbilledItem = e.IsUnbilledItem;

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

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, patientChargesDt.ItemID), ctx).FirstOrDefault();
                    patientChargesDt.AveragePrice = iPlanning.AveragePrice;
                    patientChargesDt.CostAmount = iPlanning.UnitPrice;

                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;
                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;

                    patientChargesDt.IsDiscount = totalDiscountAmount != 0 ? true : false;
                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                    patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                    patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                    patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty;

                    decimal oPatientAmount = total - totalPayer;
                    decimal oPayerAmount = totalPayer;
                    decimal oLineAmount = total;

                    if (oIsEndingAmountRoundingTo1 == "1")
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

                    if (GCItemType == Constant.ItemType.OBAT_OBATAN || GCItemType == Constant.ItemType.BARANG_MEDIS || GCItemType == Constant.ItemType.BARANG_UMUM || GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                    {
                        if (oIsEndingAmountRoundingTo100 == "1")
                        {
                            oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                            oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                            oLineAmount = oPatientAmount + oPayerAmount;
                        }
                    }

                    patientChargesDt.PatientAmount = oPatientAmount;
                    patientChargesDt.PayerAmount = oPayerAmount;
                    patientChargesDt.LineAmount = oLineAmount;

                    patientChargesDt.LocationID = e.LocationID;
                    patientChargesDt.IsApproved = true;
                    patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    patientChargesDt.TransactionID = transactionID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityChargesDtDao.Insert(patientChargesDt);

                    #endregion
                }

                if (patientChargesDt.ItemID != 0)
                {
                    e.GCTransactionDetailStatus = Constant.TransactionStatus.CLOSED;
                    e.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    draftEntityChargesDtDao.Update(e);

                    DraftPatientChargesHd draftChargesHd = draftEntityChargesHdDao.Get(e.TransactionID);
                    if (draftChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<DraftPatientChargesDt> lstdraftCharges = BusinessLayer.GetDraftPatientChargesDtList(string.Format("TransactionID = {0} AND GCTransactionDetailStatus = '{1}' AND IsDeleted = 0", draftChargesHd.TransactionID, Constant.TransactionStatus.OPEN), ctx);
                        if (lstdraftCharges.Count <= 0)
                        {
                            if (String.IsNullOrEmpty(draftChargesHd.Remarks))
                            {
                                draftChargesHd.Remarks = string.Format("Reference No : {0}", entityHd.TransactionNo);
                            }
                            else
                            {
                                draftChargesHd.Remarks = draftChargesHd.Remarks + string.Format(" | Reference No : {0}", entityHd.TransactionNo);
                            }

                            draftChargesHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            draftChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            draftEntityChargesHdDao.Update(draftChargesHd);
                        }
                    }
                }
            }

            ctx.CommitTransaction();

            return true;

            #endregion
        }

        protected bool onSaveTestOrder(Registration registration, vConsultVisit4 visit, List<vDraftTestOrderDt> lstvDraftTestOrderDt, String healthcareServiceUnitIDVisit)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DraftTestOrderHdDao draftEntityOrderHdDao = new DraftTestOrderHdDao(ctx);
            DraftTestOrderDtDao draftEntityOrderDtDao = new DraftTestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            TestOrderHdDao entityOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityOrderDtDao = new TestOrderDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            DiagnosticVisitScheduleDao diagVisitScheduleDao = new DiagnosticVisitScheduleDao(ctx);

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //2
                                                        Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //3
                                                        Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //4
                                                        Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE //5
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar, ctx);

            string ImagingServiceUnitID = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            string LaboratoryServiceUnitID = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;

            string oIsEndingAmountRoundingTo100 = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).ToList().FirstOrDefault().ParameterValue;
            string oIsEndingAmountRoundingTo1 = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).ToList().FirstOrDefault().ParameterValue;

            string isUsingMultiVisitSchedule = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).ToList().FirstOrDefault().ParameterValue;

            List<vDraftTestOrderDt> lstDraftLaboratoryOrder = lstvDraftTestOrderDt.Where(t => t.HealthcareServiceUnitID == Convert.ToInt32(LaboratoryServiceUnitID)).ToList();
            List<vDraftTestOrderDt> lstDraftImagingOrder = lstvDraftTestOrderDt.Where(t => t.HealthcareServiceUnitID == Convert.ToInt32(ImagingServiceUnitID)).ToList();
            List<vDraftTestOrderDt> lstDraftOtherOrder = lstvDraftTestOrderDt.Where(t => t.HealthcareServiceUnitID != Convert.ToInt32(LaboratoryServiceUnitID) && t.HealthcareServiceUnitID != Convert.ToInt32(ImagingServiceUnitID)).ToList();

            #region Laboratory Test Order
            if (lstDraftLaboratoryOrder.Count > 0)
            {
                if (!visit.IsLaboratoryUnit)
                {
                    var lstTestOrderHd = lstDraftLaboratoryOrder.GroupBy(test => test.DraftTestOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.DraftTestOrderID);

                    foreach (vDraftTestOrderDt e in lstTestOrderHd)
                    {
                        TestOrderHd entityHd = new TestOrderHd();
                        Int32 testOrderID;

                        DraftTestOrderHd entityDraftHd = draftEntityOrderHdDao.Get(e.DraftTestOrderID);
                        entityHd.VisitID = visit.VisitID;
                        entityHd.HealthcareServiceUnitID = e.HealthcareServiceUnitID;
                        entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(healthcareServiceUnitIDVisit);
                        entityHd.VisitHealthcareServiceUnitID = Convert.ToInt32(healthcareServiceUnitIDVisit);
                        entityHd.TestOrderDate = entityDraftHd.DraftTestOrderDate;
                        entityHd.TestOrderTime = entityDraftHd.DraftTestOrderTime;
                        entityHd.GCToBePerformed = entityDraftHd.GCToBePerformed;
                        entityHd.ScheduledDate = entityDraftHd.ScheduledDate;
                        entityHd.ScheduledTime = entityDraftHd.ScheduledTime;
                        entityHd.IsCITO = entityDraftHd.IsCITO;
                        entityHd.Remarks = entityDraftHd.Remarks;
                        entityHd.ParamedicID = entityDraftHd.ParamedicID;
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                        entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                        entityHd.ProposedBy = AppSession.UserLogin.UserID;
                        entityHd.ProposedDate = DateTime.Now;
                        entityHd.Remarks = string.Format("From Draft Order ({0})", entityDraftHd.DraftTestOrderNo);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        testOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);

                        List<vDraftTestOrderDt> lstDraftLaboratoryOrderPerHd = lstDraftLaboratoryOrder.Where(t => t.DraftTestOrderID == e.DraftTestOrderID).ToList();

                        if (lstDraftLaboratoryOrderPerHd.Count > 0)
                        {
                            foreach (vDraftTestOrderDt x in lstDraftLaboratoryOrderPerHd)
                            {
                                DraftTestOrderDt draftTestorderDt = draftEntityOrderDtDao.Get(x.ID);
                                TestOrderDt entityDt = new TestOrderDt();
                                entityDt.ItemID = draftTestorderDt.ItemID;
                                entityDt.DiagnoseID = draftTestorderDt.DiagnoseID;
                                entityDt.IsCITO = draftTestorderDt.IsCITO;
                                entityDt.Remarks = draftTestorderDt.Remarks;
                                entityDt.TestOrderID = testOrderID;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                entityDt.ItemQty = draftTestorderDt.ItemQty;
                                entityDt.ItemUnit = draftTestorderDt.ItemUnit;
                                entityOrderDtDao.Insert(entityDt);

                                draftTestorderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CLOSED;
                                draftTestorderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityOrderDtDao.Update(draftTestorderDt);

                                DraftTestOrderHd draftOrderHd = draftEntityOrderHdDao.Get(x.DraftTestOrderID);
                                if (draftOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<DraftTestOrderDt> lstdraftOrder = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND GCDraftTestOrderStatus = '{1}' AND IsDeleted = 0", draftOrderHd.DraftTestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                                    if (lstdraftOrder.Count <= 0)
                                    {
                                        if (String.IsNullOrEmpty(draftOrderHd.Remarks))
                                        {
                                            draftOrderHd.Remarks = string.Format("Reference No : {0}", entityHd.TestOrderNo);
                                        }
                                        else
                                        {
                                            draftOrderHd.Remarks = draftOrderHd.Remarks + string.Format(" | Reference No : {0}", entityHd.TestOrderNo);
                                        }

                                        draftOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        draftOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        draftEntityOrderHdDao.Update(draftOrderHd);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var lstTestOrderHd = lstDraftLaboratoryOrder.GroupBy(test => test.DraftTestOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.DraftTestOrderID);

                    foreach (vDraftTestOrderDt e in lstTestOrderHd)
                    {
                        int transactionID;
                        PatientChargesHd patientChargesHd = new PatientChargesHd();
                        Int32 chargeClassID = Convert.ToInt32(BusinessLayer.GetConsultVisit(visit.VisitID).ChargeClassID);

                        patientChargesHd.VisitID = visit.VisitID;
                        patientChargesHd.LinkedChargesID = null;
                        //patientChargesHd.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                        patientChargesHd.HealthcareServiceUnitID = e.HealthcareServiceUnitID;
                        patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        patientChargesHd.TransactionDate = DateTime.Now;
                        patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;

                        string remarks = string.Format("From Draft Order ({0})", e.DraftTestOrderNo);
                        if (!string.IsNullOrEmpty(e.Remarks))
                        {
                            remarks += string.Format("|{0}", e.Remarks);
                        }
                        patientChargesHd.Remarks = remarks;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        transactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

                        List<vDraftTestOrderDt> lstDraftLaboratoryOrderPerHd = lstDraftLaboratoryOrder.Where(t => t.DraftTestOrderID == e.DraftTestOrderID).ToList();

                        if (lstDraftLaboratoryOrderPerHd.Count > 0)
                        {
                            foreach (vDraftTestOrderDt x in lstDraftLaboratoryOrderPerHd)
                            {
                                PatientChargesDt patientChargesDt = new PatientChargesDt();
                                DraftTestOrderDt draftTestorderDt = BusinessLayer.GetDraftTestOrderDt(x.ID);

                                patientChargesDt.TransactionID = transactionID;
                                patientChargesDt.ItemID = draftTestorderDt.ItemID;
                                patientChargesDt.ChargeClassID = chargeClassID;

                                if (draftTestorderDt.ItemPackageID != 0)
                                {
                                    patientChargesDt.ItemPackageID = draftTestorderDt.ItemPackageID;
                                }

                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registration.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, draftTestorderDt.ItemID, 1, DateTime.Now, ctx);

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

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", draftTestorderDt.ItemID)).FirstOrDefault();
                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                                patientChargesDt.ParamedicID = x.ParamedicID;
                                patientChargesDt.BusinessPartnerID = draftTestorderDt.BusinessPartnerID;
                                if (patientChargesDt.BusinessPartnerID != null)
                                {
                                    patientChargesDt.IsSubContractItem = true;
                                }
                                else
                                {
                                    patientChargesDt.IsSubContractItem = false;
                                }

                                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                                if (patientChargesDt.RevenueSharingID == 0)
                                    patientChargesDt.RevenueSharingID = null;
                                patientChargesDt.IsVariable = false;
                                patientChargesDt.IsUnbilledItem = false;

                                decimal grossLineAmount = x.ItemQty * price;

                                patientChargesDt.IsCITO = false;
                                patientChargesDt.CITOAmount = 0;
                                patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                                patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;
                                if (x.IsCito)
                                {
                                    patientChargesDt.IsCITO = true;
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

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (x.ItemQty);

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

                                patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                patientChargesDt.DiscountAmount = totalDiscountAmount;
                                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                patientChargesDt.IsComplication = false;
                                patientChargesDt.ComplicationAmount = 0;

                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = x.ItemQty;

                                patientChargesDt.PatientAmount = total - totalPayer;
                                patientChargesDt.PayerAmount = totalPayer;
                                patientChargesDt.LineAmount = total;

                                decimal oPatientAmount = patientChargesDt.PatientAmount;
                                decimal oPayerAmount = patientChargesDt.PayerAmount;
                                decimal oLineAmount = patientChargesDt.LineAmount;

                                if (oIsEndingAmountRoundingTo1 == "1")
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

                                ItemMaster ims = entityItemMasterDao.Get(patientChargesDt.ItemID);
                                if (ims.GCItemType == Constant.ItemType.OBAT_OBATAN || ims.GCItemType == Constant.ItemType.BARANG_MEDIS || ims.GCItemType == Constant.ItemType.BARANG_UMUM || ims.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                                {
                                    if (oIsEndingAmountRoundingTo100 == "1")
                                    {
                                        oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                        oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                        oLineAmount = oPatientAmount + oPayerAmount;
                                    }
                                }

                                patientChargesDt.PatientAmount = oPatientAmount;
                                patientChargesDt.PayerAmount = oPayerAmount;
                                patientChargesDt.LineAmount = oLineAmount;

                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                int oID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
                                foreach (vItemServiceDt isd in isdList)
                                {
                                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                    dtpackage.PatientChargesDtID = oID;
                                    dtpackage.ItemID = isd.DetailItemID;
                                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, patientChargesHd.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    if (revID != 0 && revID != null)
                                    {
                                        dtpackage.RevenueSharingID = revID;
                                    }
                                    else
                                    {
                                        dtpackage.RevenueSharingID = null;
                                    }

                                    dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

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
                                    grossLineAmount = 0;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

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

                                    totalDiscountAmount = 0;
                                    totalDiscountAmount1 = 0;
                                    totalDiscountAmount2 = 0;
                                    totalDiscountAmount3 = 0;

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

                                    if (grossLineAmount > 0)
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
                                    entityDtPackageDao.Insert(dtpackage);
                                }

                                draftTestorderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CLOSED;
                                draftTestorderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityOrderDtDao.Update(draftTestorderDt);

                                DraftTestOrderHd draftOrderHd = draftEntityOrderHdDao.Get(x.DraftTestOrderID);
                                if (draftOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<DraftTestOrderDt> lstdraftOrder = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND GCDraftTestOrderStatus = '{1}' AND IsDeleted = 0", draftOrderHd.DraftTestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                                    if (lstdraftOrder.Count <= 0)
                                    {
                                        if (String.IsNullOrEmpty(draftOrderHd.Remarks))
                                        {
                                            draftOrderHd.Remarks = string.Format("Reference No : {0}", patientChargesHd.TransactionNo);
                                        }
                                        else
                                        {
                                            draftOrderHd.Remarks = draftOrderHd.Remarks + string.Format(" | Reference No : {0}", patientChargesHd.TransactionNo);
                                        }

                                        draftOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        draftOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        draftEntityOrderHdDao.Update(draftOrderHd);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Imaging Test Order
            if (lstDraftImagingOrder.Count > 0)
            {
                if (visit.HealthcareServiceUnitID != Convert.ToInt32(ImagingServiceUnitID))
                {
                    var lstTestOrderHd = lstDraftImagingOrder.GroupBy(test => test.DraftTestOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.DraftTestOrderID);

                    foreach (vDraftTestOrderDt f in lstTestOrderHd)
                    {
                        TestOrderHd entityHd = new TestOrderHd();
                        Int32 testOrderID;

                        DraftTestOrderHd entityDraftHd = draftEntityOrderHdDao.Get(f.DraftTestOrderID);
                        entityHd.VisitID = visit.VisitID;
                        entityHd.HealthcareServiceUnitID = entityDraftHd.HealthcareServiceUnitID;
                        entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(healthcareServiceUnitIDVisit);
                        entityHd.VisitHealthcareServiceUnitID = Convert.ToInt32(healthcareServiceUnitIDVisit);
                        entityHd.TestOrderDate = entityDraftHd.DraftTestOrderDate;
                        entityHd.TestOrderTime = entityDraftHd.DraftTestOrderTime;
                        entityHd.GCToBePerformed = entityDraftHd.GCToBePerformed;
                        entityHd.ScheduledDate = entityDraftHd.ScheduledDate;
                        entityHd.ScheduledTime = entityDraftHd.ScheduledTime;
                        entityHd.IsCITO = entityDraftHd.IsCITO;
                        entityHd.Remarks = entityDraftHd.Remarks;
                        entityHd.ParamedicID = entityDraftHd.ParamedicID;
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                        entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                        entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityHd.ProposedBy = AppSession.UserLogin.UserID;
                        entityHd.ProposedDate = DateTime.Now;

                        entityHd.Remarks = string.Format("From Draft Order ({0})", entityDraftHd.DraftTestOrderNo);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entityHd.CreatedBy = AppSession.UserLogin.UserID;

                        entityOrderHdDao.Insert(entityHd);

                        testOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);

                        List<vDraftTestOrderDt> lstDraftImagingOrderPerHd = lstDraftImagingOrder.Where(t => t.DraftTestOrderID == f.DraftTestOrderID).ToList();

                        if (lstDraftImagingOrderPerHd.Count > 0)
                        {
                            foreach (vDraftTestOrderDt x in lstDraftImagingOrderPerHd)
                            {
                                DraftTestOrderDt draftTestorderDt = draftEntityOrderDtDao.Get(x.ID);
                                TestOrderDt entityDt = new TestOrderDt();
                                entityDt.ItemID = draftTestorderDt.ItemID;
                                entityDt.DiagnoseID = draftTestorderDt.DiagnoseID;
                                entityDt.IsCITO = draftTestorderDt.IsCITO;
                                entityDt.Remarks = draftTestorderDt.Remarks;
                                entityDt.TestOrderID = testOrderID;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                entityDt.ItemQty = draftTestorderDt.ItemQty;
                                entityDt.ItemUnit = draftTestorderDt.ItemUnit;
                                entityOrderDtDao.Insert(entityDt);

                                draftTestorderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CLOSED;
                                draftTestorderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityOrderDtDao.Update(draftTestorderDt);

                                DraftTestOrderHd draftOrderHd = draftEntityOrderHdDao.Get(x.DraftTestOrderID);
                                if (draftOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<DraftTestOrderDt> lstdraftOrder = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND GCDraftTestOrderStatus = '{1}' AND IsDeleted = 0", draftOrderHd.DraftTestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                                    if (lstdraftOrder.Count <= 0)
                                    {
                                        if (String.IsNullOrEmpty(draftOrderHd.Remarks))
                                        {
                                            draftOrderHd.Remarks = string.Format("Reference No : {0}", entityHd.TestOrderNo);
                                        }
                                        else
                                        {
                                            draftOrderHd.Remarks = draftOrderHd.Remarks + string.Format(" | Reference No : {0}", entityHd.TestOrderNo);
                                        }
                                        draftOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        draftOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        draftEntityOrderHdDao.Update(draftOrderHd);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var lstTestOrderHd = lstDraftImagingOrder.GroupBy(test => test.DraftTestOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.DraftTestOrderID);

                    foreach (vDraftTestOrderDt e in lstTestOrderHd)
                    {
                        int transactionID;
                        PatientChargesHd patientChargesHd = new PatientChargesHd();
                        Int32 chargeClassID = Convert.ToInt32(BusinessLayer.GetConsultVisit(visit.VisitID).ChargeClassID);

                        patientChargesHd.VisitID = visit.VisitID;
                        patientChargesHd.LinkedChargesID = null;
                        //patientChargesHd.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                        patientChargesHd.HealthcareServiceUnitID = e.HealthcareServiceUnitID;
                        patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        patientChargesHd.TransactionDate = DateTime.Now;
                        patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;

                        string remarks = string.Format("From Draft Order ({0})", e.DraftTestOrderNo);
                        if (!string.IsNullOrEmpty(e.Remarks))
                        {
                            remarks += string.Format("|{0}", e.Remarks);
                        }

                        patientChargesHd.Remarks = remarks;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        transactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

                        List<vDraftTestOrderDt> lstDraftLaboratoryOrderPerHd = lstDraftImagingOrder.Where(t => t.DraftTestOrderID == e.DraftTestOrderID).ToList();

                        if (lstDraftLaboratoryOrderPerHd.Count > 0)
                        {
                            foreach (vDraftTestOrderDt x in lstDraftLaboratoryOrderPerHd)
                            {
                                PatientChargesDt patientChargesDt = new PatientChargesDt();
                                DraftTestOrderDt draftTestorderDt = BusinessLayer.GetDraftTestOrderDt(x.ID);

                                patientChargesDt.TransactionID = transactionID;
                                patientChargesDt.ItemID = x.ItemID;
                                patientChargesDt.ChargeClassID = chargeClassID;

                                if (x.ItemPackageID != 0)
                                {
                                    patientChargesDt.ItemPackageID = x.ItemPackageID;
                                }
                                //                        patientChargesDt.ReferenceDtID = testDt.ID;

                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registration.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, x.ItemID, 1, DateTime.Now, ctx);

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

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", x.ItemID)).FirstOrDefault();
                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                                patientChargesDt.ParamedicID = x.ParamedicID;
                                patientChargesDt.BusinessPartnerID = x.BusinessPartnerID;
                                if (patientChargesDt.BusinessPartnerID != null)
                                {
                                    patientChargesDt.IsSubContractItem = true;
                                }
                                else
                                {
                                    patientChargesDt.IsSubContractItem = false;
                                }

                                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                                if (patientChargesDt.RevenueSharingID == 0)
                                    patientChargesDt.RevenueSharingID = null;
                                patientChargesDt.IsVariable = false;
                                patientChargesDt.IsUnbilledItem = false;

                                decimal grossLineAmount = x.ItemQty * price;

                                patientChargesDt.IsCITO = false;
                                patientChargesDt.CITOAmount = 0;
                                patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                                patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;
                                if (x.IsCito)
                                {
                                    patientChargesDt.IsCITO = true;
                                    if (entityItemMaster.IsCITOInPercentage)
                                        patientChargesDt.CITOAmount = (entityItemMaster.CITOAmount * grossLineAmount) / 100;
                                    else
                                        patientChargesDt.CITOAmount = entityItemMaster.CITOAmount;
                                    grossLineAmount += patientChargesDt.CITOAmount;
                                }

                                patientChargesDt.IsComplication = false;
                                patientChargesDt.ComplicationAmount = 0;

                                //patientChargesDt.IsDiscount = false;
                                //patientChargesDt.DiscountAmount = totalDiscountAmount;

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

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (x.ItemQty);

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

                                patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                patientChargesDt.DiscountAmount = totalDiscountAmount;
                                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = x.ItemQty;

                                patientChargesDt.PatientAmount = total - totalPayer;
                                patientChargesDt.PayerAmount = totalPayer;
                                patientChargesDt.LineAmount = total;

                                decimal oPatientAmount = patientChargesDt.PatientAmount;
                                decimal oPayerAmount = patientChargesDt.PayerAmount;
                                decimal oLineAmount = patientChargesDt.LineAmount;

                                if (oIsEndingAmountRoundingTo1 == "1")
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

                                ItemMaster ims = entityItemMasterDao.Get(patientChargesDt.ItemID);
                                if (ims.GCItemType == Constant.ItemType.OBAT_OBATAN || ims.GCItemType == Constant.ItemType.BARANG_MEDIS || ims.GCItemType == Constant.ItemType.BARANG_UMUM || ims.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                                {
                                    if (oIsEndingAmountRoundingTo100 == "1")
                                    {
                                        oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                        oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                        oLineAmount = oPatientAmount + oPayerAmount;
                                    }
                                }

                                patientChargesDt.PatientAmount = oPatientAmount;
                                patientChargesDt.PayerAmount = oPayerAmount;
                                patientChargesDt.LineAmount = oLineAmount;

                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                int oID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
                                foreach (vItemServiceDt isd in isdList)
                                {
                                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                    dtpackage.PatientChargesDtID = oID;
                                    dtpackage.ItemID = isd.DetailItemID;
                                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, patientChargesHd.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    if (revID != 0 && revID != null)
                                    {
                                        dtpackage.RevenueSharingID = revID;
                                    }
                                    else
                                    {
                                        dtpackage.RevenueSharingID = null;
                                    }

                                    dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

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
                                    grossLineAmount = 0;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

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

                                    totalDiscountAmount = 0;
                                    totalDiscountAmount1 = 0;
                                    totalDiscountAmount2 = 0;
                                    totalDiscountAmount3 = 0;

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

                                    if (grossLineAmount > 0)
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
                                    entityDtPackageDao.Insert(dtpackage);
                                }

                                draftTestorderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CLOSED;
                                draftTestorderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityOrderDtDao.Update(draftTestorderDt);

                                DraftTestOrderHd draftOrderHd = draftEntityOrderHdDao.Get(x.DraftTestOrderID);
                                if (draftOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<DraftTestOrderDt> lstdraftOrder = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND GCDraftTestOrderStatus = '{1}' AND IsDeleted = 0", draftOrderHd.DraftTestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                                    if (lstdraftOrder.Count <= 0)
                                    {
                                        if (String.IsNullOrEmpty(draftOrderHd.Remarks))
                                        {
                                            draftOrderHd.Remarks = string.Format("Reference No : {0}", patientChargesHd.TransactionNo);
                                        }
                                        else
                                        {
                                            draftOrderHd.Remarks = draftOrderHd.Remarks + string.Format(" | Reference No : {0}", patientChargesHd.TransactionNo);
                                        }

                                        draftOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        draftOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        draftEntityOrderHdDao.Update(draftOrderHd);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Other Test Order
            if (lstDraftOtherOrder.Count > 0)
            {
                var lstTestOrderHd = lstDraftOtherOrder.GroupBy(test => test.DraftTestOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.DraftTestOrderID);

                foreach (vDraftTestOrderDt e in lstTestOrderHd)
                {
                    if (e.HealthcareServiceUnitID != visit.HealthcareServiceUnitID)
                    {
                        TestOrderHd entityHd = new TestOrderHd();
                        Int32 testOrderID;

                        DraftTestOrderHd entityDraftHd = draftEntityOrderHdDao.Get(e.DraftTestOrderID);
                        entityHd.VisitID = visit.VisitID;
                        entityHd.HealthcareServiceUnitID = e.HealthcareServiceUnitID;
                        entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(healthcareServiceUnitIDVisit);
                        entityHd.VisitHealthcareServiceUnitID = Convert.ToInt32(healthcareServiceUnitIDVisit);
                        entityHd.TestOrderDate = entityDraftHd.DraftTestOrderDate;
                        entityHd.TestOrderTime = entityDraftHd.DraftTestOrderTime;
                        entityHd.GCToBePerformed = entityDraftHd.GCToBePerformed;
                        entityHd.ScheduledDate = entityDraftHd.ScheduledDate;
                        entityHd.ScheduledTime = entityDraftHd.ScheduledTime;
                        entityHd.IsCITO = entityDraftHd.IsCITO;
                        entityHd.Remarks = entityDraftHd.Remarks;
                        entityHd.ParamedicID = entityDraftHd.ParamedicID;
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                        entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                        entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE; //Episode sekarang jika di appointment
                        entityHd.ScheduledDate = entityDraftHd.ScheduledDate;
                        entityHd.ScheduledTime = entityDraftHd.ScheduledTime;
                        entityHd.RoomID = entityDraftHd.RoomID;
                        entityHd.IsOperatingRoomOrder = entityDraftHd.IsOperatingRoomOrder;
                        entityHd.EstimatedDuration = entityDraftHd.EstimatedDuration;
                        entityHd.IsUsedRequestTime = entityDraftHd.IsUsedRequestTime;
                        entityHd.IsEmergency = entityDraftHd.IsEmergency;
                        entityHd.IsUsingSpecificItem = entityDraftHd.IsUsingSpecificItem;
                        entityHd.Remarks = entityDraftHd.Remarks;
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                        entityHd.ProposedBy = AppSession.UserLogin.UserID;
                        entityHd.ProposedDate = DateTime.Now;
                        entityHd.Remarks = string.Format("From Draft Order ({0})", entityDraftHd.DraftTestOrderNo);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        testOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);

                        List<vDraftTestOrderDt> lstDraftOtherOrderPerHd = lstDraftOtherOrder.Where(t => t.DraftTestOrderID == e.DraftTestOrderID).ToList();
                        if (lstDraftOtherOrderPerHd.Count > 0)
                        {
                            foreach (vDraftTestOrderDt x in lstDraftOtherOrderPerHd)
                            {
                                DraftTestOrderDt draftTestorderDt = draftEntityOrderDtDao.Get(x.ID);
                                TestOrderDt entityDt = new TestOrderDt();
                                entityDt.ItemID = draftTestorderDt.ItemID;
                                entityDt.DiagnoseID = draftTestorderDt.DiagnoseID;
                                entityDt.IsCITO = draftTestorderDt.IsCITO;
                                entityDt.Remarks = draftTestorderDt.Remarks;
                                entityDt.TestOrderID = testOrderID;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                entityDt.ItemQty = draftTestorderDt.ItemQty;
                                entityDt.ItemUnit = draftTestorderDt.ItemUnit;
                                entityOrderDtDao.Insert(entityDt);

                                draftTestorderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CLOSED;
                                draftTestorderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityOrderDtDao.Update(draftTestorderDt);

                                DraftTestOrderHd draftOrderHd = draftEntityOrderHdDao.Get(x.DraftTestOrderID);
                                if (draftOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<DraftTestOrderDt> lstdraftOrder = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND GCDraftTestOrderStatus = '{1}' AND IsDeleted = 0", draftOrderHd.DraftTestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                                    if (lstdraftOrder.Count <= 0)
                                    {
                                        if (String.IsNullOrEmpty(draftOrderHd.Remarks))
                                        {
                                            draftOrderHd.Remarks = string.Format("Reference No : {0}", entityHd.TestOrderNo);
                                        }
                                        else
                                        {
                                            draftOrderHd.Remarks = draftOrderHd.Remarks + string.Format(" | Reference No : {0}", entityHd.TestOrderNo);
                                        }

                                        draftOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        draftOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        draftEntityOrderHdDao.Update(draftOrderHd);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int transactionID;
                        PatientChargesHd patientChargesHd = new PatientChargesHd();
                        Int32 chargeClassID = Convert.ToInt32(BusinessLayer.GetConsultVisit(visit.VisitID).ChargeClassID);

                        patientChargesHd.VisitID = visit.VisitID;
                        patientChargesHd.LinkedChargesID = null;
                        //patientChargesHd.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                        patientChargesHd.HealthcareServiceUnitID = e.HealthcareServiceUnitID;
                        patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        patientChargesHd.TransactionDate = DateTime.Now;
                        patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;

                        string remarks = string.Format("From Draft Order ({0})", e.DraftTestOrderNo);
                        if (!string.IsNullOrEmpty(e.Remarks))
                        {
                            remarks += string.Format("|{0}", e.Remarks);
                        }
                        patientChargesHd.Remarks = remarks;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        transactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

                        List<vDraftTestOrderDt> lstDraftOtherOrderPerHd = lstDraftOtherOrder.Where(t => t.DraftTestOrderID == e.DraftTestOrderID).ToList();

                        List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                        int ct = 0;

                        if (lstDraftOtherOrderPerHd.Count > 0)
                        {
                            foreach (vDraftTestOrderDt x in lstDraftOtherOrderPerHd)
                            {
                                PatientChargesDt patientChargesDt = new PatientChargesDt();
                                DraftTestOrderDt draftTestorderDt = BusinessLayer.GetDraftTestOrderDt(x.ID);

                                patientChargesDt.TransactionID = transactionID;
                                patientChargesDt.ItemID = x.ItemID;
                                patientChargesDt.ChargeClassID = chargeClassID;

                                if (x.ItemPackageID != 0)
                                {
                                    patientChargesDt.ItemPackageID = x.ItemPackageID;
                                }
                                //                        patientChargesDt.ReferenceDtID = testDt.ID;

                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registration.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, x.ItemID, 1, DateTime.Now, ctx);

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

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", x.ItemID), ctx).FirstOrDefault();
                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                                patientChargesDt.ParamedicID = x.ParamedicID;
                                patientChargesDt.BusinessPartnerID = x.BusinessPartnerID;
                                if (patientChargesDt.BusinessPartnerID != null)
                                {
                                    patientChargesDt.IsSubContractItem = true;
                                }
                                else
                                {
                                    patientChargesDt.IsSubContractItem = false;
                                }

                                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                if (patientChargesDt.RevenueSharingID == 0)
                                    patientChargesDt.RevenueSharingID = null;
                                patientChargesDt.IsVariable = false;
                                patientChargesDt.IsUnbilledItem = false;

                                decimal grossLineAmount = x.ItemQty * price;

                                patientChargesDt.IsCITO = false;
                                patientChargesDt.CITOAmount = 0;
                                patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                                patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;
                                if (x.IsCito)
                                {
                                    patientChargesDt.IsCITO = true;
                                    if (entityItemMaster.IsCITOInPercentage)
                                        patientChargesDt.CITOAmount = (entityItemMaster.CITOAmount * grossLineAmount) / 100;
                                    else
                                        patientChargesDt.CITOAmount = entityItemMaster.CITOAmount;
                                    grossLineAmount += patientChargesDt.CITOAmount;
                                }

                                patientChargesDt.IsComplication = false;
                                patientChargesDt.ComplicationAmount = 0;

                                //patientChargesDt.IsDiscount = false;
                                //patientChargesDt.DiscountAmount = totalDiscountAmount;

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

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (x.ItemQty);

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

                                patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                                patientChargesDt.DiscountAmount = totalDiscountAmount;
                                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = x.ItemQty;

                                patientChargesDt.PatientAmount = total - totalPayer;
                                patientChargesDt.PayerAmount = totalPayer;
                                patientChargesDt.LineAmount = total;

                                decimal oPatientAmount = patientChargesDt.PatientAmount;
                                decimal oPayerAmount = patientChargesDt.PayerAmount;
                                decimal oLineAmount = patientChargesDt.LineAmount;

                                if (oIsEndingAmountRoundingTo1 == "1")
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
                                    if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                    {
                                        oPayerAmount = Math.Floor(oPayerAmount);
                                    }
                                    else
                                    {
                                        oPayerAmount = Math.Ceiling(oPayerAmount);
                                    }

                                    oLineAmount = oPatientAmount + oPayerAmount;
                                }

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                ItemMaster ims = entityItemMasterDao.Get(patientChargesDt.ItemID);
                                if (ims.GCItemType == Constant.ItemType.OBAT_OBATAN || ims.GCItemType == Constant.ItemType.BARANG_MEDIS || ims.GCItemType == Constant.ItemType.BARANG_UMUM || ims.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                                {
                                    if (oIsEndingAmountRoundingTo100 == "1")
                                    {
                                        oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                        oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                        oLineAmount = oPatientAmount + oPayerAmount;
                                    }
                                }

                                patientChargesDt.PatientAmount = oPatientAmount;
                                patientChargesDt.PayerAmount = oPayerAmount;
                                patientChargesDt.LineAmount = oLineAmount;

                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                                if (isUsingMultiVisitSchedule == "1")
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    List<DiagnosticVisitSchedule> lstSchedule = BusinessLayer.GetDiagnosticVisitScheduleList(string.Format("AppointmentID = {0} AND IsDeleted = 0 AND ItemID = {1}", registration.AppointmentID, x.ItemID), ctx);
                                    if (lstSchedule.Count > 0)
                                    {
                                        foreach (DiagnosticVisitSchedule schedule in lstSchedule)
                                        {
                                            schedule.RealDate = DateTime.Now;
                                            schedule.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.COMPLETED;
                                            schedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            schedule.LastUpdatedDate = DateTime.Now;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            diagVisitScheduleDao.Update(schedule);

                                            patientChargesDt.DiagnosticVisitScheduleID = schedule.ID;
                                        }
                                    }
                                }

                                lstPatientChargesDt.Add(patientChargesDt);
                                ct++;

                                draftTestorderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CLOSED;
                                draftTestorderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityOrderDtDao.Update(draftTestorderDt);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                DraftTestOrderHd draftOrderHd = draftEntityOrderHdDao.Get(x.DraftTestOrderID);
                                if (draftOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<DraftTestOrderDt> lstdraftOrder = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND GCDraftTestOrderStatus = '{1}' AND IsDeleted = 0", draftOrderHd.DraftTestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                                    if (lstdraftOrder.Count <= 0)
                                    {
                                        if (String.IsNullOrEmpty(draftOrderHd.Remarks))
                                        {
                                            draftOrderHd.Remarks = string.Format("Reference No : {0}", patientChargesHd.TransactionNo);
                                        }
                                        else
                                        {
                                            draftOrderHd.Remarks = draftOrderHd.Remarks + string.Format(" | Reference No : {0}", patientChargesHd.TransactionNo);
                                        }
                                        draftOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        draftOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        draftEntityOrderHdDao.Update(draftOrderHd);
                                    }
                                }
                            }
                            if (result)
                            {
                                foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                {
                                    //patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                    int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                    List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
                                    foreach (vItemServiceDt isd in isdList)
                                    {
                                        PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                        dtpackage.PatientChargesDtID = ID;
                                        dtpackage.ItemID = isd.DetailItemID;
                                        dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, patientChargesHd.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                        if (revID != 0 && revID != null)
                                        {
                                            dtpackage.RevenueSharingID = revID;
                                        }
                                        else
                                        {
                                            dtpackage.RevenueSharingID = null;
                                        }

                                        dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

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
                                        decimal grossLineAmount = 0;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

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

                                        if (grossLineAmount > 0)
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
                                        entityDtPackageDao.Insert(dtpackage);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ////Check if Test Order only has header i.e Surgery Order
                //DraftTestOrderHd oTestOrderHd = BusinessLayer.GetDraftTestOrderHdList();
            }
            #endregion

            ctx.CommitTransaction();
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            return result;
        }

        protected bool onSavePrescriptionOrder(Registration registration, vConsultVisit4 visit, List<DraftPrescriptionOrderDt> lstDraftPrescriptionOrderDt)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DraftPrescriptionOrderHdDao draftEntityPrescriptionHdDao = new DraftPrescriptionOrderHdDao(ctx);
            DraftPrescriptionOrderDtDao draftEntityPrescriptionDtDao = new DraftPrescriptionOrderDtDao(ctx);

            PrescriptionOrderHdDao entityPrescriptionHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityPrescriptionDtDao = new PrescriptionOrderDtDao(ctx);

            var lstDraftPrescriptionOrderDtGroupByHD = lstDraftPrescriptionOrderDt.GroupBy(test => test.DraftPrescriptionOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.DraftPrescriptionOrderID);
            List<DraftPrescriptionOrderHd> lstDraftPrescriptionOrderHd = new List<DraftPrescriptionOrderHd>();

            foreach (DraftPrescriptionOrderDt y in lstDraftPrescriptionOrderDtGroupByHD)
            {
                DraftPrescriptionOrderHd DraftPrescriptionOrderHd = BusinessLayer.GetDraftPrescriptionOrderHd(y.DraftPrescriptionOrderID);
                lstDraftPrescriptionOrderHd.Add(DraftPrescriptionOrderHd);
            }

            if (lstDraftPrescriptionOrderHd.Count > 0)
            {
                foreach (DraftPrescriptionOrderHd h in lstDraftPrescriptionOrderHd)
                {
                    int prescriptionOrderID;
                    PrescriptionOrderHd entityHd = new PrescriptionOrderHd();

                    entityHd.ParamedicID = h.ParamedicID;
                    entityHd.VisitID = visit.VisitID;
                    entityHd.VisitHealthcareServiceUnitID = visit.HealthcareServiceUnitID;
                    entityHd.PrescriptionDate = DateTime.Now;
                    entityHd.PrescriptionTime = DateTime.Now.ToString("HH:mm");
                    entityHd.ClassID = visit.ClassID;
                    entityHd.DispensaryServiceUnitID = h.DispensaryServiceUnitID;
                    entityHd.LocationID = h.LocationID;
                    entityHd.GCPrescriptionType = h.GCPrescriptionType;
                    if (visit.DepartmentID == Constant.Facility.INPATIENT)
                        entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                    else if (visit.DepartmentID == Constant.Facility.OUTPATIENT)
                        entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                    else
                        entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                    entityHd.Remarks = h.Remarks;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entityHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                    entityHd.ProposedBy = AppSession.UserLogin.UserID;
                    entityHd.ProposedDate = DateTime.Now;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.Remarks = string.Format("From Draft Charges ({0})", h.DraftPrescriptionOrderNo);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    prescriptionOrderID = entityPrescriptionHdDao.InsertReturnPrimaryKeyID(entityHd);

                    List<DraftPrescriptionOrderDt> lstDraftPrescriptionOrderDtPerHD = lstDraftPrescriptionOrderDt.Where(t => t.DraftPrescriptionOrderID == h.DraftPrescriptionOrderID).ToList();
                    foreach (DraftPrescriptionOrderDt k in lstDraftPrescriptionOrderDtPerHD)
                    {
                        PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                        DraftPrescriptionOrderDt draftPrescriptionDt = BusinessLayer.GetDraftPrescriptionOrderDt(k.DraftPrescriptionOrderDetailID);

                        entityDt.PrescriptionOrderID = prescriptionOrderID;
                        entityDt.IsRFlag = k.IsRFlag;
                        entityDt.ItemID = k.ItemID;
                        entityDt.DrugName = k.DrugName;
                        entityDt.GenericName = k.GenericName;
                        entityDt.SignaID = k.SignaID;
                        entityDt.GCDrugForm = k.GCDrugForm;
                        entityDt.GCCoenamRule = k.GCCoenamRule;
                        entityDt.Dose = k.Dose;
                        entityDt.GCDoseUnit = k.GCDoseUnit;
                        entityDt.GCDosingFrequency = k.GCDosingFrequency;
                        entityDt.Frequency = k.Frequency;
                        entityDt.NumberOfDosage = k.NumberOfDosage;
                        entityDt.ConversionFactor = k.ConversionFactor;
                        entityDt.GCDosingUnit = k.GCDosingUnit;
                        entityDt.GCRoute = k.GCRoute;
                        entityDt.StartDate = DateTime.Now;
                        entityDt.StartTime = DateTime.Now.ToString("HH:mm");
                        entityDt.MedicationPurpose = k.MedicationPurpose;
                        entityDt.MedicationAdministration = k.MedicationAdministration;
                        entityDt.DosingDuration = k.DosingDuration;
                        entityDt.DispenseQty = k.DispenseQty;
                        //entity.TakenQty = Convert.ToDecimal(hdnTakenQty.Value) == 0 ? entity.DispenseQty : Convert.ToDecimal(hdnTakenQty.Value);
                        entityDt.TakenQty = k.TakenQty;
                        entityDt.ResultQty = k.ResultQty;
                        entityDt.ChargeQty = k.ChargeQty;
                        entityDt.IsCreatedFromOrder = k.IsCreatedFromOrder;
                        entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        entityPrescriptionDtDao.Insert(entityDt);

                        draftPrescriptionDt.GCDraftPrescriptionOrderStatus = Constant.TestOrderStatus.CLOSED;
                        draftPrescriptionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        draftEntityPrescriptionDtDao.Update(draftPrescriptionDt);

                        DraftPrescriptionOrderHd draftPrescriptionOrderHd = draftEntityPrescriptionHdDao.Get(k.DraftPrescriptionOrderID);
                        if (draftPrescriptionOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            List<DraftPrescriptionOrderDt> lstdraftPrescriptionOrderDt = BusinessLayer.GetDraftPrescriptionOrderDtList(string.Format("DraftPrescriptionOrderID = {0} AND GCDraftPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", draftPrescriptionOrderHd.DraftPrescriptionOrderID, Constant.TestOrderStatus.OPEN), ctx);
                            if (lstdraftPrescriptionOrderDt.Count <= 0)
                            {
                                if (String.IsNullOrEmpty(draftPrescriptionOrderHd.Remarks))
                                {
                                    draftPrescriptionOrderHd.Remarks = string.Format("Reference No : {0}", entityHd.PrescriptionOrderNo);
                                }
                                else
                                {
                                    draftPrescriptionOrderHd.Remarks = draftPrescriptionOrderHd.Remarks + string.Format(" | Reference No : {0}", entityHd.PrescriptionOrderNo);
                                }
                                draftPrescriptionOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                draftPrescriptionOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftEntityPrescriptionHdDao.Update(draftPrescriptionOrderHd);
                            }
                        }
                    }
                }
                ctx.CommitTransaction();
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
            }
            return result;
        }

        protected abstract void BindGrid();
    }
}
