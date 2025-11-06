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
    public partial class DraftServiceOrderEmergencyEntryCtl : BaseEntryPopupCtl2
    {
        protected bool IsEditable = true;

        protected string GetServiceUnitFilterFilterExpression()
        {
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY);
        }

        public override void InitializeDataControl(string param)
        {
            string[] splitParam = param.Split('|');
            hdnAppointmentID.Value = splitParam[0];
            hdnParamedicID.Value = splitParam[1];
            hdnHealthcareServiceUnitID.Value = Convert.ToString(BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.EMERGENCY)).FirstOrDefault().HealthcareServiceUnitID);
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
            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", hdnParamedicID.Value));
            Methods.SetComboBoxField<ParamedicMaster>(cboFromParamedic, lstParamedic, "FullName", "ParamedicID");
            cboFromParamedic.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnDraftServiceOrderEmergencyID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDraftServiceOrderEmergencyNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDraftServiceOrderEmergencyDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDraftServiceOrderEmergencyTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            //            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtStatus, new ControlEntrySetting(false, false));
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true));
        }

        private void OnLoadEntity(string keyValue)
        {
            string filterExpression = string.Empty;
            if (keyValue.Equals(string.Empty))
            {
                filterExpression = string.Format("DraftServiceOrderNo = '{0}'", txtDraftServiceOrderEmergencyNo.Text);
            }
            else filterExpression = string.Format("DraftServiceOrderID = {0}", keyValue);
            vDraftServiceOrderHd entity = BusinessLayer.GetvDraftServiceOrderHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        public void LoadPage(string serviceOrderID)
        {
            SetControlEnabled(false);
            SetControlProperties();
            OnLoadEntity(serviceOrderID);
        }

        protected void cbpMainPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string serviceOrderID = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "load")
            {
                LoadPage(string.Empty);
            }
            else if (param[0] == "loadaftersave")
            {
                LoadPage(hdnDraftServiceOrderEmergencyID.Value);
            }
            else if (param[0] == "new")
            {
                ReInitControl();
                SetControlProperties();
            }
            else if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage, ref serviceOrderID))
                {
                    result += "success";
                    LoadPage(serviceOrderID);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "proposed")
            {
                if (OnProposeRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftServiceOrderEmergencyID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftServiceOrderEmergencyID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = serviceOrderID;
        }

        #region Save Entity
        public void SaveDraftServiceOrderHd(IDbContext ctx, ref int draftserviceOrderID)
        {
            DraftServiceOrderHdDao entityHdDao = new DraftServiceOrderHdDao(ctx);
            if (hdnDraftServiceOrderEmergencyID.Value == "0")
            {
                DraftServiceOrderHd entityHd = new DraftServiceOrderHd();
                entityHd.AppointmentID = Convert.ToInt32(hdnAppointmentID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.DraftServiceOrderDate = Helper.GetDatePickerValue(Request.Form[txtDraftServiceOrderEmergencyDate.UniqueID]);
                entityHd.DraftServiceOrderTime = Request.Form[txtDraftServiceOrderEmergencyTime.UniqueID];
                entityHd.Remarks = txtNotes.Text;
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.TransactionCode = Constant.TransactionCode.DRAFT_OP_EMERGENCY_ORDER;
                entityHd.DraftServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.DraftServiceOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                draftserviceOrderID = BusinessLayer.GetDraftServiceOrderHdMaxID(ctx);
            }
            else
            {
                draftserviceOrderID = Convert.ToInt32(hdnDraftServiceOrderEmergencyID.Value);
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
                    int draftServiceOrderID = 0;
                    SaveDraftServiceOrderHd(ctx, ref draftServiceOrderID);
                    hdnDraftServiceOrderEmergencyID.Value = draftServiceOrderID.ToString();
                    retval = draftServiceOrderID.ToString();
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
                    DraftServiceOrderHd entity = BusinessLayer.GetDraftServiceOrderHd(Convert.ToInt32(hdnDraftServiceOrderEmergencyID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.Remarks = txtNotes.Text;
                        BusinessLayer.UpdateDraftServiceOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Order " + entity.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftServiceOrderHdDao entityHdDao = new DraftServiceOrderHdDao(ctx);
            DraftServiceOrderDtDao entityDtDao = new DraftServiceOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    Int32 DraftServiceOrderID = Convert.ToInt32(hdnDraftServiceOrderEmergencyID.Value);
                    DraftServiceOrderHd entity = entityHdDao.Get(DraftServiceOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        List<DraftServiceOrderDt> lstDt = BusinessLayer.GetDraftServiceOrderDtList(string.Format("DraftServiceOrderID = {0} AND IsDeleted = 0", DraftServiceOrderID));
                        foreach (DraftServiceOrderDt dt in lstDt)
                        {
                            DraftServiceOrderDt entityDt = entityDtDao.Get(dt.ID);
                            entityDt.GCDraftServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
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
                        errMessage = "Draft Order " + entity.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                    Int32 DraftServiceOrderID = Convert.ToInt32(hdnDraftServiceOrderEmergencyID.Value);
                    DraftServiceOrderHd entity = BusinessLayer.GetDraftServiceOrderHd(DraftServiceOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //                    List<DraftServiceOrderDt> lstDt = BusinessLayer.GetDraftServiceOrderDtList(string.Format("DraftserviceOrderID = {0} AND IsDeleted = 0", hdnDraftServiceOrderEmergencyID.Value));
                        BusinessLayer.UpdateDraftServiceOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Order " + entity.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            int serviceOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    serviceOrderID = Convert.ToInt32(hdnDraftServiceOrderEmergencyID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref serviceOrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                serviceOrderID = Convert.ToInt32(hdnDraftServiceOrderEmergencyID.Value);
                if (OnDeleteEntityDt(ref errMessage, serviceOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpServiceOrderID"] = serviceOrderID.ToString();
        }

        private void ControlToEntity(DraftServiceOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Remarks = txtNotes.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int DraftServiceOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftServiceOrderHdDao entityHdDao = new DraftServiceOrderHdDao(ctx);
            DraftServiceOrderDtDao entityDtDao = new DraftServiceOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    SaveDraftServiceOrderHd(ctx, ref DraftServiceOrderID);
                    DraftServiceOrderHd entityHd = entityHdDao.Get(DraftServiceOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        DraftServiceOrderDt entityDt = new DraftServiceOrderDt();
                        ControlToEntity(entityDt);
                        hdnDraftServiceOrderEmergencyID.Value = DraftServiceOrderID.ToString();
                        entityDt.DraftServiceOrderID = DraftServiceOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDt.GCDraftServiceOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityDt.ItemQty = 1;
                        entityDt.ItemUnit = hdnGCItemUnit.Value;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftServiceOrderHdDao entityHdDao = new DraftServiceOrderHdDao(ctx);
            DraftServiceOrderDtDao entityDtDao = new DraftServiceOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftServiceOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftServiceOrderHd entityHd = entityHdDao.Get(entityDt.DraftServiceOrderID);
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
                            errMessage = "Draft Order " + entityHd.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftServiceOrderHdDao entityHdDao = new DraftServiceOrderHdDao(ctx);
            DraftServiceOrderDtDao entityDtDao = new DraftServiceOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftServiceOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftServiceOrderHd entityHd = entityHdDao.Get(entityDt.DraftServiceOrderID);
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
                        errMessage = "Draft Order " + entityHd.DraftServiceOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private void EntityToControl(vDraftServiceOrderHd entity)
        {
            txtStatus.Text = entity.TransactionStatusWatermark;
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == null);

            if (IsEditable)
            {
                divAddDataEmergencyDraftOrder.Style.Remove("display");
                hdnIsEditable.Value = "1";
            }
            else
            {
                divAddDataEmergencyDraftOrder.Style.Add("display", "none");
                hdnIsEditable.Value = "0";
            }

            hdnDraftServiceOrderEmergencyID.Value = entity.DraftServiceOrderID.ToString();
            txtDraftServiceOrderEmergencyNo.Text = entity.DraftServiceOrderNo;
            txtDraftServiceOrderEmergencyDate.Text = entity.DraftServiceOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDraftServiceOrderEmergencyTime.Text = entity.DraftServiceOrderTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtNotes.Text = entity.Remarks;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnDraftServiceOrderEmergencyID.Value != "")
                filterExpression = string.Format("DraftServiceOrderID = {0} AND IsDeleted = 0 AND GCDraftServiceOrderStatus != '{1}' ORDER BY ID DESC",
                                                    hdnDraftServiceOrderEmergencyID.Value, Constant.TransactionStatus.VOID);
            List<vDraftServiceOrderDt> lstEntity = BusinessLayer.GetvDraftServiceOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}