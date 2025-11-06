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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class LocationList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.LOCATION;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
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
                int row = BusinessLayer.GetvLocationRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Location Name", "Location Code", "Location Short Name" };
            fieldListValue = new string[] { "LocationName", "LocationCode", "ShortName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("HealthcareID = '{0}' AND IsDeleted = 0 AND IsHeader = 1", cboHealthcare.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvLocationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vLocation> lstEntity = BusinessLayer.GetvLocationList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LocationCode");
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
            url = ResolveUrl(string.Format("~/Program/Master/Location/LocationEntry.aspx?id=add|{0}", cboHealthcare.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                string filterLocation = string.Format("LocationID = {0} AND LocationID NOT IN (SELECT ISNULL(a.LocationID,0) FROM vLocationFromIM0008 a)", hdnID.Value);
                List<Location> locationCheck = BusinessLayer.GetLocationList(filterLocation);
                if (locationCheck.Count() > 0)
                {
                    url = ResolveUrl(string.Format("~/Program/Master/Location/LocationEntry.aspx?id=edit|{0}", hdnID.Value));
                    return true;
                }
                else
                {
                    errMessage = "Lokasi ini tidak dapat diubah karena termasuk dalam lokasi transit (IM0008)";
                    return false;
                }
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                string filterLocation = string.Format("LocationID = {0} AND LocationID NOT IN (SELECT ISNULL(a.LocationID,0) FROM vLocationFromIM0008 a)", hdnID.Value);
                List<Location> locationCheck = BusinessLayer.GetLocationList(filterLocation);
                if (locationCheck.Count() > 0)
                {
                    bool isAllowDelete = true;
                    List<ItemBalance> lstItemBalance = BusinessLayer.GetItemBalanceList(string.Format("LocationID = {0} AND IsDeleted = 0", hdnID.Value.Trim()));
                    if (lstItemBalance.Count > 0)
                    {
                        if (lstItemBalance.Sum(a => a.QuantityBEGIN) != 0)
                        {
                            isAllowDelete = false;
                        }
                        else if (lstItemBalance.Sum(a => a.QuantityEND) != 0)
                        {
                            isAllowDelete = false;
                        }
                    }

                    if (isAllowDelete)
                    {
                        Location entity = BusinessLayer.GetLocation(Convert.ToInt32(hdnID.Value));
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateLocation(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Item tidak bisa dihapus karena memiliki nilai balance.";
                        return false;
                    }
                }
                else
                {
                    errMessage = "Lokasi ini tidak dapat diubah karena termasuk dalam lokasi transit (IM0008)";
                    return false;
                }
            }
            return false;
        }
    }
}