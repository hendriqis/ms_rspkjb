using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Inpatient.Program;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class BedReservationInfoDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private RoomInformation DetailPage
        {
            get { return (RoomInformation)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnBedID.Value = param;

            //txtTempatTidur.Text = BusinessLayer.GetvBedInformationBookingList(string.Format("BedID = {0}", hdnBedID.Value)).FirstOrDefault().BedCode;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //string filterExpression = string.Format("BedID = {0} AND GCReservationStatus = 'X263^001'", hdnBedID.Value);
            //if (isCountPageCount)
            //{ 
            //    int rowCount = BusinessLayer.GetvBedInformationOccupiedDtRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            //}

            //List<vBedInformationBooking> lstEntity = BusinessLayer.GetvBedInformationBookingList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "QueueNo ASC");
            grdPopupView.DataSource = null;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}