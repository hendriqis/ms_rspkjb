using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class InformasiKelengkapanBerkasMedisPasienCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            List<StandardCode> lstStd = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEDICAL_FOLDER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboMedicalFile, lstStd.Where(x => x.ParentID == Constant.StandardCode.MEDICAL_FOLDER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboMedicalFile.SelectedIndex = 0;

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(String.Format("RegistrationID = '{0}'", param)).FirstOrDefault();
            hdnVisitID.Value = entityCV.VisitID.ToString();
            txtRegistrationNo.Text = entityCV.RegistrationNo;

            BindGridView(1, true, ref PageCount);
        }

        private List<VisitMRFolderStatus> lstVisitMRFolderStatus = null;

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("GCMedicalFolderType = '{1}'", hdnVisitID.Value, cboMedicalFile.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMedicalRecordFolderRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstVisitMRFolderStatus = BusinessLayer.GetVisitMRFolderStatusList(string.Format("VisitID = {0} AND GCMedicalFolderType = '{1}'", hdnVisitID.Value, cboMedicalFile.Value));
            List<vMedicalRecordFolder> lstEntity = BusinessLayer.GetvMedicalRecordFolderList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FormCode");
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vMedicalRecordFolder entity = e.Row.DataItem as vMedicalRecordFolder;
                if (entity != null)
                {
                    List<StandardCode> lstStatusNotes = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.JENIS_CATATAN_STATUS_RM));
                    DropDownList cboStatusNotes = e.Row.FindControl("cboMRStatusNotes") as DropDownList;
                    cboStatusNotes.DataValueField = "StandardCodeID";
                    cboStatusNotes.DataTextField = "StandardCodeName";
                    cboStatusNotes.DataSource = lstStatusNotes;
                    cboStatusNotes.DataBind();
                    CheckBox chkIsExist = e.Row.FindControl("chkIsExist") as CheckBox;
                    CheckBox chkIsCompleted = e.Row.FindControl("chkIsCompleted") as CheckBox;
                    TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
                    TextBox txtFormDate = e.Row.FindControl("txtFormDate") as TextBox;
                    TextBox txtFormTime = e.Row.FindControl("txtFormTime") as TextBox;

                    VisitMRFolderStatus visitMRFolderStatus = lstVisitMRFolderStatus.FirstOrDefault(p => p.FormID == entity.FormID);
                    if (visitMRFolderStatus != null)
                    {
                        chkIsExist.Checked = visitMRFolderStatus.IsExists;
                        chkIsCompleted.Checked = visitMRFolderStatus.IsCompleted;
                        cboStatusNotes.SelectedValue = visitMRFolderStatus.GCMRStatusNote;
                        txtRemarks.Text = visitMRFolderStatus.Remarks;
                        if (visitMRFolderStatus.FormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == "01-01-1900" || visitMRFolderStatus.FormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == null)
                        {
                            txtFormDate.Text = "";
                        }
                        else
                        {
                            txtFormDate.Text = visitMRFolderStatus.FormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        }
                        txtFormTime.Text = visitMRFolderStatus.FormTime.ToString();
                    }
                }
            }
        }
    }
}