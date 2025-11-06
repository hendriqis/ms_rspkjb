using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class RoomList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.ROOM;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;
            if (Request.Form["healthcareID"] != null)
                cboHealthcare.Value = Request.Form["healthcareID"].ToString();

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetRoomRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Room Code", "Room Name" };
            fieldListValue = new string[] { "RoomCode", "RoomName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("HealthcareID = '{0}' AND IsDeleted = 0", cboHealthcare.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetRoomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<Room> lstEntity = BusinessLayer.GetRoomList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RoomCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl(string.Format("~/Program/Master/Room/RoomEntry.aspx?id=add|{0}", cboHealthcare.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/Room/RoomEntry.aspx?id=edit|{0}", hdnID.Value));
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                Room oRoom = BusinessLayer.GetRoom(Convert.ToInt16(hdnID.Value));
                if (oRoom != null)
                {
                    oRoom.IsDeleted = true;
                    oRoom.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateRoom(oRoom);

                    return true;
                }
                else
                {
                    errMessage = "No record found.";
                    return false;
                }
            }
            return true;
        }
    }
}