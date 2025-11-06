using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class CopyVitalSignCtl : BaseProcessPopupCtl
    {
        protected string MRN = "";
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnRecordID.Value = paramInfo[0];
            txtParamedicName.Text = AppSession.UserLogin.UserFullName;
            txtNoteDateTime.Text = string.Format("{0} - {1}", DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

            hdnUserParamedicName.Value = AppSession.UserLogin.UserFullName;
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
                string referenceNo = string.Empty;

                string processResult = "0|Terjadi kesalahan ketika copy hasil pemeriksaan";
                processResult = CopyFromPreviousRecord(hdnRecordID.Value);
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
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ID = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, historyID));
                if (lstVitalSignDt.Count > 0)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
                    VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);

                    try
                    {
                        VitalSignHd oldRecord = BusinessLayer.GetVitalSignHd(Convert.ToInt32(recordID));

                        VitalSignHd entity = new VitalSignHd();
                        List<VitalSignDt> lstEntityDt = new List<VitalSignDt>();
                        ControlToEntity(oldRecord,entity, lstEntityDt, lstVitalSignDt);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        int headerID = entityDao.InsertReturnPrimaryKeyID(entity);

                        foreach (VitalSignDt entityDt in lstEntityDt)
                        {
                            entityDt.ID = headerID;
                            entityDtDao.Insert(entityDt);
                        }

                        ctx.CommitTransaction();
                        result = string.Format("1|{0}", string.Empty);
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        result = string.Format("0|{0}", ex.Message);
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

        private void ControlToEntity(VitalSignHd oldEntity, VitalSignHd entity, List<VitalSignDt> lstEntityDt, List<vVitalSignDt> lstHistoryItem)
        {
            entity.ObservationDate = DateTime.Now.Date;
            entity.ObservationTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entity.IsInitialAssessment = oldEntity.IsInitialAssessment;
            entity.MedicalResumeID = oldEntity.MedicalResumeID;
            entity.NurseAssessmentID = oldEntity.NurseAssessmentID;
            entity.NurseFormAssessmentID = oldEntity.NurseFormAssessmentID;
            entity.PreAnesthesyAssessmentID = oldEntity.PreAnesthesyAssessmentID;
            entity.PreHDAssessmentID = oldEntity.PreHDAssessmentID;
            entity.PreSurgeryAssessmentID = oldEntity.PreSurgeryAssessmentID;
            entity.SurgeryAnesthesyStatusID = oldEntity.SurgeryAnesthesyStatusID;
            entity.TestOrderID = oldEntity.TestOrderID;
            entity.VisitID = oldEntity.VisitID;
            entity.IsMonitoring = oldEntity.IsMonitoring;
            entity.GCMonitoringType = oldEntity.GCMonitoringType;
            entity.Remarks = oldEntity.Remarks;

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