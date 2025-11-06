using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ReportConfigurationUserEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.REPORT_CONFIGURATION_USER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                ReportMaster entity = BusinessLayer.GetReportMaster(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtReportCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.REPORT_TYPE, Constant.StandardCode.DATA_SOURCE_TYPE));
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtReportCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtReportTitle1, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(ReportMaster entity)
        {
            txtReportCode.Text = entity.ReportCode;
            txtReportTitle1.Text = entity.ReportTitle1;
            chkIsUsingPreview.Checked = entity.IsUsingPreview;
            chkIsShowHeader.Checked = entity.IsShowHeader;
            chkIsShowFooter.Checked = entity.IsShowFooter;
            chkIsAllowDownloadExcel.Checked = entity.IsAllowDownloadExcel;
            chkIsAllowDownloadRawExcel.Checked = entity.IsAllowDownloadRawExcel;
            chkIsAllowDownloadRawExcelStaging.Checked = entity.IsAllowDownloadRawExcelDBStaging;
            chkIsAllowDownloadImageExcel.Checked = entity.IsAllowDownloadImageExcel;
        }

        private void ControlToEntity(ReportMaster entity)
        {
            entity.IsUsingPreview = chkIsUsingPreview.Checked;
            entity.IsShowHeader = chkIsShowHeader.Checked;
            entity.IsShowFooter = chkIsShowFooter.Checked;
            entity.IsAllowDownloadExcel = chkIsAllowDownloadExcel.Checked;
            entity.IsAllowDownloadRawExcel = chkIsAllowDownloadRawExcel.Checked;
            entity.IsAllowDownloadRawExcelDBStaging = chkIsAllowDownloadRawExcelStaging.Checked;
            entity.IsAllowDownloadImageExcel = chkIsAllowDownloadImageExcel.Checked;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ReportMaster entity = BusinessLayer.GetReportMaster(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateReportMaster(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}