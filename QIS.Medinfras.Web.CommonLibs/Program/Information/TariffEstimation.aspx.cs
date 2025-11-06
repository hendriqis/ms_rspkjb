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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TariffEstimation : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            if (ModuleID == Constant.Module.INPATIENT)
                return Constant.MenuCode.Inpatient.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.OUTPATIENT)
                return Constant.MenuCode.Outpatient.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.EMERGENCY)
                return Constant.MenuCode.EmergencyCare.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.PHARMACY)
                return Constant.MenuCode.Pharmacy.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.LABORATORY)
                return Constant.MenuCode.Laboratory.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.IMAGING)
                return Constant.MenuCode.Imaging.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.RADIOTHERAPHY)
                return Constant.MenuCode.Radiotheraphy.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.MEDICAL_DIAGNOSTIC)
                return Constant.MenuCode.MedicalDiagnostic.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.INVENTORY)
                return Constant.MenuCode.Inventory.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.EMR)
                return Constant.MenuCode.EMR.TARIFF_ESTIMATION;
            return Constant.MenuCode.Finance.TARIFF_ESTIMATION;
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                            AppSession.UserLogin.HealthcareID, //0
                            Constant.SettingParameter.EMERGENCY_CLASS, //1
                            Constant.SettingParameter.MEDICAL_CHECKUP_CLASS, //2
                            Constant.SettingParameter.OUTPATIENT_CLASS, //3
                            Constant.SettingParameter.PHARMACY_CLASS //4
                        ));

            hdnEmergencyClass.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.EMERGENCY_CLASS).FirstOrDefault().ParameterValue;
            hdnMedicalCheckupClass.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.MEDICAL_CHECKUP_CLASS).FirstOrDefault().ParameterValue;
            hdnOutpatientClass.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.OUTPATIENT_CLASS).FirstOrDefault().ParameterValue;
            hdnPharmacyClass.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.PHARMACY_CLASS).FirstOrDefault().ParameterValue;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
            hdnGCItemType.Value = cboItemType.Value.ToString();
            hdnDepartmentID.Value = cboDepartment.Value.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE, Constant.StandardCode.CUSTOMER_TYPE));
            Methods.SetComboBoxField(cboCustomerType, lstSC.Where(p => p.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboCustomerType.Value = Constant.CustomerType.PERSONAL;

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 AND IsActive=1"));
            Methods.SetComboBoxField(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
            cboDepartment.Value = Constant.Facility.OUTPATIENT;

            Methods.SetComboBoxField(cboItemType, lstSC.Where(p => p.ParentID == Constant.StandardCode.ITEM_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboItemType.SelectedIndex = 0;

            List<ClassCare> lstClass = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField(cboClass, lstClass, "ClassName", "ClassID");
            cboClass.Value = hdnOutpatientClass.Value;
            cboClass.ClientEnabled = false;
            txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ItemMaster entity = e.Row.DataItem as ItemMaster;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnItemGroupID.Value == "")
                filterExpression += string.Format("GCItemType = '{0}' AND (ItemName1 LIKE '%{1}%' OR ItemCode LIKE '%{1}%') AND IsDeleted = 0 AND GCItemStatus != '{2}'", cboItemType.Value, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);
            else
                filterExpression += string.Format("GCItemType = '{0}' AND (ItemName1 LIKE '%{1}%' OR ItemCode LIKE '%{1}%') AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0 AND GCItemStatus != '{3}'", cboItemType.Value, hdnFilterItem.Value, hdnItemGroupID.Value, Constant.ItemStatus.IN_ACTIVE);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterBP = string.Format("GCCustomerType = '{0}' AND IsDeleted = 0", cboCustomerType.Value.ToString());
            string bpList = "";
            List<vCustomer> lstBP = BusinessLayer.GetvCustomerList(filterBP);
            foreach (vCustomer bp in lstBP)
            {
                bpList += bp.BusinessPartnerID + ",";
            }
            if (bpList != "")
            {
                hdnBusinessPartnerID.Value = bpList.Substring(0, bpList.Length - 1);
            }

            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetItemMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<ItemMaster> lstEntity = BusinessLayer.GetItemMasterList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}