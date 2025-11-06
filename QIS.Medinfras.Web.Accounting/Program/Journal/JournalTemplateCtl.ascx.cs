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
    public partial class JournalTemplateCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        
        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            hdnGLTransactionIDTemplateCtl.Value = param;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList("IsDeleted = 0 AND IsActive = 1");
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
            string filterExpression = string.Format("TemplateID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder, GLAccountNo", hdnTemplateID.Value);
            List<vJournalTemplateDt> lstEntity = BusinessLayer.GetvJournalTemplateDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);

            try
            {
                int GLTransactionID = 0;
                if (hdnGLTransactionIDTemplateCtl.Value != null && hdnGLTransactionIDTemplateCtl.Value != "")
                {
                    GLTransactionID = Convert.ToInt32(hdnGLTransactionIDTemplateCtl.Value);
                }
                DetailPage.SaveGLTransactionHd(ctx, ref GLTransactionID);
                if (GLTransactionID != 0)
                {
                    List<String> lstSelectedID = hdnSelectedID.Value.Split('|').ToList();
                    List<String> lstSelectedCOA = hdnSelectedCOA.Value.Split('|').ToList();
                    List<String> lstSelectedValue = hdnSelectedValue.Value.Split('|').ToList();
                    List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split('|').ToList();

                    lstSelectedID.RemoveAt(0);
                    lstSelectedCOA.RemoveAt(0);
                    lstSelectedValue.RemoveAt(0);
                    lstSelectedRemarks.RemoveAt(0);

                    string filterDt = string.Format("ID IN ({0})", hdnSelectedID.Value.Substring(1).Replace("|", ","));
                    List<JournalTemplateDt> lstJournalTemplate = BusinessLayer.GetJournalTemplateDtList(filterDt, ctx);

                    for (int i = 0; i < lstSelectedID.Count(); i++)
                    {
                        JournalTemplateDt entity = lstJournalTemplate.Where(a => a.ID.ToString() == lstSelectedID[i]).FirstOrDefault();

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

                        if (glTransactionDt.Position == "D")
                        {
                            glTransactionDt.DebitAmount = Convert.ToDecimal(lstSelectedValue[i]);
                        }
                        else
                        {
                            glTransactionDt.CreditAmount = Convert.ToDecimal(lstSelectedValue[i]);
                        }

                        if (txtRemarks.Text != "" && txtRemarks.Text != null && lstSelectedRemarks[i] != "" && lstSelectedRemarks[i] != null)
                        {
                            glTransactionDt.Remarks = txtRemarks.Text + " | " + lstSelectedRemarks[i];
                        }
                        else if (lstSelectedRemarks[i] != "" && lstSelectedRemarks[i] != null)
                        {
                            glTransactionDt.Remarks = lstSelectedRemarks[i];
                        }
                        else
                        {
                            glTransactionDt.Remarks = txtRemarks.Text;
                        }

                        glTransactionDt.DisplayOrder = Convert.ToInt16(entity.DisplayOrder + DetailPage.GetDisplayCount());
                        glTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        glTransactionDt.CreatedBy = AppSession.UserLogin.UserID;
                        glTransactionDtDao.Insert(glTransactionDt);
                    }

                    retval = GLTransactionID.ToString();

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Jurnal Pada Periode ini Telah Diposting";
                    ctx.RollBackTransaction();
                }
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