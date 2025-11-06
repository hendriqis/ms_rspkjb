using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxPivotGrid.Export;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PivotOptionCtl : System.Web.UI.UserControl
    {
        protected string GetLabel(string code)
        {
            return ((BasePageList)Page).GetLabel(code);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnPivotSaveName.Value = ((HtmlInputHidden)Helper.FindControlRecursive(Page, "hdnFileName")).Value;
            }
        }

        public void Export(bool saveAs)
        {
            ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = chkPrintHeadersOnEveryPage.Checked;
            ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = chkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = chkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = chkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

            string name = hdnPivotSaveName.Value;
            string fileName = string.Format("{0}{1}", name, DateTime.Now.ToString("yyyyMMddHHmmss"));

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

        protected void btnSavePivot_Click(object sender, EventArgs e)
        {
            Export(true);
        }
    }
}