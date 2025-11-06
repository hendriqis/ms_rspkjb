using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for MethodService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MethodService : System.Web.Services.WebService
    {

        private class Peserta
        {
            public string NoPeserta { get; set; }
            public string NoRM { get; set; }
            public string NamaPeserta { get; set; }
            public string NamaJalan { get; set; }
            public DateTime TglLahir { get; set; }
            public string JenisKelamin { get; set; }
            public string JenisPelayanan { get; set; }
            public string Kelas { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public object GetSessionValue(string sessionName)
        {
            return Session[sessionName];
        }

        #region GetSuggestedPrice
        [WebMethod(EnableSession = true)]
        public object GetSuggestedPrice(int bookID, int itemID)
        {
            string GCItemType = BusinessLayer.GetItemMaster(itemID).GCItemType;
            decimal SuggestedTariff = 0;
            decimal BaseTariff = 0;
            if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC || GCItemType == Constant.ItemGroupMaster.MEDICAL_CHECKUP)
            {
                // Get from Item Cost
                List<GetCurrentItemCost> list = BusinessLayer.GetCurrentItemCost(AppSession.UserLogin.HealthcareID, itemID);
                if (list.Count > 0)
                {
                    decimal total = list[0].Total;
                    if (total == 0)
                    {
                        // If Item Cost = 0 Get From Last Item Base Price
                        List<GetItemLastBaseTariff> lst = BusinessLayer.GetItemLastBaseTariff(AppSession.UserLogin.HealthcareID, bookID, itemID);
                        if (lst.Count > 0)
                        {
                            total = lst[0].BaseTariff;
                            SuggestedTariff = total;
                            BaseTariff = total;
                        }
                    }
                    SuggestedTariff = total;
                    BaseTariff = total;
                }
                else
                {
                    decimal total = list[0].Total;
                    if (total == 0)
                    {
                        // If Item Cost = 0 Get From Last Item Base Price
                        List<GetItemLastBaseTariff> lst = BusinessLayer.GetItemLastBaseTariff(AppSession.UserLogin.HealthcareID, bookID, itemID);
                        if (lst.Count > 0)
                        {
                            total = lst[0].BaseTariff;
                            SuggestedTariff = total;
                            BaseTariff = total;
                        }
                    }
                    SuggestedTariff = total;
                    BaseTariff = total;
                }
            }
            else
            {
                ItemProduct itemProduct = BusinessLayer.GetItemProduct(itemID);
                decimal suggestedTariff = itemProduct.HETAmount;
                decimal baseTariff = 0;

                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, itemID));
                if (lstItemPlanning.Count() > 0)
                    baseTariff = lstItemPlanning.FirstOrDefault().UnitPrice;

                SuggestedTariff = suggestedTariff;
                BaseTariff = baseTariff;
            }
            List<ItemCost> lstItemCost = BusinessLayer.GetItemCostList(string.Format("ItemID = {0} AND HealthcareID = '{1}'", itemID, AppSession.UserLogin.HealthcareID));
            if (lstItemCost.Count > 0)
            {
                ItemCost itemCost = lstItemCost.FirstOrDefault();
                return new
                {
                    SuggestedTariff = SuggestedTariff,
                    BaseTariff = BaseTariff,
                    ItemCost = itemCost
                };
            }
            return new
            {
                SuggestedTariff = SuggestedTariff,
                BaseTariff = BaseTariff,
                ItemCost = new ItemCost()
            };
        }
        #endregion

        #region GetDraftItemTariff
        [WebMethod(EnableSession = true)]
        public object GetDraftItemTariff(int appointmentID, int classID, int itemID, DateTime transactionDate, int testPartnerID = 0)
        {
            string GCItemType = BusinessLayer.GetItemMaster(itemID).GCItemType;
            if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
            {
                List<GetCurrentDraftItemTariff> list = BusinessLayer.GetCurrentDraftItemTariff(appointmentID, classID, itemID, 1, transactionDate, null, testPartnerID);
                decimal discountAmount = 0;
                decimal coverageAmount = 0;
                decimal price = 0;
                decimal basePrice = 0;
                decimal basePriceComp1 = 0;
                decimal basePriceComp2 = 0;
                decimal basePriceComp3 = 0;
                decimal priceComp1 = 0;
                decimal priceComp2 = 0;
                decimal priceComp3 = 0;
                decimal costAmount = 0;
                bool isCoverageInPercentage = false;
                bool isDiscountInPercentage = false;
                if (list.Count > 0)
                {
                    GetCurrentDraftItemTariff obj = list[0];
                    discountAmount = obj.DiscountAmount;
                    coverageAmount = obj.CoverageAmount;
                    price = obj.Price;
                    basePrice = obj.BasePrice;
                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                    isDiscountInPercentage = obj.IsDiscountInPercentage;
                    priceComp1 = obj.PriceComp1;
                    priceComp2 = obj.PriceComp2;
                    priceComp3 = obj.PriceComp3;
                    basePriceComp1 = obj.BasePriceComp1;
                    basePriceComp2 = obj.BasePriceComp2;
                    basePriceComp3 = obj.BasePriceComp3;
                    costAmount = obj.CostAmount;
                }
                vItemService entity = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", itemID)).FirstOrDefault();

                return new
                {
                    DiscountAmount = discountAmount,
                    CoverageAmount = coverageAmount,
                    Price = price,
                    BasePrice = basePrice,
                    BasePriceComp1 = basePriceComp1,
                    BasePriceComp2 = basePriceComp2,
                    BasePriceComp3 = basePriceComp3,
                    PriceComp1 = priceComp1,
                    PriceComp2 = priceComp2,
                    PriceComp3 = priceComp3,
                    IsCoverageInPercentage = isCoverageInPercentage,
                    IsDiscountInPercentage = isDiscountInPercentage,
                    IsAllowCito = entity.IsAllowCito,
                    IsAllowComplication = entity.IsAllowComplication,
                    IsAllowDiscount = entity.IsAllowDiscount,
                    IsAllowVariable = entity.IsAllowVariable,
                    IsUnbilledItem = entity.IsUnbilledItem,
                    GCItemUnit = entity.GCItemUnit,
                    ItemUnit = entity.ItemUnit,
                    IsCITOInPercentage = entity.IsCITOInPercentage,
                    CITOAmount = entity.CITOAmount,
                    IsComplicationInPercentage = entity.IsComplicationInPercentage,
                    ComplicationAmount = entity.ComplicationAmount,
                    DefaultTariffComp = entity.DefaultTariffComp,
                    CostAmount = costAmount
                };
            }
            else
            {
                vItemMaster entity = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", itemID)).FirstOrDefault();
                int type = 2;
                if (entity.GCItemType == Constant.ItemGroupMaster.LOGISTIC)
                    type = 3;
                List<GetCurrentDraftItemTariff> list = BusinessLayer.GetCurrentDraftItemTariff(appointmentID, classID, itemID, type, transactionDate);
                decimal discountAmount = 0;
                decimal coverageAmount = 0;
                decimal price = 0;
                decimal basePrice = 0;
                decimal costAmount = 0;
                bool isCoverageInPercentage = false;
                bool isDiscountInPercentage = false;
                if (list.Count > 0)
                {
                    GetCurrentDraftItemTariff obj = list[0];
                    discountAmount = obj.DiscountAmount;
                    coverageAmount = obj.CoverageAmount;
                    price = obj.Price;
                    basePrice = obj.BasePrice;
                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                    isDiscountInPercentage = obj.IsDiscountInPercentage;
                    costAmount = obj.CostAmount;
                }

                return new
                {
                    DiscountAmount = discountAmount,
                    CoverageAmount = coverageAmount,
                    Price = price,
                    BasePrice = basePrice,
                    IsCoverageInPercentage = isCoverageInPercentage,
                    IsDiscountInPercentage = isDiscountInPercentage,
                    GCItemUnit = entity.GCItemUnit,
                    ItemUnit = entity.ItemUnit,
                    CostAmount = costAmount
                };
            }
        }
        #endregion

        #region GetItemTariff
        [WebMethod(EnableSession = true)]
        public object GetItemTariff(int registrationID, int visitID, int classID, int itemID, DateTime transactionDate, int testPartnerID = 0)
        {
            vItemMaster oItemMaster = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", itemID)).FirstOrDefault();
            string oGCItemType = oItemMaster.GCItemType;
            string oGCItemUnit = oItemMaster.GCItemUnit;
            string oItemUnit = oItemMaster.ItemUnit;

            if (oGCItemType == Constant.ItemGroupMaster.SERVICE || oGCItemType == Constant.ItemGroupMaster.RADIOLOGY || oGCItemType == Constant.ItemGroupMaster.LABORATORY || oGCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
            {
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, classID, itemID, 1, transactionDate, null, testPartnerID);

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

                return new
                {
                    BasePrice = basePrice,
                    BasePriceComp1 = basePriceComp1,
                    BasePriceComp2 = basePriceComp2,
                    BasePriceComp3 = basePriceComp3,
                    Price = price,
                    PriceComp1 = priceComp1,
                    PriceComp2 = priceComp2,
                    PriceComp3 = priceComp3,
                    CoverageAmount = coverageAmount,
                    IsCoverageInPercentage = isCoverageInPercentage,
                    IsAllowDiscount = entity.IsAllowDiscount,
                    IsDiscountUsedComp = isDiscountUsedComp,
                    DiscountAmount = discountAmount,
                    DiscountAmountComp1 = discountAmountComp1,
                    DiscountAmountComp2 = discountAmountComp2,
                    DiscountAmountComp3 = discountAmountComp3,
                    IsDiscountInPercentage = isDiscountInPercentage,
                    IsDiscountInPercentageComp1 = isDiscountInPercentageComp1,
                    IsDiscountInPercentageComp2 = isDiscountInPercentageComp2,
                    IsDiscountInPercentageComp3 = isDiscountInPercentageComp3,
                    IsAllowCito = entity.IsAllowCito,
                    IsAllowComplication = entity.IsAllowComplication,
                    IsAllowVariable = entity.IsAllowVariable,
                    IsUnbilledItem = entity.IsUnbilledItem,
                    GCItemUnit = oGCItemUnit,
                    ItemUnit = oItemUnit,
                    IsCITOInPercentage = entity.IsCITOInPercentage,
                    CITOAmount = entity.CITOAmount,
                    IsComplicationInPercentage = entity.IsComplicationInPercentage,
                    ComplicationAmount = entity.ComplicationAmount,
                    DefaultTariffComp = entity.DefaultTariffComp,
                    CostAmount = costAmount
                };
            }
            else
            {
                int type = 2;
                if (oGCItemType == Constant.ItemGroupMaster.LOGISTIC)
                {
                    type = 3;
                }
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, classID, itemID, type, transactionDate);

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

                return new
                {
                    BasePrice = basePrice,
                    BasePriceComp1 = basePriceComp1,
                    BasePriceComp2 = basePriceComp2,
                    BasePriceComp3 = basePriceComp3,
                    Price = price,
                    PriceComp1 = priceComp1,
                    PriceComp2 = priceComp2,
                    PriceComp3 = priceComp3,
                    CoverageAmount = coverageAmount,
                    IsCoverageInPercentage = isCoverageInPercentage,
                    IsDiscountUsedComp = isDiscountUsedComp,
                    DiscountAmount = discountAmount,
                    DiscountAmountComp1 = discountAmountComp1,
                    DiscountAmountComp2 = discountAmountComp2,
                    DiscountAmountComp3 = discountAmountComp3,
                    IsDiscountInPercentage = isDiscountInPercentage,
                    IsDiscountInPercentageComp1 = isDiscountInPercentageComp1,
                    IsDiscountInPercentageComp2 = isDiscountInPercentageComp2,
                    IsDiscountInPercentageComp3 = isDiscountInPercentageComp3,
                    CostAmount = costAmount,
                    GCItemUnit = oGCItemUnit,
                    ItemUnit = oItemUnit
                };
            }
        }
        #endregion

        #region GetItemRevenueSharing
        [WebMethod(EnableSession = true)]
        public object GetItemRevenueSharing(string ItemCode, int ParamedicID, int ClassID, string GCParamedicRole, int VisitID, int ChargesHealthcareServiceUnitID, string TransactionDate, string TransactionTime)
        {
            return BusinessLayer.GetItemRevenueSharing(ItemCode, ParamedicID, ClassID, GCParamedicRole, VisitID, ChargesHealthcareServiceUnitID, Helper.GetDatePickerValue(TransactionDate), TransactionTime).FirstOrDefault();
        }
        #endregion

        #region GetTariffEstimation
        [WebMethod(EnableSession = true)]
        public object GetTariffEstimation(int classID, int businessPartnerID, int coverageTypeID, String lstItemID, String transactionDate, String departmentID, int itemType)
        {
            List<TariffEstimation> lst = new List<TariffEstimation>();

            string[] lsItemID = lstItemID.Split(',');
            for (int key = 0; key < lsItemID.Length; key++)
            {
                int itemID = Convert.ToInt32(lsItemID[key]);
                string GCItemType = BusinessLayer.GetItemMaster(itemID).GCItemType;
                int type = 1;
                if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY
                    || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC || GCItemType == Constant.ItemGroupMaster.MEDICAL_CHECKUP)
                    type = 1;
                else if (GCItemType == Constant.ItemGroupMaster.LOGISTIC)
                    type = 3;
                else
                    type = 2;

                List<GetTariffEstimation> list = BusinessLayer.GetTariffEstimation(AppSession.UserLogin.HealthcareID, businessPartnerID, classID, coverageTypeID, itemID, type, transactionDate, departmentID);
                if (list.Count > 0)
                {
                    Boolean IsPackageItem = false;
                    Decimal PackagePrice = 0;
                    Boolean IsUsingAccumulatedPrice = false;

                    GetTariffEstimation obj = list[0];
                    TariffEstimation tariffEst = new TariffEstimation();
                    tariffEst.ItemID = itemID;
                    tariffEst.DiscountAmount = obj.DiscountAmount;
                    tariffEst.CoverageAmount = obj.CoverageAmount;
                    tariffEst.Price = obj.Price;
                    tariffEst.PriceComp1 = obj.PriceComp1;
                    tariffEst.PriceComp2 = obj.PriceComp2;
                    tariffEst.PriceComp3 = obj.PriceComp3;
                    tariffEst.BasePrice = obj.BasePrice;
                    tariffEst.IsCoverageInPercentage = obj.IsCoverageInPercentage;
                    tariffEst.IsDiscountInPercentage = obj.IsDiscountInPercentage;
                    tariffEst.IsDiscountUsedComp = obj.IsDiscountUsedComp;

                    decimal discountComp1 = 0;
                    decimal discountComp2 = 0;
                    decimal discountComp3 = 0;

                    if (obj.IsDiscountUsedComp)
                    {
                        #region discComp1
                        if (obj.IsDiscountInPercentageComp1)
                        {
                            if (obj.IsDiscountUsedComp)
                            {
                                if (obj.PriceComp1 > 0)
                                {
                                    if (obj.IsDiscountInPercentageComp1)
                                    {
                                        discountComp1 = (obj.PriceComp1 * (obj.DiscountAmountComp1 / 100));
                                    }
                                    else
                                    {
                                        discountComp1 = obj.DiscountAmountComp1;
                                    }
                                }
                            }
                            else
                            {
                                if (obj.PriceComp1 > 0)
                                {
                                    if (obj.IsDiscountInPercentageComp1)
                                    {
                                        discountComp1 = (obj.PriceComp1 * (obj.DiscountAmountComp1 / 100));
                                    }
                                    else
                                    {
                                        discountComp1 = (obj.PriceComp1 * (obj.DiscountAmount / 100));
                                    }
                                }
                            }
                        }
                        else
                        {
                            discountComp1 = obj.DiscountAmountComp1;
                        }
                        #endregion

                        #region discComp2
                        if (obj.IsDiscountInPercentageComp2)
                        {
                            if (obj.IsDiscountUsedComp)
                            {
                                if (obj.PriceComp2 > 0)
                                {
                                    if (obj.IsDiscountInPercentageComp2)
                                    {
                                        discountComp2 = (obj.PriceComp2 * (obj.DiscountAmountComp2 / 100));
                                    }
                                    else
                                    {
                                        discountComp2 = obj.DiscountAmountComp2;
                                    }
                                }
                            }
                            else
                            {
                                if (obj.PriceComp2 > 0)
                                {
                                    if (obj.IsDiscountInPercentageComp2)
                                    {
                                        discountComp2 = (obj.PriceComp2 * (obj.DiscountAmountComp2 / 100));
                                    }
                                    else
                                    {
                                        discountComp2 = (obj.PriceComp2 * (obj.DiscountAmount / 100));
                                    }
                                }
                            }
                        }
                        else
                        {
                            discountComp2 = obj.DiscountAmountComp2;
                        }
                        #endregion

                        #region discComp3
                        if (obj.IsDiscountInPercentageComp3)
                        {
                            if (obj.IsDiscountUsedComp)
                            {
                                if (obj.PriceComp3 > 0)
                                {
                                    if (obj.IsDiscountInPercentageComp3)
                                    {
                                        discountComp3 = (obj.PriceComp3 * (obj.DiscountAmountComp3 / 100));
                                    }
                                    else
                                    {
                                        discountComp3 = obj.DiscountAmountComp3;
                                    }
                                }
                            }
                            else
                            {
                                if (obj.PriceComp3 > 0)
                                {
                                    if (obj.IsDiscountInPercentageComp3)
                                    {
                                        discountComp3 = (obj.PriceComp3 * (obj.DiscountAmountComp3 / 100));
                                    }
                                    else
                                    {
                                        discountComp3 = (obj.PriceComp3 * (obj.DiscountAmount / 100));
                                    }
                                }
                            }
                        }
                        else
                        {
                            discountComp3 = obj.DiscountAmountComp3;
                        }
                        #endregion

                        tariffEst.DiscountAmount = discountComp1 + discountComp2 + discountComp3;
                    }


                    SetPackageItem(ref IsPackageItem, tariffEst.ItemID, classID, transactionDate, departmentID, ref PackagePrice, ref IsUsingAccumulatedPrice);

                    tariffEst.IsPackageItem = IsPackageItem;
                    if (IsPackageItem == true)
                    {
                        if (IsUsingAccumulatedPrice == true)
                        {
                            if (GCItemType == Constant.ItemType.MEDICAL_CHECKUP)
                            {
                                tariffEst.ItemPackagePrice = PackagePrice;
                            }
                            else
                            {
                                if (businessPartnerID == 1)
                                {
                                    tariffEst.ItemPackagePrice = PackagePrice;
                                }
                                else
                                {
                                    tariffEst.ItemPackagePrice = tariffEst.Price;
                                }
                            }
                            //if (businessPartnerID == 1)
                            //{
                            //    tariffEst.ItemPackagePrice = PackagePrice;
                            //}
                            //else
                            //{
                            //    tariffEst.ItemPackagePrice = tariffEst.Price;
                            //}
                        }
                        else
                        {
                            tariffEst.ItemPackagePrice = tariffEst.Price;
                        }
                    }
                    else
                    {
                        tariffEst.ItemPackagePrice = 0;
                    }



                    lst.Add(tariffEst);
                }
            }
            return lst;
        }
        private void SetPackageItem(ref Boolean IsPackageItem, int ItemID, int ClassID, String TransactionDate, string DepartmentID, ref decimal PackagePrice, ref Boolean IsUsingAccumulatedPrice)
        {
            ItemService oItemService = BusinessLayer.GetItemService(ItemID);
            if (oItemService != null)
            {
                if (oItemService.IsPackageItem)
                {

                    IsPackageItem = oItemService.IsPackageItem;
                    IsUsingAccumulatedPrice = oItemService.IsUsingAccumulatedPrice;

                    SettingParameter setPar = BusinessLayer.GetSettingParameter(Constant.SettingParameter.MEDICAL_CHECKUP_CLASS);
                    string SetParMedicalCheckupClass = string.Empty;
                    if (setPar != null)
                    {
                        SetParMedicalCheckupClass = setPar.ParameterValue;
                    }
                    string filter = string.Format("");
                    decimal totalDetail = 0;

                    string filter2 = string.Format("IsDeleted = 0 AND ItemID = {0}", ItemID);
                    List<vItemServiceDt> lstItemServiceDtAll = BusinessLayer.GetvItemServiceDtList(filter2);
                    List<vItemServiceDtPrice> lstItemServiceDtWithPriceAll = new List<vItemServiceDtPrice>();
                    //SetPriceServiceDt(lstItemServiceDtAll, lstItemServiceDtWithPriceAll, SetParMedicalCheckupClass);
                    SetPriceServiceDt(lstItemServiceDtAll, lstItemServiceDtWithPriceAll, ClassID, TransactionDate, DepartmentID);
                    totalDetail = lstItemServiceDtWithPriceAll.Sum(t => t.Quantity * (t.Price - t.DiscountAmount));
                    PackagePrice = totalDetail;
                }

            }
        }
        private void SetPriceServiceDt(List<vItemServiceDt> lstItemServiceDt, List<vItemServiceDtPrice> lstItemServiceDtWithPrice, int ClassID, String transactionDate, string DepartmentID)
        {
            foreach (vItemServiceDt entity in lstItemServiceDt)
            {
                vItemServiceDtPrice entityvItemServiceDtWithPrice = new vItemServiceDtPrice();
                entityvItemServiceDtWithPrice.ID = entity.ID;
                entityvItemServiceDtWithPrice.ItemID = entity.ItemID;
                entityvItemServiceDtWithPrice.ItemCode = entity.ItemCode;
                entityvItemServiceDtWithPrice.ItemName1 = entity.ItemName1;
                entityvItemServiceDtWithPrice.DepartmentID = entity.DepartmentID;
                entityvItemServiceDtWithPrice.ParamedicID = entity.ParamedicID;
                entityvItemServiceDtWithPrice.ParamedicCode = entity.ParamedicCode;
                entityvItemServiceDtWithPrice.ParamedicName = entity.ParamedicName;
                entityvItemServiceDtWithPrice.DepartmentName = entity.DepartmentName;
                entityvItemServiceDtWithPrice.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                entityvItemServiceDtWithPrice.ServiceUnitID = entity.ServiceUnitID;
                entityvItemServiceDtWithPrice.ServiceUnitCode = entity.ServiceUnitCode;
                entityvItemServiceDtWithPrice.ServiceUnitName = entity.ServiceUnitName;
                entityvItemServiceDtWithPrice.GCItemType = entity.GCItemType;
                entityvItemServiceDtWithPrice.ItemType = entity.ItemType;
                entityvItemServiceDtWithPrice.DetailItemID = entity.DetailItemID;
                entityvItemServiceDtWithPrice.DetailItemCode = entity.DetailItemCode;
                entityvItemServiceDtWithPrice.DetailItemName1 = entity.DetailItemName1;
                entityvItemServiceDtWithPrice.Quantity = entity.Quantity;
                entityvItemServiceDtWithPrice.GCItemUnit = entity.GCItemUnit;
                entityvItemServiceDtWithPrice.ItemUnit = entity.ItemUnit;
                entityvItemServiceDtWithPrice.DiscountAmount = entity.DiscountAmount;
                entityvItemServiceDtWithPrice.DiscountComp1 = entity.DiscountComp1;
                entityvItemServiceDtWithPrice.DiscountComp2 = entity.DiscountComp2;
                entityvItemServiceDtWithPrice.DiscountComp3 = entity.DiscountComp3;
                entityvItemServiceDtWithPrice.IsAutoPosted = entity.IsAutoPosted;
                entityvItemServiceDtWithPrice.IsAllowChanged = entity.IsAllowChanged;
                entityvItemServiceDtWithPrice.IsDeleted = entity.IsDeleted;

                decimal tariff = 0, tariffComp1 = 0, tariffComp2 = 0, tariffComp3 = 0;

                bool isHasPackageAccumulated = false;

                ItemService oItemServiceDt = BusinessLayer.GetItemService(entity.DetailItemID);
                if (oItemServiceDt != null)
                {
                    if (oItemServiceDt.IsPackageItem && oItemServiceDt.IsUsingAccumulatedPrice)
                    {
                        isHasPackageAccumulated = true;

                        string filterDtDt = string.Format("IsDeleted = 0 AND ItemID = {0}", entity.DetailItemID);
                        List<ItemServiceDt> lstItemServiceDtDt = BusinessLayer.GetItemServiceDtList(filterDtDt);
                        foreach (ItemServiceDt entityDtDt in lstItemServiceDtDt)
                        {
                            int itemTypeDt = entityDtDt.GCItemType == Constant.ItemType.OBAT_OBATAN || entityDtDt.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : entityDtDt.GCItemType == Constant.ItemType.BARANG_UMUM || entity.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                            GetTariffEstimationCustom tariffEstDt = BusinessLayer.GetTariffEstimationCustom(AppSession.UserLogin.HealthcareID, 1, ClassID, 0, entityDtDt.DetailItemID, itemTypeDt, transactionDate, DepartmentID).FirstOrDefault();
                            if (tariffEstDt != null)
                            {
                                tariffComp1 += tariffEstDt.PriceComp1 - tariffEstDt.FinalDiscountComp1;
                                tariffComp2 += tariffEstDt.PriceComp2 - tariffEstDt.FinalDiscountComp2;
                                tariffComp3 += tariffEstDt.PriceComp3 - tariffEstDt.FinalDiscountComp3;
                            }

                        }
                    }
                }

                if (!isHasPackageAccumulated)
                {
                    int itemType = entity.GCItemType == Constant.ItemType.OBAT_OBATAN || entity.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : entity.GCItemType == Constant.ItemType.BARANG_UMUM || entity.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;

                    GetTariffEstimationCustom tariffEst = BusinessLayer.GetTariffEstimationCustom(AppSession.UserLogin.HealthcareID, 1, ClassID, 0, entity.DetailItemID, itemType, transactionDate, DepartmentID).FirstOrDefault();
                    if (tariffEst != null)
                    {
                        tariffComp1 = tariffEst.PriceComp1 - tariffEst.FinalDiscountComp1;
                        tariffComp2 = tariffEst.PriceComp2 - tariffEst.FinalDiscountComp2;
                        tariffComp3 = tariffEst.PriceComp3 - tariffEst.FinalDiscountComp3;
                    }
                }

                tariff = tariffComp1 + tariffComp2 + tariffComp3;

                entityvItemServiceDtWithPrice.Price = tariff;
                entityvItemServiceDtWithPrice.PriceComp1 = tariffComp1;
                entityvItemServiceDtWithPrice.PriceComp2 = tariffComp2;
                entityvItemServiceDtWithPrice.PriceComp3 = tariffComp3;

                lstItemServiceDtWithPrice.Add(entityvItemServiceDtWithPrice);
            }
        }
        public class vItemServiceDtPrice
        {
            private Int32 _ID;
            private Int32 _ItemID;
            private String _ItemCode;
            private String _ItemName1;
            private String _DepartmentID;
            private Int32 _ParamedicID;
            private String _ParamedicCode;
            private String _ParamedicName;
            private String _DepartmentName;
            private Int32 _HealthcareServiceUnitID;
            private Int32 _ServiceUnitID;
            private String _ServiceUnitCode;
            private String _ServiceUnitName;
            private String _GCItemType;
            private String _ItemType;
            private Int32 _DetailItemID;
            private String _DetailItemCode;
            private String _DetailItemName1;
            private Decimal _Quantity;
            private String _GCItemUnit;
            private String _ItemUnit;
            private Decimal _DiscountAmount;
            private Decimal _DiscountComp1;
            private Decimal _DiscountComp2;
            private Decimal _DiscountComp3;
            private Boolean _IsAutoPosted;
            private Boolean _IsAllowChanged;
            private Boolean _IsDeleted;
            private Decimal _Price;
            private Decimal _PriceComp1;
            private Decimal _PriceComp2;
            private Decimal _PriceComp3;

            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            public String ItemCode
            {
                get { return _ItemCode; }
                set { _ItemCode = value; }
            }
            public String ItemName1
            {
                get { return _ItemName1; }
                set { _ItemName1 = value; }
            }
            public String DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }
            public Int32 ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }
            public String ParamedicCode
            {
                get { return _ParamedicCode; }
                set { _ParamedicCode = value; }
            }
            public String ParamedicName
            {
                get { return _ParamedicName; }
                set { _ParamedicName = value; }
            }
            public String DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }
            public Int32 HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
            }
            public Int32 ServiceUnitID
            {
                get { return _ServiceUnitID; }
                set { _ServiceUnitID = value; }
            }
            public String ServiceUnitCode
            {
                get { return _ServiceUnitCode; }
                set { _ServiceUnitCode = value; }
            }
            public String ServiceUnitName
            {
                get { return _ServiceUnitName; }
                set { _ServiceUnitName = value; }
            }
            public String GCItemType
            {
                get { return _GCItemType; }
                set { _GCItemType = value; }
            }
            public String ItemType
            {
                get { return _ItemType; }
                set { _ItemType = value; }
            }
            public Int32 DetailItemID
            {
                get { return _DetailItemID; }
                set { _DetailItemID = value; }
            }
            public String DetailItemCode
            {
                get { return _DetailItemCode; }
                set { _DetailItemCode = value; }
            }
            public String DetailItemName1
            {
                get { return _DetailItemName1; }
                set { _DetailItemName1 = value; }
            }
            public Decimal Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }
            public String GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
            public String ItemUnit
            {
                get { return _ItemUnit; }
                set { _ItemUnit = value; }
            }
            public Decimal DiscountAmount
            {
                get { return _DiscountAmount; }
                set { _DiscountAmount = value; }
            }
            public Decimal DiscountComp1
            {
                get { return _DiscountComp1; }
                set { _DiscountComp1 = value; }
            }
            public Decimal DiscountComp2
            {
                get { return _DiscountComp2; }
                set { _DiscountComp2 = value; }
            }
            public Decimal DiscountComp3
            {
                get { return _DiscountComp3; }
                set { _DiscountComp3 = value; }
            }
            public Boolean IsAutoPosted
            {
                get { return _IsAutoPosted; }
                set { _IsAutoPosted = value; }
            }
            public Boolean IsAllowChanged
            {
                get { return _IsAllowChanged; }
                set { _IsAllowChanged = value; }
            }
            public Boolean IsDeleted
            {
                get { return _IsDeleted; }
                set { _IsDeleted = value; }
            }
            public Decimal Price
            {
                get { return _Price; }
                set { _Price = value; }
            }
            public Decimal PriceComp1
            {
                get { return _PriceComp1; }
                set { _PriceComp1 = value; }
            }
            public Decimal PriceComp2
            {
                get { return _PriceComp2; }
                set { _PriceComp2 = value; }
            }
            public Decimal PriceComp3
            {
                get { return _PriceComp3; }
                set { _PriceComp3 = value; }
            }

            public String MCUPrice
            {
                get
                {
                    return (Price - DiscountAmount).ToString("N2");
                }
            }
        }
        class TariffEstimation
        {
            public Int32 ItemID { get; set; }
            public Decimal DiscountAmount { get; set; }
            public Decimal CoverageAmount { get; set; }
            public Decimal Price { get; set; }
            public Decimal PriceComp1 { get; set; }
            public Decimal PriceComp2 { get; set; }
            public Decimal PriceComp3 { get; set; }
            public Decimal BasePrice { get; set; }
            public Boolean IsCoverageInPercentage { get; set; }
            public Boolean IsDiscountInPercentage { get; set; }
            public Boolean IsDiscountUsedComp { get; set; }
            public Boolean IsPackageItem { get; set; }
            public Decimal ItemPackagePrice { get; set; }
        }

        #endregion

        #region GetItemMasterPurchase
        [WebMethod(EnableSession = true)]
        public object GetItemMasterPurchase(int itemID, int businessPartnerID)
        {
            return BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, itemID, businessPartnerID).FirstOrDefault();
        }
        #endregion

        #region GetItemMasterPurchaseWithDate
        [WebMethod(EnableSession = true)]
        public object GetItemMasterPurchaseWithDate(int itemID, int businessPartnerID, string effectiveDate)
        {
            return BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, itemID, businessPartnerID, effectiveDate).FirstOrDefault();
        }
        #endregion

        #region GetItemMasterPurchaseList
        [WebMethod(EnableSession = true)]
        public object GetItemMasterPurchaseList(int itemID, int businessPartnerID)
        {
            return BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, itemID, businessPartnerID);
        }
        #endregion

        #region GetItemMasterPurchaseWithDateList
        [WebMethod(EnableSession = true)]
        public object GetItemMasterPurchaseWithDateList(int itemID, int businessPartnerID, string effectiveDate)
        {
            return BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, itemID, businessPartnerID, effectiveDate);
        }
        #endregion

        #region GetItemQtyOnOrder
        [WebMethod(EnableSession = true)]
        public object GetItemQtyOnOrder(int itemID, int locationID, int type)
        {
            return BusinessLayer.GetItemQtyOnOrder(itemID, locationID, type).FirstOrDefault();
        }
        #endregion

        #region GetDateDiffPOPORPerSupplier
        [WebMethod(EnableSession = true)]
        public object GetDateDiffPOPORPerSupplier(int supplierID)
        {
            return BusinessLayer.GetDateDiffPOPORPerSupplier(supplierID).FirstOrDefault();
        }
        #endregion

        #region GetPatientChargesValidationDoubleInputList
        [WebMethod(EnableSession = true)]
        public object GetPatientChargesValidationDoubleInputList(int visitID, int transactionID, string transactionDate)
        {
            string result = "";
            List<GetPatientChargesValidationDoubleInput> lst = BusinessLayer.GetPatientChargesValidationDoubleInputList(visitID, transactionID, transactionDate);
            if (lst.Count > 0)
            {
                foreach (GetPatientChargesValidationDoubleInput e in lst)
                {
                    if (String.IsNullOrEmpty(result))
                    {
                        result = string.Format("<BR>{0}", e.ItemName1);
                    }
                    else
                    {
                        result += string.Format("<BR>{0}", e.ItemName1);
                    }
                }
            }

            return new
            {
                check = result
            };
        }
        #endregion

        #region Get Object
        [WebMethod()]
        public object GetLimitListObject(string methodName, string filterExpression, int pageCount)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string), typeof(int), typeof(int), typeof(string) });
                if (method != null)
                {
                    object obj = method.Invoke(null, new object[] { filterExpression, pageCount, 1, "" });
                    IList list = (IList)obj;
                    return list;
                }
                return GetListObject(methodName, filterExpression);
            }
            else
            {
                return null;
            }
        }
        [WebMethod()]
        public object GetLimitListObject2(string methodName, string filterExpression, int pageCount, string orderByExpression)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string), typeof(int), typeof(int), typeof(string) });
                if (method != null)
                {
                    object obj = method.Invoke(null, new object[] { filterExpression, pageCount, 1, orderByExpression });
                    IList list = (IList)obj;
                    return list;
                }
                return GetListObject(methodName, filterExpression);
            }
            else
            {
                return null;
            }
        }

        [WebMethod()]
        public object GetListObject(string methodName, string filterExpression)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                object obj = method.Invoke(null, new string[] { filterExpression });
                IList list = (IList)obj;
                return list;
            }
            else
            {
                return null;
            }
        }

        [WebMethod()]
        public object GetObject(string methodName, string filterExpression)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                object obj = method.Invoke(null, new string[] { Server.HtmlDecode(filterExpression) });
                IList list = (IList)obj;
                if (list.Count > 0)
                    return list[0];
                return null;
            }
            else
            {
                return null;
            }
        }

        [WebMethod()]
        public object GetValue(string methodName, string filterExpression)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                object obj = method.Invoke(null, new string[] { filterExpression });
                return obj;
            }
            else
            {
                return null;
            }
        }

        [WebMethod()]
        public object GetObjectValue(string methodName, string filterExpression, string returnField)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                object tempObj = method.Invoke(null, new string[] { filterExpression });
                IList list = (IList)tempObj;
                if (list.Count > 0)
                {
                    object obj = list[0];
                    return obj.GetType().GetProperty(returnField).GetValue(obj, null);
                }
                return "";
            }
            else
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public object GetObjectFromSession(string sessionName, string filterBy, object filterValue)
        {
            //Type t = Type.GetType("MyPersonClass");
            //Type listDataType = typeof(List<>).MakeGenericType(t);

            //list = Convert.ChangeType(HttpContext.Current.Session[sessionName], listDataType);



            IList list = (IList)HttpContext.Current.Session[sessionName];
            //Type type = list.GetType().GetGenericArguments()[0];

            //return list.FirstOrDefault(o => o.GetType().GetProperty(filterBy).GetValue(o, null).ToString() == filterValue.ToString());

            foreach (object obj in list)
            {
                object val = obj.GetType().GetProperty(filterBy).GetValue(obj, null);
                if (val.ToString() == filterValue.ToString())
                {
                    //return type.GetProperty(returnField).GetValue(obj, null);
                    return obj;
                }
            }
            //List<list.GetType()> lst = list;
            return null;
        }

        [WebMethod(EnableSession = true)]
        public object GetObjectValueFromSession(string sessionName, string filterBy, object filterValue, string returnField)
        {
            IList list = (IList)HttpContext.Current.Session[sessionName];
            //Type type = list.GetType().GetGenericArguments()[0];

            foreach (object obj in list)
            {
                object val = obj.GetType().GetProperty(filterBy).GetValue(obj, null);
                if (val.ToString() == filterValue.ToString())
                {
                    return obj.GetType().GetProperty(returnField).GetValue(obj, null);
                }
            }

            return null;
        }
        #endregion

        #region Get Custom Object
        [WebMethod()]
        public object GetCustomObject(string listParentValue, string selectedColumnID, string selectedColumnName, string valueColumn, string valueColumnType, string filterExpression, string orderByExpression, string objectTypeName)
        {
            if (Helper.IsValidCheckSqlInjectionFilterParameter(filterExpression))
            {
                List<Variable2> lstVariable = new List<Variable2>();
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    if (filterExpression != "")
                        filterExpression = string.Format(" WHERE {0}", filterExpression);
                    string groupBy = string.Format("{0}, {1}", selectedColumnID, selectedColumnName);
                    if (listParentValue != "")
                        groupBy += ", " + listParentValue;
                    if (selectedColumnID == selectedColumnName)
                        ctx.CommandText = string.Format("WITH cte AS (SELECT {0}, {2} = {3}({2}) FROM {7}{5} GROUP BY {4}) SELECT ID = {0}, Name = {1}, Value = {2} FROM cte ORDER BY {6}", selectedColumnID, selectedColumnName, valueColumn, valueColumnType, groupBy, filterExpression, orderByExpression, objectTypeName);
                    else
                        ctx.CommandText = string.Format("WITH cte AS (SELECT {0}, {1}, {2} = {3}({2}) FROM {7}{5} GROUP BY {4}) SELECT ID = {0}, Name = {1}, Value = {2} FROM cte ORDER BY {6}", selectedColumnID, selectedColumnName, valueColumn, valueColumnType, groupBy, filterExpression, orderByExpression, objectTypeName);
                    using (IDataReader reader = DaoBase.GetDataReader(ctx))
                        while (reader.Read())
                            lstVariable.Add(new Variable2 { ID = reader["ID"].ToString(), Name = reader["Name"].ToString(), Value = Convert.ToDecimal(reader["Value"]) });
                }
                finally
                {
                    ctx.Close();
                }
                return lstVariable;
            }
            else
            {
                return null;
            }
        }

        private class Variable2
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public Decimal Value { get; set; }
            public String cfValue { get { return Value.ToString("N2"); } }
        }
        #endregion

        #region Get Paramedic Work Times
        [WebMethod()]
        public object GetParamedicWorkTimes(int healthcareServiceUnitID, int paramedicID, int selectedDay, string date, int appointmentID)
        {
            List<String> result = new List<String>();
            int serviceInterval = 0;
            List<vParamedicSchedule> lstSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}", healthcareServiceUnitID, paramedicID, selectedDay));

            if (lstSchedule.Count > 0)
            {
                List<TimeOfDayInterval> list = GetWorkTimes(lstSchedule[0]);
                serviceInterval = BusinessLayer.GetHealthcareServiceUnit(healthcareServiceUnitID).ServiceInterval;
                string filterExpression = "";
                if (appointmentID > 0)
                    filterExpression = string.Format("AppointmentID != {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}'", appointmentID, paramedicID, date, Constant.AppointmentStatus.DELETED);
                else
                    filterExpression = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus != '{2}'", paramedicID, date, Constant.AppointmentStatus.DELETED);
                List<String> lstAppointmentParamedicSchedule = BusinessLayer.GetAppointmentStartTimeList(filterExpression);
                foreach (TimeOfDayInterval tod in list)
                {
                    result.Add(tod.Start.ToString(@"hh\:mm"));
                    TimeSpan ctrTime = tod.Start;
                    while (ctrTime < tod.End)
                    {
                        ctrTime = ctrTime.Add(new TimeSpan(0, serviceInterval, 0));
                        string temp = ctrTime.ToString(@"hh\:mm");
                        if (!lstAppointmentParamedicSchedule.Contains(temp))
                            result.Add(temp);
                    }
                }
            }
            return new { WorkTimes = result, ServiceInterval = serviceInterval };
        }
        private List<TimeOfDayInterval> GetWorkTimes(vParamedicSchedule schedule)
        {
            TimeOfDayInterval tm;
            List<TimeOfDayInterval> WorkTimes = new List<TimeOfDayInterval>();
            if (schedule.StartTime1 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime1), TimeSpan.Parse(schedule.EndTime1));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime2 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime2), TimeSpan.Parse(schedule.EndTime2));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime3 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime3), TimeSpan.Parse(schedule.EndTime3));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime4 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime4), TimeSpan.Parse(schedule.EndTime4));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime5 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime5), TimeSpan.Parse(schedule.EndTime5));
                WorkTimes.Add(tm);
            }
            return WorkTimes;
        }
        #endregion

        #region Get Control
        [WebMethod(EnableSession = true)]
        public string GetControlHtml(string controlLocation, string queryString)
        {
            try
            {
                Page page = new Page();
                BaseViewPopupCtl userControl = (BaseViewPopupCtl)page.LoadControl(controlLocation);
                userControl.InitializeControl(queryString);
                userControl.EnableViewState = false;
                HtmlForm form = new HtmlForm();
                form.Controls.Add(userControl);
                page.Controls.Add(form);

                StringWriter textWriter = new StringWriter();
                HttpContext.Current.Server.Execute(page, textWriter, false);
                return textWriter.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetUserControl(string controlLocation, string queryString)
        {
            try
            {
                Page page = new Page();
                BaseDataCtl userControl = (BaseDataCtl)page.LoadControl(controlLocation);
                userControl.InitializeDataControl(queryString);
                HtmlForm form = new HtmlForm();
                form.Controls.Add(userControl);
                page.Controls.Add(form);

                StringWriter textWriter = new StringWriter();
                HttpContext.Current.Server.Execute(page, textWriter, false);
                return textWriter.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string CleanHtml(string html)
        {
            return Regex.Replace(html, @"<[/]?(form)[^>]*?>", "", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Mobile
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMobileObject(string methodName, string filterExpression)
        {
            MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            object obj = method.Invoke(null, new string[] { filterExpression });
            IList list = (IList)obj;
            //return list[0];
            //return new JavaScriptSerializer().Serialize(list[0]);

            Object returnObj = new { ReturnObj = list[0], Timestamp = DateTime.Now };
            return new JavaScriptSerializer().Serialize(returnObj);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMobileListObject(string methodName, string filterExpression)
        {
            MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            object obj = method.Invoke(null, new string[] { filterExpression });
            IList list = (IList)obj;
            //return list;
            //return new JavaScriptSerializer().Serialize(list);

            Object returnObj = new { ReturnObj = list, Timestamp = DateTime.Now };
            return new JavaScriptSerializer().Serialize(returnObj);
        }
        #endregion

        [WebMethod()]
        public object GetPesertaLocalCache(string noPeserta)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");
            string fileName = string.Format(@"{0}\NoKartuBPJS.csv", filePath);
            IEnumerable<string> lstPeserta = File.ReadAllLines(fileName);
            List<Peserta> list = new List<Peserta>();
            foreach (string peserta in lstPeserta)
            {
                string[] info = peserta.Split(';');
                Peserta oPeserta = new Peserta();
                oPeserta.NoPeserta = info[0].TrimEnd();
                oPeserta.NoRM = info[1].TrimEnd();
                oPeserta.NamaPeserta = info[2].TrimEnd();
                oPeserta.NamaJalan = info[3].TrimEnd();
                //oPeserta.TglLahir = Convert.ToDateTime(info[4].TrimEnd(),"yyyymmdd");
                oPeserta.JenisKelamin = info[5].TrimEnd();
                oPeserta.JenisPelayanan = info[6].TrimEnd();
                oPeserta.Kelas = info[7].TrimEnd();
                list.Add(oPeserta);
            }

            IEnumerable<Peserta> lst = (IEnumerable<Peserta>)list;
            var dataPeserta = lst.Where(qry => qry.NoPeserta.Equals(noPeserta)).FirstOrDefault();
            if (dataPeserta != null)
                return dataPeserta;
            else
                return null;
        }

        #region GetINACBGGrouperTariff
        [WebMethod(EnableSession = true)]
        public object GetINACBGGrouperTariff(string jnsrawat, string klsrawat, string diagnosisCode, string procedureCode)
        {
            List<GetINACBGTariff> list = BusinessLayer.GetINACBGGrouperTariff(jnsrawat, klsrawat, diagnosisCode, procedureCode);
            string grouperCode = string.Empty;
            string grouperName = string.Empty;
            decimal tariff = 0;
            if (list.Count > 0)
            {
                GetINACBGTariff obj = list[0];
                grouperCode = obj.GrouperCode;
                grouperName = obj.GrouperName;
                tariff = obj.Tariff;
            }
            return new
            {
                GrouperCode = grouperCode,
                GrouperName = grouperName,
                Tariff = tariff
            };
        }
        #endregion

        [WebMethod()]
        public object GetMedicationTime(int frequency)
        {
            string result = "-|-|-|-|-|-";
            result = Methods.GetMedicationSequenceTime(frequency);
            return result;
        }

        #region GetStatusRegistrationOutstanding
        [WebMethod(EnableSession = true)]
        public bool GetStatusRegistrationOutstanding()
        {

            Boolean result = true;
            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression).FirstOrDefault();
            bool allowClose = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder + lstInfo.Charges + lstInfo.Billing == 0);
            if (allowClose == false)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region GetExpiredLicense
        [WebMethod(EnableSession = true)]
        public string GetExpiredLicense()
        {
            string result = string.Format("0|");
            if (AppSession.UserLogin != null)
            {
                if (AppSession.UserLogin.UserID > 1)
                { //kecuali sysadmin dan system
                    Healthcare oHealthcare = BusinessLayer.GetHealthcareList(string.Format("HealthcareID='{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    if (oHealthcare != null)
                    {

                        if (oHealthcare.ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                        {
                            DateTime dt1 = DateTime.Parse(oHealthcare.ExpiredDate.ToString());
                            DateTime dateNow = DateTime.Now;
                            if (dateNow.Date > dt1.Date)
                            {
                                result = string.Format("1|{0}", oHealthcare.ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                            }
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region MCU Pivot
        /// <summary>
        /// MCU Pivot Get Batch No
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMCUBatchNo(String businessPartnerName, String date)
        {
            string result = string.Empty;

            List<vAppointmentRequest> lstApmReq = BusinessLayer.GetvAppointmentRequestList(string.Format("RequestBatchNo IS NOT NULL AND IsDeleted = 0 AND RegistrationDate = '{0}' AND BusinessPartnerName = '{1}'", date, businessPartnerName)).GroupBy(g => g.RequestBatchNo).Select(s => s.FirstOrDefault()).OrderBy(o => o.RequestBatchNo).ToList();
            string lst = string.Empty;
            if (lstApmReq.Count > 0)
            {
                for (int i = 0; i < lstApmReq.Count; i++)
                {
                    lst += string.Format("{0},", lstApmReq[i].RequestBatchNo);
                }
                result = string.Format("{0}|{1}|{2}", "1", "null", lst.Remove(lst.Length - 1, 1));
            }
            else
            {
                result = string.Format("{0}|{1}|{2}", "0", "Data tidak ditemukan", "null");
            }

            return result;
        }

        /// <summary>
        /// MCU Pivot Get Result Type
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMCUResultTypeByRequestBatchNo(String requestBatchNo)
        {
            string result = string.Empty;

            List<vMCUResultForm1> lstApmReq = BusinessLayer.GetvMCUResultForm1List(string.Format("RequestBatchNo = '{0}'", requestBatchNo)).GroupBy(g => g.ResultType).Select(s => s.FirstOrDefault()).OrderBy(o => o.RequestBatchNo).ToList();
            string lst = string.Empty;
            if (lstApmReq.Count > 0)
            {
                for (int i = 0; i < lstApmReq.Count; i++)
                {
                    lst += string.Format("{0};", lstApmReq[i].ResultType);
                }
                result = string.Format("{0}|{1}|{2}", "1", "null", lst.Remove(lst.Length - 1, 1));
            }
            else
            {
                result = string.Format("{0}|{1}|{2}", "0", "Data tidak ditemukan", "null");
            }

            return result;
        }

        /// <summary>
        /// MCU Get Pivot Data
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPivotData(String requestBatchNo, String resultType, String date)
        {
            string result = string.Empty;
            string lstJson = string.Empty;
            string jsonFinal = string.Empty;

            List<vMCUResultForm1> lstEntityJson = BusinessLayer.GetvMCUResultForm1List(string.Format("RequestBatchNo = '{0}' AND ResultType = '{1}' AND RegistrationDate = '{2}'", requestBatchNo, resultType, date));
            string lst = string.Empty;
            if (lstEntityJson.Count > 0)
            {

                foreach (vMCUResultForm1 json in lstEntityJson)
                {
                    Dictionary<String, dynamic> obj = new Dictionary<string, dynamic>();
                    obj["RegistrationNo"] = json.RegistrationNo;
                    obj["PatientName"] = json.PatientName;
                    obj["BusinessPartnerName"] = json.BusinessPartnerName;
                    obj["MedicalNo"] = json.MedicalNo;
                    obj["CorporateAccountNo"] = json.CorporateAccountNo;
                    obj["CorporateAccountName"] = json.CorporateAccountName;
                    obj["CorporateAccountDepartment"] = json.CorporateAccountDepartment;
                    obj["ParamedicCode"] = json.ParamedicCode;
                    obj["ParamedicName"] = json.ParamedicName;
                    obj["ResultType"] = json.ResultType;
                    obj[json.TagField1QuestionName] = json.TagField1QuestionValue;
                    obj[json.TagField2QuestionName] = json.TagField2QuestionValue;
                    obj[json.TagField3QuestionName] = json.TagField3QuestionValue;
                    obj[json.TagField4QuestionName] = json.TagField4QuestionValue;
                    obj[json.TagField5QuestionName] = json.TagField5QuestionValue;
                    obj[json.TagField6QuestionName] = json.TagField6QuestionValue;
                    obj[json.TagField7QuestionName] = json.TagField7QuestionValue;
                    obj[json.TagField8QuestionName] = json.TagField8QuestionValue;
                    obj[json.TagField9QuestionName] = json.TagField9QuestionValue;
                    obj[json.TagField10QuestionName] = json.TagField10QuestionValue;

                    obj[json.TagField11QuestionName] = json.TagField11QuestionValue;
                    obj[json.TagField12QuestionName] = json.TagField12QuestionValue;
                    obj[json.TagField13QuestionName] = json.TagField13QuestionValue;
                    obj[json.TagField14QuestionName] = json.TagField14QuestionValue;
                    obj[json.TagField15QuestionName] = json.TagField15QuestionValue;
                    obj[json.TagField16QuestionName] = json.TagField16QuestionValue;
                    obj[json.TagField17QuestionName] = json.TagField17QuestionValue;
                    obj[json.TagField18QuestionName] = json.TagField18QuestionValue;
                    obj[json.TagField19QuestionName] = json.TagField19QuestionValue;
                    obj[json.TagField20QuestionName] = json.TagField20QuestionValue;

                    obj[json.TagField21QuestionName] = json.TagField21QuestionValue;
                    obj[json.TagField22QuestionName] = json.TagField22QuestionValue;
                    obj[json.TagField23QuestionName] = json.TagField23QuestionValue;
                    obj[json.TagField24QuestionName] = json.TagField24QuestionValue;
                    obj[json.TagField25QuestionName] = json.TagField25QuestionValue;
                    obj[json.TagField26QuestionName] = json.TagField26QuestionValue;
                    obj[json.TagField27QuestionName] = json.TagField27QuestionValue;
                    obj[json.TagField28QuestionName] = json.TagField28QuestionValue;
                    obj[json.TagField29QuestionName] = json.TagField29QuestionValue;
                    obj[json.TagField30QuestionName] = json.TagField30QuestionValue;

                    obj[json.TagField31QuestionName] = json.TagField31QuestionValue;
                    obj[json.TagField32QuestionName] = json.TagField32QuestionValue;
                    obj[json.TagField33QuestionName] = json.TagField33QuestionValue;
                    obj[json.TagField34QuestionName] = json.TagField34QuestionValue;
                    obj[json.TagField35QuestionName] = json.TagField35QuestionValue;
                    obj[json.TagField36QuestionName] = json.TagField36QuestionValue;
                    obj[json.TagField37QuestionName] = json.TagField37QuestionValue;
                    obj[json.TagField38QuestionName] = json.TagField38QuestionValue;
                    obj[json.TagField39QuestionName] = json.TagField39QuestionValue;
                    obj[json.TagField40QuestionName] = json.TagField40QuestionValue;

                    obj[json.TagField41QuestionName] = json.TagField41QuestionValue;
                    obj[json.TagField42QuestionName] = json.TagField42QuestionValue;
                    obj[json.TagField43QuestionName] = json.TagField43QuestionValue;
                    obj[json.TagField44QuestionName] = json.TagField44QuestionValue;
                    obj[json.TagField45QuestionName] = json.TagField45QuestionValue;
                    obj[json.TagField46QuestionName] = json.TagField46QuestionValue;
                    obj[json.TagField47QuestionName] = json.TagField47QuestionValue;
                    obj[json.TagField48QuestionName] = json.TagField48QuestionValue;
                    obj[json.TagField49QuestionName] = json.TagField49QuestionValue;
                    obj[json.TagField50QuestionName] = json.TagField50QuestionValue;
                    obj.Remove("");
                    lstJson += JsonConvert.SerializeObject(obj) + ",";
                }
                jsonFinal = ("[" + lstJson.Remove(lstJson.Length - 1, 1) + "]").Replace(" ", string.Empty).Trim();
                result = string.Format("{0}|{1}|{2}", "1", "null", jsonFinal);
            }
            else
            {
                result = string.Format("{0}|{1}|{2}", "0", "Data tidak ditemukan", "null");
            }

            return result;
        }
        #endregion
    }
}
