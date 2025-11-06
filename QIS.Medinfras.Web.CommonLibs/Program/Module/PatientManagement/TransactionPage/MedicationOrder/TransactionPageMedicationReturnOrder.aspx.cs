using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageMedicationReturnOrder : BasePageTrx
    {
        private vConsultVisit entityCV = null;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER;
                default: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER;
            }

        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !entityCV.IsLockDown);
            IsAllowSave = !entityCV.IsLockDown;
            IsAllowVoid = !entityCV.IsLockDown;
        }

        protected override void InitializeDataControl()
        {

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

            hdnDepartmentID.Value = entityCV.DepartmentID;
            hdnGCRegistrationStatus.Value = entityCV.GCVisitStatus;
            hdnRegistrationID.Value = entityCV.RegistrationID.ToString();
            hdnClassID.Value = entityCV.ClassID.ToString();

            hdnDefaultVisitParamedicID.Value = entityCV.ParamedicID.ToString();
            hdnDefaultVisitParamedicCode.Value = entityCV.ParamedicCode;
            hdnDefaultVisitParamedicName.Value = entityCV.ParamedicName;

            //txtServiceCode.Attributes.Add("validationgroup", "mpTrxService");
            IsLoadFirstRecord = (OnGetRowCount() > 0);

            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            hdnDefaultDispensaryServiceUnitID.Value = lstHealthcareServiceUnit.FirstOrDefault().DispensaryServiceUnitID.ToString();

            Helper.SetControlEntrySetting(txtReturnQty, new ControlEntrySetting(true, true, true), "mpTrx");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            IsEditable = entityCV.IsLockDown ? false : true;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            BindGridView();
        }

        protected override void SetControlProperties()
        {
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0 AND IsUsingRegistration = 1"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (hdnDefaultDispensaryServiceUnitID.Value == "0") cboDispensaryUnit.SelectedIndex = 0;
            else
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFILL_INSTRUCTION, Constant.StandardCode.PRESCRIPTION_RETURN_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboReturnType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_RETURN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            BindCboLocation();
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnit.Value)).FirstOrDefault();

            if (location != null)
            {
                int locationID = location.LocationID;
                Location loc = BusinessLayer.GetLocation(locationID);
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
        }

        protected override void OnControlEntrySetting()
        {
            entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

            hdnPhysicianID.Value = entityCV.ParamedicID.ToString();
            txtPhysicianCode.Text = entityCV.ParamedicCode;
            txtPhysicianName.Text = entityCV.ParamedicName;

            SetControlEntrySetting(hdnPrescriptionReturnOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPrescriptionReturnOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboDispensaryUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboReturnType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnDefaultVisitParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultVisitParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultVisitParamedicName.Value));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        protected bool IsEditable = true;
        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            if (hdnDefaultDispensaryServiceUnitID.Value == "0") cboDispensaryUnit.SelectedIndex = 0;
            else
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;
            IsEditable = true;

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

            BindGridView();
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvPrescriptionReturnOrderHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHd(filterExpression, PageIndex, " PrescriptionReturnOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvPrescriptionReturnOrderHdRowIndex(filterExpression, keyValue, "PrescriptionReturnOrderID DESC");
            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHd(filterExpression, PageIndex, "PrescriptionReturnOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPrescriptionReturnOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
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
            hdnPrescriptionReturnOrderID.Value = entity.PrescriptionReturnOrderID.ToString();
            txtPrescriptionReturnOrderNo.Text = entity.PrescriptionReturnOrderNo;
            txtPrescriptionDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.OrderTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            cboReturnType.Value = entity.GCPrescriptionReturnType;
            cboDispensaryUnit.Value = entity.HealthcareServiceUnitID.ToString();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedByName;
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
                divVoidBy.InnerHtml = entity.VoidByName;
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

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionReturnOrderID.Value != "")
                filterExpression = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0 ORDER BY PrescriptionReturnOrderDtID DESC", hdnPrescriptionReturnOrderID.Value);
            List<vPrescriptionReturnOrderDt> lstEntity = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression);
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
        #endregion

        #region Save Entity
        public void SavePrescriptionReturnOrderHd(IDbContext ctx, ref int PrescriptionReturnOrderID)
        {
            PrescriptionReturnOrderHdDao entityHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao entityDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PrescriptionReturnOrderHd entityHd = null;
            if (hdnPrescriptionReturnOrderID.Value == "0")
            {
                entityHd = new PrescriptionReturnOrderHd();
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.OrderDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityHd.OrderTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.GCPrescriptionReturnType = cboReturnType.Value.ToString();
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                entityHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
                //if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                //    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                //else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                //    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                //else
                //    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionReturnOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.OrderDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHdDao.Insert(entityHd);

                PrescriptionReturnOrderID = BusinessLayer.GetPrescriptionReturnOrderHdMaxID(ctx);
            }
            else
            {
                PrescriptionReturnOrderID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PrescriptionReturnOrderID = 0;
                SavePrescriptionReturnOrderHd(ctx, ref PrescriptionReturnOrderID);

                retval = PrescriptionReturnOrderID.ToString();
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
            try
            {
                PrescriptionReturnOrderHd entity = BusinessLayer.GetPrescriptionReturnOrderHd(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCPrescriptionReturnType = cboReturnType.Value.ToString();
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionReturnOrderHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Order Retur Resep " + entity.PrescriptionReturnOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderHdDao entityHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao entityDtDao = new PrescriptionReturnOrderDtDao(ctx);
            try
            {
                Int32 PrescriptionReturnOrderID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
                PrescriptionReturnOrderHd entity = entityHdDao.Get(PrescriptionReturnOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterDt = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", entity.PrescriptionReturnOrderID);
                    List<PrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionReturnOrderDtList(filterDt, ctx);
                    foreach (PrescriptionReturnOrderDt entityDt in lstEntityDt)
                    {
                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDt.GCDeleteReason = Constant.DeleteReason.OTHER;
                        entityDt.DeleteReason = "HEADER IS CANCELLED";
                        entityDtDao.Update(entityDt);
                    }

                    entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
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
                    errMessage = "Order Retur Resep " + entity.PrescriptionReturnOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                PrescriptionReturnOrderHdDao entityHdDao = new PrescriptionReturnOrderHdDao(ctx);
                PrescriptionReturnOrderDtDao entityDtDao = new PrescriptionReturnOrderDtDao(ctx);
                try
                {
                    Int32 PrescriptionReturnOrderID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
                    PrescriptionReturnOrderHd entity = entityHdDao.Get(PrescriptionReturnOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entityHdDao.Update(entity);

                        List<PrescriptionReturnOrderDt> lstDt = BusinessLayer.GetPrescriptionReturnOrderDtList(string.Format("PrescriptionReturnOrderID = {0}", PrescriptionReturnOrderID));
                        foreach (PrescriptionReturnOrderDt dt in lstDt)
                        {
                            PrescriptionReturnOrderDt entityDt = entityDtDao.Get(dt.PrescriptionReturnOrderDtID);
                            entityDt.GCPrescriptionReturnOrderStatus = Constant.OrderStatus.CANCELLED;
                            entityDt.GCDeleteReason = Constant.DeleteReason.OTHER;
                            entityDt.DeleteReason = "HEADER IS CANCELLED";
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
            }
            return result;
        }
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = false;
            try
            {
                if (hdnPrescriptionReturnOrderID.Value != "")
                {
                    string filterExpression = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0 ORDER BY PrescriptionReturnOrderDtID DESC", hdnPrescriptionReturnOrderID.Value);
                    List<vPrescriptionReturnOrderDt> lstEntity = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression);
                    if (lstEntity != null)
                    {
                        Int32 PrescriptionReturnOrderID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
                        PrescriptionReturnOrderHd entity = BusinessLayer.GetPrescriptionReturnOrderHd(PrescriptionReturnOrderID);
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            string filterDt = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", entity.PrescriptionReturnOrderID);
                            List<PrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionReturnOrderDtList(filterDt);
                            foreach (PrescriptionReturnOrderDt entityDt in lstEntityDt)
                            {
                                entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePrescriptionReturnOrderDt(entityDt);
                            }

                            entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePrescriptionReturnOrderHd(entity);

                            result = true;
                        }
                        else
                        {
                            errMessage = "Order Retur Resep " + entity.PrescriptionReturnOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = "Tidak ada obat/alkes yang dikembalikan";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (OnSaveEditRecordEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            try
            {
                if (BusinessLayer.GetPrescriptionReturnOrderHd(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PrescriptionReturnOrderDt entity = BusinessLayer.GetPrescriptionReturnOrderDt(Convert.ToInt32(hdnEntryID.Value));
                    if (!entity.IsDeleted)
                    {
                        List<GetPrescriptionReturnOrderRemainingQty> lstEntity = BusinessLayer.GetPrescriptionReturnOrderRemainingQtyList(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(cboLocation.Value));
                        GetPrescriptionReturnOrderRemainingQty entityCheck = lstEntity.Where(t => t.ItemID == entity.ItemID).FirstOrDefault();

                        Decimal maxQty = (entity.ItemQty * -1);
                        if (entityCheck != null)
                        {
                            maxQty = (entityCheck.RemainingChargesQty + (entity.ItemQty * -1));
                        }

                        if (Convert.ToDecimal(txtReturnQty.Value) >= 0)
                        {
                            entity.ItemQty = Convert.ToDecimal(txtReturnQty.Value) * -1;
                        }
                        else
                        {
                            entity.ItemQty = Convert.ToDecimal(txtReturnQty.Value);
                        }

                        if (maxQty + entity.ItemQty >= 0)
                        {
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.LastUpdatedDate = DateTime.Now;
                            BusinessLayer.UpdatePrescriptionReturnOrderDt(entity);
                        }
                        else
                        {
                            result = false;
                            errMessage = "Jumlah Retur Resep Melebihi Batas Maksimal";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                        }
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Order Retur Resep " + BusinessLayer.GetPrescriptionReturnOrderHd(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value)).PrescriptionReturnOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }


        private bool OnDeleteEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderHdDao entityHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao entityDtDao = new PrescriptionReturnOrderDtDao(ctx);
            try
            {
                if (entityHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PrescriptionReturnOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Order Retur Resep " + entityHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value)).PrescriptionReturnOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}