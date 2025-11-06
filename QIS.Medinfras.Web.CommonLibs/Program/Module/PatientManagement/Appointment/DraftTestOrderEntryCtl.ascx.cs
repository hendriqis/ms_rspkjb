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
    public partial class DraftTestOrderEntryCtl : BaseEntryPopupCtl2
    {
        protected bool IsEditable = true;

        protected string GetServiceUnitFilterFilterExpression()
        {
            if (hdnDepartmentFromID.Value == "IS")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != '{2}' AND IsDeleted = 0 AND IsUsingJobOrder = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnImagingServiceUnitID.Value);
            }
            else if (hdnDepartmentFromID.Value == "LB")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != '{2}' AND IsDeleted = 0 AND IsLaboratoryUnit = 0 AND IsUsingJobOrder = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnLaboratoryServiceUnitID.Value);
            }
            else if (hdnDepartmentFromID.Value == "MD")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != '{2}' AND IsDeleted = 0 AND IsUsingJobOrder = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitFromID.Value);
            }
            else
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingJobOrder = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);
            }
        }

        public override void InitializeDataControl(string param)
        {
            string[] splitParam = param.Split('|');
            hdnAppointmentID.Value = splitParam[0];
            hdnDepartmentFromID.Value = splitParam[1];

            Appointment appointment = BusinessLayer.GetAppointment(Convert.ToInt32(hdnAppointmentID.Value));
            if (AppSession.HealthcareServiceUnitID.ToString() != "" && AppSession.HealthcareServiceUnitID.ToString() != "0")
            {
                hdnHealthcareServiceUnitFromID.Value = AppSession.HealthcareServiceUnitID.ToString();
            }
            else
            {
                hdnHealthcareServiceUnitFromID.Value = Convert.ToString(appointment.HealthcareServiceUnitID);
            }
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
            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
            Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");
            cboToBePerformed.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnDraftTestOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDraftTestOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDraftTestOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDraftTestOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduledDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduledTime, new ControlEntrySetting(true, false, true, "00:00"));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtStatus, new ControlEntrySetting(false, false));
            SetControlEntrySetting(cboToBePerformed, new ControlEntrySetting(true, true, false));
            if (cboToBePerformed.Items.Count > 0)
                cboToBePerformed.SelectedIndex = 0;
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true));
        }

        private void OnLoadEntity(string keyValue)
        {
            string filterExpression = string.Empty;
            if (keyValue.Equals(string.Empty))
            {
                filterExpression = string.Format("DraftTestOrderNo = '{0}'", txtDraftTestOrderNo.Text);
            }
            else filterExpression = string.Format("DraftTestOrderID = {0}", keyValue);
            vDraftTestOrderHd entity = BusinessLayer.GetvDraftTestOrderHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        public void LoadPage(string testOrderID)
        {
            SetControlEnabled(false);
            SetControlProperties();
            OnLoadEntity(testOrderID);
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
                LoadPage(hdnDraftTestOrderID.Value);
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
                    LoadPage(hdnDraftTestOrderID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnDraftTestOrderID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = testOrderID;
        }

        #region Save Entity
        public void SaveDraftTestOrderHd(IDbContext ctx, ref int draftTestOrderID)
        {
            DraftTestOrderHdDao entityHdDao = new DraftTestOrderHdDao(ctx);
            if (hdnDraftTestOrderID.Value == "0")
            {
                DraftTestOrderHd entityHd = new DraftTestOrderHd();
                entityHd.AppointmentID = Convert.ToInt32(hdnAppointmentID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.DraftTestOrderDate = Helper.GetDatePickerValue(Request.Form[txtDraftTestOrderDate.UniqueID]);
                entityHd.DraftTestOrderTime = Request.Form[txtDraftTestOrderTime.UniqueID];
                entityHd.GCToBePerformed = cboToBePerformed.Value.ToString();
                entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtScheduledDate.UniqueID]);
                entityHd.ScheduledTime = Request.Form[txtScheduledTime.UniqueID];
                entityHd.IsCITO = chkIsCITO.Checked;
                entityHd.Remarks = txtNotes.Text;
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                    entityHd.TransactionCode = Constant.TransactionCode.DRAFT_IMAGING_TEST_ORDER;
                else if (hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                    entityHd.TransactionCode = Constant.TransactionCode.DRAFT_LABORATORY_TEST_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.DRAFT_OTHER_TEST_ORDER;
                entityHd.DraftTestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.DraftTestOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                draftTestOrderID = BusinessLayer.GetDraftTestOrderHdMaxID(ctx);
            }
            else
            {
                draftTestOrderID = Convert.ToInt32(hdnDraftTestOrderID.Value);
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
                    int draftTestOrderID = 0;
                    SaveDraftTestOrderHd(ctx, ref draftTestOrderID);
                    hdnDraftTestOrderID.Value = draftTestOrderID.ToString();
                    retval = draftTestOrderID.ToString();
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
                    DraftTestOrderHd entity = BusinessLayer.GetDraftTestOrderHd(Convert.ToInt32(hdnDraftTestOrderID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.IsCITO = chkIsCITO.Checked;
                        entity.Remarks = txtNotes.Text;
                        BusinessLayer.UpdateDraftTestOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Order " + entity.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftTestOrderHdDao entityHdDao = new DraftTestOrderHdDao(ctx);
            DraftTestOrderDtDao entityDtDao = new DraftTestOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    Int32 DraftTestOrderID = Convert.ToInt32(hdnDraftTestOrderID.Value);
                    DraftTestOrderHd entity = entityHdDao.Get(DraftTestOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = Constant.DeleteReason.OTHER;
                        entity.VoidReason = "HEADER IS CANCELLED";
                        entity.VoidDate = DateTime.Now;
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        List<DraftTestOrderDt> lstDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0}", DraftTestOrderID));
                        foreach (DraftTestOrderDt dt in lstDt)
                        {
                            DraftTestOrderDt entityDt = entityDtDao.Get(dt.ID);
                            entityDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
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
                        errMessage = "Draft Order " + entity.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                    Int32 DraftTestOrderID = Convert.ToInt32(hdnDraftTestOrderID.Value);
                    DraftTestOrderHd entity = BusinessLayer.GetDraftTestOrderHd(DraftTestOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        List<DraftTestOrderDt> lstDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND IsDeleted = 0", hdnDraftTestOrderID.Value));
                        if (lstDt != null)
                        {
                            //jika semua testOrderDt IsCito 1 dan testOrderHD IsCito 0 maka testOrderHd IsCito 1
                            int entityDtCitoCount = lstDt.Where(t => t.IsCITO).ToList().Count;
                            int entityDtValidCount = lstDt.Where(t => t.IsDeleted == false && t.GCDraftTestOrderStatus != Constant.TestOrderStatus.CANCELLED).ToList().Count;

                            if (entityDtCitoCount == entityDtValidCount && entity.IsCITO == false)
                            {
                                entity.IsCITO = true;
                            }

                            //cek untuk setiap testOrderDt jika IsCito 0 sementara testOrderHD IsCito 1 maka testOrderDt IsCito 1
                            foreach (DraftTestOrderDt e in lstDt)
                            {
                                if (entity.IsCITO == true)
                                {
                                    if (e.IsCITO == false && e.IsDeleted == false && e.GCDraftTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                    {
                                        e.IsCITO = true;
                                        BusinessLayer.UpdateDraftTestOrderDt(e);
                                    }
                                }
                            }
                        }
                        BusinessLayer.UpdateDraftTestOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Order " + entity.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            int testOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    testOrderID = Convert.ToInt32(hdnDraftTestOrderID.Value);
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
                testOrderID = Convert.ToInt32(hdnDraftTestOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, testOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTestOrderID"] = testOrderID.ToString();
        }

        private void ControlToEntity(DraftTestOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.DiagnoseID = txtDiagnoseID.Text;
            entityDt.IsCITO = chkIsCITO_Detail.Checked;
            entityDt.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int DraftTestOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            DraftTestOrderHdDao entityHdDao = new DraftTestOrderHdDao(ctx);
            DraftTestOrderDtDao entityDtDao = new DraftTestOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    SaveDraftTestOrderHd(ctx, ref DraftTestOrderID);
                    DraftTestOrderHd entityHd = entityHdDao.Get(DraftTestOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        DraftTestOrderDt entityDt = new DraftTestOrderDt();
                        ControlToEntity(entityDt);
                        hdnDraftTestOrderID.Value = DraftTestOrderID.ToString();
                        entityDt.DraftTestOrderID = DraftTestOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityDt.ItemQty = 1;
                        entityDt.ItemUnit = hdnGCItemUnit.Value;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftTestOrderHdDao entityHdDao = new DraftTestOrderHdDao(ctx);
            DraftTestOrderDtDao entityDtDao = new DraftTestOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftTestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftTestOrderHd entityHd = entityHdDao.Get(entityDt.DraftTestOrderID);
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
                            errMessage = "Draft Order " + entityHd.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Draft Order " + entityHd.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            DraftTestOrderHdDao entityHdDao = new DraftTestOrderHdDao(ctx);
            DraftTestOrderDtDao entityDtDao = new DraftTestOrderDtDao(ctx);
            try
            {
                Appointment appointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (appointment.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                {
                    DraftTestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    DraftTestOrderHd entityHd = entityHdDao.Get(entityDt.DraftTestOrderID);
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
                        errMessage = "Draft Order " + entityHd.DraftTestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private void EntityToControl(vDraftTestOrderHd entity)
        {
            txtStatus.Text = entity.TransactionStatusWatermark;
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == null);

            if (IsEditable)
            {
                divAddDataDraftTestOrder.Style.Remove("display");
                hdnIsEditable.Value = "1";
            }
            else
            {
                divAddDataDraftTestOrder.Style.Add("display", "none");
                hdnIsEditable.Value = "0";
            }

            hdnDraftTestOrderID.Value = entity.DraftTestOrderID.ToString();
            txtDraftTestOrderNo.Text = entity.DraftTestOrderNo;
            txtDraftTestOrderDate.Text = entity.DraftTestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDraftTestOrderTime.Text = entity.DraftTestOrderTime;
            cboToBePerformed.Value = entity.GCToBePerformed;
            txtScheduledDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduledTime.Text = entity.ScheduledTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            chkIsCITO.Checked = entity.IsCITO;
            txtNotes.Text = entity.Remarks;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnDraftTestOrderID.Value != "")
                filterExpression = string.Format("DraftTestOrderID = {0} AND IsDeleted = 0 AND GCDraftTestOrderStatus != '{1}' ORDER BY ID DESC",
                                                    hdnDraftTestOrderID.Value, Constant.TransactionStatus.VOID);
            List<vDraftTestOrderDt> lstEntity = BusinessLayer.GetvDraftTestOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Utility Function
        //private void SetControlEnabled(bool isAdd)
        //{
        //    foreach (DictionaryEntry entry in ControlEntryList)
        //    {
        //        Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
        //        ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
        //        bool isEnabled = (isAdd ? setting.IsEditAbleInAddMode : setting.IsEditAbleInEditMode);
        //        SetControlAttribute(ctrl, isEnabled);
        //    }
        //}

        //public void ReInitControl()
        //{
        //    SetControlEnabled(true);
        //    LoadWords();
        //    foreach (DictionaryEntry entry in ControlEntryList)
        //    {
        //        Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
        //        if (ctrl is ASPxEdit || ctrl is WebControl || ctrl is HtmlInputHidden)
        //        {
        //            ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
        //            switch (setting.DefaultValue.ToString())
        //            {
        //                case Constant.DefaultValueEntry.DATE_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)); break;
        //                case Constant.DefaultValueEntry.TIME_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)); break;
        //                default: SetControlValue(ctrl, setting.DefaultValue); break;
        //            }
        //            if (ctrl is ASPxEdit)
        //            {
        //                ASPxEdit ctl = ctrl as ASPxEdit;
        //                ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
        //                ctl.ValidationSettings.RequiredField.ErrorText = "";
        //                ctl.ValidationSettings.CausesValidation = true;
        //                ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
        //                ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

        //                //if (setting.IsRequired)
        //                ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
        //            }
        //            else if (ctrl is WebControl)
        //            {
        //                if (setting.IsRequired)
        //                    Helper.AddCssClass(((WebControl)ctrl), "required");
        //                ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
        //            }
        //        }
        //    }
        //}

        //protected void SetControlEntrySetting(Control ctrl, ControlEntrySetting setting)
        //{
        //    ControlEntryList.Add(ctrl.ID, setting);
        //    if (ctrl is ASPxEdit)
        //    {
        //        ASPxEdit ctl = ctrl as ASPxEdit;
        //        ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
        //        ctl.ValidationSettings.RequiredField.ErrorText = "";
        //        ctl.ValidationSettings.CausesValidation = true;
        //        ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
        //        ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

        //        //if (setting.IsRequired)
        //        ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
        //    }
        //    else if (ctrl is WebControl)
        //    {
        //        if (setting.IsRequired)
        //            Helper.AddCssClass(((WebControl)ctrl), "required");
        //        ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
        //    }
        //}

        //private void SetControlAttribute(Control ctrl, bool isEnabled)
        //{
        //    if (ctrl is ASPxEdit)
        //    {
        //        ((ASPxEdit)ctrl).ClientEnabled = isEnabled;
        //    }
        //    else if (ctrl is TextBox)
        //    {
        //        if (isEnabled)
        //            ((TextBox)ctrl).ReadOnly = false;
        //        else
        //            ((TextBox)ctrl).ReadOnly = true;
        //    }
        //    else if (ctrl is DropDownList)
        //    {
        //        ((DropDownList)ctrl).Enabled = isEnabled;
        //    }
        //    else if (ctrl is CheckBox)
        //    {
        //        ((CheckBox)ctrl).Enabled = isEnabled;
        //    }
        //    else if (ctrl is HtmlGenericControl)
        //    {
        //        HtmlGenericControl lbl = ctrl as HtmlGenericControl;
        //        if (!isEnabled)
        //            lbl.Attributes.Add("class", "lblDisabled");
        //    }
        //}

        //private void SetControlValue(Control ctrl, object value)
        //{
        //    if (ctrl is ASPxEdit)
        //        ((ASPxEdit)ctrl).Value = value;
        //    else if (ctrl is TextBox)
        //        ((TextBox)ctrl).Text = value.ToString();
        //    else if (ctrl is DropDownList)
        //        Helper.SetDropDownListValue((DropDownList)ctrl, value.ToString());
        //    else if (ctrl is CheckBox)
        //    {
        //        if (value.ToString() == "")
        //            ((CheckBox)ctrl).Checked = false;
        //        else
        //            ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
        //    }

        //    else if (ctrl is HtmlInputHidden)
        //        ((HtmlInputHidden)ctrl).Value = value.ToString();
        //}
        #endregion

        #region Session & View State
        //public Hashtable ControlEntryList
        //{
        //    get
        //    {
        //        if (Session["__PopupControlEntryList"] == null)
        //            Session["__PopupControlEntryList"] = new Hashtable();

        //        return (Hashtable)Session["__PopupControlEntryList"];
        //    }
        //    set { Session["__PopupControlEntryList"] = value; }
        //}
        #endregion
    }
}