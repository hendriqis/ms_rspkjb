using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MedicalFolderTrackingAppointmentList : BasePageCheckRegisteredPatient //BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_FOLDER_TRACKING;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            txtPicker.Text = AppSession.UserLogin.UserFullName;

            List<StandardCode> lstPatientType = new List<StandardCode>();
            List<StandardCode> lstPatientStatus = new List<StandardCode>();

            lstPatientType.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Pasien Masuk" });
            lstPatientType.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Pasien Keluar" });
            //lstPatientType.Add(new StandardCode() { StandardCodeID = "3", StandardCodeName = "Peminjaman Berkas" });

            lstPatientStatus.Add(new StandardCode() { StandardCodeID = "0", StandardCodeName = "Semua" });
            lstPatientStatus.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Pasien Baru" });
            lstPatientStatus.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Pasien Lama" });

            Methods.SetComboBoxField<StandardCode>(cboPatientType, lstPatientType, "StandardCodeName", "StandardCodeID");
            cboPatientType.SelectedIndex = 2;

            Methods.SetComboBoxField<StandardCode>(cboPatientStatus, lstPatientStatus, "StandardCodeName", "StandardCodeID");
            cboPatientStatus.SelectedIndex = 0;

            List<StandardCode> lstJBerkas = BusinessLayer.GetStandardCodeList(string.Format("ParentID ='{0}'  AND IsDeleted = 0", Constant.StandardCode.MEDICAL_FILE_STATUS));
            Methods.SetComboBoxField<StandardCode>(cboJenisBerkas, lstJBerkas.Where(sc => sc.StandardCodeID != Constant.MedicalFileStatus.RETURN_TO_BIN && sc.StandardCodeID != Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN).ToList(), "StandardCodeName", "StandardCodeID");
            cboJenisBerkas.SelectedIndex = 0;

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.PHARMACY));

            lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
            cboPatientFrom.SelectedIndex = 0;

            Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

            Helper.SetControlEntrySetting(txtDateToDay, new ControlEntrySetting(true, true, true), "mpPatientEntry");
            Helper.SetControlEntrySetting(txtTimeToDay, new ControlEntrySetting(true, true, true), "mpPatientEntry");
            Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpPatientEntry");
            Helper.SetControlEntrySetting(txtEndTime, new ControlEntrySetting(true, true, true), "mpPatientEntry");
            Helper.SetControlEntrySetting(txtPicker, new ControlEntrySetting(true, true, true), "mpPatientEntry");

            Helper.SetControlEntrySetting(txtDateToDay, new ControlEntrySetting(true, true, true), "mpPatientEntry");
            Helper.SetControlEntrySetting(txtTimeToDay, new ControlEntrySetting(true, true, true), "mpPatientEntry");

            txtDateToDay.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeToDay.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtEndTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            txtFromRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            grdAppointmentPatient.InitializeControl();

            hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnIsPrintWhenProcess.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_CETAK_BUKTI_PENDAFTARAN_KETIKA_KONTROL_BERKAS_APPOINTMENT).ParameterValue;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;

            if (cboPatientFrom.Value == null)
                filterExpression = string.Format("DepartmentID != '{0}' AND (Convert(DATE,StartDate) BETWEEN '{1}' AND '{2}' AND StartTime BETWEEN '{3}' AND '{4}') AND GCAppointmentStatus NOT IN ('{3}','{4}')", Constant.Facility.PHARMACY, Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtStartTime.Text, txtEndTime.Text, Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED);
            else
                filterExpression = string.Format("DepartmentID = '{0}'  AND (Convert(DATE,StartDate) BETWEEN '{1}' AND '{2}' AND StartTime BETWEEN '{3}' AND '{4}') AND GCAppointmentStatus NOT IN ('{5}','{6}')", cboPatientFrom.Value, Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtStartTime.Text, txtEndTime.Text, Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED);

            if (cboPatientType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND (GCMedicalFileStatus IS NULL)");
            }
            else
            {
                filterExpression += string.Format(" AND (GCMedicalFileStatus IN ('{0}','{1}'))", Constant.MedicalFileStatus.CHECK_OUT, Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN);
            }

            if (cboPatientStatus.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND IsNewPatient = 1");
            }
            else if (cboPatientStatus.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND IsNewPatient = 0");
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                MRTrackingLogDao trackingDao = new MRTrackingLogDao(ctx);
                PatientDao patientDao = new PatientDao(ctx);
                AppointmentDao appointmentDao = new AppointmentDao(ctx);
                try
                {
                    List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID IN ({0})", hdnParam.Value), ctx);
                    List<Patient> lstPatient = BusinessLayer.GetPatientList(string.Format("MRN IN (SELECT MRN FROM vAppointment WHERE AppointmentID IN ({0}))", hdnParam.Value), ctx);

                    string logDate = Helper.GetDatePickerValue(txtDateToDay).ToString(Constant.FormatString.DATE_FORMAT_112);
                    string logTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                    if (!chkIsUsedCurrentDateTime.Checked)
                    {
                        logDate = txtDateToDay.Text;
                        logTime = txtTimeToDay.Text;
                    }

                    foreach (vAppointment appointmentRecord in lstAppointment)
                    {
                        MRTrackingLog tracking = new MRTrackingLog();
                        tracking.AppointmentID = appointmentRecord.AppointmentID;
                        tracking.MRN = appointmentRecord.MRN;
                        tracking.LogDate = Convert.ToDateTime(txtDateToDay.Text);
                        tracking.LogTime = logTime;
                        tracking.VisitID = null;
                        tracking.GCMedicalFileStatus = cboJenisBerkas.Value.ToString();
                        tracking.TransporterName = txtPicker.Text;
                        tracking.Remarks = txtRemarks.Text;
                        tracking.CreatedBy = AppSession.UserLogin.UserID;

                        trackingDao.Insert(tracking);

                        Patient entityPatient = lstPatient.FirstOrDefault(p => p.MRN == appointmentRecord.MRN);
                        if (cboJenisBerkas.Value.ToString() == Constant.MedicalFileStatus.CHECK_OUT)
                        {
                            entityPatient.GCMedicalFileStatus = Constant.MedicalFileStatus.PROCESSED;
                        }
                        else
                        {
                            entityPatient.GCMedicalFileStatus = Constant.MedicalFileStatus.CHECK_OUT;
                        }

                        entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDao.Update(entityPatient);

                        string filterExp = string.Format("AppointmentID = {0}", appointmentRecord.AppointmentID);
                        Appointment oAppointment = BusinessLayer.GetAppointmentList(filterExp, ctx).FirstOrDefault();
                        if (oAppointment != null)
                        {
                            oAppointment.GCMedicalFileStatus = cboJenisBerkas.Value.ToString();
                            //if (cboJenisBerkas.Value.ToString() == Constant.MedicalFileStatus.CHECK_OUT)
                            //{
                            //    oRegistration.MedicalFileCheckOutBy = txtPicker.Text;
                            //    oRegistration.MedicalFileCheckOutDate = logDate;
                            //    oRegistration.MedicalFileCheckOutTime = logTime;
                            //}
                            //else if (cboJenisBerkas.Value.ToString() == Constant.MedicalFileStatus.PROCESSED)
                            //{
                            //    oRegistration.MedicalFileCheckInBy = txtPicker.Text;
                            //    oRegistration.MedicalFileCheckInDate = logDate;
                            //    oRegistration.MedicalFileCheckInTime = logTime;
                            //}
                            appointmentDao.Update(oAppointment);
                        }
                    }
                    if (hdnIsPrintWhenProcess.Value == "1")
                    {
                        PrintTracerAppointment(lstAppointment);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        private void PrintTracerAppointment(List<vAppointment> oAppointment)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    //List<vAppointment> oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID IN ({0})", id));
                    if (oAppointment != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN_PERJANJIAN);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintBuktiPerjanjian1(oAppointment, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
        }
    }
}