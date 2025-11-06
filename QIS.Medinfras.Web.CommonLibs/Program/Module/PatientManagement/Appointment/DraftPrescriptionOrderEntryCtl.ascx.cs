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
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DraftPrescriptionOrderEntryCtl : BaseEntryPopupCtl2
    {
        protected bool IsEditable = true;

        protected string GetServiceUnitFilterFilterExpression()
        {
            if (hdnDepartmentFromID.Value == "IS")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != '{2}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnImagingServiceUnitID.Value);
            }
            else if (hdnDepartmentFromID.Value == "LB")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != '{2}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnLaboratoryServiceUnitID.Value);
            }
            else if (hdnDepartmentFromID.Value == "MD")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != '{2}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitFromID.Value);
            }
            else
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);
            }
        }

        public override void InitializeDataControl(string param)
        {
            string[] splitParam = param.Split('|');
            hdnAppointmentID.Value = splitParam[0];
            hdnDepartmentFromID.Value = splitParam[1];
            Appointment appointment = BusinessLayer.GetAppointment(Convert.ToInt32(hdnAppointmentID.Value));
            hdnHealthcareServiceUnitFromID.Value = Convert.ToString(appointment.HealthcareServiceUnitID);
            ControlEntryList.Clear();
            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}') AND HealthcareID = '{2}'", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, AppSession.UserLogin.HealthcareID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;

            OnControlEntrySetting();
            ReInitControl();
            SetControlProperties();
            hdnIsAdd.Value = "1";
            hdnIsEditable.Value = "0";
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        protected override void SetControlProperties()
        {
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0 AND IsUsingRegistration = 1"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (hdnDefaultDispensaryServiceUnitID.Value == "0") cboDispensaryUnit.SelectedIndex = 0;
            else
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.REFILL_INSTRUCTION, Constant.StandardCode.COENAM_RULE, Constant.StandardCode.PRESCRIPTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            Helper.SetControlEntrySetting(hdnDrugID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDrugCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboForm, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnGCDosingUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnDraftPrescriptionOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDraftPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDraftPrescriptionOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDraftPrescriptionOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtStatus, new ControlEntrySetting(false, false));
            SetControlEntrySetting(cboDispensaryUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboPrescriptionType, new ControlEntrySetting(true, true, true));
        }

        private void OnLoadEntity(string keyValue)
        {
            string filterExpression = string.Empty;
            if (keyValue.Equals(string.Empty))
            {
                filterExpression = string.Format("DraftPrescriptionOrderNo = '{0}'", txtDraftPrescriptionOrderNo.Text);
            }
            else filterExpression = string.Format("DraftPrescriptionOrderID = {0}", keyValue);
            vDraftPrescriptionOrderHd entity = BusinessLayer.GetvDraftPrescriptionOrderHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        public void LoadPage(string prescriptionOrderID)
        {
            SetControlEnabled(false);
            SetControlProperties();
            OnLoadEntity(prescriptionOrderID);
        }

        protected void cbpMainPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string prescriptionOrderID = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "load")
            {
                LoadPage(string.Empty);
            }
            else if (param[0] == "loadaftersave")
            {
                LoadPage(hdnDraftPrescriptionOrderID.Value);
            }
            else if (param[0] == "new")
            {
                ReInitControl();
                SetControlProperties();
            }
            else if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage, ref prescriptionOrderID))
                {
                    result += "success";
                    LoadPage(prescriptionOrderID);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "proposed")
            {
                if (OnProposeRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftPrescriptionOrderID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftPrescriptionOrderID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = prescriptionOrderID;
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation(false, 0);
        }

        private void BindCboLocation(bool isOnLoad, Int32 locationIDFromEntity)
        {
            string filterExpression = "";
            if (!isOnLoad)
            {
                filterExpression = string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnit.Value);
            }
            else
            {
                filterExpression = string.Format("LocationID = {0}", locationIDFromEntity);
            }

            List<Location> location = BusinessLayer.GetLocationList(filterExpression);

            if (location.Count > 0)
            {
                int locationID = location.FirstOrDefault().LocationID;
                Location loc = BusinessLayer.GetLocation(locationID);
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }

                if (!isOnLoad)
                {
                    hdnIsAllowOverIssued.Value = loc.IsAllowOverIssued ? "1" : "0";
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                }
                else
                {
                    Methods.SetComboBoxField<Location>(cboLocation, location, "LocationName", "LocationID");
                }
            }
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnDrugID.Value != null && hdnDrugID.Value.ToString() != "")
            {
                //List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID IN (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}) OR StandardCodeID IN (SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {1}) AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT, hdnDrugID.Value));
                //Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (TagProperty LIKE '%1%' OR TagProperty LIKE '%PRE%') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT));
                Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
                cboDosingUnit.SelectedIndex = 0;
            }
        }

        #region Save Entity
        public void SaveDraftPrescriptionOrderHd(IDbContext ctx, ref int draftPrescriptionOrderID)
        {
            DraftPrescriptionOrderHdDao entityHdDao = new DraftPrescriptionOrderHdDao(ctx);
            if (hdnDraftPrescriptionOrderID.Value == "0")
            {
                DraftPrescriptionOrderHd entityHd = new DraftPrescriptionOrderHd();
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.AppointmentID = Convert.ToInt32(hdnAppointmentID.Value);
                entityHd.DraftPrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtDraftPrescriptionOrderDate.UniqueID]);
                entityHd.DraftPrescriptionTime = Request.Form[txtDraftPrescriptionOrderTime.UniqueID];
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                if (hdnDepartmentFromID.Value == "OP")
                {
                    entityHd.TransactionCode = Constant.TransactionCode.DRAFT_OP_MEDICATION_ORDER;
                }
                else
                {
                    entityHd.TransactionCode = Constant.TransactionCode.DRAFT_OTHER_MEDICATION_ORDER;
                }

                entityHd.DraftPrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.DraftPrescriptionDate, ctx);
                entityHd.Remarks = txtNotes.Text;
                entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                draftPrescriptionOrderID = BusinessLayer.GetDraftPrescriptionOrderHdMaxID(ctx);
            }
            else
            {
                draftPrescriptionOrderID = Convert.ToInt32(hdnDraftPrescriptionOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityDao = new AppointmentDao(ctx);
            try
            {
                Appointment entity = entityDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (entity.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int draftPrescriptionOrderID = 0;
                    SaveDraftPrescriptionOrderHd(ctx, ref draftPrescriptionOrderID);
                    hdnDraftPrescriptionOrderID.Value = draftPrescriptionOrderID.ToString();
                    retval = draftPrescriptionOrderID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                string x = retval;
                Appointment appointment = BusinessLayer.GetAppointment(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftPrescriptionOrderHd entity = BusinessLayer.GetDraftPrescriptionOrderHd(Convert.ToInt32(hdnDraftPrescriptionOrderID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.Remarks = txtNotes.Text;
                        BusinessLayer.UpdateDraftPrescriptionOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Order " + entity.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPrescriptionOrderHdDao entityHdDao = new DraftPrescriptionOrderHdDao(ctx);
            DraftPrescriptionOrderDtDao entityDtDao = new DraftPrescriptionOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    Int32 DraftPrescriptionOrderID = Convert.ToInt32(hdnDraftPrescriptionOrderID.Value);
                    DraftPrescriptionOrderHd entity = entityHdDao.Get(DraftPrescriptionOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = Constant.DeleteReason.OTHER;
                        entity.VoidReason = "HEADER IS CANCELLED";
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        List<DraftPrescriptionOrderDt> lstDt = BusinessLayer.GetDraftPrescriptionOrderDtList(string.Format("DraftPrescriptionOrderID = {0} AND IsDeleted = 0", DraftPrescriptionOrderID));
                        foreach (DraftPrescriptionOrderDt dt in lstDt)
                        {
                            DraftPrescriptionOrderDt entityDt = entityDtDao.Get(dt.DraftPrescriptionOrderDetailID);
                            entityDt.GCDraftPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                            entityDt.VoidReason = "HEADER IS CANCELLED";
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entity.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
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
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            try
            {
                Appointment appointment = BusinessLayer.GetAppointment(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    Int32 PrescriptionTestOrderID = Convert.ToInt32(hdnDraftPrescriptionOrderID.Value);
                    DraftPrescriptionOrderHd entity = BusinessLayer.GetDraftPrescriptionOrderHd(PrescriptionTestOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        List<DraftPrescriptionOrderDt> lstDt = BusinessLayer.GetDraftPrescriptionOrderDtList(string.Format("DraftPrescriptionOrderID = {0} AND IsDeleted = 0", hdnDraftPrescriptionOrderID.Value));
                        if (lstDt != null)
                        {
                            foreach (DraftPrescriptionOrderDt e in lstDt)
                            {
                                e.GCDraftPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateDraftPrescriptionOrderDt(e);
                            }
                        }
                        BusinessLayer.UpdateDraftPrescriptionOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Order " + entity.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        #region Process Details
        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int prescriptionOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    prescriptionOrderID = Convert.ToInt32(hdnDraftPrescriptionOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref prescriptionOrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                prescriptionOrderID = Convert.ToInt32(hdnDraftPrescriptionOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, prescriptionOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPrescriptionOrderID"] = prescriptionOrderID.ToString();
        }

        private void ControlToEntity(DraftPrescriptionOrderDt entity)
        {
            entity.IsRFlag = chkIsRx.Checked;
            if (hdnDrugID.Value.ToString() != "")
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            else
                entity.ItemID = null;
            entity.DrugName = Request.Form[txtDrugName.UniqueID];
            entity.GenericName = txtGenericName.Text;
            if (hdnSignaID.Value == "" || hdnSignaID.Value == "0")
                entity.SignaID = null;
            else
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            entity.GCDrugForm = cboForm.Value.ToString();
            if (cboCoenamRule.Value != null && cboCoenamRule.Value.ToString() != "")
                entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            else
                entity.GCCoenamRule = null;
            if (hdnGCDoseUnit.Value != "")
            {
                entity.Dose = Convert.ToDecimal(Request.Form[txtStrengthAmount.UniqueID]);
                entity.GCDoseUnit = hdnGCDoseUnit.Value;
            }
            else
            {
                entity.Dose = null;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.ConversionFactor = 1;
            if (hdnGCItemUnit.Value != cboDosingUnit.Value.ToString())
            {
                if (entity.Dose != null)
                {
                    entity.ConversionFactor = 1 / Convert.ToDecimal(entity.Dose);
                }
            }
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            //entity.TakenQty = Convert.ToDecimal(hdnTakenQty.Value) == 0 ? entity.DispenseQty : Convert.ToDecimal(hdnTakenQty.Value);
            entity.TakenQty = entity.DispenseQty;
            entity.ResultQty = entity.TakenQty;
            entity.ChargeQty = entity.TakenQty;
            entity.IsCreatedFromOrder = true;
            entity.GCDraftPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int DraftPrescriptionOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPrescriptionOrderHdDao entityHdDao = new DraftPrescriptionOrderHdDao(ctx);
            DraftPrescriptionOrderDtDao entityDtDao = new DraftPrescriptionOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    SaveDraftPrescriptionOrderHd(ctx, ref DraftPrescriptionOrderID);
                    DraftPrescriptionOrderHd entityHd = entityHdDao.Get(DraftPrescriptionOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        DraftPrescriptionOrderDt entityDt = new DraftPrescriptionOrderDt();
                        ControlToEntity(entityDt);
                        hdnDraftPrescriptionOrderID.Value = DraftPrescriptionOrderID.ToString();
                        entityDt.DraftPrescriptionOrderID = DraftPrescriptionOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPrescriptionOrderHdDao entityHdDao = new DraftPrescriptionOrderHdDao(ctx);
            DraftPrescriptionOrderDtDao entityDtDao = new DraftPrescriptionOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftPrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftPrescriptionOrderHd entityHd = entityHdDao.Get(entityDt.DraftPrescriptionOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        if (!entityDt.IsDeleted)
                        {
                            ControlToEntity(entityDt);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Order " + entityHd.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPrescriptionOrderHdDao entityHdDao = new DraftPrescriptionOrderHdDao(ctx);
            DraftPrescriptionOrderDtDao entityDtDao = new DraftPrescriptionOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftPrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftPrescriptionOrderHd entityHd = entityHdDao.Get(entityDt.DraftPrescriptionOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftPrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Appointment Sudah diproses. Harap Refresh halaman ini.";
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
        #endregion

        private void EntityToControl(vDraftPrescriptionOrderHd entity)
        {
            SetControlProperties();
            BindCboLocation(true, entity.LocationID);
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            cboLocation.Value = entity.LocationID.ToString();
            cboPrescriptionType.Value = entity.GCPrescriptionType.ToString();
            txtStatus.Text = entity.TransactionStatusWatermark;
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == null);

            if (IsEditable)
            {
                divAddDataPrescriptionDraftOrder.Style.Remove("display");
                hdnIsEditable.Value = "1";
            }
            else
            {
                divAddDataPrescriptionDraftOrder.Style.Add("display", "none");
                hdnIsEditable.Value = "0";
            }

            hdnDraftPrescriptionOrderID.Value = entity.DraftPrescriptionOrderID.ToString();
            txtDraftPrescriptionOrderNo.Text = entity.DraftPrescriptionOrderNo;
            txtDraftPrescriptionOrderDate.Text = entity.DraftPrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDraftPrescriptionOrderTime.Text = entity.DraftPrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtNotes.Text = entity.Remarks;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnDraftPrescriptionOrderID.Value != "")
                filterExpression = string.Format("DraftPrescriptionOrderID = {0} AND IsDeleted = 0 AND GCDraftPrescriptionOrderStatus != '{1}' ORDER BY DraftPrescriptionOrderID DESC",
                                                    hdnDraftPrescriptionOrderID.Value, Constant.TransactionStatus.VOID);
            List<vDraftPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvDraftPrescriptionOrderDt1List(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}