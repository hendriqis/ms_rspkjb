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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EpisodeSummaryReviewOfSystemCtl : BaseViewPopupCtl
    {
        private List<vReviewOfSystemDt> ListAllROSystemDt = null;
        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            ListAllROSystemDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);

            rptReviewOfSystem.DataSource = BusinessLayer.GetvReviewOfSystemHdList(filterExpression);
            rptReviewOfSystem.DataBind();
        }

        protected void rptReviewOfSystem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Item.DataItem;

                Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptReviewOfSystemDt");
                rptVitalSignDt.DataSource = GetReviewOfStstemDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (rptReviewOfSystem.Items.Count < 1)
                {
                    HtmlGenericControl divRptEmpty = (HtmlGenericControl)e.Item.FindControl("divRptEmpty");
                    divRptEmpty.Style["display"] = "block";
                }
            }  
        }
        protected object GetReviewOfStstemDt(Int32 ID)
        {
            return ListAllROSystemDt.Where(p => p.ID == ID).ToList();
        }
    }
}