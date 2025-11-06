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
    public partial class PrescriptionOrderDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.MEDICATION_ORDER;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.MEDICATION_ORDER;
            //    default: return Constant.MenuCode.Outpatient.MEDICATION_ORDER;
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
            }   
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REFILL_INSTRUCTION));
            Methods.SetComboBoxField<StandardCode>(cboRefillInstruction, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboRefillInstruction.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboRefillInstruction, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnPhysicianID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, txtPhysicianCode.Text));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, txtPhysicianName.Text));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        public override void OnAddRecord()
        {
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvPrescriptionOrderHdRowCount(filterExpression);
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHd(filterExpression, PageIndex, " PrescriptionOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvPrescriptionOrderHdRowIndex(filterExpression, keyValue, "PrescriptionOrderID DESC");
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHd(filterExpression, PageIndex, "PrescriptionOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPrescriptionOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            txtPrescriptionOrderNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            cboRefillInstruction.Value = entity.GCRefillInstruction;
            //txtNotes.Text = entity.Notes;
        }
        #endregion

        #region Save Entity
        public void SavePrescriptionOrderHd(IDbContext ctx, ref int prescriptionOrderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            if (hdnPrescriptionOrderID.Value == "0")
            {
                PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate.Text);
                entityHd.PrescriptionTime = txtPrescriptionTime.Text;
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                entityHd.GCPrescriptionType = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                prescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int prescriptionOrderID = 0;
                SavePrescriptionOrderHd(ctx, ref prescriptionOrderID);

                retval = prescriptionOrderID.ToString();
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
                PrescriptionOrderHd entityHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionOrderHd(entityHd);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd entity = entityHdDao.Get(PrescriptionOrderID);
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
                Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(PrescriptionOrderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionOrderHd(entity);
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