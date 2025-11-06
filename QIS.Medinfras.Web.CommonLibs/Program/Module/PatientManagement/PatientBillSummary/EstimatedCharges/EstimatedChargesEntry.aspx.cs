using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EstimatedChargesEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.ESTIMATED_CHARGES_COPY;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            hdnChargesClassID.Value = entity.ChargeClassID.ToString();
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
        }

        private void BindGridDetail()
        {
            string filter = string.Format("EstimatedChargesHdID = '0' AND IsDeleted = 0 ORDER BY ItemID");
            if (!String.IsNullOrEmpty(hdnTransactionID.Value))
            {
                filter = string.Format("EstimatedChargesHdID = {0} AND IsDeleted = 0 ORDER BY ItemID", hdnTransactionID.Value);
            }
            List<vEstimatedChargesDt> lst = BusinessLayer.GetvEstimatedChargesDtList(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //}
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0];

            if (result == "afterProcess")
            {
                BindGridDetail();
                result += "|";
            }
            else
            {
                BindGridDetail();
                result += "|";
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            if (type == "process")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
                EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);
                PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
                PatientChargesDtParamedicDao patientChargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
                PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
                HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);
                ItemMasterDao itemDao = new ItemMasterDao(ctx);
                try
                {
                    EstimatedChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    string filterDt = string.Format("EstimatedChargesHdID = '{0}' AND IsDeleted = 0", entityHd.ID);
                    List<EstimatedChargesDt> lstEntityDt = BusinessLayer.GetEstimatedChargesDtList(filterDt, ctx);

                    PatientChargesHd patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    switch (hdnDepartmentID.Value)
                    {
                        case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                        case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                        case Constant.Facility.DIAGNOSTIC:
                            if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                                patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                            else if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                                patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                            else
                                patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                        default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                    }
                    patientChargesHd.TransactionDate = DateTime.Now;
                    int hourNow = DateTime.Now.Hour;
                    int minutesNow = DateTime.Now.Minute;
                    string hour = "0";
                    if (hourNow < 10)
                    {
                        hour += string.Format("{0}", hourNow);
                    }
                    else
                    {
                        hour = string.Format("{0}", hourNow);
                    }

                    string minute = "0";
                    if (minutesNow < 10)
                    {
                        minute += string.Format("{0}", minutesNow);
                    }
                    else
                    {
                        minute = string.Format("{0}", minutesNow);
                    }
                    patientChargesHd.TransactionTime = string.Format("{0}:{1}", hour, minute);
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.Remarks = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

                    foreach (EstimatedChargesDt e in lstEntityDt)
                    {
                        ItemMaster item = itemDao.Get(e.ItemID);

                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = e.ItemID;
                        patientChargesDt.ChargeClassID = Convert.ToInt32(hdnChargesClassIDSelected.Value);

                        List<GetCurrentItemTariff> list = new List<GetCurrentItemTariff>();
                        if (item.GCItemType == Constant.ItemGroupMaster.SERVICE || item.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || item.GCItemType == Constant.ItemGroupMaster.LABORATORY || item.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
                        {
                            list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), patientChargesHd.VisitID, patientChargesDt.ChargeClassID, e.ItemID, 1, DateTime.Now, ctx);
                        }
                        else
                        {
                            int typeItem = 2;
                            if (item.GCItemType == Constant.ItemGroupMaster.LOGISTIC)
                            {
                                typeItem = 3;
                            }
                            list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), patientChargesHd.VisitID, patientChargesDt.ChargeClassID, e.ItemID, typeItem, DateTime.Now, ctx);
                        }

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

                        if (item.GCItemType == Constant.ItemGroupMaster.SERVICE || item.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || item.GCItemType == Constant.ItemGroupMaster.LABORATORY || item.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterService = string.Format("ItemID = '{0}'", e.ItemID);
                            vItemService entity = BusinessLayer.GetvItemServiceList(filterService, ctx).FirstOrDefault();

                            patientChargesDt.IsSubContractItem = entity.IsSubContractItem;
                            patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entity.GCItemUnit;

                            patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, patientChargesHd.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            if (patientChargesDt.RevenueSharingID == 0)
                                patientChargesDt.RevenueSharingID = null;
                        }
                        else
                        {
                            patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = item.GCItemUnit;
                        }

                        patientChargesDt.ParamedicID = AppSession.RegisteredPatient.ParamedicID;

                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal qty = e.Qty;
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
                        patientChargesDt.IsCITOInPercentage = false;
                        patientChargesDt.BaseCITOAmount = 0;
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

                        //patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID; -> comment by RN - 20181119
                        patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                        patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                        patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                        int oChargesDtID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

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
                            patientChargesDtParamedicDao.Insert(dtparamedic);
                        }

                        int countInventoryItemDetail = 0;

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
                            entityDtPackageDao.Insert(dtpackage);
                        }

                        if (countInventoryItemDetail > 0)
                        {
                            HealthcareServiceUnit chargesHSU = chargesHSUDao.Get(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));

                            PatientChargesDt pcdt = patientChargesDtDao.Get(oChargesDtID);
                            pcdt.LocationID = chargesHSU.LocationID;
                            pcdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            patientChargesDtDao.Update(pcdt);
                        }
                    }

                    retval = patientChargesHd.TransactionNo;

                    entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
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
            else if (type == "cancel")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
                EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);
                try
                {
                    EstimatedChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
                    ctx.CommitTransaction();
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
            return true;
        }
    }
}