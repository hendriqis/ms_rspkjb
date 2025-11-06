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
    public partial class RevenueSharingItemEditEntry : BasePageList
    {
        protected string OnGetItemFilterExpression()
        {
            return string.Format("GCItemType NOT IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.LOGISTIC);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_EDIT_ENTRY;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtDateFrom.Text = DateTime.Now.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstCusttype = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PARAMEDIC_ROLE));
            Methods.SetComboBoxField<StandardCode>(cboParamedicRole, lstCusttype, "StandardCodeName", "StandardCodeID");
            cboParamedicRole.SelectedIndex = 0;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            lstClassCare.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
            Methods.SetComboBoxField<ClassCare>(cboClass, lstClassCare, "ClassName", "ClassID");
            cboClass.SelectedIndex = 0;

            Helper.SetControlEntrySetting(txtDateFrom, new ControlEntrySetting(true, true, true), "mpList");
            Helper.SetControlEntrySetting(txtDateTo, new ControlEntrySetting(true, true, true), "mpList");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpList");
            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpList");
            Helper.SetControlEntrySetting(cboParamedicRole, new ControlEntrySetting(true, true, true), "mpList");
            Helper.SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, true), "mpList");
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            try
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                int itemID = Convert.ToInt32(hdnItemID.Value);
                int classID = Convert.ToInt32(cboClass.Value);
                DateTime startDate = Helper.GetDatePickerValue(txtDateFrom);
                DateTime endDate = Helper.GetDatePickerValue(txtDateTo);
                string gcParamedicRole = cboParamedicRole.Value.ToString();
                int revenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
                bool result = BusinessLayer.ProcessRevenueSharingEditItem(paramedicID, itemID, classID, startDate, endDate, gcParamedicRole, revenueSharingID);
                return result;
            }
            catch(Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}