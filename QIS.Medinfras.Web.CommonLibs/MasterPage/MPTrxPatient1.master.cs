using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.MasterPage
{
    public partial class MPTrxPatient1 : BaseMP
    {
        #region Top Charm Bar Menu
        public class TopCharmBarMenu
        {
            public string Title { get; set; }
            public string Postbackurl { get; set; }
            public string Normalsrc { get; set; }
            public string Hoversrc { get; set; }
            public string Selectedsrc { get; set; }
            public string Pressedsrc { get; set; }
            public string Disabledsrc { get; set; }
        }
        #endregion

        private BasePageTrx _basePageEntry;
        private BasePageTrx BasePageEntry
        {
            get
            {
                if (_basePageEntry == null)
                    _basePageEntry = (BasePageTrx)Page;
                return _basePageEntry;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool isAdd = !BasePageEntry.IsLoadFirstRecord;
                hdnIsAdd.Value = isAdd ? "1" : "0";
                int rowCount = BasePageEntry.OnGetRowCount();
                hdnRowCount.Value = rowCount.ToString();

                bool IsAllowAdd, IsAllowSave, IsAllowVoid, IsAllowNextPrev, IsAllowEdit;
                IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = IsAllowEdit = true;
                BasePageEntry.SetToolbarVisibility(ref IsAllowAdd, ref IsAllowSave, ref IsAllowVoid, ref IsAllowNextPrev);
                if (!IsAllowAdd)
                    btnMPEntryNew.Style.Add("display", "none");
                if (!IsAllowSave)
                    btnMPEntrySave.Style.Add("display", "none");
                if (!IsAllowVoid)
                    btnMPEntryVoid.Style.Add("display", "none");
                if (!IsAllowNextPrev)
                {
                    btnMPEntryNext.Style.Add("display", "none");
                    btnMPEntryPrev.Style.Add("display", "none");
                }
                if (BasePageEntry.IsRefreshControlAfterSaveAddRecord())
                    hdnIsRefreshControlAfterSaveAddRecord.Value = "1";
                else
                    hdnIsRefreshControlAfterSaveAddRecord.Value = "0";

                String menuCode = BasePageEntry.OnGetMenuCode();
                GetUserMenuAccess menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                string CRUDMode = menu.CRUDMode;

                if (!IsAllowAdd) CRUDMode = CRUDMode.Replace("C", "");
                if (!IsAllowEdit) CRUDMode = CRUDMode.Replace("U", "");
                if (!IsAllowVoid) CRUDMode = CRUDMode.Replace("V", "");
                //if (!IsAllowPrint) CRUDMode = CRUDMode.Replace("P", "");
                hdnIsAllowEdit.Value = CRUDMode.Contains("U") ? "1" : "0";
                hdnIsAllowNextPrev.Value = IsAllowNextPrev ? "1" : "0";
                hdnIsAllowVoid.Value = CRUDMode.Contains("V") ? "1" : "0";

                foreach (Control c in ulMPTrxToolbar.Controls)
                {
                    if (c is HtmlControl && ((HtmlControl)c).TagName.ToLower() == "li")
                    {
                        HtmlGenericControl li = c as HtmlGenericControl;
                        SetToolbarButtonVisibility(li, CRUDMode);
                    }
                    else if (c is ContentPlaceHolder)
                    {
                        foreach (Control c2 in c.Controls)
                        {
                            if (c2 is HtmlControl && ((HtmlControl)c2).TagName.ToLower() == "li")
                            {
                                HtmlGenericControl li = c2 as HtmlGenericControl;
                                SetToolbarButtonVisibility(li, CRUDMode);
                            }
                        }
                    }
                }

                if (rowCount < 1)
                {
                    btnMPEntryNext.Style.Add("display", "none");
                    btnMPEntryPrev.Style.Add("display", "none");
                }
                if (isAdd)
                    btnMPEntryVoid.Style.Add("display", "none");
                else
                {
                    if (!CRUDMode.Contains("U"))
                        btnMPEntrySave.Style.Add("display", "none");
                    if (BasePageEntry.isShowWatermark)
                        hdnWatermark.Value = "1|" + BasePageEntry.watermarkText;
                    else
                        hdnWatermark.Value = "0";
                    hdnPageIndex.Value = "0";
                }


                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                string topBarMenuCode = string.Empty;

                switch (ModuleID)
                {
                    case Constant.Module.EMERGENCY:
                        topBarMenuCode = Constant.MenuCode.EmergencyCare.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.EMR:
                        topBarMenuCode = Constant.MenuCode.EMR.INFORMATION;
                        break;
                    case Constant.Module.FINANCE:
                        topBarMenuCode = Constant.MenuCode.Finance.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.IMAGING:
                        topBarMenuCode = Constant.MenuCode.Imaging.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.INPATIENT:
                        topBarMenuCode = Constant.MenuCode.Inpatient.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.LABORATORY:
                        topBarMenuCode = Constant.MenuCode.Laboratory.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.MEDICAL_CHECKUP:
                        topBarMenuCode = Constant.MenuCode.MedicalCheckup.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        topBarMenuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.MEDICAL_RECORD:
                        topBarMenuCode = Constant.MenuCode.MedicalRecord.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.NUTRITION:
                        topBarMenuCode = Constant.MenuCode.Nutrition.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.OUTPATIENT:
                        topBarMenuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.PHARMACY:
                        topBarMenuCode = Constant.MenuCode.Pharmacy.PATIENT_PAGE_CHARM_BAR;
                        break;
                    default:
                        break;
                }

                #region Top Charm Bar
                if (!string.IsNullOrEmpty(topBarMenuCode))
                {
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}' OR ParentCode = '{0}'", topBarMenuCode));
                    GetUserMenuAccess menuInformation = lstMenu.FirstOrDefault(p => p.MenuCode == topBarMenuCode);
                    if (menuInformation != null)
                    {
                        List<GetUserMenuAccess> lstUserMenuCharmBar = lstMenu.Where(p => p.ParentID == menuInformation.MenuID).OrderBy(p => p.MenuIndex).ToList();
                        List<TopCharmBarMenu> ListTopCharmBarMenu = new List<TopCharmBarMenu>();
                        foreach (GetUserMenuAccess oMenu in lstUserMenuCharmBar)
                        {
                            TopCharmBarMenu topCharmBar = new TopCharmBarMenu();
                            topCharmBar.Title = oMenu.MenuCaption;
                            topCharmBar.Postbackurl = oMenu.MenuUrl;
                            topCharmBar.Normalsrc = Page.ResolveUrl(oMenu.ImageUrl);
                            topCharmBar.Hoversrc = string.Format("{0}_hover.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            topCharmBar.Selectedsrc = string.Format("{0}_selected.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            topCharmBar.Pressedsrc = string.Format("{0}_pressed.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            topCharmBar.Disabledsrc = string.Format("{0}_disabled.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            ListTopCharmBarMenu.Add(topCharmBar);
                        }
                        rptTopCharmBarMenu.DataSource = ListTopCharmBarMenu;
                        rptTopCharmBarMenu.DataBind();
                    }
                }
                #endregion

            }
        }

        private void SetToolbarButtonVisibility(HtmlGenericControl li, string CRUDMode)
        {
            if (li.Attributes["CRUDMode"] != null)
            {
                string liCRUDMode = li.Attributes["CRUDMode"];
                if (!CRUDMode.Contains(liCRUDMode))
                    li.Style.Add("display", "none");
            }
        }

        protected void cbpMPEntryContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int rowCount = Convert.ToInt32(hdnRowCount.Value);
            bool isShowWatermark = false;
            string watermarkText = "";
            if (param[0] == "new")
            {
                BasePageEntry.AddRecord();
                pageIndex = -1;
            }
            else if (param[0] == "next")
                BasePageEntry.NextPageIndex(rowCount, ref pageIndex, ref isShowWatermark, ref watermarkText);
            else if (param[0] == "prev")
                BasePageEntry.PrevPageIndex(rowCount, ref pageIndex, ref isShowWatermark, ref watermarkText);
            else if (param[0] == "load")
                BasePageEntry.LoadPage(pageIndex, ref isShowWatermark, ref watermarkText);
            else if (param[0] == "refresh")
            {
                BasePageEntry.RefreshControl();
                pageIndex = -1;
            }
            else if (param[0] == "loadobject")
                BasePageEntry.LoadPage(param[1], ref pageIndex, ref isShowWatermark, ref watermarkText);

            string cpWatermark = isShowWatermark ? "1" : "0";
            if (isShowWatermark)
                cpWatermark = "1|" + watermarkText;
            else
                cpWatermark = "0";
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpParam"] = param[0];
            panel.JSProperties["cpPageIndex"] = pageIndex;
            panel.JSProperties["cpWatermark"] = cpWatermark;
        }

        protected void cbpMPEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string retval = "";
            string result = "";
            string[] param = e.Parameter.Split('|');
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (param[0] == "save")
            {
                bool isAdd = (param[1] == "1");
                BasePageEntry.OnBtnSaveClick(ref result, ref retval, isAdd);
            }
            else if (param[0] == "confirm")
            {
                bool isAdd = (param[1] == "1");
                BasePageEntry.OnBtnConfirmClick(ref result, ref retval, isAdd);
            }
            else if (param[0] == "void")
                BasePageEntry.OnBtnVoidClick(ref result);
            else if (param[0] == "customclick")
            {
                BasePageEntry.OnBtnCustomClick(ref result, param[1], ref retval);
                panel.JSProperties["cpType"] = param[1];
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }
    }
}