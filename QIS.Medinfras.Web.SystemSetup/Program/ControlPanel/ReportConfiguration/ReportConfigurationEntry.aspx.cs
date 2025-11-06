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
    public partial class ReportConfigurationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.REPORT_CONFIGURATION;
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
            Methods.SetComboBoxField<StandardCode>(cboReportType, lst.Where(p => p.ParentID == Constant.StandardCode.REPORT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDataSourceType, lst.Where(p => p.ParentID == Constant.StandardCode.DATA_SOURCE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboReportType.SelectedIndex = 0;
            cboDataSourceType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtReportCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReportTitle1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReportTitle2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReportReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboReportType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDataSourceType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectTypeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAdditionalFilterExpression, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTopMargin, new ControlEntrySetting(true, true, true, 0));
        }

        private void EntityToControl(ReportMaster entity)
        {
            txtReportCode.Text = entity.ReportCode;
            txtReportTitle1.Text = entity.ReportTitle1;
            txtReportTitle2.Text = entity.ReportTitle2;
            txtReportReferenceNo.Text = entity.ReportReferenceNo;
            cboReportType.Value = entity.GCReportType;
            txtClassName.Text = entity.ClassName;
            cboDataSourceType.Value = entity.GCDataSourceType;
            txtObjectTypeName.Text = entity.ObjectTypeName;
            txtAdditionalFilterExpression.Text = entity.AdditionalFilterExpression;
            chkIsShowHeader.Checked = entity.IsShowHeader;
            chkIsShowFooter.Checked = entity.IsShowFooter;
            chkIsShowParameter.Checked = entity.IsShowParameter;
            txtTopMargin.Text = entity.TopMargin.ToString();
            chkIsDirectPrint.Checked = entity.IsDirectPrint;
            chkIsUsingPreview.Checked = entity.IsUsingPreview;
            chkIsAllowDownloadExcel.Checked = entity.IsAllowDownloadExcel;
            chkIsAllowDownloadRawExcel.Checked = entity.IsAllowDownloadRawExcel;
            chkIsAllowDownloadRawExcelStaging.Checked = entity.IsAllowDownloadRawExcelDBStaging;
            chkIsAllowDownloadImageExcel.Checked = entity.IsAllowDownloadImageExcel;
        }

        private void ControlToEntity(ReportMaster entity)
        {
            entity.ReportCode = txtReportCode.Text;
            entity.ReportTitle1 = txtReportTitle1.Text;
            entity.ReportTitle2 = txtReportTitle2.Text;
            entity.ReportReferenceNo = txtReportReferenceNo.Text;
            entity.GCReportType = cboReportType.Value.ToString();
            entity.ClassName = txtClassName.Text;
            entity.GCDataSourceType = cboDataSourceType.Value.ToString();
            entity.ObjectTypeName = txtObjectTypeName.Text;
            entity.AdditionalFilterExpression = txtAdditionalFilterExpression.Text;
            entity.IsShowHeader = chkIsShowHeader.Checked;
            entity.IsShowFooter = chkIsShowFooter.Checked;
            entity.IsShowParameter = chkIsShowParameter.Checked;
            entity.TopMargin = Convert.ToInt16(txtTopMargin.Text);
            entity.IsDirectPrint = chkIsDirectPrint.Checked;
            entity.IsUsingPreview = chkIsUsingPreview.Checked;
            entity.IsAllowDownloadExcel = chkIsAllowDownloadExcel.Checked;
            entity.IsAllowDownloadRawExcel = chkIsAllowDownloadRawExcel.Checked;
            entity.IsAllowDownloadRawExcelDBStaging = chkIsAllowDownloadRawExcelStaging.Checked;
            entity.IsAllowDownloadImageExcel = chkIsAllowDownloadImageExcel.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ReportCode = '{0}'", txtReportCode.Text);
            List<ReportMaster> lst = BusinessLayer.GetReportMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Report With Code " + txtReportCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ReportCode = '{0}' AND ReportID != {1}", txtReportCode.Text, hdnID.Value);
            List<ReportMaster> lst = BusinessLayer.GetReportMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Report With Code " + txtReportCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ReportMasterDao entityDao = new ReportMasterDao(ctx);
            bool result = false;
            try
            {
                ReportMaster entity = new ReportMaster();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetReportMasterMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
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