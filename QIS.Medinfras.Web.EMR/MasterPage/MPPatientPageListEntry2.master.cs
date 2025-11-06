using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.MasterPage
{
    public partial class MPPatientPageListEntry2 : BaseMP
    {
        private BasePagePatientPageListEntry _basePageList;
        private BasePagePatientPageListEntry BasePageList
        {
            get
            {
                if (_basePageList == null)
                    _basePageList = (BasePagePatientPageListEntry)Page;
                return _basePageList;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string menuCode = BasePageList.OnGetMenuCode();
                string CRUDMode = ((MPPatientPage)Master).ListMenu.FirstOrDefault(p => p.MenuCode == menuCode).CRUDMode;

                bool IsAllowAdd, IsAllowEdit, IsAllowDelete;
                IsAllowAdd = IsAllowEdit = IsAllowDelete = true;
                _basePageList.SetCRUDMode(ref IsAllowAdd, ref IsAllowEdit, ref IsAllowDelete);
                if (!IsAllowAdd) CRUDMode = CRUDMode.Replace("C", "");
                if (!IsAllowEdit) CRUDMode = CRUDMode.Replace("U", "");
                if (!IsAllowDelete) CRUDMode = CRUDMode.Replace("D", "");

                if (!CRUDMode.Contains("C"))
                {
                    btnMPListAdd.Style.Add("display", "none");
                    pnlQuickEntry.Visible = false;
                }
                if (!CRUDMode.Contains("U"))
                    btnMPListEdit.Style.Add("display", "none");
                if (!CRUDMode.Contains("D"))
                    btnMPListDelete.Style.Add("display", "none");
               
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
    }
}