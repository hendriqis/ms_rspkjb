using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class SpecialtyEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.SPECIALTY;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                Specialty entity = BusinessLayer.GetSpecialty(ID);
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtSpecialtyID.Focus();
            GetSettingParameter();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.SPECIALTY_GROUP));
            lst.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCSpecialtyGroup, lst, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCaseType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.VISIT_CASE_TYPE));
            lstCaseType.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboVisitCaseType, lstCaseType, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstGroupRL = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.RL_REPORT_GROUP_3_12));
            lstGroupRL.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboRLReportGroup, lstGroupRL, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSpecialtyID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSpecialtyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCSpecialtyGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboVisitCaseType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboRLReportGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo1Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo1Name, new ControlEntrySetting(false, false, false));
        }

        private void EntityToControl(Specialty entity)
        {
            txtSpecialtyID.Text = entity.SpecialtyID;
            txtSpecialtyName.Text = entity.SpecialtyName;
            txtSpecialtyName2.Text = entity.SpecialtyName2;
            cboGCSpecialtyGroup.Value = entity.GCSpecialtyGroup;
            cboVisitCaseType.Value = entity.GCCaseType;
            cboRLReportGroup.Value = entity.GCRLReportGroup;
            txtBPJSReferenceInfo1Code.Text = entity.BPJSReferenceInfo;
            txtBPJSReferenceInfo.Text = entity.BPJSReferenceInfo;

            if (!string.IsNullOrEmpty(entity.BPJSReferenceInfo))
            {
                if (entity.BPJSReferenceInfo.Contains('|'))
                {
                    string[] bpjsInfo = entity.BPJSReferenceInfo.Split('|');
                    txtBPJSReferenceInfo1Code.Text = bpjsInfo[0];
                    txtBPJSReferenceInfo1Name.Text = bpjsInfo[1];
                }
                else
                {
                    txtBPJSReferenceInfo1Code.Text = entity.BPJSReferenceInfo;
                    txtBPJSReferenceInfo1Name.Text = string.Empty;
                }
            }
            else
            {
                txtBPJSReferenceInfo1Code.Text = string.Empty;
                txtBPJSReferenceInfo1Name.Text = string.Empty;
            }
        }

        private void ControlToEntity(Specialty entity)
        {
            entity.SpecialtyName = txtSpecialtyName.Text;
            entity.SpecialtyName2 = txtSpecialtyName2.Text;
            entity.BPJSReferenceInfo = txtBPJSReferenceInfo1Code.Text;

            if (cboGCSpecialtyGroup.Value != null)
            {
                entity.GCSpecialtyGroup = cboGCSpecialtyGroup.Value.ToString();
            }

            if (cboVisitCaseType.Value != null)
            {
                entity.GCCaseType = cboVisitCaseType.Value.ToString();
            }

            if (cboRLReportGroup.Value != null)
            {
                entity.GCRLReportGroup = cboRLReportGroup.Value.ToString();
            }

            if (!string.IsNullOrEmpty(txtBPJSReferenceInfo1Code.Text))
            {
                entity.BPJSReferenceInfo = string.Format("{0}|{1}", txtBPJSReferenceInfo1Code.Text, txtBPJSReferenceInfo1Name.Text);
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SpecialtyID = '{0}'", txtSpecialtyID.Text);
            List<Specialty> lst = BusinessLayer.GetSpecialtyList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Procedure with Code " + txtSpecialtyID.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Specialty entity = new Specialty();
                ControlToEntity(entity);
                entity.SpecialtyID = txtSpecialtyID.Text;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertSpecialty(entity);
                retval = entity.SpecialtyID;
                BridgingToMedinfrasMobileApps(entity.SpecialtyID, entity.SpecialtyName, entity.SpecialtyName2, "001");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Specialty entity = BusinessLayer.GetSpecialty(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSpecialty(entity);
                BridgingToMedinfrasMobileApps(entity.SpecialtyID, entity.SpecialtyName, entity.SpecialtyName2, "002");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private void BridgingToMedinfrasMobileApps(string specialtyID, string specialtyName, string specialtyName2, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (specialtyID != string.Empty)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnSpecialtyMasterChanged(specialtyID, specialtyName, specialtyName2, eventType);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}