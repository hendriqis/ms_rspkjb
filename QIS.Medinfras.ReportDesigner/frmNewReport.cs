using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesigner
{
    public partial class frmNewReport : Form
    {
        public ReportMaster rm;
        public int reportOrientation = 0;
        const string reportTypeCode = "X140";
        const string reportDataSourceTypeCode = "X141";
        string methodExt = "";
        List<SysObjects> lstTable, lstView;
        public frmNewReport()
        {
            InitializeComponent();
            rm = null;
            List<StandardCode> lstOfRptType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", reportTypeCode));
            List<StandardCode> lstOfDSType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0",reportDataSourceTypeCode));
            cboReportType.DataSource = lstOfRptType;
            cboReportType.DisplayMember = "StandardCodeName";
            cboReportType.ValueMember = "StandardCodeID";
            cboSourceType.DataSource = lstOfDSType;
            cboSourceType.DisplayMember = "StandardCodeName";
            cboSourceType.ValueMember = "StandardCodeID";
            cboOrientation.SelectedIndex = 0;
            lstTable = BusinessLayer.GetSysObjectsList("type = 'U' ORDER BY name ASC");
            lstView = BusinessLayer.GetSysObjectsList("type = 'V' ORDER BY name ASC");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String objectTypeName = methodExt + cboSourceName.SelectedValue.ToString() + "List";
            ReportMaster rm = new ReportMaster();
            rm.ReportCode = txtReportCode.Text.ToString();
            rm.ReportTitle1 = txtReportTitle1.Text.ToString();
            rm.ReportTitle2 = txtReportTitle1.Text.ToString();
            rm.ClassName = txtReportName.Text.ToString();
            rm.GCDataSourceType = cboSourceType.SelectedValue.ToString();
            rm.GCReportType = cboReportType.SelectedValue.ToString();
            rm.ObjectTypeName = objectTypeName;
            rm.AdditionalFilterExpression = txtFilterExp.Text.ToString();
            string topMargin = txtTopMargin.Text.ToString();
            if (topMargin.Equals("")) {
                topMargin = "0";
            }
            rm.TopMargin = Convert.ToInt16(topMargin);
            rm.IsCustomSetting = cbCustomSetting.Checked;
            if (cbCustomSetting.Checked) {
                rm.CustomSettingUrl = txtCustomUrl.Text.ToString();
            }
            rm.IsShowHeader = cbShowHeader.Checked;
            rm.IsShowParameter = cbShowParameter.Checked;
            rm.IsReportBasedOnUserLogin = cbBasedOnLogin.Checked;
            rm.IsDeleted = false;
            rm.CreatedDate = rm.LastUpdatedDate = DateTime.Now;
            //BusinessLayer.InsertReportMaster(rm);
            this.rm = rm;
            this.reportOrientation = cboOrientation.SelectedIndex;
            //this.methodName = rm.ObjectTypeName;
            //this.reportName = rm.ClassName;
        }

        private void cboSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            StandardCode stdCode = (StandardCode)cboSourceType.SelectedItem;
            methodExt = "Get";
            switch (stdCode.StandardCodeName)
            {
                case "View":
                    cboSourceName.DataSource = lstView;
                    break;
                case "Stored Procedure":
                    cboSourceName.DataSource = lstTable;
                    break;
            }
            cboSourceName.Enabled = true;
            cboSourceName.ValueMember = "Name";
            cboSourceName.DisplayMember = "Name";
        }
        private void cboSourceName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
