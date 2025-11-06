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
    public partial class MedicalFolderTrackingList : BasePageCheckRegisteredPatient //BasePagePatientPageList
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
            cboPatientType.SelectedIndex = 0;

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
            Helper.SetControlEntrySetting(txtPicker, new ControlEntrySetting(true, true, true), "mpPatientEntry");

            txtDateToDay.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeToDay.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            txtFromRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            grdRegisteredPatient.InitializeControl();

            hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;

            if (cboPatientFrom.Value == null)
                filterExpression = string.Format("DepartmentID != '{0}' AND (VisitDate BETWEEN '{1}' AND '{2}') AND GCVisitStatus != '{3}'", Constant.Facility.PHARMACY, Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            else
                filterExpression = string.Format("DepartmentID = '{0}'  AND (VisitDate BETWEEN '{1}' AND '{2}') AND GCVisitStatus != '{3}'", cboPatientFrom.Value, Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);

            if (cboPatientType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND (RegistrationMRFileStatus IS NULL)");
            }
            else
            {
                filterExpression += string.Format(" AND (RegistrationMRFileStatus IN ('{0}','{1}'))", Constant.MedicalFileStatus.CHECK_OUT, Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN);
            }

            if (cboPatientStatus.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND IsNewPatient = 1");
            }
            else if (cboPatientStatus.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND IsNewPatient = 0");
            }
            filterExpression += string.Format(" AND MRN IS NOT NULL");
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
                RegistrationDao registrationDao = new RegistrationDao(ctx);
                try
                {
                    List<vConsultVisit3> lstVisitRecord = BusinessLayer.GetvConsultVisit3List(string.Format("VisitID IN ({0})", hdnParam.Value), ctx);
                    List<Patient> lstPatient = BusinessLayer.GetPatientList(string.Format("MRN IN (SELECT MRN FROM vConsultVisit3 WHERE VisitID IN ({0}))", hdnParam.Value), ctx);

                    DateTime logDate = DateTime.Now.Date;
                    string logTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                    if (!chkIsUsedCurrentDateTime.Checked)
                    {
                        logDate = Helper.GetDatePickerValue(txtDateToDay);
                        logTime = txtTimeToDay.Text;
                    }

                    foreach (vConsultVisit3 visitRecord in lstVisitRecord)
                    {
                        MRTrackingLog tracking = new MRTrackingLog();
                        tracking.MRN = visitRecord.MRN;
                        tracking.LogDate = logDate;
                        tracking.LogTime = logTime;
                        tracking.VisitID = visitRecord.VisitID;
                        tracking.GCMedicalFileStatus = cboJenisBerkas.Value.ToString();
                        tracking.TransporterName = txtPicker.Text;
                        tracking.Remarks = txtRemarks.Text;
                        tracking.CreatedBy = AppSession.UserLogin.UserID;

                        trackingDao.Insert(tracking);

                        Patient entityPatient = lstPatient.FirstOrDefault(p => p.MRN == visitRecord.MRN);
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

                        string filterExp = string.Format("RegistrationID = {0}", visitRecord.RegistrationID);
                        Registration oRegistration = BusinessLayer.GetRegistrationList(filterExp, ctx).FirstOrDefault();
                        if (oRegistration != null)
                        {
                            oRegistration.GCMedicalFileStatus = cboJenisBerkas.Value.ToString();
                            if (cboJenisBerkas.Value.ToString() == Constant.MedicalFileStatus.CHECK_OUT)
                            {
                                oRegistration.MedicalFileCheckOutBy = txtPicker.Text;
                                oRegistration.MedicalFileCheckOutDate = logDate;
                                oRegistration.MedicalFileCheckOutTime = logTime;
                            }
                            else if (cboJenisBerkas.Value.ToString() == Constant.MedicalFileStatus.PROCESSED)
                            {
                                oRegistration.MedicalFileCheckInBy = txtPicker.Text;
                                oRegistration.MedicalFileCheckInDate = logDate;
                                oRegistration.MedicalFileCheckInTime = logTime;
                            }
                            registrationDao.Update(oRegistration);
                        }
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
    }
}