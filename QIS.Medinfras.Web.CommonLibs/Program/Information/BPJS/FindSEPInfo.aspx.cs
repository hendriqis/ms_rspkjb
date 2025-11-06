using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FindSEPInfo : BasePageTrx
    {
        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ER_CARI_DATA_SEP;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.OPEN_CLOSE_PATIENT_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.OPEN_CLOSE_PATIENT_REGISTRATION;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.CARI_DATA_SEP;
                    return Constant.MenuCode.MedicalDiagnostic.OPEN_CLOSE_PATIENT_REGISTRATION;
                default: return Constant.MenuCode.Outpatient.OPEN_CLOSE_PATIENT_REGISTRATION;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        private GetUserMenuAccess menu;
        protected override void InitializeDataControl()
        {
            hdnDepartmentID.Value = Page.Request.QueryString["id"];
            if (hdnDepartmentID.Value == LABORATORY || hdnDepartmentID.Value == IMAGING)
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
            //InitializeControlProperties();
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            return true;
        }
    }
}