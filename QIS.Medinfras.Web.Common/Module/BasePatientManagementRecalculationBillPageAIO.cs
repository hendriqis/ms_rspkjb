using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Common
{
    public abstract class BasePatientManagementRecalculationBillPageAIO : BaseEntryPopupCtl
    {
        private Int32 hdnLinkedRegistrationID;
        private Int32 hdnRegistrationID;

        protected void OnProcessRecalculation(int registrationID, int[] lstParam, string paramTo)
        {
            Registration registration = BusinessLayer.GetRegistration(registrationID);

            string isPriceDrugUpdate = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_HARGA_OBAT_ALKES_LOGISTIK_BERUBAH_KE_HARGA_TERAKHIR).ParameterValue;

            List<vPatientChargesDt8> lstPatientChargesDtTemp = ListPatientChargesDt.Where(a => lstParam.Any(b => b == a.ID)).ToList();
            foreach (vPatientChargesDt8 patientChargesDt in lstPatientChargesDtTemp)
            {
                if (patientChargesDt.IsVariable)
                {
                    continue;
                }

                if (patientChargesDt.IsUnbilledItem)
                {
                    continue;
                }

                int type = 1;
                if (patientChargesDt.GCItemType == Constant.ItemType.OBAT_OBATAN || patientChargesDt.GCItemType == Constant.ItemType.BARANG_MEDIS)
                {
                    type = 2;
                }
                else if (patientChargesDt.GCItemType == Constant.ItemType.BARANG_UMUM)
                {
                    type = 3;
                }

                string isObatAlkesUmum = "0";
                if (patientChargesDt.GCItemType == Constant.ItemType.OBAT_OBATAN || patientChargesDt.GCItemType == Constant.ItemType.BARANG_MEDIS || patientChargesDt.GCItemType == Constant.ItemType.BARANG_UMUM)
                {
                    isObatAlkesUmum = "1";
                }

                string[] paramToFix = paramTo.Split(';');

                int toClassID = paramToFix[0] != "0" ? Convert.ToInt32(paramToFix[0]) : patientChargesDt.ChargeClassID;
                string toClassName = paramToFix[0] != "0" ? paramToFix[1] : patientChargesDt.ChargeClassName;

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

                decimal lastCostAmount = 0, lastAveragePrice = 0;

                lastAveragePrice = patientChargesDt.AveragePrice;
                lastCostAmount = patientChargesDt.CostAmount;

                if (type == 2 || type == 3)
                {
                    #region Rekal Pakai HNA Terakhir
                    List<GetCurrentItemTariffAIO> list = BusinessLayer.GetCurrentItemTariffAIO(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, type, patientChargesDt.TransactionDate, null, 0, patientChargesDt.TransactionID);

                    if (list.Count > 0)
                    {
                        GetCurrentItemTariffAIO obj = list[0];
                        basePrice = obj.BasePrice;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                        priceComp1 = obj.PriceComp1;
                        priceComp2 = obj.PriceComp2;
                        priceComp3 = obj.PriceComp3;
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

                    ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, patientChargesDt.ItemID)).FirstOrDefault();
                    lastAveragePrice = iPlanning.AveragePrice;
                    lastCostAmount = iPlanning.UnitPrice;
                    #endregion
                }
                else
                {
                    List<GetCurrentItemTariffAIO> list = BusinessLayer.GetCurrentItemTariffAIO(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, type, patientChargesDt.TransactionDate, null, 0, patientChargesDt.TransactionID);

                    if (list.Count > 0)
                    {
                        GetCurrentItemTariffAIO obj = list[0];
                        basePrice = obj.BasePrice;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                        priceComp1 = obj.PriceComp1;
                        priceComp2 = obj.PriceComp2;
                        priceComp3 = obj.PriceComp3;
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
                }

                #region Rekal Buku Tariff

                decimal totalDiscountAmount = discountAmount;
                decimal totalDiscountAmount1 = discountAmountComp1;
                decimal totalDiscountAmount2 = discountAmountComp2;
                decimal totalDiscountAmount3 = discountAmountComp3;

                decimal grossLineAmount = patientChargesDt.GrossLineAmount;

                if (isPriceDrugUpdate == "1" || isObatAlkesUmum == "0")
                {
                    #region Update Tariff Obat, Alkes, Barang Umum

                    patientChargesDt.ChargeClassID = toClassID;
                    patientChargesDt.ChargeClassName = toClassName;
                    patientChargesDt.BaseTariff = basePrice;
                    patientChargesDt.Tariff = price;
                    patientChargesDt.BaseComp1 = basePriceComp1;
                    patientChargesDt.BaseComp2 = basePriceComp2;
                    patientChargesDt.BaseComp3 = basePriceComp3;
                    patientChargesDt.TariffComp1 = priceComp1;
                    patientChargesDt.TariffComp2 = priceComp2;
                    patientChargesDt.TariffComp3 = priceComp3;

                    patientChargesDt.IsVariable = false;

                    grossLineAmount = patientChargesDt.ChargedQuantity * price;

                    //coverageAmount = coverageAmount + patientChargesDt.EmbalaceAmount; // ditutup oleh RN per patch 202112-01 karna saat rekal dari instansi ke personal nilai embalace double dan sisa ke instansinya

                    patientChargesDt.CITOAmount = 0;
                    patientChargesDt.ComplicationAmount = 0;

                    if (patientChargesDt.GCItemType == Constant.ItemGroupMaster.SERVICE || patientChargesDt.GCItemType == Constant.ItemGroupMaster.LABORATORY || patientChargesDt.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || patientChargesDt.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC || patientChargesDt.GCItemType == Constant.ItemGroupMaster.MEDICAL_CHECKUP)
                    {
                        vItemService entity = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", patientChargesDt.ItemID)).FirstOrDefault();
                        patientChargesDt.BaseComplicationAmount = entity.ComplicationAmount;
                        patientChargesDt.BaseCITOAmount = entity.CITOAmount;

                        if (patientChargesDt.IsComplication)
                        {
                            decimal totalComplicationAmount = 0;
                            if (entity.IsComplicationInPercentage)
                                totalComplicationAmount = grossLineAmount * entity.ComplicationAmount / 100;
                            else
                                totalComplicationAmount = entity.ComplicationAmount * patientChargesDt.ChargedQuantity;
                            patientChargesDt.ComplicationAmount = totalComplicationAmount;
                        }
                        else
                            patientChargesDt.ComplicationAmount = 0;

                        if (patientChargesDt.IsCITO)
                        {
                            decimal totalCITOAmount = 0;
                            if (entity.IsCITOInPercentage)
                                totalCITOAmount = grossLineAmount * entity.CITOAmount / 100;
                            else
                                totalCITOAmount = entity.CITOAmount * patientChargesDt.ChargedQuantity;
                            patientChargesDt.CITOAmount = totalCITOAmount;
                        }
                        else
                            patientChargesDt.CITOAmount = 0;

                        patientChargesDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                        patientChargesDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                        patientChargesDt.BaseCITOAmount = entity.CITOAmount;
                        patientChargesDt.BaseComplicationAmount = entity.ComplicationAmount;
                    }

                    #endregion
                }
                else
                {
                    if (patientChargesDt.ChargedQuantity != 0)
                    {
                        coverageAmount = patientChargesDt.PayerAmount / patientChargesDt.ChargedQuantity;
                    }
                    else
                    {
                        coverageAmount = 0;
                    }
                }

                patientChargesDt.AveragePrice = lastAveragePrice;
                patientChargesDt.CostAmount = lastCostAmount;

                //patientChargesDt.IsDiscount = totalDiscountAmount != 0 ? true : false;
                //patientChargesDt.DiscountAmount = totalDiscountAmount;
                //patientChargesDt.DiscountComp1 = totalDiscountAmount;
                //patientChargesDt.DiscountComp2 = 0;
                //patientChargesDt.DiscountComp3 = 0;

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

                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (patientChargesDt.ChargedQuantity);

                if (grossLineAmount >= 0)
                {
                    if (totalDiscountAmount > grossLineAmount)
                    {
                        totalDiscountAmount = grossLineAmount;
                    }
                }

                #region dtPackage
                ItemService its = BusinessLayer.GetItemService(patientChargesDt.ItemID);
                if (patientChargesDt.IsPackageItem)
                {
                    string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", patientChargesDt.ID);
                    List<PatientChargesDtPackage> lstPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage);

                    decimal oAccumulatedDiscountAmount = 0;
                    decimal oAccumulatedDiscountAmountComp1 = 0;
                    decimal oAccumulatedDiscountAmountComp2 = 0;
                    decimal oAccumulatedDiscountAmountComp3 = 0;

                    List<PatientChargesDtPackage> lstDtPackage = new List<PatientChargesDtPackage>();

                    foreach (PatientChargesDtPackage dtpackage in lstPackage)
                    {
                        decimal totalDiscountAmountPackage = 0;
                        decimal totalDiscountAmount1Package = 0;
                        decimal totalDiscountAmount2Package = 0;
                        decimal totalDiscountAmount3Package = 0;

                        string filterPackageService = string.Format("ItemID = {0} AND DetailItemID = {1} AND IsDeleted = 0", patientChargesDt.ItemID, dtpackage.ItemID);
                        vItemServiceDt isd = BusinessLayer.GetvItemServiceDtList(filterPackageService).FirstOrDefault();

                        int itemType = isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                        GetCurrentItemTariffAIO tariff = BusinessLayer.GetCurrentItemTariffAIO(registrationID, patientChargesDt.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now).FirstOrDefault();

                        decimal basePricePackage = 0;
                        decimal basePriceComp1Package = 0;
                        decimal basePriceComp2Package = 0;
                        decimal basePriceComp3Package = 0;
                        decimal pricePackage = 0;
                        decimal priceComp1Package = 0;
                        decimal priceComp2Package = 0;
                        decimal priceComp3Package = 0;
                        bool isDiscountUsedCompPackage = false;
                        decimal discountAmountPackage = 0;
                        decimal discountAmountComp1Package = 0;
                        decimal discountAmountComp2Package = 0;
                        decimal discountAmountComp3Package = 0;
                        decimal coverageAmountPackage = 0;
                        bool isDiscountInPercentagePackage = false;
                        bool isDiscountInPercentageComp1Package = false;
                        bool isDiscountInPercentageComp2Package = false;
                        bool isDiscountInPercentageComp3Package = false;
                        bool isCoverageInPercentagePackage = false;
                        decimal costAmountPackage = 0;
                        decimal grossLineAmountPackage = 0;

                        basePricePackage = tariff.BasePrice;
                        basePriceComp1Package = tariff.BasePriceComp1;
                        basePriceComp2Package = tariff.BasePriceComp2;
                        basePriceComp3Package = tariff.BasePriceComp3;
                        pricePackage = tariff.Price;
                        priceComp1Package = tariff.PriceComp1;
                        priceComp2Package = tariff.PriceComp2;
                        priceComp3Package = tariff.PriceComp3;
                        isDiscountUsedCompPackage = tariff.IsDiscountUsedComp;
                        discountAmountPackage = tariff.DiscountAmount;
                        discountAmountComp1Package = tariff.DiscountAmountComp1;
                        discountAmountComp2Package = tariff.DiscountAmountComp2;
                        discountAmountComp3Package = tariff.DiscountAmountComp3;
                        coverageAmountPackage = tariff.CoverageAmount;
                        isDiscountInPercentagePackage = tariff.IsDiscountInPercentage;
                        isDiscountInPercentageComp1Package = tariff.IsDiscountInPercentageComp1;
                        isDiscountInPercentageComp2Package = tariff.IsDiscountInPercentageComp2;
                        isDiscountInPercentageComp3Package = tariff.IsDiscountInPercentageComp3;
                        isCoverageInPercentagePackage = tariff.IsCoverageInPercentage;
                        costAmountPackage = tariff.CostAmount;
                        grossLineAmountPackage = dtpackage.ChargedQuantity * pricePackage;

                        dtpackage.BaseTariff = tariff.BasePrice;
                        dtpackage.BaseComp1 = tariff.BasePriceComp1;
                        dtpackage.BaseComp2 = tariff.BasePriceComp2;
                        dtpackage.BaseComp3 = tariff.BasePriceComp3;
                        dtpackage.Tariff = tariff.Price;
                        dtpackage.TariffComp1 = tariff.PriceComp1;
                        dtpackage.TariffComp2 = tariff.PriceComp2;
                        dtpackage.TariffComp3 = tariff.PriceComp3;
                        dtpackage.CostAmount = tariff.CostAmount;

                        if (isDiscountInPercentagePackage || isDiscountInPercentageComp1Package || isDiscountInPercentageComp2Package || isDiscountInPercentageComp3Package)
                        {
                            if (isDiscountUsedCompPackage)
                            {
                                if (priceComp1Package > 0)
                                {
                                    if (isDiscountInPercentageComp1Package)
                                    {
                                        totalDiscountAmount1Package = priceComp1Package * discountAmountComp1Package / 100;
                                        dtpackage.DiscountPercentageComp1 = discountAmountComp1Package;
                                    }
                                    else
                                    {
                                        totalDiscountAmount1Package = discountAmountComp1Package;
                                    }
                                }

                                if (priceComp2Package > 0)
                                {
                                    if (isDiscountInPercentageComp2Package)
                                    {
                                        totalDiscountAmount2Package = priceComp2Package * discountAmountComp2Package / 100;
                                        dtpackage.DiscountPercentageComp2 = discountAmountComp2Package;
                                    }
                                    else
                                    {
                                        totalDiscountAmount2Package = discountAmountComp2Package;
                                    }
                                }

                                if (priceComp3Package > 0)
                                {
                                    if (isDiscountInPercentageComp3Package)
                                    {
                                        totalDiscountAmount3Package = priceComp3Package * discountAmountComp3Package / 100;
                                        dtpackage.DiscountPercentageComp3 = discountAmountComp3Package;
                                    }
                                    else
                                    {
                                        totalDiscountAmount3Package = discountAmountComp3Package;
                                    }
                                }
                            }
                            else
                            {
                                if (priceComp1Package > 0)
                                {
                                    totalDiscountAmount1Package = priceComp1Package * discountAmountPackage / 100;
                                    dtpackage.DiscountPercentageComp1 = discountAmountPackage;
                                }

                                if (priceComp2Package > 0)
                                {
                                    totalDiscountAmount2Package = priceComp2Package * discountAmountPackage / 100;
                                    dtpackage.DiscountPercentageComp2 = discountAmountPackage;
                                }

                                if (priceComp3Package > 0)
                                {
                                    totalDiscountAmount3Package = priceComp3Package * discountAmountPackage / 100;
                                    dtpackage.DiscountPercentageComp3 = discountAmountPackage;
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
                            if (isDiscountUsedCompPackage)
                            {
                                if (priceComp1Package > 0)
                                    totalDiscountAmount1Package = discountAmountComp1Package;
                                if (priceComp2Package > 0)
                                    totalDiscountAmount2Package = discountAmountComp2Package;
                                if (priceComp3Package > 0)
                                    totalDiscountAmount3Package = discountAmountComp3Package;
                            }
                            else
                            {
                                if (priceComp1Package > 0)
                                    totalDiscountAmount1Package = discountAmountPackage;
                                if (priceComp2Package > 0)
                                    totalDiscountAmount2Package = discountAmountPackage;
                                if (priceComp3Package > 0)
                                    totalDiscountAmount3Package = discountAmountPackage;
                            }
                        }

                        totalDiscountAmountPackage = (totalDiscountAmount1Package + totalDiscountAmount2Package + totalDiscountAmount3Package) * (dtpackage.ChargedQuantity);

                        if (grossLineAmountPackage > 0)
                        {
                            if (totalDiscountAmountPackage > grossLineAmountPackage)
                            {
                                totalDiscountAmountPackage = grossLineAmountPackage;
                            }
                        }

                        dtpackage.DiscountAmount = totalDiscountAmountPackage;
                        dtpackage.DiscountComp1 = totalDiscountAmount1Package;
                        dtpackage.DiscountComp2 = totalDiscountAmount2Package;
                        dtpackage.DiscountComp3 = totalDiscountAmount3Package;

                        if (isd.IsPackageItem && isd.IsUsingAccumulatedPrice)
                        {
                            oAccumulatedDiscountAmount += dtpackage.DiscountAmount;
                            oAccumulatedDiscountAmountComp1 += dtpackage.DiscountComp1;
                            oAccumulatedDiscountAmountComp2 += dtpackage.DiscountComp2;
                            oAccumulatedDiscountAmountComp3 += dtpackage.DiscountComp3;
                        }

                        string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                        List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP);
                        if (iplan.Count() > 0)
                        {
                            dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                        }
                        else
                        {
                            dtpackage.AveragePrice = 0;
                        }

                        dtpackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                        lstDtPackage.Add(dtpackage);
                    }

                    if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                    {
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
                        foreach (PatientChargesDtPackage e in lstDtPackage)
                        {
                            BaseTariff += e.BaseTariff * e.ChargedQuantity;
                            BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                            BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                            BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                            Tariff += e.Tariff * e.ChargedQuantity;
                            TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                            TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                            TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                            DiscountAmount += e.DiscountAmount;
                            DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                            DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                            DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;
                        }

                        patientChargesDt.BaseTariff = BaseTariff / patientChargesDt.ChargedQuantity;
                        patientChargesDt.BaseComp1 = BaseComp1 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.BaseComp2 = BaseComp2 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.BaseComp3 = BaseComp3 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.Tariff = Tariff / patientChargesDt.ChargedQuantity;
                        patientChargesDt.TariffComp1 = TariffComp1 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.TariffComp2 = TariffComp2 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.TariffComp3 = TariffComp3 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.DiscountAmount = DiscountAmount;
                        patientChargesDt.DiscountComp1 = DiscountComp1 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.DiscountComp2 = DiscountComp2 / patientChargesDt.ChargedQuantity;
                        patientChargesDt.DiscountComp3 = DiscountComp3 / patientChargesDt.ChargedQuantity;

                        totalDiscountAmount = oAccumulatedDiscountAmount;
                        totalDiscountAmount1 = oAccumulatedDiscountAmountComp1;
                        totalDiscountAmount2 = oAccumulatedDiscountAmountComp2;
                        totalDiscountAmount3 = oAccumulatedDiscountAmountComp3;
                    }
                }
                #endregion

                patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                patientChargesDt.DiscountAmount = totalDiscountAmount;
                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                //decimal total = grossLineAmount - totalDiscountAmount + patientChargesDt.CITOAmount + patientChargesDt.ComplicationAmount;
                decimal total = grossLineAmount - totalDiscountAmount + patientChargesDt.CITOAmount + patientChargesDt.ComplicationAmount + patientChargesDt.EmbalaceAmount;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                {
                    if (total != 0)
                    {
                        totalPayer = total * coverageAmount / 100;
                    }
                    else
                    {
                        totalPayer = 0;
                    }
                }
                else
                {
                    totalPayer = coverageAmount * patientChargesDt.ChargedQuantity;
                }

                if (totalPayer < 0 && totalPayer < total)
                {
                    totalPayer = total;
                }
                else if (totalPayer > 0 & totalPayer > total)
                {
                    totalPayer = total;
                }

                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;

                #endregion
            }
            BindGrid();
        }

        protected bool OnSaveRecord(ref string errMessage, ref string retval, int registrationID, int linkedRegistrationID, int[] lstParam)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            try
            {
                hdnRegistrationID = registrationID;
                if (lstParam.Count() > 0)
                {
                    List<vPatientChargesDt8> lstPatientChargesDtTemp = ListPatientChargesDt.Where(a => lstParam.Any(b => b == a.ID)).ToList();
                    foreach (vPatientChargesDt8 patientChargesDt in lstPatientChargesDtTemp)
                    {
                        ItemService its = itemServiceDao.Get(patientChargesDt.ItemID);

                        PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                        entity.ChargeClassID = patientChargesDt.ChargeClassID;
                        entity.BaseTariff = patientChargesDt.BaseTariff;
                        entity.BaseComp1 = patientChargesDt.BaseComp1;
                        entity.BaseComp2 = patientChargesDt.BaseComp2;
                        entity.BaseComp3 = patientChargesDt.BaseComp3;

                        entity.AveragePrice = patientChargesDt.AveragePrice;
                        entity.CostAmount = patientChargesDt.CostAmount;

                        if (!entity.IsUnbilledItem)
                        {
                            entity.Tariff = patientChargesDt.Tariff;
                            entity.TariffComp1 = patientChargesDt.TariffComp1;
                            entity.TariffComp2 = patientChargesDt.TariffComp2;
                            entity.TariffComp3 = patientChargesDt.TariffComp3;
                            entity.IsVariable = entity.IsVariable;

                            if (patientChargesDt.GCItemType == Constant.ItemGroupMaster.SERVICE)
                            {
                                entity.BaseComplicationAmount = patientChargesDt.BaseComplicationAmount;
                                entity.BaseCITOAmount = patientChargesDt.BaseCITOAmount;
                                entity.ComplicationAmount = patientChargesDt.ComplicationAmount;
                                entity.CITOAmount = patientChargesDt.CITOAmount;
                                entity.IsCITOInPercentage = patientChargesDt.IsCITOInPercentage;
                                entity.IsComplicationInPercentage = patientChargesDt.IsComplicationInPercentage;
                            }

                            entity.IsDiscount = patientChargesDt.IsDiscount;
                            entity.DiscountAmount = patientChargesDt.DiscountAmount;
                            entity.DiscountComp1 = patientChargesDt.DiscountComp1;
                            entity.DiscountComp2 = patientChargesDt.DiscountComp2;
                            entity.DiscountComp3 = patientChargesDt.DiscountComp3;
                            entity.IsDiscountInPercentageComp1 = patientChargesDt.IsDiscountInPercentageComp1;
                            entity.IsDiscountInPercentageComp2 = patientChargesDt.IsDiscountInPercentageComp2;
                            entity.IsDiscountInPercentageComp3 = patientChargesDt.IsDiscountInPercentageComp3;
                            entity.DiscountPercentageComp1 = patientChargesDt.DiscountPercentageComp1;
                            entity.DiscountPercentageComp2 = patientChargesDt.DiscountPercentageComp2;
                            entity.DiscountPercentageComp3 = patientChargesDt.DiscountPercentageComp3;
                            entity.PatientAmount = patientChargesDt.PatientAmount;
                            entity.PayerAmount = patientChargesDt.PayerAmount;
                            entity.LineAmount = patientChargesDt.LineAmount;
                        }
                        entity.IsVerified = false;
                        entity.VerifiedBy = null;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);

                        #region dtPackage
                        if (patientChargesDt.IsPackageItem)
                        {
                            string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", patientChargesDt.ID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PatientChargesDtPackage> lstPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);

                            decimal oAccumulatedDiscountAmount = 0;
                            decimal oAccumulatedDiscountAmountComp1 = 0;
                            decimal oAccumulatedDiscountAmountComp2 = 0;
                            decimal oAccumulatedDiscountAmountComp3 = 0;

                            List<PatientChargesDtPackage> lstDtPackage = new List<PatientChargesDtPackage>();

                            foreach (PatientChargesDtPackage dtpackage in lstPackage)
                            {
                                string filterPackageService = string.Format("ItemID = {0} AND DetailItemID = {1} AND IsDeleted = 0", patientChargesDt.ItemID, dtpackage.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vItemServiceDt isd = BusinessLayer.GetvItemServiceDtList(filterPackageService, ctx).FirstOrDefault();

                                int itemType = isd.GCItemType == Constant.ItemType.OBAT_OBATAN || isd.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : isd.GCItemType == Constant.ItemType.BARANG_UMUM || isd.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                GetCurrentItemTariffAIO tariff = BusinessLayer.GetCurrentItemTariffAIO(registrationID, patientChargesDt.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

                                decimal basePricePackage = 0;
                                decimal basePriceComp1Package = 0;
                                decimal basePriceComp2Package = 0;
                                decimal basePriceComp3Package = 0;
                                decimal pricePackage = 0;
                                decimal priceComp1Package = 0;
                                decimal priceComp2Package = 0;
                                decimal priceComp3Package = 0;
                                bool isDiscountUsedCompPackage = false;
                                decimal discountAmountPackage = 0;
                                decimal discountAmountComp1Package = 0;
                                decimal discountAmountComp2Package = 0;
                                decimal discountAmountComp3Package = 0;
                                decimal coverageAmountPackage = 0;
                                bool isDiscountInPercentagePackage = false;
                                bool isDiscountInPercentageComp1Package = false;
                                bool isDiscountInPercentageComp2Package = false;
                                bool isDiscountInPercentageComp3Package = false;
                                bool isCoverageInPercentagePackage = false;
                                decimal costAmountPackage = 0;
                                decimal grossLineAmountPackage = 0;

                                basePricePackage = tariff.BasePrice;
                                basePriceComp1Package = tariff.BasePriceComp1;
                                basePriceComp2Package = tariff.BasePriceComp2;
                                basePriceComp3Package = tariff.BasePriceComp3;
                                pricePackage = tariff.Price;
                                priceComp1Package = tariff.PriceComp1;
                                priceComp2Package = tariff.PriceComp2;
                                priceComp3Package = tariff.PriceComp3;
                                isDiscountUsedCompPackage = tariff.IsDiscountUsedComp;
                                discountAmountPackage = tariff.DiscountAmount;
                                discountAmountComp1Package = tariff.DiscountAmountComp1;
                                discountAmountComp2Package = tariff.DiscountAmountComp2;
                                discountAmountComp3Package = tariff.DiscountAmountComp3;
                                coverageAmountPackage = tariff.CoverageAmount;
                                isDiscountInPercentagePackage = tariff.IsDiscountInPercentage;
                                isDiscountInPercentageComp1Package = tariff.IsDiscountInPercentageComp1;
                                isDiscountInPercentageComp2Package = tariff.IsDiscountInPercentageComp2;
                                isDiscountInPercentageComp3Package = tariff.IsDiscountInPercentageComp3;

                                isCoverageInPercentagePackage = tariff.IsCoverageInPercentage;
                                costAmountPackage = tariff.CostAmount;
                                grossLineAmountPackage = dtpackage.ChargedQuantity * pricePackage;

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

                                if (isDiscountInPercentagePackage || isDiscountInPercentageComp1Package || isDiscountInPercentageComp2Package || isDiscountInPercentageComp3Package)
                                {
                                    if (isDiscountUsedCompPackage)
                                    {
                                        if (priceComp1Package > 0)
                                        {
                                            if (isDiscountInPercentageComp1Package)
                                            {
                                                totalDiscountAmount1 = priceComp1Package * discountAmountComp1Package / 100;
                                                dtpackage.DiscountPercentageComp1 = discountAmountComp1Package;
                                            }
                                            else
                                            {
                                                totalDiscountAmount1 = discountAmountComp1Package;
                                            }
                                        }

                                        if (priceComp2Package > 0)
                                        {
                                            if (isDiscountInPercentageComp2Package)
                                            {
                                                totalDiscountAmount2 = priceComp2Package * discountAmountComp2Package / 100;
                                                dtpackage.DiscountPercentageComp2 = discountAmountComp2Package;
                                            }
                                            else
                                            {
                                                totalDiscountAmount2 = discountAmountComp2Package;
                                            }
                                        }

                                        if (priceComp3Package > 0)
                                        {
                                            if (isDiscountInPercentageComp3Package)
                                            {
                                                totalDiscountAmount3 = priceComp3Package * discountAmountComp3Package / 100;
                                                dtpackage.DiscountPercentageComp3 = discountAmountComp3Package;
                                            }
                                            else
                                            {
                                                totalDiscountAmount3 = discountAmountComp3Package;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (priceComp1Package > 0)
                                        {
                                            totalDiscountAmount1 = priceComp1Package * discountAmountPackage / 100;
                                            dtpackage.DiscountPercentageComp1 = discountAmountPackage;
                                        }

                                        if (priceComp2Package > 0)
                                        {
                                            totalDiscountAmount2 = priceComp2Package * discountAmountPackage / 100;
                                            dtpackage.DiscountPercentageComp2 = discountAmountPackage;
                                        }

                                        if (priceComp3Package > 0)
                                        {
                                            totalDiscountAmount3 = priceComp3Package * discountAmountPackage / 100;
                                            dtpackage.DiscountPercentageComp3 = discountAmountPackage;
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
                                    if (isDiscountUsedCompPackage)
                                    {
                                        if (priceComp1Package > 0)
                                            totalDiscountAmount1 = discountAmountComp1Package;
                                        if (priceComp2Package > 0)
                                            totalDiscountAmount2 = discountAmountComp2Package;
                                        if (priceComp3Package > 0)
                                            totalDiscountAmount3 = discountAmountComp3Package;
                                    }
                                    else
                                    {
                                        if (priceComp1Package > 0)
                                            totalDiscountAmount1 = discountAmountPackage;
                                        if (priceComp2Package > 0)
                                            totalDiscountAmount2 = discountAmountPackage;
                                        if (priceComp3Package > 0)
                                            totalDiscountAmount3 = discountAmountPackage;
                                    }
                                }

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                                if (grossLineAmountPackage > 0)
                                {
                                    if (totalDiscountAmount > grossLineAmountPackage)
                                    {
                                        totalDiscountAmount = grossLineAmountPackage;
                                    }
                                }

                                dtpackage.DiscountAmount = totalDiscountAmount;
                                dtpackage.DiscountComp1 = totalDiscountAmount1;
                                dtpackage.DiscountComp2 = totalDiscountAmount2;
                                dtpackage.DiscountComp3 = totalDiscountAmount3;

                                if (isd.IsPackageItem && isd.IsUsingAccumulatedPrice)
                                {
                                    oAccumulatedDiscountAmount += dtpackage.DiscountAmount;
                                    oAccumulatedDiscountAmountComp1 += dtpackage.DiscountComp1;
                                    oAccumulatedDiscountAmountComp2 += dtpackage.DiscountComp2;
                                    oAccumulatedDiscountAmountComp3 += dtpackage.DiscountComp3;
                                }

                                string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                                if (iplan.Count() > 0)
                                {
                                    dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                                }
                                else
                                {
                                    dtpackage.AveragePrice = 0;
                                }

                                dtpackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtPackageDao.Update(dtpackage);

                                lstDtPackage.Add(dtpackage);
                            }

                            if (oAccumulatedDiscountAmount != 0)
                            {
                                entity.IsDiscount = true;
                                entity.DiscountAmount = oAccumulatedDiscountAmount;
                                entity.DiscountComp1 = oAccumulatedDiscountAmountComp1;
                                entity.DiscountComp2 = oAccumulatedDiscountAmountComp2;
                                entity.DiscountComp3 = oAccumulatedDiscountAmountComp3;
                                entity.DiscountPercentageComp1 = 0;
                                entity.DiscountPercentageComp2 = 0;
                                entity.DiscountPercentageComp3 = 0;
                                entity.IsDiscountInPercentageComp1 = false;
                                entity.IsDiscountInPercentageComp2 = false;
                                entity.IsDiscountInPercentageComp3 = false;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }

                            PatientChargesDt pcdt = entityDtDao.Get(entity.ID);
                            if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                            {
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
                                foreach (PatientChargesDtPackage e in lstDtPackage)
                                {
                                    BaseTariff += e.BaseTariff * e.ChargedQuantity;
                                    BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                                    BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                                    BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                                    Tariff += e.Tariff * e.ChargedQuantity;
                                    TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                                    TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                                    TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                                    DiscountAmount += e.DiscountAmount;
                                    DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                                    DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                                    DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;
                                }

                                pcdt.BaseTariff = BaseTariff / entity.ChargedQuantity;
                                pcdt.BaseComp1 = BaseComp1 / entity.ChargedQuantity;
                                pcdt.BaseComp2 = BaseComp2 / entity.ChargedQuantity;
                                pcdt.BaseComp3 = BaseComp3 / entity.ChargedQuantity;
                                pcdt.Tariff = Tariff / entity.ChargedQuantity;
                                pcdt.TariffComp1 = TariffComp1 / entity.ChargedQuantity;
                                pcdt.TariffComp2 = TariffComp2 / entity.ChargedQuantity;
                                pcdt.TariffComp3 = TariffComp3 / entity.ChargedQuantity;
                                pcdt.DiscountAmount = DiscountAmount;
                                pcdt.DiscountComp1 = DiscountComp1 / entity.ChargedQuantity;
                                pcdt.DiscountComp2 = DiscountComp2 / entity.ChargedQuantity;
                                pcdt.DiscountComp3 = DiscountComp3 / entity.ChargedQuantity;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<GetCurrentItemTariffAIO> list = BusinessLayer.GetCurrentItemTariffAIO(registrationID, patientChargesDt.VisitID, pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                                decimal coverageAmount = 0;
                                bool isCoverageInPercentage = false;
                                if (list.Count > 0)
                                {
                                    GetCurrentItemTariffAIO obj = list[0];
                                    coverageAmount = obj.CoverageAmount;
                                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                                }

                                decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                                decimal totalDiscountAmount = pcdt.DiscountAmount;
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

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Update(pcdt);
                            }
                            else if (!its.IsUsingAccumulatedPrice && its.IsPackageItem)
                            {
                                foreach (PatientChargesDtPackage e in lstDtPackage)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    PatientChargesDtPackage packageDt = entityDtPackageDao.Get(e.ID);
                                    if (e.TariffComp1 != 0)
                                    {
                                        packageDt.DiscountComp1 = ((e.Tariff / pcdt.Tariff) * pcdt.DiscountComp1);
                                    }
                                    if (e.TariffComp2 != 0)
                                    {
                                        packageDt.DiscountComp2 = ((e.Tariff / pcdt.Tariff) * pcdt.DiscountComp2);
                                    }
                                    if (e.TariffComp3 != 0)
                                    {
                                        packageDt.DiscountComp3 = ((e.Tariff / pcdt.Tariff) * pcdt.DiscountComp3);
                                    }
                                    packageDt.DiscountAmount = ((packageDt.DiscountComp1 + packageDt.DiscountComp2 + packageDt.DiscountComp3) * packageDt.ChargedQuantity);
                                    packageDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityDtPackageDao.Update(packageDt);
                                }
                            }
                        }
                        #endregion
                    }

                    String filterExpression = GetFilterExpression();
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression, ctx);
                    foreach (vPatientChargesHd patientChargesHd in lstPatientChargesHd)
                    {
                        PatientChargesHd entityHd = entityHdDao.Get(patientChargesHd.TransactionID);

                        List<vPatientChargesDt8> lst = ListPatientChargesDt.Where(p => p.TransactionID == entityHd.TransactionID).ToList();
                        entityHd.TotalPatientAmount = lst.Select(p => p.PatientAmount).Sum();
                        entityHd.TotalPayerAmount = lst.Select(p => p.PayerAmount).Sum();
                        entityHd.TotalAmount = lst.Select(p => p.LineAmount).Sum();
                        entityHd.IsVerified = false;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHd.LastRecalculatedBy = AppSession.UserLogin.UserID;
                        entityHd.LastRecalculatedDate = DateTime.Now;
                        entityHd.IsPendingRecalculated = false;
                        entityHdDao.Update(entityHd);
                    }
                    HttpContext.Current.Session.Remove(SESSION_LIST_CHARGES_DT);
                    ctx.CommitTransaction();
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            //if (hdnLinkedRegistrationID.ToString() != "" || hdnLinkedRegistrationID.ToString() != "0")
            //    filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID, hdnLinkedRegistrationID);
            //else
            //    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID);

            filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1))", hdnRegistrationID);
            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            return filterExpression;
        }

        protected abstract void BindGrid();

        private const string SESSION_LIST_CHARGES_DT = "RecalculateListChargesDt";
        public static List<vPatientChargesDt8> ListPatientChargesDt
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_LIST_CHARGES_DT] == null)
                {
                    HttpContext.Current.Session[SESSION_LIST_CHARGES_DT] = new List<vPatientChargesDt8>();
                }
                return (List<vPatientChargesDt8>)HttpContext.Current.Session[SESSION_LIST_CHARGES_DT];
            }
            set
            {
                HttpContext.Current.Session[SESSION_LIST_CHARGES_DT] = value;
            }
        }
    }
}
