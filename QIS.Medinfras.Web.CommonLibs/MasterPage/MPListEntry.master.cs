using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.MasterPage
{
    public partial class MPListEntry : BaseMP
    {
        private BasePageListEntry _basePageList;
        private BasePageListEntry BasePageList
        {
            get
            {
                if (_basePageList == null)
                    _basePageList = (BasePageListEntry)Page;
                return _basePageList;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool IsAllowAdd, IsAllowEdit, IsAllowDelete, IsAllowPrint;
                IsAllowAdd = IsAllowEdit = IsAllowDelete = IsAllowPrint = true;
                menuCode = BasePageList.OnGetMenuCode();
                BasePageList.SetCRUDMode(ref IsAllowAdd, ref IsAllowEdit, ref IsAllowDelete);

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                string CRUDMode = menu.CRUDMode;

                if (!IsAllowAdd) CRUDMode = CRUDMode.Replace("C", "");
                if (!IsAllowEdit) CRUDMode = CRUDMode.Replace("U", "");
                if (!IsAllowDelete) CRUDMode = CRUDMode.Replace("D", "");
                if (!IsAllowPrint) CRUDMode = CRUDMode.Replace("P", "");

                if (!CRUDMode.Contains("C"))
                    pnlQuickEntry.Visible = false;

                foreach (Control c in ulMPListToolbar.Controls)
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

        protected void cbpMPListProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string retval = "";
            string[] param = e.Parameter.Split('|');

            if (param[0] == "delete")
                BasePageList.OnBtnDeleteClick(ref result);
            else if (param[0] == "save")
            {
                bool IsAdd = (hdnIsAdd.Value == "1");
                BasePageList.OnBtnSaveClick(ref result, IsAdd, ref retval);
            }
            else if (param[0] == "savequickentry")
                BasePageList.OnQuickEntryClick(ref result, param[1], ref retval);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        protected string menuCode = "";
        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            string custom = BasePageList.OnGetCustomMenuCaption();
            if (custom == "")
                return menu.MenuCaption;
            return custom;
        }
        protected String GetBreadcrumbs()
        {
            List<GetUserMenuAccess> lstMenu = ((MPMain)Master).ListMenu;
            StringBuilder result = new StringBuilder();
            List<GetUserMenuAccess> imagesHierarchy = new List<GetUserMenuAccess>();

            GetUserMenuAccess currMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
            while (currMenu != null)
            {
                imagesHierarchy.Insert(0, currMenu);
                currMenu = lstMenu.FirstOrDefault(p => p.MenuID == currMenu.ParentID);
            }

            string breadcrumb = string.Join(" > ", imagesHierarchy.Select(i => i.MenuCaption));
            string custom = BasePageList.OnGetCustomBreadcrumbs(breadcrumb);
            if (custom == "")
                return breadcrumb;
            return custom;
        }
    }
}