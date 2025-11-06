using System;
using System.Collections;
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
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OrderDetailList : BasePageTrx
    {
        protected string defaultParamedicName = string.Empty;
        protected string defaultParamedicID = string.Empty;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            string id = param[1];
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.GENERATE_ORDER_AIO;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.GENERATE_ORDER_AIO;
                case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.GENERATE_ORDER_AIO;
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.GENERATE_ORDER_AIO;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.GENERATE_ORDER_AIO;
                default: return Constant.MenuCode.Outpatient.GENERATE_ORDER_AIO;
            }
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        List<PatientChargesDt> lstEntityDt = new List<PatientChargesDt>();

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                MCUOrderList.Clear();
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnVisitID.Value = param[0];
                hdnDepartmentID.Value = param[1];
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnFromHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                ctlPatientBanner.InitializePatientBanner(entity);
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}') AND HealthcareID = '{5}'",
                                                                                                        Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                                                                                                        Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                                                                                        Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER,
                                                                                                        Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST,
                                                                                                        Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID,
                                                                                                        AppSession.UserLogin.HealthcareID));

                hdnDefaultItemIDMCUPackage.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST).ParameterValue;
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                hdnIsUsingRegistrationParamedicID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID).ParameterValue;
                hdnRegistrationParamedicID.Value = entity.ParamedicID.ToString();
                defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
                defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;

                string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hdnVisitID.Value, Constant.TransactionStatus.OPEN);
                List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);
                string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));

                filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
                List<vItemServiceDt> lstEntity = BusinessLayer.GetvItemServiceDtList(filterExpression);
                List<vItemServiceDt> lstHSU = lstEntity.GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();

                List<vItemServiceDt> lstHSUNew = new List<vItemServiceDt>();
                foreach (vItemServiceDt e in lstHSU)
                {
                    if (e.HealthcareServiceUnitID == null || e.HealthcareServiceUnitID == 0)
                    {
                        e.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                        e.DepartmentID = entity.DepartmentID;
                        e.ServiceUnitCode = entity.ServiceUnitCode;
                        e.ServiceUnitName = entity.ServiceUnitName;
                    }
                    lstHSUNew.Add(e);
                }

                rptHSU.DataSource = lstHSUNew;
                rptHSU.DataBind();

                Methods.SetComboBoxField<vItemServiceDt>(cboServiceUnit, lstHSUNew, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                SaveMCUOrderSessionState(lstEntity);
                hdnHealthcareServiceUnitIDFrom.Value = "";
                hdnHealthcareServiceUnitIDTo.Value = lstHSU[0].HealthcareServiceUnitID.ToString();

                ((PatientManagementTransactionTestOrderCtl2)ctlOrder).InitializeControl(hdnFromHealthcareServiceUnitID.Value);

                BindGridView();
            }
        }

        private void BindGridView()
        {
            if (!string.IsNullOrEmpty(hdnHealthcareServiceUnitIDFrom.Value))
            {
                UpdateMCUOrderSessionState();
            }
            List<MCUOrder> lstEntityTemp = MCUOrderList.Where(t => t.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitIDTo.Value)).ToList();
            hdnNumberOfItems.Value = lstEntityTemp.Count.ToString();
            lvwView.Items.Clear();
            lvwView.DataSource = lstEntityTemp;
            lvwView.DataBind();

        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                MCUOrder entity = e.Item.DataItem as MCUOrder;

                CheckBox chkIsConfirm = (CheckBox)e.Item.FindControl("chkIsConfirm");
                chkIsConfirm.Checked = entity.IsConfirm;

                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                HtmlInputHidden hdnParamedicID = (HtmlInputHidden)e.Item.FindControl("hdnParamedicID");
                HtmlInputHidden hdnDetailItemID = (HtmlInputHidden)e.Item.FindControl("hdnDetailItemID");
                HtmlGenericControl lblParamedicName = (HtmlGenericControl)e.Item.FindControl("lblParamedicName");

                hdnKey.Value = entity.Key.ToString();
                lblParamedicName.InnerText = entity.ParamedicName;
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                hdnDetailItemID.Value = entity.DetailItemID.ToString();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changeTab")
                {
                    BindGridView();
                    result = "success|refresh";
                }
                else if (param[0] == "generate")
                {
                    UpdateMCUOrderSessionState();
                    GenerateOrder(ref result);
                }
                else if (param[0] == "saveParamedic")
                {
                    UpdateParamedicOrder();
                    BindGridView();
                    result = "success|saveParamedic";
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void UpdateParamedicOrder()
        {
            int hsuID = Convert.ToInt32(cboServiceUnit.Value.ToString());
            int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
            string paramedicCode = hdnParamedicCode.Value;
            string paramedicName = hdnParamedicName.Value;
            List<vItemServiceDt> lstTemp = new List<vItemServiceDt>();

            string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hdnVisitID.Value, Constant.TransactionStatus.OPEN);
            List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);
            string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));

            filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
            List<vItemServiceDt> lstEntity = BusinessLayer.GetvItemServiceDtList(filterExpression);

            foreach (vItemServiceDt e in lstEntity)
            {
                if (e.HealthcareServiceUnitID == hsuID)
                {
                    e.ParamedicID = paramedicID;
                    e.ParamedicCode = paramedicCode;
                    e.ParamedicName = paramedicName;
                }
                lstTemp.Add(e);
            }

            defaultParamedicID = AppSession.RegisteredPatient.ParamedicID.ToString();
            defaultParamedicName = AppSession.RegisteredPatient.ParamedicName;

            SaveMCUOrderSessionState(lstTemp);
        }

        private void SaveMCUOrderSessionState(List<vItemServiceDt> lstEntity)
        {
            int a = 0;
            MCUOrderList = new List<MCUOrder>();
            foreach (vItemServiceDt entity in lstEntity)
            {
                int paramedicID = 0;
                string paramedicName = "";
                if (!String.IsNullOrEmpty(defaultParamedicID))
                {
                    paramedicID = Convert.ToInt32(defaultParamedicID);
                }

                if (!String.IsNullOrEmpty(defaultParamedicName))
                {
                    paramedicName = defaultParamedicName;
                }

                if (hdnIsUsingRegistrationParamedicID.Value == "0")
                {
                    if (entity.ParamedicID != 0)
                    {
                        paramedicID = entity.ParamedicID;
                        paramedicName = entity.ParamedicName;
                    }
                }
                else
                {
                    ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnRegistrationParamedicID.Value));
                    paramedicID = entityParamedic.ParamedicID;
                    paramedicName = entityParamedic.FullName;
                }

                MCUOrder eMCUOrder = new MCUOrder(entity.DetailItemID, Convert.ToInt32(paramedicID), paramedicName, true);
                eMCUOrder.Key = a;
                eMCUOrder.ItemID = entity.ItemID;
                eMCUOrder.DetailItemID = entity.DetailItemID;
                eMCUOrder.DetailItemCode = entity.DetailItemCode;
                eMCUOrder.DetailItemName1 = entity.DetailItemName1;
                eMCUOrder.HealthcareServiceUnitID = entity.HealthcareServiceUnitID != null && entity.HealthcareServiceUnitID != 0 ? entity.HealthcareServiceUnitID : AppSession.RegisteredPatient.HealthcareServiceUnitID;

                string filterHSU = string.Format("HealthcareServiceUnitID = {0}", eMCUOrder.HealthcareServiceUnitID);
                List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(filterHSU);
                if (lstHSU.Count() > 0)
                {
                    vHealthcareServiceUnit hsu = lstHSU.FirstOrDefault();
                    eMCUOrder.DepartmentID = hsu.DepartmentID;
                    eMCUOrder.DepartmentName = hsu.DepartmentName;
                    eMCUOrder.ServiceUnitName = hsu.ServiceUnitName;
                }
                else
                {
                    eMCUOrder.DepartmentID = entity.DepartmentID;
                    eMCUOrder.DepartmentName = entity.DepartmentName;
                    eMCUOrder.ServiceUnitName = entity.ServiceUnitName;
                }

                eMCUOrder.Quantity = entity.Quantity;
                eMCUOrder.GCItemUnit = entity.GCItemUnit;
                eMCUOrder.DiscountAmount = entity.DiscountAmount;
                eMCUOrder.DiscountAmount1 = entity.DiscountComp1;
                eMCUOrder.DiscountAmount2 = entity.DiscountComp2;
                eMCUOrder.DiscountAmount3 = entity.DiscountComp3;
                eMCUOrder.IsControlAmount = entity.IsControlAmount;
                MCUOrderList.Add(eMCUOrder);
                a++;
            }
        }

        private void UpdateMCUOrderSessionState()
        {
            String[] listKey = hdnListKey.Value.Split('|');
            String[] listDetailItemID = hdnListDetailItemID.Value.Split('|');
            String[] listParamedicID = hdnListParamedicID.Value.Split('|');
            String[] listIsConfirm = hdnListIsConfirm.Value.Split('|');
            string paramParamedic = hdnListParamedicID.Value.Replace('|', ',');
            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN ({0})", paramParamedic));

            for (int a = 0; a < listKey.Length; a++)
            {
                foreach (MCUOrder entity in MCUOrderList)
                {
                    if (entity.Key == Convert.ToInt32(listKey[a]))
                    {
                        entity.DetailItemID = Convert.ToInt32(listDetailItemID[a]);
                        entity.ParamedicID = Convert.ToInt32(listParamedicID[a]);
                        entity.ParamedicName = lstParamedic.Where(t => t.ParamedicID == Convert.ToInt32(listParamedicID[a])).FirstOrDefault().FullName;
                        entity.IsConfirm = listIsConfirm[a] == "1" ? true : false;
                    }
                }
            }
        }

        private void SaveTestOrder(IDbContext ctx, ConsultVisit visit, List<MCUOrder> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            TestOrderHd entityHd = new TestOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.TestOrderDate = dateNow;
            entityHd.TestOrderTime = timeNow;
            entityHd.ScheduledDate = dateNow;
            entityHd.ScheduledTime = timeNow;
            entityHd.IsAIOTransaction = true;
            if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
            {
                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
            }
            else if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
            {
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            }
            else
            {
                entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
            }
            entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHd.TestOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

            #region Patient Charges HD
            //patientChargesHd.VisitID = visit.VisitID;
            //patientChargesHd.LinkedChargesID = null;
            //patientChargesHd.TestOrderID = entityHd.TestOrderID;
            //patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            //patientChargesHd.TransactionDate = DateTime.Now;
            //patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            //patientChargesHd.PatientBillingID = null;
            //patientChargesHd.ReferenceNo = "";
            //patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            //patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
            //patientChargesHd.ProposedDate = DateTime.Now;
            //patientChargesHd.GCVoidReason = null;
            //patientChargesHd.TotalPatientAmount = 0;
            //patientChargesHd.TotalPayerAmount = 0;
            //patientChargesHd.TotalAmount = 0;
            //patientChargesHd.IsAutoTransaction = true;
            //patientChargesHd.IsAIOTransaction = true;
            //patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            //patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            //ctx.CommandType = CommandType.Text;
            //ctx.Command.Parameters.Clear();
            //patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
            #endregion

            #region Patient Charges DT
            foreach (MCUOrder itemServiceDt in lstItemServiceDt)
            {
                TestOrderDt entity = new TestOrderDt();
                entity.TestOrderID = entityHd.TestOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.DiagnoseID = null;
                entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

            //    PatientChargesDt patientChargesDt = new PatientChargesDt();
            //    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
            //    patientChargesDt.ItemID = entity.ItemID;
            //    patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
            //    patientChargesDt.ItemPackageID = entity.ItemPackageID;
            //    patientChargesDt.ReferenceDtID = entity.ID;
            //    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

            //    decimal basePrice = 0;
            //    decimal basePriceComp1 = 0;
            //    decimal basePriceComp2 = 0;
            //    decimal basePriceComp3 = 0;
            //    decimal price = 0;
            //    decimal priceComp1 = 0;
            //    decimal priceComp2 = 0;
            //    decimal priceComp3 = 0;
            //    bool isDiscountUsedComp = false;
            //    decimal discountAmount = 0;
            //    decimal discountAmountComp1 = 0;
            //    decimal discountAmountComp2 = 0;
            //    decimal discountAmountComp3 = 0;
            //    decimal coverageAmount = 0;
            //    bool isDiscountInPercentage = false;
            //    bool isDiscountInPercentageComp1 = false;
            //    bool isDiscountInPercentageComp2 = false;
            //    bool isDiscountInPercentageComp3 = false;
            //    bool isCoverageInPercentage = false;
            //    decimal costAmount = 0;

            //    decimal totalDiscountAmount = 0;
            //    decimal totalDiscountAmount1 = 0;
            //    decimal totalDiscountAmount2 = 0;
            //    decimal totalDiscountAmount3 = 0;

            //    if (list.Count > 0)
            //    {
            //        GetCurrentItemTariff obj = list[0];
            //        basePrice = obj.BasePrice;
            //        basePriceComp1 = obj.BasePriceComp1;
            //        basePriceComp2 = obj.BasePriceComp2;
            //        basePriceComp3 = obj.BasePriceComp3;
            //        price = obj.Price;
            //        priceComp1 = obj.PriceComp1;
            //        priceComp2 = obj.PriceComp2;
            //        priceComp3 = obj.PriceComp3;
            //        isDiscountUsedComp = obj.IsDiscountUsedComp;
            //        discountAmount = obj.DiscountAmount;
            //        discountAmountComp1 = obj.DiscountAmountComp1;
            //        discountAmountComp2 = obj.DiscountAmountComp2;
            //        discountAmountComp3 = obj.DiscountAmountComp3;
            //        coverageAmount = obj.CoverageAmount;
            //        isDiscountInPercentage = obj.IsDiscountInPercentage;
            //        isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
            //        isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
            //        isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
            //        isCoverageInPercentage = obj.IsCoverageInPercentage;
            //        costAmount = obj.CostAmount;
            //    }

            //    patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
            //    patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountAmount1;
            //    patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountAmount2;
            //    patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountAmount3;

            //    patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
            //    patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountAmount1;
            //    patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountAmount2;
            //    patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountAmount3;

            //    ctx.CommandType = CommandType.Text;
            //    ctx.Command.Parameters.Clear();
            //    ItemMaster entityItemMaster = itemMasterDao.Get(entity.ItemID);
            //    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
            //    patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
            //    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
            //    if (patientChargesDt.RevenueSharingID == 0)
            //    {
            //        patientChargesDt.RevenueSharingID = null;
            //    }

            //    patientChargesDt.IsVariable = false;
            //    patientChargesDt.IsUnbilledItem = false;
            //    patientChargesDt.IsControlAmount = itemServiceDt.IsControlAmount;
            //    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;
            //    patientChargesDt.IsCITO = false;
            //    patientChargesDt.CITOAmount = 0;

            //    patientChargesDt.DiscountAmount = 0;
            //    patientChargesDt.DiscountComp1 = 0;
            //    patientChargesDt.DiscountComp2 = 0;
            //    patientChargesDt.DiscountComp3 = 0;

            //    decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
            //    decimal total = grossLineAmount - 0;
            //    decimal totalPayer = 0;
            //    if (isCoverageInPercentage)
            //        totalPayer = total * coverageAmount / 100;
            //    else
            //        totalPayer = coverageAmount * 1;

            //    if (total == 0)
            //    {
            //        totalPayer = total;
            //    }
            //    else
            //    {
            //        if (totalPayer < 0 && totalPayer < total)
            //        {
            //            totalPayer = total;
            //        }
            //        else if (totalPayer > 0 & totalPayer > total)
            //        {
            //            totalPayer = total;
            //        }
            //    }

            //    patientChargesDt.IsComplication = false;
            //    patientChargesDt.ComplicationAmount = 0;
            //    patientChargesDt.IsDiscount = false;
            //    patientChargesDt.PatientAmount = total - totalPayer;
            //    patientChargesDt.PayerAmount = totalPayer;
            //    patientChargesDt.LineAmount = total;
            //    patientChargesDt.IsCreatedBySystem = true;
            //    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            //    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            //    ctx.CommandType = CommandType.Text;
            //    ctx.Command.Parameters.Clear();
            //    lstEntityDt.Add(patientChargesDt);
            //    int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                //string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                //List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                //foreach (vItemServiceDt isd in isdList)
                //{
                //    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                //    dtpackage.PatientChargesDtID = ID;
                //    dtpackage.ItemID = isd.DetailItemID;
                //    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                //    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                //    if (revID != 0 && revID != null)
                //    {
                //        dtpackage.RevenueSharingID = revID;
                //    }
                //    else
                //    {
                //        dtpackage.RevenueSharingID = null;
                //    }

                //    dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                //    ctx.CommandType = CommandType.Text;
                //    ctx.Command.Parameters.Clear();
                //    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                //    basePrice = tariff.BasePrice;
                //    basePriceComp1 = tariff.BasePriceComp1;
                //    basePriceComp2 = tariff.BasePriceComp2;
                //    basePriceComp3 = tariff.BasePriceComp3;
                //    price = tariff.Price;
                //    priceComp1 = tariff.PriceComp1;
                //    priceComp2 = tariff.PriceComp2;
                //    priceComp3 = tariff.PriceComp3;
                //    isDiscountUsedComp = tariff.IsDiscountUsedComp;
                //    discountAmount = tariff.DiscountAmount;
                //    discountAmountComp1 = tariff.DiscountAmountComp1;
                //    discountAmountComp2 = tariff.DiscountAmountComp2;
                //    discountAmountComp3 = tariff.DiscountAmountComp3;
                //    coverageAmount = tariff.CoverageAmount;
                //    isDiscountInPercentage = tariff.IsDiscountInPercentage;
                //    isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                //    isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                //    isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                //    isCoverageInPercentage = tariff.IsCoverageInPercentage;
                //    costAmount = tariff.CostAmount;
                //    grossLineAmount = dtpackage.ChargedQuantity * price;

                //    dtpackage.BaseTariff = tariff.BasePrice;
                //    dtpackage.BaseComp1 = tariff.BasePriceComp1;
                //    dtpackage.BaseComp2 = tariff.BasePriceComp2;
                //    dtpackage.BaseComp3 = tariff.BasePriceComp3;
                //    dtpackage.Tariff = tariff.Price;
                //    dtpackage.TariffComp1 = tariff.PriceComp1;
                //    dtpackage.TariffComp2 = tariff.PriceComp2;
                //    dtpackage.TariffComp3 = tariff.PriceComp3;
                //    dtpackage.CostAmount = tariff.CostAmount;

                //    if (isDiscountInPercentage)
                //    {
                //        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                //        if (isDiscountUsedComp)
                //        {
                //            if (priceComp1 > 0)
                //            {
                //                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                //                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                //            }

                //            if (priceComp2 > 0)
                //            {
                //                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                //                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                //            }

                //            if (priceComp3 > 0)
                //            {
                //                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                //                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                //            }
                //        }
                //        else
                //        {
                //            if (priceComp1 > 0)
                //            {
                //                totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                //                dtpackage.DiscountPercentageComp1 = discountAmount;
                //            }

                //            if (priceComp2 > 0)
                //            {
                //                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                //                dtpackage.DiscountPercentageComp2 = discountAmount;
                //            }

                //            if (priceComp3 > 0)
                //            {
                //                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                //                dtpackage.DiscountPercentageComp3 = discountAmount;
                //            }
                //        }

                //        if (totalDiscountAmount1 > 0)
                //        {
                //            dtpackage.IsDiscountInPercentageComp1 = true;
                //        }

                //        if (totalDiscountAmount2 > 0)
                //        {
                //            dtpackage.IsDiscountInPercentageComp2 = true;
                //        }

                //        if (totalDiscountAmount3 > 0)
                //        {
                //            dtpackage.IsDiscountInPercentageComp3 = true;
                //        }
                //    }
                //    else
                //    {
                //        //totalDiscountAmount = discountAmount * 1;

                //        if (isDiscountUsedComp)
                //        {
                //            if (priceComp1 > 0)
                //                totalDiscountAmount1 = discountAmountComp1;
                //            if (priceComp2 > 0)
                //                totalDiscountAmount2 = discountAmountComp2;
                //            if (priceComp3 > 0)
                //                totalDiscountAmount3 = discountAmountComp3;
                //        }
                //        else
                //        {
                //            if (priceComp1 > 0)
                //                totalDiscountAmount1 = discountAmount;
                //            if (priceComp2 > 0)
                //                totalDiscountAmount2 = discountAmount;
                //            if (priceComp3 > 0)
                //                totalDiscountAmount3 = discountAmount;
                //        }
                //    }

                //    totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                //    if (grossLineAmount >= 0)
                //    {
                //        if (totalDiscountAmount > grossLineAmount)
                //        {
                //            totalDiscountAmount = grossLineAmount;
                //        }
                //    }

                //    dtpackage.DiscountAmount = totalDiscountAmount;
                //    dtpackage.DiscountComp1 = totalDiscountAmount1;
                //    dtpackage.DiscountComp2 = totalDiscountAmount2;
                //    dtpackage.DiscountComp3 = totalDiscountAmount3;

                //    ctx.CommandType = CommandType.Text;
                //    ctx.Command.Parameters.Clear();
                //    string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                //    List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                //    if (iplan.Count() > 0)
                //    {
                //        dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                //    }
                //    else
                //    {
                //        dtpackage.AveragePrice = 0;
                //    }

                //    dtpackage.CreatedBy = AppSession.UserLogin.UserID;

                //    ctx.CommandType = CommandType.Text;
                //    ctx.Command.Parameters.Clear();
                //    patientChargesDtPackageDao.Insert(dtpackage);
                //}

                #endregion

            }
            #endregion
        }

        private void SaveServiceOrder(IDbContext ctx, ConsultVisit visit, List<MCUOrder> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDao = new ServiceOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            #region Patient Charges HD

            PatientChargesHd patientChargesHd = new PatientChargesHd();

            if (lstItemServiceDt[0].DepartmentID != Constant.Facility.INPATIENT)
            {
                ServiceOrderHd entityHd = new ServiceOrderHd();
                entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
                entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
                entityHd.VisitID = visit.VisitID;
                entityHd.ServiceOrderDate = dateNow;
                entityHd.ServiceOrderTime = timeNow;
                entityHd.IsAIOTransaction = true;
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.IP_EMERGENCY_ORDER;
                    }
                    else if (lstItemServiceDt[0].DepartmentID == Constant.Facility.OUTPATIENT)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.IP_OUTPATIENT_ORDER;
                    }
                }
                else if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.OP_EMERGENCY_ORDER;
                    }
                }
                else
                {
                    if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.MCU_EMERGENCY_ORDER;
                    }
                    else
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.MCU_OUTPATIENT_ORDER;
                    }
                }
                entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int serviceOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                patientChargesHd.ServiceOrderID = serviceOrderID;
            }

            //if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
            //{
            //    patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
            //}
            //else if (lstItemServiceDt[0].DepartmentID == Constant.Facility.OUTPATIENT)
            //{
            //    patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
            //}
            //else if (lstItemServiceDt[0].DepartmentID == Constant.Facility.INPATIENT)
            //{
            //    patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES;
            //}

            //patientChargesHd.VisitID = visit.VisitID;
            //patientChargesHd.LinkedChargesID = null;
            //patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            //patientChargesHd.TransactionDate = DateTime.Now;
            //patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            //patientChargesHd.PatientBillingID = null;
            //patientChargesHd.ReferenceNo = "";
            //patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            //patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
            //patientChargesHd.ProposedDate = DateTime.Now;
            //patientChargesHd.GCVoidReason = null;
            //patientChargesHd.TotalPatientAmount = 0;
            //patientChargesHd.TotalPayerAmount = 0;
            //patientChargesHd.TotalAmount = 0;
            //patientChargesHd.IsAutoTransaction = true;
            //patientChargesHd.IsAIOTransaction = true;
            //patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            //patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            //ctx.CommandType = CommandType.Text;
            //ctx.Command.Parameters.Clear();
            //patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

            #endregion

            #region Patient Charges DT
            foreach (MCUOrder itemServiceDt in lstItemServiceDt)
            {
                //PatientChargesDt patientChargesDt = new PatientChargesDt();
                //patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                //patientChargesDt.ItemID = itemServiceDt.DetailItemID;
                //patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                //patientChargesDt.ItemPackageID = itemServiceDt.ItemID;

                if (lstItemServiceDt[0].DepartmentID != Constant.Facility.INPATIENT)
                {
                    ServiceOrderDt entity = new ServiceOrderDt();
                    entity.ServiceOrderID = Convert.ToInt32(patientChargesHd.ServiceOrderID);
                    entity.ItemID = itemServiceDt.DetailItemID;
                    entity.ItemPackageID = itemServiceDt.ItemID;
                    entity.ItemQty = itemServiceDt.Quantity;
                    entity.ItemUnit = itemServiceDt.GCItemUnit;
                    entity.GCServiceOrderStatus = Constant.TestOrderStatus.OPEN;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    int entityID = entityDao.InsertReturnPrimaryKeyID(entity);

                    //patientChargesDt.ReferenceDtID = entityID;
                }

                //List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, patientChargesDt.ItemID, 1, DateTime.Now, ctx);

                //decimal basePrice = 0;
                //decimal basePriceComp1 = 0;
                //decimal basePriceComp2 = 0;
                //decimal basePriceComp3 = 0;
                //decimal price = 0;
                //decimal priceComp1 = 0;
                //decimal priceComp2 = 0;
                //decimal priceComp3 = 0;
                //bool isDiscountUsedComp = false;
                //decimal discountAmount = 0;
                //decimal discountAmountComp1 = 0;
                //decimal discountAmountComp2 = 0;
                //decimal discountAmountComp3 = 0;
                //decimal coverageAmount = 0;
                //bool isDiscountInPercentage = false;
                //bool isDiscountInPercentageComp1 = false;
                //bool isDiscountInPercentageComp2 = false;
                //bool isDiscountInPercentageComp3 = false;
                //bool isCoverageInPercentage = false;
                //decimal costAmount = 0;

                //if (list.Count > 0)
                //{
                //    GetCurrentItemTariff obj = list[0];
                //    basePrice = obj.BasePrice;
                //    basePriceComp1 = obj.BasePriceComp1;
                //    basePriceComp2 = obj.BasePriceComp2;
                //    basePriceComp3 = obj.BasePriceComp3;
                //    price = obj.Price;
                //    priceComp1 = obj.PriceComp1;
                //    priceComp2 = obj.PriceComp2;
                //    priceComp3 = obj.PriceComp3;
                //    isDiscountUsedComp = obj.IsDiscountUsedComp;
                //    discountAmount = obj.DiscountAmount;
                //    discountAmountComp1 = obj.DiscountAmountComp1;
                //    discountAmountComp2 = obj.DiscountAmountComp2;
                //    discountAmountComp3 = obj.DiscountAmountComp3;
                //    coverageAmount = obj.CoverageAmount;
                //    isDiscountInPercentage = obj.IsDiscountInPercentage;
                //    isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                //    isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                //    isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                //    isCoverageInPercentage = obj.IsCoverageInPercentage;
                //    costAmount = obj.CostAmount;
                //}

                //patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                //patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountAmount1;
                //patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountAmount2;
                //patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountAmount3;

                //patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                //patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountAmount1;
                //patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountAmount2;
                //patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountAmount3;

                //ctx.CommandType = CommandType.Text;
                //ctx.Command.Parameters.Clear();
                //ItemMaster entityItemMaster = itemMasterDao.Get(patientChargesDt.ItemID);
                //patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                //patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                //patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                //if (patientChargesDt.RevenueSharingID == 0)
                //{
                //    patientChargesDt.RevenueSharingID = null;
                //}

                //patientChargesDt.IsVariable = false;
                //patientChargesDt.IsUnbilledItem = false;
                //patientChargesDt.IsControlAmount = itemServiceDt.IsControlAmount;
                //patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = itemServiceDt.Quantity;

                //patientChargesDt.DiscountAmount = 0;
                //patientChargesDt.DiscountComp1 = 0;
                //patientChargesDt.DiscountComp2 = 0;
                //patientChargesDt.DiscountComp3 = 0;

                //decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
                //decimal total = grossLineAmount - 0;
                //decimal totalPayer = 0;
                //if (isCoverageInPercentage)
                //    totalPayer = total * coverageAmount / 100;
                //else
                //    totalPayer = coverageAmount * 1;

                //if (total == 0)
                //{
                //    totalPayer = total;
                //}
                //else
                //{
                //    if (totalPayer < 0 && totalPayer < total)
                //    {
                //        totalPayer = total;
                //    }
                //    else if (totalPayer > 0 & totalPayer > total)
                //    {
                //        totalPayer = total;
                //    }
                //}

                //patientChargesDt.PatientAmount = total - totalPayer;
                //patientChargesDt.PayerAmount = totalPayer;
                //patientChargesDt.LineAmount = total;

                //patientChargesDt.IsCITO = false;
                //patientChargesDt.CITOAmount = 0;
                //patientChargesDt.IsComplication = false;
                //patientChargesDt.ComplicationAmount = 0;
                //patientChargesDt.IsDiscount = false;
                //patientChargesDt.IsCreatedBySystem = true;
                //patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                //patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                //ctx.CommandType = CommandType.Text;
                //ctx.Command.Parameters.Clear();
                //lstEntityDt.Add(patientChargesDt);
                //int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                //string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                //List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                //foreach (vItemServiceDt isd in isdList)
                //{
                //    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                //    dtpackage.PatientChargesDtID = ID;
                //    dtpackage.ItemID = isd.DetailItemID;
                //    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                //    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                //    if (revID != 0 && revID != null)
                //    {
                //        dtpackage.RevenueSharingID = revID;
                //    }
                //    else
                //    {
                //        dtpackage.RevenueSharingID = null;
                //    }

                //    dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                //    ctx.CommandType = CommandType.Text;
                //    ctx.Command.Parameters.Clear();
                //    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                //    basePrice = tariff.BasePrice;
                //    basePriceComp1 = tariff.BasePriceComp1;
                //    basePriceComp2 = tariff.BasePriceComp2;
                //    basePriceComp3 = tariff.BasePriceComp3;
                //    price = tariff.Price;
                //    priceComp1 = tariff.PriceComp1;
                //    priceComp2 = tariff.PriceComp2;
                //    priceComp3 = tariff.PriceComp3;
                //    isDiscountUsedComp = tariff.IsDiscountUsedComp;
                //    discountAmount = tariff.DiscountAmount;
                //    discountAmountComp1 = tariff.DiscountAmountComp1;
                //    discountAmountComp2 = tariff.DiscountAmountComp2;
                //    discountAmountComp3 = tariff.DiscountAmountComp3;
                //    coverageAmount = tariff.CoverageAmount;
                //    isDiscountInPercentage = tariff.IsDiscountInPercentage;
                //    isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                //    isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                //    isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                //    isCoverageInPercentage = tariff.IsCoverageInPercentage;
                //    costAmount = tariff.CostAmount;
                //    grossLineAmount = dtpackage.ChargedQuantity * price;

                //    dtpackage.BaseTariff = tariff.BasePrice;
                //    dtpackage.BaseComp1 = tariff.BasePriceComp1;
                //    dtpackage.BaseComp2 = tariff.BasePriceComp2;
                //    dtpackage.BaseComp3 = tariff.BasePriceComp3;
                //    dtpackage.Tariff = tariff.Price;
                //    dtpackage.TariffComp1 = tariff.PriceComp1;
                //    dtpackage.TariffComp2 = tariff.PriceComp2;
                //    dtpackage.TariffComp3 = tariff.PriceComp3;
                //    dtpackage.CostAmount = tariff.CostAmount;

                //    decimal totalDiscountAmount = 0;
                //    decimal totalDiscountAmount1 = 0;
                //    decimal totalDiscountAmount2 = 0;
                //    decimal totalDiscountAmount3 = 0;

                //    if (isDiscountInPercentage)
                //    {
                //        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                //        if (isDiscountUsedComp)
                //        {
                //            if (priceComp1 > 0)
                //            {
                //                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                //                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                //            }

                //            if (priceComp2 > 0)
                //            {
                //                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                //                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                //            }

                //            if (priceComp3 > 0)
                //            {
                //                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                //                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                //            }
                //        }
                //        else
                //        {
                //            if (priceComp1 > 0)
                //            {
                //                totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                //                dtpackage.DiscountPercentageComp1 = discountAmount;
                //            }

                //            if (priceComp2 > 0)
                //            {
                //                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                //                dtpackage.DiscountPercentageComp2 = discountAmount;
                //            }

                //            if (priceComp3 > 0)
                //            {
                //                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                //                dtpackage.DiscountPercentageComp3 = discountAmount;
                //            }
                //        }

                //        if (totalDiscountAmount1 > 0)
                //        {
                //            dtpackage.IsDiscountInPercentageComp1 = true;
                //        }

                //        if (totalDiscountAmount2 > 0)
                //        {
                //            dtpackage.IsDiscountInPercentageComp2 = true;
                //        }

                //        if (totalDiscountAmount3 > 0)
                //        {
                //            dtpackage.IsDiscountInPercentageComp3 = true;
                //        }
                //    }
                //    else
                //    {
                //        //totalDiscountAmount = discountAmount * 1;

                //        if (isDiscountUsedComp)
                //        {
                //            if (priceComp1 > 0)
                //                totalDiscountAmount1 = discountAmountComp1;
                //            if (priceComp2 > 0)
                //                totalDiscountAmount2 = discountAmountComp2;
                //            if (priceComp3 > 0)
                //                totalDiscountAmount3 = discountAmountComp3;
                //        }
                //        else
                //        {
                //            if (priceComp1 > 0)
                //                totalDiscountAmount1 = discountAmount;
                //            if (priceComp2 > 0)
                //                totalDiscountAmount2 = discountAmount;
                //            if (priceComp3 > 0)
                //                totalDiscountAmount3 = discountAmount;
                //        }
                //    }

                //    totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                //    if (grossLineAmount >= 0)
                //    {
                //        if (totalDiscountAmount > grossLineAmount)
                //        {
                //            totalDiscountAmount = grossLineAmount;
                //        }
                //    }

                //    dtpackage.DiscountAmount = totalDiscountAmount;
                //    dtpackage.DiscountComp1 = totalDiscountAmount1;
                //    dtpackage.DiscountComp2 = totalDiscountAmount2;
                //    dtpackage.DiscountComp3 = totalDiscountAmount3;

                //    ctx.CommandType = CommandType.Text;
                //    ctx.Command.Parameters.Clear();
                //    string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                //    List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                //    if (iplan.Count() > 0)
                //    {
                //        dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                //    }
                //    else
                //    {
                //        dtpackage.AveragePrice = 0;
                //    }

                //    dtpackage.CreatedBy = AppSession.UserLogin.UserID;

                //    ctx.CommandType = CommandType.Text;
                //    ctx.Command.Parameters.Clear();
                //    patientChargesDtPackageDao.Insert(dtpackage);
                //}

                #endregion

            }
            #endregion
        }

        //private void InsertMCUDefaultCharges(List<ConsultVisitItemPackage> lstEntityItemPackage, ConsultVisit entityVisit, IDbContext ctx)
        //{
        //    ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
        //    ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
        //    PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
        //    PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

        //    List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
        //    foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
        //    {
        //        ItemService entityItemServicePackage = itemServiceDao.Get(entity.ItemID);
        //        List<GetCurrentItemTariff> listPackage = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, Convert.ToInt32(entityVisit.ChargeClassID), entity.ItemID, 1, DateTime.Now, ctx);
        //        decimal itemPackagePrice = listPackage.FirstOrDefault().Price;
        //        decimal itemPackagePriceComp1 = listPackage.FirstOrDefault().PriceComp1;
        //        decimal itemPackagePriceComp2 = listPackage.FirstOrDefault().PriceComp2;
        //        decimal itemPackagePriceComp3 = listPackage.FirstOrDefault().PriceComp3;

        //        ctx.CommandType = CommandType.Text;
        //        ctx.Command.Parameters.Clear();
        //        string filterPCD = string.Format("ItemPackageID = {0} AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {1} AND GCTransactionStatus != '{2}') AND IsDeleted = 0",
        //                            entity.ItemID, entityVisit.VisitID, Constant.TransactionStatus.VOID);
        //        List<PatientChargesDt> lstPCD = BusinessLayer.GetPatientChargesDtList(filterPCD, ctx);
        //        decimal totalChargesDTBaseTariff = lstPCD.Sum(a => a.BaseTariff);
        //        decimal totalChargesDTBaseComp1 = lstPCD.Sum(a => a.BaseComp1);
        //        decimal totalChargesDTBaseComp2 = lstPCD.Sum(a => a.BaseComp2);
        //        decimal totalChargesDTBaseComp3 = lstPCD.Sum(a => a.BaseComp3);
        //        decimal totalChargesDTTariff = lstPCD.Sum(a => a.Tariff);
        //        decimal totalChargesDTTariffComp1 = lstPCD.Sum(a => a.TariffComp1);
        //        decimal totalChargesDTTariffComp2 = lstPCD.Sum(a => a.TariffComp2);
        //        decimal totalChargesDTTariffComp3 = lstPCD.Sum(a => a.TariffComp3);

        //        if (!entityItemServicePackage.IsUsingAccumulatedPrice)
        //        {
        //            PatientChargesDt patientChargesDt = new PatientChargesDt();
        //            patientChargesDt.ItemID = Convert.ToInt32(hdnDefaultItemIDMCUPackage.Value);
        //            patientChargesDt.ChargeClassID = Convert.ToInt32(entityVisit.ChargeClassID);
        //            patientChargesDt.ItemPackageID = entity.ItemID;
        //            patientChargesDt.ReferenceDtID = entity.ItemID;
        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();

        //            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

        //            decimal basePrice = 0;
        //            decimal basePriceComp1 = 0;
        //            decimal basePriceComp2 = 0;
        //            decimal basePriceComp3 = 0;
        //            decimal price = 0;
        //            decimal priceComp1 = 0;
        //            decimal priceComp2 = 0;
        //            decimal priceComp3 = 0;
        //            bool isDiscountUsedComp = false;
        //            decimal discountAmount = 0;
        //            decimal discountAmountComp1 = 0;
        //            decimal discountAmountComp2 = 0;
        //            decimal discountAmountComp3 = 0;
        //            decimal coverageAmount = 0;
        //            bool isDiscountInPercentage = false;
        //            bool isDiscountInPercentageComp1 = false;
        //            bool isDiscountInPercentageComp2 = false;
        //            bool isDiscountInPercentageComp3 = false;
        //            bool isCoverageInPercentage = false;
        //            decimal costAmount = 0;

        //            if (list.Count > 0)
        //            {
        //                GetCurrentItemTariff obj = list[0];
        //                basePrice = obj.BasePrice;
        //                basePriceComp1 = obj.BasePriceComp1;
        //                basePriceComp2 = obj.BasePriceComp2;
        //                basePriceComp3 = obj.BasePriceComp3;
        //                price = obj.Price;
        //                priceComp1 = obj.PriceComp1;
        //                priceComp2 = obj.PriceComp2;
        //                priceComp3 = obj.PriceComp3;
        //                isDiscountUsedComp = obj.IsDiscountUsedComp;
        //                discountAmount = obj.DiscountAmount;
        //                discountAmountComp1 = obj.DiscountAmountComp1;
        //                discountAmountComp2 = obj.DiscountAmountComp2;
        //                discountAmountComp3 = obj.DiscountAmountComp3;
        //                coverageAmount = obj.CoverageAmount;
        //                isDiscountInPercentage = obj.IsDiscountInPercentage;
        //                isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
        //                isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
        //                isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
        //                isCoverageInPercentage = obj.IsCoverageInPercentage;
        //                costAmount = obj.CostAmount;
        //            }

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            ItemMaster entityItemMaster = itemMasterDao.Get(patientChargesDt.ItemID);
        //            patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
        //            patientChargesDt.ParamedicID = Convert.ToInt32(hdnRegistrationParamedicID.Value);
        //            patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ctx).FirstOrDefault().RevenueSharingID;
        //            if (patientChargesDt.RevenueSharingID == 0)
        //            {
        //                patientChargesDt.RevenueSharingID = null;
        //            }
        //            patientChargesDt.IsVariable = false;
        //            patientChargesDt.IsUnbilledItem = false;
        //            patientChargesDt.IsCITO = false;
        //            patientChargesDt.CITOAmount = 0;

        //            patientChargesDt.IsComplication = false;
        //            patientChargesDt.ComplicationAmount = 0;
        //            patientChargesDt.IsDiscount = false;
        //            patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
        //            patientChargesDt.IsCreatedBySystem = true;
        //            patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

        //            patientChargesDt.BaseTariff = itemPackagePrice - totalChargesDTBaseTariff;
        //            patientChargesDt.BaseComp1 = itemPackagePriceComp1 - totalChargesDTBaseComp1;
        //            patientChargesDt.BaseComp2 = itemPackagePriceComp2 - totalChargesDTBaseComp2;
        //            patientChargesDt.BaseComp3 = itemPackagePriceComp3 - totalChargesDTBaseComp3;

        //            patientChargesDt.Tariff = itemPackagePrice - totalChargesDTTariff;
        //            patientChargesDt.TariffComp1 = itemPackagePriceComp1 - totalChargesDTTariffComp1;
        //            patientChargesDt.TariffComp2 = itemPackagePriceComp2 - totalChargesDTTariffComp2;
        //            patientChargesDt.TariffComp3 = itemPackagePriceComp3 - totalChargesDTTariffComp3;

        //            decimal total = patientChargesDt.Tariff;
        //            decimal totalPayer = 0;
        //            if (isCoverageInPercentage)
        //                totalPayer = total * coverageAmount / 100;
        //            else
        //                totalPayer = coverageAmount * 1;

        //            if (total == 0)
        //            {
        //                totalPayer = total;
        //            }
        //            else
        //            {
        //                if (totalPayer < 0 && totalPayer < total)
        //                {
        //                    totalPayer = total;
        //                }
        //                else if (totalPayer > 0 & totalPayer > total)
        //                {
        //                    totalPayer = total;
        //                }
        //            }

        //            patientChargesDt.PatientAmount = total - totalPayer;
        //            patientChargesDt.PayerAmount = totalPayer;
        //            patientChargesDt.LineAmount = total;

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            lstPatientChargesDt.Add(patientChargesDt);
        //        }
        //    }

        //    if (lstPatientChargesDt.Count > 0)
        //    {
        //        ctx.CommandType = CommandType.Text;
        //        ctx.Command.Parameters.Clear();
        //        PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND IsAutoTransaction = 1 AND HealthcareServiceUnitID = {1} AND GCTransactionStatus <> '{2}'", hdnVisitID.Value, entityVisit.HealthcareServiceUnitID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
        //        if (patientChargesHd == null)
        //        {
        //            patientChargesHd = new PatientChargesHd();
        //            patientChargesHd.VisitID = entityVisit.VisitID;
        //            patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
        //            patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES;
        //            patientChargesHd.TransactionDate = DateTime.Now;
        //            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //            patientChargesHd.PatientBillingID = null;
        //            patientChargesHd.ReferenceNo = "";
        //            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //            patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
        //            patientChargesHd.ProposedDate = DateTime.Now;
        //            patientChargesHd.GCVoidReason = null;
        //            patientChargesHd.IsAutoTransaction = true;
        //            patientChargesHd.IsAIOTransaction = true;
        //            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
        //            patientChargesHd.CreatedBy = 0;
        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
        //        }

        //        foreach (PatientChargesDt entityPatientChargesDt in lstPatientChargesDt)
        //        {
        //            entityPatientChargesDt.TransactionID = patientChargesHd.TransactionID;
        //            entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            patientChargesDtDao.Insert(entityPatientChargesDt);
        //        }
        //    }
        //}

        private void GenerateOrder(ref string result)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
            try
            {
                ConsultVisit entityVisit = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));

                DateTime dateNow = DateTime.Now;
                string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                List<MCUOrder> lstMCUOrder = MCUOrderList.Where(t => t.IsConfirm).GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();                
                foreach (MCUOrder healthcareServiceUnit in lstMCUOrder)
                {
                    List<MCUOrder> lstSelectedItemServiceDt = MCUOrderList.Where(p => p.HealthcareServiceUnitID == healthcareServiceUnit.HealthcareServiceUnitID && p.IsConfirm).ToList();
                    if (healthcareServiceUnit.DepartmentID == Constant.Facility.DIAGNOSTIC)
                    {
                        SaveTestOrder(ctx, entityVisit, lstSelectedItemServiceDt, dateNow, timeNow);
                    }
                    else
                    {
                        SaveServiceOrder(ctx, entityVisit, lstSelectedItemServiceDt, dateNow, timeNow);
                    }
                }

                string filterCVIP = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", entityVisit.VisitID, Constant.TransactionStatus.OPEN);
                List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterCVIP);
                string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));
                foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entity.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    consultVisitItemPackageDao.Update(entity);
                }
                
                //InsertMCUDefaultCharges(lstEntityItemPackage, entityVisit, ctx);

                result = "success|generate";
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }

        public class MCUOrder
        {
            private int _Key;
            private int _ItemID;
            private int _DetailItemID;
            private string _DetailItemCode;
            private string _DetailItemName1;
            private string _ServiceUnitName;
            private string _DepartmentID;
            private string _DepartmentName;
            private int _ParamedicID;
            private int _HealthcareServiceUnitID;
            private string _ParamedicName;
            private bool _IsConfirm;
            private decimal _Quantity;
            private decimal _DiscountAmount;
            private decimal _DiscountAmount1;
            private decimal _DiscountAmount2;
            private decimal _DiscountAmount3;
            private string _GCItemUnit;
            private bool _IsControlAmount;

            public MCUOrder(int detailItemID, int paramedicID, string paramedicName, bool isConfirm)
            {
                _DetailItemID = detailItemID;
                _ParamedicID = paramedicID;
                _ParamedicName = paramedicName;
                _IsConfirm = isConfirm;
            }

            public int Key
            {
                get { return _Key; }
                set { _Key = value; }
            }

            public int ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }

            public int DetailItemID
            {
                get { return _DetailItemID; }
                set { _DetailItemID = value; }
            }

            public string DetailItemCode
            {
                get { return _DetailItemCode; }
                set { _DetailItemCode = value; }
            }

            public string DetailItemName1
            {
                get { return _DetailItemName1; }
                set { _DetailItemName1 = value; }
            }

            public string DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }

            public string ServiceUnitName
            {
                get { return _ServiceUnitName; }
                set { _ServiceUnitName = value; }
            }

            public string DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }

            public int ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }

            public int HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
            }

            public string ParamedicName
            {
                get { return _ParamedicName; }
                set { _ParamedicName = value; }
            }

            public bool IsConfirm
            {
                get { return _IsConfirm; }
                set { _IsConfirm = value; }
            }

            public decimal Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }

            public decimal DiscountAmount
            {
                get { return _DiscountAmount; }
                set { _DiscountAmount = value; }
            }

            public decimal DiscountAmount1
            {
                get { return _DiscountAmount1; }
                set { _DiscountAmount1 = value; }
            }

            public decimal DiscountAmount2
            {
                get { return _DiscountAmount2; }
                set { _DiscountAmount2 = value; }
            }

            public decimal DiscountAmount3
            {
                get { return _DiscountAmount3; }
                set { _DiscountAmount3 = value; }
            }

            public string GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }

            public bool IsControlAmount
            {
                get { return _IsControlAmount; }
                set { _IsControlAmount = value; }
            }
        }

        public List<MCUOrder> MCUOrderList
        {
            get
            {
                if (Session["__MCUOrderList"] == null)
                    Session["__MCUOrderList"] = new List<MCUOrder>();

                return (List<MCUOrder>)Session["__MCUOrderList"];
            }
            set { Session["__MCUOrderList"] = value; }
        }

    }
}