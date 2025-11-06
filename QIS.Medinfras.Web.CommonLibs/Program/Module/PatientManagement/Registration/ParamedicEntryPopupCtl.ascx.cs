using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ParamedicEntryPopupCtl : BaseViewPopupCtl
    {
        protected string filterExpressionOtherMedicalDiagnosticCtlPopup = "";

        public override void InitializeDataControl(string param)
        {
            string[] paramSplit = param.Split('|');
            txtRegistrationDateCtlPopup.Text = paramSplit[0];
            txtRegistrationHourCtlPopup.Text = paramSplit[1];
            hdnDeptIDCtlPopup.Value = paramSplit[3];
            hdnDiagnosticTypeCtlPopup.Value = paramSplit[4];
            hdnHealthcareServiceUnitIDCtlPopup.Value = paramSplit[5];
            filterExpressionOtherMedicalDiagnosticCtlPopup = paramSplit[6];

            if (paramSplit[7] == "IMAGING")
            {
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitIDCtlPopup.Value = hsu.HealthcareServiceUnitID.ToString();
            }
            else if (paramSplit[7] == Constant.Facility.MEDICAL_CHECKUP)
            {
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                hdnHealthcareServiceUnitIDCtlPopup.Value = hsu.HealthcareServiceUnitID.ToString();
            }
        }

        protected string GetServiceUnitLabel()
        {
            switch (hdnDeptIDCtlPopup.Value)
            {
                case Constant.Facility.INPATIENT: return GetLabel("Ruang Perawatan");
                case Constant.Facility.DIAGNOSTIC: return GetLabel("Penunjang Medis");
                case Constant.Facility.PHARMACY: return GetLabel("Farmasi");
                default: return GetLabel("Klinik");
            }
        }

        protected string GetServiceUnitUserParameter()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDeptIDCtlPopup.Value);
        }

        protected string GetDayNumber()
        {
            DateTime selectedDate = Helper.GetDatePickerValue(txtRegistrationDateCtlPopup.Text);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            return daynumber.ToString();
        }
    }
}