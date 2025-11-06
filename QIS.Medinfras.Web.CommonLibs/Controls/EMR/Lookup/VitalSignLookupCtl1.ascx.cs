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
    public partial class VitalSignLookupCtl1 : BaseProcessPopupCtl
    {
        protected int PageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnIsNewRecord.Value = paramInfo[0] == "0" ? "1" : "0";

            string _visitNoteID = paramInfo[1];
            hdnIsInitialAssessment.Value = paramInfo[2];
            if (hdnIsInitialAssessment.Value == "1")
            {
                string noteType = Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT;
                if (paramInfo[2] == "2")
                    noteType = Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT;

                if (_visitNoteID == "0" && _visitNoteID != "")
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
                hdnLinkChiefComplaintID.Value = paramInfo[5];
            }

            if (paramInfo.Length >= 8)
            {
                hdnLinkSurgeryAnesthesyStatusID.Value = paramInfo[7];
            }

            if (paramInfo.Length >= 9)
            {
                hdnLinkPreAnesthesyAssesmentID.Value = paramInfo[8];
            }
            if (paramInfo.Length >= 10)
            {
                hdnLinkPreSurgeryAssessmentID.Value = paramInfo[9];
            }

            txtObservationDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.RegistrationID)).FirstOrDefault();
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            if (entityRegistration.LinkedRegistrationID != null)
            {
                ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                filterExpression = string.Format("VisitID IN ({0},{1}) AND IsDeleted = 0", entityVisit.VisitID, entityLinkedentityVisit.VisitID);
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0},{1}) ORDER BY DisplayOrder", entityVisit.VisitID, entityLinkedentityVisit.VisitID));
            }
            else
            {
                filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", entityVisit.VisitID);
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) ORDER BY DisplayOrder", entityVisit.VisitID));
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ObservationDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
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
                string filterExpression = string.Format("VisitID = {0} AND ID = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, historyID);
                if (entityRegistration.LinkedRegistrationID != null)
                {
                    ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                    filterExpression = string.Format("VisitID IN ({0},{1}) AND ID = '{2}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, entityLinkedentityVisit.VisitID, historyID);
                }
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterExpression);
                if (lstVitalSignDt.Count > 0)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
                    VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);

                    try
                    {
                        VitalSignHd entity = new VitalSignHd();
                        List<VitalSignDt> lstEntityDt = new List<VitalSignDt>();
                        ControlToEntity(entity, lstEntityDt, lstVitalSignDt);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        int headerID = entityDao.InsertReturnPrimaryKeyID(entity);

                        foreach (VitalSignDt entityDt in lstEntityDt)
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

        private void ControlToEntity(VitalSignHd entity, List<VitalSignDt> lstEntityDt, List<vVitalSignDt> lstHistoryItem)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;
            entity.PatientVisitNoteID = Convert.ToInt32(hdnVisitNoteID.Value);
            entity.Remarks = txtRemarks.Text;
            entity.IsInitialAssessment = hdnIsInitialAssessment.Value == "1" ? true : false;
            if (hdnLinkMedicalResumeID.Value != "0" && hdnLinkMedicalResumeID.Value != "" && hdnLinkMedicalResumeID.Value != null)
            {
                entity.MedicalResumeID = Convert.ToInt32(hdnLinkMedicalResumeID.Value);
            }
            else
            {
                entity.MedicalResumeID = null;
            }

            if (hdnLinkChiefComplaintID.Value != "0" && hdnLinkChiefComplaintID.Value != "" && hdnLinkChiefComplaintID.Value != null)
            {
                entity.ChiefComplaintID = Convert.ToInt32(hdnLinkChiefComplaintID.Value);
            }
            else
            {
                entity.ChiefComplaintID = null;
            }

            if (hdnLinkPreSurgeryAssessmentID.Value != "0" && hdnLinkPreSurgeryAssessmentID.Value != "" && hdnLinkPreSurgeryAssessmentID.Value != null)
            {
                entity.PreSurgeryAssessmentID = Convert.ToInt32(hdnLinkPreSurgeryAssessmentID.Value);
            }
            else
            {
                entity.PreSurgeryAssessmentID = null;
            }

            if (hdnLinkSurgeryAnesthesyStatusID.Value != "0" && hdnLinkSurgeryAnesthesyStatusID.Value != "" && hdnLinkSurgeryAnesthesyStatusID.Value != null)
            {
                entity.SurgeryAnesthesyStatusID = Convert.ToInt32(hdnLinkSurgeryAnesthesyStatusID.Value);
            }
            else
            {
                entity.SurgeryAnesthesyStatusID = null;
            }

            if (hdnLinkPreAnesthesyAssesmentID.Value != "0" && hdnLinkPreAnesthesyAssesmentID.Value != "" && hdnLinkPreAnesthesyAssesmentID.Value != null)
            {
                entity.PreAnesthesyAssessmentID = Convert.ToInt32(hdnLinkPreAnesthesyAssesmentID.Value);
            }
            else
            {
                entity.PreAnesthesyAssessmentID = null;
            }

            #region Vital Sign Dt
            string summaryText = string.Empty;
            foreach (vVitalSignDt item in lstHistoryItem)
            {
                string itemID = item.VitalSignID.ToString();
                string itemValue = item.VitalSignValue;

                VitalSignDt entityDt = new VitalSignDt();
                entityDt.VitalSignID = Convert.ToInt32(item.VitalSignID);


                if (item.GCValueType == Constant.ControlType.COMBO_BOX)
                    itemValue = item.GCVitalSignValue;

                entityDt.VitalSignValue = itemValue;

                if (entityDt.VitalSignValue != "")
                    lstEntityDt.Add(entityDt);
            }
            #endregion
        }
    }
}