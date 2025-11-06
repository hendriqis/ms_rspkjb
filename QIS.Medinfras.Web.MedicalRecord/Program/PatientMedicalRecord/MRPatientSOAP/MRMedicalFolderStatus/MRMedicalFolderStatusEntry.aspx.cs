using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MRMedicalFolderStatusEntry : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }

        public string GetMedicalFileStatusReturnToBin()
        {
            return Constant.MedicalFileStatus.RETURN_TO_BIN;
        }

        public string GetMedicalFileStatusReturnToPhysician()
        {
            return Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN;
        }

        public string GetReasonOther()
        {
            return Constant.ReturnReason.LAIN_LAIN;
        }

        public string GetReasonBerkasKurangLengkap()
        {
            return Constant.ReturnReason.BERKAS_KURANG_LENGKAP;
        }
        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(7);

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            string filterExpression = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.RM_OUTPATIENT_USING_EMR, Constant.SettingParameter.RM_EMR_MEDICAL_FORM_TYPE);
            List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(filterExpression);
            bool isUsingEMR = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.RM_OUTPATIENT_USING_EMR).FirstOrDefault().ParameterValue == "1" ? true : false;
            string medicalFormType = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.RM_EMR_MEDICAL_FORM_TYPE).FirstOrDefault().ParameterValue;

            filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEDICAL_FOLDER_TYPE, Constant.StandardCode.MEDICAL_FILE_STATUS, Constant.StandardCode.RETURN_REASON);
            List<StandardCode> lstStd = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboFolderType, lstStd.Where(x => x.ParentID == Constant.StandardCode.MEDICAL_FOLDER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicalFileStatus, lstStd.Where(x => x.ParentID == Constant.StandardCode.MEDICAL_FILE_STATUS && (x.StandardCodeID != Constant.MedicalFileStatus.CHECK_OUT && x.StandardCodeID != Constant.MedicalFileStatus.PROCESSED)).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReason, lstStd.Where(x => x.ParentID == Constant.StandardCode.RETURN_REASON).ToList(), "StandardCodeName", "StandardCodeID");

            cboFolderType.SelectedIndex = 0;
            cboMedicalFileStatus.SelectedIndex = 0;

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                if (isUsingEMR)
                {
                    StandardCode entitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", medicalFormType)).FirstOrDefault();
                    if (entitySC != null)
                    {
                        List<StandardCode> lstStdMain = lstStd.Where(x => x.ParentID == Constant.StandardCode.MEDICAL_FOLDER_TYPE).ToList();
                        lstStdMain.Add(entitySC);
                        Methods.SetComboBoxField<StandardCode>(cboFolderType, lstStdMain, "StandardCodeName", "StandardCodeID");

                        cboFolderType.Value = entitySC.StandardCodeName;
                    }
                }
            }

            txtTransporterName.Text = AppSession.UserLogin.UserFullName;

            Patient entityPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
            string lastCheckedInfo = string.Empty;
            if (entityPatient != null)
            {
                lastCheckedInfo = string.IsNullOrEmpty(entityPatient.MedicalFileLastCheckedBy) ? string.Empty : string.Format("{0}", entityPatient.MedicalFileLastCheckedBy);
            }

            txtLastCheckedInfo.Text = lastCheckedInfo;

            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();

            String filterExpressionLog = string.Format("visitID = {0}", AppSession.RegisteredPatient.VisitID);
            List<vMRTrackingLogView> entitylog = BusinessLayer.GetvMRTrackingLogViewList(filterExpressionLog, 1, 1, " ID DESC");
            if (entitylog.Count != 0)
            {
                txtLastCheckedDateInfo.Text = String.Format("{0} {1}", entitylog.FirstOrDefault().LogDate.ToString(Constant.FormatString.DATE_FORMAT), entitylog.FirstOrDefault().LogTime);
                txtMedicalFileStatus.Text = String.Format("{0}", entitylog.FirstOrDefault().MedicalFileStatus);
                if (entitylog.FirstOrDefault().GCMedicalFileStatus == Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN)
                {
                    trReturnToName.Style.Remove("display");
                    txtReturnToName.Text = entitylog.FirstOrDefault().ReturnToName;
                }
                else
                {
                    trReturnToName.Style.Add("display", "none");
                    txtReturnToName.Text = "";
                }

                txtReturnReason.Text = entitylog.FirstOrDefault().ReturnReasonName;
                cboFolderType.Value = entitylog.FirstOrDefault().GCMedicalFolderType;
                cboMedicalFileStatus.Value = entitylog.FirstOrDefault().GCMedicalFileStatus;
                txtParamedicName.Text = entitylog.FirstOrDefault().ReturnToName;
                cboReason.Value = entitylog.FirstOrDefault().GCReturnReason;
                txtRemarks.Text = entitylog.FirstOrDefault().Remarks;
                txtTransporterName.Text = entitylog.FirstOrDefault().TransporterName;
            }

            hdnFilterExpression.Value = filterExpression;

            BindGridView(1, true, ref PageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public string GetFilterExpression()
        {
            string filterExpression = string.Format("GCMedicalFolderType = '{0}' ", cboFolderType.Value);
            return filterExpression;
        }

        private List<VisitMRFolderStatus> lstVisitMRFolderStatus = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMedicalRecordFolderRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstVisitMRFolderStatus = BusinessLayer.GetVisitMRFolderStatusList(string.Format("VisitID = {0} AND GCMedicalFolderType = '{1}'", hdnVisitID.Value, cboFolderType.Value));
            List<vMedicalRecordFolder> lstEntity = BusinessLayer.GetvMedicalRecordFolderList(filterExpression + "  ORDER BY FormCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vMedicalRecordFolder entity = e.Row.DataItem as vMedicalRecordFolder;
                if (entity != null)
                {
                    List<StandardCode> lstStatusNotes = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.JENIS_CATATAN_STATUS_RM));
                    DropDownList cboStatusNotes = e.Row.FindControl("cboMRStatusNotes") as DropDownList;
                    cboStatusNotes.DataValueField = "StandardCodeID";
                    cboStatusNotes.DataTextField = "StandardCodeName";
                    cboStatusNotes.DataSource = lstStatusNotes;
                    cboStatusNotes.DataBind();
                    CheckBox chkIsExist = e.Row.FindControl("chkIsExist") as CheckBox;
                    CheckBox chkIsCompleted = e.Row.FindControl("chkIsCompleted") as CheckBox;
                    TextBox txtMRStatusNoteOthers = e.Row.FindControl("txtMRStatusNoteOthers") as TextBox;
                    TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
                    TextBox txtFormDate = e.Row.FindControl("txtFormDate") as TextBox;
                    TextBox txtFormTime = e.Row.FindControl("txtFormTime") as TextBox;

                    VisitMRFolderStatus visitMRFolderStatus = lstVisitMRFolderStatus.FirstOrDefault(p => p.FormID == entity.FormID);
                    if (visitMRFolderStatus != null)
                    {
                        chkIsExist.Checked = visitMRFolderStatus.IsExists;
                        chkIsCompleted.Checked = visitMRFolderStatus.IsCompleted;
                        cboStatusNotes.SelectedValue = visitMRFolderStatus.GCMRStatusNote;
                        txtMRStatusNoteOthers.Text = visitMRFolderStatus.MRStatusNoteOthers;
                        txtRemarks.Text = visitMRFolderStatus.Remarks;
                        txtFormDate.Text = visitMRFolderStatus.FormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtFormTime.Text = visitMRFolderStatus.FormTime.ToString();

                        if (cboStatusNotes.SelectedValue != Constant.MRStatusNote.OTHERS)
                        {
                            txtMRStatusNoteOthers.Style.Add("style", "display:none;");
                        }
                        else
                        {
                            txtMRStatusNoteOthers.Style.Remove("display");
                        }
                    }
                }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VisitMRFolderStatusDao itemVisitDao = new VisitMRFolderStatusDao(ctx);
            MRTrackingLogDao MRTrackingLogDaoObj = new MRTrackingLogDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            try
            {
                string filterExpressionVisit = GetFilterExpression();
                List<VisitMRFolderStatus> lstVisitMRFolderStatus = BusinessLayer.GetVisitMRFolderStatusList(string.Format("VisitID = {0} AND GCMedicalFolderType = '{1}'", hdnVisitID.Value, cboFolderType.Value), ctx);

                #region MRTrackingLog
                MRTrackingLog oLog = new MRTrackingLog();
                oLog.MRN = Convert.ToInt32(hdnMRN.Value);
                oLog.LogDate = DateTime.Now;
                oLog.LogTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                oLog.VisitID = Convert.ToInt32(hdnVisitID.Value);
                oLog.TransporterName = txtTransporterName.Text;
                oLog.GCMedicalFileStatus = cboMedicalFileStatus.Value.ToString();
                if (cboMedicalFileStatus.Value.ToString() == Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN)
                {
                    oLog.ReturnToName = txtParamedicName.Text;
                    oLog.GCReturnReason = cboReason.Value.ToString();
                    if (cboReason.Value.ToString() == Constant.ReturnReason.LAIN_LAIN)
                    {
                        oLog.Remarks = txtRemarks.Text;
                    }
                }
                else
                {
                    Registration oRegistration = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
                    if (oRegistration != null)
                    {
                        oRegistration.GCMedicalFileStatus = Constant.MedicalFileStatus.RETURN_TO_BIN;
                        oRegistration.MedicalFileCheckInBy = txtTransporterName.Text;
                        oRegistration.MedicalFileCheckInDate = DateTime.Now; ;
                        oRegistration.MedicalFileCheckInTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        registrationDao.Update(oRegistration);
                    }
                }

                oLog.GCMedicalFolderType = cboFolderType.Value.ToString();
                oLog.CreatedBy = AppSession.UserLogin.UserID;
                oLog.LastUpdatedBy = AppSession.UserLogin.UserID;
                MRTrackingLogDaoObj.Insert(oLog);

                Patient entityPatient = patientDao.Get(oLog.MRN);
                entityPatient.MedicalFileLastCheckedBy = AppSession.UserLogin.UserFullName;
                entityPatient.MedicalFileLastCheckedDate = DateTime.Now.Date;
                patientDao.Update(entityPatient);
                #endregion

                string[] listParam = hdnParam.Value.Split('|');
                string listCheckedExists = "";
                foreach (string param in listParam)
                {
                    string[] listIsi = param.Split(',');
                    int formID = Convert.ToInt32(listIsi[0]);
                    VisitMRFolderStatus visitStatus = lstVisitMRFolderStatus.FirstOrDefault(p => p.FormID == formID);
                    if (visitStatus == null)
                    {
                        visitStatus = new VisitMRFolderStatus();
                        visitStatus.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        visitStatus.GCMedicalFolderType = cboFolderType.Value.ToString();
                        visitStatus.FormID = formID;
                        visitStatus.IsExists = (listIsi[1] == "1");
                        visitStatus.IsCompleted = (listIsi[2] == "1");
                        visitStatus.GCMRStatusNote = listIsi[3];
                        visitStatus.MRStatusNoteOthers = listIsi[4];
                        visitStatus.Remarks = listIsi[5];
                        if (!string.IsNullOrEmpty(listIsi[6]))
                            visitStatus.FormDate = Helper.GetDatePickerValue(listIsi[6]);
                        if (!string.IsNullOrEmpty(listIsi[7]))
                            visitStatus.FormTime = listIsi[7];

                        visitStatus.CreatedBy = AppSession.UserLogin.UserID;
                        itemVisitDao.Insert(visitStatus);
                    }
                    else
                    {
                        visitStatus.IsExists = (listIsi[1] == "1");
                        visitStatus.IsCompleted = (listIsi[2] == "1");
                        visitStatus.GCMRStatusNote = listIsi[3];
                        visitStatus.MRStatusNoteOthers = listIsi[4];
                        visitStatus.Remarks = listIsi[5];
                        if (!string.IsNullOrEmpty(listIsi[6]))
                            visitStatus.FormDate = Helper.GetDatePickerValue(listIsi[6]);
                        if (!string.IsNullOrEmpty(listIsi[7]))
                            visitStatus.FormTime = listIsi[7];
                        visitStatus.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemVisitDao.Update(visitStatus);
                    }

                    if (listCheckedExists != "")
                        listCheckedExists += ",";
                    listCheckedExists += formID;
                }

                List<VisitMRFolderStatus> lstDeletedVisitMRFolderStatus = BusinessLayer.GetVisitMRFolderStatusList(string.Format(
                                                                                            "VisitID = {0} AND GCMedicalFolderType = '{1}' AND FormID NOT IN ({2}) AND IsExists = 1",
                                                                                            hdnVisitID.Value, cboFolderType.Value, listCheckedExists), ctx);
                foreach (VisitMRFolderStatus deletedVisitMR in lstDeletedVisitMRFolderStatus)
                {
                    deletedVisitMR.IsExists = deletedVisitMR.IsCompleted = false;
                    deletedVisitMR.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemVisitDao.Update(deletedVisitMR);
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
    }
}