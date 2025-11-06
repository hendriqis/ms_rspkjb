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
    public partial class EpisodeSummaryVitalSignIndicatorCtl : BaseViewPopupCtl
    {
        private List<vVitalSignDt> ListvVitalSignDt = null;
        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            ListvVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterExpression);

            rptVitalSign.DataSource = BusinessLayer.GetvVitalSignHdList(filterExpression);
            rptVitalSign.DataBind();
        }

        protected void rptVitalSign_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Item.DataItem;

                Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignData(obj.ID);
                rptVitalSignDt.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (rptVitalSign.Items.Count < 1)
                {
                    HtmlGenericControl divRptEmpty = (HtmlGenericControl)e.Item.FindControl("divRptEmpty");
                    divRptEmpty.Style["display"] = "block";
                }
            }        
        }
        protected void rptVitalSignDt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vVitalSignDt obj = (vVitalSignDt)e.Item.DataItem;
                if (obj.VitalSignValue == "")
                {
                    HtmlTableRow trVitalSignDt = (HtmlTableRow)e.Item.FindControl("trVitalSignDt");
                    trVitalSignDt.Style.Add("display", "none");
                }
                else
                {
                    HtmlTableCell td = null;
                    if (obj.GCValueType == Constant.ControlType.TEXT_BOX)
                        td = (HtmlTableCell)e.Item.FindControl("tdTxt");
                    else if (obj.GCValueType == Constant.ControlType.COMBO_BOX)
                        td = (HtmlTableCell)e.Item.FindControl("tdCbo");

                    td.Style.Remove("display");
                }
            }
        }

        protected object GetVitalSignData(Int32 ID)
        {
            return ListvVitalSignDt.Where(p => p.ID == ID).ToList();
        }
    }
}