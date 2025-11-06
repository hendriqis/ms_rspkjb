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
using System.Data;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MedicalFolderStatusEntry : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_FOLDER_STATUS;
        }

        public string GetMedicalFileStatusReturn()
        {
            return Constant.MedicalFileStatus.RETURN_TO_BIN;
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
            if (Page.Request.QueryString.Count > 0)
            {
                hdnVisitID.Value = Page.Request.QueryString["id"];

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                ctlPatientBanner.InitializePatientBanner(entity);
                string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEDICAL_FOLDER_TYPE, Constant.StandardCode.MEDICAL_FILE_STATUS, Constant.StandardCode.RETURN_REASON);
                List<StandardCode> lstStd = BusinessLayer.GetStandardCodeList(filterExpression);
                Methods.SetComboBoxField<StandardCode>(cboFolderType, lstStd.Where(x => x.ParentID == Constant.StandardCode.MEDICAL_FOLDER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                Methods.SetComboBoxField<StandardCode>(cboMedicalFileStatus, lstStd.Where(x => x.ParentID == Constant.StandardCode.MEDICAL_FILE_STATUS && (x.StandardCodeID != Constant.MedicalFileStatus.CHECK_OUT && x.StandardCodeID != Constant.MedicalFileStatus.PROCESSED)).ToList(), "StandardCodeName", "StandardCodeID");
                Methods.SetComboBoxField<StandardCode>(cboReason, lstStd.Where(x => x.ParentID == Constant.StandardCode.RETURN_REASON).ToList(), "StandardCodeName", "StandardCodeID");
                cboFolderType.SelectedIndex = 0;

                hdnMRN.Value = entity.MRN.ToString();
                hdnFilterExpression.Value = filterExpression;
                hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;

                BindGridView(1, true, ref PageCount);
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
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
            List<vMedicalRecordFolder> lstEntity = BusinessLayer.GetvMedicalRecordFolderList(filterExpression);
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
                    List<StandardCode> lstStatusNotes = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' ORDER BY StandardCodeID", Constant.StandardCode.JENIS_CATATAN_STATUS_RM));
                    DropDownList cboStatusNotes = e.Row.FindControl("cboMRStatusNotes") as DropDownList;
                    cboStatusNotes.DataValueField = "StandardCodeID";
                    cboStatusNotes.DataTextField = "StandardCodeName";
                    cboStatusNotes.DataSource = lstStatusNotes;
                    cboStatusNotes.DataBind();
                    CheckBox chkIsExist = e.Row.FindControl("chkIsExist") as CheckBox;
                    CheckBox chkIsCompleted = e.Row.FindControl("chkIsCompleted") as CheckBox;
                    TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;

                    VisitMRFolderStatus visitMRFolderStatus = lstVisitMRFolderStatus.FirstOrDefault(p => p.FormID == entity.FormID);
                    if (visitMRFolderStatus != null)
                    {
                        chkIsExist.Checked = visitMRFolderStatus.IsExists;
                        chkIsCompleted.Checked = visitMRFolderStatus.IsCompleted;
                        cboStatusNotes.SelectedValue = visitMRFolderStatus.GCMRStatusNote;
                        txtRemarks.Text = visitMRFolderStatus.Remarks;
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

            try
            {
                string filterExpressionVisit = GetFilterExpression();
                List<VisitMRFolderStatus> lstVisitMRFolderStatus = BusinessLayer.GetVisitMRFolderStatusList(string.Format("VisitID = {0} AND GCMedicalFolderType = '{1}'", hdnVisitID.Value, cboFolderType.Value), ctx);

                #region MRTrackingLog
                MRTrackingLog MRTrackingLogObj = new MRTrackingLog();
                MRTrackingLogObj.MRN = Convert.ToInt32(hdnMRN.Value);
                MRTrackingLogObj.LogDate = DateTime.Now;
                MRTrackingLogObj.LogTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                MRTrackingLogObj.VisitID = Convert.ToInt32(hdnVisitID.Value);
                MRTrackingLogObj.TransporterName = txtTransporterName.Text;
                MRTrackingLogObj.GCMedicalFileStatus = cboMedicalFileStatus.Value.ToString();
                if (cboMedicalFileStatus.Value.ToString() == Constant.MedicalFileStatus.RETURN_TO_PHYSICIAN)
                {
                    MRTrackingLogObj.GCReturnReason = cboReason.Value.ToString();
                    if (cboReason.Value.ToString() == Constant.ReturnReason.LAIN_LAIN)
                        MRTrackingLogObj.Remarks = txtRemarks.Text;
                }
                MRTrackingLogObj.CreatedBy = AppSession.UserLogin.UserID;
                MRTrackingLogObj.LastUpdatedBy = AppSession.UserLogin.UserID;
                MRTrackingLogDaoObj.Insert(MRTrackingLogObj);

                Patient entityPatient = patientDao.Get(MRTrackingLogObj.MRN);
                entityPatient.GCMedicalFileStatus = cboMedicalFileStatus.Value.ToString();
                entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
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
                        visitStatus.Remarks = listIsi[4];

                        visitStatus.CreatedBy = AppSession.UserLogin.UserID;
                        itemVisitDao.Insert(visitStatus);
                    }
                    else
                    {
                        visitStatus.IsExists = (listIsi[1] == "1");
                        visitStatus.IsCompleted = (listIsi[2] == "1");
                        visitStatus.GCMRStatusNote = listIsi[3];
                        visitStatus.Remarks = listIsi[4];
                        visitStatus.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemVisitDao.Update(visitStatus);
                    }

                    if (listCheckedExists != "")
                        listCheckedExists += ",";
                    listCheckedExists += formID;
                }

                List<VisitMRFolderStatus> lstDeletedVisitMRFolderStatus = BusinessLayer.GetVisitMRFolderStatusList(string.Format("VisitID = {0} AND GCMedicalFolderType = '{1}' AND FormID NOT IN ({2}) AND IsExists = 1", hdnVisitID.Value, cboFolderType.Value, listCheckedExists), ctx);
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}