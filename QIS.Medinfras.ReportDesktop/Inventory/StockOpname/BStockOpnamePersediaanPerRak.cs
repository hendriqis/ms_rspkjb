using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BStockOpnamePersediaanPerRak : BaseCustomDailyPotraitRpt
    {
        public BStockOpnamePersediaanPerRak()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vStockTakingHd entityHd = BusinessLayer.GetvStockTakingHdList(param[0]).FirstOrDefault();
            cStockOpnameNo.Text = entityHd.StockTakingNo;
            cStockOpnameDate.Text = entityHd.FormDateInString;
            cLocation.Text = entityHd.LocationName;
            cRemarks.Text = entityHd.Remarks;
            cABCClass.Text = entityHd.ABCClass;
            cItemType.Text = entityHd.ItemType;

            cStockTakingNo.Text = entityHd.StockTakingNo;
            cStockTakingLocation.Text = "Location : " + entityHd.LocationName;
            cABC.Text = "Class : " + entityHd.ABCClass;
            cStockTakingItemType.Text = "Item Type : " + entityHd.ItemType;
            cProductLine.Text = entityHd.ProductLineName;

            lblCreatedByName.Text = entityHd.CreatedByName;

            List<StockTakingDt> lst = BusinessLayer.GetStockTakingDtList(String.Format(
                "StockTakingID = {0} AND GCItemDetailStatus != '{1}'", entityHd.StockTakingID, Constant.TransactionStatus.VOID));
            decimal totalItem = lst.Count();
            decimal totalDifference = lst.Where(a => a.QuantityAdjustment != 0).Count();
            decimal totalAccurate = totalItem - totalDifference;
            decimal accuracy = totalAccurate / totalItem * 100;

            cAccuracy.Text = accuracy.ToString("N2") + "%";

            base.InitializeReport(param);
        }

    }
}
