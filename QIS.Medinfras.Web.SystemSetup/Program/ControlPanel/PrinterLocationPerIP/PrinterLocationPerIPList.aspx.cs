using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class PrinterLocationPerIPList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PRINTER_LOCATION_PER_IP;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            SetControlProperties();
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvPrinterLocationRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        protected override void SetControlProperties()
        {
            List<vPrinterLocationGetIP> lst = BusinessLayer.GetvPrinterLocationGetIPList("");
            
            lst.Insert(0, new vPrinterLocationGetIP { IPAddress = "", Jumlah = 0 });

            Methods.SetComboBoxField<vPrinterLocationGetIP>(cboIPAddress, lst, "IPAddress", "IPAddress");
            cboIPAddress.SelectedIndex = 0;
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1", Constant.StandardCode.PRINTER_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDeviceType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboDeviceType.SelectedIndex = 0;
          
        }

        //public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        //{
        //    fieldListText = new string[] { "Nama", "Kode", "Tag Property" };
        //    fieldListValue = new string[] { "StandardCodeName", "StandardCodeID", "TagProperty" };
        //}

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            string ipAddress = hdnIPAddress.Value;
            if (ipAddress != "")
            {
                filterExpression += string.Format("IsDeleted = 0 AND IPAddress = '{0}'", hdnIPAddress.Value);
            }
            else
            {
                filterExpression += "IsDeleted = 0";
            }

            string DeviceType = hdnGCPrinterType.Value.ToString();
            if (DeviceType != "")
            {
                filterExpression += string.Format(" AND GCPrinterType = '{0}'", hdnGCPrinterType.Value);
            }
            //GCPrinterType
            //else
            //{
            //    filterExpression += "IsDeleted = 0";
            //}
            //
            //if (cboDeviceType.Value.ToString() != "")
            //{
            //    filterExpression += string.Format("IsDeleted = 0 AND StandardCodeID LIKE '%({0})%'", cboDeviceType.Value.ToString());
            //}
            //
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrinterLocationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrinterLocation> lstEntity = BusinessLayer.GetvPrinterLocationList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl(string.Format("~/Program/ControlPanel/PrinterLocationPerIP/PrinterLocationPerIPEntry.aspx?id=add|"));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/PrinterLocationPerIP/PrinterLocationPerIPEntry.aspx?id=edit|{0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                PrinterLocation entity = BusinessLayer.GetPrinterLocation(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrinterLocation(entity);
                return true;
            }
            return false;
        }
    }
}