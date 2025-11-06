using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestOrderDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.TEST_ORDER;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.TEST_ORDER;
            //    default: return Constant.MenuCode.Outpatient.TEST_ORDER;
            //}
            return "";
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

                hdnVisitID.Value = Page.Request.QueryString["id"];
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;
                //txtServiceCode.Attributes.Add("validationgroup", "mpTrxService");

                BindGridView();

                Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboToBePerformed, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtPerformDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            }   
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
                filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnTestOrderID.Value);
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
            Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTestOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtTestOderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTestOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnPhysicianID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, txtPhysicianCode.Text));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, txtPhysicianName.Text));

            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        public override void OnAddRecord()
        {
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHd(filterExpression, PageIndex, " TestOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvTestOrderHdRowIndex(filterExpression, keyValue, "TestOrderID DESC");
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHd(filterExpression, PageIndex, "TestOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTestOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnTestOrderID.Value = entity.TestOrderID.ToString();
            txtTestOderNo.Text = entity.TestOrderNo;
            txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTestOrderTime.Text = entity.TestOrderTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;

            BindGridView();
        }
        #endregion

        #region Save Entity
        public void SaveTestOrderHd(IDbContext ctx, ref int testOrderID)
        {
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            if (hdnTestOrderID.Value == "0")
            {
                TestOrderHd entityHd = new TestOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TestOrderDate = Helper.GetDatePickerValue(txtTestOrderDate.Text);
                entityHd.TestOrderTime = txtTestOrderTime.Text;
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                if (hdnServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                    entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                else if (hdnServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                    entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                testOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);
            }
            else
            {
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int testOrderID = 0;
                SaveTestOrderHd(ctx, ref testOrderID);

                retval = testOrderID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
                //PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                //entity.ReferenceNo = txtReferenceNo.Text;
                //entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
                //entity.TransactionTime = txtTransactionTime.Text;
                //BusinessLayer.UpdatePatientChargesHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
        
        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int testOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref testOrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, testOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTestOrderID"] = testOrderID.ToString();
        }

        private void ControlToEntity(TestOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.DiagnoseID = txtDiagnoseID.Text;
            entityDt.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int testOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                SaveTestOrderHd(ctx, ref testOrderID);
                TestOrderDt entityDt = new TestOrderDt();
                ControlToEntity(entityDt);
                entityDt.TestOrderID = testOrderID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                entityDt.ItemQty = 1;
                entityDt.ItemUnit = hdnGCItemUnit.Value;
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
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
        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.IsDeleted = true;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            try
            {
                Int32 TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = entityHdDao.Get(TestOrderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            try
            {
                Int32 TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(TestOrderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTestOrderHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}