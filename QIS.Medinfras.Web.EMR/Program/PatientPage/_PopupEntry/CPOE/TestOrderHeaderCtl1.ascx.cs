using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class TestOrderHeaderCtl1 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnTestOrderType.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            hdnHealthcareServiceUnitID.Value = paramInfo[2];
            if (paramInfo.Count() >= 3)
                hdnDiagnosisSummary.Value = paramInfo[3];
            if (paramInfo.Count() >= 4)
                hdnChiefComplaint.Value = paramInfo[4];

            TestOrderHd obj = BusinessLayer.GetTestOrderHdList(string.Format("VisitID = {0} AND TestOrderID = {1}", AppSession.RegisteredPatient.VisitID, hdnTestOrderID.Value)).FirstOrDefault();
            if (obj != null)
            {
                txtOrderDate.Text = obj.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtOrderTime.Text = obj.TestOrderTime;
            }
            else
            {
                txtOrderDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtOrderTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            IsAdd = hdnTestOrderID.Value == "0";
            SetControlProperties(obj);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityOrderHdDao = new TestOrderHdDao(ctx);

            try
            {
                TestOrderHd entity = new TestOrderHd();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.TestOrderNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.TestOrderDate,ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                hdnTestOrderID.Value = entityOrderHdDao.InsertReturnPrimaryKeyID(entity).ToString();

                ctx.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = string.Format("<strong>{0} ({1})</strong><br/><br/><i>{2}</i>", ex.Message, ex.Source, ex.StackTrace);
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                ControlToEntity(entity);
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

        private void ControlToEntity(TestOrderHd entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.TestOrderDate = Helper.GetDatePickerValue(txtOrderDate);
            entity.TestOrderTime = txtOrderTime.Text;
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;

            if (DateTime.Compare(entity.TestOrderDate, DateTime.Now.Date) > 0)
            {
                entity.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
                entity.ScheduledDate = entity.TestOrderDate;
                entity.ScheduledTime = entity.TestOrderTime;
            }
            else
            {
                entity.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                entity.ScheduledDate = entity.TestOrderDate;
                entity.ScheduledTime = entity.TestOrderTime;
            }

            if (hdnTestOrderType.Value == Constant.ItemType.RADIOLOGI)
                entity.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
            else if (hdnTestOrderType.Value == Constant.ItemType.LABORATORIUM)
                entity.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
            else
                entity.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;

            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.Remarks = txtRemarks.Text;
            entity.GCOrderStatus = Constant.OrderStatus.OPEN;
            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            entity.IsCITO = chkIsCITO.Checked;
        }

        private void SetControlProperties(TestOrderHd obj)
        {
            #region Physician Combobox
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = userLoginParamedic.ToString();
            }

            cboPhysician.Enabled = false;
            #endregion

            if (!IsAdd)
            {
                txtRemarks.Text = obj.Remarks;
            }
            else
            {
                txtRemarks.Text = string.IsNullOrEmpty(hdnDiagnosisSummary.Value) ? hdnChiefComplaint.Value : hdnDiagnosisSummary.Value;
            }
        }
    }
}