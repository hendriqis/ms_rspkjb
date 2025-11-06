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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CloseRegistration : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.CLOSE_REGISTRATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.CLOSE_REGISTRATION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.CLOSE_REGISTRATION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.CLOSE_REGISTRATION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.CLOSE_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.CLOSE_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.CLOSE_REGISTRATION;
                    return Constant.MenuCode.MedicalDiagnostic.CLOSE_REGISTRATION;
                default: return Constant.MenuCode.Outpatient.CLOSE_REGISTRATION;
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
            if (hdnDepartmentID.Value == LABORATORY || hdnDepartmentID.Value == IMAGING)            
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

            List<Variable> lstFilterDateType = new List<Variable>();
            lstFilterDateType.Add(new Variable { Code = "1", Value = "Tanggal Registrasi" });
            lstFilterDateType.Add(new Variable { Code = "2", Value = "Tanggal Pulang" });
            Methods.SetComboBoxField<Variable>(cboFilterDateType, lstFilterDateType, "Value", "Code");
            cboFilterDateType.Value = "1";

            List<Variable> lstResultType = new List<Variable>();
            lstResultType.Add(new Variable { Code = "0", Value = "Semua" });
            lstResultType.Add(new Variable { Code = "1", Value = "Belum Bisa Diproses" });
            lstResultType.Add(new Variable { Code = "2", Value = "Sudah Bisa Diproses" });
            Methods.SetComboBoxField<Variable>(cboResultType, lstResultType, "Value", "Code");
            cboResultType.Value = "2";

            IsLoadFirstRecord = false;

            SetControlVisibility();

            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value, "IsUsingRegistration = 1");
            lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

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
                    List<vHealthcareServiceUnitCustom> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID));
                    lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                    Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;
                }
            }
            else
            {
                //List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value));
                //lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                //Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                //cboServiceUnit.SelectedIndex = 0;
            }

            txtFilterDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(CurrPage, true, ref PageCount);
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            SettingParameterDt setpar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_CHECK_TEST_RESULT);
            hdnIsCheckResultTest.Value = setpar.ParameterValue;
        }

        private void SetControlVisibility()
        {
            switch (hdnDepartmentID.Value)
            {
                case "EMERGENCY": trServiceUnit.Style.Add("display", "none"); break;
                case "DIAGNOSTIC": if(AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic) 
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
                case Constant.Facility.OUTPATIENT: return GetLabel("Klinik");
                case Constant.Facility.PHARMACY: return GetLabel("Farmasi");
                default: return GetLabel("Klinik");
            }
        }

        protected override void SetControlProperties()
        {
            cboServiceUnit.SelectedIndex = 0;
            txtFilterDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFilterDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, false));
        }

        //protected void cbpCloseRegistrationGetNumPatient_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    int processedPatient = 0;
        //    int outstandingPatient = 0;
        //    GetNumPatient(ref processedPatient, ref outstandingPatient);
        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = string.Format("{0}|{1}", processedPatient, outstandingPatient);
        //}

        //private void GetNumPatient(ref int processedPatient, ref int outstandingPatient)
        //{
        //    string filterExpression = string.Format("RegistrationDate = '{0}' AND GCRegistrationStatus != '{1}'", Helper.GetDatePickerValue(txtFilterDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
        //    if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
        //    {
        //        filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.EMERGENCY);
        //    }
        //    else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
        //    {
        //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
        //            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
        //        else
        //        {
        //            Int32 serviceUnit = Convert.ToInt32(cboServiceUnit.Value);
        //            if (serviceUnit > 0)
        //                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", serviceUnit);
        //            else
        //                filterExpression += string.Format(" AND RegistrationID IN (SELECT RegistrationID FROM vRegistration WHERE DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2}))", hdnDepartmentID.Value, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
        //        }
        //    }
        //    else
        //    {
        //        Int32 serviceUnit = Convert.ToInt32(cboServiceUnit.Value);
        //        if (serviceUnit > 0)
        //            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", serviceUnit);
        //        else
        //            filterExpression += string.Format(" AND RegistrationID IN (SELECT RegistrationID FROM vRegistration WHERE DepartmentID = '{0}')", hdnDepartmentID.Value);
        //    }

        //    List<String> lstGCRegistration = BusinessLayer.GetRegistrationGCRegistrationStatusList(filterExpression);
        //    processedPatient = lstGCRegistration.Where(p => p == Constant.VisitStatus.CLOSED).Count();
        //    outstandingPatient = lstGCRegistration.Count - processedPatient;
        //}

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
                {
                    filterExpression += string.Format("HealthcareServiceUnitID = {0}", cboServiceUnitVal);
                }
                else
                {
                    filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
                }
            }

            #endregion

            #region filter Tanggal Registrasi / Tanggal Pulang

            if (cboFilterDateType.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND DischargeDate = '{0}'", Helper.GetDatePickerValue(txtFilterDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else
            {
                filterExpression += string.Format(" AND RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtFilterDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            #endregion

            #region filter Status Registrasi

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
            }
            else
            {
                filterExpression += string.Format(" AND GCRegistrationStatus NOT IN ('{0}', '{1}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
            }

            #endregion

            #region filter Tampilan Hasil (Bisa Diproses / Belum)

            if (resultType == "")
            {
                resultType = cboResultType.Value.ToString();
            }

            if (resultType == "1") // Belum Bisa Diproses
            {
                filterExpression += " AND ServiceOrder + PrescriptionOrder + PrescriptionReturnOrder + LaboratoriumOrder + RadiologiOrder + OtherOrder + Charges + Billing > 0";
            }
            else if (resultType == "2") // Sudah Bisa Diproses
            {
                filterExpression += " AND ServiceOrder + PrescriptionOrder + PrescriptionReturnOrder + LaboratoriumOrder + RadiologiOrder + OtherOrder + Charges + Billing = 0";
            }

            #endregion

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
                int rowCount = BusinessLayer.GetvRegistrationOutstandingInfoRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRegistrationOutstandingInfo> lstEntity = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationID ASC");

            hdnRegistrationIDList.Value = "";
            foreach (vRegistrationOutstandingInfo e in lstEntity) {
                if (hdnRegistrationIDList.Value == "")
                {
                    hdnRegistrationIDList.Value = Convert.ToString(e.RegistrationID);
                }
                else {
                    hdnRegistrationIDList.Value = hdnRegistrationIDList.Value + "," + Convert.ToString(e.RegistrationID);
                }
            }

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
            if (type == "closeregistration")
            {
                bool result = true;

                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityDao = new RegistrationDao(ctx);
                ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
                TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
                ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
                try
                {
                    string filterExpression = string.Format("RegistrationID IN (SELECT RegistrationID FROM vRegistrationOutstandingInfo WHERE {0})", GetFilterExpression("2"));
                    List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(filterExpression, ctx);
                    if (lstRegistration.Count > 0)
                    {
                        foreach (Registration registration in lstRegistration)
                        {
                            registration.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                            registration.ClosedBy = AppSession.UserLogin.UserID;
                            registration.ClosedDate = DateTime.Now;
                            registration.ClosedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            registration.IsBillingClosed = true;
                            registration.BillingClosedBy = AppSession.UserLogin.UserID;
                            registration.BillingClosedDate = DateTime.Now;
                            registration.IsBillingReopen = false;
                            registration.BillingReopenBy = null;
                            registration.BillingReopenDate = null;
                            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDao.Update(registration);

                            List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID IN ({0}) AND GCVisitStatus <> '{1}'", registration.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                            foreach (ConsultVisit consultVisit in lstConsultVisit)
                            {
                                List<TestOrderHd> lstTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", consultVisit.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                foreach (TestOrderHd testOrderHd in lstTestOrderHd)
                                {
                                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    testOrderHd.VoidBy = AppSession.UserLogin.UserID;
                                    testOrderHd.VoidDate = DateTime.Now;
                                    testOrderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                    testOrderHd.VoidReason = "Void dari Proses Harian Tutup Pendaftaran.";
                                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    testOrderHdDao.Update(testOrderHd);
                                }

                                List<ServiceOrderHd> lstServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", consultVisit.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                foreach (ServiceOrderHd serviceOrderHd in lstServiceOrderHd)
                                {
                                    serviceOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    serviceOrderHd.VoidBy = AppSession.UserLogin.UserID;
                                    serviceOrderHd.VoidDate = DateTime.Now;
                                    serviceOrderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                    serviceOrderHd.VoidReason = "Void dari Proses Harian Tutup Pendaftaran.";
                                    serviceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    serviceOrderHdDao.Update(serviceOrderHd);
                                }

                                consultVisit.GCVisitStatus = Constant.VisitStatus.CLOSED;
                                consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                consultVisitDao.Update(consultVisit);
                            }
                        }
                    }

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
            return false;
        }
    }
}