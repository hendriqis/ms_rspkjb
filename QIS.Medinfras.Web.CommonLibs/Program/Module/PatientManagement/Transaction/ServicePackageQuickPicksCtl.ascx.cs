using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServicePackageQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            hdnParam.Value = param;
            hdnTransactionID.Value = hdnParam.Value.Split('|')[0];
            hdnHealthcareServiceUnitID.Value = hdnParam.Value.Split('|')[1];
            hdnVisitIDCtl.Value = hdnParam.Value.Split('|')[2];
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[3];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[4];
            hdnIsAccompany.Value = hdnParam.Value.Split('|')[5];

            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            //if (reg.GuestID != 0 && reg.GuestID != null)
            //{
            //    hdnGuestID.Value = reg.GuestID.ToString();
            //}
            //else
            //{
            //    Patient patient = BusinessLayer.GetPatient(Convert.ToInt32(reg.MRN));
            //    hdnGuestID.Value = "0";
            //}
            hdnGuestID.Value = reg.GuestID > 0 ? reg.GuestID.ToString() : "";
            hdnMRN.Value = reg.MRN > 0 ? reg.MRN.ToString() : "";

            hdnIsBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

            ConsultVisit visit = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitIDCtl.Value));

            vHealthcareServiceUnit visitHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", visit.HealthcareServiceUnitID)).FirstOrDefault();

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField(cboServiceChargeClassIDCtl, lstClassCare, "ClassName", "ClassID");

            if (visit.ChargeClassID != 0)
            {
                cboServiceChargeClassIDCtl.Value = Convert.ToString(visit.ChargeClassID);
            }

            if (visitHSU.DepartmentID == Constant.Facility.INPATIENT)
            {
                cboServiceChargeClassIDCtl.Enabled = true;
            }
            else
            {
                cboServiceChargeClassIDCtl.Enabled = false;
            }

            SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS);
            hdnIsOnlyBPJSItem.Value = setvar.ParameterValue;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

            if (hdnParam.Value.Split('|')[4] == Constant.Facility.DIAGNOSTIC)
            {
                if (hdnHealthcareServiceUnitID.Value == BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue)
                {
                    hdnGCItemType.Value = Constant.ItemGroupMaster.LABORATORY;

                    SettingParameter sp = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.LB_KODE_DEFAULT_DOKTER))[0];
                    SettingParameterDt spd = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", sp.ParameterCode))[0];
                    if (spd.ParameterValue != null && spd.ParameterValue != "")
                    {
                        ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0} AND IsDeleted = 0", Convert.ToInt32(spd.ParameterValue)))[0];
                        hdnParamedicID.Value = spd.ParameterValue;
                        txtParamedicCode.Text = pm.ParamedicCode;
                        txtParamedicName.Text = pm.FullName;
                    }
                }
                else if (hdnHealthcareServiceUnitID.Value == BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue)
                {
                    hdnGCItemType.Value = Constant.ItemGroupMaster.RADIOLOGY;

                    SettingParameter sp = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.IS_KODE_DEFAULT_DOKTER))[0];
                    SettingParameterDt spd = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", sp.ParameterCode))[0];
                    if (spd.ParameterValue != null && spd.ParameterValue != "")
                    {
                        ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0} AND IsDeleted = 0", Convert.ToInt32(spd.ParameterValue)))[0];
                        hdnParamedicID.Value = spd.ParameterValue;
                        txtParamedicCode.Text = pm.ParamedicCode;
                        txtParamedicName.Text = pm.FullName;
                    }
                }
                else
                {
                    hdnGCItemType.Value = Constant.ItemGroupMaster.DIAGNOSTIC;
                }
            }
            else
                hdnGCItemType.Value = Constant.ItemGroupMaster.SERVICE;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string medicSupport = "";
            medicSupport += hdnParam.Value.Split('|')[1];

            if (hdnItemGroupID.Value == "")
                filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND (ItemName2 LIKE '%{1}%' OR ItemCode LIKE '%{1}%')", medicSupport, hdnFilterItem.Value);
            else
                filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND (ItemName2 LIKE '%{1}%' OR ItemCode LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%')", medicSupport, hdnFilterItem.Value, hdnItemGroupID.Value);

            if (hdnRegistrationID.Value == "1")
            {
                if (hdnIsOnlyBPJSItem.Value == "1")
                {
                    filterExpression += " AND IsBPJS = 1";
                }
            }

            filterExpression += string.Format(" AND GCItemStatus != '{0}' AND Quantity > 0", Constant.ItemStatus.IN_ACTIVE);

            if (!string.IsNullOrEmpty(hdnMRN.Value))
            {
                filterExpression += string.Format(" AND MRN = {0}", hdnMRN.Value);
            }
            else
            {
                filterExpression += string.Format(" AND GuestID = {0}", hdnGuestID.Value);
            }
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //vServiceUnitItemPackageBalance entity = e.Row.DataItem as vServiceUnitItemPackageBalance;
                //CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                //if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                //    chkIsSelected.Checked = true;
                //if (entity.isNotExpired == false)
                //{
                //    e.Row.BackColor = System.Drawing.Color.Red;
                //}
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
            {
                List<vPatientChargesDt> lstItemID = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}','{4}') AND IsDeleted = 0", hdnTransactionID.Value, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vPatientChargesDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitItemPackageBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vServiceUnitItemPackageBalance> lstEntity = BusinessLayer.GetvServiceUnitItemPackageBalanceList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            //if (AppSession.IsHiddenPrice) //transaksi klinik
            //{
            //    ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            //    if (entityVisit != null)
            //    {
            //        if (entityVisit.GCVisitStatus != Constant.VisitStatus.NURSE_DISCHARGE && entityVisit.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE)
            //        {
            //            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityVisit.ParamedicID)).FirstOrDefault();
            //            if (entityPM != null)
            //            {
            //                if (entityPM.GCParamedicMasterType != Constant.ParamedicType.Physician)
            //                {
            //                    if (AppSession.UserLogin.ParamedicID != entityVisit.ParamedicID)
            //                    {
            //                        errMessage = string.Format("Tidak bisa menyimpan transaksi, kunjungan ini bukan ditujukan untuk anda");
            //                        return false;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtInfoDao patientChargesDtInfoDao = new PatientChargesDtInfoDao(ctx);
            VisitPackageBalanceHdDao VisitPackageBalanceHdDao = new VisitPackageBalanceHdDao(ctx);
            VisitPackageBalanceDtDao VisitPackageBalanceDtDao = new VisitPackageBalanceDtDao(ctx);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            PatientChargesDtParamedicDao patientChargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            try
            {
                #region Patient Charges
                PatientChargesHd patientChargesHd = null;
                int visitID = Convert.ToInt32(hdnParam.Value.Split('|')[2]);
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = visitID;
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnParam.Value.Split('|')[1]);
                    if (hdnIsAccompany.Value == "0")
                    {
                        switch (hdnDepartmentID.Value)
                        {
                            case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                            case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                            case Constant.Facility.PHARMACY: patientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OUTPATIENT; break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                else if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                else
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                            default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                        }
                    }
                    else
                    {
                        patientChargesHd.TransactionCode = Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES;
                    }
                    patientChargesHd.TransactionDate = Helper.GetDatePickerValue(DetailPage.GetTransactionDate());
                    patientChargesHd.TransactionTime = DetailPage.GetTransactionTime();
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    hdnTransactionID.Value = patientChargesHd.TransactionID.ToString();
                }
                else
                {
                    //to make sure, transaction id is belong to selected patient (visit id)
                    string filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, hdnTransactionID.Value);
                    patientChargesHd = BusinessLayer.GetPatientChargesHdList(filterExpression).FirstOrDefault();
                    //patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                }
                retval = patientChargesHd.TransactionNo;
                #endregion

                if (!patientChargesHd.IsEntryByPhysician)
                {
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            int registrationID = Convert.ToInt32(hdnParam.Value.Split('|')[3]);
                            lstSelectedMember = hdnSelectedMember.Value.Split(',');
                            string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');

                            List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                            int ct = 0;
                            foreach (String itemID in lstSelectedMember)
                            {
                                PatientChargesDt patientChargesDt = new PatientChargesDt();
                                patientChargesDt.ItemID = Convert.ToInt32(itemID);
                                patientChargesDt.ChargeClassID = Convert.ToInt32(cboServiceChargeClassIDCtl.Value);

                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 1, DateTime.Now, ctx);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID);

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
                                int oRevenueSharingID = 0;

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
                                    priceComp1 = obj.PriceComp1;
                                    priceComp2 = obj.PriceComp2;
                                    priceComp3 = obj.PriceComp3;
                                    basePriceComp1 = obj.BasePriceComp1;
                                    basePriceComp2 = obj.BasePriceComp2;
                                    basePriceComp3 = obj.BasePriceComp3;
                                    costAmount = obj.CostAmount;
                                    //oRevenueSharingID = obj.RevenueSharingID != null ? Convert.ToInt32(obj.RevenueSharingID) : 0;
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
                                patientChargesDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                                patientChargesDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                patientChargesDt.BaseCITOAmount = entity.CITOAmount;
                                patientChargesDt.BaseComplicationAmount = entity.ComplicationAmount;

                                vItemService itemService = lstItemService.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemService.GCItemUnit;
                                patientChargesDt.IsSubContractItem = entity.IsSubContractItem;

                                //patientChargesDt.IsPPN = itemService.IsPPN;

                                patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);

                                patientChargesDt.IsVariable = false;
                                patientChargesDt.IsUnbilledItem = true;

                                decimal totalDiscountAmount = 0;
                                decimal totalDiscountAmount1 = 0;
                                decimal totalDiscountAmount2 = 0;
                                decimal totalDiscountAmount3 = 0;

                                decimal packageQty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                                decimal grossLineAmount = packageQty * price;
                                if (isDiscountInPercentage)
                                {
                                    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                                    if (priceComp1 > 0)
                                        totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                    if (priceComp2 > 0)
                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                    if (priceComp3 > 0)
                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                }
                                else
                                {
                                    totalDiscountAmount = discountAmount * 1;
                                    if (priceComp1 > 0)
                                        totalDiscountAmount1 = discountAmount;
                                    if (priceComp2 > 0)
                                        totalDiscountAmount2 = discountAmount;
                                    if (priceComp3 > 0)
                                        totalDiscountAmount3 = discountAmount;
                                }

                                if (totalDiscountAmount > grossLineAmount)
                                    totalDiscountAmount = grossLineAmount;

                                decimal total = grossLineAmount - totalDiscountAmount;
                                decimal totalPayer = 0;
                                if (isCoverageInPercentage)
                                {
                                    totalPayer = total * coverageAmount / 100;
                                }
                                else
                                {
                                    totalPayer = coverageAmount * packageQty;
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
                                patientChargesDt.IsDiscount = totalDiscountAmount > 0;
                                patientChargesDt.DiscountAmount = totalDiscountAmount;
                                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                patientChargesDt.DiscountComp3 = totalDiscountAmount3;
                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                                patientChargesDt.PatientAmount = 0;
                                patientChargesDt.PayerAmount = 0;
                                patientChargesDt.LineAmount = 0;

                                if (oRevenueSharingID != null && oRevenueSharingID != 0)
                                {
                                    patientChargesDt.RevenueSharingID = oRevenueSharingID; //revSharing dari skema promosi
                                }
                                else
                                {
                                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                }

                                if (patientChargesDt.RevenueSharingID == 0)
                                    patientChargesDt.RevenueSharingID = null;

                                patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                                patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                string filterBalance = string.Format("MRN = {0} AND ItemID = {1} AND HealthcareServiceUnitID = {2} AND Quantity > 0", hdnMRN.Value, patientChargesDt.ItemID, AppSession.RegisteredPatient.HealthcareServiceUnitID);
                                vVisitPackageBalanceHdChargesInfo balanceHD = BusinessLayer.GetvVisitPackageBalanceHdChargesInfoList(filterBalance, ctx).FirstOrDefault();

                                string filterInfo = string.Format("VisitPackageBalanceTransactionID = {0} AND ISNULL(PackageBalanceQtyTaken,0) > 0", balanceHD.TransactionID);
                                List<PatientChargesDtInfo> lstInfo = BusinessLayer.GetPatientChargesDtInfoList(filterInfo, ctx);

                                int takenNumber = 1;
                                if (lstInfo.Count > 0)
                                {
                                    takenNumber = lstInfo.Count + 1;
                                }

                                PatientChargesDtInfo info = patientChargesDtInfoDao.Get(ID);
                                info.IsPackageBalance = true;
                                info.IsFirstPackageBalance = false;
                                info.TakenNumber = takenNumber;
                                info.PackageBalanceQtyTaken = packageQty;
                                info.VisitPackageBalanceTransactionID = balanceHD.TransactionID;
                                info.LastUpdatedBy = AppSession.UserLogin.UserID;

                                decimal qtySisa = balanceHD.Quantity - patientChargesDt.ChargedQuantity;
                                if (qtySisa < 0)
                                {
                                    result = false;
                                    ItemMaster item = itemMasterDao.Get(patientChargesDt.ItemID);
                                    if (!String.IsNullOrEmpty(errMessage))
                                    {
                                        errMessage += string.Format("Sisa paket untuk item {0} tidak mencukupi", item.ItemName1);
                                    }
                                    else
                                    {
                                        errMessage = string.Format("Sisa paket untuk item {0} tidak mencukupi", item.ItemName1);
                                    }
                                }
                                else
                                {
                                    patientChargesDtInfoDao.Update(info);
                                }

                                string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", patientChargesDt.ParamedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                                List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
                                foreach (ParamedicMasterTeam pmt in pmtList)
                                {
                                    PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                                    dtparamedic.ID = ID;
                                    dtparamedic.ItemID = patientChargesDt.ItemID;
                                    dtparamedic.ParamedicID = pmt.ParamedicID;
                                    dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                                    dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesDtParamedicDao.Insert(dtparamedic);
                                }

                                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                                foreach (vItemServiceDt isd in isdList)
                                {
                                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                    dtpackage.PatientChargesDtID = ID;
                                    dtpackage.ItemID = isd.DetailItemID;
                                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                    //dtpackage.ConversionFactor = isd.ConversionFactor;
                                    //dtpackage.GCItemUnit = isd.GCItemUnit;

                                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    if (revID != 0 && revID != null)
                                    {
                                        dtpackage.RevenueSharingID = revID;
                                    }
                                    else
                                    {
                                        dtpackage.RevenueSharingID = null;
                                    }

                                    dtpackage.ChargedQuantity = (isd.Quantity * info.PackageBalanceQtyTaken);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    dtpackage.BaseTariff = 0;
                                    dtpackage.BaseComp1 = 0;
                                    dtpackage.BaseComp2 = 0;
                                    dtpackage.BaseComp3 = 0;
                                    dtpackage.Tariff = 0;
                                    dtpackage.TariffComp1 = 0;
                                    dtpackage.TariffComp2 = 0;
                                    dtpackage.TariffComp3 = 0;
                                    dtpackage.CostAmount = 0;
                                    dtpackage.DiscountAmount = 0;
                                    dtpackage.DiscountComp1 = 0;
                                    dtpackage.DiscountComp2 = 0;
                                    dtpackage.DiscountComp3 = 0;

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

                                ct++;

                                if ((patientChargesDt.ChargedQuantity % 1) > 0)
                                {
                                    ItemMaster master = itemMasterDao.Get(patientChargesDt.ItemID);
                                    if (String.IsNullOrEmpty(errMessage))
                                    {
                                        errMessage = string.Format("Jumlah Pelayanan Untuk Item {0} harus bulat", master.ItemName1);
                                        result = false;
                                    }
                                    else
                                    {
                                        errMessage += string.Format("<br/> Jumlah Pelayanan Untuk Item {0} harus bulat", master.ItemName1);
                                        result = false;
                                    }
                                }
                            }

                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                            result = false;
                        }
                    }
                    else
                    {
                        if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                            int registrationID = Convert.ToInt32(hdnParam.Value.Split('|')[3]);
                            lstSelectedMember = hdnSelectedMember.Value.Split(',');
                            string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');

                            List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                            int ct = 0;
                            foreach (String itemID in lstSelectedMember)
                            {
                                PatientChargesDt patientChargesDt = new PatientChargesDt();
                                patientChargesDt.ItemID = Convert.ToInt32(itemID);
                                patientChargesDt.ChargeClassID = Convert.ToInt32(cboServiceChargeClassIDCtl.Value);

                                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 1, DateTime.Now, ctx);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vItemService entity = lstItemService.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID);
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
                                int oRevenueSharingID = 0;

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
                                    priceComp1 = obj.PriceComp1;
                                    priceComp2 = obj.PriceComp2;
                                    priceComp3 = obj.PriceComp3;
                                    basePriceComp1 = obj.BasePriceComp1;
                                    basePriceComp2 = obj.BasePriceComp2;
                                    basePriceComp3 = obj.BasePriceComp3;
                                    costAmount = obj.CostAmount;
                                    //oRevenueSharingID = obj.RevenueSharingID != null ? Convert.ToInt32(obj.RevenueSharingID) : 0;
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
                                patientChargesDt.IsCITOInPercentage = entity.IsCITOInPercentage;
                                patientChargesDt.IsComplicationInPercentage = entity.IsComplicationInPercentage;
                                patientChargesDt.BaseCITOAmount = entity.CITOAmount;
                                patientChargesDt.BaseComplicationAmount = entity.ComplicationAmount;
                                vItemService itemService = lstItemService.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemService.GCItemUnit;

                                patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);

                                patientChargesDt.IsVariable = false;
                                patientChargesDt.IsUnbilledItem = true;
                                //patientChargesDt.IsPPN = itemService.IsPPN;

                                decimal totalDiscountAmount = 0;
                                decimal totalDiscountAmount1 = 0;
                                decimal totalDiscountAmount2 = 0;
                                decimal totalDiscountAmount3 = 0;

                                decimal packageQty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                                decimal grossLineAmount = packageQty * price;
                                if (isDiscountInPercentage)
                                {
                                    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                                    if (priceComp1 > 0)
                                        totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                    if (priceComp2 > 0)
                                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                    if (priceComp3 > 0)
                                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                }
                                else
                                {
                                    totalDiscountAmount = discountAmount * 1;
                                    if (priceComp1 > 0)
                                        totalDiscountAmount1 = discountAmount;
                                    if (priceComp2 > 0)
                                        totalDiscountAmount2 = discountAmount;
                                    if (priceComp3 > 0)
                                        totalDiscountAmount3 = discountAmount;
                                }

                                if (totalDiscountAmount > grossLineAmount)
                                    totalDiscountAmount = grossLineAmount;

                                decimal total = grossLineAmount - totalDiscountAmount;
                                decimal totalPayer = 0;
                                if (isCoverageInPercentage)
                                    totalPayer = total * coverageAmount / 100;
                                else
                                    totalPayer = coverageAmount * packageQty;
                                if (totalPayer > total)
                                    totalPayer = total;

                                patientChargesDt.IsCITO = false;
                                patientChargesDt.CITOAmount = 0;
                                patientChargesDt.IsComplication = false;
                                patientChargesDt.ComplicationAmount = 0;
                                patientChargesDt.IsDiscount = totalDiscountAmount > 0;
                                patientChargesDt.DiscountAmount = totalDiscountAmount;
                                patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                                patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                                patientChargesDt.DiscountComp3 = totalDiscountAmount3;
                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                                patientChargesDt.PatientAmount = 0;
                                patientChargesDt.PayerAmount = 0;
                                patientChargesDt.LineAmount = 0;

                                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                if (patientChargesDt.RevenueSharingID == 0)
                                    patientChargesDt.RevenueSharingID = null;

                                //patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID; -> comment by RN - 20181119
                                patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                                patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                                string filterBalance = string.Format("MRN = {0} AND ItemID = {1} AND HealthcareServiceUnitID = {2} AND Quantity > 0", hdnMRN.Value, patientChargesDt.ItemID, AppSession.RegisteredPatient.HealthcareServiceUnitID);
                                vVisitPackageBalanceHdChargesInfo balanceHD = BusinessLayer.GetvVisitPackageBalanceHdChargesInfoList(filterBalance, ctx).FirstOrDefault();

                                string filterInfo = string.Format("VisitPackageBalanceTransactionID = {0} AND ISNULL(PackageBalanceQtyTaken,0) > 0", balanceHD.TransactionID);
                                List<PatientChargesDtInfo> lstInfo = BusinessLayer.GetPatientChargesDtInfoList(filterInfo, ctx);

                                int takenNumber = 1;
                                if (lstInfo.Count > 0)
                                {
                                    takenNumber = lstInfo.Count + 1;
                                }

                                PatientChargesDtInfo info = patientChargesDtInfoDao.Get(ID);
                                info.IsPackageBalance = true;
                                info.IsFirstPackageBalance = false;
                                info.TakenNumber = takenNumber;
                                info.PackageBalanceQtyTaken = packageQty;
                                info.VisitPackageBalanceTransactionID = balanceHD.TransactionID;
                                info.LastUpdatedBy = AppSession.UserLogin.UserID;
                                //info.LastUpdatedDate = GetCurrentDate();

                                decimal qtySisa = balanceHD.Quantity - info.PackageBalanceQtyTaken;
                                if (qtySisa < 0)
                                {
                                    result = false;
                                    ItemMaster item = itemMasterDao.Get(patientChargesDt.ItemID);
                                    if (!String.IsNullOrEmpty(errMessage))
                                    {
                                        errMessage += string.Format("Sisa paket untuk item {0} tidak mencukupi", item.ItemName1);
                                    }
                                    else
                                    {
                                        errMessage = string.Format("Sisa paket untuk item {0} tidak mencukupi", item.ItemName1);
                                    }
                                }
                                else
                                {
                                    patientChargesDtInfoDao.Update(info);
                                }

                                string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", patientChargesDt.ParamedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                                List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
                                foreach (ParamedicMasterTeam pmt in pmtList)
                                {
                                    PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                                    dtparamedic.ID = ID;
                                    dtparamedic.ItemID = patientChargesDt.ItemID;
                                    dtparamedic.ParamedicID = pmt.ParamedicID;
                                    dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                                    dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesDtParamedicDao.Insert(dtparamedic);
                                }

                                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                                foreach (vItemServiceDt isd in isdList)
                                {
                                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                    dtpackage.PatientChargesDtID = ID;
                                    dtpackage.ItemID = isd.DetailItemID;
                                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                    //dtpackage.ConversionFactor = isd.ConversionFactor;
                                    //dtpackage.GCItemUnit = isd.GCItemUnit;

                                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                    if (revID != 0 && revID != null)
                                    {
                                        dtpackage.RevenueSharingID = revID;
                                    }
                                    else
                                    {
                                        dtpackage.RevenueSharingID = null;
                                    }

                                    dtpackage.ChargedQuantity = (isd.Quantity * info.PackageBalanceQtyTaken);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    dtpackage.BaseTariff = 0;
                                    dtpackage.BaseComp1 = 0;
                                    dtpackage.BaseComp2 = 0;
                                    dtpackage.BaseComp3 = 0;
                                    dtpackage.Tariff = 0;
                                    dtpackage.TariffComp1 = 0;
                                    dtpackage.TariffComp2 = 0;
                                    dtpackage.TariffComp3 = 0;
                                    dtpackage.CostAmount = 0;
                                    dtpackage.DiscountAmount = 0;
                                    dtpackage.DiscountComp1 = 0;
                                    dtpackage.DiscountComp2 = 0;
                                    dtpackage.DiscountComp3 = 0;

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

                                ct++;

                                if ((patientChargesDt.ChargedQuantity % 1) > 0)
                                {
                                    ItemMaster master = itemMasterDao.Get(patientChargesDt.ItemID);
                                    if (String.IsNullOrEmpty(errMessage))
                                    {
                                        errMessage = string.Format("Jumlah Pelayanan Untuk Item {0} harus bulat", master.ItemName1);
                                        result = false;
                                    }
                                    else
                                    {
                                        errMessage += string.Format("<br/> Jumlah Pelayanan Untuk Item {0} harus bulat", master.ItemName1);
                                        result = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                            result = false;
                        }
                    }
                }
                else
                {
                    errMessage = "Tidak bisa menyimpan / mengubah transaksi yang dibuat oleh dokter.";
                    return false;
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
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