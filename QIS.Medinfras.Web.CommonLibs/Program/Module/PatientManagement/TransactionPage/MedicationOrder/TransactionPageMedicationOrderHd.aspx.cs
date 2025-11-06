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
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageMedicationOrderHd : BasePageTrx
    {
        private vConsultVisit entityCV = null;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PRESCRIPTION_ORDER;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PRESCRIPTION_ORDER;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_PRESCRIPTION_ORDER;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_PRESCRIPTION_ORDER;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_PRESCRIPTION_ORDER;
                default: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_PRESCRIPTION_ORDER;
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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
        }

        protected override void SetControlProperties()
        {
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0 AND IsUsingRegistration = 1"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (cboDispensaryUnit.Value == null && hdnDefaultDispensaryServiceUnitID.Value != "0")
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;
            BindCboLocation();
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (cboDispensaryUnit.Value != null)
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

                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(cboDispensaryUnit.Value));
                hdnIPAddress.Value = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboDispensaryUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnDefaultVisitParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultVisitParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultVisitParamedicName.Value));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        protected bool IsEditable = true;
        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            if (hdnDefaultDispensaryServiceUnitID.Value != "0")
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;
            else
                cboDispensaryUnit.SelectedIndex = -1;
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
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvPrescriptionOrderHdRowCount(filterExpression);
        }

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
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            txtPrescriptionOrderNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            txtNotes.Text = entity.Remarks;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
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
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(PrescriptionOrderHd entityHd)
        {
            entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entityHd.PrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
            entityHd.PrescriptionTime = Request.Form[txtPrescriptionTime.UniqueID];
            entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
            entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
            entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
            entityHd.GCPrescriptionType = Constant.PrescriptionType.MEDICATION_ORDER;
            entityHd.Remarks = txtNotes.Text;
        }

        public void SavePrescriptionOrderHd(IDbContext ctx, ref int prescriptionOrderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderHd entityHd = null;
            if (hdnPrescriptionOrderID.Value == "0")
            {
                entityHd = new PrescriptionOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                ControlToEntity(entityHd);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
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
                int PrescriptionOrderID = 0;
                SavePrescriptionOrderHd(ctx, ref PrescriptionOrderID);

                retval = PrescriptionOrderID.ToString();
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
            bool result = true;
            try
            {
                PrescriptionOrderHd entityHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityHd);
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderHd(entityHd);
                }
                else 
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
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
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
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
                    ctx.RollBackTransaction();
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                }
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                try
                {
                    Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                    PrescriptionOrderHd entity = entityHdDao.Get(PrescriptionOrderID);
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
            try
            {
                Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(PrescriptionOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                    entity.SendOrderDateTime = DateTime.Now.Date;
                    entity.SendOrderBy = AppSession.UserLogin.UserID;
                    entity.ProposedBy = AppSession.UserLogin.UserID;
                    entity.ProposedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderHd(entity);

                    try
                    {
                        if (!String.IsNullOrEmpty(hdnIPAddress.Value))
                        {
                            SendNotification(entity);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    return true;
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void SendNotification(PrescriptionOrderHd order)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No : {0}", order.PrescriptionOrderNo));
            sbMessage.AppendLine(string.Format("Fr : {0}", txtPhysicianName.Text));
            sbMessage.AppendLine(string.Format("Px : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("R/ :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(hdnIPAddress.Value), Convert.ToInt16(hdnPort.Value));
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                { 
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}