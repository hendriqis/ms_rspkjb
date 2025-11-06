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

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCUOrderDetail : BasePageTrx
    {
        protected string defaultParamedicName = string.Empty;
        protected string defaultParamedicID = string.Empty;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.GENERATE_ORDER_NEW;
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
                hdnConsultVisitItemPackageID.Value = param[1];

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnClassID.Value = entity.ChargeClassID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnFromHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                ctlPatientBanner.InitializePatientBanner(entity);

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                string filterexpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //1
                                                            Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //2
                                                            Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER, //3
                                                            Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST, //4
                                                            Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID, //5
                                                            Constant.SettingParameter.MC_GENERATE_ORDER_AUTO_PROPOSED, //6
                                                            Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE, //7
                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //8
                                                        );
                List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(filterexpression);

                hdnDefaultItemIDMCUPackage.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST).ParameterValue;
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                hdnIsUsingRegistrationParamedicID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID).ParameterValue;
                hdnIsGenerateOrderAutoProposed.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_GENERATE_ORDER_AUTO_PROPOSED).ParameterValue;
                hdnRegistrationParamedicID.Value = entity.ParamedicID.ToString();
                hdnIsItemTambahanPropose.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE).ParameterValue;
                hdnIsEndingAmountRoundingTo1.Value = lstSettingParameter.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

                defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
                defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;

                string filterExpression = string.Format("VisitID = '{0}' AND ID = {1} AND IsDeleted = 0 AND GCItemDetailStatus = '{2}'",
                                                                hdnVisitID.Value,
                                                                hdnConsultVisitItemPackageID.Value,
                                                                Constant.TransactionStatus.OPEN
                                                            );
                List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);
                string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));

                hdnMCUPackageListItemID.Value = lstItemPackageID;

                filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
                List<vItemServiceDt> lstEntity = BusinessLayer.GetvItemServiceDtList(filterExpression);
                List<vItemServiceDt> lstHSU = lstEntity.GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();
                rptHSU.DataSource = lstHSU;
                rptHSU.DataBind();

                Methods.SetComboBoxField<vItemServiceDt>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
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
                    List<TestOrderHd> lstTestOrderHD = new List<TestOrderHd>();
                    if (hdnIsItemTambahanPropose.Value == "1")
                    {
                        List<TestOrderHd> lstTestOrder = BusinessLayer.GetTestOrderHdList(string.Format("VisitID='{0}' AND GCTransactionStatus='{1}' AND GCOrderStatus='{2}'", hdnVisitID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.OrderStatus.OPEN));
                        UpdateMCUOrderSessionState();
                        GenerateOrder(ref result, lstTestOrder);
                    }
                    else
                    {
                        UpdateMCUOrderSessionState();
                        GenerateOrder(ref result, lstTestOrderHD);
                    }


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
            int paramedicID = Convert.ToInt32(hdnParamedicIDPerUnit.Value);
            string paramedicCode = hdnParamedicCodePerUnit.Value;
            string ParamedicName = hdnParamedicNamePerUnit.Value;
            List<vItemServiceDt> lstTemp = new List<vItemServiceDt>();

            string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}' AND ID = '{2}'", hdnVisitID.Value, Constant.TransactionStatus.OPEN, hdnConsultVisitItemPackageID.Value);
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
                    e.ParamedicName = ParamedicName;
                }
                else
                {
                    MCUOrder obj2 = MCUOrderList.Where(t => t.DetailItemID == e.DetailItemID && t.HealthcareServiceUnitID == e.HealthcareServiceUnitID).FirstOrDefault();
                    if (obj2 != null)
                    {
                        e.ParamedicID = obj2.ParamedicID;
                        e.ParamedicName = obj2.ParamedicName;
                    }
                }
                lstTemp.Add(e);
            }

            defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
            defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;

            MCUOrderList.Clear();
            SaveMCUOrderSessionStateUpdateParamedic(lstTemp);
        }

        private void SaveMCUOrderSessionState(List<vItemServiceDt> lstEntity)
        {
            int a = 0;
            MCUOrderList = new List<MCUOrder>();
            foreach (vItemServiceDt entity in lstEntity)
            {
                int paramedicID = 0;
                string paramedicName = "";

                ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnRegistrationParamedicID.Value));
                if (hdnIsUsingRegistrationParamedicID.Value == "1")
                {
                    paramedicID = entityParamedic.ParamedicID;
                    paramedicName = entityParamedic.FullName;
                }
                else
                {
                    if (entity.ParamedicID != 0)
                    {
                        paramedicID = entity.ParamedicID;
                        paramedicName = entity.ParamedicName;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(defaultParamedicID))
                        {
                            paramedicID = Convert.ToInt32(defaultParamedicID);
                            paramedicName = defaultParamedicName;
                        }
                        else
                        {
                            paramedicID = entityParamedic.ParamedicID;
                            paramedicName = entityParamedic.FullName;
                        }
                    }
                }

                MCUOrder eMCUOrder = new MCUOrder(entity.DetailItemID, Convert.ToInt32(paramedicID), paramedicName, true);
                eMCUOrder.Key = a;
                eMCUOrder.ItemID = entity.ItemID;
                eMCUOrder.DetailItemID = entity.DetailItemID;
                eMCUOrder.DetailItemCode = entity.DetailItemCode;
                eMCUOrder.DetailItemName1 = entity.DetailItemName1;
                eMCUOrder.ServiceUnitName = entity.ServiceUnitName;
                eMCUOrder.DepartmentID = entity.DepartmentID;
                eMCUOrder.DepartmentName = entity.DepartmentName;
                eMCUOrder.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                eMCUOrder.Quantity = entity.Quantity;
                eMCUOrder.GCItemUnit = entity.GCItemUnit;
                eMCUOrder.DiscountAmount = entity.DiscountAmount;
                eMCUOrder.DiscountAmount1 = entity.DiscountComp1;
                eMCUOrder.DiscountAmount2 = entity.DiscountComp2;
                eMCUOrder.DiscountAmount3 = entity.DiscountComp3;
                MCUOrderList.Add(eMCUOrder);
                a++;
            }
        }

        private void SaveMCUOrderSessionStateUpdateParamedic(List<vItemServiceDt> lstEntity)
        {
            int a = 0;
            MCUOrderList.Clear();
            MCUOrderList = new List<MCUOrder>();
            foreach (vItemServiceDt entity in lstEntity)
            {
                int paramedicID = 0;
                string paramedicName = "";
                paramedicID = entity.ParamedicID;
                paramedicName = entity.ParamedicName;

                MCUOrder eMCUOrder = new MCUOrder(entity.DetailItemID, Convert.ToInt32(paramedicID), paramedicName, true);
                eMCUOrder.Key = a;
                eMCUOrder.ItemID = entity.ItemID;
                eMCUOrder.DetailItemID = entity.DetailItemID;
                eMCUOrder.DetailItemCode = entity.DetailItemCode;
                eMCUOrder.DetailItemName1 = entity.DetailItemName1;
                eMCUOrder.ServiceUnitName = entity.ServiceUnitName;
                eMCUOrder.DepartmentID = entity.DepartmentID;
                eMCUOrder.DepartmentName = entity.DepartmentName;
                eMCUOrder.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                eMCUOrder.Quantity = entity.Quantity;
                eMCUOrder.GCItemUnit = entity.GCItemUnit;
                eMCUOrder.DiscountAmount = entity.DiscountAmount;
                eMCUOrder.DiscountAmount1 = entity.DiscountComp1;
                eMCUOrder.DiscountAmount2 = entity.DiscountComp2;
                eMCUOrder.DiscountAmount3 = entity.DiscountComp3;
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
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            TestOrderHd entityHd = new TestOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.VisitHealthcareServiceUnitID = Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.TestOrderDate = dateNow;
            entityHd.TestOrderTime = timeNow;
            entityHd.ScheduledDate = dateNow;
            entityHd.ScheduledTime = timeNow;
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
            entityHd.IsCreatedBySystem = true;
            entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCOrderStatus = Constant.OrderStatus.IN_PROGRESS;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHd.TestOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

            #region Patient Charges HD
            patientChargesHd.VisitID = visit.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.TestOrderID = entityHd.TestOrderID;
            patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            if (hdnIsGenerateOrderAutoProposed.Value != "1")
            {
                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            }
            else
            {
                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            }
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.IsAutoTransaction = true;
            patientChargesHd.ConsultVisitItemPackageID = Convert.ToInt32(hdnConsultVisitItemPackageID.Value);
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
            #endregion

            #region Patient Charges DT
            foreach (MCUOrder itemServiceDt in lstItemServiceDt)
            {
                TestOrderDt entity = new TestOrderDt();
                entity.TestOrderID = entityHd.TestOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.DiagnoseID = null;
                entity.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

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

                decimal totalDiscountAmount = 0;
                decimal totalDiscountAmount1 = 0;
                decimal totalDiscountAmount2 = 0;
                decimal totalDiscountAmount3 = 0;

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

                patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountAmount3;

                patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountAmount3;

                ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;
                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;
                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;

                // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

                //decimal totalDiscountAmount = 0;
                //if (isDiscountInPercentage)
                //{
                //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                //}
                //else
                //{
                //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                //}

                //if (totalDiscountAmount > grossLineAmount)
                //{
                //    totalDiscountAmount = grossLineAmount;
                //}

                //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

                //var tempDiscountTotal = totalDiscountAmount;
                //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
                //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
                //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

                //if (discountComp1 > priceComp1)
                //{
                //    discountComp1 = priceComp1;
                //}

                //if (discountComp2 > priceComp2)
                //{
                //    discountComp2 = priceComp2;
                //}

                //if (discountComp3 > priceComp3)
                //{
                //    discountComp3 = priceComp3;
                //}

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

                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.IsCreatedBySystem = true;
                if (hdnIsGenerateOrderAutoProposed.Value != "1")
                {
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                }
                else
                {
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                }
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstEntityDt.Add(patientChargesDt);
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

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

                    if (isDiscountInPercentage)
                    {
                        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
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

                        if (totalDiscountAmount1 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
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
                    patientChargesDtPackageDao.Insert(dtpackage);
                }

                #endregion

            }
            #endregion
        }

        private void SaveServiceOrder(IDbContext ctx, ConsultVisit visit, List<MCUOrder> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDao = new ServiceOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            ServiceOrderHd entityHd = new ServiceOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.VisitHealthcareServiceUnitID = Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.ServiceOrderDate = dateNow;
            entityHd.ServiceOrderTime = timeNow;
            if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityHd.TransactionCode = Constant.TransactionCode.MCU_EMERGENCY_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
            }
            else
            {
                entityHd.TransactionCode = Constant.TransactionCode.MCU_OUTPATIENT_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
            }
            entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHd.ServiceOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

            #region Patient Charges HD

            patientChargesHd.VisitID = visit.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.ServiceOrderID = entityHd.ServiceOrderID;
            patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            if (hdnIsGenerateOrderAutoProposed.Value != "1")
            {
                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            }
            else
            {
                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            }
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.IsAutoTransaction = true;
            patientChargesHd.ConsultVisitItemPackageID = Convert.ToInt32(hdnConsultVisitItemPackageID.Value);
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

            #endregion

            #region Patient Charges DT
            foreach (MCUOrder itemServiceDt in lstItemServiceDt)
            {
                ServiceOrderDt entity = new ServiceOrderDt();
                entity.ServiceOrderID = entityHd.ServiceOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.GCServiceOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

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

                patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountAmount3;

                patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountAmount3;

                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;

                // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

                //if (isDiscountInPercentage)
                //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                //else
                //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                //if (totalDiscountAmount > grossLineAmount)
                //    totalDiscountAmount = grossLineAmount;
                //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

                //decimal totalDiscountAmount = 0;
                //var tempDiscountTotal = totalDiscountAmount;
                //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
                //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
                //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

                //if (discountComp1 > priceComp1)
                //{
                //    discountComp1 = priceComp1;
                //}

                //if (discountComp2 > priceComp2)
                //{
                //    discountComp2 = priceComp2;
                //}

                //if (discountComp3 > priceComp3)
                //{
                //    discountComp3 = priceComp3;
                //}

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

                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;
                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.IsCreatedBySystem = true;
                if (hdnIsGenerateOrderAutoProposed.Value != "1")
                {
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                }
                else
                {
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                }
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstEntityDt.Add(patientChargesDt);
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
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
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

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

                    if (isDiscountInPercentage)
                    {
                        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
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

                        if (totalDiscountAmount1 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
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
                    patientChargesDtPackageDao.Insert(dtpackage);
                }

                #endregion

            }
            #endregion
        }

        private void InsertMCUDefaultCharges(List<ConsultVisitItemPackage> lstEntityItemPackage, ConsultVisit entityVisit, IDbContext ctx)
        {
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
            foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
            {
                ItemService entityItemServicePackage = itemServiceDao.Get(entity.ItemID);
                List<GetCurrentItemTariff> listPackage = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, Convert.ToInt32(entityVisit.ChargeClassID), entity.ItemID, 1, DateTime.Now, ctx);
                decimal itemPackagePrice = listPackage.FirstOrDefault().Price;
                decimal itemPackagePriceComp1 = listPackage.FirstOrDefault().PriceComp1;
                decimal itemPackagePriceComp2 = listPackage.FirstOrDefault().PriceComp2;
                decimal itemPackagePriceComp3 = listPackage.FirstOrDefault().PriceComp3;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterPCD = string.Format("ItemPackageID = {0} AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {1} AND GCTransactionStatus != '{2}') AND IsDeleted = 0",
                                    entity.ItemID, entityVisit.VisitID, Constant.TransactionStatus.VOID);
                List<PatientChargesDt> lstPCD = BusinessLayer.GetPatientChargesDtList(filterPCD, ctx);
                decimal totalChargesDTBaseComp1 = lstPCD.Sum(a => ((a.BaseComp1 - a.DiscountComp1) * a.ChargedQuantity));
                decimal totalChargesDTBaseComp2 = lstPCD.Sum(a => ((a.BaseComp2 - a.DiscountComp2) * a.ChargedQuantity));
                decimal totalChargesDTBaseComp3 = lstPCD.Sum(a => ((a.BaseComp3 - a.DiscountComp3) * a.ChargedQuantity));
                decimal totalChargesDTBaseTariff = totalChargesDTBaseComp1 + totalChargesDTBaseComp2 + totalChargesDTBaseComp3;
                decimal totalChargesDTTariffComp1 = lstPCD.Sum(a => ((a.TariffComp1 - a.DiscountComp1) * a.ChargedQuantity));
                decimal totalChargesDTTariffComp2 = lstPCD.Sum(a => ((a.TariffComp2 - a.DiscountComp2) * a.ChargedQuantity));
                decimal totalChargesDTTariffComp3 = lstPCD.Sum(a => ((a.TariffComp3 - a.DiscountComp3) * a.ChargedQuantity));
                decimal totalChargesDTTariff = totalChargesDTTariffComp1 + totalChargesDTTariffComp2 + totalChargesDTTariffComp3;

                if (!entityItemServicePackage.IsUsingAccumulatedPrice)
                {
                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                    patientChargesDt.ItemID = Convert.ToInt32(hdnDefaultItemIDMCUPackage.Value);
                    patientChargesDt.ChargeClassID = Convert.ToInt32(entityVisit.ChargeClassID);
                    patientChargesDt.ItemPackageID = entity.ItemID;
                    patientChargesDt.ReferenceDtID = entity.ItemID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);
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
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", patientChargesDt.ItemID), ctx).FirstOrDefault();
                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                    patientChargesDt.ParamedicID = Convert.ToInt32(hdnRegistrationParamedicID.Value);
                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)).FirstOrDefault().RevenueSharingID;
                    if (patientChargesDt.RevenueSharingID == 0)
                        patientChargesDt.RevenueSharingID = null;
                    patientChargesDt.IsVariable = false;
                    patientChargesDt.IsUnbilledItem = false;
                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;

                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;
                    patientChargesDt.IsDiscount = false;
                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                    patientChargesDt.IsCreatedBySystem = true;
                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

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

                    lstPatientChargesDt.Add(patientChargesDt);
                }
            }

            if (hdnIsGenerateOrderAutoProposed.Value != "1")
            {
                if (lstPatientChargesDt.Count > 0)
                {
                    PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND IsAutoTransaction = 1 AND HealthcareServiceUnitID = {1} AND GCTransactionStatus <> '{2}'", hdnVisitID.Value, entityVisit.HealthcareServiceUnitID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                    if (patientChargesHd == null)
                    {
                        patientChargesHd = new PatientChargesHd();
                        patientChargesHd.VisitID = entityVisit.VisitID;
                        patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                        patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES;
                        patientChargesHd.TransactionDate = DateTime.Now;
                        patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.IsAutoTransaction = true;
                        patientChargesHd.ConsultVisitItemPackageID = Convert.ToInt32(hdnConsultVisitItemPackageID.Value);
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = 0;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    }

                    foreach (PatientChargesDt entityPatientChargesDt in lstPatientChargesDt)
                    {
                        entityPatientChargesDt.TransactionID = patientChargesHd.TransactionID;
                        entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                        patientChargesDtDao.Insert(entityPatientChargesDt);
                    }
                }
                else
                {
                    if (lstPatientChargesDt.Count > 0)
                    {
                        PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND IsAutoTransaction = 1 AND HealthcareServiceUnitID = {1} AND GCTransactionStatus <> '{2}'", hdnVisitID.Value, entityVisit.HealthcareServiceUnitID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                        if (patientChargesHd == null)
                        {
                            patientChargesHd = new PatientChargesHd();
                            patientChargesHd.VisitID = entityVisit.VisitID;
                            patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                            patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES;
                            patientChargesHd.TransactionDate = DateTime.Now;
                            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            patientChargesHd.PatientBillingID = null;
                            patientChargesHd.ReferenceNo = "";
                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            patientChargesHd.GCVoidReason = null;
                            patientChargesHd.IsAutoTransaction = true;
                            patientChargesHd.ConsultVisitItemPackageID = Convert.ToInt32(hdnConsultVisitItemPackageID.Value);
                            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                            patientChargesHd.CreatedBy = 0;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                        }

                        foreach (PatientChargesDt entityPatientChargesDt in lstPatientChargesDt)
                        {
                            entityPatientChargesDt.TransactionID = patientChargesHd.TransactionID;
                            entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            patientChargesDtDao.Insert(entityPatientChargesDt);
                        }
                    }
                }
            }
        }

        private void GenerateOrder(ref string result, List<TestOrderHd> lstTestOrderHDAdditional)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
            ParamedicTeamDao paramedicTeamDao = new ParamedicTeamDao(ctx);
            try
            {
                DateTime dateNow = DateTime.Now;
                string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                List<MCUOrder> lstMCUOrder = MCUOrderList.Where(t => t.IsConfirm).GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();
                ConsultVisit entityVisit = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));
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

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterParamedicTeam = string.Format("RegistrationID = {0} AND IsDeleted = 0", entityVisit.RegistrationID);
                List<ParamedicTeam> lstParamedic = BusinessLayer.GetParamedicTeamList(filterParamedicTeam, ctx);

                var lstMCUOrderParamedicList = MCUOrderList.Where(x => x.IsConfirm).GroupBy(test => test.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
                foreach (MCUOrder e in lstMCUOrderParamedicList)
                {
                    if (lstParamedic.Where(t => t.ParamedicID == e.ParamedicID).Count() <= 0)
                    {
                        if (e.ParamedicID != entityVisit.ParamedicID)
                        {
                            ParamedicTeam entityPar = new ParamedicTeam();
                            entityPar.RegistrationID = entityVisit.RegistrationID;
                            entityPar.ParamedicID = e.ParamedicID;
                            entityPar.GCParamedicRole = Constant.ParamedicRole.KONSULEN;
                            entityPar.CreatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            paramedicTeamDao.Insert(entityPar);
                        }
                    }
                }

                string filterExpression = string.Format("VisitID = '{0}' AND ID = {1} AND IsDeleted = 0 AND GCItemDetailStatus = '{2}'",
                                                                hdnVisitID.Value,
                                                                hdnConsultVisitItemPackageID.Value,
                                                                Constant.TransactionStatus.OPEN
                                                            );
                List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);
                string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));
                foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
                {
                    entity.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    consultVisitItemPackageDao.Update(entity);
                }
                InsertMCUDefaultCharges(lstEntityItemPackage, entityVisit, ctx);


                if (hdnIsItemTambahanPropose.Value == "1")
                {
                    if (lstTestOrderHDAdditional.Count > 0)
                    {
                        OnApproveItemAdditional(ctx, lstTestOrderHDAdditional);
                    }
                }

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
        private bool OnCheckAIOBalance(IDbContext ctx, int regID, ref string errMessage, List<TestOrderHd> lstTestOrderHD)
        {
            bool result = true;

            if (hdnIsHasAIOPackage.Value == "1")
            {

                List<TestOrderDt> lstOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID IN( {0}) AND GCTestOrderStatus != '{1}' AND IsDeleted = 0",
                    lstTestOrderHD.Select(p => p.TestOrderID), Constant.OrderStatus.CANCELLED), ctx);
                foreach (TestOrderDt dt in lstOrderDt)
                {
                    List<vConsultVisitItemPackageBalance> balance = BusinessLayer.GetvConsultVisitItemPackageBalanceList(string.Format("DtHealthcareServiceUnitID = {0} AND RegistrationID = {1} AND DtItemID = {2} AND QuantityEnd > 0", lstTestOrderHD.FirstOrDefault().HealthcareServiceUnitID, regID, dt.ItemID), ctx);
                    if (balance.Count > 0)
                    {
                        result = false;
                        errMessage = string.Format("Item <b>{0} ({1})</b> masih memiliki balance di paket AIO.", balance.FirstOrDefault().DtItemName1, balance.FirstOrDefault().DtItemCode);
                        break;
                    }
                }
            }

            return result;
        }

        public void OnApproveItemAdditional(IDbContext ctx, List<TestOrderHd> lstTestOrderHD)
        {
            bool result = true;
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao patientChargesHdInfoDao = new PatientChargesHdInfoDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            TestOrderDtDao TestOrderDtDao = new TestOrderDtDao(ctx);
            TestOrderHdDao TestOrderHdDao = new TestOrderHdDao(ctx);
            string errMessage = "";

            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            int visitID = Convert.ToInt32(hdnVisitID.Value);
            if (OnCheckAIOBalance(ctx, registrationID, ref errMessage, lstTestOrderHD))
            {
                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                int ct = 0;
                //string[] tempListOrderDt = hdnListTestOrderDtID.Value.Split(',');
                //string[] tempListIsCito = hdnListIsCito.Value.Split(',');
                //string[] tempListTestSupplier = hdnListTestPartnerID.Value.Split(',');
                //string[] tempListParamedicID = hdnListTestParamedicID.Value.Split(',');

                foreach (TestOrderHd orderHD in lstTestOrderHD)
                {
                    List<TestOrderDt> lstOrderdt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID='{0}' AND IsDeleted=0", orderHD.TestOrderID), ctx);

                    foreach (TestOrderDt testDt in lstOrderdt)
                    {
                        #region On Process Order
                        if (testDt.GCTestOrderStatus == Constant.TestOrderStatus.CANCELLED || testDt.GCTestOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                        {
                            result = false;
                            errMessage = "Order sudah diproses oleh user lain, silahkan merefresh halaman ini";
                            break;
                        }

                        //if (tempListTestSupplier[ct] == "" || tempListTestSupplier[ct] == "0")
                        //{
                        //    testDt.BusinessPartnerID = null;
                        //}
                        //else
                        //{
                        //    testDt.BusinessPartnerID = Convert.ToInt32(tempListTestSupplier[ct]);
                        //}
                        testDt.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                        testDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        TestOrderDtDao.Update(testDt);

                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = testDt.ItemID;
                        patientChargesDt.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                        patientChargesDt.ItemPackageID = testDt.ItemPackageID;
                        patientChargesDt.ReferenceDtID = testDt.ID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, testDt.ItemID, 1, DateTime.Now, ctx);

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

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", testDt.ItemID), ctx).FirstOrDefault();
                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                        if (testDt.ParamedicID > 0)
                        {
                            patientChargesDt.ParamedicID = testDt.ParamedicID.GetValueOrDefault();
                        }

                        //if (tempListParamedicID[ct] != "0")
                        //{
                        //    patientChargesDt.ParamedicID = Convert.ToInt32(tempListParamedicID[ct]);
                        //}
                        //else
                        //{
                        //    patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                        //}
                        patientChargesDt.BusinessPartnerID = testDt.BusinessPartnerID;
                        if (patientChargesDt.BusinessPartnerID != null)
                        {
                            patientChargesDt.IsSubContractItem = true;
                        }
                        else
                        {
                            patientChargesDt.IsSubContractItem = false;
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, orderHD.HealthcareServiceUnitID, DateTime.Now.Date, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)).FirstOrDefault().RevenueSharingID;
                        if (patientChargesDt.RevenueSharingID == 0)
                            patientChargesDt.RevenueSharingID = null;
                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal grossLineAmount = testDt.ItemQty * price;

                        patientChargesDt.IsCITO = false;
                        patientChargesDt.CITOAmount = 0;
                        patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                        patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;

                        if (testDt.IsCITO)
                        {
                            patientChargesDt.IsCITO = true;
                            if (entityItemMaster.IsCITOInPercentage)
                                patientChargesDt.CITOAmount = (entityItemMaster.CITOAmount * grossLineAmount) / 100;
                            else
                                patientChargesDt.CITOAmount = entityItemMaster.CITOAmount;
                            grossLineAmount += patientChargesDt.CITOAmount;
                        }

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

                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (testDt.ItemQty);

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
                            totalPayer = coverageAmount * 1;
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

                        patientChargesDt.IsComplication = false;
                        patientChargesDt.ComplicationAmount = 0;

                        patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                        patientChargesDt.DiscountAmount = totalDiscountAmount;
                        patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                        patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                        patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = testDt.ItemQty;

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

                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        lstPatientChargesDt.Add(patientChargesDt);
                        ct++;
                        #endregion
                    }

                    if (result)
                    {
                        #region Patient Charges Hd

                        PatientChargesHd patientChargesHd = null;
                        patientChargesHd = new PatientChargesHd();
                        patientChargesHd.VisitID = visitID;
                        //if (hdnLinkedChargesID.Value != "0" && hdnLinkedChargesID.Value != "")
                        //    patientChargesHd.LinkedChargesID = Convert.ToInt32(hdnLinkedChargesID.Value);
                        //else
                        //    patientChargesHd.LinkedChargesID = null;
                        patientChargesHd.LinkedChargesID = null;
                        patientChargesHd.TestOrderID = orderHD.TestOrderID;
                        patientChargesHd.HealthcareServiceUnitID = orderHD.HealthcareServiceUnitID;
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else
                            patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        patientChargesHd.TransactionDate = DateTime.Now.Date;
                        patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.Remarks = "";
                        if (hdnIsGenerateOrderAutoProposed.Value != "1")
                        {
                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        }
                        else
                        {
                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        }
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

                        //if (hdnIsLaboratoryUnit.Value == "1")
                        //{
                        //    PatientChargesHdInfo hdInfo = patientChargesHdInfoDao.Get(patientChargesHd.TransactionID);
                        //    hdInfo.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
                        //    hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                        //    hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                        //    patientChargesHdInfoDao.Update(hdInfo);
                        //}

                        ///  retval = patientChargesHd.TransactionNo;
                        #endregion

                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                            int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                            List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage, ctx);
                            foreach (vItemServiceDt isd in isdList)
                            {
                                PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                dtpackage.PatientChargesDtID = ID;
                                dtpackage.ItemID = isd.DetailItemID;
                                dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

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

                                decimal totalDiscountAmount = 0;
                                decimal totalDiscountAmount1 = 0;
                                decimal totalDiscountAmount2 = 0;
                                decimal totalDiscountAmount3 = 0;

                                if (isDiscountInPercentage)
                                {
                                    //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                                    if (isDiscountUsedComp)
                                    {
                                        if (priceComp1 > 0)
                                        {
                                            totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                            dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                                        }

                                        if (priceComp2 > 0)
                                        {
                                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                            dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                                        }

                                        if (priceComp3 > 0)
                                        {
                                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                            dtpackage.DiscountPercentageComp3 = discountAmountComp3;
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

                                    if (totalDiscountAmount1 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp1 = true;
                                    }

                                    if (totalDiscountAmount2 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp2 = true;
                                    }

                                    if (totalDiscountAmount3 > 0)
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
                        }


                        int testOrderDtCount = BusinessLayer.GetTestOrderDtRowCount(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}' AND IsDeleted = 0", orderHD.TestOrderID, Constant.TestOrderStatus.OPEN), ctx);
                        TestOrderHd testOrderHd = TestOrderHdDao.Get(orderHD.TestOrderID);
                        if (testOrderDtCount < 1)
                        {
                            testOrderHd.ParamedicID = orderHD.ParamedicID;
                            testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            TestOrderHdDao.Update(testOrderHd);
                        }

                        //if (hdnIsLaboratoryUnit.Value == "1")
                        //{
                        //    testOrderHd.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
                        //    TestOrderHdDao.Update(testOrderHd);
                        //}
                    }


                }

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