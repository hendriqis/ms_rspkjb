using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class GenerateOrderEntry : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.GENERATE_ORDER;
        }

        #region Error Message
        protected string GetErrMessageSelectRegistrationFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_REGISTRATION_FIRST_VALIDATION);
        }
        #endregion

        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}') AND HealthcareID = '{3}'", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER, AppSession.UserLogin.HealthcareID));
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                hdnDefaultMCUParamedicID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                BindGridView();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND GCItemDetailStatus = '{1}' AND GCRegistrationStatus = '{2}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.VisitStatus.CHECKED_IN);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            List<vConsultVisitMCUItem> lstEntity = BusinessLayer.GetvConsultVisitMCUItemList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void SaveTestOrder(IDbContext ctx, vConsultVisitMCUItem registration, vItemServiceDtHealthcareServiceUnit healthcareServiceUnit, List<vItemServiceDtHealthcareServiceUnit> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);


            int defaultParamedicID = Convert.ToInt32(hdnDefaultMCUParamedicID.Value);
            TestOrderHd entityHd = new TestOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.HealthcareServiceUnitID = healthcareServiceUnit.HealthcareServiceUnitID;
            entityHd.ParamedicID = registration.ParamedicID;
            entityHd.VisitID = registration.VisitID;
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
            entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);



            #region Patient Charges
            patientChargesHd.VisitID = registration.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.TestOrderID = entityHd.TestOrderID;
            patientChargesHd.HealthcareServiceUnitID = healthcareServiceUnit.HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

            #endregion

            foreach (vItemServiceDtHealthcareServiceUnit itemServiceDt in lstItemServiceDt)
            {
                TestOrderDt entity = new TestOrderDt();
                entity.TestOrderID = entityHd.TestOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = registration.ItemID;
                entity.DiagnoseID = null;
                entity.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entity.ID = BusinessLayer.GetTestOrderDtMaxID(ctx);
                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(registration.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registration.RegistrationID, registration.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);
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

                patientChargesDt.BaseTariff = patientChargesDt.BaseComp1 = basePrice;
                patientChargesDt.Tariff = patientChargesDt.TariffComp1 = price;

                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                //sini
                if (!string.IsNullOrEmpty(hdnListPhysicianID.Value))
                {
                    int paramedicID = findParamedicID(itemServiceDt.DetailItemID);
                    patientChargesDt.ParamedicID = paramedicID;                    
                }
                else patientChargesDt.ParamedicID = defaultParamedicID;
                //patientChargesDt.BusinessPartnerID = TestDt.BusinessPartnerID;

                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, registration.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                decimal totalDiscountAmount = 0;
                decimal grossLineAmount = 1 * price;

                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;

                if (isDiscountInPercentage)
                    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                else
                    totalDiscountAmount = discountAmount * 1;
                if (totalDiscountAmount > grossLineAmount)
                    totalDiscountAmount = grossLineAmount;

                decimal total = grossLineAmount - totalDiscountAmount;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * 1;
                if (totalPayer > total)
                    totalPayer = total;


                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.DiscountAmount = totalDiscountAmount;
                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                patientChargesDtDao.Insert(patientChargesDt);
            }
        }

        private int findParamedicID(int detailItemID)
        {
            string[] lstDetailItemID = hdnListDetailItemID.Value.Split('|');
            string[] lstParamedicID = hdnListPhysicianID.Value.Split('|');
            int index = Array.FindIndex(lstDetailItemID, w => w == detailItemID.ToString());
            int paramedicID = Convert.ToInt32(lstParamedicID[index]);
            return paramedicID;
        }

        private void SaveServiceOrder(IDbContext ctx, vConsultVisitMCUItem registration, vItemServiceDtHealthcareServiceUnit healthcareServiceUnit, List<vItemServiceDtHealthcareServiceUnit> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDao = new ServiceOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);


            int defaultParamedicID = Convert.ToInt32(hdnDefaultMCUParamedicID.Value);

            ServiceOrderHd entityHd = new ServiceOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.HealthcareServiceUnitID = healthcareServiceUnit.HealthcareServiceUnitID;
            entityHd.ParamedicID = registration.ParamedicID;
            entityHd.VisitID = registration.VisitID;
            entityHd.ServiceOrderDate = dateNow;
            entityHd.ServiceOrderTime = timeNow;
            if (healthcareServiceUnit.DepartmentID == Constant.Facility.EMERGENCY)
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
            entityHdDao.Insert(entityHd);
            entityHd.ServiceOrderID = BusinessLayer.GetServiceOrderHdMaxID(ctx);

            #region Patient Charges

            patientChargesHd.VisitID = registration.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.ServiceOrderID = entityHd.ServiceOrderID;
            patientChargesHd.HealthcareServiceUnitID = healthcareServiceUnit.HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
            #endregion

            foreach (vItemServiceDtHealthcareServiceUnit itemServiceDt in lstItemServiceDt)
            {
                ServiceOrderDt entity = new ServiceOrderDt();
                entity.ServiceOrderID = entityHd.ServiceOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = registration.ItemID;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.GCServiceOrderStatus = Constant.TestOrderStatus.COMPLETED;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = registration.ChargeClassID;
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registration.RegistrationID, registration.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);
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

                patientChargesDt.BaseTariff = basePrice;
                patientChargesDt.Tariff = price;

                //ItemMaster entityItemMaster = itemDao.Get(entity.ItemID);
                //patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                if (!string.IsNullOrEmpty(hdnListPhysicianID.Value))
                {
                    int paramedicID = findParamedicID(itemServiceDt.DetailItemID);
                    patientChargesDt.ParamedicID = paramedicID;
                }
                else patientChargesDt.ParamedicID = defaultParamedicID;

                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, registration.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                decimal totalDiscountAmount = 0;
                decimal grossLineAmount = 1 * price;
                if (isDiscountInPercentage)
                    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                else
                    totalDiscountAmount = discountAmount * 1;
                if (totalDiscountAmount > grossLineAmount)
                    totalDiscountAmount = grossLineAmount;

                decimal total = grossLineAmount - totalDiscountAmount;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * 1;
                if (totalPayer > total)
                    totalPayer = total;

                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;
                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.DiscountAmount = totalDiscountAmount;
                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                patientChargesDtDao.Insert(patientChargesDt);
            }
        }

        protected void cbpRegistrationConfirmation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
            string result = "";
            try
            {
                List<vConsultVisitMCUItem> lstRegistration = BusinessLayer.GetvConsultVisitMCUItemList(string.Format("ID IN ({0})", hdnSelectedRegistration.Value), ctx);
                string lstItemID = String.Join(",", lstRegistration.Select(p => p.ItemID).ToList());
                List<vItemServiceDtHealthcareServiceUnit> lstItemServiceDtAll = BusinessLayer.GetvItemServiceDtHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID), ctx);

                DateTime dateNow = DateTime.Now;
                string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                foreach (vConsultVisitMCUItem registration in lstRegistration)
                {
                    List<vItemServiceDtHealthcareServiceUnit> lstItemServiceDt = lstItemServiceDtAll.Where(p => p.ItemID == registration.ItemID).ToList();
                    List<vItemServiceDtHealthcareServiceUnit> lstHealthcareServiceUnitID = (from p in lstItemServiceDt
                                                                                            select new vItemServiceDtHealthcareServiceUnit { HealthcareServiceUnitID = Convert.ToInt32(p.HealthcareServiceUnitID), DepartmentID = p.DepartmentID }).GroupBy(p => p.HealthcareServiceUnitID).Select(p => p.First()).ToList();

                    foreach (vItemServiceDtHealthcareServiceUnit healthcareServiceUnit in lstHealthcareServiceUnitID)
                    {
                        if (healthcareServiceUnit.DepartmentID == Constant.Facility.DIAGNOSTIC)
                        {
                            List<vItemServiceDtHealthcareServiceUnit> lstSelectedItemServiceDt = lstItemServiceDt.Where(p => p.HealthcareServiceUnitID == healthcareServiceUnit.HealthcareServiceUnitID).ToList();
                            SaveTestOrder(ctx, registration, healthcareServiceUnit, lstSelectedItemServiceDt, dateNow, timeNow);
                        }
                        else
                        {
                            List<vItemServiceDtHealthcareServiceUnit> lstSelectedItemServiceDt = lstItemServiceDt.Where(p => p.HealthcareServiceUnitID == healthcareServiceUnit.HealthcareServiceUnitID).ToList();
                            SaveServiceOrder(ctx, registration, healthcareServiceUnit, lstSelectedItemServiceDt, dateNow, timeNow);
                        }
                    }

                    ConsultVisitItemPackage consultVisitItemPackage = consultVisitItemPackageDao.Get(registration.ID);
                    consultVisitItemPackage.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                    consultVisitItemPackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                    consultVisitItemPackageDao.Update(consultVisitItemPackage);
                }

                result = "success";
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
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}