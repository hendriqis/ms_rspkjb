using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using System.Web;

namespace QIS.Medinfras.Web.Common
{
    public abstract class BasePatientManagementRecalculationControlClassBillPage : BaseEntryPopupCtl
    {
        private Int32 hdnLinkedRegistrationID;
        private Int32 hdnRegistrationID;

        protected void OnProcessRecalculationControlClass(int registrationID, bool IsIncludeVariableTariff, bool IsResetItemTariff, int[] lstParam, string paramTo)
        {
            Registration registration = BusinessLayer.GetRegistration(registrationID);

            string isPriceDrugUpdate = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_HARGA_OBAT_ALKES_LOGISTIK_BERUBAH_KE_HARGA_TERAKHIR).ParameterValue;

            List<vPatientChargesClassCoverageDt> lstPatientChargesDtTemp = ListPatientChargesDt.Where(a => lstParam.Any(b => b == a.ID)).ToList();
            foreach (vPatientChargesClassCoverageDt patientChargesDt in lstPatientChargesDtTemp)
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

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, patientChargesDt.VisitID, toClassID, patientChargesDt.ItemID, type, patientChargesDt.TransactionDate, null, 0, patientChargesDt.TransactionID);

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
                        coverageAmount = coverageAmount + patientChargesDt.EmbalaceAmount;

                        if (isDiscountInPercentage)
                            totalDiscountAmount = grossLineAmount * discountAmount / 100;
                        else
                            totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                        if (patientChargesDt.ChargedQuantity > -1)
                        {
                            if (totalDiscountAmount > grossLineAmount)
                                totalDiscountAmount = grossLineAmount;
                        }

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

                    //patientChargesDt.IsDiscount = false;
                    //patientChargesDt.DiscountAmount = totalDiscountAmount;

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

                    if (totalDiscountAmount > grossLineAmount)
                    {
                        totalDiscountAmount = grossLineAmount;
                    }

                    patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                    patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                    patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                    patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;
                    patientChargesDt.LineAmount = total;

                    #endregion
                }
                else
                {
                    #region Rekal Biasa

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

                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;

                    #endregion
                }
            }
            BindGrid();
        }

        protected bool OnSaveRecord(ref string errMessage, ref string retval, int registrationID,int linkedRegistrationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesClassCoverageHdDao entityHdDao = new PatientChargesClassCoverageHdDao(ctx);
            PatientChargesClassCoverageDtDao entityDtDao = new PatientChargesClassCoverageDtDao(ctx);
            try
            {
                hdnRegistrationID = registrationID;
                hdnLinkedRegistrationID = linkedRegistrationID;
                foreach (vPatientChargesClassCoverageDt patientChargesDt in ListPatientChargesDt)
                {
                    PatientChargesClassCoverageDt entity = entityDtDao.Get(patientChargesDt.ID);
                    entity.ChargeClassID = patientChargesDt.ChargeClassID;
                    entity.BaseTariff = patientChargesDt.BaseTariff;
                    if (!entity.IsUnbilledItem)
                    {
                        entity.Tariff = patientChargesDt.Tariff;
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
                    //entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                }
                String filterExpression = GetFilterExpression();
                List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression, ctx);
                foreach (vPatientChargesHd patientChargesHd in lstPatientChargesHd)
                {
                    PatientChargesClassCoverageHd entityHd = entityHdDao.Get(patientChargesHd.TransactionID);

                    List<vPatientChargesClassCoverageDt> lst = ListPatientChargesDt.Where(p => p.TransactionID == entityHd.TransactionID).ToList();
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.ToString() != "" || hdnLinkedRegistrationID.ToString() != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID, hdnLinkedRegistrationID);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID);

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            return filterExpression;
        }

        protected abstract void BindGrid();

        private const string SESSION_LIST_CHARGES_DT = "CalculateListChargesControlClass";
        public static List<vPatientChargesClassCoverageDt> ListPatientChargesDt
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_LIST_CHARGES_DT] == null)
                {
                    HttpContext.Current.Session[SESSION_LIST_CHARGES_DT] = new List<vPatientChargesDt>();
                }
                return (List<vPatientChargesClassCoverageDt>)HttpContext.Current.Session[SESSION_LIST_CHARGES_DT];
            }
            set
            {
                HttpContext.Current.Session[SESSION_LIST_CHARGES_DT] = value;
            }
        }
    }
}
