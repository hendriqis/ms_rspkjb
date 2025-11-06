using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TreatmentProcedureCtl : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        protected int ParamedicID = 0;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                if (!param.Contains('|'))
                {
                    hdnID.Value = param;
                }
                else
                {
                    string[] paramArr = param.Split('|');
                    hdnID.Value = paramArr[0];
                    hdnDepartmentID.Value = paramArr[1];
                    if (!string.IsNullOrEmpty(hdnID.Value))
                    {
                        IsAdd = false;
                        SetControlProperties();
                        PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnID.Value));
                        EntityToControl(entity);
                    }
                    else
                    {
                        hdnID.Value = "";
                        IsAdd = true;
                        SetControlProperties();
                    }
                }
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            ParamedicID = AppSession.RegisteredPatient.ParamedicID;
            ledProcedure.FilterExpression = string.Format("IsDeleted = '0'");

            txtObservationDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = AppSession.RegisteredPatient.VisitTime;

            string filterParamedic = string.Empty;
            if (hdnDepartmentID.Value != "md")
            {
                filterParamedic = string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WITH(NOLOCK) WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID);
            }
            else 
            {
                filterParamedic = string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WITH(NOLOCK) WHERE HealthcareServiceUnitID = {0})", AppSession.RegisteredPatient.HealthcareServiceUnitID);
                if (AppSession.UserLogin.ParamedicID > 0)
                {
                    ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                }
            }

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterParamedic);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                //int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                //cboParamedicID.ClientEnabled = false;
                //cboParamedicID.Value = userLoginParamedic.ToString();
                List<vParamedicMaster> lstParamLogin = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN ({1})", Constant.ParamedicType.Physician, AppSession.UserLogin.ParamedicID));
                Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamLogin, "ParamedicName", "ParamedicID");
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(ddlYear, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProcedureText, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(PatientProcedure entity)
        {
            txtObservationDate.Text = entity.ProcedureDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ProcedureTime;

            string filterParamedic = string.Empty;
            if (hdnDepartmentID.Value != "md")
            {
                filterParamedic = string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN ({1})", Constant.ParamedicType.Physician, entity.ParamedicID);
            }
            else
            {
                filterParamedic = string.Format("ParamedicID IN ({0})", entity.ParamedicID);
                if (AppSession.UserLogin.ParamedicID > 0)
                {
                    ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                }
            }

            List<vParamedicMaster> lstParamLogin = BusinessLayer.GetvParamedicMasterList(filterParamedic);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamLogin, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = entity.ParamedicID.ToString();
            //cboParamedicID.Value = entity.ParamedicID;

            hdnProcedureID.Value = entity.ProcedureID;
            txtProcedureText.Text = entity.ProcedureText;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(PatientProcedure entity)
        {
            entity.ProcedureDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ProcedureTime = txtObservationTime.Text;

            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.ProcedureID = hdnProcedureID.Value;
            entity.ProcedureText = txtProcedureText.Text;
            entity.Remarks = txtRemarks.Text;
            entity.IsCreatedBySystem = false;
            entity.ReferenceID = null;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientProcedure entity = new PatientProcedure();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientProcedure(entity);
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
                PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientProcedure(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}