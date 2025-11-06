using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InstructionQuickPicks1Ctl : BasePagePatientPageEntryCtl
    {
        private List<vInstruction> ListAllInstruction = null;
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ListAllInstruction = BusinessLayer.GetvInstructionList("IsDeleted = 0 ORDER BY GCInstructionGroup");
            rptInstruction.DataSource = (from p in ListAllInstruction
                                         select p).GroupBy(p => p.GCInstructionGroup).Select(p => p.First()).ToList();
            rptInstruction.DataBind();
        }

        protected void rptInstruction_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vInstruction obj = (vInstruction)e.Item.DataItem;

                Repeater rptInstructionDt = (Repeater)e.Item.FindControl("rptInstructionDt");
                rptInstructionDt.DataSource = GetInstructionDt(obj.GCInstructionGroup);
                rptInstructionDt.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (rptInstruction.Items.Count < 1)
                {
                    HtmlGenericControl divRptEmpty = (HtmlGenericControl)e.Item.FindControl("divRptEmpty");
                    divRptEmpty.Style["display"] = "block";
                }
            }
        }

        protected object GetInstructionDt(String GCInstructionGroup)
        {
            return ListAllInstruction.Where(p => p.GCInstructionGroup == GCInstructionGroup).ToList();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                string selectedItem = string.Empty;
                foreach (RepeaterItem itemHd in rptInstruction.Items)
                {
                    Repeater rptInstructionDt = (Repeater)itemHd.FindControl("rptInstructionDt");
                    foreach (RepeaterItem item in rptInstructionDt.Items)
                    {
                        CheckBox chkInstruction = (CheckBox)item.FindControl("chkInstruction");
                        if (chkInstruction.Checked)
                        {
                            HtmlInputHidden hdnGCInstructionGroup = (HtmlInputHidden)item.FindControl("hdnGCInstructionGroup");
                            HtmlGenericControl spnDescription = (HtmlGenericControl)item.FindControl("spnDescription");

                            if (selectedItem == string.Empty)
                            {
                                selectedItem = spnDescription.InnerHtml;
                            }
                            else
                            {
                                selectedItem = string.Format("{0}|{1}", selectedItem, spnDescription.InnerHtml);
                            }
                        }
                    }
                }
                retval = selectedItem;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                retval = string.Empty;
            }
            finally
            {
            }
            return result;
        }
    }
}