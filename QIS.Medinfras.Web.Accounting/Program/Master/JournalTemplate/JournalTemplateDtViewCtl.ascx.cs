using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalTemplateDtViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnTemplateID.Value = param;

            JournalTemplateHd entity = BusinessLayer.GetJournalTemplateHd(Convert.ToInt32(hdnTemplateID.Value));
            txtTemplateCode.Text = entity.TemplateCode;
            txtTemplateName.Text = entity.TemplateName;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList("IsDeleted = 0");
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboJournalTemplateType, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.JOURNAL_TEMPLATE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboJournalTemplateType.Value = entity.GCJournalTemplateType;

            if (cboJournalTemplateType.Value.ToString() == Constant.JournalTemplateType.ALOKASI)
            {
                txtTransactionAmount.Attributes.Remove("readonly");
                btnCalculate.Attributes.Remove("style");
            }
            else
            {
                txtTransactionAmount.Attributes.Add("readonly", "readonly");
                btnCalculate.Attributes.Add("style", "display:none");
            }

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TemplateID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnTemplateID.Value);
            List<vJournalTemplateDt> lstEntity = BusinessLayer.GetvJournalTemplateDtList(filterExpression);
            grdViewD.DataSource = lstEntity.Where(p => p.Position == "D").ToList();
            grdViewD.DataBind();

            grdViewK.DataSource = lstEntity.Where(p => p.Position == "K").ToList();
            grdViewK.DataBind();
        }

        protected void grdViewD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vJournalTemplateDt entity = e.Row.DataItem as vJournalTemplateDt;
                TextBox txtAmountD = e.Row.FindControl("txtAmountD") as TextBox;
                TextBox txtAmountK = e.Row.FindControl("txtAmountK") as TextBox;

                if (entity.Position == "D")
                {
                    txtAmountD.Text = entity.Amount.ToString();
                    txtAmountK.Text = "0";
                }
            }

        }

        protected void grdViewK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vJournalTemplateDt entity = e.Row.DataItem as vJournalTemplateDt;
                TextBox txtAmountD = e.Row.FindControl("txtAmountD") as TextBox;
                TextBox txtAmountK = e.Row.FindControl("txtAmountK") as TextBox;

                if (entity.Position == "K")
                {
                    txtAmountK.Text = entity.Amount.ToString();
                    txtAmountD.Text = "0";
                }
            }

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

            BindGridView();
        }
    }
}