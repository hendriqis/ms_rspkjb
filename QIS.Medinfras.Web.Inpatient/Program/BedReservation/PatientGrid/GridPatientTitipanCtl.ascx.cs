using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientTitipanCtl : System.Web.UI.UserControl
    {
        protected int pageCountTitipan = 1;
        protected int currPageTitipan = 1;
        public void InitializeControl()
        {
            BindGridView(currPageTitipan, true, ref pageCountTitipan);
        }

        protected void cbpViewTitipan_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePagePatientOrder)Page).LoadAllWords();
            int pageCountTitipan = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCountTitipan);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCountTitipan);
                    result = "refresh|" + pageCountTitipan;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePagePatientOrder)Page).GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit7RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vConsultVisit7> lstEntity = BusinessLayer.GetvConsultVisit7List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex);
            lvwViewTitipan.DataSource = lstEntity;
            lvwViewTitipan.DataBind();
        }

        protected void lvwViewTitipan_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //vTestOrderHdVisit entity = (vTestOrderHdVisit)e.Item.DataItem;
                //HtmlGenericControl spnProcessed = e.Item.FindControl("spnProcessed") as HtmlGenericControl;
                //if(entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                //    spnProcessed.Style.Add("display", "none");
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePagePatientOrder)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDtTitipan_Click(object sender, EventArgs e)
        {
            String ReservationID = hdnReservationID.Value;
            String RegistratioID = hdnRegistrationID.Value;
            if (ReservationID != "0")
            {
                vBedReservation oReservation = BusinessLayer.GetvBedReservationList(string.Format("ReservationID = {0}", ReservationID))[0];
                if (oReservation.GCReservationStatus != Constant.Bed_Reservation_Status.CANCELLED)
                {
                    ((BasePagePatientOrder)Page).OnGrdRowClickTestOrder(ReservationID, RegistratioID);
                }
            }
            else
            {
                String ReservationIDCancel = Convert.ToString(0);
                ((BasePagePatientOrder)Page).OnGrdRowClickTestOrder(ReservationIDCancel, RegistratioID);
            }
        }
    }
}