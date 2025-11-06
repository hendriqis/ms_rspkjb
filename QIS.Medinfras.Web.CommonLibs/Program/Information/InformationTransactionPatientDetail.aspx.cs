using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class InformationTransactionPatientDetail : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1";
        private string filterSer = "IsDeleted = 0";

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {   
            //if (Page.Request.QueryString["id"] == "MD")
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param[0] == "MD")
                return Constant.MenuCode.MedicalDiagnostic.INFORMATION_TRANSACTION_PATIENT_DETAIL_MD080231;
            else //if  (Page.Request.QueryString["id"] == "SA")
                return Constant.MenuCode.SystemSetup.INFORMATION_TRANSACTION_PATIENT_DETAIL;
            //else
            //    return Constant.MenuCode..INFORMATION_TRANSACTION_PATIENT_DETAIL_MD080231;

        }

        //public override string OnGetMenuCode()
        //{
        //    return Constant.MenuCode.SystemSetup.INFORMATION_TRANSACTION_PATIENT_DETAIL;
        //}

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnRequestID.Value = param[0];
                hdnVisitID.Value = param[1];

                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                        "HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

                string roleID = "";

                List<UserInRole> lst = BusinessLayer.GetUserInRoleList(string.Format(
                        "HealthcareID = '{0}' AND UserID = {1}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                foreach (UserInRole uir in lst)
                {
                    roleID += "," + uir.RoleID;
                }
                roleID = roleID.Substring(1, roleID.Length - 1);

                if (roleID != "")
                {
                    List<vServiceUnitUserRole> lstServiceUnit = BusinessLayer.GetvServiceUnitUserRoleList(string.Format("HealthcareID = '{0}' AND RoleID IN ({1})",
                            AppSession.UserLogin.HealthcareID, roleID));
                    foreach (vServiceUnitUserRole sur in lstServiceUnit)
                    {
                        deptID += "," + "'" + sur.DepartmentID + "'";
                        suID += "," + "'" + sur.ServiceUnitID + "'";
                    }

                    if (lstServiceUnit.Count > 0)
                    {
                        deptID = deptID.Substring(1, deptID.Length - 1);
                        filterDept += string.Format(" AND DepartmentID IN ({0})", deptID);

                        suID = suID.Substring(1, suID.Length - 1);
                        filterSer += string.Format(" AND ServiceUnitID IN ({0})", suID);
                    }

                    List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                    lstDepartment.Insert(0, new Department { DepartmentID = string.Format("0"), DepartmentName = string.Format(" - {0} - ", GetLabel("All")) });
                    Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                    hdnFilterServiceUnitID.Value = filterSer;
                }
                else
                {
                    List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                    lstDepartment.Insert(0, new Department { DepartmentID = "0", DepartmentName = string.Format(" - {0} - ", GetLabel("All")) });
                    Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentName");

                    hdnFilterServiceUnitID.Value = filterSer;
                }

                cboDepartment.SelectedIndex = 0;

                //BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            }

            filterExpression += string.Format(" AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);

            if (cboDepartment.Value != "" && cboDepartment.Value != null && cboDepartment.Value.ToString() != "0")
            {
                filterExpression += String.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
            }

            List<vPatientChargesHd3> lst = BusinessLayer.GetvPatientChargesHd3List(filterExpression);
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