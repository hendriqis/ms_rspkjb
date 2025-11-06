using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPPatientPageBillSummary : BaseMP
    {
        private BasePageContent _basePageList;
        private BasePageContent BasePageList
        {
            get
            {
                if (_basePageList == null)
                    _basePageList = (BasePageContent)Page;
                return _basePageList;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                ((MPBasePatientPage)Master).urlReferrer = ResolveUrl("~/Program/PatientList/VisitList.aspx?id=bs");
                //urlReferrer = Session["_UrlReferrerPatientPage"].ToString();
            }
        }

        protected void rptMenuHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                //HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                //IEnumerable<GetUserMenuAccess> mn = lstMenu.Where(p => p.ParentID == obj.MenuID && p.MenuCode == menuCode);
                //if (mn.Count() > 0)
                //{
                //    liCaption.Attributes.Add("class", "selected");
                //}
                //List<GetUserMenuAccess> lstMn = lstMenu.Where(p => p.ParentID == obj.MenuID).OrderBy(p => p.MenuIndex).ToList();
                //if (lstMn.Count > 0)
                //    liCaption.Attributes.Add("url", lstMn[0].MenuUrl);
                //else
                //    liCaption.Visible = false;

            }
        }
    }
}