using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CreateClaimCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnIsBridgingToEKlaim.Value = AppSession.IsBridgingToEKlaim ? "1" : "0";

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN));
            hdnPatientSearchDialogType.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN).ParameterValue;

            txtDOBCtl.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterGender = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.GENDER);
            List<StandardCode> lstGender = BusinessLayer.GetStandardCodeList(filterGender);
            Methods.SetComboBoxField<StandardCode>(cboGenderCtl, lstGender, "StandardCodeName", "StandardCodeID");
            cboGenderCtl.SelectedIndex = 0;
        }
    }
}