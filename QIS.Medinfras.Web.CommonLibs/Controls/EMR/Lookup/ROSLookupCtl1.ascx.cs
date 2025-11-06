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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ROSLookupCtl1 : BaseProcessPopupCtl
    {
        protected int PageCount = 1;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnIsNewRecord.Value = paramInfo[0] == "0" ? "1" : "0";
            hdnVisitNoteID.Value = paramInfo[1];
            hdnIsInitialAssessment.Value = paramInfo[2];

            if (hdnIsInitialAssessment.Value == "1")
            {
                string noteType = Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT;
                if (paramInfo[2] == "2")
                    noteType = Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT;

                if (hdnVisitNoteID.Value == "0" && hdnVisitNoteID.Value != "")
                {
                    PatientVisitNote obj = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, noteType)).FirstOrDefault();
                    if (obj != null)
                        hdnVisitNoteID.Value = obj.ID.ToString();
                    else
                        hdnVisitNoteID.Value = "0";
                }
                else
                {
                    hdnVisitNoteID.Value = string.IsNullOrEmpty(paramInfo[1]) ? "0" : paramInfo[1];
                }
            }
            else
            {
                hdnVisitNoteID.Value = "0";
            }

            if (paramInfo.Length >= 4)
            {
                hdnLinkMedicalResumeID.Value = paramInfo[3];
            }

            if (paramInfo.Length >= 5)
            {
                hdnLinkPreSurgeryAssessmentID.Value = paramInfo[5];
            }

            txtObservationDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.RegistrationID)).FirstOrDefault();
            string filterExpression = string.Empty;
            if (entityRegistration.LinkedRegistrationID != null)
            {
                ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                filterExpression = string.Format("VisitID IN ({0},{1}) AND IsDeleted = 0", entityVisit.VisitID, entityLinkedentityVisit.VisitID);
                lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID IN ({0},{1}) AND IsDeleted = 0", entityVisit.VisitID, entityLinkedentityVisit.VisitID));
            }
            else
            {
                filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", entityVisit.VisitID);
                lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", entityVisit.VisitID));
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ObservationDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Row.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Row.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = GetReviewOfSystemDt(obj.ID);
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected List<vReviewOfSystemDt> GetReviewOfSystemDt(Int32 ID)
        {
            return lstReviewOfSystemDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                int visitNoteID = Convert.ToInt32(hdnVisitNoteID.Value);
                string referenceNo = string.Empty;

                string processResult = "0|Terjadi kesalahan ketika copy hasil pemeriksaan";
                processResult = CopyFromPreviousRecord(hdnSelectedID.Value);
                string[] resultInfo = ((string)processResult).Split('|');

                if (resultInfo[0] == "1")
                {
                    result = true;
                }
                else
                {
                    result = false;
                    errMessage = resultInfo[1];
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private string CopyFromPreviousRecord(string recordID)
        {
            string result = "0|Terjadi kesalahan ketika copy hasil pemeriksaan";

            if (!string.IsNullOrEmpty(recordID) && recordID != "0")
            {
                int historyID = Convert.ToInt32(recordID);
                Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                string filterExpression = string.Format("VisitID = {0} AND ID = '{1}' AND IsDeleted = 0 ORDER BY GCROSystem", AppSession.RegisteredPatient.VisitID, historyID);
                if (entityRegistration.LinkedRegistrationID != null) 
                {
                    ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                    filterExpression = string.Format("VisitID IN ({0},{1}) AND ID = '{2}' AND IsDeleted = 0 ORDER BY GCROSystem", AppSession.RegisteredPatient.VisitID, entityLinkedentityVisit.VisitID, historyID);
                }
                List<vReviewOfSystemDt> lstDetail = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
                if (lstDetail.Count > 0)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    ReviewOfSystemHdDao entityDao = new ReviewOfSystemHdDao(ctx);
                    ReviewOfSystemDtDao entityDtDao = new ReviewOfSystemDtDao(ctx);

                    try
                    {
                        ReviewOfSystemHd entity = new ReviewOfSystemHd();
                        List<ReviewOfSystemDt> lstEntityDt = new List<ReviewOfSystemDt>();
                        ControlToEntity(entity, lstEntityDt, lstDetail);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        int headerID = entityDao.InsertReturnPrimaryKeyID(entity);

                        foreach (ReviewOfSystemDt entityDt in lstEntityDt)
                        {
                            entityDt.ID = headerID;
                            entityDtDao.Insert(entityDt);
                        }

                        ctx.CommitTransaction   ();
                        result = string.Format("1|{0}",string.Empty);
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        result = string.Format("0|{0}",ex.Message);
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
            }
            else
            {
                result = "0|Record ID untuk hasil pemeriksaan tidak boleh kosong";
            }
            return result;
        }

        private void ControlToEntity(ReviewOfSystemHd entity, List<ReviewOfSystemDt> lstEntityDt, List<vReviewOfSystemDt> lstHistoryItem)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;
            entity.PatientVisitNoteID = Convert.ToInt32(hdnVisitNoteID.Value);
            entity.IsInitialAssessment = hdnIsInitialAssessment.Value == "1" ? true : false;
            if (hdnLinkMedicalResumeID.Value != "0" && hdnLinkMedicalResumeID.Value != "" && hdnLinkMedicalResumeID.Value != null)
            {
                entity.MedicalResumeID = Convert.ToInt32(hdnLinkMedicalResumeID.Value);
            }
            else
            {
                entity.MedicalResumeID = null;
            }
            if (hdnLinkPreSurgeryAssessmentID.Value != "0" && hdnLinkPreSurgeryAssessmentID.Value != "" && hdnLinkPreSurgeryAssessmentID.Value != null)
            {
                entity.PreSurgeryAssessmentID = Convert.ToInt32(hdnLinkPreSurgeryAssessmentID.Value);
            }
            else
            {
                entity.PreSurgeryAssessmentID = null;
            }
            if (hdnLinkChiefComplaintID.Value != "0" && hdnLinkChiefComplaintID.Value != "" && hdnLinkChiefComplaintID.Value != null)
            {
                entity.ChiefComplaintID = Convert.ToInt32(hdnLinkChiefComplaintID.Value);
            }

            #region Detail
            foreach (vReviewOfSystemDt item in lstHistoryItem)
            {
                ReviewOfSystemDt entityDt = new ReviewOfSystemDt();
                entityDt.GCROSystem = item.GCROSystem;
                entityDt.IsNotExamined = item.IsNotExamined;
                entityDt.IsNormal = item.IsNormal;
                entityDt.Remarks = item.Remarks;

                lstEntityDt.Add(entityDt);
            }
            #endregion
        }
    }
}