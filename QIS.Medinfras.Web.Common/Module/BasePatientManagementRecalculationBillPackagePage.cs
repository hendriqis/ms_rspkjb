using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Common
{
    public abstract class BasePatientManagementRecalculationBillPackagePage : BaseEntryPopupCtl
    {
        private Int32 hdnLinkedRegistrationID;
        private Int32 hdnRegistrationID;

        protected void OnProcessRecalculation(int registrationID, bool IsUsedLastHNA, bool IsIncludeVariableTariff, bool IsResetItemTariff, int[] lstParam, string paramTo)
        {
            Registration registration = BusinessLayer.GetRegistration(registrationID);

            string isPriceDrugUpdate = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_HARGA_OBAT_ALKES_LOGISTIK_BERUBAH_KE_HARGA_TERAKHIR).ParameterValue;

            SettingParameterDt SettingParameter = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST);

            List<vPatientChargesDt8> lstItemPackage = ListPatientChargesDt.Where(t => t.ItemPackageID != 0).GroupBy(test => test.ItemPackageID).Select(grp => grp.First()).ToList().OrderBy(x => x.ItemPackageID).ToList();
            string itemPackageID = string.Join(",", lstItemPackage.Select(x => x.ItemPackageID));
            string filterExpressionDt = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", itemPackageID);
            List<vItemServiceDt> lstItemServiceDt = BusinessLayer.GetvItemServiceDtList(filterExpressionDt);

            List<vPatientChargesDt8> lstMainWithoutSelisih = ListPatientChargesDt.Where(t => t.ItemID.ToString() != SettingParameter.ParameterValue).ToList();
            List<vPatientChargesDt8> lstMainSelisih = ListPatientChargesDt.Where(t => t.ItemID.ToString() == SettingParameter.ParameterValue).ToList();

            List<vPatientChargesDt8> lstPatientChargesDtTemp = lstMainWithoutSelisih.Where(a => lstParam.Any(b => b == a.ID)).ToList();
            List<vPatientChargesDt8> lstPatientChargesDtTemp2 = lstMainSelisih.Where(a => lstParam.Any(b => b == a.ID)).ToList();

            foreach (vPatientChargesDt8 patientChargesDt in lstPatientChargesDtTemp)
            {
                if (!IsIncludeVariableTariff && patientChargesDt.IsVariable)
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
                    if (IsUsedLastHNA)
                    {
                        #region Rekal Pakai HNA Terakhir
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, type, patientChargesDt.TransactionDate, null, 0, patientChargesDt.TransactionID);

                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff obj = list[0];
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
                        #region Rekal Pakai HNA Sebelumnya
                        List<GetCurrentItemTariff2> list = BusinessLayer.GetCurrentItemTariff2(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, type, patientChargesDt.TransactionDate, null, 0, patientChargesDt.TransactionID, patientChargesDt.CostAmount);

                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff2 obj = list[0];
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
                        #endregion
                    }
                }
                else
                {
                    #region other
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, type, DateTime.Now);

                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
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
                    #endregion
                }

                if (IsResetItemTariff)
                {
                    #region Rekal Buku Tariff

                    decimal totalDiscountAmount = patientChargesDt.DiscountAmount;
                    decimal totalDiscountAmount1 = 0;
                    decimal totalDiscountAmount2 = 0;
                    decimal totalDiscountAmount3 = 0;

                    decimal grossLineAmount = patientChargesDt.GrossLineAmount;

                    if (isPriceDrugUpdate == "1" || isObatAlkesUmum == "0")
                    {
                        #region Update Tariff Obat, Alkes, Barang Umum

                        patientChargesDt.ChargeClassID = toClassID;
                        patientChargesDt.ChargeClassName = toClassName;

                        vItemServiceDt itemServiceDt = lstItemServiceDt.Where(t => t.ItemID == patientChargesDt.ItemPackageID && t.DetailItemID == patientChargesDt.ItemID).FirstOrDefault();
                        if (patientChargesDt.ItemPackageID != 0 && itemServiceDt != null)
                        {
                            patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                            patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                            patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountComp1;
                            patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountComp2;
                            patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountComp3;
                            patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountComp1;
                            patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountComp2;
                            patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountComp3;
                        }
                        else
                        {
                            patientChargesDt.BaseTariff = basePrice;
                            patientChargesDt.Tariff = price;
                            patientChargesDt.BaseComp1 = basePriceComp1;
                            patientChargesDt.BaseComp2 = basePriceComp2;
                            patientChargesDt.BaseComp3 = basePriceComp3;
                            patientChargesDt.TariffComp1 = priceComp1;
                            patientChargesDt.TariffComp2 = priceComp2;
                            patientChargesDt.TariffComp3 = priceComp3;
                        }

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
                else
                {
                    #region Rekal Biasa
                    vItemServiceDt itemServiceDt = lstItemServiceDt.Where(t => t.ItemID == patientChargesDt.ItemPackageID && t.DetailItemID == patientChargesDt.ItemID).FirstOrDefault();
                    if (patientChargesDt.ItemPackageID == 0 || itemServiceDt == null)
                    {
                        patientChargesDt.ChargeClassID = toClassID;
                        patientChargesDt.ChargeClassName = toClassName;
                        decimal total = patientChargesDt.LineAmount;
                        decimal totalPayer = 0;
                        if (isCoverageInPercentage)
                        {
                            totalPayer = total * (coverageAmount / 100);
                        }
                        else
                        {
                            totalPayer = coverageAmount * patientChargesDt.ChargedQuantity;
                        }

                        //if (total > 0 && totalPayer > total)
                        //{
                        //    totalPayer = total;
                        //}

                        if (totalPayer < 0 && totalPayer < total)
                        {
                            totalPayer = total;
                        }
                        else if (totalPayer > 0 & totalPayer > total)
                        {
                            totalPayer = total;
                        }

                        patientChargesDt.AveragePrice = lastAveragePrice;
                        patientChargesDt.CostAmount = lastCostAmount;

                        patientChargesDt.PatientAmount = total - totalPayer;
                        patientChargesDt.PayerAmount = totalPayer;
                    }
                    else
                    {
                        patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                        patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountComp1;
                        patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountComp2;
                        patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountComp3;

                        patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                        patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountComp1;
                        patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountComp2;
                        patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountComp3;

                        ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", patientChargesDt.ItemID)).FirstOrDefault();
                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                        patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;
                        patientChargesDt.IsCITO = false;
                        patientChargesDt.CITOAmount = 0;

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
                    }
                    #endregion
                }
            }

            #region Rekalkulasi Selisih Paket
            if (lstPatientChargesDtTemp2.Count > 0)
            {
                foreach (vPatientChargesDt8 patientChargesDt in lstPatientChargesDtTemp2)
                {
                    string[] paramToFix = paramTo.Split(';');
                    int toClassID = paramToFix[0] != "0" ? Convert.ToInt32(paramToFix[0]) : patientChargesDt.ChargeClassID;

                    vItemServiceDt itemServiceDtCheck = lstItemServiceDt.Where(t => t.ItemID == patientChargesDt.ItemPackageID).FirstOrDefault();

                    List<GetCurrentItemTariff> listPackage = BusinessLayer.GetCurrentItemTariff(registrationID, patientChargesDt.VisitID, toClassID, itemServiceDtCheck.ItemID, 1, DateTime.Now);
                    decimal itemPackagePrice = listPackage.FirstOrDefault().Price;
                    decimal itemPackagePriceComp1 = listPackage.FirstOrDefault().PriceComp1;
                    decimal itemPackagePriceComp2 = listPackage.FirstOrDefault().PriceComp2;
                    decimal itemPackagePriceComp3 = listPackage.FirstOrDefault().PriceComp3;

                    List<vPatientChargesDt8> lstPCD = ListPatientChargesDt.Where(t => t.ItemPackageID == patientChargesDt.ItemPackageID && t.ItemID != patientChargesDt.ItemID).ToList();
                    decimal totalChargesDTBaseTariff = lstPCD.Sum(a => a.BaseTariff);
                    decimal totalChargesDTBaseComp1 = lstPCD.Sum(a => a.BaseComp1);
                    decimal totalChargesDTBaseComp2 = lstPCD.Sum(a => a.BaseComp2);
                    decimal totalChargesDTBaseComp3 = lstPCD.Sum(a => a.BaseComp3);
                    decimal totalChargesDTTariff = lstPCD.Sum(a => a.Tariff);
                    decimal totalChargesDTTariffComp1 = lstPCD.Sum(a => a.TariffComp1);
                    decimal totalChargesDTTariffComp2 = lstPCD.Sum(a => a.TariffComp2);
                    decimal totalChargesDTTariffComp3 = lstPCD.Sum(a => a.TariffComp3);

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, 1, DateTime.Now);
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
                }
            }
            #endregion

            BindGrid();
        }

        protected bool OnSaveRecord(ref string errMessage, ref string retval, int registrationID, int linkedRegistrationID, int[] lstParam)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                hdnRegistrationID = registrationID;
                if (lstParam.Count() > 0)
                {
                    List<vPatientChargesDt8> lstPatientChargesDtTemp = ListPatientChargesDt.Where(a => lstParam.Any(b => b == a.ID)).ToList();
                    foreach (vPatientChargesDt8 patientChargesDt in lstPatientChargesDtTemp)
                    {
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
                    }
                    String filterExpression = GetFilterExpression();
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

        private const string SESSION_LIST_CHARGES_DT = "RecalculateListChargesDtPackage";
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
