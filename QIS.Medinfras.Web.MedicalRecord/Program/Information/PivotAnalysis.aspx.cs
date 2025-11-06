using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PivotAnalysis : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PIVOT_ANALYSIS;
        }
        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            UpdatePivotGridFieldLayout();
        }

        void UpdatePivotGridFieldLayout()
        {
            ChangePivotGridFieldLayout();
        }
        void ChangePivotGridFieldLayout()
        {
            pvRegistraition.BeginUpdate();
            foreach (PivotGridField field in pvRegistraition.Fields)
            {
                field.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea;
                field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            }

            pvRegistraition.Fields["Year"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvRegistraition.Fields["Year"].AreaIndex = 0;
            pvRegistraition.Fields["Month"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvRegistraition.Fields["Month"].AreaIndex = 1;
            pvRegistraition.Fields["visitID"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvRegistraition.Fields["ServiceUnit"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvRegistraition.Fields["ServiceUnit"].AreaIndex = 0;
            pvRegistraition.Fields["ServiceUnit"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;


            pvRegistraition.Width = Unit.Percentage(100);
            pvRegistraition.EndUpdate();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                Export(true);
                return true;
            }
            else if (type == "open")
            {
                Export(false);
                return true;
            }
            return false;
        }

        protected void btnSavePivot_Click(object sender, EventArgs e)
        {
            Export(true);
        }

        void Export(bool saveAs)
        {
            ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = chkPrintHeadersOnEveryPage.Checked;
            ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = chkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = chkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = chkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

            string fileName = "PivotGrid";
            switch (cboListExportFormat.SelectedIndex)
            {
                case 0:
                    ASPxPivotGridExporter1.ExportPdfToResponse(fileName, saveAs);
                    break;
                case 1:
                    ASPxPivotGridExporter1.ExportXlsToResponse(fileName, saveAs);
                    break;
            }
        }

    }
}