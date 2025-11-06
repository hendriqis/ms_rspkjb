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
    public partial class JournalAcrualPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        
        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            hdnGLTransactionIDCtlAcc.Value = param;

            List<Variable> lst = new List<Variable>();
            lst.Insert(0, new Variable { Code = "0", Value = "Original" });
            lst.Insert(1, new Variable { Code = "1", Value = "Jurnal Balik" });
            Methods.SetComboBoxField<Variable>(cboCopyType, lst, "Value", "Code");
            cboCopyType.SelectedIndex = 0;
        }

        private JournalEntry DetailPage
        {
            get { return (JournalEntry)Page; }
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("JournalNo = '{0}' AND GCTransactionStatus != '{1}' AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder ASC",
                                            txtJournalNoSelected.Text, Constant.TransactionStatus.VOID);
            List<vGLTransactionDt> lstEntity = BusinessLayer.GetvGLTransactionDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (lstEntity.Count() > 0)
            {
                hdnGLTransactionIDCtlCopySelected.Value = lstEntity.FirstOrDefault().GLTransactionID.ToString();
            }
            else
            {
                hdnGLTransactionIDCtlCopySelected.Value = "";
                txtJournalNoSelected.Text = "";
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);

            try
            {
                int GLTransactionID = 0;
                if (hdnGLTransactionIDCtlAcc.Value != null && hdnGLTransactionIDCtlAcc.Value != "")
                {
                    GLTransactionID = Convert.ToInt32(hdnGLTransactionIDCtlAcc.Value);
                }
                DetailPage.SaveGLTransactionHd(ctx, ref GLTransactionID);
                if (GLTransactionID != 0 && txtJournalNoSelected.Text != "")
                {
                    string filterDt = string.Format("GLTransactionID IN (SELECT GLTransactionID FROM GLTransactionHd WHERE JournalNo = '{0}') AND IsDeleted = 0", txtJournalNoSelected.Text);
                    List<GLTransactionDt> lstGLDT = BusinessLayer.GetGLTransactionDtList(filterDt, ctx);
                    foreach(GLTransactionDt gldt in lstGLDT)
                    {
                        GLTransactionDt glTransactionDt = new GLTransactionDt();
                        glTransactionDt.GLTransactionID = GLTransactionID;
                        glTransactionDt.GLAccount = gldt.GLAccount;
                        glTransactionDt.SubLedger = gldt.SubLedger;
                        glTransactionDt.HealthcareID = gldt.HealthcareID;
                        glTransactionDt.DepartmentID = gldt.DepartmentID;
                        glTransactionDt.ServiceUnitID = gldt.ServiceUnitID;
                        glTransactionDt.RevenueCostCenterID = gldt.RevenueCostCenterID;
                        glTransactionDt.BusinessPartnerID = gldt.BusinessPartnerID;
                        glTransactionDt.CustomerGroupID = gldt.CustomerGroupID;

                        if (cboCopyType.Value.ToString() == "1")
                        {
                            if (gldt.Position == "D")
                            {
                                glTransactionDt.Position = "K";
                            }
                            else
                            {
                                glTransactionDt.Position = "D";
                            }

                            glTransactionDt.DebitAmount = gldt.CreditAmount;
                            glTransactionDt.CreditAmount = gldt.DebitAmount;
                        }
                        else
                        {
                            glTransactionDt.Position = gldt.Position;
                            glTransactionDt.DebitAmount = gldt.DebitAmount;
                            glTransactionDt.CreditAmount = gldt.CreditAmount;
                        }

                        glTransactionDt.Remarks = "Salin dari jurnal nomor " + txtJournalNoSelected.Text + " || " + gldt.Remarks;
                        glTransactionDt.DisplayOrder = gldt.DisplayOrder;
                        glTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        glTransactionDt.GLTransactionIDSource = gldt.GLTransactionID;
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