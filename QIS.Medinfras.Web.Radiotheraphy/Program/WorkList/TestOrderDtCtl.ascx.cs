using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Imaging.Program.WorkList
{
    public partial class TestOrderDtCtl : BaseViewPopupCtl
    {
        public int PageCount = 1;
        public int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            hdnTestOrderID.Value = param;
            vTestOrderHdVisit entity = BusinessLayer.GetvTestOrderHdVisitList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value)).FirstOrDefault();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnClassID.Value = BusinessLayer.GetRegistration(entity.RegistrationID).ChargeClassID.ToString();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();

            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
            hdnHSULaboratoryID.Value = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue)).FirstOrDefault().HealthcareServiceUnitID.ToString();
            hdnHSUImagingID.Value = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue)).FirstOrDefault().HealthcareServiceUnitID.ToString();
            txtTestOrderDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            txtTestOrderTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            

            BindGridView(CurrPage, true, ref PageCount);
        }


        public void EntitytoControl(vTestOrderHdVisit entity) {
            txtTransactionNo.Text = entity.TestOrderNo;
            txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtTestOrderTime.Text = entity.TestOrderTime.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private string GetFilterExpression()
        {
            //string filterExpression = string.Format("VisitID='{0}' AND TestOrderID='{1}'", hdnVisitID.Value, hdnTestOrderID.Value);
            string filterExpression = string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}'", hdnTestOrderID.Value, Constant.TestOrderStatus.OPEN);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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
                else
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

 
        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "approve")
            {
                if (OnApproveRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "decline")
            {
                if (OnDeclineRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }

            BindGridView(CurrPage, true, ref PageCount);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            TestOrderDtDao TestOrderDtDAO = new TestOrderDtDao(ctx);
            try
            {
                String filterExpressionHd = String.Format("ItemID IN ({0})", hdnParam.Value);

                List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(filterExpressionHd);
                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                foreach (TestOrderDt TestDt in lstTestOrderDt)
                {
                    TestDt.GCTestOrderStatus = Constant.TestOrderStatus.COMPLETED;
                    TestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    TestOrderDtDAO.Update(TestDt);

                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                    patientChargesDt.ItemID = TestDt.ItemID;
                    patientChargesDt.ChargeClassID = Convert.ToInt32(hdnClassID.Value);

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, TestDt.ItemID, 1, DateTime.Now, ctx);
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
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemDao.Get(TestDt.ItemID).GCItemUnit;
                    patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

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

                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    lstPatientChargesDt.Add(patientChargesDt);
                }

                #region Patient Charges

                PatientChargesHd patientChargesHd = new PatientChargesHd();
                patientChargesHd.VisitID = visitID;
                patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnHSUImagingID.Value))
                    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                else if (patientChargesHd.HealthcareServiceUnitID == Convert.ToInt32(hdnHSUImagingID.Value))
                    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                else
                    patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                patientChargesHd.TransactionDate = DateTime.Now;
                patientChargesHd.TransactionTime = DateTime.Now.ToString("HH:mm");
                patientChargesHd.PatientBillingID = null;
                patientChargesHd.ReferenceNo = "";
                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                patientChargesHd.GCVoidReason = null;
                patientChargesHd.TotalPatientAmount = lstPatientChargesDt.Sum(p => p.PatientAmount);
                patientChargesHd.TotalPayerAmount = lstPatientChargesDt.Sum(p => p.PayerAmount);
                patientChargesHd.TotalAmount = patientChargesHd.TotalPatientAmount + patientChargesHd.TotalPayerAmount;
                patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;

                patientChargesHdDao.Insert(patientChargesHd);
                patientChargesHd.TransactionID = BusinessLayer.GetPatientChargesHdMaxID(ctx);
                foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                {
                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                    patientChargesDtDao.Insert(patientChargesDt);
                }
                #endregion
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

        private bool OnDeclineRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            //PatientChargesHdDao HdDAO = new PatientChargesHdDao(ctx);
            //PatientChargesDtDao DtDAO = new PatientChargesDtDao(ctx);
            TestOrderDtDao TestOrderDtDAO = new TestOrderDtDao(ctx);
            try
            {
                String filterExpressionHd = String.Format("ItemID IN ({0})", hdnParam.Value);

                List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(filterExpressionHd);
                foreach (TestOrderDt TestDt in lstTestOrderDt)
                {
                    TestDt.IsDeleted = true;
                    TestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    TestOrderDtDAO.Update(TestDt);
                }
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
    }
}