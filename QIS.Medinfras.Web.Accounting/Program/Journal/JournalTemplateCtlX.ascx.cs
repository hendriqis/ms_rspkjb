using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalTemplateCtlX : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        
        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            hdnGLTransactionID.Value = param;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList("IsDeleted = 0");
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboJournalTemplateType, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.JOURNAL_TEMPLATE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            
        }

        private JournalEntry DetailPage
        {
            get { return (JournalEntry)Page; }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTransactionAmount, new ControlEntrySetting(true, true, true, "0"));
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);

            try
            {
                int GLTransactionID = 0;
                if (hdnGLTransactionID.Value != null && hdnGLTransactionID.Value != "")
                    GLTransactionID = Convert.ToInt32(hdnGLTransactionID.Value);
                DetailPage.SaveGLTransactionHd(ctx, ref GLTransactionID);
                if (GLTransactionID != 0)
                {
                    List<JournalTemplateDt> lstJournalTemplate = BusinessLayer.GetJournalTemplateDtList(String.Format("TemplateID = {0}", hdnTemplateID.Value));

                    foreach (JournalTemplateDt entity in lstJournalTemplate)
                    {
                        GLTransactionDt glTransactionDt = new GLTransactionDt();
                        glTransactionDt.GLTransactionID = GLTransactionID;
                        glTransactionDt.GLAccount = entity.GLAccountID;
                        glTransactionDt.SubLedger = entity.SubLedgerID;
                        glTransactionDt.HealthcareID = entity.HealthcareID;
                        glTransactionDt.DepartmentID = entity.DepartmentID;
                        glTransactionDt.ServiceUnitID = entity.ServiceUnitID;
                        glTransactionDt.RevenueCostCenterID = entity.RevenueCostCenterID;
                        glTransactionDt.BusinessPartnerID = entity.BusinessPartnerID;
                        glTransactionDt.CustomerGroupID = entity.CustomerGroupID;
                        glTransactionDt.Position = entity.Position;


                        //////Decimal amount = Convert.ToDecimal(txtTransactionAmount.Text);

                        //////if (cboJournalTemplateType.Value.ToString() == Constant.JournalTemplateType.ALOKASI)
                        //////{
                        //////    if (entity.Amount != 0)
                        //////    {
                        //////        if (entity.Position == "D")
                        //////        {
                        //////            glTransactionDt.DebitAmount = entity.Amount;
                        //////        }
                        //////        else
                        //////        {
                        //////            glTransactionDt.CreditAmount = entity.Amount;
                        //////        }
                        //////    }
                        //////    else
                        //////    {
                        //////        if (entity.Position == "D")
                        //////        {
                        //////            glTransactionDt.DebitAmount = amount * (entity.AmountPercentage / 100);
                        //////        }
                        //////        else
                        //////        {
                        //////            glTransactionDt.CreditAmount = amount * (entity.AmountPercentage / 100);
                        //////        }
                        //////    }
                        //////}
                        //////else
                        //////{
                        //////    if (entity.Amount != 0)
                        //////    {
                        //////        if (entity.Position == "D")
                        //////        {
                        //////            glTransactionDt.DebitAmount = entity.Amount;
                        //////        }
                        //////        else
                        //////        {
                        //////            glTransactionDt.CreditAmount = entity.Amount;
                        //////        }
                        //////    }
                        //////    else
                        //////    {
                        //////        if (entity.Position == "D")
                        //////        {
                        //////            glTransactionDt.DebitAmount = amount * (entity.AmountPercentage / 100);
                        //////        }
                        //////        else
                        //////        {
                        //////            glTransactionDt.CreditAmount = amount * (entity.AmountPercentage / 100);
                        //////        }
                        //////    }
                        //////}

                        glTransactionDt.Remarks = txtRemarks.Text;
                        glTransactionDt.DisplayOrder = Convert.ToInt16(entity.DisplayOrder + DetailPage.GetDisplayCount());
                        glTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        glTransactionDt.CreatedBy = AppSession.UserLogin.UserID;
                        glTransactionDtDao.Insert(glTransactionDt);
                    }

                    retval = GLTransactionID.ToString();
                }
                else 
                {
                    errMessage = "Jurnal Pada Periode ini Telah Diposting";
                    result = false;
                }
                ctx.CommitTransaction();
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

    }
}