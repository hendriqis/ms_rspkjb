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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChargesLabQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnTransactionIDCtl.Value = hdnParam.Value.Split('|')[0];
            hdnHealthcareServiceUnitID.Value = hdnParam.Value.Split('|')[1];
            hdnVisitIDCtl.Value = hdnParam.Value.Split('|')[2];
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[3];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[4];
            hdnIsAccompany.Value = hdnParam.Value.Split('|')[5];
            hdnTransactionDateCtl.Value = hdnParam.Value.Split('|')[6];
            hdnIsPATest.Value = hdnParam.Value.Split('|')[7];

            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
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

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS, //1
                                                        Constant.SettingParameter.IS_PEMERIKSAAN_RADIOLOGI_HANYA_BPJS, //2
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //3
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsOnlyBPJSLab.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS).FirstOrDefault().ParameterValue;
            hdnIsOnlyBPJSRad.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.IS_PEMERIKSAAN_RADIOLOGI_HANYA_BPJS).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            bool isLimitedCPOEItemForBPJSLab = hdnIsOnlyBPJSLab.Value != null ? (hdnIsOnlyBPJSLab.Value == "1" ? true : false) : false;
            bool isLimitedCPOEItemForBPJSRad = hdnIsOnlyBPJSRad.Value != null ? (hdnIsOnlyBPJSRad.Value == "1" ? true : false) : false;

            string filterHSULB = string.Format("IsLaboratoryUnit = 1 AND HealthcareServiceUnitID = {0}", hdnParam.Value.Split('|')[1]);
            List<vHealthcareServiceUnit> lstHSULB = BusinessLayer.GetvHealthcareServiceUnitList(filterHSULB);
            if (lstHSULB.Count() > 0)
            {
                hdnIsLaboratoryUnit.Value = "1";
            }
            else
            {
                hdnIsLaboratoryUnit.Value = "0";
            }

            Helper.SetControlEntrySetting(txtServicePhysicianCode, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServicePhysicianName, new ControlEntrySetting(false, false, true), "mpTrxService");

            SetControlProperties();
            BindGridView();
        }

        private void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;

            if (hdnParam.Value.Split('|')[1] == hdnLaboratoryServiceUnitID.Value)
                hdnGCItemType.Value = Constant.ItemGroupMaster.LABORATORY;
            else if (hdnParam.Value.Split('|')[1] == hdnImagingServiceUnitID.Value)
                hdnGCItemType.Value = Constant.ItemGroupMaster.RADIOLOGY;
            else
                hdnGCItemType.Value = Constant.ItemGroupMaster.DIAGNOSTIC;

        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void rptDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vServiceUnitItem entityDt = e.Item.DataItem as vServiceUnitItem;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                if (entityDt != null)
                {
                    if (lstSelectedMember.Contains(entityDt.ItemID.ToString()))
                    {
                        int idx = Array.IndexOf(lstSelectedMember, entityDt.ItemID.ToString());
                        chkIsSelected.Checked = true;
                    }
                }
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string medicSupport = "";
            medicSupport += hdnParam.Value.Split('|')[1];

            filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND IsTestItem = 1", medicSupport, hdnFilterItem.Value);

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);
            filterExpression += string.Format(" AND IsPackageAllInOne = 0");

            if (hdnGCItemType.Value == Constant.ItemGroupMaster.LABORATORY)
            {
                if (hdnIsBPJSRegistration.Value == "1")
                {
                    if (hdnIsOnlyBPJSLab.Value == "1")
                    {
                        filterExpression += " AND IsBPJS = 1";
                    }
                }
            }
            else if (hdnGCItemType.Value == Constant.ItemGroupMaster.RADIOLOGY)
            {
                if (hdnIsBPJSRegistration.Value == "1")
                {
                    if (hdnIsOnlyBPJSRad.Value == "1")
                    {
                        filterExpression += " AND IsBPJS = 1";
                    }
                }
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            if (hdnTransactionIDCtl.Value != "0" && hdnTransactionIDCtl.Value != "")
            {
                List<PatientChargesDt> lstItemID = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", hdnTransactionIDCtl.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                foreach (PatientChargesDt itm in lstItemID)
                {
                    lstSelectedID += "," + itm.ItemID;
                }
                if (!lstSelectedID.Equals(string.Empty)) filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
            }

            if (hdnSelectedMember.Value != "")
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
            }

            filterExpression += " ORDER BY GroupOrder";
            lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression);

            List<ItemGroupMaster> lstItemGroupMaster = (from p in lstEntity
                                                        select new ItemGroupMaster { ItemGroupID = p.ItemGroupID, ItemGroupCode = p.ItemGroupCode, ItemGroupName1 = p.ItemGroupName1, DisplayBackColor1 = p.DisplayBackColor1, DisplayBackColor2 = p.DisplayBackColor2, DisplayForeColor = p.DisplayForeColor }).GroupBy(p => p.ItemGroupCode).Select(p => p.First()).ToList();

            rptView.DataSource = lstItemGroupMaster;
            rptView.DataBind();
        }

        protected void rptView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ItemGroupMaster entity = (ItemGroupMaster)e.Item.DataItem;
                HtmlGenericControl divGroupHeader = e.Item.FindControl("divGroupHeader") as HtmlGenericControl;
                HtmlGenericControl divGroupDetail = e.Item.FindControl("divGroupDetail") as HtmlGenericControl;

                if (divGroupHeader != null)
                {
                    if (!string.IsNullOrEmpty(entity.DisplayBackColor1))
                    {
                        divGroupHeader.Style.Add("background-color", entity.DisplayBackColor1);
                        divGroupDetail.Style.Add("background-color", entity.DisplayBackColor2);
                        divGroupHeader.Style.Add("color", entity.DisplayForeColor);
                    }
                    else
                    {
                        divGroupHeader.Style.Add("background-color", "#747d8c");
                        divGroupDetail.Style.Add("background-color", "#dfe4ea");
                        divGroupHeader.Style.Add("color", "#2f3542");
                    }
                }
                else
                {
                    divGroupHeader.Style.Add("background-color", "#747d8c");
                    divGroupDetail.Style.Add("background-color", "#dfe4ea");
                    divGroupHeader.Style.Add("color", "#2f3542");
                }

                DataList rptDetail = (DataList)e.Item.FindControl("rptDetail");
                rptDetail.DataSource = lstEntity.Where(p => p.ItemGroupID == entity.ItemGroupID && p.IsTestItem == true).OrderBy(p => p.PrintOrder).ToList();
                rptDetail.DataBind();
            }
        }

        List<vServiceUnitItem> lstEntity = null;

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtInfoDao entityDtInfoDao = new PatientChargesDtInfoDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);
            bool result = true;

            string[] memberList = hdnSelectedMember.Value.Split(',');
            string member = "";
            for (int i = 0; i < memberList.Length; i++)
            {
                if (!String.IsNullOrEmpty(memberList[i]))
                {
                    if (String.IsNullOrEmpty(member))
                    {
                        member = string.Format("{0}", memberList[i]);
                    }
                    else
                    {
                        member += string.Format(",{0}", memberList[i]);
                    }
                }
            }
            lstSelectedMember = member.Split(',');

            try
            {
                string[] param = hdnParam.Value.Split('|');

                #region Patient Charges Hd

                PatientChargesHd entityHd = null;
                if (hdnTransactionIDCtl.Value == "" || hdnTransactionIDCtl.Value == "0")
                {
                    entityHd = new PatientChargesHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(param[1]);
                    entityHd.VisitID = Convert.ToInt32(param[2]);
                    entityHd.TransactionDate = Helper.GetDatePickerValue(DetailPage.GetTransactionDate());
                    entityHd.IsCorrectionTransaction = DetailPage.GetIsCorrectionTransactionHd() != null ? DetailPage.GetIsCorrectionTransactionHd() : false;
                    entityHd.TransactionTime = DetailPage.GetTransactionTime();
                    entityHd.PatientBillingID = null;
                    entityHd.Remarks = DetailPage.GetRemarksHd() + " " + txtRemarks.Text;
                    entityHd.ReferenceNo = DetailPage.GetReferenceNoHd();
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.GCVoidReason = null;
                    if (hdnIsAccompany.Value == "0")
                    {
                        if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else if (hdnIsLaboratoryUnit.Value == "1")
                            entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                    }
                    else
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES;
                    }
                    entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.TransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                    if (hdnIsLaboratoryUnit.Value == "1") 
                    {
                        PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(entityHd.TransactionID);
                        hdInfo.IsPathologicalAnatomyTest = hdnIsPATest.Value == "1" ? true : false;
                        hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                        hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                        entityHdInfoDao.Update(hdInfo);
                    }
                }
                else
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionIDCtl.Value));

                    if (hdnIsLaboratoryUnit.Value == "1")
                    {
                        PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(entityHd.TransactionID);
                        hdInfo.IsPathologicalAnatomyTest = hdnIsPATest.Value == "1" ? true : false;
                        hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                        hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                        entityHdInfoDao.Update(hdInfo);
                    }
                }
                retval = entityHd.TransactionNo;

                #endregion

                bool isAllowSaveDt = false;
                if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat ditambah detail item lagi.", entityHd.TransactionNo);
                        result = false;
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat ditambah detail item lagi.", entityHd.TransactionNo);
                        result = false;
                    }
                }

                if (isAllowSaveDt)
                {
                    int registrationID = Convert.ToInt32(hdnParam.Value.Split('|')[3]);
                    int visitID = Convert.ToInt32(hdnParam.Value.Split('|')[2]);
                    lstSelectedMember = member.Split(',');

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", member), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        #region Patient Charges Dt

                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = Convert.ToInt32(itemID);
                        patientChargesDt.ChargeClassID = Convert.ToInt32(cboServiceChargeClassIDCtl.Value);

                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, Convert.ToInt32(itemID), 1, DateTime.Now, ctx);
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

                        vItemService itemService = lstItemService.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemService.GCItemUnit;

                        if (hdnServicePhysicianID.Value != "")
                        {
                            patientChargesDt.ParamedicID = Convert.ToInt32(hdnServicePhysicianID.Value);
                        }
                        else
                        {
                            patientChargesDt.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                        }

                        patientChargesDt.IsSubContractItem = itemService.IsSubContractItem;
                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal qty = 1;
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

                        decimal oPatientAmount = total - totalPayer;
                        decimal oPayerAmount = totalPayer;
                        decimal oLineAmount = total;

                        if (hdnIsEndingAmountRoundingTo1.Value == "1")
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

                        patientChargesDt.PatientAmount = oPatientAmount;
                        patientChargesDt.PayerAmount = oPayerAmount;
                        patientChargesDt.LineAmount = oLineAmount;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entity.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(param[2]), entityHd.HealthcareServiceUnitID, entityHd.TransactionDate, entityHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                        
                        if (patientChargesDt.RevenueSharingID == 0)
                            patientChargesDt.RevenueSharingID = null;

                        patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                        patientChargesDt.GCTransactionDetailStatus = entityHd.GCTransactionStatus;
                        patientChargesDt.TransactionID = entityHd.TransactionID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        int oChargesDtID = entityDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                        string filterTeam = string.Format("ParamedicParentID = {0} AND StartDate <= '{1}' AND IsDeleted = 0", patientChargesDt.ParamedicID, DateTime.Now);
                        List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam);
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
                            entityDtParamedicDao.Insert(dtparamedic);
                        }

                        int countInventoryItemDetail = 0;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                        List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
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

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, Convert.ToInt32(param[2]), entityHd.HealthcareServiceUnitID, entityHd.TransactionDate, entityHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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

                            totalDiscountAmount = 0;
                            totalDiscountAmount1 = 0;
                            totalDiscountAmount2 = 0;
                            totalDiscountAmount3 = 0;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, entityHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                            basePrice = tariff.BasePrice;
                            basePriceComp1 = tariff.BasePriceComp1;
                            basePriceComp2 = tariff.BasePriceComp2;
                            basePriceComp3 = tariff.BasePriceComp3;
                            price = tariff.Price;
                            priceComp1 = tariff.PriceComp1;
                            priceComp2 = tariff.PriceComp2;
                            priceComp3 = tariff.PriceComp3;
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

                        if (countInventoryItemDetail > 0)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            HealthcareServiceUnit chargesHSU = chargesHSUDao.Get(Convert.ToInt32(param[1]));

                            PatientChargesDt pcdt = entityDtDao.Get(oChargesDtID);
                            pcdt.LocationID = chargesHSU.LocationID;
                            pcdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtDao.Update(pcdt);
                        }

                        ct++;

                        #endregion
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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