using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessAllocationClass : BasePageTrx
    {
        private string pageTitle = string.Empty;

        protected string GetPageTitle()
        {
            return pageTitle;
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PROCESS_CONTROL_CLASS;
                default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PROCESS_CONTROL_CLASS;
            }
        }

        protected override void InitializeDataControl()
        {
            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit2 entityVisit = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnVisitID.Value = entityVisit.VisitID.ToString();

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField(cboServiceChargeClassID, lstClassCare, "ClassName", "ClassID");
            if (entityVisit.ChargeClassID != 0)
            {
                cboServiceChargeClassID.Value = Convert.ToString(entityVisit.ChargeClassID);
            }

            BindGrid();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrid();
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1))", hdnRegistrationID.Value);

            return filterExpression;
        }

        private void BindGrid()
        {
            string filterExpression = GetFilterExpression() + " ORDER BY TransactionID DESC, ItemName1 ASC";
            List<vPatientChargesDt9> lstEntity = BusinessLayer.GetvPatientChargesDt9List(filterExpression);

            hdnPatientChargesHdID.Value = "";
            hdnPatientChargesDtID.Value = "";

            var lstEntityHd = lstEntity.GroupBy(a => a.TransactionID).Select(b => b.First()).ToList().OrderBy(c => c.TransactionID);
            foreach (vPatientChargesDt9 hd in lstEntityHd)
            {
                if (hdnPatientChargesHdID.Value == "")
                {
                    hdnPatientChargesHdID.Value = Convert.ToString(hd.TransactionID);
                }
                else
                {
                    hdnPatientChargesHdID.Value = hdnPatientChargesHdID.Value + "," + Convert.ToString(hd.TransactionID);
                }
            }

            foreach (vPatientChargesDt9 dt in lstEntity)
            {
                if (hdnPatientChargesDtID.Value == "")
                {
                    hdnPatientChargesDtID.Value = Convert.ToString(dt.ID);
                }
                else
                {
                    hdnPatientChargesDtID.Value = hdnPatientChargesDtID.Value + "," + Convert.ToString(dt.ID);
                }
            }

            hdnPatientChargesHdIDLinked.Value = "";
            hdnPatientChargesDtIDLinked.Value = "";

            var lstEntityHdLinked = lstEntity.Where(z => z.LinkedToRegistrationID != null && z.LinkedToRegistrationID != 0).GroupBy(a => a.TransactionID).Select(b => b.First()).ToList().OrderBy(c => c.TransactionID);
            foreach (vPatientChargesDt9 hd in lstEntityHdLinked)
            {
                if (hdnPatientChargesHdIDLinked.Value == "")
                {
                    hdnPatientChargesHdIDLinked.Value = Convert.ToString(hd.TransactionID);
                }
                else
                {
                    hdnPatientChargesHdIDLinked.Value = hdnPatientChargesHdIDLinked.Value + "," + Convert.ToString(hd.TransactionID);
                }
            }

            foreach (vPatientChargesDt9 dt in lstEntity.Where(z => z.LinkedToRegistrationID != null && z.LinkedToRegistrationID != 0))
            {
                if (hdnPatientChargesDtIDLinked.Value == "")
                {
                    hdnPatientChargesDtIDLinked.Value = Convert.ToString(dt.ID);
                }
                else
                {
                    hdnPatientChargesDtIDLinked.Value = hdnPatientChargesDtIDLinked.Value + "," + Convert.ToString(dt.ID);
                }
            }

            #region services
            List<vPatientChargesDt9> lstService = lstEntity.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.LABORATORY
                                                                                || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            lvwService.DataSource = lstService;
            lvwService.DataBind();
            if (lstService.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = lstService.Sum(p => p.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = lstService.Sum(p => p.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = lstService.Sum(p => p.LineAmount).ToString(Constant.FormatString.NUMERIC_2);
            }
            #endregion

            #region drugms
            List<vPatientChargesDt9> lstDrugMS = lstEntity.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            lvwDrugMS.DataSource = lstDrugMS;
            lvwDrugMS.DataBind();
            if (lstDrugMS.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPayer")).InnerHtml = lstDrugMS.Sum(p => p.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPatient")).InnerHtml = lstDrugMS.Sum(p => p.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotal")).InnerHtml = lstDrugMS.Sum(p => p.LineAmount).ToString(Constant.FormatString.NUMERIC_2);
            }
            #endregion

            #region logistics
            List<vPatientChargesDt9> lstLogistics = lstEntity.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            lvwLogistic.DataSource = lstLogistics;
            lvwLogistic.DataBind();
            if (lstLogistics.Count > 0)
            {
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPayer")).InnerHtml = lstLogistics.Sum(p => p.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPatient")).InnerHtml = lstLogistics.Sum(p => p.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotal")).InnerHtml = lstLogistics.Sum(p => p.LineAmount).ToString(Constant.FormatString.NUMERIC_2);
            }
            #endregion
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityPatientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityPatientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesClassCoverageHdDao entityPatientChargesClassCoverageHdDao = new PatientChargesClassCoverageHdDao(ctx);
            PatientChargesClassCoverageDtDao entityPatientChargesClassCoverageDtDao = new PatientChargesClassCoverageDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);

            try
            {
                if (type == "process")
                {
                    if (!String.IsNullOrEmpty(hdnPatientChargesHdID.Value) && !String.IsNullOrEmpty(hdnPatientChargesDtID.Value))
                    {
                        if (!chkIsLinkedRegExclude.Checked)
                        {
                            #region CALCULATE ALL

                            string filterExpressionHd = string.Format("TransactionID IN ({0})", hdnPatientChargesHdID.Value);
                            List<PatientChargesHd> lstChargesHD = BusinessLayer.GetPatientChargesHdList(filterExpressionHd, ctx);

                            string filterExpressionDt = string.Format("ID IN ({0})", hdnPatientChargesDtID.Value);
                            List<PatientChargesDt> lstChargesDT = BusinessLayer.GetPatientChargesDtList(filterExpressionDt, ctx);

                            if (lstChargesHD.Count > 0)
                            {
                                foreach (PatientChargesHd hd in lstChargesHD)
                                {
                                    PatientChargesClassCoverageHd entityHdCheck = entityPatientChargesClassCoverageHdDao.Get(hd.TransactionID);

                                    if (entityHdCheck != null)
                                    {
                                        #region UPDATE HD

                                        entityHdCheck.TransactionID = hd.TransactionID;
                                        entityHdCheck.VisitID = hd.VisitID;
                                        entityHdCheck.HealthcareServiceUnitID = hd.HealthcareServiceUnitID;
                                        entityHdCheck.TransactionCode = hd.TransactionCode;
                                        entityHdCheck.TransactionNo = hd.TransactionNo;
                                        entityHdCheck.TransactionDate = hd.TransactionDate;
                                        entityHdCheck.TransactionTime = hd.TransactionTime;
                                        entityHdCheck.PatientBillingID = hd.PatientBillingID;
                                        entityHdCheck.TestOrderID = hd.TestOrderID;
                                        entityHdCheck.ServiceOrderID = hd.ServiceOrderID;
                                        entityHdCheck.PrescriptionOrderID = hd.PrescriptionOrderID;
                                        entityHdCheck.PrescriptionReturnOrderID = hd.PrescriptionReturnOrderID;
                                        entityHdCheck.ReferenceNo = hd.ReferenceNo;
                                        entityHdCheck.GCTransactionStatus = hd.GCTransactionStatus;
                                        entityHdCheck.GCVoidReason = hd.GCVoidReason;
                                        entityHdCheck.VoidReason = hd.VoidReason;
                                        entityHdCheck.VoidBy = hd.VoidBy;
                                        entityHdCheck.VoidDate = hd.VoidDate;
                                        entityHdCheck.IsAutoTransaction = hd.IsAutoTransaction;
                                        entityHdCheck.IsVerified = hd.IsVerified;
                                        entityHdCheck.IsChargesTransfered = hd.IsChargesTransfered;
                                        entityHdCheck.Remarks = hd.Remarks;

                                        entityHdCheck.LinkedChargesID = hd.LinkedChargesID;
                                        entityHdCheck.IsPendingRecalculated = hd.IsPendingRecalculated;
                                        entityHdCheck.IsEntryByPhysician = hd.IsEntryByPhysician;
                                        entityHdCheck.IsCorrectionTransaction = hd.IsCorrectionTransaction;
                                        entityHdCheck.LastRecalculatedBy = hd.LastRecalculatedBy;

                                        if (hd.LastRecalculatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                        {
                                            entityHdCheck.LastRecalculatedDate = hd.LastRecalculatedDate;
                                        }

                                        entityHdCheck.GCRecalculateReason = hd.GCRecalculateReason;
                                        entityHdCheck.RecalculateReason = hd.RecalculateReason;

                                        entityHdCheck.CreatedBy = hd.CreatedBy;
                                        entityHdCheck.CreatedDate = hd.CreatedDate;

                                        if (hd.LastUpdatedBy != null)
                                        {
                                            entityHdCheck.LastUpdatedBy = hd.LastUpdatedBy;
                                        }

                                        if (hd.LastUpdatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                        {
                                            entityHdCheck.LastUpdatedDate = hd.LastUpdatedDate;
                                        }

                                        entityHdCheck.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                        entityHdCheck.CalculateCreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageHdDao.Update(entityHdCheck);

                                        #endregion
                                    }
                                    else
                                    {
                                        #region INSERT HD

                                        PatientChargesClassCoverageHd entityHd = new PatientChargesClassCoverageHd();
                                        entityHd.TransactionID = hd.TransactionID;
                                        entityHd.VisitID = hd.VisitID;
                                        entityHd.HealthcareServiceUnitID = hd.HealthcareServiceUnitID;
                                        entityHd.TransactionCode = hd.TransactionCode;
                                        entityHd.TransactionNo = hd.TransactionNo;
                                        entityHd.TransactionDate = hd.TransactionDate;
                                        entityHd.TransactionTime = hd.TransactionTime;
                                        entityHd.PatientBillingID = hd.PatientBillingID;
                                        entityHd.TestOrderID = hd.TestOrderID;
                                        entityHd.ServiceOrderID = hd.ServiceOrderID;
                                        entityHd.PrescriptionOrderID = hd.PrescriptionOrderID;
                                        entityHd.PrescriptionReturnOrderID = hd.PrescriptionReturnOrderID;
                                        entityHd.ReferenceNo = hd.ReferenceNo;
                                        entityHd.GCTransactionStatus = hd.GCTransactionStatus;
                                        entityHd.GCVoidReason = hd.GCVoidReason;
                                        entityHd.VoidReason = hd.VoidReason;
                                        entityHd.VoidBy = hd.VoidBy;
                                        entityHd.VoidDate = hd.VoidDate;
                                        entityHd.IsAutoTransaction = hd.IsAutoTransaction;
                                        entityHd.IsVerified = hd.IsVerified;
                                        entityHd.IsChargesTransfered = hd.IsChargesTransfered;
                                        entityHd.Remarks = hd.Remarks;

                                        entityHd.LinkedChargesID = hd.LinkedChargesID;
                                        entityHd.IsPendingRecalculated = hd.IsPendingRecalculated;
                                        entityHd.IsEntryByPhysician = hd.IsEntryByPhysician;
                                        entityHd.IsCorrectionTransaction = hd.IsCorrectionTransaction;
                                        entityHd.LastRecalculatedBy = hd.LastRecalculatedBy;

                                        if (hd.LastRecalculatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                        {
                                            entityHd.LastRecalculatedDate = hd.LastRecalculatedDate;
                                        }

                                        entityHd.GCRecalculateReason = hd.GCRecalculateReason;
                                        entityHd.RecalculateReason = hd.RecalculateReason;

                                        entityHd.CreatedBy = hd.CreatedBy;
                                        entityHd.CreatedDate = hd.CreatedDate;

                                        if (hd.LastUpdatedBy != null)
                                        {
                                            entityHd.LastUpdatedBy = hd.LastUpdatedBy;
                                        }

                                        if (hd.LastUpdatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                        {
                                            entityHd.LastUpdatedDate = hd.LastUpdatedDate;
                                        }

                                        entityHd.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                        entityHd.CalculateCreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageHdDao.Insert(entityHd);

                                        #endregion
                                    }

                                    List<PatientChargesDt> lstChargesDtForInsert = lstChargesDT.Where(t => t.TransactionID == hd.TransactionID).ToList();
                                    foreach (PatientChargesDt dt in lstChargesDtForInsert)
                                    {
                                        ItemMaster im = itemDao.Get(dt.ItemID);

                                        PatientChargesClassCoverageDt entityDtCheck = entityPatientChargesClassCoverageDtDao.Get(dt.ID);

                                        if (entityDtCheck != null)
                                        {
                                            #region UPDATE DT

                                            entityDtCheck.TransactionID = dt.TransactionID;
                                            entityDtCheck.ItemID = dt.ItemID;
                                            entityDtCheck.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);

                                            if (im.GCItemType == Constant.ItemType.PELAYANAN || im.GCItemType == Constant.ItemType.LABORATORIUM || im.GCItemType == Constant.ItemType.RADIOLOGI || im.GCItemType == Constant.ItemType.PENUNJANG_MEDIS || im.GCItemType == Constant.ItemType.MEDICAL_CHECKUP)
                                            {
                                                #region PELAYANAN, LABORATORIUM, RADIOLOGI, PENUNJANG_MEDIS DAN MEDICAL_CHECKUP
                                                List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", entityDtCheck.ItemID), ctx);

                                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDtCheck.ChargeClassID, entityDtCheck.ItemID, 1, DateTime.Now, ctx);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == entityDtCheck.ItemID);

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

                                                entityDtCheck.BaseTariff = basePrice;
                                                entityDtCheck.Tariff = price;
                                                entityDtCheck.BaseComp1 = basePriceComp1;
                                                entityDtCheck.BaseComp2 = basePriceComp2;
                                                entityDtCheck.BaseComp3 = basePriceComp3;
                                                entityDtCheck.TariffComp1 = priceComp1;
                                                entityDtCheck.TariffComp2 = priceComp2;
                                                entityDtCheck.TariffComp3 = priceComp3;
                                                entityDtCheck.CostAmount = costAmount;
                                                entityDtCheck.IsCITOInPercentage = entity.IsCITOInPercentage;
                                                entityDtCheck.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                                entityDtCheck.BaseCITOAmount = entity.CITOAmount;
                                                entityDtCheck.BaseComplicationAmount = entity.ComplicationAmount;
                                                entityDtCheck.GCBaseUnit = entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                                entityDtCheck.IsSubContractItem = entity.IsSubContractItem;

                                                entityDtCheck.ParamedicID = dt.ParamedicID;

                                                entityDtCheck.IsVariable = dt.IsVariable;
                                                entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;

                                                decimal qty = dt.ChargedQuantity;
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
                                                                entityDtCheck.DiscountPercentageComp1 = discountAmountComp1;
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
                                                                entityDtCheck.DiscountPercentageComp2 = discountAmountComp2;
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
                                                                entityDtCheck.DiscountPercentageComp3 = discountAmountComp3;
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
                                                            entityDtCheck.DiscountPercentageComp1 = discountAmount;
                                                        }

                                                        if (priceComp2 > 0)
                                                        {
                                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                            entityDtCheck.DiscountPercentageComp2 = discountAmount;
                                                        }

                                                        if (priceComp3 > 0)
                                                        {
                                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                            entityDtCheck.DiscountPercentageComp3 = discountAmount;
                                                        }
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp1 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp1 = true;
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp2 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp2 = true;
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp3 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp3 = true;
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

                                                if (grossLineAmount >= 0)
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

                                                entityDtCheck.IsCITO = dt.IsCITO;
                                                entityDtCheck.CITOAmount = dt.CITOAmount;
                                                entityDtCheck.IsComplication = dt.IsComplication;
                                                entityDtCheck.ComplicationAmount = dt.ComplicationAmount;

                                                entityDtCheck.IsDiscount = totalDiscountAmount != 0;
                                                entityDtCheck.DiscountAmount = totalDiscountAmount;
                                                entityDtCheck.DiscountComp1 = totalDiscountAmount1;
                                                entityDtCheck.DiscountComp2 = totalDiscountAmount2;
                                                entityDtCheck.DiscountComp3 = totalDiscountAmount3;

                                                entityDtCheck.UsedQuantity = entityDtCheck.BaseQuantity = entityDtCheck.ChargedQuantity = qty;
                                                entityDtCheck.PatientAmount = total - totalPayer;
                                                entityDtCheck.PayerAmount = totalPayer;
                                                entityDtCheck.LineAmount = total;

                                                entityDtCheck.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityDtCheck.ParamedicID, entityDtCheck.ChargeClassID, Constant.ParamedicRole.PELAKSANA, hd.VisitID, hd.HealthcareServiceUnitID, hd.TransactionDate, hd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                if (entityDtCheck.RevenueSharingID == 0)
                                                    entityDtCheck.RevenueSharingID = null;

                                                entityDtCheck.CreatedBy = dt.CreatedBy;
                                                entityDtCheck.CreatedDate = dt.CreatedDate;
                                                entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                                entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                                #endregion
                                            }
                                            else if (im.GCItemType == Constant.ItemType.OBAT_OBATAN || im.GCItemType == Constant.ItemType.BARANG_MEDIS)
                                            {
                                                #region OBAT_OBATAN DAN BARANG_MEDIS
                                                decimal qty = dt.ChargedQuantity;

                                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDtCheck.ChargeClassID, entityDtCheck.ItemID, 2, DateTime.Now, ctx);

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

                                                entityDtCheck.BaseTariff = basePrice;
                                                entityDtCheck.Tariff = price;
                                                entityDtCheck.BaseComp1 = basePriceComp1;
                                                entityDtCheck.BaseComp2 = basePriceComp2;
                                                entityDtCheck.BaseComp3 = basePriceComp3;
                                                entityDtCheck.TariffComp1 = priceComp1;
                                                entityDtCheck.TariffComp2 = priceComp2;
                                                entityDtCheck.TariffComp3 = priceComp3;
                                                entityDtCheck.CostAmount = costAmount;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                entityDtCheck.GCBaseUnit = dt.GCBaseUnit;
                                                entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                                entityDtCheck.ParamedicID = dt.ParamedicID;

                                                entityDtCheck.IsVariable = dt.IsVariable;
                                                entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                                entityDtCheck.DiscountPercentageComp1 = discountAmountComp1;
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
                                                                entityDtCheck.DiscountPercentageComp2 = discountAmountComp2;
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
                                                                entityDtCheck.DiscountPercentageComp3 = discountAmountComp3;
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
                                                            entityDtCheck.DiscountPercentageComp1 = discountAmount;
                                                        }

                                                        if (priceComp2 > 0)
                                                        {
                                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                            entityDtCheck.DiscountPercentageComp2 = discountAmount;
                                                        }

                                                        if (priceComp3 > 0)
                                                        {
                                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                            entityDtCheck.DiscountPercentageComp3 = discountAmount;
                                                        }
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp1 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp1 = true;
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp2 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp2 = true;
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp3 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp3 = true;
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

                                                if (grossLineAmount >= 0)
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

                                                entityDtCheck.ConversionFactor = dt.ConversionFactor;

                                                ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDtCheck.ItemID), ctx).FirstOrDefault();
                                                entityDtCheck.AveragePrice = iPlanning.AveragePrice;
                                                entityDtCheck.CostAmount = iPlanning.UnitPrice;

                                                entityDtCheck.IsCITO = dt.IsCITO;
                                                entityDtCheck.CITOAmount = dt.CITOAmount;
                                                entityDtCheck.IsComplication = dt.IsComplication;
                                                entityDtCheck.ComplicationAmount = dt.ComplicationAmount;

                                                entityDtCheck.IsDiscount = totalDiscountAmount != 0;
                                                entityDtCheck.DiscountAmount = totalDiscountAmount;
                                                entityDtCheck.DiscountComp2 = totalDiscountAmount2;
                                                entityDtCheck.DiscountComp3 = totalDiscountAmount3;

                                                entityDtCheck.UsedQuantity = entityDtCheck.BaseQuantity = entityDtCheck.ChargedQuantity = qty;
                                                entityDtCheck.PatientAmount = total - totalPayer;
                                                entityDtCheck.PayerAmount = totalPayer;
                                                entityDtCheck.LineAmount = total;

                                                if (dt.PrescriptionOrderDetailID != 0)
                                                {
                                                    entityDtCheck.PrescriptionOrderDetailID = dt.PrescriptionOrderDetailID;
                                                }

                                                entityDtCheck.LocationID = dt.LocationID;
                                                entityDtCheck.IsApproved = dt.IsApproved;
                                                entityDtCheck.CreatedBy = dt.CreatedBy;
                                                entityDtCheck.CreatedDate = dt.CreatedDate;
                                                entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                                entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                                #endregion
                                            }
                                            else
                                            {
                                                #region BARANG_UMUM
                                                decimal qty = dt.ChargedQuantity;

                                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDtCheck.ChargeClassID, entityDtCheck.ItemID, 3, DateTime.Now, ctx);

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

                                                entityDtCheck.BaseTariff = basePrice;
                                                entityDtCheck.Tariff = price;
                                                entityDtCheck.BaseComp1 = basePriceComp1;
                                                entityDtCheck.BaseComp2 = basePriceComp2;
                                                entityDtCheck.BaseComp3 = basePriceComp3;
                                                entityDtCheck.TariffComp1 = priceComp1;
                                                entityDtCheck.TariffComp2 = priceComp2;
                                                entityDtCheck.TariffComp3 = priceComp3;
                                                entityDtCheck.CostAmount = costAmount;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                entityDtCheck.GCBaseUnit = dt.GCBaseUnit;
                                                entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                                entityDtCheck.ParamedicID = dt.ParamedicID;

                                                entityDtCheck.IsVariable = dt.IsVariable;
                                                entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                                entityDtCheck.DiscountPercentageComp1 = discountAmountComp1;
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
                                                                entityDtCheck.DiscountPercentageComp2 = discountAmountComp2;
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
                                                                entityDtCheck.DiscountPercentageComp3 = discountAmountComp3;
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
                                                            entityDtCheck.DiscountPercentageComp1 = discountAmount;
                                                        }

                                                        if (priceComp2 > 0)
                                                        {
                                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                            entityDtCheck.DiscountPercentageComp2 = discountAmount;
                                                        }

                                                        if (priceComp3 > 0)
                                                        {
                                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                            entityDtCheck.DiscountPercentageComp3 = discountAmount;
                                                        }
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp1 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp1 = true;
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp2 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp2 = true;
                                                    }

                                                    if (entityDtCheck.DiscountPercentageComp3 > 0)
                                                    {
                                                        entityDtCheck.IsDiscountInPercentageComp3 = true;
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

                                                if (grossLineAmount >= 0)
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

                                                entityDtCheck.ConversionFactor = dt.ConversionFactor;

                                                ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDtCheck.ItemID), ctx).FirstOrDefault();
                                                entityDtCheck.AveragePrice = iPlanning.AveragePrice;
                                                entityDtCheck.CostAmount = iPlanning.UnitPrice;

                                                entityDtCheck.IsCITO = dt.IsCITO;
                                                entityDtCheck.CITOAmount = dt.CITOAmount;
                                                entityDtCheck.IsComplication = dt.IsComplication;
                                                entityDtCheck.ComplicationAmount = dt.ComplicationAmount;

                                                entityDtCheck.IsDiscount = totalDiscountAmount != 0;
                                                entityDtCheck.DiscountAmount = totalDiscountAmount;
                                                entityDtCheck.DiscountComp2 = totalDiscountAmount2;
                                                entityDtCheck.DiscountComp3 = totalDiscountAmount3;

                                                entityDtCheck.UsedQuantity = entityDtCheck.BaseQuantity = entityDtCheck.ChargedQuantity = qty;
                                                entityDtCheck.PatientAmount = total - totalPayer;
                                                entityDtCheck.PayerAmount = totalPayer;
                                                entityDtCheck.LineAmount = total;
                                                entityDtCheck.LocationID = dt.LocationID;
                                                entityDtCheck.IsApproved = dt.IsApproved;
                                                entityDtCheck.CreatedBy = dt.CreatedBy;
                                                entityDtCheck.CreatedDate = dt.CreatedDate;
                                                entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                                entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                                #endregion
                                            }

                                            entityDtCheck.GCTransactionDetailStatus = dt.GCTransactionDetailStatus;
                                            entityDtCheck.TransactionID = dt.TransactionID;
                                            entityDtCheck.ID = dt.ID;
                                            entityDtCheck.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                            entityDtCheck.CalculateCreatedDate = DateTime.Now;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPatientChargesClassCoverageDtDao.Update(entityDtCheck);

                                            #endregion
                                        }
                                        else
                                        {
                                            #region INSERT DT

                                            PatientChargesClassCoverageDt entityDt = new PatientChargesClassCoverageDt();
                                            entityDt.TransactionID = dt.TransactionID;
                                            entityDt.ItemID = dt.ItemID;
                                            entityDt.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);

                                            if (im.GCItemType == Constant.ItemType.PELAYANAN || im.GCItemType == Constant.ItemType.LABORATORIUM || im.GCItemType == Constant.ItemType.RADIOLOGI || im.GCItemType == Constant.ItemType.PENUNJANG_MEDIS || im.GCItemType == Constant.ItemType.MEDICAL_CHECKUP)
                                            {
                                                #region PELAYANAN, LABORATORIUM, RADIOLOGI, PENUNJANG_MEDIS DAN MEDICAL_CHECKUP
                                                List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", entityDt.ItemID), ctx);

                                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDt.ChargeClassID, entityDt.ItemID, 1, DateTime.Now, ctx);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == entityDt.ItemID);

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

                                                entityDt.BaseTariff = basePrice;
                                                entityDt.Tariff = price;
                                                entityDt.BaseComp1 = basePriceComp1;
                                                entityDt.BaseComp2 = basePriceComp2;
                                                entityDt.BaseComp3 = basePriceComp3;
                                                entityDt.TariffComp1 = priceComp1;
                                                entityDt.TariffComp2 = priceComp2;
                                                entityDt.TariffComp3 = priceComp3;
                                                entityDt.CostAmount = costAmount;
                                                entityDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                                                entityDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                                entityDt.BaseCITOAmount = entity.CITOAmount;
                                                entityDt.BaseComplicationAmount = entity.ComplicationAmount;

                                                entityDt.GCBaseUnit = dt.GCBaseUnit;
                                                entityDt.GCItemUnit = dt.GCItemUnit;
                                                entityDt.IsSubContractItem = entity.IsSubContractItem;

                                                entityDt.ParamedicID = dt.ParamedicID;

                                                entityDt.IsVariable = dt.IsVariable;
                                                entityDt.IsUnbilledItem = dt.IsUnbilledItem;

                                                decimal qty = dt.ChargedQuantity;
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
                                                                entityDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                                entityDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                                entityDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                            entityDt.DiscountPercentageComp1 = discountAmount;
                                                        }

                                                        if (priceComp2 > 0)
                                                        {
                                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                            entityDt.DiscountPercentageComp2 = discountAmount;
                                                        }

                                                        if (priceComp3 > 0)
                                                        {
                                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                            entityDt.DiscountPercentageComp3 = discountAmount;
                                                        }
                                                    }

                                                    if (entityDt.DiscountPercentageComp1 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp1 = true;
                                                    }

                                                    if (entityDt.DiscountPercentageComp2 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp2 = true;
                                                    }

                                                    if (entityDt.DiscountPercentageComp3 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp3 = true;
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

                                                if (grossLineAmount >= 0)
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

                                                entityDt.IsCITO = dt.IsCITO;
                                                entityDt.CITOAmount = dt.CITOAmount;
                                                entityDt.IsComplication = dt.IsComplication;
                                                entityDt.ComplicationAmount = dt.ComplicationAmount;

                                                entityDt.IsDiscount = totalDiscountAmount != 0;
                                                entityDt.DiscountAmount = totalDiscountAmount;
                                                entityDt.DiscountComp1 = totalDiscountAmount1;
                                                entityDt.DiscountComp2 = totalDiscountAmount2;
                                                entityDt.DiscountComp3 = totalDiscountAmount3;

                                                entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;
                                                entityDt.PatientAmount = total - totalPayer;
                                                entityDt.PayerAmount = totalPayer;
                                                entityDt.LineAmount = total;

                                                entityDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityDt.ParamedicID, entityDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, hd.VisitID, hd.HealthcareServiceUnitID, hd.TransactionDate, hd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                if (entityDt.RevenueSharingID == 0)
                                                    entityDt.RevenueSharingID = null;

                                                entityDt.CreatedBy = dt.CreatedBy;
                                                entityDt.CreatedDate = dt.CreatedDate;

                                                entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                                entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                                #endregion
                                            }
                                            else if (im.GCItemType == Constant.ItemType.OBAT_OBATAN || im.GCItemType == Constant.ItemType.BARANG_MEDIS)
                                            {
                                                #region OBAT_OBATAN DAN BARANG_MEDIS
                                                decimal qty = dt.ChargedQuantity;

                                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDt.ChargeClassID, entityDt.ItemID, 2, DateTime.Now, ctx);

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

                                                entityDt.BaseTariff = basePrice;
                                                entityDt.Tariff = price;
                                                entityDt.BaseComp1 = basePriceComp1;
                                                entityDt.BaseComp2 = basePriceComp2;
                                                entityDt.BaseComp3 = basePriceComp3;
                                                entityDt.TariffComp1 = priceComp1;
                                                entityDt.TariffComp2 = priceComp2;
                                                entityDt.TariffComp3 = priceComp3;
                                                entityDt.CostAmount = costAmount;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                entityDt.GCBaseUnit = dt.GCBaseUnit;
                                                entityDt.GCItemUnit = dt.GCItemUnit;
                                                entityDt.ParamedicID = dt.ParamedicID;

                                                entityDt.IsVariable = dt.IsVariable;
                                                entityDt.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                                entityDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                                entityDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                                entityDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                            entityDt.DiscountPercentageComp1 = discountAmount;
                                                        }

                                                        if (priceComp2 > 0)
                                                        {
                                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                            entityDt.DiscountPercentageComp2 = discountAmount;
                                                        }

                                                        if (priceComp3 > 0)
                                                        {
                                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                            entityDt.DiscountPercentageComp3 = discountAmount;
                                                        }
                                                    }

                                                    if (entityDt.DiscountPercentageComp1 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp1 = true;
                                                    }

                                                    if (entityDt.DiscountPercentageComp2 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp2 = true;
                                                    }

                                                    if (entityDt.DiscountPercentageComp3 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp3 = true;
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

                                                if (grossLineAmount >= 0)
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

                                                entityDt.ConversionFactor = dt.ConversionFactor;

                                                ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault();
                                                entityDt.AveragePrice = iPlanning.AveragePrice;
                                                entityDt.CostAmount = iPlanning.UnitPrice;

                                                entityDt.IsCITO = dt.IsCITO;
                                                entityDt.CITOAmount = dt.CITOAmount;
                                                entityDt.IsComplication = dt.IsComplication;
                                                entityDt.ComplicationAmount = dt.ComplicationAmount;

                                                //entityDt.IsDiscount = dt.IsDiscount;
                                                //entityDt.DiscountAmount = totalDiscountAmount;

                                                entityDt.IsDiscount = totalDiscountAmount != 0;
                                                entityDt.DiscountAmount = totalDiscountAmount;
                                                entityDt.DiscountComp1 = totalDiscountAmount1;
                                                entityDt.DiscountComp2 = totalDiscountAmount2;
                                                entityDt.DiscountComp3 = totalDiscountAmount3;

                                                entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;
                                                entityDt.PatientAmount = total - totalPayer;
                                                entityDt.PayerAmount = totalPayer;
                                                entityDt.LineAmount = total;

                                                if (dt.PrescriptionOrderDetailID != 0)
                                                {
                                                    entityDt.PrescriptionOrderDetailID = dt.PrescriptionOrderDetailID;
                                                }

                                                entityDt.LocationID = dt.LocationID;
                                                entityDt.IsApproved = dt.IsApproved;
                                                entityDt.CreatedBy = dt.CreatedBy;
                                                entityDt.CreatedDate = dt.CreatedDate;
                                                entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                                entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                                #endregion
                                            }
                                            else
                                            {
                                                #region BARANG_UMUM
                                                decimal qty = dt.ChargedQuantity;

                                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDt.ChargeClassID, entityDt.ItemID, 3, DateTime.Now, ctx);

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

                                                entityDt.BaseTariff = basePrice;
                                                entityDt.Tariff = price;
                                                entityDt.BaseComp1 = basePriceComp1;
                                                entityDt.BaseComp2 = basePriceComp2;
                                                entityDt.BaseComp3 = basePriceComp3;
                                                entityDt.TariffComp1 = priceComp1;
                                                entityDt.TariffComp2 = priceComp2;
                                                entityDt.TariffComp3 = priceComp3;
                                                entityDt.CostAmount = costAmount;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                entityDt.GCBaseUnit = dt.GCBaseUnit;
                                                entityDt.GCItemUnit = dt.GCItemUnit;
                                                entityDt.ParamedicID = dt.ParamedicID;

                                                entityDt.IsVariable = dt.IsVariable;
                                                entityDt.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                                entityDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                                entityDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                                entityDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                            entityDt.DiscountPercentageComp1 = discountAmount;
                                                        }

                                                        if (priceComp2 > 0)
                                                        {
                                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                            entityDt.DiscountPercentageComp2 = discountAmount;
                                                        }

                                                        if (priceComp3 > 0)
                                                        {
                                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                            entityDt.DiscountPercentageComp3 = discountAmount;
                                                        }
                                                    }

                                                    if (entityDt.DiscountPercentageComp1 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp1 = true;
                                                    }

                                                    if (entityDt.DiscountPercentageComp2 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp2 = true;
                                                    }

                                                    if (entityDt.DiscountPercentageComp3 > 0)
                                                    {
                                                        entityDt.IsDiscountInPercentageComp3 = true;
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

                                                if (grossLineAmount >= 0)
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

                                                entityDt.ConversionFactor = dt.ConversionFactor;

                                                ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault();
                                                entityDt.AveragePrice = iPlanning.AveragePrice;
                                                entityDt.CostAmount = iPlanning.UnitPrice;

                                                entityDt.IsCITO = dt.IsCITO;
                                                entityDt.CITOAmount = dt.CITOAmount;
                                                entityDt.IsComplication = dt.IsComplication;
                                                entityDt.ComplicationAmount = dt.ComplicationAmount;

                                                //entityDt.IsDiscount = dt.IsDiscount;
                                                //entityDt.DiscountAmount = totalDiscountAmount;

                                                entityDt.IsDiscount = totalDiscountAmount != 0;
                                                entityDt.DiscountAmount = totalDiscountAmount;
                                                entityDt.DiscountComp1 = totalDiscountAmount1;
                                                entityDt.DiscountComp2 = totalDiscountAmount2;
                                                entityDt.DiscountComp3 = totalDiscountAmount3;

                                                entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;
                                                entityDt.PatientAmount = total - totalPayer;
                                                entityDt.PayerAmount = totalPayer;
                                                entityDt.LineAmount = total;
                                                entityDt.LocationID = dt.LocationID;
                                                entityDt.IsApproved = dt.IsApproved;
                                                entityDt.CreatedBy = dt.CreatedBy;
                                                entityDt.CreatedDate = dt.CreatedDate;
                                                entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                                entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);
                                                #endregion
                                            }

                                            entityDt.GCTransactionDetailStatus = dt.GCTransactionDetailStatus;
                                            entityDt.TransactionID = hd.TransactionID;
                                            entityDt.ID = dt.ID;
                                            entityDt.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                            entityDt.CalculateCreatedDate = DateTime.Now;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPatientChargesClassCoverageDtDao.Insert(entityDt);

                                            #endregion
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region CALCULATE WITHOUT LINKEDREG

                            #region MainReg

                            string filterExpressionHd = string.Format("TransactionID IN ({0}) AND TransactionID NOT IN ({1})", hdnPatientChargesHdID.Value, hdnPatientChargesHdIDLinked.Value);
                            List<PatientChargesHd> lstChargesHD = BusinessLayer.GetPatientChargesHdList(filterExpressionHd, ctx);
                            foreach (PatientChargesHd hd in lstChargesHD)
                            {
                                PatientChargesClassCoverageHd entityHdCheck = entityPatientChargesClassCoverageHdDao.Get(hd.TransactionID);

                                if (entityHdCheck != null)
                                {
                                    #region UPDATE HD

                                    entityHdCheck.TransactionID = hd.TransactionID;
                                    entityHdCheck.VisitID = hd.VisitID;
                                    entityHdCheck.HealthcareServiceUnitID = hd.HealthcareServiceUnitID;
                                    entityHdCheck.TransactionCode = hd.TransactionCode;
                                    entityHdCheck.TransactionNo = hd.TransactionNo;
                                    entityHdCheck.TransactionDate = hd.TransactionDate;
                                    entityHdCheck.TransactionTime = hd.TransactionTime;
                                    entityHdCheck.PatientBillingID = hd.PatientBillingID;
                                    entityHdCheck.TestOrderID = hd.TestOrderID;
                                    entityHdCheck.ServiceOrderID = hd.ServiceOrderID;
                                    entityHdCheck.PrescriptionOrderID = hd.PrescriptionOrderID;
                                    entityHdCheck.PrescriptionReturnOrderID = hd.PrescriptionReturnOrderID;
                                    entityHdCheck.ReferenceNo = hd.ReferenceNo;
                                    entityHdCheck.GCTransactionStatus = hd.GCTransactionStatus;
                                    entityHdCheck.GCVoidReason = hd.GCVoidReason;
                                    entityHdCheck.VoidReason = hd.VoidReason;
                                    entityHdCheck.VoidBy = hd.VoidBy;
                                    entityHdCheck.VoidDate = hd.VoidDate;
                                    entityHdCheck.IsAutoTransaction = hd.IsAutoTransaction;
                                    entityHdCheck.IsVerified = hd.IsVerified;
                                    entityHdCheck.IsChargesTransfered = hd.IsChargesTransfered;
                                    entityHdCheck.Remarks = hd.Remarks;

                                    entityHdCheck.LinkedChargesID = hd.LinkedChargesID;
                                    entityHdCheck.IsPendingRecalculated = hd.IsPendingRecalculated;
                                    entityHdCheck.IsEntryByPhysician = hd.IsEntryByPhysician;
                                    entityHdCheck.IsCorrectionTransaction = hd.IsCorrectionTransaction;
                                    entityHdCheck.LastRecalculatedBy = hd.LastRecalculatedBy;

                                    if (hd.LastRecalculatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHdCheck.LastRecalculatedDate = hd.LastRecalculatedDate;
                                    }

                                    entityHdCheck.GCRecalculateReason = hd.GCRecalculateReason;
                                    entityHdCheck.RecalculateReason = hd.RecalculateReason;

                                    entityHdCheck.CreatedBy = hd.CreatedBy;
                                    entityHdCheck.CreatedDate = hd.CreatedDate;

                                    if (hd.LastUpdatedBy != null)
                                    {
                                        entityHdCheck.LastUpdatedBy = hd.LastUpdatedBy;
                                    }

                                    if (hd.LastUpdatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHdCheck.LastUpdatedDate = hd.LastUpdatedDate;
                                    }

                                    entityHdCheck.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                    entityHdCheck.CalculateCreatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityPatientChargesClassCoverageHdDao.Update(entityHdCheck);

                                    #endregion
                                }
                                else
                                {
                                    #region INSERT HD

                                    PatientChargesClassCoverageHd entityHd = new PatientChargesClassCoverageHd();
                                    entityHd.TransactionID = hd.TransactionID;
                                    entityHd.VisitID = hd.VisitID;
                                    entityHd.HealthcareServiceUnitID = hd.HealthcareServiceUnitID;
                                    entityHd.TransactionCode = hd.TransactionCode;
                                    entityHd.TransactionNo = hd.TransactionNo;
                                    entityHd.TransactionDate = hd.TransactionDate;
                                    entityHd.TransactionTime = hd.TransactionTime;
                                    entityHd.PatientBillingID = hd.PatientBillingID;
                                    entityHd.TestOrderID = hd.TestOrderID;
                                    entityHd.ServiceOrderID = hd.ServiceOrderID;
                                    entityHd.PrescriptionOrderID = hd.PrescriptionOrderID;
                                    entityHd.PrescriptionReturnOrderID = hd.PrescriptionReturnOrderID;
                                    entityHd.ReferenceNo = hd.ReferenceNo;
                                    entityHd.GCTransactionStatus = hd.GCTransactionStatus;
                                    entityHd.GCVoidReason = hd.GCVoidReason;
                                    entityHd.VoidReason = hd.VoidReason;
                                    entityHd.VoidBy = hd.VoidBy;
                                    entityHd.VoidDate = hd.VoidDate;
                                    entityHd.IsAutoTransaction = hd.IsAutoTransaction;
                                    entityHd.IsVerified = hd.IsVerified;
                                    entityHd.IsChargesTransfered = hd.IsChargesTransfered;
                                    entityHd.Remarks = hd.Remarks;

                                    entityHd.LinkedChargesID = hd.LinkedChargesID;
                                    entityHd.IsPendingRecalculated = hd.IsPendingRecalculated;
                                    entityHd.IsEntryByPhysician = hd.IsEntryByPhysician;
                                    entityHd.IsCorrectionTransaction = hd.IsCorrectionTransaction;
                                    entityHd.LastRecalculatedBy = hd.LastRecalculatedBy;

                                    if (hd.LastRecalculatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHd.LastRecalculatedDate = hd.LastRecalculatedDate;
                                    }

                                    entityHd.GCRecalculateReason = hd.GCRecalculateReason;
                                    entityHd.RecalculateReason = hd.RecalculateReason;

                                    entityHd.CreatedBy = hd.CreatedBy;
                                    entityHd.CreatedDate = hd.CreatedDate;

                                    if (hd.LastUpdatedBy != null)
                                    {
                                        entityHd.LastUpdatedBy = hd.LastUpdatedBy;
                                    }

                                    if (hd.LastUpdatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHd.LastUpdatedDate = hd.LastUpdatedDate;
                                    }

                                    entityHd.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                    entityHd.CalculateCreatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityPatientChargesClassCoverageHdDao.Insert(entityHd);

                                    #endregion
                                }

                                string filterExpressionDt = string.Format("TransactionID IN ({0})", hd.TransactionID);
                                List<PatientChargesDt> lstChargesDtForInsert = BusinessLayer.GetPatientChargesDtList(filterExpressionDt, ctx);
                                foreach (PatientChargesDt dt in lstChargesDtForInsert)
                                {
                                    ItemMaster im = itemDao.Get(dt.ItemID);

                                    PatientChargesClassCoverageDt entityDtCheck = entityPatientChargesClassCoverageDtDao.Get(dt.ID);

                                    if (entityDtCheck != null)
                                    {
                                        #region UPDATE DT

                                        entityDtCheck.TransactionID = dt.TransactionID;
                                        entityDtCheck.ItemID = dt.ItemID;
                                        entityDtCheck.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);

                                        if (im.GCItemType == Constant.ItemType.PELAYANAN || im.GCItemType == Constant.ItemType.LABORATORIUM || im.GCItemType == Constant.ItemType.RADIOLOGI || im.GCItemType == Constant.ItemType.PENUNJANG_MEDIS || im.GCItemType == Constant.ItemType.MEDICAL_CHECKUP)
                                        {
                                            #region PELAYANAN, LABORATORIUM, RADIOLOGI, PENUNJANG_MEDIS DAN MEDICAL_CHECKUP
                                            List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", entityDtCheck.ItemID), ctx);

                                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDtCheck.ChargeClassID, entityDtCheck.ItemID, 1, DateTime.Now, ctx);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == entityDtCheck.ItemID);

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

                                            entityDtCheck.BaseTariff = basePrice;
                                            entityDtCheck.Tariff = price;
                                            entityDtCheck.BaseComp1 = basePriceComp1;
                                            entityDtCheck.BaseComp2 = basePriceComp2;
                                            entityDtCheck.BaseComp3 = basePriceComp3;
                                            entityDtCheck.TariffComp1 = priceComp1;
                                            entityDtCheck.TariffComp2 = priceComp2;
                                            entityDtCheck.TariffComp3 = priceComp3;
                                            entityDtCheck.CostAmount = costAmount;
                                            entityDtCheck.IsCITOInPercentage = entity.IsCITOInPercentage;
                                            entityDtCheck.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                            entityDtCheck.BaseCITOAmount = entity.CITOAmount;
                                            entityDtCheck.BaseComplicationAmount = entity.ComplicationAmount;
                                            entityDtCheck.GCBaseUnit = entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                            entityDtCheck.IsSubContractItem = entity.IsSubContractItem;

                                            entityDtCheck.ParamedicID = dt.ParamedicID;

                                            entityDtCheck.IsVariable = dt.IsVariable;
                                            entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;

                                            decimal qty = dt.ChargedQuantity;
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
                                                            entityDtCheck.DiscountPercentageComp1 = discountAmountComp1;
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
                                                            entityDtCheck.DiscountPercentageComp2 = discountAmountComp2;
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
                                                            entityDtCheck.DiscountPercentageComp3 = discountAmountComp3;
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
                                                        entityDtCheck.DiscountPercentageComp1 = discountAmount;
                                                    }

                                                    if (priceComp2 > 0)
                                                    {
                                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                        entityDtCheck.DiscountPercentageComp2 = discountAmount;
                                                    }

                                                    if (priceComp3 > 0)
                                                    {
                                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                        entityDtCheck.DiscountPercentageComp3 = discountAmount;
                                                    }
                                                }

                                                if (entityDtCheck.DiscountPercentageComp1 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp1 = true;
                                                }

                                                if (entityDtCheck.DiscountPercentageComp2 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp2 = true;
                                                }

                                                if (entityDtCheck.DiscountPercentageComp3 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp3 = true;
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

                                            if (grossLineAmount >= 0)
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

                                            entityDtCheck.IsCITO = dt.IsCITO;
                                            entityDtCheck.CITOAmount = dt.CITOAmount;
                                            entityDtCheck.IsComplication = dt.IsComplication;
                                            entityDtCheck.ComplicationAmount = dt.ComplicationAmount;

                                            entityDtCheck.IsDiscount = totalDiscountAmount != 0;
                                            entityDtCheck.DiscountAmount = totalDiscountAmount;
                                            entityDtCheck.DiscountComp1 = totalDiscountAmount1;
                                            entityDtCheck.DiscountComp2 = totalDiscountAmount2;
                                            entityDtCheck.DiscountComp3 = totalDiscountAmount3;

                                            entityDtCheck.UsedQuantity = entityDtCheck.BaseQuantity = entityDtCheck.ChargedQuantity = qty;
                                            entityDtCheck.PatientAmount = total - totalPayer;
                                            entityDtCheck.PayerAmount = totalPayer;
                                            entityDtCheck.LineAmount = total;

                                            entityDtCheck.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityDtCheck.ParamedicID, entityDtCheck.ChargeClassID, Constant.ParamedicRole.PELAKSANA, hd.VisitID, hd.HealthcareServiceUnitID, hd.TransactionDate, hd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            if (entityDtCheck.RevenueSharingID == 0)
                                                entityDtCheck.RevenueSharingID = null;

                                            entityDtCheck.CreatedBy = dt.CreatedBy;
                                            entityDtCheck.CreatedDate = dt.CreatedDate;
                                            entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                            entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                            #endregion
                                        }
                                        else if (im.GCItemType == Constant.ItemType.OBAT_OBATAN || im.GCItemType == Constant.ItemType.BARANG_MEDIS)
                                        {
                                            #region OBAT_OBATAN DAN BARANG_MEDIS
                                            decimal qty = dt.ChargedQuantity;

                                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDtCheck.ChargeClassID, entityDtCheck.ItemID, 2, DateTime.Now, ctx);

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

                                            entityDtCheck.BaseTariff = basePrice;
                                            entityDtCheck.Tariff = price;
                                            entityDtCheck.BaseComp1 = basePriceComp1;
                                            entityDtCheck.BaseComp2 = basePriceComp2;
                                            entityDtCheck.BaseComp3 = basePriceComp3;
                                            entityDtCheck.TariffComp1 = priceComp1;
                                            entityDtCheck.TariffComp2 = priceComp2;
                                            entityDtCheck.TariffComp3 = priceComp3;
                                            entityDtCheck.CostAmount = costAmount;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            entityDtCheck.GCBaseUnit = dt.GCBaseUnit;
                                            entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                            entityDtCheck.ParamedicID = dt.ParamedicID;

                                            entityDtCheck.IsVariable = dt.IsVariable;
                                            entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                            entityDtCheck.DiscountPercentageComp1 = discountAmountComp1;
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
                                                            entityDtCheck.DiscountPercentageComp2 = discountAmountComp2;
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
                                                            entityDtCheck.DiscountPercentageComp3 = discountAmountComp3;
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
                                                        entityDtCheck.DiscountPercentageComp1 = discountAmount;
                                                    }

                                                    if (priceComp2 > 0)
                                                    {
                                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                        entityDtCheck.DiscountPercentageComp2 = discountAmount;
                                                    }

                                                    if (priceComp3 > 0)
                                                    {
                                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                        entityDtCheck.DiscountPercentageComp3 = discountAmount;
                                                    }
                                                }

                                                if (entityDtCheck.DiscountPercentageComp1 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp1 = true;
                                                }

                                                if (entityDtCheck.DiscountPercentageComp2 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp2 = true;
                                                }

                                                if (entityDtCheck.DiscountPercentageComp3 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp3 = true;
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

                                            if (grossLineAmount >= 0)
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

                                            entityDtCheck.ConversionFactor = dt.ConversionFactor;

                                            ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDtCheck.ItemID), ctx).FirstOrDefault();
                                            entityDtCheck.AveragePrice = iPlanning.AveragePrice;
                                            entityDtCheck.CostAmount = iPlanning.UnitPrice;

                                            entityDtCheck.IsCITO = dt.IsCITO;
                                            entityDtCheck.CITOAmount = dt.CITOAmount;
                                            entityDtCheck.IsComplication = dt.IsComplication;
                                            entityDtCheck.ComplicationAmount = dt.ComplicationAmount;

                                            entityDtCheck.IsDiscount = totalDiscountAmount != 0;
                                            entityDtCheck.DiscountAmount = totalDiscountAmount;
                                            entityDtCheck.DiscountComp2 = totalDiscountAmount2;
                                            entityDtCheck.DiscountComp3 = totalDiscountAmount3;

                                            entityDtCheck.UsedQuantity = entityDtCheck.BaseQuantity = entityDtCheck.ChargedQuantity = qty;
                                            entityDtCheck.PatientAmount = total - totalPayer;
                                            entityDtCheck.PayerAmount = totalPayer;
                                            entityDtCheck.LineAmount = total;

                                            if (dt.PrescriptionOrderDetailID != 0)
                                            {
                                                entityDtCheck.PrescriptionOrderDetailID = dt.PrescriptionOrderDetailID;
                                            }

                                            entityDtCheck.LocationID = dt.LocationID;
                                            entityDtCheck.IsApproved = dt.IsApproved;
                                            entityDtCheck.CreatedBy = dt.CreatedBy;
                                            entityDtCheck.CreatedDate = dt.CreatedDate;
                                            entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                            entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                            #endregion
                                        }
                                        else
                                        {
                                            #region BARANG_UMUM
                                            decimal qty = dt.ChargedQuantity;

                                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDtCheck.ChargeClassID, entityDtCheck.ItemID, 3, DateTime.Now, ctx);

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

                                            entityDtCheck.BaseTariff = basePrice;
                                            entityDtCheck.Tariff = price;
                                            entityDtCheck.BaseComp1 = basePriceComp1;
                                            entityDtCheck.BaseComp2 = basePriceComp2;
                                            entityDtCheck.BaseComp3 = basePriceComp3;
                                            entityDtCheck.TariffComp1 = priceComp1;
                                            entityDtCheck.TariffComp2 = priceComp2;
                                            entityDtCheck.TariffComp3 = priceComp3;
                                            entityDtCheck.CostAmount = costAmount;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            entityDtCheck.GCBaseUnit = dt.GCBaseUnit;
                                            entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                            entityDtCheck.ParamedicID = dt.ParamedicID;

                                            entityDtCheck.IsVariable = dt.IsVariable;
                                            entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                            entityDtCheck.DiscountPercentageComp1 = discountAmountComp1;
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
                                                            entityDtCheck.DiscountPercentageComp2 = discountAmountComp2;
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
                                                            entityDtCheck.DiscountPercentageComp3 = discountAmountComp3;
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
                                                        entityDtCheck.DiscountPercentageComp1 = discountAmount;
                                                    }

                                                    if (priceComp2 > 0)
                                                    {
                                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                        entityDtCheck.DiscountPercentageComp2 = discountAmount;
                                                    }

                                                    if (priceComp3 > 0)
                                                    {
                                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                        entityDtCheck.DiscountPercentageComp3 = discountAmount;
                                                    }
                                                }

                                                if (entityDtCheck.DiscountPercentageComp1 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp1 = true;
                                                }

                                                if (entityDtCheck.DiscountPercentageComp2 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp2 = true;
                                                }

                                                if (entityDtCheck.DiscountPercentageComp3 > 0)
                                                {
                                                    entityDtCheck.IsDiscountInPercentageComp3 = true;
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

                                            if (grossLineAmount >= 0)
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

                                            entityDtCheck.ConversionFactor = dt.ConversionFactor;

                                            ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDtCheck.ItemID), ctx).FirstOrDefault();
                                            entityDtCheck.AveragePrice = iPlanning.AveragePrice;
                                            entityDtCheck.CostAmount = iPlanning.UnitPrice;

                                            entityDtCheck.IsCITO = dt.IsCITO;
                                            entityDtCheck.CITOAmount = dt.CITOAmount;
                                            entityDtCheck.IsComplication = dt.IsComplication;
                                            entityDtCheck.ComplicationAmount = dt.ComplicationAmount;

                                            entityDtCheck.IsDiscount = totalDiscountAmount != 0;
                                            entityDtCheck.DiscountAmount = totalDiscountAmount;
                                            entityDtCheck.DiscountComp2 = totalDiscountAmount2;
                                            entityDtCheck.DiscountComp3 = totalDiscountAmount3;

                                            entityDtCheck.UsedQuantity = entityDtCheck.BaseQuantity = entityDtCheck.ChargedQuantity = qty;
                                            entityDtCheck.PatientAmount = total - totalPayer;
                                            entityDtCheck.PayerAmount = totalPayer;
                                            entityDtCheck.LineAmount = total;
                                            entityDtCheck.LocationID = dt.LocationID;
                                            entityDtCheck.IsApproved = dt.IsApproved;
                                            entityDtCheck.CreatedBy = dt.CreatedBy;
                                            entityDtCheck.CreatedDate = dt.CreatedDate;
                                            entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                            entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                            #endregion
                                        }

                                        entityDtCheck.GCTransactionDetailStatus = dt.GCTransactionDetailStatus;
                                        entityDtCheck.TransactionID = dt.TransactionID;
                                        entityDtCheck.ID = dt.ID;
                                        entityDtCheck.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                        entityDtCheck.CalculateCreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageDtDao.Update(entityDtCheck);

                                        #endregion
                                    }
                                    else
                                    {
                                        #region INSERT DT

                                        PatientChargesClassCoverageDt entityDt = new PatientChargesClassCoverageDt();
                                        entityDt.TransactionID = dt.TransactionID;
                                        entityDt.ItemID = dt.ItemID;
                                        entityDt.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);

                                        if (im.GCItemType == Constant.ItemType.PELAYANAN || im.GCItemType == Constant.ItemType.LABORATORIUM || im.GCItemType == Constant.ItemType.RADIOLOGI || im.GCItemType == Constant.ItemType.PENUNJANG_MEDIS || im.GCItemType == Constant.ItemType.MEDICAL_CHECKUP)
                                        {
                                            #region PELAYANAN, LABORATORIUM, RADIOLOGI, PENUNJANG_MEDIS DAN MEDICAL_CHECKUP
                                            List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", entityDt.ItemID), ctx);

                                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDt.ChargeClassID, entityDt.ItemID, 1, DateTime.Now, ctx);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == entityDt.ItemID);

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

                                            entityDt.BaseTariff = basePrice;
                                            entityDt.Tariff = price;
                                            entityDt.BaseComp1 = basePriceComp1;
                                            entityDt.BaseComp2 = basePriceComp2;
                                            entityDt.BaseComp3 = basePriceComp3;
                                            entityDt.TariffComp1 = priceComp1;
                                            entityDt.TariffComp2 = priceComp2;
                                            entityDt.TariffComp3 = priceComp3;
                                            entityDt.CostAmount = costAmount;
                                            entityDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                                            entityDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                            entityDt.BaseCITOAmount = entity.CITOAmount;
                                            entityDt.BaseComplicationAmount = entity.ComplicationAmount;

                                            entityDt.GCBaseUnit = dt.GCBaseUnit;
                                            entityDt.GCItemUnit = dt.GCItemUnit;
                                            entityDt.IsSubContractItem = entity.IsSubContractItem;

                                            entityDt.ParamedicID = dt.ParamedicID;

                                            entityDt.IsVariable = dt.IsVariable;
                                            entityDt.IsUnbilledItem = dt.IsUnbilledItem;

                                            decimal qty = dt.ChargedQuantity;
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
                                                            entityDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                            entityDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                            entityDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                        entityDt.DiscountPercentageComp1 = discountAmount;
                                                    }

                                                    if (priceComp2 > 0)
                                                    {
                                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                        entityDt.DiscountPercentageComp2 = discountAmount;
                                                    }

                                                    if (priceComp3 > 0)
                                                    {
                                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                        entityDt.DiscountPercentageComp3 = discountAmount;
                                                    }
                                                }

                                                if (entityDt.DiscountPercentageComp1 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp1 = true;
                                                }

                                                if (entityDt.DiscountPercentageComp2 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp2 = true;
                                                }

                                                if (entityDt.DiscountPercentageComp3 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp3 = true;
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

                                            if (grossLineAmount >= 0)
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

                                            entityDt.IsCITO = dt.IsCITO;
                                            entityDt.CITOAmount = dt.CITOAmount;
                                            entityDt.IsComplication = dt.IsComplication;
                                            entityDt.ComplicationAmount = dt.ComplicationAmount;

                                            entityDt.IsDiscount = totalDiscountAmount != 0;
                                            entityDt.DiscountAmount = totalDiscountAmount;
                                            entityDt.DiscountComp1 = totalDiscountAmount1;
                                            entityDt.DiscountComp2 = totalDiscountAmount2;
                                            entityDt.DiscountComp3 = totalDiscountAmount3;

                                            entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;
                                            entityDt.PatientAmount = total - totalPayer;
                                            entityDt.PayerAmount = totalPayer;
                                            entityDt.LineAmount = total;

                                            entityDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, entityDt.ParamedicID, entityDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, hd.VisitID, hd.HealthcareServiceUnitID, hd.TransactionDate, hd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            if (entityDt.RevenueSharingID == 0)
                                                entityDt.RevenueSharingID = null;

                                            entityDt.CreatedBy = dt.CreatedBy;
                                            entityDt.CreatedDate = dt.CreatedDate;

                                            entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                            entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                            #endregion
                                        }
                                        else if (im.GCItemType == Constant.ItemType.OBAT_OBATAN || im.GCItemType == Constant.ItemType.BARANG_MEDIS)
                                        {
                                            #region OBAT_OBATAN DAN BARANG_MEDIS
                                            decimal qty = dt.ChargedQuantity;

                                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDt.ChargeClassID, entityDt.ItemID, 2, DateTime.Now, ctx);

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

                                            entityDt.BaseTariff = basePrice;
                                            entityDt.Tariff = price;
                                            entityDt.BaseComp1 = basePriceComp1;
                                            entityDt.BaseComp2 = basePriceComp2;
                                            entityDt.BaseComp3 = basePriceComp3;
                                            entityDt.TariffComp1 = priceComp1;
                                            entityDt.TariffComp2 = priceComp2;
                                            entityDt.TariffComp3 = priceComp3;
                                            entityDt.CostAmount = costAmount;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            entityDt.GCBaseUnit = dt.GCBaseUnit;
                                            entityDt.GCItemUnit = dt.GCItemUnit;
                                            entityDt.ParamedicID = dt.ParamedicID;

                                            entityDt.IsVariable = dt.IsVariable;
                                            entityDt.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                            entityDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                            entityDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                            entityDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                        entityDt.DiscountPercentageComp1 = discountAmount;
                                                    }

                                                    if (priceComp2 > 0)
                                                    {
                                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                        entityDt.DiscountPercentageComp2 = discountAmount;
                                                    }

                                                    if (priceComp3 > 0)
                                                    {
                                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                        entityDt.DiscountPercentageComp3 = discountAmount;
                                                    }
                                                }

                                                if (entityDt.DiscountPercentageComp1 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp1 = true;
                                                }

                                                if (entityDt.DiscountPercentageComp2 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp2 = true;
                                                }

                                                if (entityDt.DiscountPercentageComp3 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp3 = true;
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

                                            if (grossLineAmount >= 0)
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

                                            entityDt.ConversionFactor = dt.ConversionFactor;

                                            ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault();
                                            entityDt.AveragePrice = iPlanning.AveragePrice;
                                            entityDt.CostAmount = iPlanning.UnitPrice;

                                            entityDt.IsCITO = dt.IsCITO;
                                            entityDt.CITOAmount = dt.CITOAmount;
                                            entityDt.IsComplication = dt.IsComplication;
                                            entityDt.ComplicationAmount = dt.ComplicationAmount;

                                            //entityDt.IsDiscount = dt.IsDiscount;
                                            //entityDt.DiscountAmount = totalDiscountAmount;

                                            entityDt.IsDiscount = totalDiscountAmount != 0;
                                            entityDt.DiscountAmount = totalDiscountAmount;
                                            entityDt.DiscountComp1 = totalDiscountAmount1;
                                            entityDt.DiscountComp2 = totalDiscountAmount2;
                                            entityDt.DiscountComp3 = totalDiscountAmount3;

                                            entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;
                                            entityDt.PatientAmount = total - totalPayer;
                                            entityDt.PayerAmount = totalPayer;
                                            entityDt.LineAmount = total;

                                            if (dt.PrescriptionOrderDetailID != 0)
                                            {
                                                entityDt.PrescriptionOrderDetailID = dt.PrescriptionOrderDetailID;
                                            }

                                            entityDt.LocationID = dt.LocationID;
                                            entityDt.IsApproved = dt.IsApproved;
                                            entityDt.CreatedBy = dt.CreatedBy;
                                            entityDt.CreatedDate = dt.CreatedDate;
                                            entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                            entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);

                                            #endregion
                                        }
                                        else
                                        {
                                            #region BARANG_UMUM
                                            decimal qty = dt.ChargedQuantity;

                                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityDt.ChargeClassID, entityDt.ItemID, 3, DateTime.Now, ctx);

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

                                            entityDt.BaseTariff = basePrice;
                                            entityDt.Tariff = price;
                                            entityDt.BaseComp1 = basePriceComp1;
                                            entityDt.BaseComp2 = basePriceComp2;
                                            entityDt.BaseComp3 = basePriceComp3;
                                            entityDt.TariffComp1 = priceComp1;
                                            entityDt.TariffComp2 = priceComp2;
                                            entityDt.TariffComp3 = priceComp3;
                                            entityDt.CostAmount = costAmount;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            entityDt.GCBaseUnit = dt.GCBaseUnit;
                                            entityDt.GCItemUnit = dt.GCItemUnit;
                                            entityDt.ParamedicID = dt.ParamedicID;

                                            entityDt.IsVariable = dt.IsVariable;
                                            entityDt.IsUnbilledItem = dt.IsUnbilledItem;

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
                                                            entityDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                            entityDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                            entityDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                        entityDt.DiscountPercentageComp1 = discountAmount;
                                                    }

                                                    if (priceComp2 > 0)
                                                    {
                                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                        entityDt.DiscountPercentageComp2 = discountAmount;
                                                    }

                                                    if (priceComp3 > 0)
                                                    {
                                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                        entityDt.DiscountPercentageComp3 = discountAmount;
                                                    }
                                                }

                                                if (entityDt.DiscountPercentageComp1 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp1 = true;
                                                }

                                                if (entityDt.DiscountPercentageComp2 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp2 = true;
                                                }

                                                if (entityDt.DiscountPercentageComp3 > 0)
                                                {
                                                    entityDt.IsDiscountInPercentageComp3 = true;
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

                                            if (grossLineAmount >= 0)
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

                                            entityDt.ConversionFactor = dt.ConversionFactor;

                                            ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault();
                                            entityDt.AveragePrice = iPlanning.AveragePrice;
                                            entityDt.CostAmount = iPlanning.UnitPrice;

                                            entityDt.IsCITO = dt.IsCITO;
                                            entityDt.CITOAmount = dt.CITOAmount;
                                            entityDt.IsComplication = dt.IsComplication;
                                            entityDt.ComplicationAmount = dt.ComplicationAmount;

                                            //entityDt.IsDiscount = dt.IsDiscount;
                                            //entityDt.DiscountAmount = totalDiscountAmount;

                                            entityDt.IsDiscount = totalDiscountAmount != 0;
                                            entityDt.DiscountAmount = totalDiscountAmount;
                                            entityDt.DiscountComp1 = totalDiscountAmount1;
                                            entityDt.DiscountComp2 = totalDiscountAmount2;
                                            entityDt.DiscountComp3 = totalDiscountAmount3;

                                            entityDt.UsedQuantity = entityDt.BaseQuantity = entityDt.ChargedQuantity = qty;
                                            entityDt.PatientAmount = total - totalPayer;
                                            entityDt.PayerAmount = totalPayer;
                                            entityDt.LineAmount = total;
                                            entityDt.LocationID = dt.LocationID;
                                            entityDt.IsApproved = dt.IsApproved;
                                            entityDt.CreatedBy = dt.CreatedBy;
                                            entityDt.CreatedDate = dt.CreatedDate;
                                            entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                            entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);
                                            #endregion
                                        }

                                        entityDt.GCTransactionDetailStatus = dt.GCTransactionDetailStatus;
                                        entityDt.TransactionID = hd.TransactionID;
                                        entityDt.ID = dt.ID;
                                        entityDt.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                        entityDt.CalculateCreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageDtDao.Insert(entityDt);

                                        #endregion
                                    }
                                }
                            }

                            #endregion

                            #region LinkedReg

                            string filterExpressionHdLinked = string.Format("TransactionID IN ({0})", hdnPatientChargesHdIDLinked.Value);
                            List<PatientChargesHd> lstChargesHDLinked = BusinessLayer.GetPatientChargesHdList(filterExpressionHdLinked, ctx);
                            foreach (PatientChargesHd hd in lstChargesHDLinked)
                            {
                                PatientChargesClassCoverageHd entityHdCheck = entityPatientChargesClassCoverageHdDao.Get(hd.TransactionID);

                                if (entityHdCheck != null)
                                {
                                    #region UPDATE HD

                                    entityHdCheck.TransactionID = hd.TransactionID;
                                    entityHdCheck.VisitID = hd.VisitID;
                                    entityHdCheck.HealthcareServiceUnitID = hd.HealthcareServiceUnitID;
                                    entityHdCheck.TransactionCode = hd.TransactionCode;
                                    entityHdCheck.TransactionNo = hd.TransactionNo;
                                    entityHdCheck.TransactionDate = hd.TransactionDate;
                                    entityHdCheck.TransactionTime = hd.TransactionTime;
                                    entityHdCheck.PatientBillingID = hd.PatientBillingID;
                                    entityHdCheck.TestOrderID = hd.TestOrderID;
                                    entityHdCheck.ServiceOrderID = hd.ServiceOrderID;
                                    entityHdCheck.PrescriptionOrderID = hd.PrescriptionOrderID;
                                    entityHdCheck.PrescriptionReturnOrderID = hd.PrescriptionReturnOrderID;
                                    entityHdCheck.ReferenceNo = hd.ReferenceNo;
                                    entityHdCheck.GCTransactionStatus = hd.GCTransactionStatus;
                                    entityHdCheck.GCVoidReason = hd.GCVoidReason;
                                    entityHdCheck.VoidReason = hd.VoidReason;
                                    entityHdCheck.VoidBy = hd.VoidBy;
                                    entityHdCheck.VoidDate = hd.VoidDate;
                                    entityHdCheck.IsAutoTransaction = hd.IsAutoTransaction;
                                    entityHdCheck.IsVerified = hd.IsVerified;
                                    entityHdCheck.IsChargesTransfered = hd.IsChargesTransfered;
                                    entityHdCheck.Remarks = hd.Remarks;

                                    entityHdCheck.LinkedChargesID = hd.LinkedChargesID;
                                    entityHdCheck.IsPendingRecalculated = hd.IsPendingRecalculated;
                                    entityHdCheck.IsEntryByPhysician = hd.IsEntryByPhysician;
                                    entityHdCheck.IsCorrectionTransaction = hd.IsCorrectionTransaction;
                                    entityHdCheck.LastRecalculatedBy = hd.LastRecalculatedBy;

                                    if (hd.LastRecalculatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHdCheck.LastRecalculatedDate = hd.LastRecalculatedDate;
                                    }

                                    entityHdCheck.GCRecalculateReason = hd.GCRecalculateReason;
                                    entityHdCheck.RecalculateReason = hd.RecalculateReason;

                                    entityHdCheck.CreatedBy = hd.CreatedBy;
                                    entityHdCheck.CreatedDate = hd.CreatedDate;

                                    if (hd.LastUpdatedBy != null)
                                    {
                                        entityHdCheck.LastUpdatedBy = hd.LastUpdatedBy;
                                    }

                                    if (hd.LastUpdatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHdCheck.LastUpdatedDate = hd.LastUpdatedDate;
                                    }

                                    entityHdCheck.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                    entityHdCheck.CalculateCreatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityPatientChargesClassCoverageHdDao.Update(entityHdCheck);

                                    #endregion
                                }
                                else
                                {
                                    #region INSERT HD

                                    PatientChargesClassCoverageHd entityHd = new PatientChargesClassCoverageHd();
                                    entityHd.TransactionID = hd.TransactionID;
                                    entityHd.VisitID = hd.VisitID;
                                    entityHd.HealthcareServiceUnitID = hd.HealthcareServiceUnitID;
                                    entityHd.TransactionCode = hd.TransactionCode;
                                    entityHd.TransactionNo = hd.TransactionNo;
                                    entityHd.TransactionDate = hd.TransactionDate;
                                    entityHd.TransactionTime = hd.TransactionTime;
                                    entityHd.PatientBillingID = hd.PatientBillingID;
                                    entityHd.TestOrderID = hd.TestOrderID;
                                    entityHd.ServiceOrderID = hd.ServiceOrderID;
                                    entityHd.PrescriptionOrderID = hd.PrescriptionOrderID;
                                    entityHd.PrescriptionReturnOrderID = hd.PrescriptionReturnOrderID;
                                    entityHd.ReferenceNo = hd.ReferenceNo;
                                    entityHd.GCTransactionStatus = hd.GCTransactionStatus;
                                    entityHd.GCVoidReason = hd.GCVoidReason;
                                    entityHd.VoidReason = hd.VoidReason;
                                    entityHd.VoidBy = hd.VoidBy;
                                    entityHd.VoidDate = hd.VoidDate;
                                    entityHd.IsAutoTransaction = hd.IsAutoTransaction;
                                    entityHd.IsVerified = hd.IsVerified;
                                    entityHd.IsChargesTransfered = hd.IsChargesTransfered;
                                    entityHd.Remarks = hd.Remarks;

                                    entityHd.LinkedChargesID = hd.LinkedChargesID;
                                    entityHd.IsPendingRecalculated = hd.IsPendingRecalculated;
                                    entityHd.IsEntryByPhysician = hd.IsEntryByPhysician;
                                    entityHd.IsCorrectionTransaction = hd.IsCorrectionTransaction;
                                    entityHd.LastRecalculatedBy = hd.LastRecalculatedBy;

                                    if (hd.LastRecalculatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHd.LastRecalculatedDate = hd.LastRecalculatedDate;
                                    }

                                    entityHd.GCRecalculateReason = hd.GCRecalculateReason;
                                    entityHd.RecalculateReason = hd.RecalculateReason;

                                    entityHd.CreatedBy = hd.CreatedBy;
                                    entityHd.CreatedDate = hd.CreatedDate;

                                    if (hd.LastUpdatedBy != null)
                                    {
                                        entityHd.LastUpdatedBy = hd.LastUpdatedBy;
                                    }

                                    if (hd.LastUpdatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityHd.LastUpdatedDate = hd.LastUpdatedDate;
                                    }

                                    entityHd.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                    entityHd.CalculateCreatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityPatientChargesClassCoverageHdDao.Insert(entityHd);

                                    #endregion
                                }

                                string filterExpressionDt = string.Format("TransactionID IN ({0})", hd.TransactionID);
                                List<PatientChargesDt> lstChargesDtForInsert = BusinessLayer.GetPatientChargesDtList(filterExpressionDt, ctx);
                                foreach (PatientChargesDt dt in lstChargesDtForInsert)
                                {
                                    ItemMaster im = itemDao.Get(dt.ItemID);

                                    PatientChargesClassCoverageDt entityDtCheck = entityPatientChargesClassCoverageDtDao.Get(dt.ID);

                                    if (entityDtCheck != null)
                                    {
                                        #region UPDATE DT

                                        entityDtCheck.TransactionID = dt.TransactionID;
                                        entityDtCheck.ItemID = dt.ItemID;
                                        entityDtCheck.ChargeClassID = dt.ChargeClassID;
                                        entityDtCheck.BaseTariff = dt.BaseTariff;
                                        entityDtCheck.Tariff = dt.Tariff;
                                        entityDtCheck.BaseComp1 = dt.BaseComp1;
                                        entityDtCheck.BaseComp2 = dt.BaseComp2;
                                        entityDtCheck.BaseComp3 = dt.BaseComp3;
                                        entityDtCheck.TariffComp1 = dt.TariffComp1;
                                        entityDtCheck.TariffComp2 = dt.TariffComp2;
                                        entityDtCheck.TariffComp3 = dt.TariffComp3;
                                        entityDtCheck.CostAmount = dt.CostAmount;
                                        entityDtCheck.IsCITOInPercentage = dt.IsCITOInPercentage;
                                        entityDtCheck.IsComplicationInPercentage = dt.IsComplicationInPercentage;
                                        entityDtCheck.BaseCITOAmount = dt.CITOAmount;
                                        entityDtCheck.BaseComplicationAmount = dt.ComplicationAmount;
                                        entityDtCheck.GCBaseUnit = entityDtCheck.GCItemUnit = dt.GCItemUnit;
                                        entityDtCheck.IsSubContractItem = dt.IsSubContractItem;
                                        entityDtCheck.ParamedicID = dt.ParamedicID;
                                        entityDtCheck.IsVariable = dt.IsVariable;
                                        entityDtCheck.IsUnbilledItem = dt.IsUnbilledItem;
                                        entityDtCheck.DiscountPercentageComp1 = dt.DiscountPercentageComp1;
                                        entityDtCheck.DiscountPercentageComp2 = dt.DiscountPercentageComp2;
                                        entityDtCheck.DiscountPercentageComp3 = dt.DiscountPercentageComp3;
                                        entityDtCheck.IsDiscountInPercentageComp1 = dt.IsDiscountInPercentageComp1;
                                        entityDtCheck.IsDiscountInPercentageComp2 = dt.IsDiscountInPercentageComp2;
                                        entityDtCheck.IsDiscountInPercentageComp3 = dt.IsDiscountInPercentageComp3;
                                        entityDtCheck.IsCITO = dt.IsCITO;
                                        entityDtCheck.CITOAmount = dt.CITOAmount;
                                        entityDtCheck.IsComplication = dt.IsComplication;
                                        entityDtCheck.ComplicationAmount = dt.ComplicationAmount;
                                        entityDtCheck.IsDiscount = dt.IsDiscount;
                                        entityDtCheck.DiscountAmount = dt.DiscountAmount;
                                        entityDtCheck.DiscountComp1 = dt.DiscountComp1;
                                        entityDtCheck.DiscountComp2 = dt.DiscountComp2;
                                        entityDtCheck.DiscountComp3 = dt.DiscountComp3;
                                        entityDtCheck.UsedQuantity = dt.UsedQuantity;
                                        entityDtCheck.BaseQuantity = dt.BaseQuantity;
                                        entityDtCheck.ChargedQuantity = dt.ChargedQuantity;
                                        entityDtCheck.PatientAmount = dt.PatientAmount;
                                        entityDtCheck.PayerAmount = dt.PayerAmount;
                                        entityDtCheck.LineAmount = dt.LineAmount;
                                        entityDtCheck.RevenueSharingID = dt.RevenueSharingID;
                                        entityDtCheck.CreatedBy = dt.CreatedBy;
                                        entityDtCheck.CreatedDate = dt.CreatedDate;
                                        entityDtCheck.LastUpdatedBy = dt.LastUpdatedBy;
                                        entityDtCheck.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);
                                        entityDtCheck.GCTransactionDetailStatus = dt.GCTransactionDetailStatus;
                                        entityDtCheck.TransactionID = dt.TransactionID;
                                        entityDtCheck.ID = dt.ID;
                                        entityDtCheck.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                        entityDtCheck.CalculateCreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageDtDao.Update(entityDtCheck);

                                        #endregion
                                    }
                                    else
                                    {
                                        #region INSERT DT

                                        PatientChargesClassCoverageDt entityDt = new PatientChargesClassCoverageDt();

                                        entityDt.TransactionID = dt.TransactionID;
                                        entityDt.ItemID = dt.ItemID;
                                        entityDt.ChargeClassID = dt.ChargeClassID;
                                        entityDt.BaseTariff = dt.BaseTariff;
                                        entityDt.Tariff = dt.Tariff;
                                        entityDt.BaseComp1 = dt.BaseComp1;
                                        entityDt.BaseComp2 = dt.BaseComp2;
                                        entityDt.BaseComp3 = dt.BaseComp3;
                                        entityDt.TariffComp1 = dt.TariffComp1;
                                        entityDt.TariffComp2 = dt.TariffComp2;
                                        entityDt.TariffComp3 = dt.TariffComp3;
                                        entityDt.CostAmount = dt.CostAmount;
                                        entityDt.IsCITOInPercentage = dt.IsCITOInPercentage;
                                        entityDt.IsComplicationInPercentage = dt.IsComplicationInPercentage;
                                        entityDt.BaseCITOAmount = dt.CITOAmount;
                                        entityDt.BaseComplicationAmount = dt.ComplicationAmount;
                                        entityDt.GCBaseUnit = entityDt.GCItemUnit = dt.GCItemUnit;
                                        entityDt.IsSubContractItem = dt.IsSubContractItem;
                                        entityDt.ParamedicID = dt.ParamedicID;
                                        entityDt.IsVariable = dt.IsVariable;
                                        entityDt.IsUnbilledItem = dt.IsUnbilledItem;
                                        entityDt.DiscountPercentageComp1 = dt.DiscountPercentageComp1;
                                        entityDt.DiscountPercentageComp2 = dt.DiscountPercentageComp2;
                                        entityDt.DiscountPercentageComp3 = dt.DiscountPercentageComp3;
                                        entityDt.IsDiscountInPercentageComp1 = dt.IsDiscountInPercentageComp1;
                                        entityDt.IsDiscountInPercentageComp2 = dt.IsDiscountInPercentageComp2;
                                        entityDt.IsDiscountInPercentageComp3 = dt.IsDiscountInPercentageComp3;
                                        entityDt.IsCITO = dt.IsCITO;
                                        entityDt.CITOAmount = dt.CITOAmount;
                                        entityDt.IsComplication = dt.IsComplication;
                                        entityDt.ComplicationAmount = dt.ComplicationAmount;
                                        entityDt.IsDiscount = dt.IsDiscount;
                                        entityDt.DiscountAmount = dt.DiscountAmount;
                                        entityDt.DiscountComp1 = dt.DiscountComp1;
                                        entityDt.DiscountComp2 = dt.DiscountComp2;
                                        entityDt.DiscountComp3 = dt.DiscountComp3;
                                        entityDt.UsedQuantity = dt.UsedQuantity;
                                        entityDt.BaseQuantity = dt.BaseQuantity;
                                        entityDt.ChargedQuantity = dt.ChargedQuantity;
                                        entityDt.PatientAmount = dt.PatientAmount;
                                        entityDt.PayerAmount = dt.PayerAmount;
                                        entityDt.LineAmount = dt.LineAmount;
                                        entityDt.RevenueSharingID = dt.RevenueSharingID;
                                        entityDt.CreatedBy = dt.CreatedBy;
                                        entityDt.CreatedDate = dt.CreatedDate;
                                        entityDt.LastUpdatedBy = dt.LastUpdatedBy;
                                        entityDt.LastUpdatedDate = Convert.ToDateTime(dt.LastUpdatedDate);
                                        entityDt.GCTransactionDetailStatus = dt.GCTransactionDetailStatus;
                                        entityDt.TransactionID = dt.TransactionID;
                                        entityDt.ID = dt.ID;
                                        entityDt.CalculateCreatedBy = AppSession.UserLogin.UserID;
                                        entityDt.CalculateCreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageDtDao.Insert(entityDt);

                                        #endregion
                                    }
                                }
                            }

                            #endregion

                            #endregion
                        }
                    }
                    ctx.CommitTransaction();
                }
                else if (type == "reset")
                {
                    if (!String.IsNullOrEmpty(hdnPatientChargesHdID.Value) && !String.IsNullOrEmpty(hdnPatientChargesDtID.Value))
                    {
                        #region RESET ALL COVERAGE

                        string filterExpressionHd = string.Format("TransactionID IN ({0})", hdnPatientChargesHdID.Value);
                        List<PatientChargesHd> lstChargesHD = BusinessLayer.GetPatientChargesHdList(filterExpressionHd, ctx);

                        string filterExpressionDt = string.Format("ID IN ({0})", hdnPatientChargesDtID.Value);
                        List<PatientChargesDt> lstChargesDT = BusinessLayer.GetPatientChargesDtList(filterExpressionDt, ctx);

                        if (lstChargesHD.Count > 0)
                        {
                            foreach (PatientChargesHd hd in lstChargesHD)
                            {
                                PatientChargesClassCoverageHd entityHdCheck = entityPatientChargesClassCoverageHdDao.Get(hd.TransactionID);

                                if (entityHdCheck != null)
                                {
                                    #region UPDATE HD

                                    entityHdCheck.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entityHdCheck.GCVoidReason = Constant.DeleteReason.OTHER;
                                    entityHdCheck.VoidReason = "RESET HITUNG JATAH KELAS";
                                    entityHdCheck.VoidBy = AppSession.UserLogin.UserID;
                                    entityHdCheck.VoidDate = DateTime.Now;
                                    entityHdCheck.LastUpdatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityPatientChargesClassCoverageHdDao.Update(entityHdCheck);

                                    #endregion
                                }

                                List<PatientChargesDt> lstChargesDtForInsert = lstChargesDT.Where(t => t.TransactionID == hd.TransactionID).ToList();
                                foreach (PatientChargesDt dt in lstChargesDtForInsert)
                                {
                                    PatientChargesClassCoverageDt entityDtCheck = entityPatientChargesClassCoverageDtDao.Get(dt.ID);

                                    if (entityDtCheck != null)
                                    {
                                        #region UPDATE DT

                                        entityDtCheck.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                        entityDtCheck.LastUpdatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityPatientChargesClassCoverageDtDao.Update(entityDtCheck);

                                        #endregion
                                    }
                                }
                            }
                        }

                        #endregion
                    }
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
    }
}