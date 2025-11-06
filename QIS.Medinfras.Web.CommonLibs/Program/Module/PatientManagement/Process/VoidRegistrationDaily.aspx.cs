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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VoidRegistrationDaily : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.VOID_REGISTRATION_DAILY;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.VOID_REGISTRATION_DAILY;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.VOID_REGISTRATION_DAILY;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.VOID_REGISTRATION_DAILY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.VOID_REGISTRATION_DAILY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.VOID_REGISTRATION_DAILY;
                    return Constant.MenuCode.MedicalDiagnostic.VOID_REGISTRATION_DAILY;
                default: return Constant.MenuCode.Outpatient.VOID_REGISTRATION_DAILY;
            }
        }

        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
            hdnDepartmentID.Value = Page.Request.QueryString["id"];

            if (hdnDepartmentID.Value == Constant.Facility.LABORATORY || hdnDepartmentID.Value == Constant.Facility.IMAGING)
            {
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
            }

            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value, "IsUsingRegistration = 1");
            lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Bisa Diproses" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Bisa Diproses" });
            Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");
            cboResultType.Value = "2";

            IsLoadFirstRecord = false;

            SetControlVisibility();

            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                trServiceUnit.Style.Add("display", "none");
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                {
                    trServiceUnit.Style.Add("display", "none");
                }
                else
                {
                    List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID));
                    lstHealthcareServiceUnit = lstHealthcareServiceUnit.OrderBy(unit => unit.ServiceUnitName).ToList();
                    lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                    Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;
                }
            }
            else
            {
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value));
                lstHealthcareServiceUnit = lstHealthcareServiceUnit.OrderBy(unit => unit.ServiceUnitName).ToList();
                lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
            }

            txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}', '{3}', '{4}', '{5}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.RM_VOID_REGISTRASI_VALIDASI_DATA_CHIEF_COMPLAINT, //1
                                                            Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, //2
                                                            Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, //3
                                                            Constant.SettingParameter.RM_VOID_REGISTRASI_VALIDASI_DATA_CHIEF_COMPLAINT, //4
                                                            Constant.SettingParameter.RM_IS_VOID_REG_DELETE_LINKEDREG //5
                                                        ));

            hdnMenggunakanValidasiChiefComplaint.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_VOID_REGISTRASI_VALIDASI_DATA_CHIEF_COMPLAINT).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnIsVoidRegistrationDeleteLinkedReg.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_VOID_REG_DELETE_LINKEDREG).FirstOrDefault().ParameterValue;

            BindGridView(CurrPage, true, ref PageCount);

            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

        }

        private void SetControlVisibility()
        {
            switch (hdnDepartmentID.Value)
            {
                case "INPATIENT": trRegistrationDate.Style.Add("display", "none"); break;
                case "EMERGENCY": trServiceUnit.Style.Add("display", "none"); break;
                case "DIAGNOSTIC": if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                        trServiceUnit.Style.Add("display", "none"); break;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string GetServiceUnitLabel()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return GetLabel("Ruang Perawatan");
                case Constant.Facility.DIAGNOSTIC: return GetLabel("Penunjang Medis");
                case Constant.Facility.PHARMACY: return GetLabel("Farmasi");
                default: return GetLabel("Klinik");
            }
        }

        protected override void SetControlProperties()
        {
            cboServiceUnit.SelectedIndex = 0;
            txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtProcessedPatient.Text = processedPatient.ToString();
            //txtOutstandingPatient.Text = outstandingPatient.ToString();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtProcessedPatient, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(txtOutstandingPatient, new ControlEntrySetting(false, false, false));
        }

        protected void cbpCloseRegistrationGetNumPatient_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int processedPatient = 0;
            int outstandingPatient = 0;
            GetNumPatient(ref processedPatient, ref outstandingPatient);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = string.Format("{0}|{1}", processedPatient, outstandingPatient);
        }

        private void GetNumPatient(ref int processedPatient, ref int outstandingPatient)
        {
            string filterExpression = string.Format("RegistrationDate = '{0}' AND GCRegistrationStatus != '{1}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hsu.HealthcareServiceUnitID);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                else
                {
                    Int32 serviceUnit = Convert.ToInt32(cboServiceUnit.Value);
                    if (serviceUnit > 0)
                        filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", serviceUnit);
                    else
                        filterExpression += string.Format(" AND RegistrationID IN (SELECT RegistrationID FROM vRegistration WHERE DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2}))", hdnDepartmentID.Value, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                }
            }
            else
            {
                Int32 serviceUnit = Convert.ToInt32(cboServiceUnit.Value);
                if (serviceUnit > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", serviceUnit);
                else
                    filterExpression += string.Format(" AND RegistrationID IN (SELECT RegistrationID FROM vRegistration WHERE DepartmentID = '{0}')", hdnDepartmentID.Value);
            }
            List<String> lstGCRegistration = BusinessLayer.GetRegistrationGCRegistrationStatusList(filterExpression);
            processedPatient = lstGCRegistration.Where(p => p == Constant.VisitStatus.CLOSED).Count();
            outstandingPatient = lstGCRegistration.Count - processedPatient;
        }

        private string GetFilterExpression(string resultType = "")
        {
            string filterExpression = "";

            #region filter Service Unit

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                {
                    filterExpression += string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                }
                else
                {
                    filterExpression += string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1}, {2})", Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID);
                }
            }
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            }
            else
            {
                int cboServiceUnitVal = Convert.ToInt32(cboServiceUnit.Value);
                if (cboServiceUnitVal != 0)
                    filterExpression += string.Format("HealthcareServiceUnitID = {0}", cboServiceUnitVal);
                else
                    filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            }

            #endregion

            #region filter Tanggal Registrasi

            filterExpression += string.Format(" AND RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            #endregion

            #region filter Status Registrasi

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND GCRegistrationStatus IN ('{0}')", Constant.VisitStatus.OPEN);
            }
            else
            {
                filterExpression += string.Format(" AND GCRegistrationStatus IN ('{0}', '{1}', '{2}')", Constant.VisitStatus.OPEN, Constant.VisitStatus.CHECKED_IN, Constant.VisitStatus.RECEIVING_TREATMENT);
            }

            #endregion

            #region filter Tampilan Hasil (Bisa Diproses / Belum)

            if (resultType == "")
            {
                resultType = cboResultType.Value.ToString();
            }

            if (resultType == "1")
            {
                filterExpression += " AND ((ServiceOrder + PrescriptionOrder + PrescriptionReturnOrder + LaboratoriumOrder + RadiologiOrder + OtherOrder + Charges + Billing + Payment + PatientVisitNote + ChiefComplaint + NurseChiefComplaint) > 0)";
            }
            else if (resultType == "2")
            {
                filterExpression += " AND ((ServiceOrder + PrescriptionOrder + PrescriptionReturnOrder + LaboratoriumOrder + RadiologiOrder + OtherOrder + Charges + Billing + Payment + PatientVisitNote + ChiefComplaint + NurseChiefComplaint) = 0)";
            }

            #endregion

            //#region filter ChiefComplaint & NurseChiefComplaint

            //filterExpression += " AND ChiefComplaint = 0";

            //if (hdnMenggunakanValidasiChiefComplaint.Value == "1")
            //{
            //    filterExpression += " AND NurseChiefComplaint = 0 AND VitalSign = 0";
            //}

            //#endregion

            #region Quick Filter

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            #endregion

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationOutstandingInfo4RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRegistrationOutstandingInfo4> lstEntity = BusinessLayer.GetvRegistrationOutstandingInfo4List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationID ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "voidregistration")
            {
                bool result = true;

                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityDao = new RegistrationDao(ctx);
                ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
                PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
                PatientPaymentHdDao patientPaymentHdDao = new PatientPaymentHdDao(ctx);
                AppointmentDao appointmentDao = new AppointmentDao(ctx);
                ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
                NurseChiefComplaintDao nurseChiefComplaintDao = new NurseChiefComplaintDao(ctx);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                BedDao entityBedDao = new BedDao(ctx);

                try
                {
                    List<vRegistrationOutstandingInfo4> lstVisit = BusinessLayer.GetvRegistrationOutstandingInfo4List(GetFilterExpression());
                    if (lstVisit.Count > 0)
                    {
                        //string lstRegistrationID = "";
                        foreach (vRegistrationOutstandingInfo4 visit in lstVisit)
                        {

                            string filterTransfer = string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", visit.RegistrationID, Constant.PatientTransferStatus.OPEN);
                            List<PatientTransfer> lstTransfer = BusinessLayer.GetPatientTransferList(filterTransfer, ctx);
                            if (lstTransfer.Count == 0)
                            {

                                // cek apa ada outstanding charges, order, payment
                                string filterOutstanding = string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", visit.RegistrationID, Constant.VisitStatus.CANCELLED);
                                List<vRegistrationAllInfo> lstInfo = BusinessLayer.GetvRegistrationAllInfoList(filterOutstanding, ctx);
                                int oCharges = 0, oTestOrder = 0, oPrescriptionOrder = 0, oPrescriptionReturnOrder = 0, oServiceOrder = 0, oPayment = 0;
                                foreach (vRegistrationAllInfo info in lstInfo)
                                {
                                    oCharges += info.Charges;
                                    oTestOrder += info.TestOrder;
                                    oPrescriptionOrder += info.PrescriptionOrder;
                                    oPrescriptionReturnOrder += info.PrescriptionReturnOrder;
                                    oServiceOrder += info.ServiceOrder;
                                    oPayment += info.Payment;
                                }
                                bool outstanding = (oCharges + oTestOrder + oPrescriptionOrder + oPrescriptionReturnOrder + oServiceOrder + oPayment > 0);

                                if (!outstanding)
                                {

                                    Registration entity = entityDao.Get(Convert.ToInt32(visit.RegistrationID));
                                    if (entity.SourceAmount == 0)
                                    {
                                        #region Cek CPPT
                                        // list transaksi
                                        string filterTransaction = string.Format("RegistrationID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND IsDeleted = 0", visit.RegistrationID, Constant.TransactionStatus.VOID);
                                        List<vPatientChargesDt3> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt3List(filterTransaction, ctx);
                                        List<vPatientChargesDt3> lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => !t.IsCreatedBySystem || (t.PatientBillingID != 0)).ToList();

                                        // list chief complaint
                                        string filterCC = string.Format("VisitID = {0} AND IsDeleted = 0", visit.VisitID);
                                        List<ChiefComplaint> lstCC = BusinessLayer.GetChiefComplaintList(filterCC, ctx);

                                        // list nurse chief complaint
                                        string filterNCC = string.Format("VisitID = {0} AND IsDeleted = 0", visit.VisitID);
                                        List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(filterCC, ctx);

                                        // list patient visit note
                                        string filterPVN = string.Format("VisitID = {0} AND IsDeleted = 0", visit.VisitID);
                                        List<PatientVisitNote> listPVN = BusinessLayer.GetPatientVisitNoteList(filterCC, ctx);

                                        if ((lstCC.Count() + lstNCC.Count() + listPVN.Count()) > 0)
                                        {
                                            result = false;
                                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_REGISTRATION_CANNOT_VOID_BECAUSE_CHIEF_COMPLAINT_ANAMNESA);
                                            Exception ex = new Exception(errMessage);
                                            Helper.InsertErrorLog(ex);
                                            ctx.RollBackTransaction();
                                            break;
                                        }

                                        //foreach (ChiefComplaint a in lstCC)
                                        //{
                                        //    a.IsDeleted = true;
                                        //    a.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        //    chiefComplaintDao.Update(a);
                                        //}

                                        //foreach (NurseChiefComplaint b in lstNCC)
                                        //{
                                        //    b.IsDeleted = true;
                                        //    b.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        //    nurseChiefComplaintDao.Update(b);
                                        //}

                                        //foreach (PatientVisitNote c in listPVN)
                                        //{
                                        //    c.IsDeleted = true;
                                        //    c.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        //    patientVisitNoteDao.Update(c);
                                        //}
                                        #endregion

                                        #region Update Data Linked To Registration
                                        if (entity.LinkedRegistrationID != null)
                                        {
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            Registration entityLinkedFrom = entityDao.Get(Convert.ToInt32(entity.LinkedRegistrationID));

                                            entityLinkedFrom.LinkedToRegistrationID = null;
                                            entityLinkedFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityDao.Update(entityLinkedFrom);
                                        }
                                        #endregion

                                        #region Update Data Auto Bill Item (PatientCharges)

                                        lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => t.IsCreatedBySystem).ToList();
                                        List<string> lstTransactionID = new List<string>();
                                        foreach (vPatientChargesDt3 entityDt in lstPatientChargesDtTemp)
                                        {
                                            PatientChargesDt entityPatientChargesDt = patientChargesDtDao.Get(entityDt.ID);
                                            entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                            entityPatientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            patientChargesDtDao.Update(entityPatientChargesDt);
                                            if (!lstTransactionID.Any(t => t == entityDt.TransactionID.ToString()))
                                            {
                                                lstTransactionID.Add(entityDt.TransactionID.ToString());
                                            }
                                        }

                                        foreach (string entityTransactionID in lstTransactionID)
                                        {
                                            List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = '{0}'", entityTransactionID), ctx).ToList();
                                            if (lstChargesDt.Where(t => t.GCTransactionDetailStatus != Constant.TransactionStatus.VOID).Count() == 0)
                                            {
                                                PatientChargesHd entityPatientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(entityTransactionID));
                                                entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                                entityPatientChargesHd.VoidBy = AppSession.UserLogin.UserID;
                                                entityPatientChargesHd.VoidDate = DateTime.Now;
                                                entityPatientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                patientChargesHdDao.Update(entityPatientChargesHd);
                                            }
                                        }
                                        #endregion

                                        #region Update Data Status Registrasi (Registration)

                                        if (hdnIsVoidRegistrationDeleteLinkedReg.Value == "1")
                                        {
                                            entity.LinkedRegistrationID = null;
                                        }

                                        entity.GCRegistrationStatus = Constant.VisitStatus.CANCELLED;
                                        entity.GCVoidReason = Constant.DeleteReason.OTHER;
                                        entity.VoidReason = "Void dari menu Proses Batal Pendaftaran";
                                        entity.VoidBy = AppSession.UserLogin.UserID;
                                        entity.VoidDate = DateTime.Now;
                                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDao.Update(entity);

                                        #endregion

                                        #region Update Data Penunggu Pasien (PatientAccompany)
                                        if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                                        {
                                            List<PatientAccompany> lstPatientAccompany = BusinessLayer.GetPatientAccompanyList(String.Format("RegistrationID = {0}", visit.RegistrationID), ctx);
                                            if (lstPatientAccompany.Count > 0)
                                            {
                                                foreach (PatientAccompany e in lstPatientAccompany)
                                                {
                                                    Bed bedPA = entityBedDao.Get(Convert.ToInt32(e.BedID));
                                                    bedPA.RegistrationID = null;
                                                    bedPA.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                                    bedPA.IsPatientAccompany = false;
                                                    bedPA.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityBedDao.Update(bedPA);
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Update Data Status Visit (ConsultVisit + Bed)
                                        List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", visit.RegistrationID), ctx);
                                        foreach (ConsultVisit consultVisit in lstConsultVisit)
                                        {
                                            consultVisit.GCVisitStatus = Constant.VisitStatus.CANCELLED;
                                            consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            consultVisitDao.Update(consultVisit);

                                            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                                            {
                                                //Bed entityBed = entityBedDao.Get((int)consultVisit.BedID);
                                                Bed entityBed = BusinessLayer.GetBedList(String.Format("BedID = {0} AND IsPatientAccompany = 0", (int)consultVisit.BedID), ctx).FirstOrDefault();
                                                if (entityBed.RegistrationID == entity.RegistrationID)
                                                {
                                                    entityBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                                    entityBed.RegistrationID = null;
                                                    entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityBedDao.Update(entityBed);
                                                }
                                            }
                                            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                                            {
                                                String BedID = Convert.ToString(consultVisit.BedID);
                                                if (BedID != "")
                                                {
                                                    Bed entityBed = BusinessLayer.GetBedList(String.Format("BedID = {0} AND IsPatientAccompany = 0", (int)consultVisit.BedID), ctx).FirstOrDefault();
                                                    entityBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                                    entityBed.RegistrationID = null;
                                                    entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityBedDao.Update(entityBed);
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Update Data Appointment
                                        if (entity.AppointmentID != null)
                                        {
                                            Appointment appointment = BusinessLayer.GetAppointment(Convert.ToInt32(entity.AppointmentID));
                                            if (!appointment.IsAutoAppointment)
                                            {
                                                appointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                                appointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                appointmentDao.Update(appointment);
                                            }
                                            else
                                            {
                                                appointment.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                                appointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                appointmentDao.Update(appointment);
                                            }
                                        }
                                        #endregion

                                        #region Bridging Gateway

                                        if (hdnIsBridgingToGateway.Value == "1")
                                        {
                                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                                            {
                                                string queue = string.Empty;
                                                GatewayService oService = new GatewayService();
                                                APIMessageLog entityAPILog = new APIMessageLog();
                                                entityAPILog.Sender = "MEDINFRAS";
                                                entityAPILog.Recipient = "QUEUE ENGINE";
                                                vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
                                                if (entityReg != null)
                                                {
                                                    string apiResult = oService.GetQueueNoByVoidRegistration(entityReg.MedicalNo, entityReg.RegistrationDate, entityReg.ParamedicCode, entityReg.ServiceUnitCode, entityReg.RegistrationTime, Convert.ToString(entityReg.QueueNo), entityReg.HealthcareServiceUnitID.ToString());
                                                    string[] apiResultInfo = apiResult.Split('|');
                                                    if (apiResultInfo[0] == "0")
                                                    {
                                                        queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                                                        entityAPILog.MessageDateTime = DateTime.Now;
                                                        entityAPILog.IsSuccess = false;
                                                        entityAPILog.MessageText = apiResultInfo[2];
                                                        entityAPILog.Response = apiResultInfo[1];
                                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                                        Exception ex = new Exception(apiResultInfo[1]);
                                                        Helper.InsertErrorLog(ex);
                                                    }
                                                    else
                                                    {
                                                        queue = apiResult;
                                                        entityAPILog.MessageDateTime = DateTime.Now;
                                                        entityAPILog.MessageText = apiResultInfo[2];
                                                        entityAPILog.Response = apiResultInfo[1];
                                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                                    }
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }

                                        #endregion

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_REGISTRATION_CANNOT_VOID_BECAUSE_HAS_SOURCE_AMOUNT);
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        ctx.RollBackTransaction();
                                        break;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = ("Kunjungan tidak dapat dibatalkan karena sudah memiliki order / transaksi / pembayaran.");
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                    break;
                                }
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = ("Kunjungan tidak dapat dibatalkan karena sudah memiliki order / transaksi / pembayaran.");
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
            return false;
        }
    }
}