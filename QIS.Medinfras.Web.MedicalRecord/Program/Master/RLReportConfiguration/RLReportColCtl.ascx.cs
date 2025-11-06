using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RLReportColCtl : BaseEntryPopupCtl
    {

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            hdnReportID.Value = param;
            RLReport entity = BusinessLayer.GetRLReport(Convert.ToInt32(hdnReportID.Value));
            txtHeaderText.Text = string.Format("{0} - {1}", entity.RLReportCode, entity.RLReportName);

            BindGridView();
        }

        private class RLReportColItem
        {
            public int Index { get; set; }
            public string RLReportID { get; set; }
            public string Value { get; set; }
        }        

        private List<RLReportColItem> GetListRLReportColItem(RLReportColumn entity)
        {
            List<RLReportColItem> lst = new List<RLReportColItem>();
            for (int i = 1; i <= 20; i++)
            {
                RLReportColItem item = new RLReportColItem();
                item.Index = i;
                item.RLReportID = String.Format("Column{0} Title", i);
                string columnName = String.Format("Column{0}Title", i);
                item.Value = entity.GetType().GetProperty(columnName).GetValue(entity, null).ToString();
                lst.Add(item);
            }
            return lst;
        }                

        private void BindGridView()
        {            
            List<RLReportColItem> lst;
            RLReportColumn entity = BusinessLayer.GetRLReportColumn(Convert.ToInt32(hdnReportID.Value));
            if (entity == null)
            {
                entity = new RLReportColumn();
                lst = new List<RLReportColItem>();
                for (int i = 1; i < 20; i++)
                {
                    RLReportColItem item = new RLReportColItem();
                    item.Index = i;
                    item.RLReportID = String.Format("Column{0} Title", i);
                    item.Value = "";
                    lst.Add(item);
                }
            }
            else
            {
                lst = GetListRLReportColItem(entity);
            }
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                BindGridView();

                bool IsAdd = false;
                RLReportColumn entity = BusinessLayer.GetRLReportColumn(Convert.ToInt32(hdnReportID.Value));
                
                if (entity == null)
                {
                    IsAdd = true;
                    entity = new RLReportColumn();
                }
                int ctr = 1;
                foreach (GridViewRow row in grdView.Rows)
                {
                    TextBox txt = (TextBox)row.FindControl("txtValue");
                    String value = Request.Form[txt.UniqueID];

                    string columnName = String.Format("Column{0}Title", ctr);
                    entity.GetType().GetProperty(columnName).SetValue(entity,value, null);

                    ctr++;
                }
                if (IsAdd)
                    BusinessLayer.InsertRLReportColumn(entity);
                else
                    BusinessLayer.UpdateRLReportColumn(entity);

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}