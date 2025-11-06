using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using System.Globalization;
using DevExpress.Web.ASPxEditors;
using System.IO;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Common
{
    public partial class Helper
    {
        #region INSERT LOG PrescriptionOrderTaskLog
        public static void InsertPrescriptionOrderTaskLog(IDbContext ctx, int PrescriptionOrderID, string GCTaskLogStatus, int CreatedBy, Boolean isAPS)
        {
            if (!string.IsNullOrEmpty(GCTaskLogStatus))
            {
                PrescriptionOrderTaskLogDao pDao = new PrescriptionOrderTaskLogDao(ctx);
                PrescriptionOrderHdInfoDao pOrderHDInfoDao = new PrescriptionOrderHdInfoDao(ctx);

                PrescriptionOrderHdInfo oHdInfo = BusinessLayer.GetPrescriptionOrderHdInfoList(string.Format("PrescriptionOrderID='{0}'", PrescriptionOrderID), ctx).FirstOrDefault();
                if (oHdInfo != null)
                {
                    switch (GCTaskLogStatus)
                    {
                        case Constant.PrescriptionTaskLogStatus.Sent:
                            oHdInfo.PrescriptionSentBy = CreatedBy;
                            oHdInfo.PrescriptionSentDateTime = DateTime.Now;
                            oHdInfo.PrescriptionReceivedBy = null;
                            oHdInfo.PrescriptionReceivedDateTime = null;
                            oHdInfo.PrescriptionStartedBy = null;
                            oHdInfo.PrescriptionStartedDateTime = null;
                            oHdInfo.PrescriptionCompletedBy = null;
                            oHdInfo.PrescriptionCompletedDateTime = null;
                            oHdInfo.PrescriptionClosedBy = null;
                            oHdInfo.PrescriptionClosedDateTime = null;
                            break;
                        case Constant.PrescriptionTaskLogStatus.Received:
                            oHdInfo.PrescriptionReceivedBy = CreatedBy;
                            oHdInfo.PrescriptionReceivedDateTime = DateTime.Now;
                            break;
                        case Constant.PrescriptionTaskLogStatus.Started:
                            oHdInfo.PrescriptionStartedBy = CreatedBy;
                            oHdInfo.PrescriptionStartedDateTime = DateTime.Now;
                            break;
                        case Constant.PrescriptionTaskLogStatus.Completed:
                            oHdInfo.PrescriptionCompletedBy = CreatedBy;
                            oHdInfo.PrescriptionCompletedDateTime = DateTime.Now;
                            break;
                        case Constant.PrescriptionTaskLogStatus.Closed:
                            oHdInfo.PrescriptionClosedBy = CreatedBy;
                            oHdInfo.PrescriptionClosedDateTime = DateTime.Now;
                            break;
                        case Constant.PrescriptionTaskLogStatus.Reopen:
                            oHdInfo.PrescriptionReopenBy = CreatedBy;
                            oHdInfo.PrescriptionReopenDateTime = DateTime.Now;

                            oHdInfo.PrescriptionSentBy = null;
                            oHdInfo.PrescriptionSentDateTime = null;
                            oHdInfo.PrescriptionReceivedBy = null;
                            oHdInfo.PrescriptionReceivedDateTime = null;
                            oHdInfo.PrescriptionStartedBy = null;
                            oHdInfo.PrescriptionStartedDateTime = null;
                            oHdInfo.PrescriptionCompletedBy = null;
                            oHdInfo.PrescriptionCompletedDateTime = null;
                            oHdInfo.PrescriptionClosedBy = null;
                            oHdInfo.PrescriptionClosedDateTime = null;

                            break;
                        case Constant.PrescriptionTaskLogStatus.Void:
                            oHdInfo.PrescriptionVoidBy = CreatedBy;
                            oHdInfo.PrescriptionVoidDateTime = DateTime.Now;
                            break;
                    }
                    oHdInfo.GCTaskLogStatus = GCTaskLogStatus;
                    pOrderHDInfoDao.Update(oHdInfo);

                    PrescriptionOrderTaskLog oData = new PrescriptionOrderTaskLog();
                    oData.GCTaskLogStatus = GCTaskLogStatus;
                    oData.PrescriptionOrderID = PrescriptionOrderID;
                    oData.CreatedBy = CreatedBy;
                    oData.CreatedDate = DateTime.Now;
                    pDao.Insert(oData);

                }
            }

        }
        #endregion

        #region Auto Bill Item
        public static void InsertAutoBillItem(IDbContext ctx, ConsultVisit entityVisit, String DepartmentID, Int32 ClassID, String GCCustomerType, bool IsPrintingPatientCard, int cvItemID, string ChargeCodeAdministration = "", bool skipAdm = false)
        {
            List<PatientChargesDt> lstPatientChargesDt = GetAutoBillItemPatientChargesDt(ctx, entityVisit, ClassID, GCCustomerType, IsPrintingPatientCard, ChargeCodeAdministration, skipAdm);

            if (lstPatientChargesDt.Count > 0)
            {
                InsertAutoBillItemPatientCharges(ctx, entityVisit, DepartmentID, lstPatientChargesDt);
            }

            if (cvItemID > 0)
            {
                InsertConsultVisitItemPackage(ctx, entityVisit, DepartmentID, cvItemID, 1);
            }
        }

        public static void InsertPatientCardBillItem(IDbContext ctx, ConsultVisit entityVisit, String DepartmentID, Int32 ClassID)
        {
            List<PatientChargesDt> lstPatientChargesDt = GetPatientCardChargesDt(ctx, entityVisit, ClassID);
            if (lstPatientChargesDt.Count > 0)
                InsertPatientCardCharges(ctx, entityVisit, DepartmentID, lstPatientChargesDt);
        }

        public static void InsertConsultVisitItemPackage(IDbContext ctx, ConsultVisit entityVisit, String DepartmentID, int cvItemID, int IsMainPackage)
        {
            ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);

            ConsultVisitItemPackage consultVisitItemPackage = new ConsultVisitItemPackage();
            consultVisitItemPackage.VisitID = entityVisit.VisitID;
            consultVisitItemPackage.ItemID = cvItemID;
            consultVisitItemPackage.Quantity = 1;

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

            decimal totalBaseAmount = 0;
            decimal totalBaseAmount1 = 0;
            decimal totalBaseAmount2 = 0;
            decimal totalBaseAmount3 = 0;

            decimal totalAmount = 0;
            decimal totalAmount1 = 0;
            decimal totalAmount2 = 0;
            decimal totalAmount3 = 0;

            decimal totalDiscountAmount = 0;
            decimal totalDiscountAmount1 = 0;
            decimal totalDiscountAmount2 = 0;
            decimal totalDiscountAmount3 = 0;

            ItemService iSrv = itemServiceDao.Get(consultVisitItemPackage.ItemID);

            string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", consultVisitItemPackage.ItemID);
            List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
            if (isdList.Count() > 0 && iSrv.IsUsingAccumulatedPrice)
            {
                decimal discAmount = 0, discComp1 = 0, discComp2 = 0, discComp3 = 0;

                foreach (vItemServiceDt isd in isdList)
                {
                    basePriceComp1 = 0;
                    basePriceComp2 = 0;
                    basePriceComp3 = 0;

                    priceComp1 = 0;
                    priceComp2 = 0;
                    priceComp3 = 0;

                    discountAmountComp1 = 0;
                    discountAmountComp2 = 0;
                    discountAmountComp3 = 0;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int itemType = isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, (int)entityVisit.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now, ctx).FirstOrDefault();
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

                    if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                    {
                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                if (isDiscountInPercentageComp1)
                                {
                                    totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
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
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                            }
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

                    totalBaseAmount1 += basePriceComp1 * isd.Quantity;
                    totalBaseAmount2 += basePriceComp2 * isd.Quantity;
                    totalBaseAmount3 += basePriceComp3 * isd.Quantity;
                    totalBaseAmount += (basePriceComp1 + basePriceComp2 + basePriceComp3) * isd.Quantity;

                    //totalAmount1 += priceComp1 - totalDiscountAmount1; //[ad] kasus mcu seharusnya final tanpa diskon
                    //totalAmount2 += priceComp2 - totalDiscountAmount2;
                    //totalAmount3 += priceComp3 - totalDiscountAmount3;
                    totalAmount1 += priceComp1 * isd.Quantity;
                    totalAmount2 += priceComp2 * isd.Quantity;
                    totalAmount3 += priceComp3 * isd.Quantity;
                    totalAmount += (priceComp1 + priceComp2 + priceComp3) * isd.Quantity;

                    discComp1 += isd.DiscountComp1 * isd.Quantity;
                    discComp2 += isd.DiscountComp2 * isd.Quantity;
                    discComp3 += isd.DiscountComp3 * isd.Quantity;
                    discAmount += (isd.DiscountComp1 + isd.DiscountComp2 + isd.DiscountComp3) * isd.Quantity;

                }

                consultVisitItemPackage.BaseTariff = totalBaseAmount;
                consultVisitItemPackage.BaseComp1 = totalBaseAmount1;
                consultVisitItemPackage.BaseComp2 = totalBaseAmount2;
                consultVisitItemPackage.BaseComp3 = totalBaseAmount3;

                consultVisitItemPackage.Tariff = totalAmount - discAmount;
                consultVisitItemPackage.TariffComp1 = totalAmount1 - discComp1;
                consultVisitItemPackage.TariffComp2 = totalAmount2 - discComp2;
                consultVisitItemPackage.TariffComp3 = totalAmount3 - discComp3;

            }
            else
            {
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, (int)entityVisit.ChargeClassID, consultVisitItemPackage.ItemID, 1, entityVisit.VisitDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

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

                consultVisitItemPackage.BaseTariff = basePrice;
                consultVisitItemPackage.BaseComp1 = basePriceComp1;
                consultVisitItemPackage.BaseComp2 = basePriceComp2;
                consultVisitItemPackage.BaseComp3 = basePriceComp3;

                consultVisitItemPackage.Tariff = price;
                consultVisitItemPackage.TariffComp1 = priceComp1;
                consultVisitItemPackage.TariffComp2 = priceComp2;
                consultVisitItemPackage.TariffComp3 = priceComp3;

            }

            consultVisitItemPackage.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
            if (IsMainPackage == 1)
            {
                consultVisitItemPackage.IsMainPackage = true;
            }
            else
            {
                consultVisitItemPackage.IsMainPackage = false;
            }
            consultVisitItemPackage.IsDeleted = false;
            consultVisitItemPackage.CreatedBy = Convert.ToInt32(entityVisit.CreatedBy);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            consultVisitItemPackageDao.Insert(consultVisitItemPackage);

            ////if (iSrv.IsPackageAllInOne)
            ////{
            ////    InsertAutoBillPatientChargesControlAmountAIO(ctx, entityVisit, DepartmentID, consultVisitItemPackage.ItemID);
            ////}
        }

        #region Auto Bill Item Charges Dt
        private static vServiceUnitAutoBillItem GetChargesCardFee(IDbContext ctx)
        {
            SettingParameterDtDao parameterDao = new SettingParameterDtDao(ctx);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);

            vServiceUnitAutoBillItem entity = null;
            SettingParameterDt oParameter = parameterDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU);
            if (oParameter != null)
            {
                if (oParameter.ParameterValue != "" && oParameter.ParameterValue != "0")
                {
                    entity = new vServiceUnitAutoBillItem();
                    entity.ItemID = Convert.ToInt32(oParameter.ParameterValue);
                    entity.Quantity = 1;
                    ItemMaster itemMaster = itemMasterDao.Get(entity.ItemID);
                    entity.ItemCode = itemMaster.ItemCode;
                    entity.GCItemUnit = itemMaster.GCItemUnit;
                }
            }
            return entity;
        }

        private static List<PatientChargesDt> GetAutoBillItemPatientChargesDt(IDbContext ctx, ConsultVisit entityVisit, Int32 ClassID, String GCCustomerType, bool IsPrintingPatientCard, string ChargeCodeAdministration, bool skipAdm)
        {
            List<vServiceUnitAutoBillItem> lstServiceUnitAutoBillItem = new List<vServiceUnitAutoBillItem>();
            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                    AppSession.UserLogin.HealthcareID, //0
                                                    Constant.SettingParameter.SA_AUTOBILL_ITEM_PRIORITY_FROM_PARAMEDIC, //1
                                                    Constant.SettingParameter.SA_AUTOBILL_CHARGES_PARAMEDIC_FROM_DEFAULTPARAMEDIC, //2
                                                    Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI, //3
                                                    Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI //4
                                                );
            List<SettingParameterDt> setParDtList = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            string valueSA0163 = setParDtList.Where(a => a.ParameterCode == Constant.SettingParameter.SA_AUTOBILL_ITEM_PRIORITY_FROM_PARAMEDIC).FirstOrDefault().ParameterValue;
            string valueSA0164 = setParDtList.Where(a => a.ParameterCode == Constant.SettingParameter.SA_AUTOBILL_CHARGES_PARAMEDIC_FROM_DEFAULTPARAMEDIC).FirstOrDefault().ParameterValue;

            string valueFN0014 = setParDtList.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI).FirstOrDefault().ParameterValue;
            string valueFN0030 = setParDtList.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI).FirstOrDefault().ParameterValue;

            if (valueSA0163 == "1")
            {
                #region autobill paramedic
                string filterExpressionAutoBillParamedic = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND IsDeleted = 0", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID);
                if (!entityVisit.IsMainVisit)
                    filterExpressionAutoBillParamedic += " AND IsAdministrationItem = 0";

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<vAutoBillItemParamedic> lstAutoBillItemParamedic = BusinessLayer.GetvAutoBillItemParamedicList(filterExpressionAutoBillParamedic, ctx);
                #endregion

                if (lstAutoBillItemParamedic.Count <= 0)
                {
                    string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", entityVisit.HealthcareServiceUnitID);
                    if (!entityVisit.IsMainVisit)
                        filterExpression += " AND IsAdministrationItem = 0";

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    lstServiceUnitAutoBillItem = BusinessLayer.GetvServiceUnitAutoBillItemList(filterExpression, ctx);
                }
                else
                {
                    List<vServiceUnitAutoBillItem> lstServiceUnitAutoBillItemTemp = new List<vServiceUnitAutoBillItem>();
                    foreach (vAutoBillItemParamedic e in lstAutoBillItemParamedic)
                    {
                        vServiceUnitAutoBillItem entityTemp = new vServiceUnitAutoBillItem();
                        entityTemp.ItemID = e.ItemID;
                        entityTemp.IsAdministrationItem = e.IsAdministrationItem;
                        entityTemp.GCItemUnit = e.GCItemUnit;
                        entityTemp.Quantity = e.Quantity;
                        entityTemp.ItemCode = e.ItemCode;
                        lstServiceUnitAutoBillItemTemp.Add(entityTemp);
                    }
                    lstServiceUnitAutoBillItem = lstServiceUnitAutoBillItemTemp;
                }
            }
            else
            {
                string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", entityVisit.HealthcareServiceUnitID);
                if (!entityVisit.IsMainVisit)
                    filterExpression += " AND IsAdministrationItem = 0";

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstServiceUnitAutoBillItem = BusinessLayer.GetvServiceUnitAutoBillItemList(filterExpression, ctx);

                string filterExpressionAutoBillParamedic = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND IsDeleted = 0", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID);
                if (!entityVisit.IsMainVisit)
                    filterExpressionAutoBillParamedic += " AND IsAdministrationItem = 0";

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<vAutoBillItemParamedic> lstAutoBillItemParamedic = BusinessLayer.GetvAutoBillItemParamedicList(filterExpressionAutoBillParamedic, ctx);
                if (lstAutoBillItemParamedic.Count > 0)
                {
                    //List<vServiceUnitAutoBillItem> lstServiceUnitAutoBillItemTemp = new List<vServiceUnitAutoBillItem>();
                    foreach (vAutoBillItemParamedic e in lstAutoBillItemParamedic)
                    {
                        vServiceUnitAutoBillItem entityTemp = new vServiceUnitAutoBillItem();
                        entityTemp.ItemID = e.ItemID;
                        entityTemp.IsAdministrationItem = e.IsAdministrationItem;
                        entityTemp.GCItemUnit = e.GCItemUnit;
                        entityTemp.Quantity = e.Quantity;
                        entityTemp.ItemCode = e.ItemCode;
                        lstServiceUnitAutoBillItem.Add(entityTemp);
                    }
                }
            }

            if (IsPrintingPatientCard)
            {
                vServiceUnitAutoBillItem oItem = GetChargesCardFee(ctx);
                if (oItem != null)
                {
                    lstServiceUnitAutoBillItem.Add(oItem);
                }
            }

            if (lstServiceUnitAutoBillItem.Count() == 0)
            {
                if (GCCustomerType != Constant.CustomerType.PERSONAL)
                {
                    if (valueFN0014 != null && valueFN0014 != "")
                    {
                        string filterItemSetVar = string.Format("ItemID IN ({0}) AND IsDeleted = 0", valueFN0014);
                        List<ItemMaster> lstIM = BusinessLayer.GetItemMasterList(filterItemSetVar);
                        foreach (ItemMaster e in lstIM)
                        {
                            vServiceUnitAutoBillItem entitySetvar = new vServiceUnitAutoBillItem();
                            entitySetvar.ItemID = e.ItemID;
                            entitySetvar.IsAdministrationItem = false;
                            entitySetvar.GCItemUnit = e.GCItemUnit;
                            entitySetvar.Quantity = 1;
                            entitySetvar.ItemCode = e.ItemCode;
                            lstServiceUnitAutoBillItem.Add(entitySetvar);
                        }
                    }
                }
                else
                {
                    if (valueFN0030 != null && valueFN0030 != "")
                    {
                        string filterItemSetVar = string.Format("ItemID IN ({0}) AND IsDeleted = 0", valueFN0030);
                        List<ItemMaster> lstIM = BusinessLayer.GetItemMasterList(filterItemSetVar);
                        foreach (ItemMaster e in lstIM)
                        {
                            vServiceUnitAutoBillItem entitySetvar = new vServiceUnitAutoBillItem();
                            entitySetvar.ItemID = e.ItemID;
                            entitySetvar.IsAdministrationItem = false;
                            entitySetvar.GCItemUnit = e.GCItemUnit;
                            entitySetvar.Quantity = 1;
                            entitySetvar.ItemCode = e.ItemCode;
                            lstServiceUnitAutoBillItem.Add(entitySetvar);
                        }
                    }
                }
            }

            foreach (vServiceUnitAutoBillItem serviceUnitAutoBillItem in lstServiceUnitAutoBillItem)
            {
                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.ItemID = serviceUnitAutoBillItem.ItemID;
                patientChargesDt.ChargeClassID = ClassID;
                if (serviceUnitAutoBillItem.IsAdministrationItem)
                {
                    if (skipAdm) continue;
                    if (!ChargeCodeAdministration.Equals(string.Empty))
                    {
                        patientChargesDt.ItemID = Convert.ToInt32(ChargeCodeAdministration);
                    }
                }

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, patientChargesDt.ChargeClassID, patientChargesDt.ItemID, 1, entityVisit.VisitDate, ctx);

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

                patientChargesDt.BaseTariff = basePrice;
                patientChargesDt.Tariff = price;
                patientChargesDt.BaseComp1 = basePriceComp1;
                patientChargesDt.BaseComp2 = basePriceComp2;
                patientChargesDt.BaseComp3 = basePriceComp3;
                patientChargesDt.TariffComp1 = priceComp1;
                patientChargesDt.TariffComp2 = priceComp2;
                patientChargesDt.TariffComp3 = priceComp3;
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = serviceUnitAutoBillItem.GCItemUnit;

                if (valueSA0164 == "1")
                {
                    if (serviceUnitAutoBillItem.DefaultParamedicID != null && serviceUnitAutoBillItem.DefaultParamedicID != 0)
                    {
                        patientChargesDt.ParamedicID = serviceUnitAutoBillItem.DefaultParamedicID;
                    }
                    else
                    {
                        patientChargesDt.ParamedicID = (int)entityVisit.ParamedicID;
                    }
                }
                else
                {
                    patientChargesDt.ParamedicID = (int)entityVisit.ParamedicID;
                }

                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                decimal grossLineAmount = serviceUnitAutoBillItem.Quantity * price;

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

                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (serviceUnitAutoBillItem.Quantity);

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
                    totalPayer = coverageAmount * serviceUnitAutoBillItem.Quantity;
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

                patientChargesDt.IsCreatedBySystem = true;

                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;
                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;

                //patientChargesDt.IsDiscount = false;
                //patientChargesDt.DiscountAmount = totalDiscountAmount;

                patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                patientChargesDt.DiscountAmount = totalDiscountAmount;
                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = serviceUnitAutoBillItem.Quantity;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(serviceUnitAutoBillItem.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ctx).FirstOrDefault().RevenueSharingID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;

                patientChargesDt.CreatedBy = 0;
                lstPatientChargesDt.Add(patientChargesDt);
            }
            return lstPatientChargesDt;
        }

        private static List<PatientChargesDt> GetPatientCardChargesDt(IDbContext ctx, ConsultVisit entityVisit, Int32 ClassID)
        {
            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
            List<vServiceUnitAutoBillItem> lstServiceUnitAutoBillItem = new List<vServiceUnitAutoBillItem>();
            lstServiceUnitAutoBillItem.Add(GetChargesCardFee(ctx));
            foreach (vServiceUnitAutoBillItem serviceUnitAutoBillItem in lstServiceUnitAutoBillItem)
            {
                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.ItemID = serviceUnitAutoBillItem.ItemID;
                patientChargesDt.ChargeClassID = ClassID;

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, patientChargesDt.ChargeClassID, serviceUnitAutoBillItem.ItemID, 1, entityVisit.VisitDate, ctx);
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

                patientChargesDt.BaseTariff = basePrice;
                patientChargesDt.Tariff = price;
                patientChargesDt.BaseComp1 = basePriceComp1;
                patientChargesDt.BaseComp2 = basePriceComp2;
                patientChargesDt.BaseComp3 = basePriceComp3;
                patientChargesDt.TariffComp1 = priceComp1;
                patientChargesDt.TariffComp2 = priceComp2;
                patientChargesDt.TariffComp3 = priceComp3;
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = serviceUnitAutoBillItem.GCItemUnit;

                patientChargesDt.ParamedicID = (int)entityVisit.ParamedicID;

                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                decimal grossLineAmount = serviceUnitAutoBillItem.Quantity * price;

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

                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (serviceUnitAutoBillItem.Quantity);

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
                    totalPayer = coverageAmount * serviceUnitAutoBillItem.Quantity;
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
                patientChargesDt.CITOAmount = 0;
                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;

                //patientChargesDt.IsDiscount = false;
                //patientChargesDt.DiscountAmount = totalDiscountAmount;

                patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                patientChargesDt.DiscountAmount = totalDiscountAmount;
                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = serviceUnitAutoBillItem.Quantity;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(serviceUnitAutoBillItem.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ctx).FirstOrDefault().RevenueSharingID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;

                patientChargesDt.CreatedBy = 0;
                lstPatientChargesDt.Add(patientChargesDt);
            }
            return lstPatientChargesDt;
        }
        #endregion

        #region Insert Charges
        private static void InsertAutoBillItemPatientCharges(IDbContext ctx, ConsultVisit entityVisit, String DepartmentID, List<PatientChargesDt> lstPatientChargesDt)
        {
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);

            PatientChargesHd patientChargesHd = new PatientChargesHd();
            patientChargesHd.VisitID = entityVisit.VisitID;
            patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;

            switch (DepartmentID)
            {
                case Constant.Facility.MEDICAL_CHECKUP: patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES; break;
                case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                    else
                        patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
            }
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
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
            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
            {
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region PatientChargesDtPackage

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                    discountAmount = tariff.DiscountAmount;
                    coverageAmount = tariff.CoverageAmount;
                    price = tariff.Price;
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

                #endregion
            }
        }

        private static void InsertAutoBillPatientChargesControlAmountAIO(IDbContext ctx, ConsultVisit entityVisit, String DepartmentID, Int32 ItemID)
        {
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao imasterDao = new ItemMasterDao(ctx);

            Registration reg = registrationDao.Get(entityVisit.RegistrationID);

            string filterISD = string.Format("ItemID = {0} AND IsControlAmount = 1 AND IsDeleted = 0", ItemID);
            List<ItemServiceDt> lstISDt = BusinessLayer.GetItemServiceDtList(filterISD, ctx);
            foreach (ItemServiceDt isd in lstISDt)
            {
                string oDeptID = DepartmentID;
                PatientChargesHd patientChargesHd = new PatientChargesHd();
                patientChargesHd.VisitID = entityVisit.VisitID;
                if (isd.DepartmentID == Constant.Facility.INPATIENT)
                {
                    patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                }
                else
                {
                    oDeptID = isd.DepartmentID;

                    string filterHSU = string.Format("ServiceUnitID = {0} AND IsDeleted = 0", isd.ServiceUnitID);
                    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnitList(filterHSU, ctx).FirstOrDefault();
                    if (hsu != null)
                    {
                        patientChargesHd.HealthcareServiceUnitID = hsu.HealthcareServiceUnitID;
                    }
                    else
                    {
                        patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                    }
                }
                switch (oDeptID)
                {
                    case Constant.Facility.MEDICAL_CHECKUP: patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES; break;
                    case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                    case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else
                            patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                    default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                patientChargesHd.TransactionDate = DateTime.Now;
                patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
                patientChargesHd.PatientBillingID = null;
                patientChargesHd.ReferenceNo = null;
                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                patientChargesHd.GCVoidReason = null;
                patientChargesHd.IsAutoTransaction = true;
                patientChargesHd.IsAIOTransaction = true;
                patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                patientChargesHd.CreatedBy = 0;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int oTransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

                PatientChargesDt chargesDt = new PatientChargesDt();
                chargesDt.TransactionID = oTransactionID;
                chargesDt.ItemID = isd.DetailItemID;
                chargesDt.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                chargesDt.ChargeClassID = Convert.ToInt32(entityVisit.ChargeClassID);
                chargesDt.ChargedQuantity = isd.Quantity;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                ItemMaster imaster = imasterDao.Get(chargesDt.ItemID);
                chargesDt.GCBaseUnit = imaster.GCItemUnit;
                chargesDt.GCItemUnit = imaster.GCItemUnit;
                chargesDt.ConversionFactor = 1;

                decimal tariff = 0;
                decimal tariffComp1 = 0;
                decimal tariffComp2 = 0;
                decimal tariffComp3 = 0;

                decimal discountAmount = 0;
                decimal discountComp1 = 0;
                decimal discountComp2 = 0;
                decimal discountComp3 = 0;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterISDTariff = string.Format("ItemServiceDtID = {0} AND GCTariffScheme = '{1}' AND ClassID = {2} AND IsDeleted = 0", isd.ID, reg.GCTariffScheme, entityVisit.ChargeClassID);
                List<ItemServiceDtTariff> lstISDTariff = BusinessLayer.GetItemServiceDtTariffList(filterISDTariff, ctx);
                foreach (ItemServiceDtTariff isdtrf in lstISDTariff)
                {
                    tariff = isdtrf.Tariff;
                    tariffComp1 = isdtrf.TariffComp1;
                    tariffComp2 = isdtrf.TariffComp2;
                    tariffComp3 = isdtrf.TariffComp3;

                    discountAmount = isdtrf.DiscountAmount;
                    discountComp1 = isdtrf.DiscountComp1;
                    discountComp2 = isdtrf.DiscountComp2;
                    discountComp3 = isdtrf.DiscountComp3;
                }

                chargesDt.BaseTariff = tariff;
                chargesDt.BaseComp1 = tariffComp1;
                chargesDt.BaseComp2 = tariffComp2;
                chargesDt.BaseComp3 = tariffComp3;

                chargesDt.Tariff = tariff;
                chargesDt.TariffComp1 = tariffComp1;
                chargesDt.TariffComp2 = tariffComp2;
                chargesDt.TariffComp3 = tariffComp3;

                chargesDt.DiscountAmount = discountAmount;
                chargesDt.DiscountComp1 = discountComp1;
                chargesDt.DiscountComp2 = discountComp2;
                chargesDt.DiscountComp3 = discountComp3;

                if (reg.BusinessPartnerID != 1)
                {
                    chargesDt.PayerAmount = tariff - discountAmount;
                }
                else
                {
                    chargesDt.PatientAmount = tariff - discountAmount;
                }
                chargesDt.LineAmount = chargesDt.PayerAmount + chargesDt.PatientAmount;

                chargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                chargesDt.CreatedBy = 0;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                patientChargesDtDao.Insert(chargesDt);
            }
        }

        private static void InsertPatientCardCharges(IDbContext ctx, ConsultVisit entityVisit, String DepartmentID, List<PatientChargesDt> lstPatientChargesDt)
        {
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            PatientChargesHd patientChargesHd = new PatientChargesHd();
            patientChargesHd.VisitID = entityVisit.VisitID;
            patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;

            switch (DepartmentID)
            {
                case Constant.Facility.MEDICAL_CHECKUP: patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES; break;
                case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                    else
                        patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
            }
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
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
            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
            {
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.IsCreatedBySystem = true;
                patientChargesDtDao.Insert(patientChargesDt);
            }
        }
        #endregion
        #endregion

        #region Paramedic Schedule Availability, Maximum Appointment
        public static bool ValidateParamedicSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate, int paramedicID, int healthcareServiceUnitID, DateTime selectedDate, string departmentID, ref string errMessage, IDbContext ctx = null)
        {
            bool result = true;
            ParamedicMaster entityPM = null;
            vHealthcareServiceUnit entityHSU = null;

            #region validation paramedic schedule and max quota
            if (paramedicID > 0 && healthcareServiceUnitID > 0)
            {
                int dayNumber = (int)selectedDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }
                obj = BusinessLayer.GetvParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}", healthcareServiceUnitID, paramedicID, dayNumber), ctx).FirstOrDefault();

                objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'", healthcareServiceUnitID, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx).FirstOrDefault();

                ParamedicLeaveSchedule objSchDateLeave = BusinessLayer.GetParamedicLeaveScheduleList(string.Format("ParamedicID = {0} AND ('{1}' BETWEEN CONVERT(VARCHAR, StartDate,112) AND CONVERT(VARCHAR, EndDate, 112)) AND IsDeleted = 0", paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx).FirstOrDefault();

                if (objSchDateLeave == null)
                {
                    List<vAppointment> lstAppointment = new List<vAppointment>();
                    if (objSchDate != null)
                    {

                        lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND Session = 1", healthcareServiceUnitID, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                        if (!onCheckMaximumAppointment(lstAppointment, 1, dayNumber, ref errMessage, true, Convert.ToInt32(objSchDate.MaximumAppointment1), Convert.ToInt32(objSchDate.MaximumAppointment2), Convert.ToInt32(objSchDate.MaximumAppointment3), Convert.ToInt32(objSchDate.MaximumAppointment4), Convert.ToInt32(objSchDate.MaximumAppointment5)))
                        {
                            result = false;
                        }
                    }
                    else if (obj != null && objSchDate == null)
                    {
                        lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND Session = 1", healthcareServiceUnitID, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                        if (!onCheckMaximumAppointment(lstAppointment, 1, dayNumber, ref errMessage, true, Convert.ToInt32(obj.MaximumAppointment1), Convert.ToInt32(obj.MaximumAppointment2), Convert.ToInt32(obj.MaximumAppointment3), Convert.ToInt32(obj.MaximumAppointment4), Convert.ToInt32(obj.MaximumAppointment5)))
                        {
                            result = false;
                        }
                    }
                    else if (obj == null && objSchDate == null)
                    {
                        entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID), ctx).FirstOrDefault();
                        entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID), ctx).FirstOrDefault();
                        errMessage = string.Format("Tidak ada jadwal untuk dokter <b>{0}</b> di klinik <b>{1}</b>", entityPM.FullName, entityHSU.ServiceUnitName);
                        result = false;
                    }
                }
                else
                {
                    entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID), ctx).FirstOrDefault();
                    errMessage = string.Format("<b>{0}</b> sedang cuti dari <b>{1}</b> sampai dengan <b>{2}</b>", entityPM.FullName, objSchDateLeave.StartDate.ToString(Constant.FormatString.DATE_FORMAT), objSchDateLeave.EndDate.ToString(Constant.FormatString.DATE_FORMAT));
                    result = false;
                }

            }
            #endregion

            return result;
        }

        public static bool ValidateParamedicSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate, int paramedicID, int healthcareServiceUnitID, DateTime selectedDate, string departmentID, ref string errMessage)
        {
            bool result = true;
            ParamedicMaster entityPM = null;
            vHealthcareServiceUnit entityHSU = null;

            #region validation paramedic schedule and max quota
            if (paramedicID > 0 && healthcareServiceUnitID > 0)
            {
                int dayNumber = (int)selectedDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }
                obj = BusinessLayer.GetvParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}", healthcareServiceUnitID, paramedicID, dayNumber)).FirstOrDefault();

                objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'", healthcareServiceUnitID, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();

                ParamedicLeaveSchedule objSchDateLeave = BusinessLayer.GetParamedicLeaveScheduleList(string.Format("ParamedicID = {0} AND ('{1}' BETWEEN CONVERT(VARCHAR, StartDate,112) AND CONVERT(VARCHAR, EndDate, 112)) AND IsDeleted = 0", paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();

                if (objSchDateLeave == null)
                {
                    List<vAppointment> lstAppointment = new List<vAppointment>();
                    if (objSchDate != null)
                    {

                        lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND Session = 1", healthcareServiceUnitID, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                        if (!onCheckMaximumAppointment(lstAppointment, 1, dayNumber, ref errMessage, true, Convert.ToInt32(objSchDate.MaximumAppointment1), Convert.ToInt32(objSchDate.MaximumAppointment2), Convert.ToInt32(objSchDate.MaximumAppointment3), Convert.ToInt32(objSchDate.MaximumAppointment4), Convert.ToInt32(objSchDate.MaximumAppointment5)))
                        {
                            result = false;
                        }
                    }
                    else if (obj != null && objSchDate == null)
                    {
                        lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND Session = 1", healthcareServiceUnitID, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                        if (!onCheckMaximumAppointment(lstAppointment, 1, dayNumber, ref errMessage, true, Convert.ToInt32(obj.MaximumAppointment1), Convert.ToInt32(obj.MaximumAppointment2), Convert.ToInt32(obj.MaximumAppointment3), Convert.ToInt32(obj.MaximumAppointment4), Convert.ToInt32(obj.MaximumAppointment5)))
                        {
                            result = false;
                        }
                    }
                    else if (obj == null && objSchDate == null)
                    {
                        entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID)).FirstOrDefault();
                        entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
                        errMessage += string.Format("Tidak ada jadwal untuk dokter <b>{0}</b> di klinik <b>{1}</b>", entityPM.FullName, entityHSU.ServiceUnitName);
                        result = false;
                    }
                }
                else
                {
                    entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID)).FirstOrDefault();
                    errMessage += string.Format("<b>{0}</b> sedang cuti dari <b>{1}</b> sampai dengan <b>{2}</b>", entityPM.FullName, objSchDateLeave.StartDate.ToString(Constant.FormatString.DATE_FORMAT), objSchDateLeave.EndDate.ToString(Constant.FormatString.DATE_FORMAT));
                    result = false;
                }

            }
            #endregion

            return result;
        }

        public static bool onCheckMaximumAppointment(List<vAppointment> lstAppointment, int session, int dayNumber, ref string errMessage, bool isHasScheduleDate, int maxApm1, int maxApm2, int maxApm3, int maxApm4, int maxApm5)
        {
            bool result = true;

            int[] lstMaxApm = new int[6];
            lstMaxApm[0] = 0;
            lstMaxApm[1] = maxApm1;
            lstMaxApm[2] = maxApm2;
            lstMaxApm[3] = maxApm3;
            lstMaxApm[4] = maxApm4;
            lstMaxApm[5] = maxApm5;

            string[] lstDay = new string[8];
            lstDay[0] = "";
            lstDay[1] = "Senin";
            lstDay[2] = "Selasa";
            lstDay[3] = "Rabu";
            lstDay[4] = "Kamis";
            lstDay[5] = "Jumat";
            lstDay[6] = "Sabtu";
            lstDay[7] = "Minggu";

            List<vAppointment> filterApm = null;
            if (session > 0)
            {
                if (lstAppointment.Count != 0)
                {
                    for (int i = 1; i <= session; i++)
                    {
                        filterApm = new List<vAppointment>();
                        if (!isHasScheduleDate)
                        {
                            filterApm = lstAppointment.Where(w => w.Session == i && w.StartDate.ToString(Constant.FormatString.DATE_FORMAT_112) == DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112)).ToList();
                            if (lstMaxApm[i] > 0)
                            {
                                if (lstMaxApm[i] <= filterApm.Count)
                                {
                                    result = false;
                                    errMessage += string.Format("Kuota di sesi <b>{0}</b> tidak bisa kurang dari jumlah perjanjian pasien (Sudah ada <b>{1}</b> perjanjian yang valid)", i, filterApm.Count);
                                }
                            }
                        }
                        else
                        {
                            filterApm = lstAppointment.Where(w => w.Session == i && w.StartDate.Date >= DateTime.Now.Date && w.DayNumber == dayNumber).ToList();
                            if (lstMaxApm[i] > 0)
                            {
                                if (lstMaxApm[i] <= filterApm.Count)
                                {
                                    result = false;
                                    errMessage += string.Format("Kuota di sesi <b>{0}</b> tidak bisa kurang dari jumlah perjanjian pasien (Sudah ada <b>{1}</b> perjanjian yang valid di hari <b>{2}</b>)", i, filterApm.Count, lstDay[dayNumber]);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}
