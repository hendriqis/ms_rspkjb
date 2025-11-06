using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HealthyInformationReasonCtl : BaseViewPopupCtl
    {
        string isRadiologi = "";
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
            //vHealthcareServiceUnit entityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
            //List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = {0} AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            //isRadiologi = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;

            //hdnIsLaboratoryUnit.Value = Convert.ToString(entityServiceUnit.IsLaboratoryUnit);

            //if (Convert.ToString(entity.HealthcareServiceUnitID) == isRadiologi)
            //{
            //    hdnIsImagingUnit.Value = "True";
            //}
            //else
            //{
            //    hdnIsImagingUnit.Value = "False";
            //}

            //hdnDepartmentID.Value = entity.DepartmentID;
        }

        protected void cbpPrintPatientLabel_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}