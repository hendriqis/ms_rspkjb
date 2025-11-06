using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class DifferentDiagnoseList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        protected string IsMainDiagnosisExists = "0";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.DIFFERENTIAL_DIAGNOSIS;
        }

        #region List
        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();
            IsMainDiagnosisExists = lstMainDiagnosis.Count > 0 ? "1" : "0";

            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (lstEntity.Count > 0)
                grdView.SelectedIndex = 0;
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
            panel.JSProperties["cpRetval"] = IsMainDiagnosisExists ;
        }
        
        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                int recordID = Convert.ToInt32(hdnID.Value);
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);
                if (entity != null)
                {
                    if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                    {
                        errMessage = string.Format("Data diagnosa pasien ini tidak dapat dihapus karena sudah dilengkapi oleh Casemix.");
                        return false;
                    }
                    else
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientDiagnosis(entity);

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1(3, entity);
                            }
                        }

                        return true;
                    }
                }
                else
                {
                    errMessage = string.Format("Invalid Patient Diagnosis Record Information");
                    return false;
                }
            }
            else
            {
                errMessage = string.Format("Invalid Patient Diagnosis Record Information");
                return false;
            }
        }
        #endregion

        #region Entry

        protected override void SetControlProperties()
        {
            txtDifferentialDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDifferentialTime.Text = AppSession.RegisteredPatient.VisitTime;

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDiagnoseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDifferentialDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDifferentialTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDiagnoseType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(ledDiagnose, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDiagnosisText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsFollowUp, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsChronic, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            entity.DifferentialDate = Helper.GetDatePickerValue(txtDifferentialDate);
            entity.DifferentialTime = txtDifferentialTime.Text;

            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
            IsMainDiagnosisExists = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
            entity.GCDifferentialStatus = cboStatus.Value.ToString();
            entity.GCFinalStatus = cboStatus.Value.ToString();

            if (hdnDiagnoseID.Value != "")
                entity.DiagnoseID = hdnDiagnoseID.Value;
            else
                entity.DiagnoseID = null;

            entity.DiagnosisText = txtDiagnosisText.Text;
            entity.MorphologyID = hdnMorphologyID.Value;
            entity.IsChronicDisease = chkIsChronic.Checked;
            entity.IsFollowUpCase = chkIsFollowUp.Checked;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientDiagnosis entity = new PatientDiagnosis();
                entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
                if (IsValidChange(entity, ref errMessage))
                {
                    ControlToEntity(entity);
                    entity.VisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.InsertPatientDiagnosis(entity);

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(1, entity);
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool IsValidChange(PatientDiagnosis entity, ref string errMessage)
        {
            bool result = true;
            if (entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS || entity.GCDiagnoseType == Constant.DiagnoseType.EARLY_DIAGNOSIS)
            {
                //Check if main-diagnosis already exists
                string filterExpression = String.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, entity.GCDiagnoseType);
                if (hdnEntryID.Value != "") filterExpression += string.Format(" AND ID != {0}", hdnEntryID.Value);
                List<PatientDiagnosis> lstDiagnosis = BusinessLayer.GetPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    if (entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS) errMessage = "There is only one main diagnosis allow for each episode";
                    else errMessage = "There is only one early diagnosis allow for each episode";
                    result = false;
                    
                }
                else
                    result = true;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnEntryID.Value));
                entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
                if (IsValidChange(entity, ref errMessage))
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientDiagnosis(entity);

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(2, entity);
                        }
                    }

                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected string GetIsMainDiagnosisExist()
        {
            return IsMainDiagnosisExists;
        }

        #endregion

        private void BridgingToMedinfrasV1(int ProcessType, PatientDiagnosis entity)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = oService.OnSendPatientDiagnoseInformation(ProcessType, entity);
            string[] serviceResultInfo = serviceResult.Split('|');
            if (serviceResultInfo[0] == "1")
            {
                apiLog.IsSuccess = true;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
            }
            else
            {
                apiLog.IsSuccess = false;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
                apiLog.ErrorMessage = serviceResultInfo[2];
            }
            BusinessLayer.InsertAPIMessageLog(apiLog);
        }
    }
}