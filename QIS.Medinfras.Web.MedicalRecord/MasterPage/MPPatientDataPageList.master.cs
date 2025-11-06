using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.MedicalRecord.MasterPage
{
    public partial class MPPatientDataPageList : BaseMP
    {
        protected string IntellisenseHints = "";
        private BasePagePatientPageList _basePageList;
        private BasePagePatientPageList BasePageList
        {
            get
            {
                if (_basePageList == null)
                    _basePageList = (BasePagePatientPageList)Page;
                return _basePageList;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string menuCode = BasePageList.OnGetMenuCode();
                string CRUDMode = ((MPPatientDataPage)Master).ListMenu.FirstOrDefault(p => p.MenuCode == menuCode).CRUDMode;
                //if (!CRUDMode.Contains("C"))
                //    btnMPListAdd.Style.Add("display", "none");
                //if (!CRUDMode.Contains("U"))
                //    btnMPListEdit.Style.Add("display", "none");
                //if (!CRUDMode.Contains("D"))
                //    btnMPListDelete.Style.Add("display", "none");

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

                hdnIsEntryUsePopup.Value = BasePageList.IsEntryUsePopup() ? "1" : "0";
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
            string url = "";
            int popupWidth = 0;
            int popupHeight = 0;
            string popupHeaderText = "";
            string queryString = "";
            bool isEntryUsePopup = (hdnIsEntryUsePopup.Value == "1");
            string[] param = e.Parameter.Split('|');

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (param[0] == "add")
                BasePageList.OnBtnAddClick(ref result, ref url, ref queryString, ref popupWidth, ref popupHeight, ref popupHeaderText, isEntryUsePopup);
            else if (param[0] == "edit")
                BasePageList.OnBtnEditClick(ref result, ref url, ref queryString, ref popupWidth, ref popupHeight, ref popupHeaderText, isEntryUsePopup);
            else if (param[0] == "delete")
                BasePageList.OnBtnDeleteClick(ref result);
            else if (param[0] == "customclick")
            {
                BasePageList.OnBtnCustomClick(ref result, param[1]);
                panel.JSProperties["cpType"] = param[1];
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpURL"] = url;
            panel.JSProperties["cpSize"] = string.Format("{0}|{1}", popupWidth, popupHeight);
            panel.JSProperties["cpHeaderText"] = popupHeaderText;
            panel.JSProperties["cpQueryString"] = queryString;
        }
    }
}