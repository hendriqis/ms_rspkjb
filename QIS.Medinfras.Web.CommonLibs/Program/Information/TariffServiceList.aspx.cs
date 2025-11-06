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
using System.Text;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TariffServiceList : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "SERVICE": return Constant.MenuCode.Finance.TARIFF_SERVICE;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.TARIFF_SERVICE_INFORMATION;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.TARIFF_SERVICE_INFORMATION;
                    return Constant.MenuCode.MedicalDiagnostic.TARIFF_SERVICE_INFORMATION;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.TARIFF_SERVICE_INFORMATION;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.TARIFF_SERVICE_INFORMATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.TARIFF_SERVICE_INFORMATION;
                default:
                    string moduleName = Helper.GetModuleName();
                    string ModuleID = Helper.GetModuleID(moduleName);
                    if (ModuleID == Constant.Module.INPATIENT)
                        return Constant.MenuCode.Inpatient.DRUGS_LOGISTICS_PRICE;
                    else if (ModuleID == Constant.Module.OUTPATIENT)
                        return Constant.MenuCode.Outpatient.DRUGS_LOGISTICS_PRICE;
                    else if (ModuleID == Constant.Module.EMERGENCY)
                        return Constant.MenuCode.EmergencyCare.DRUGS_LOGISTICS_PRICE;
                    else if (ModuleID == Constant.Module.PHARMACY)
                        return Constant.MenuCode.Pharmacy.DRUGS_LOGISTICS_PRICE;
                    else if (ModuleID == Constant.Module.LABORATORY)
                        return Constant.MenuCode.Laboratory.DRUGS_LOGISTICS_PRICE;
                    else if (ModuleID == Constant.Module.IMAGING)
                        return Constant.MenuCode.Imaging.DRUGS_LOGISTICS_PRICE;
                    else if (ModuleID == Constant.Module.MEDICAL_DIAGNOSTIC)
                        return Constant.MenuCode.MedicalDiagnostic.DRUGS_LOGISTICS_PRICE;
                    return Constant.MenuCode.Finance.DRUGS_LOGISTICS_PRICE;
            }
        }

        protected int PageCount = 1;
        protected int ClassCount = 0;
        private List<ClassCare> ListClassCare = null;
        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected override void OnLoad(EventArgs e)
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            StringBuilder sbListClassID = new StringBuilder();
            StringBuilder sbListClassName = new StringBuilder();
            
            foreach (ClassCare classCare in lstClassCare)
            {
                if (sbListClassID.ToString() != "")
                {
                    sbListClassID.Append("|");
                    sbListClassName.Append("|");
                }
                sbListClassID.Append(classCare.ClassID);
                sbListClassName.Append(classCare.ClassName);
            }

            hdnListClassID.Value = sbListClassID.ToString();
            hdnListClassName.Value = sbListClassName.ToString();

            BindingRptClass();

            base.OnLoad(e);
        }

        private void BindingRptClass()
        {
            string[] lstClassID = hdnListClassID.Value.Split('|');
            string[] lstClassName = hdnListClassName.Value.Split('|');
            ListClassCare = new List<ClassCare>();
            for (int i = 0; i < lstClassID.Length; ++i)
            {
                ClassCare entity = new ClassCare();
                entity.ClassID = Convert.ToInt32(lstClassID[i]);
                entity.ClassName = lstClassName[i];
                ListClassCare.Add(entity);
            }

            ClassCount = ListClassCare.Count;

            rptTariffClassHeader.DataSource = ListClassCare;
            rptTariffClassHeader.DataBind();
        }

        List<SettingParameter> lstSettingParameter = null;
        protected override void InitializeDataControl()
        {
            lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
            Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT));
            
            menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'",Constant.StandardCode.TARIFF_SCHEME));
            Methods.SetComboBoxField<StandardCode>(cboTariffScheme, lstSC, "StandardCodeName", "StandardCodeID");
            cboTariffScheme.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        protected string GetTariffComponent1Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
        }

        protected string GetTariffComponent2Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
        }

        protected string GetTariffComponent3Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;
        }

        protected bool IsItemProduct = false;
        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = "";
            string id = Page.Request.QueryString["id"];

            if (id == "SERVICE")
                filterExpression = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}')", Constant.ItemGroupMaster.SERVICE,
                    Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC);
            else if (id == Constant.Facility.INPATIENT || id == Constant.Facility.OUTPATIENT || id == Constant.Facility.EMERGENCY)
                filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.SERVICE);
            else if (id == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.LABORATORY);
                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.RADIOLOGY);
                else
                    filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.DIAGNOSTIC);
            }
            else
            {
                IsItemProduct = true;
                filterExpression = string.Format("GCItemType IN ('{0}','{1}','{2}')", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = OnGetItemGroupFilterExpression();

            if (hdnItemGroupID.Value != "0" && hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value);
            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            filterExpression += string.Format(" AND GCTariffScheme = '{0}'", cboTariffScheme.Value.ToString());
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemTariffCustomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            List<vItemTariffCustom> lstEntity = BusinessLayer.GetvItemTariffCustomList(filterExpression, 10, pageIndex, "ItemName1");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                    vItemTariffCustom obj = (vItemTariffCustom)e.Item.DataItem;
                    Repeater rptTarifClass = (Repeater)e.Item.FindControl("rptTariffClass");
                    //List<decimal> lstTariff = obj.Tariff.Split('|').ToList().Select(p => Convert.ToDecimal(p.Split(';')[0])).ToList();
                    List<string> lstTariff = obj.Tariff.Split('|').ToList();
                    List<int> lstClassID = obj.ClassID.Split('|').ToList().Select(int.Parse).ToList();
                    List<ItemTariff> lstOrderedTariff = new List<ItemTariff>();
                    for (int i = 0; i < ListClassCare.Count; ++i)
                    {
                        int idx = lstClassID.IndexOf(ListClassCare[i].ClassID);
                        if (idx > -1)
                        {
                            string[] temp = lstTariff[idx].Split(';');
                            ItemTariff entity = new ItemTariff { TariffComp1 = Convert.ToDecimal(temp[0]), TariffComp2 = Convert.ToDecimal(temp[1]), TariffComp3 = Convert.ToDecimal(temp[2]) };
                            entity.Tariff = entity.TariffComp1 + entity.TariffComp2 + entity.TariffComp3;
                            lstOrderedTariff.Add(entity);
                        }
                        else
                            lstOrderedTariff.Add(new ItemTariff());
                    }

                    rptTarifClass.DataSource = lstOrderedTariff;
                    rptTarifClass.DataBind();
            }
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
    }
}