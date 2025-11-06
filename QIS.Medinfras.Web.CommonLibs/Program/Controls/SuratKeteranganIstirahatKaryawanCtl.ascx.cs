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
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SuratKeteranganIstirahatKaryawanCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", param))[0];
            vConsultVisit7 entityVisit = BusinessLayer.GetvConsultVisit7List(string.Format("RegistrationID = {0}", param))[0];
            hdnRegistrationIDVisit.Value = param;
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnHealthcareInitial.Value = healthcare.Initial;

            if (hdnHealthcareInitial.Value == "rsdo-ska")
            {
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                {
                    trHari.Style.Add("display", "none");
                    trTanggal.Style.Add("display", "none");
                    trJam.Style.Add("display", "none");
                }
            }
            else
            {
                trNIK.Style.Add("display", "none");
                trUnit.Style.Add("display", "none");
            }

            Helper.SetControlEntrySetting(txtDay, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDay, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueJam, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueHari, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDateFrom, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDateTo, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            txtValueDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYYDari = txtValueDateFrom.Text.Substring(6, 4);
            string MMDari = txtValueDateFrom.Text.Substring(3, 2);
            string DDDari = txtValueDateFrom.Text.Substring(0, 2);
            string dateALLDari = DDDari + '-' + MMDari + '-' + YYYYDari;
            hdnTanggalDari.Value = dateALLDari;
            hdnTanggalDariString.Value = txtValueDateFrom.Text;

            txtValueDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYYSampai = txtValueDateTo.Text.Substring(6, 4);
            string MMSampai = txtValueDateTo.Text.Substring(3, 2);
            string DDSampai = txtValueDateTo.Text.Substring(0, 2);
            string dateALLSampai = DDSampai + '-' + MMSampai + '-' + YYYYSampai;
            hdnTanggalSampai.Value = dateALLSampai;
            hdnTanggalSampaiString.Value = txtValueDateTo.Text;
        }

        protected void cbpEmployeeRestLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}