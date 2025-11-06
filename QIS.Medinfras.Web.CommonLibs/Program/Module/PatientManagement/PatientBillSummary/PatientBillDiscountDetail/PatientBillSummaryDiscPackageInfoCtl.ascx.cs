using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryDiscPackageInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] data = param.Split('|');
            hdnChargesDtID.Value = data[0];

            hdnVisitIDCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();

            PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(hdnChargesDtID.Value));
            hdnChargesClassID.Value = chargesDt.ChargeClassID.ToString();

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHd(chargesDt.TransactionID);
            hdnChargesHealthcareServiceUnitID.Value = entityHd.HealthcareServiceUnitID.ToString();
            hdnTransactionDate.Value = entityHd.TransactionDateInDatePickerFormat;
            hdnTransactionTime.Value = entityHd.TransactionTime;

            ItemMaster imaster = BusinessLayer.GetItemMaster(Convert.ToInt32(data[1]));
            hdnItemID.Value = imaster.ItemID.ToString();
            txtItemServiceName.Text = string.Format("{0} - {1}", imaster.ItemCode, imaster.ItemName1);

            SetControlEntrySetting();

            BindGridView();
        }

        private void SetControlEntrySetting()
        {
        }

        private void BindGridView()
        {
            string filterDt = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0 ORDER BY ID ASC", hdnChargesDtID.Value);
            List<vPatientChargesDtPackage> lstItemService = BusinessLayer.GetvPatientChargesDtPackageList(filterDt);

            grdView.DataSource = from i in lstItemService
                                 where i.GCItemType != Constant.ItemType.OBAT_OBATAN && i.GCItemType != Constant.ItemType.BARANG_MEDIS && i.GCItemType != Constant.ItemType.BARANG_UMUM
                                 select i;
            grdView.DataBind();

            grdViewObat.DataSource = from i in lstItemService
                                     where i.GCItemType == Constant.ItemType.OBAT_OBATAN || i.GCItemType == Constant.ItemType.BARANG_MEDIS
                                     select i;
            grdViewObat.DataBind();

            grdViewBarang.DataSource = from i in lstItemService
                                       where i.GCItemType == Constant.ItemType.BARANG_UMUM
                                       select i;
            grdViewBarang.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdViewObat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupViewObat_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdViewBarang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupViewBarang_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        
    }
}