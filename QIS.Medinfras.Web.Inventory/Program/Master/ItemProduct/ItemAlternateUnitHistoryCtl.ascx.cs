using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemAlternateUnitHistoryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnItemIDCtl.Value = param;

            ItemMaster item = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemIDCtl.Value));
            txtItemName.Text = String.Format("{0} | {1}", item.ItemCode, item.ItemName1);

            BindGridView();
        }

        private void BindGridView()
        {
            List<tempHistory> lstHistory = new List<tempHistory>();
            List<tempHistoryDt> lstHistoryDetail = new List<tempHistoryDt>();

            string filterExpression = string.Format("ItemID = {0}", hdnItemIDCtl.Value);
            List<vAuditLogItemAlternateHistory> lstEntity = BusinessLayer.GetvAuditLogItemAlternateHistoryList(filterExpression);
            foreach (vAuditLogItemAlternateHistory entity in lstEntity)
            {
                tempHistory hist = new tempHistory();

                hist.ID = entity.ID.ToString();
                hist.LogDate = entity.LogDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                hist.UserFullName = entity.UserFullName;

                if (entity.OldValues != "")
                {
                    tempHistoryDt oldDt = JsonConvert.DeserializeObject<tempHistoryDt>(entity.OldValues);
                    hist.OldAlternateUnit = oldDt.AlternateUnit;
                    hist.OldConversionFactor = oldDt.ConversionFactor;
                    hist.OldIsDeleted = oldDt.IsDeleted;
                    hist.OldCreatedByName = oldDt.CreatedByName;
                    hist.OldCreatedDate = oldDt.CreatedDate;
                    hist.OldLastUpdatedByName = oldDt.LastUpdatedByName;
                    hist.OldLastUpdatedDate = oldDt.LastUpdatedDate;
                }
                else
                {
                    hist.OldAlternateUnit = "";
                    hist.OldConversionFactor = "";
                    hist.OldIsDeleted = "";
                    hist.OldCreatedByName = "";
                    hist.OldCreatedDate = "";
                    hist.OldLastUpdatedByName = "";
                    hist.OldLastUpdatedDate = "";
                }

                if (entity.NewValues != "")
                {
                    tempHistoryDt newDt = JsonConvert.DeserializeObject<tempHistoryDt>(entity.NewValues);
                    hist.NewAlternateUnit = newDt.AlternateUnit;
                    hist.NewConversionFactor = newDt.ConversionFactor;
                    hist.NewIsDeleted = newDt.IsDeleted;
                    hist.NewCreatedByName = newDt.CreatedByName;
                    hist.NewCreatedDate = newDt.CreatedDate;
                    hist.NewLastUpdatedByName = newDt.LastUpdatedByName;
                    hist.NewLastUpdatedDate = newDt.LastUpdatedDate;
                }
                else
                {
                    hist.NewAlternateUnit = "";
                    hist.NewConversionFactor = "";
                    hist.NewIsDeleted = "";
                    hist.NewCreatedByName = "";
                    hist.NewCreatedDate = "";
                    hist.NewLastUpdatedByName = "";
                    hist.NewLastUpdatedDate = "";
                }

                lstHistory.Add(hist);
            }

            lvwView.DataSource = lstHistory;
            lvwView.DataBind();
        }
    }

    public class tempHistoryDt
    {
        public string ID { get; set; }
        public string ItemID { get; set; }
        public string GCAlternateUnit { get; set; }
        public string AlternateUnit { get; set; }
        public string ConversionFactor { get; set; }
        public string ConversionFactorLabel { get; set; }
        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public string LastUpdatedDate { get; set; }
    }

    public class tempHistory
    {
        public string ID { get; set; }
        public string LogDate { get; set; }
        public string UserFullName { get; set; }
        public string OldAlternateUnit { get; set; }
        public string OldConversionFactor { get; set; }
        public string OldIsDeleted { get; set; }
        public string OldCreatedByName { get; set; }
        public string OldCreatedDate { get; set; }
        public string OldLastUpdatedByName { get; set; }
        public string OldLastUpdatedDate { get; set; }
        public string NewAlternateUnit { get; set; }
        public string NewConversionFactor { get; set; }
        public string NewIsDeleted { get; set; }
        public string NewCreatedByName { get; set; }
        public string NewCreatedDate { get; set; }
        public string NewLastUpdatedByName { get; set; }
        public string NewLastUpdatedDate { get; set; }
    }

    public class SortList : IComparer<string>
    {
        public int Compare(string x, string y)
        {

            if (x == null || y == null)
            {
                return 0;
            }

            // "CompareTo()" method 
            return x.CompareTo(y);

        }
    }
}