using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CopyPreProcedureChecklistEntry : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = true;
                SetControlProperties(paramInfo);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormGroup.Value = paramInfo[1];
            hdnFormType.Value = paramInfo[2];
            hdnReferenceID.Value = paramInfo[3];
            hdnPopupVisitID.Value = paramInfo[4];
            hdnPopupRegistrationID.Value = paramInfo[10];

            txtObservationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            divFormContent.InnerHtml = paramInfo[8];
            hdnFormLayout.Value = paramInfo[8];
            hdnFormValues.Value = paramInfo[9];


            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.ClientEnabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(PreProcedureChecklist entity)
        {
            entity.ChecklistDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ChecklistTime = txtObservationTime.Text;
            entity.GCChecklistGroup = hdnFormGroup.Value;
            entity.GCChecklistType = hdnFormType.Value;
            entity.RegistrationID = Convert.ToInt32(hdnPopupRegistrationID.Value);
            entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            if (string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
            {
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            }
            else
            {
                entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            }
            entity.TestOrderID = Convert.ToInt32(hdnReferenceID.Value);
            entity.ChecklistFormLayout = hdnFormLayout.Value;
            entity.ChecklistFormValue = hdnFormValues.Value;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.CreatedBy = AppSession.UserLogin.UserID;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PreProcedureChecklistDao entityDao = new PreProcedureChecklistDao(ctx);
            try
            {
                PreProcedureChecklist entity = new PreProcedureChecklist();
                ControlToEntity(entity);
                retVal = string.Format("checklist|{0}", entityDao.InsertReturnPrimaryKeyID(entity).ToString());

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                retVal = string.Format("checklist|{0}", "0");
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}