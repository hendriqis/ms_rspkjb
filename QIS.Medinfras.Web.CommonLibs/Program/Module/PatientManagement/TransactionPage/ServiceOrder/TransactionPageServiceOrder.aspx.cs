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
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageServiceOrder : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT:
                    if (param[0] == "er")
                    {
                        if (param.Length > 1)
                        {
                            if (param[1] == "2")
                            {
                                return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_EMERGENCY_ORDER_2;
                            }
                            else
                            {
                                return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_EMERGENCY_ORDER;
                            }
                        }
                        else
                        {
                            return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_EMERGENCY_ORDER;
                        }
                    }
                    else
                    {
                        return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_OUTPATIENT_ORDER;
                    }
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_OUTPATIENT_ORDER;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        if (param[0] == "er")
                            return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_EMERGENCY_ORDER;
                        else
                            return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_OUTPATIENT_ORDER;
                    }

                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    {
                        if (param[0] == "er")
                            return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_EMERGENCY_ORDER;
                        else
                            return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_OUTPATIENT_ORDER;
                    }

                    if (param[0] == "er")
                    {
                        if (param[1] == "2")
                        {
                            return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_EMERGENCY_ORDER_2;
                        }
                        else
                        {
                            return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_EMERGENCY_ORDER;
                        }
                    }
                    else
                    {
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_OUTPATIENT_ORDER;
                    }

                default: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_EMERGENCY_ORDER;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !AppSession.RegisteredPatient.IsLockDown);
            IsAllowSave = !AppSession.RegisteredPatient.IsLockDown;
            IsAllowVoid = !AppSession.RegisteredPatient.IsLockDown;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');

            hdnDepartmentIDTo.Value = Constant.Facility.INPATIENT;
            if (param[0] == "er")
            {
                trServiceUnit.Style.Add("display", "none");
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = HSU.HealthcareServiceUnitID.ToString();
                hdnDepartmentIDTo.Value = Constant.Facility.EMERGENCY;
            }
            else if (param[0] == "op")
            {
                hdnDepartmentIDTo.Value = Constant.Facility.OUTPATIENT;
            }
            hdnCode.Value = param[0];
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();
            IsEditable = AppSession.RegisteredPatient.IsLockDown ? false : true;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            IsLoadFirstRecord = (OnGetRowCount() > 0);
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnServiceOrderID.Value != "")
                filterExpression = string.Format("ServiceOrderID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnServiceOrderID.Value);
            List<vServiceOrderDt> lstEntity = BusinessLayer.GetvServiceOrderDtList(filterExpression);
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

        protected override void OnControlEntrySetting()
        {
            SetComboBox();
            SetControlEntrySetting(hdnServiceOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtServiceOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtServiceOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtServiceOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, hdnDefaultParamedicName.Value));
            SetControlEntrySetting(cboParamedicFrom, new ControlEntrySetting(true, false, true, cboParamedicFrom.Items[0].Value.ToString()));

            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param[0] == "er")
                SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false, hdnDefaultHealthcareServiceUnitID.Value));
            else
                SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(lblSrvceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        private void SetComboBox()
        {
            List<vParamedicTeam> lstParamedicTeam = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = '{0}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID));
            List<vConsultVisit> lstParamedic = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID));
            vRegistration paramedic = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            if (lstParamedicTeam.Count == 0)
            {
                Methods.SetComboBoxField<vConsultVisit>(cboParamedicFrom, lstParamedic, "ParamedicName", "ParamedicID");
            }
            else
            {
                lstParamedicTeam.Insert(0, new vParamedicTeam() { ParamedicName = paramedic.ParamedicName, ParamedicID = paramedic.ParamedicID });
                Methods.SetComboBoxField<vParamedicTeam>(cboParamedicFrom, lstParamedicTeam, "ParamedicName", "ParamedicID");
            }

            //cboParamedicFrom.SelectedIndex = 0;
        }

        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            IsEditable = true;
            string filterExpression = "1 = 0";
            List<vServiceOrderDt> lstEntity = BusinessLayer.GetvServiceOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            trVoidReason.Style.Add("display", "none");
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpressionServiceOrderHd();
            return BusinessLayer.GetvServiceOrderHdRowCount(filterExpression);
        }

        #region Load Entity

        public string GetFilterExpressionServiceOrderHd()
        {
            return string.Format("VisitID = {0} AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = '{1}')", hdnVisitID.Value, hdnDepartmentIDTo.Value);
        }

        protected bool IsEditable = true;
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpressionServiceOrderHd();
            vServiceOrderHd entity = BusinessLayer.GetvServiceOrderHd(filterExpression, PageIndex, " ServiceOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);

        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpressionServiceOrderHd();
            PageIndex = BusinessLayer.GetvServiceOrderHdRowIndex(filterExpression, keyValue, "ServiceOrderID DESC");
            vServiceOrderHd entity = BusinessLayer.GetvServiceOrderHd(filterExpression, PageIndex, "ServiceOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vServiceOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            IsEditable = entityRegistration.IsLockDown ? false : IsEditable;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnServiceOrderID.Value = entity.ServiceOrderID.ToString();
            txtServiceOrderNo.Text = entity.ServiceOrderNo;
            txtServiceOrderDate.Text = entity.ServiceOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceOrderTime.Text = entity.ServiceOrderTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtNotes.Text = entity.Remarks;
            cboParamedicFrom.Value = entity.OrderParamedicID.ToString();
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            divCreatedBy.InnerHtml = entity.CreatedBy;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateBy;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedBy;
                if (entity.ProposedDate != null && entity.ProposedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divProposedDate.InnerHtml = entity.ProposedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                trProposedBy.Style.Remove("display");
                trProposedDate.Style.Remove("display");
            }
            else
            {
                trProposedBy.Style.Add("display", "none");
                trProposedDate.Style.Add("display", "none");
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                divVoidBy.InnerHtml = entity.VoidBy;
                if (entity.VoidDate != null && entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                string voidReason = "";

                if (entity.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entity.VoidReasonWatermark + " ( " + entity.VoidReason + " )";
                }
                else
                {
                    voidReason = entity.VoidReasonWatermark;
                }

                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");
                divVoidReason.InnerHtml = voidReason;
                trVoidReason.Style.Remove("display");
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
                trVoidReason.Style.Add("display", "none");
            }

            BindGridView();
        }
        #endregion

        #region Save Entity
        public void SaveServiceOrderHd(IDbContext ctx, ref int serviceOrderID)
        {
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            if (hdnServiceOrderID.Value == "0")
            {
                ServiceOrderHd entityHd = new ServiceOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.ServiceOrderDate = Helper.GetDatePickerValue(Request.Form[txtServiceOrderDate.UniqueID]);
                entityHd.ServiceOrderTime = Request.Form[txtServiceOrderTime.UniqueID];
                entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.Remarks = txtNotes.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                {
                    if (hdnCode.Value == "op")
                        entityHd.TransactionCode = Constant.TransactionCode.IP_OUTPATIENT_ORDER;
                    else entityHd.TransactionCode = Constant.TransactionCode.IP_EMERGENCY_ORDER;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_EMERGENCY_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_OUTPATIENT_ORDER;
                entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.OrderParamedicID = Convert.ToInt32(cboParamedicFrom.Value);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                serviceOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                serviceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int serviceOrderID = 0;
                SaveServiceOrderHd(ctx, ref serviceOrderID);

                retval = serviceOrderID.ToString();
                ctx.CommitTransaction();
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                ServiceOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.Remarks = txtNotes.Text;
                    entityHd.ReferenceNo = txtReferenceNo.Text;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Order {0} tidak dapat diubah. Harap refresh halaman ini.", entityHd.ServiceOrderNo);
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

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                Int32 ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                ServiceOrderHd entity = entityHdDao.Get(ServiceOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.VoidBy = AppSession.UserLogin.UserID;

                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Order {0} tidak dapat diubah. Harap refresh halaman ini.", entity.ServiceOrderNo);
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
                ServiceOrderDtDao entityDtDao = new ServiceOrderDtDao(ctx);
                try
                {
                    Int32 ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                    ServiceOrderHd entity = entityHdDao.Get(ServiceOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entityHdDao.Update(entity);

                        List<ServiceOrderDt> lstDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0}", ServiceOrderID));
                        foreach (ServiceOrderDt dt in lstDt)
                        {
                            ServiceOrderDt entityDt = entityDtDao.Get(dt.ID);
                            entityDt.GCServiceOrderStatus = Constant.OrderStatus.CANCELLED;
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
                        errMessage = string.Format("Order {0} tidak dapat diubah. Harap refresh halaman ini.", entity.ServiceOrderNo);
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
            return result;
        }
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDtDao = new ServiceOrderDtDao(ctx);

            try
            {
                Int32 ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                ServiceOrderHd entity = entityHdDao.Get(ServiceOrderID);

                List<ServiceOrderDt> lstDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0}", ServiceOrderID), ctx);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.ProposedBy = AppSession.UserLogin.UserID;
                    entity.ProposedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    if (lstDt.Count > 0)
                    {
                        foreach (ServiceOrderDt e in lstDt)
                        {
                            if (e.Remarks == "")
                            {
                                e.Remarks = txtNotes.Text;
                            }
                            else
                            {
                                e.Remarks = txtRemarks.Text;
                            }
                            e.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(e);
                        }
                    }

                    ctx.CommitTransaction();
                    result = true;
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Order {0} tidak dapat diubah. Harap refresh halaman ini.", entity.ServiceOrderNo);
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
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    serviceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
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
                serviceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, serviceOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpServiceOrderID"] = serviceOrderID.ToString();
        }

        private void ControlToEntity(ServiceOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int serviceOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDtDao = new ServiceOrderDtDao(ctx);
            try
            {
                if (hdnServiceOrderID.Value.ToString() == "" || hdnServiceOrderID.Value.ToString() == "0")
                {
                    SaveServiceOrderHd(ctx, ref serviceOrderID);
                    ServiceOrderDt entityDt = new ServiceOrderDt();
                    ControlToEntity(entityDt);
                    entityDt.ServiceOrderID = serviceOrderID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    //TO-DO ini nanya itu bener ga pake test order status untuk GC
                    entityDt.GCServiceOrderStatus = Constant.TestOrderStatus.OPEN;
                    entityDt.ItemQty = 1;
                    entityDt.ItemUnit = hdnGCItemUnit.Value;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else if (hdnServiceOrderID.Value.ToString() != "" || hdnServiceOrderID.Value.ToString() != "0")
                {
                    if (entityHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        SaveServiceOrderHd(ctx, ref serviceOrderID);
                        ServiceOrderDt entityDt = new ServiceOrderDt();
                        ControlToEntity(entityDt);
                        entityDt.ServiceOrderID = serviceOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        //TO-DO ini nanya itu bener ga pake test order status untuk GC
                        entityDt.GCServiceOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityDt.ItemQty = 1;
                        entityDt.ItemUnit = hdnGCItemUnit.Value;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                }
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
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDtDao = new ServiceOrderDtDao(ctx);
            try
            {
                if (entityHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ServiceOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDtDao = new ServiceOrderDtDao(ctx);
            try
            {
                if (entityHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ServiceOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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
    }
}