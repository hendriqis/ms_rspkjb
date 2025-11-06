using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePatientBlackListStatus : BasePageTrx
    {
        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";
        public override string OnGetMenuCode()
        {
            string departmentID = Page.Request.QueryString["id"];

            if (departmentID == LABORATORY || departmentID == IMAGING)
                departmentID = Constant.Facility.DIAGNOSTIC;
            switch (departmentID)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.CHANGE_PATIENT_BLACK_LIST_STATUS;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_BLACK_LIST_STATUS;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.CHANGE_PATIENT_BLACK_LIST_STATUS;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.CHANGE_PATIENT_BLACK_LIST_STATUS;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.CHANGE_PATIENT_BLACK_LIST_STATUS;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.CHANGE_PATIENT_BLACK_LIST_STATUS;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.CHANGE_PATIENT_BLACK_LIST_STATUS;
                    return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_BLACK_LIST_STATUS;
                default: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_BLACK_LIST_STATUS;
            }
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetErrorMsgSelectMedicalNoFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_MEDICAL_NO_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_BLACKLIST_REASON));
            Methods.SetComboBoxField<StandardCode>(cboGCBlacklistReason, lstSC, "StandardCodeName", "StandardCodeID");
            cboGCBlacklistReason.SelectedIndex = 0;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMRN, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTelephone, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMobilePhoneNo, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(chkIsBlackList, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPSE, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCBlacklistReason, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtOtherBlackListReason, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "changepatientblacklist")
            {
                try
                {
                    Patient entity = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));
                    entity.IsBlackList = chkIsBlackList.Checked;
                    entity.GCBlacklistReason = cboGCBlacklistReason.Value.ToString();
                    if (cboGCBlacklistReason.Value != Constant.Patient_BlackList_Reason.OTHER)
                    {
                        entity.OtherBlacklistReason = txtOtherBlackListReason.Text;
                    }
                        
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatient(entity);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }
    }
}