using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SurgeryOrderEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.ORDER_JADWAL_KAMAR_OPERASI;
        }

        #region List
        protected override void InitializeDataControl()
        {

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID = {2} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.MD0006));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            if (Page.Request.QueryString.Count > 0)
            {
                hdnTestOrderID.Value = Page.Request.QueryString["id"];
            }

            if (hdnTestOrderID.Value != "")
            {
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));

                txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTestOrderTime.Text = entity.TestOrderTime;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = entity.ParamedicID.ToString();
                cboServiceUnit.ClientEnabled = false;
                cboServiceUnit.Value = entity.HealthcareServiceUnitID.ToString();
               
                txtPerformDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPerformTime.Text = entity.ScheduledTime;
                
                chkIsCITO.Checked = entity.IsCITO;
                hdnIsProposed.Value = entity.GCTransactionStatus == Constant.TransactionStatus.OPEN ? "0" : "1";
            }
            else
            {
                //hdnTestOrderID.Value = "";
                txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTestOrderTime.Text = DateTime.Now.ToString("HH:mm");
                txtPerformDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPerformTime.Text = "00:00";

                if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }

                cboServiceUnit.SelectedIndex = 0;
                cboServiceUnit.ClientEnabled = false;
                hdnIsProposed.Value = "0";
            }

            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo, AppSession.RegisteredPatient.ServiceUnitName, AppSession.RegisteredPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT));

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                TestOrderDt entity = BusinessLayer.GetTestOrderDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTestOrderDt(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            string filterExpression = "HealthcareServiceUnitID = {HealthcareServiceUnitID}";
            hdnFilterExpressionItemNewTransHd.Value = filterExpression;
            if (hdnTestOrderID.Value != "")
            {
                string tempFilterExpression = filterExpression + string.Format(" AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {0} AND IsDeleted = 0)", hdnTestOrderID.Value);
                hdnFilterExpressionItemAdd.Value = tempFilterExpression;

                tempFilterExpression = filterExpression + string.Format(" AND (ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {0} AND IsDeleted = 0) OR ItemID = {{ItemID}})", hdnTestOrderID.Value);
                hdnFilterExpressionItemEdit.Value = tempFilterExpression;
            }
            else
            {
                string tempFilterExpression = filterExpression + string.Format(" AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {{TestOrderID}} AND IsDeleted = 0)");
                hdnFilterExpressionItemAdd.Value = tempFilterExpression;

                tempFilterExpression = filterExpression + string.Format(" AND (ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {{TestOrderID}} AND IsDeleted = 0) OR ItemID = {{ItemID}})");
                hdnFilterExpressionItemEdit.Value = tempFilterExpression;
            }

            List<PatientDiagnosis> lstDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            Methods.SetComboBoxField<PatientDiagnosis>(cboDiagnose, lstDiagnose, "DiagnosisText", "DiagnoseID");

            SetControlEntrySetting(cboDiagnose, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPerformDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPerformTime, new ControlEntrySetting(true, false, true, "00:00"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(TestOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            if(cboDiagnose.Value != null && cboDiagnose.Value != "")
                entityDt.DiagnoseID = cboDiagnose.Value.ToString();
            entityDt.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            try
            {
                TestOrderHd entityHd = null;
                if (hdnTestOrderID.Value == "")
                {
                    entityHd = new TestOrderHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
                    entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(txtTestOrderDate);
                    entityHd.TestOrderTime = txtTestOrderTime.Text;
                    entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
                    entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtPerformDate.UniqueID]);
                    entityHd.ScheduledTime = Request.Form[txtPerformTime.UniqueID];
                    entityHd.IsCITO = chkIsCITO.Checked;

                    List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
                    string imagingServiceUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                    string laboratoryServiceUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

                    if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(imagingServiceUnitID))
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    else if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(laboratoryServiceUnitID))
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                    else
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;

                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Insert(entityHd);

                    entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);
                    hdnTestOrderID.Value = entityHd.TestOrderID.ToString();
                }
                else
                {
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                }
                retval = entityHd.TestOrderID.ToString();
                hdnTestOrderID.Value = retval;

                TestOrderDt entityDt = new TestOrderDt();
                ControlToEntity(entityDt);
                entityDt.TestOrderID = entityHd.TestOrderID;
                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                entityDt.ItemQty = 1;
                entityDt.ItemUnit = entityItemMasterDao.Get(entityDt.ItemID).GCItemUnit;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                TestOrderHd entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                if (entityHd != null)
                {
                    entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
                    entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtPerformDate.UniqueID]);
                    entityHd.ScheduledTime = Request.Form[txtPerformTime.UniqueID];
                    entityHd.IsCITO = chkIsCITO.Checked;
                    BusinessLayer.UpdateTestOrderHd(entityHd);
                }
                TestOrderDt entity = BusinessLayer.GetTestOrderDt(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTestOrderDt(entity);
                retval = hdnTestOrderID.Value;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = true;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(hdnTestOrderID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", hdnTestOrderID.Value))[0];
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        BusinessLayer.UpdateTestOrderHd(entity);
                    }
                }
                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }
    }
}