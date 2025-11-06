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
    public partial class PrepareRevenueSharingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParamedicID.Value = param;

            txtValueDateFrom.Text = DateTime.Now.AddDays(-14).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTanggalDari.Value = DateTime.Now.AddDays(-14).ToString(Constant.FormatString.DATE_FORMAT_112);
            hdnTanggalDariString.Value = txtValueDateFrom.Text;
            txtValueDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTanggalSampai.Value = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
            hdnTanggalSampaiString.Value = txtValueDateTo.Text;

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.Value = Constant.Facility.OUTPATIENT;
            hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
        }

        protected void cbpRevenueSharing_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}