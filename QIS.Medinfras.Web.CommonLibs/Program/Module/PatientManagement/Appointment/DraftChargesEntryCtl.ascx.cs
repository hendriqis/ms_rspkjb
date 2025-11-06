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
    public partial class DraftChargesEntryCtl : BaseEntryPopupCtl2
    {
        protected bool IsEditable = true;
        protected bool IsShowParamedicTeam = false;
        protected bool IsShowSwitchIcon = false;

//        string departmentID;

        protected string GetMainParamedicRole()
        {
            return Constant.ParamedicRole.PELAKSANA;
        }

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
            else if (hdnDepartmentFromID.Value == "OP")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
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
            ParamedicMaster paramedic = BusinessLayer.GetParamedicMaster(appointment.ParamedicID);
            hdnPhysicianID.Value = Convert.ToString(appointment.ParamedicID);
            hdnPhysicianCode.Value = paramedic.ParamedicCode;
            hdnPhysicianName.Value = paramedic.FullName;
            hdnGCCustomerType.Value = appointment.GCCustomerType;
            HealthcareServiceUnit healthcareAppointment = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", appointment.HealthcareServiceUnitID)).FirstOrDefault();
            hdnLocationDrugID.Value = Convert.ToString(healthcareAppointment.LocationID);
            hdnLocationLogisticID.Value = Convert.ToString(healthcareAppointment.LogisticLocationID);
            ControlEntryList.Clear();
            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}') AND HealthcareID = '{2}'", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, AppSession.UserLogin.HealthcareID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;

            if (hdnDepartmentFromID.Value == "IS" || hdnDepartmentFromID.Value == "LB" || hdnDepartmentFromID.Value == "MD")
            {
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
            }
            else if (hdnDepartmentFromID.Value == "OP")
            {
                hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
            }
            else
            {
                hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
            }
            string serviceUnitID = "";
            SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
            SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
            serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);

            //string filterHSU = "";
            //filterHSU = string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2})",
            //                        AppSession.UserLogin.HealthcareID, departmentID, serviceUnitID);
            //if (hdnHealthcareServiceUnitFromID.Value != "" && hdnHealthcareServiceUnitFromID.Value != "0")
            //{
            //    filterHSU += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitFromID.Value);
            //}

            string filterHSU = string.Format("HealthcareID = {0} AND HealthcareServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, hdnHealthcareServiceUnitFromID.Value);
            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(filterHSU).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            hdnServiceUnitCodeFrom.Value = hsu.ServiceUnitCode;
            hdnServiceUnitNameFrom.Value = hsu.ServiceUnitName;

            txtServiceUnitCode.Text = hdnServiceUnitCodeFrom.Value;
            txtServiceUnitName.Text = hdnServiceUnitNameFrom.Value;

            hdnServiceItemFilterExpression.Value = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
            hdnLedDrugMSItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            hdnLedLogisticItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType = '{0}' AND IsChargeToPatient = 1 AND IsDeleted = 0", Constant.ItemGroupMaster.LOGISTIC);

            OnControlEntrySetting();
            ReInitControl();
            SetControlProperties();
            BindCboLocationDrug();
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
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField(cboServiceChargeClassID, lstClassCare, "ClassName", "ClassID");
            Methods.SetComboBoxField(cboLogisticChargeClassID, lstClassCare, "ClassName", "ClassID");

            cboServiceChargeClassID.SelectedIndex = 0;
            cboLogisticChargeClassID.SelectedIndex = 0;

            if (!String.IsNullOrEmpty(hdnServiceUnitCodeFrom.Value))
            {
                txtServiceUnitCode.Text = hdnServiceUnitCodeFrom.Value;
            }

            if (!String.IsNullOrEmpty(hdnServiceUnitNameFrom.Value))
            {
                txtServiceUnitName.Text = hdnServiceUnitNameFrom.Value;
            }

            if (!String.IsNullOrEmpty(hdnLocationDrugID.Value))
            {
                BindCboLocationDrug();
            }

            if (!String.IsNullOrEmpty(hdnLocationLogisticID.Value))
            {
                BindCboLocationLogistic();
            }

            Methods.SetComboBoxField(cboDrugMSChargeClassID, lstClassCare, "ClassName", "ClassID");
            cboDrugMSChargeClassID.SelectedIndex = 0;

            Helper.SetControlEntrySetting(txtServiceCITO, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceDiscount, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceComplication, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServicePatient, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServicePayer, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceQty, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceTariff, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceTotal, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceUnitTariff, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServiceItemCode, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(txtServicePhysicianCode, new ControlEntrySetting(true, true, true), "mpTrxService");
            Helper.SetControlEntrySetting(cboServiceChargeClassID, new ControlEntrySetting(true, true, true), "mpTrxService");

            txtStatus.Text = "";
        }

        protected string GetTariffComponent1Text()
        {
            return hdnTariffComp1Text.Value;
        }

        protected string GetTariffComponent2Text()
        {
            return hdnTariffComp2Text.Value;
        }

        protected string GetTariffComponent3Text()
        {
            return hdnTariffComp3Text.Value;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnDraftChargesID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDraftChargesNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDraftChargesDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDraftChargesTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            //            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStatus, new ControlEntrySetting(false, false));
        }

        private void OnLoadEntity(string keyValue)
        {
            string filterExpression = string.Empty;
            if (keyValue.Equals(string.Empty))
            {
                filterExpression = string.Format("DraftTransactionNo = '{0}'", txtDraftChargesNo.Text);
            }
            else filterExpression = string.Format("TransactionID = {0}", keyValue);
            vDraftPatientChargesHd entity = BusinessLayer.GetvDraftPatientChargesHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        public void LoadPage(string transactionID)
        {
            txtDraftChargesDate.Attributes.Add("readonly", "readonly");
            txtDraftChargesTime.Attributes.Add("readonly", "readonly");
            txtStatus.Attributes.Add("readonly", "readonly");

            SetControlEnabled(false);
            SetControlProperties();
            OnLoadEntity(transactionID);
        }

        protected void cbpMainPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string testOrderID = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "load")
            {
                LoadPage(string.Empty);
            }
            else if (param[0] == "loadaftersave")
            {
                LoadPage(hdnDraftChargesID.Value);
            }
            else if (param[0] == "new")
            {
                ReInitControl();
                SetControlProperties();
            }
            else if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage, ref testOrderID))
                {
                    result += "success";
                    LoadPage(testOrderID);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "proposed")
            {
                if (OnProposeRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftChargesID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftChargesID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = testOrderID;
        }

        #region Save Entity
        public void SaveDraftPatientChargesHd(IDbContext ctx, ref int transactionID)
        {
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            if (hdnDraftChargesID.Value == "0")
            {
                DraftPatientChargesHd entityHd = new DraftPatientChargesHd();
                entityHd.AppointmentID = Convert.ToInt32(hdnAppointmentID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IP_CHARGES; break;
                    case Constant.Facility.EMERGENCY: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_ER_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (hdnDepartmentFromID.Value == "IS")
                            entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IMAGING_CHARGES;
                        else
                        {
                            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IMAGING_CHARGES;
                            else
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OP_CHARGES; break;
                }
                entityHd.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtDraftChargesDate.UniqueID]);
                entityHd.DraftTransactionTime = Request.Form[txtDraftChargesTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.Remarks = txtNotes.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.DraftTransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.DraftTransactionCode, entityHd.DraftTransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                //entityHdDao.Insert(entityHd);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
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
                    int draftChargesID = 0;
                    SaveDraftPatientChargesHd(ctx, ref draftChargesID);
                    hdnDraftChargesID.Value = draftChargesID.ToString();
                    retval = draftChargesID.ToString();
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
                    DraftPatientChargesHd entity = BusinessLayer.GetDraftPatientChargesHd(Convert.ToInt32(hdnDraftChargesID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.ReferenceNo = txtReferenceNo.Text;
                        entity.Remarks = txtNotes.Text;
                        entity.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtDraftChargesDate.UniqueID]);
                        entity.DraftTransactionTime = Request.Form[txtDraftChargesTime.UniqueID];
                        BusinessLayer.UpdateDraftPatientChargesHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    Int32 draftChargesID = Convert.ToInt32(hdnDraftChargesID.Value);
                    DraftPatientChargesHd entity = entityHdDao.Get(draftChargesID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = Constant.DeleteReason.OTHER;
                        entity.VoidReason = "HEADER IS CANCELLED";
                        entity.VoidDate = DateTime.Now;
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        List<DraftPatientChargesDt> lstDt = BusinessLayer.GetDraftPatientChargesDtList(string.Format("TransactionID = {0}", draftChargesID));
                        foreach (DraftPatientChargesDt dt in lstDt)
                        {
                            DraftPatientChargesDt entityDt = entityDtDao.Get(dt.ID);
                            if (!entityDt.IsDeleted)
                            {
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                    Int32 draftChargesID = Convert.ToInt32(hdnDraftChargesID.Value);
                    DraftPatientChargesHd entity = BusinessLayer.GetDraftPatientChargesHd(draftChargesID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        List<DraftPatientChargesDt> lstDt = BusinessLayer.GetDraftPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnDraftChargesID.Value));
                        if (lstDt != null)
                        {
                            foreach (DraftPatientChargesDt e in lstDt)
                            {
                                if ((e.LocationID != null && e.LocationID != 0) && !e.IsApproved)
                                {
                                    e.IsApproved = true;
                                }
                                e.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateDraftPatientChargesDt(e);
                            }
                        }
                        BusinessLayer.UpdateDraftPatientChargesHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private void EntityToControl(vDraftPatientChargesHd entity)
        {
            hdnDraftChargesID.Value = Convert.ToString(entity.TransactionID);
            txtStatus.Text = entity.TransactionStatusWatermark;
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == null);
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            //if (IsEditable)
            //{
            //    divAddDataDraftTestOrder.Style.Remove("display");
            //    hdnIsEditable.Value = "1";
            //}
            //else
            //{
            //    divAddDataDraftTestOrder.Style.Add("display", "none");
            //    hdnIsEditable.Value = "0";
            //}
            txtDraftChargesNo.Text = entity.DraftTransactionNo;
            txtDraftChargesDate.Text = entity.DraftTransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDraftChargesTime.Text = entity.DraftTransactionTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtNotes.Text = entity.Remarks;
            BindGridService();
            BindGridDrugMS();
            BindGridLogistic();
        }

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftPatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(ID));
                    DraftPatientChargesHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entityDt.GCDeleteReason = Constant.DeleteReason.OTHER;
                        //                    entityDt.DeleteReason = reason;
                        entityDt.DeleteDate = DateTime.Now;
                        entityDt.DeleteBy = AppSession.UserLogin.UserID;
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Transaksi " + entityHd.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Service
        #region Process Details
        protected void cbpViewServiceCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (OnSaveEditRecordEntityDtService(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDtService(ref errMessage, ref transactionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                if (OnDeleteEntityDt(ref errMessage, Convert.ToInt32(param[1])))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private void ControlToEntityService(DraftPatientChargesDt entity)
        {
            entity.RevenueSharingID = Convert.ToInt32(hdnServiceRevenueSharingID.Value);
            if (entity.RevenueSharingID == 0)
                entity.RevenueSharingID = null;
            entity.ParamedicID = Convert.ToInt32(hdnServicePhysicianID.Value);
            if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
                entity.IsSubContractItem = true;
            }
            else
            {
                entity.BusinessPartnerID = null;
                entity.IsSubContractItem = false;
            }
            entity.ChargeClassID = Convert.ToInt32(cboServiceChargeClassID.Value);
            entity.IsVariable = chkServiceIsVariable.Checked;
            entity.IsUnbilledItem = chkServiceIsUnbilledItem.Checked;
            entity.Tariff = Convert.ToDecimal(Request.Form[txtServiceUnitTariff.UniqueID]);
            entity.IsCITO = chkServiceIsCITO.Checked;
            entity.CITOAmount = Convert.ToDecimal(Request.Form[txtServiceCITO.UniqueID]);
            entity.IsComplication = chkServiceIsComplication.Checked;
            entity.ComplicationAmount = Convert.ToDecimal(Request.Form[txtServiceComplication.UniqueID]);
            entity.IsDiscount = chkServiceIsDiscount.Checked;
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtServiceDiscount.UniqueID]);
            entity.UsedQuantity = entity.BaseQuantity = entity.ChargedQuantity = Convert.ToDecimal(txtServiceQty.Text);
            entity.PatientAmount = Convert.ToDecimal(Request.Form[txtServicePatient.UniqueID]);
            entity.PayerAmount = Convert.ToDecimal(Request.Form[txtServicePayer.UniqueID]);
            entity.LineAmount = Convert.ToDecimal(Request.Form[txtServiceTotal.UniqueID]);
            entity.TariffComp1 = Convert.ToDecimal(Request.Form[txtServiceTariffComp1.UniqueID]);
            entity.TariffComp2 = Convert.ToDecimal(Request.Form[txtServiceTariffComp2.UniqueID]);
            entity.TariffComp3 = Convert.ToDecimal(Request.Form[txtServiceTariffComp3.UniqueID]);
            entity.DiscountComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscComp1.UniqueID]);
            entity.DiscountComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscComp2.UniqueID]);
            entity.DiscountComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscComp3.UniqueID]);
            entity.CITODiscount = Convert.ToDecimal(Request.Form[txtServiceCITODisc.UniqueID]);
            entity.CostAmount = Convert.ToDecimal(Request.Form[txtServiceCostAmount.UniqueID]);
        }

        private bool OnSaveAddRecordEntityDtService(ref string errMessage, ref int draftChargesID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            DraftTestOrderDtDao entityDraftTestOrderDtDao = new DraftTestOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    SaveDraftPatientChargesHd(ctx, ref draftChargesID);
                    DraftPatientChargesHd entityHd = entityHdDao.Get(draftChargesID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        DraftPatientChargesDt entityDt = new DraftPatientChargesDt();
                        ControlToEntityService(entityDt);
                        entityDt.ItemID = Convert.ToInt32(hdnServiceItemID.Value);
                        if (entityHd.DraftTestOrderID != 0 && entityHd.DraftTestOrderID != null)
                        {
                            DraftTestOrderDt entityTestOrderDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityHd.DraftTestOrderID, entityDt.ItemID)).FirstOrDefault();
                            if (entityTestOrderDt != null)
                            {
                                if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
                                {
                                    entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
                                }
                                else entityTestOrderDt.BusinessPartnerID = null;
                                entityDraftTestOrderDtDao.Update(entityTestOrderDt);
                            }
                        }
                        entityDt.BaseTariff = Convert.ToDecimal(hdnServiceBaseTariff.Value);
                        entityDt.BaseComp1 = Convert.ToDecimal(hdnServiceBasePriceComp1.Value);
                        entityDt.BaseComp2 = Convert.ToDecimal(hdnServiceBasePriceComp2.Value);
                        entityDt.BaseComp3 = Convert.ToDecimal(hdnServiceBasePriceComp3.Value);
                        entityDt.GCBaseUnit = entityDt.GCItemUnit = hdnServiceItemUnit.Value;
                        entityDt.IsCITOInPercentage = (hdnServiceIsCITOInPercentage.Value == "1");
                        entityDt.IsComplicationInPercentage = (hdnServiceIsComplicationInPercentage.Value == "1");
                        entityDt.BaseCITOAmount = Convert.ToDecimal(hdnServiceBaseCITOAmount.Value);
                        entityDt.BaseComplicationAmount = Convert.ToDecimal(hdnServiceBaseComplicationAmount.Value);
                        entityDt.TransactionID = draftChargesID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDt.LastUpdatedBy = entityDt.CreatedBy;
                        entityDt.GCTransactionDetailStatus = entityHd.GCTransactionStatus;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Transaksi " + entityHd.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnSaveEditRecordEntityDtService(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            DraftTestOrderDtDao entityDraftTestOrderDtDao = new DraftTestOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftPatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftPatientChargesHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        if (!entityDt.IsDeleted)
                        {
                            ControlToEntityService(entityDt);
                            if (entityHd.DraftTestOrderID != 0 && entityHd.DraftTestOrderID != null)
                            {
                                DraftTestOrderDt entityTestOrderDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityHd.DraftTestOrderID, entityDt.ItemID)).FirstOrDefault();
                                if (entityTestOrderDt != null)
                                {
                                    if (!hdnTestPartnerID.Value.Equals(string.Empty) && hdnTestPartnerID.Value != "0")
                                    {
                                        entityTestOrderDt.BusinessPartnerID = Convert.ToInt32(hdnTestPartnerID.Value);
                                    }
                                    else entityTestOrderDt.BusinessPartnerID = null;
                                    entityDraftTestOrderDtDao.Update(entityTestOrderDt);
                                }
                            }
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi " + entityHd.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Transaksi " + entityHd.DraftTransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnSwitchChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted)
                            {
                                decimal temp = entity.PayerAmount;
                                entity.PayerAmount = entity.PatientAmount;
                                entity.PatientAmount = temp;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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

        private void BindGridService()
        {
            string GCTransactionStatus = hdnGCTransactionStatus.Value;
            DraftPatientChargesHd entity = BusinessLayer.GetDraftPatientChargesHd(Convert.ToInt32(hdnDraftChargesID.Value));

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                IsEditable = true;
                hdnIsEditable.Value = "1";
            }
            else
            {
                IsEditable = false;
                hdnIsEditable.Value = "0";
            }

            string filterExpression = "1 = 0";
            hdnServiceTransactionID.Value = hdnDraftChargesID.Value;
            if (hdnServiceTransactionID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 ORDER BY ID", hdnServiceTransactionID.Value, Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, Constant.ItemGroupMaster.MEDICAL_CHECKUP);
            List<vDraftPatientChargesDt> lst = BusinessLayer.GetvDraftPatientChargesDtList(filterExpression);

            if (hdnHealthcareServiceUnitID.Value == hdnLabHealthcareServiceUnitID.Value)
                IsShowParamedicTeam = false;
            else
                IsShowParamedicTeam = (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC);
            IsShowSwitchIcon = hdnGCCustomerType.Value != Constant.CustomerType.PERSONAL;
            lvwService.DataSource = lst;
            lvwService.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnServiceAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnServiceAllTotalPayer.Value = totalPayerAmount.ToString();
        }
        #endregion

        #region Drug
        protected void cbpDrugMS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnDrugMSTransactionDtID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (OnSaveEditRecordDrug(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordDrug(ref errMessage, ref transactionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "approve")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnApproveChargesDtDrug(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnVoidChargesDtDrug(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                if (OnDeleteChargesDtDrug(ref errMessage, Convert.ToInt32(param[1])))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        protected void cboDrugMSUoM_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnDrugMSItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboDrugMSUoM, lst, "StandardCodeName", "StandardCodeID");
            cboDrugMSUoM.SelectedIndex = -1;
        }

        private void BindCboLocationDrug()
        {
            int locationID = Convert.ToInt32(hdnLocationDrugID.Value);
            if (locationID > 0)
            {
                Location loc = BusinessLayer.GetLocation(locationID);
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                hdnIsAllowOverIssued.Value = loc.IsAllowOverIssued ? "1" : "0";
                Methods.SetComboBoxField<Location>(cboDrugMSLocation, lstLocation, "LocationName", "LocationID");
                cboDrugMSLocation.SelectedIndex = 0;
            }
        }

        private bool OnSwitchChargesDtDrug(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                decimal temp = entity.PayerAmount;
                                entity.PayerAmount = entity.PatientAmount;
                                entity.PatientAmount = temp;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnApproveChargesDtDrug(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsApproved = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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

        private bool OnVoidChargesDtDrug(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && entity.IsApproved)
                            {
                                entity.IsApproved = false;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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

        private bool OnDeleteChargesDtDrug(ref string errMessage, Int32 ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entity.IsDeleted = true;
                                entity.GCDeleteReason = Constant.DeleteReason.OTHER;
                                //entity.DeleteReason = reason;
                                entity.DeleteDate = DateTime.Now;
                                entity.DeleteBy = AppSession.UserLogin.UserID;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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

        private void DrugMSControlToEntity(DraftPatientChargesDt entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.ChargeClassID = Convert.ToInt32(cboDrugMSChargeClassID.Value);
            entity.IsVariable = false;
            entity.TariffComp1 = entity.Tariff = Convert.ToDecimal(Request.Form[txtDrugMSUnitTariff.UniqueID]);
            entity.TariffComp2 = entity.TariffComp3 = 0;
            entity.CostAmount = Convert.ToDecimal(hdnDrugMSCostAmount.Value);
            entity.IsDiscount = false;
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtDrugMSPriceDiscount.UniqueID]);
            entity.GCItemUnit = cboDrugMSUoM.Value.ToString();
            entity.IsCITO = false;
            entity.CITOAmount = 0;
            entity.IsComplication = false;
            entity.ComplicationAmount = 0;
            entity.BaseQuantity = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            entity.UsedQuantity = Convert.ToDecimal(txtDrugMSQtyUsed.Text);
            entity.ChargedQuantity = Convert.ToDecimal(txtDrugMSQtyCharged.Text);
            entity.DiscountComp1 = entity.DiscountAmount / entity.ChargedQuantity;
            entity.PatientAmount = Convert.ToDecimal(Request.Form[txtDrugMSPatient.UniqueID]);
            entity.PayerAmount = Convert.ToDecimal(Request.Form[txtDrugMSPayer.UniqueID]);
            entity.LineAmount = Convert.ToDecimal(Request.Form[txtDrugMSTotal.UniqueID]);
            if (hdnDrugMSConversionValue.Value != "0")
            {
                entity.ConversionFactor = Convert.ToDecimal(hdnDrugMSConversionValue.Value);
            }
            else
            {
                entity.ConversionFactor = 1;
            }
            entity.IsApproved = true;
        }

        private bool OnSaveAddRecordDrug(ref string errMessage, ref int transactionID)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboDrugMSLocation.Value.ToString(), hdnDrugMSItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();
            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;
                if (qtyOnHand >= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
                    DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
                    DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
                    try
                    {
                        Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                        if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                        {
                            SaveDraftPatientChargesHd(ctx, ref transactionID);
                            DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                DraftPatientChargesDt entityDt = new DraftPatientChargesDt();
                                DrugMSControlToEntity(entityDt);
                                entityDt.ItemID = Convert.ToInt32(hdnDrugMSItemID.Value);
                                entityDt.LocationID = Convert.ToInt32(cboDrugMSLocation.Value);
                                entityDt.BaseComp1 = entityDt.BaseTariff = Convert.ToDecimal(hdnDrugMSBaseTariff.Value);
                                entityDt.BaseComp2 = entityDt.BaseComp3 = 0;
                                entityDt.GCBaseUnit = hdnDrugMSDefaultUoM.Value;
                                entityDt.AveragePrice = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault().AveragePrice;
                                entityDt.TransactionID = transactionID;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDt.LastUpdatedBy = entityDt.CreatedBy;
                                entityDt.GCTransactionDetailStatus = entityHd.GCTransactionStatus;
                                entityDtDao.Insert(entityDt);
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboDrugMSUoM.Text, itemQty.ToString("N2"), cboDrugMSUoM.Text);
                }
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboDrugMSLocation.Text);
            }
            return result;
        }

        private bool OnSaveEditRecordDrug(ref string errMessage)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboDrugMSLocation.Value.ToString(), hdnDrugMSItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();
            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;
                if (qtyOnHand >= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
                    DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
                    DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
                    try
                    {
                        Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                        if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                        {
                            int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                            if (transactionID > 0)
                            {
                                DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    DraftPatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDrugMSTransactionDtID.Value));
                                    if (!entityDt.IsDeleted && !entityDt.IsApproved)
                                    {
                                        DrugMSControlToEntity(entityDt);
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDtDao.Update(entityDt);
                                        ctx.CommitTransaction();
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboDrugMSUoM.Text, itemQty.ToString("N2"), cboDrugMSUoM.Text);
                }
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboDrugMSLocation.Text);
            }
            return result;
        }

        private void BindGridDrugMS()
        {
            DraftPatientChargesHd entity = BusinessLayer.GetDraftPatientChargesHd(Convert.ToInt32(hdnDraftChargesID.Value));
            //            oBusinessPartnerID = entity.BusinessPartnerID;
            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                IsEditable = true;
                hdnIsEditable.Value = "1";
            }
            else
            {
                IsEditable = false;
                hdnIsEditable.Value = "0";
            }

            string filterExpression = "1 = 0";
            string transactionID = hdnGCTransactionStatus.Value;
            if (transactionID != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}') AND UsedQuantity > -1 AND IsDeleted = 0", hdnDraftChargesID.Value, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            List<vDraftPatientChargesDt> lst = BusinessLayer.GetvDraftPatientChargesDtList(filterExpression);
            IsShowSwitchIcon = hdnGCCustomerType.Value != Constant.CustomerType.PERSONAL;
            lvwDrugMS.DataSource = lst;
            lvwDrugMS.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnDrugMSAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnDrugMSAllTotalPayer.Value = totalPayerAmount.ToString();
        }

        #endregion

        #region Logistic
        protected void cboLogisticLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocationLogistic();
        }

        private void BindCboLocationLogistic()
        {
            int locationID = Convert.ToInt32(hdnLocationLogisticID.Value);
            if (locationID > 0)
            {
                Location loc = BusinessLayer.GetLocation(locationID);
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                Methods.SetComboBoxField<Location>(cboLogisticLocation, lstLocation, "LocationName", "LocationID");
                cboLogisticLocation.SelectedIndex = 0;
            }
        }

        protected void cbpLogistic_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnLogisticTransactionDtID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (OnSaveEditRecordLogistic(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordLogistic(ref errMessage, ref transactionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "approve")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnApproveChargesDtLogistic(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnVoidChargesDtLogistic(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                if (OnDeleteChargesDtLogistic(ref errMessage, Convert.ToInt32(param[1])))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private bool OnApproveChargesDtLogistic(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsApproved = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            ctx.RollBackTransaction();
                        }
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

        private bool OnVoidChargesDtLogistic(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && entity.IsApproved)
                            {
                                entity.IsApproved = false;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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

        private bool OnDeleteChargesDtLogistic(ref string errMessage, Int32 ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                    if (transactionID > 0)
                    {
                        DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            DraftPatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsDeleted = true;
                                entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entity.GCDeleteReason = Constant.DeleteReason.OTHER;
                                //entity.DeleteReason = reason;
                                entity.DeleteDate = DateTime.Now;
                                entity.DeleteBy = AppSession.UserLogin.UserID;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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

        private void LogisticControlToEntity(DraftPatientChargesDt entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.ChargeClassID = Convert.ToInt32(cboLogisticChargeClassID.Value);
            entity.IsVariable = false;
            entity.TariffComp1 = entity.Tariff = Convert.ToDecimal(Request.Form[txtLogisticUnitTariff.UniqueID]);
            entity.TariffComp2 = entity.TariffComp3 = 0;
            entity.CostAmount = Convert.ToDecimal(hdnLogisticCostAmount.Value);
            entity.IsDiscount = false;
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtLogisticPriceDiscount.UniqueID]);
            entity.GCItemUnit = cboLogisticUoM.Value.ToString();
            entity.IsCITO = false;
            entity.CITOAmount = 0;
            entity.IsComplication = false;
            entity.ComplicationAmount = 0;
            entity.BaseQuantity = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            entity.UsedQuantity = Convert.ToDecimal(txtLogisticQtyUsed.Text);
            entity.ChargedQuantity = Convert.ToDecimal(txtLogisticQtyCharged.Text);
            entity.PatientAmount = Convert.ToDecimal(Request.Form[txtLogisticPatient.UniqueID]);
            entity.PayerAmount = Convert.ToDecimal(Request.Form[txtLogisticPayer.UniqueID]);
            entity.LineAmount = Convert.ToDecimal(Request.Form[txtLogisticTotal.UniqueID]);
            if (hdnLogisticConversionValue.Value != "0")
            {
                entity.ConversionFactor = Convert.ToDecimal(hdnLogisticConversionValue.Value);
            }
            else
            {
                entity.ConversionFactor = 1;
            }
            entity.IsApproved = true;
        }

        private bool OnSaveAddRecordLogistic(ref string errMessage, ref int transactionID)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboLogisticLocation.Value.ToString(), hdnLogisticItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();

            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;
                if (qtyOnHand >= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
                    DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
                    DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
                    try
                    {
                        Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                        if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                        {
                            SaveDraftPatientChargesHd(ctx, ref transactionID);
                            DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                DraftPatientChargesDt entityDt = new DraftPatientChargesDt();
                                LogisticControlToEntity(entityDt);
                                entityDt.ItemID = Convert.ToInt32(hdnLogisticItemID.Value);
                                entityDt.LocationID = Convert.ToInt32(cboLogisticLocation.Value);
                                entityDt.BaseComp1 = entityDt.BaseTariff = Convert.ToDecimal(hdnLogisticBaseTariff.Value);
                                entityDt.BaseComp2 = entityDt.BaseComp3 = 0;
                                entityDt.GCBaseUnit = hdnLogisticDefaultUoM.Value;
                                entityDt.AveragePrice = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault().AveragePrice;
                                entityDt.TransactionID = transactionID;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDt.LastUpdatedBy = entityDt.CreatedBy;
                                entityDt.GCTransactionDetailStatus = entityHd.GCTransactionStatus;
                                entityDtDao.Insert(entityDt);
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtLogisticItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Draft Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboLogisticUoM.Text, itemQty.ToString("N2"), cboLogisticUoM.Text);
                }
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtLogisticItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboLogisticLocation.Text);
            }
            return result;
        }

        private bool OnSaveEditRecordLogistic(ref string errMessage)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboLogisticLocation.Value.ToString(), hdnLogisticItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();

            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;

                if (qtyOnHand >= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
                    DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
                    DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);

                    try
                    {
                        Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                        if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                        {
                            int transactionID = Convert.ToInt32(hdnDraftChargesID.Value);
                            if (transactionID > 0)
                            {
                                DraftPatientChargesHd entityHd = entityHdDao.Get(transactionID);
                                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    DraftPatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnLogisticTransactionDtID.Value));
                                    if (!entityDt.IsDeleted && !entityDt.IsApproved)
                                    {
                                        LogisticControlToEntity(entityDt);
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDtDao.Update(entityDt);
                                        ctx.CommitTransaction();
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Draft Transaksi Sudah Diproses. Tidak Bisa Diubah";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtLogisticItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Draft Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboLogisticUoM.Text, itemQty.ToString("N2"), cboLogisticUoM.Text);
                }
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtLogisticItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboLogisticLocation.Text);
            }
            return result;
        }

        protected void cboLogisticUoM_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnLogisticItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboLogisticUoM, lst, "StandardCodeName", "StandardCodeID");
            cboLogisticUoM.SelectedIndex = -1;
        }

        private void BindGridLogistic()
        {
            DraftPatientChargesHd entity = BusinessLayer.GetDraftPatientChargesHd(Convert.ToInt32(hdnDraftChargesID.Value));
            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                IsEditable = true;
                hdnIsEditable.Value = "1";
            }
            else
            {
                IsEditable = false;
                hdnIsEditable.Value = "0";
            }

            string filterExpression = "1 = 0";
            string transactionID = hdnGCTransactionStatus.Value;
            if (transactionID != "")
                filterExpression = string.Format("TransactionID = {0} AND UsedQuantity >= 0 AND GCItemType = '{1}' AND IsDeleted = 0", hdnDraftChargesID.Value, Constant.ItemGroupMaster.LOGISTIC);
            List<vDraftPatientChargesDt> lst = BusinessLayer.GetvDraftPatientChargesDtList(filterExpression);
            IsShowSwitchIcon = hdnGCCustomerType.Value != Constant.CustomerType.PERSONAL;
            lvwLogistic.DataSource = lst;
            lvwLogistic.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnLogisticAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnLogisticAllTotalPayer.Value = totalPayerAmount.ToString();
        }
        #endregion
    }
}