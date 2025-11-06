using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Extensions;
using QIS.Medinfras.Data.Service;
using System.Reflection;
using DevExpress.XtraReports.Native;

namespace QIS.Medinfras.ReportDesigner
{
    public partial class frmReportDesigner : Form
    {
        String reportPath;
        private string reportName = "";
        private string methodName = "";

        private string filterExp = "";
        private bool isSaved = false;
        SaveCommandHandler customSave;
        static frmReportDesigner()
        {
            ReportExtension.RegisterExtensionGlobal(new ReportExtension());
            ReportDesignExtension.RegisterExtension(new DesignExtension(), ExtensionName);
        }
        public frmReportDesigner()
        {
            InitializeComponent();
            var bs = new BindingSource();
            bs.DataSource = BusinessLayer.GetReportMasterList("");
            dgvReport.DataSource = bs;
            StreamReader sr = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString())+"\\rpt.conf");
            reportPath = sr.ReadLine();
        }
        private static string ExtensionName = "CustomExtension";
        XRDesignMdiController mdiController;
        public void CreateNewReport(int rptType,ReportMaster rm) 
        {
            const int portraitRpt = 0;
            const int landscapeRpt = 1;
            const int customRpt = 2;
            string baseRptFolder = reportPath + "\\Base\\";
            switch (rptType) 
            {
                case portraitRpt:
                    baseRptFolder += "BaseDailyPortraitRpt";
                    break;
                case landscapeRpt:
                    baseRptFolder += "BaseDailyLandscapeRpt";
                    break;
                case customRpt:
                    baseRptFolder += "BaseCustomPaperRpt";
                    break;
            }
            XtraReport rpt = new XtraReport();
            rpt.LoadLayoutFromXml(baseRptFolder);
            XRDesignForm form = new XRDesignForm();
            mdiController = form.DesignMdiController;
            mdiController.DesignPanelLoaded +=
                new DesignerLoadedEventHandler(mdiController_DesignPanelLoaded);

            if (!methodName.Equals(""))
            {
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                dynamic obj = method.Invoke(null, new string[] { filterExp });
                rpt.DataSource = obj;
            }
            mdiController.OpenReport(rpt);
            form.ShowDialog();
            if (customSave.isSaved) {
                BusinessLayer.InsertReportMaster(rm);
            }
            if (mdiController.ActiveDesignPanel != null)
            {
                mdiController.ActiveDesignPanel.CloseReport();
            }
        }
        private void loadReportfromFile_Click(object sender, EventArgs e)
        {
            XtraReport rpt = new XtraReport();
            XRDesignForm form = new XRDesignForm();
            mdiController = form.DesignMdiController;
            mdiController.DesignPanelLoaded +=
                new DesignerLoadedEventHandler(mdiController_DesignPanelLoaded);
            DataGridViewRow selectedRow = this.dgvReport.SelectedRows[0];
            if (dgvReport.SelectedRows.Count > 0)
            {
                methodName = selectedRow.Cells["ObjectTypeName"].Value.ToString();
                reportName = selectedRow.Cells["ClassName"].Value.ToString();
                filterExp = selectedRow.Cells["AdditionalFilterExpression"].Value.ToString();
            }
            string customReport = reportPath + "\\Custom\\"+reportName;
            if (!File.Exists(customReport))
            {
                reportName = reportPath + "\\Original\\" + reportName;
            }
            else
            {
                reportName = customReport;
            }
            rpt.LoadLayoutFromXml(reportName);
            if (!methodName.Equals(""))
            {  
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                dynamic obj = method.Invoke(null, new string[] { filterExp });
                rpt.DataSource = obj;
            }
            mdiController.OpenReport(rpt);
            form.ShowDialog();
            if (mdiController.ActiveDesignPanel != null)
            {
                mdiController.ActiveDesignPanel.CloseReport();
            }
        }
        private void mnuSettings_Click(object sender, EventArgs e)
        {
            frmReportSetting frmSettings = new frmReportSetting();
            frmSettings.ShowDialog();
        }
        private void btnNewReport_Click(object sender, EventArgs e)
        {
            frmNewReport frmRpt = new frmNewReport();
            if (frmRpt.ShowDialog() == DialogResult.OK) {
                this.methodName = frmRpt.rm.ObjectTypeName;
                this.reportName = frmRpt.rm.ClassName;
                this.CreateNewReport(frmRpt.reportOrientation,frmRpt.rm);
            };
        }

        void mdiController_DesignPanelLoaded(object sender, DesignerLoadedEventArgs e)
        {
            XRDesignPanel panel = (XRDesignPanel)sender;
            customSave = new SaveCommandHandler(panel, reportPath + "\\Custom\\" + reportName);
            panel.AddCommandHandler(customSave);
        }
        #region "SaveAndLoadXMLExtension"
        class ReportExtension : ReportStorageExtension
        {
            public override void SetData(XtraReport report, Stream stream)
            {
                report.SaveLayoutToXml(stream);
            }
        }
        class DesignExtension : ReportDesignExtension
        {
            protected override bool CanSerialize(object data)
            {
                return data is DataSet || data is OleDbDataAdapter;
            }
            protected override string SerializeData(object data, XtraReport report)
            {
                if (data is DataSet)
                    return (data as DataSet).GetXmlSchema();
                if (data is OleDbDataAdapter)
                {
                    OleDbDataAdapter adapter = data as OleDbDataAdapter;
                    return adapter.SelectCommand.Connection.ConnectionString +
                        "\r\n" + adapter.SelectCommand.CommandText;
                }

                return base.SerializeData(data, report);
            }
            protected override bool CanDeserialize(string value, string typeName)
            {
                return typeof(DataSet).FullName ==
                    typeName || typeof(OleDbDataAdapter).FullName == typeName;
            }
            protected override object DeserializeData(string value, string typeName, XtraReport report)
            {
                if (typeof(DataSet).FullName == typeName)
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(new StringReader(value));
                    return dataSet;
                }
                if (typeof(OleDbDataAdapter).FullName == typeName)
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    string[] values = value.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    adapter.SelectCommand = new OleDbCommand(values[1], new OleDbConnection(values[0]));
                    return adapter;
                }
                
                return base.DeserializeData(value, typeName, report);
            }
        }
        #endregion 
        #region "Override Designer Save Method"
        public class SaveCommandHandler : DevExpress.XtraReports.UserDesigner.ICommandHandler
        {
            string reportPath;
            XRDesignPanel panel;
            public bool isSaved;
            public SaveCommandHandler(XRDesignPanel panel,string reportPath)
            {
                this.panel = panel;
                isSaved = false;
                this.reportPath = reportPath;
            }

            #region ICommandHandler Members

            public bool CanHandleCommand(ReportCommand command)
            {
                if (command == ReportCommand.SaveFile || command == ReportCommand.SaveFileAs) 
                    return true;
                return false;
            }

            public void HandleCommand(ReportCommand command, object[] args, ref bool handled)
            {
                Save();
            }
            void Save()
            {
                panel.Report.SaveLayoutToXml(reportPath);
                panel.ReportState = ReportState.Saved;
                isSaved = true;
            }
            #endregion
        }
        #endregion "Override Designer Save Method"
    }
}
