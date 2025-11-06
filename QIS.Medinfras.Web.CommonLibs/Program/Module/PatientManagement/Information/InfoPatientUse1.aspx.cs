using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientUse1 : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_INFORMATION_PATIENT_USE1;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_INFORMATION_PATIENT_USE1;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.INFORMATION_PATIENT_USE1;
                    default:
                        return Constant.MenuCode.Inpatient.INFORMATION_PATIENT_USE1;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();

            hdnRegistrationID.Value = entityReg.RegistrationID.ToString();
            hdnDepartmentID.Value = entityReg.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entityReg.HealthcareServiceUnitID.ToString();

            BindGridDetail();

        }

        private void BindGridDetail()
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            List<vPatientUse> lst = BusinessLayer.GetvPatientUseList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}