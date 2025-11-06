using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PurchaseRequestApproval : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PURCHASE_REQUEST_APPROVAL;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowNextPrev = IsAllowVoid = false;
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
            List<Location> lstLocation = null;
            if (loc.IsHeader)
                lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
            else
            {
                lstLocation = new List<Location>();
                lstLocation.Add(loc);
            }
            Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
            cboLocation.SelectedIndex = 0;
        }

        protected override void InitializeDataControl()
        {
            List<Location> lstLocation = new List<Location>();
            Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
            cboLocation.SelectedIndex = 0;

            txtTransactionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            //SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));

            //SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            //SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            //SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
        }

        public override void OnAddRecord()
        {
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            
        }

        public override int OnGetRowCount()
        {
            //string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            //return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
            return 0;
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            //string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            //vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            //EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            //string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            //PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            //vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");
            //EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            //if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            //{
            //    isShowWatermark = true;
            //    watermarkText = entity.TransactionStatusWatermark;
            //}
            //hdnTransactionHdID.Value = entity.TransactionID.ToString();
            //txtTransactionNo.Text = entity.TransactionNo;
            //txtReferenceNo.Text = entity.ReferenceNo;
            //txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtTransactionTime.Text = entity.TransactionTime;

            //ctlService.InitializeTransactionControl(entity);
            //ctlDrugMS.InitializeTransactionControl(entity);
            //ctlLogistic.InitializeTransactionControl(entity);
        }
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            //try
            //{
            //    PatientChargesHd entityHd = new PatientChargesHd();
            //    entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
            //    entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            //    entityHd.TransactionCode = Constant.TransactionCode.CHARGES;
            //    entityHd.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            //    entityHd.TransactionTime = txtTransactionTime.Text;
            //    entityHd.PatientBillingID = null;
            //    entityHd.ReferenceNo = txtReferenceNo.Text;
            //    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            //    entityHd.GCVoidReason = null;
            //    entityHd.TotalPatientAmount = 0;
            //    entityHd.TotalPayerAmount = 0;
            //    entityHd.TotalAmount = 0;
            //    entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.HealthcareServiceUnitID, entityHd.TransactionCode, entityHd.TransactionDate, ctx);
            //    ctx.CommandType = CommandType.Text;
            //    ctx.Command.Parameters.Clear();

            //    entityHd.CreatedBy = AppSession.UserLogin.UserID;

            //    entityHdDao.Insert(entityHd);

            //    retval = BusinessLayer.GetPatientChargesHdMaxID(ctx).ToString();
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    ctx.RollBackTransaction();
            //    errMessage = ex.Message;
            //    result = false;
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            //try
            //{
            //    PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
            //    entity.ReferenceNo = txtReferenceNo.Text;
            //    entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            //    entity.TransactionTime = txtTransactionTime.Text;
            //    BusinessLayer.UpdatePatientChargesHd(entity);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    errMessage = ex.Message;
            //    return false;
            //}
            return true;
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            //try
            //{
            //    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
            //    PatientChargesHd entity = entityHdDao.Get(TransactionID);
            //    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
            //    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //    entityHdDao.Update(entity);

            //    ctlService.OnVoidAllChargesDt(ctx, TransactionID);
            //    ctlDrugMS.OnVoidAllChargesDt(ctx, TransactionID);
            //    ctlLogistic.OnVoidAllChargesDt(ctx, TransactionID);

            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    ctx.RollBackTransaction();
            //    errMessage = ex.Message;
            //    result = false;
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            return result;
        }
        #endregion
    }
}