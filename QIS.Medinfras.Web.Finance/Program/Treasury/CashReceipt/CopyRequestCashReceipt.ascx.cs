using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CopyRequestCashReceipt : BaseEntryPopupCtl
    {
        private RequestRealizationCashReceiptEntry DetailPage
        {
            get { return (RequestRealizationCashReceiptEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');
            hdnGLTransactionIDctl.Value = paramList[0];
            hdnTreasuryTypectl.Value = paramList[1];
            hdnDepartmentIDCtl.Value = paramList[2];
            hdnServiceUnitIDCtl.Value = paramList[3];
            hdnBusinessPartnerIDCtl.Value = paramList[4];
            hdnCOACashReceiptCtl.Value = paramList[5];

            List<Variable> lstAmountCopy = new List<Variable>();
            lstAmountCopy.Add(new Variable { Code = "X", Value = "Kosongkan Semua" });
            lstAmountCopy.Add(new Variable { Code = "D", Value = "Isi ke Debit" });
            lstAmountCopy.Add(new Variable { Code = "K", Value = "Isi ke Kredit" });
            Methods.SetComboBoxField<Variable>(cboAmountCopy, lstAmountCopy, "Value", "Code");
            cboAmountCopy.Value = "X";

            string filterDisplay = string.Format("GLTransactionID = {0} AND TransactionDtID = (SELECT MAX(TransactionDtID) FROM GLTransactionDt WHERE GLTransactionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}')", hdnGLTransactionIDctl.Value, Constant.TransactionStatus.VOID);
            GLTransactionDt glDT = BusinessLayer.GetGLTransactionDtList(filterDisplay).FirstOrDefault();
            if (glDT != null)
            {
                hdnDisplayOrderTemp.Value = glDT.DisplayOrder.ToString();
            }

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = "BalanceEND != 0";

            List<vGLBalanceDtDocumentPerReferenceNoDetailGLDT> lstEntity = BusinessLayer.GetvGLBalanceDtDocumentPerReferenceNoDetailGLDTList(filterExpression, int.MaxValue, 1, "TransactionDtID DESC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vGLBalanceDtDocumentPerReferenceNoDetailGLDT entity = e.Item.DataItem as vGLBalanceDtDocumentPerReferenceNoDetailGLDT;
                TextBox txtDebitAmount = e.Item.FindControl("txtDebitAmount") as TextBox;
                TextBox txtCreditAmount = e.Item.FindControl("txtCreditAmount") as TextBox;

                if (cboAmountCopy.Value.ToString() == "D")
                {
                    txtDebitAmount.Text = entity.BalanceEND.ToString(Constant.FormatString.NUMERIC_2);
                    txtCreditAmount.Text = "0";
                }
                else if (cboAmountCopy.Value.ToString() == "K")
                {
                    txtDebitAmount.Text = "0";
                    txtCreditAmount.Text = entity.BalanceEND.ToString(Constant.FormatString.NUMERIC_2);
                }
                else
                {
                    txtDebitAmount.Text = "0";
                    txtCreditAmount.Text = "0";
                }
            }
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(IDbContext ctx, List<GLTransactionDt> lstEntity)
        {
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);
            GLTransactionHdDao glTransactionHdDao = new GLTransactionHdDao(ctx);
            BusinessPartnersDao businessPartnersDao = new BusinessPartnersDao(ctx);

            int count = 0;
            List<String> lstSelectedCashReceiptID = hdnSelectedGLTransactionDtID.Value.Split(',').ToList();
            List<String> lstSelectedDebitAmount = hdnSelectedDebitAmount.Value.Split(',').ToList();
            List<String> lstSelectedCreditAmount = hdnSelectedCreditAmount.Value.Split(',').ToList();
            List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split(',').ToList();

            lstSelectedCashReceiptID.RemoveAt(0);
            lstSelectedDebitAmount.RemoveAt(0);
            lstSelectedCreditAmount.RemoveAt(0);
            lstSelectedRemarks.RemoveAt(0);

            string filterExpression = string.Format("TransactionDtID IN ({0})", hdnSelectedGLTransactionDtID.Value.Substring(1));
            List<vGLBalanceDtDocumentPerReferenceNoDetailGLDT> lst = BusinessLayer.GetvGLBalanceDtDocumentPerReferenceNoDetailGLDTList(filterExpression, ctx);

            GLTransactionHd treasuryHd = glTransactionHdDao.Get(Convert.ToInt32(hdnGLTransactionIDctl.Value));

            foreach (vGLBalanceDtDocumentPerReferenceNoDetailGLDT entity in lst)
            {
                GLTransactionDt entityDt = new GLTransactionDt();
                entityDt.ReferenceNo = entity.ReferenceNo;
                entityDt.HealthcareID = entity.HealthcareID;
                entityDt.BusinessPartnerID = entity.BusinessPartnerID;
                entityDt.DepartmentID = entity.DepartmentID;
                entityDt.ServiceUnitID = entity.ServiceUnitID;
                entityDt.RevenueCostCenterID = entity.RevenueCostCenterID;

                entityDt.GLAccount = hdnCOACashReceiptCtl.Value != null && hdnCOACashReceiptCtl.Value != "" ? Convert.ToInt32(hdnCOACashReceiptCtl.Value) : 0;

                decimal debitAmount = lstSelectedDebitAmount[count] != "" ? Convert.ToDecimal(lstSelectedDebitAmount[count]) : 0;
                decimal creditAmount = lstSelectedCreditAmount[count] != "" ? Convert.ToDecimal(lstSelectedCreditAmount[count]) : 0;

                if (debitAmount != 0 && creditAmount == 0)
                {
                    entityDt.Position = "D";
                    entityDt.DebitAmount = debitAmount;
                    entityDt.CreditAmount = 0;
                }
                else
                {
                    entityDt.Position = "K";
                    entityDt.DebitAmount = 0;
                    entityDt.CreditAmount = creditAmount;
                }

                int displayOrder = Convert.ToInt16(hdnDisplayOrderTemp.Value) + count + 1;
                entityDt.DisplayOrder = Convert.ToInt16(displayOrder);

                string remarks = lstSelectedRemarks[count];
                entityDt.Remarks = string.Format("{0}|{1}|{2}|{3}",
                                                    entity.ReferenceNo,
                                                    entity.JournalNo,
                                                    entity.JournalDate.ToString(Constant.FormatString.DATE_FORMAT),
                                                    remarks);
                entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                entityDt.GLTransactionID = treasuryHd.GLTransactionID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                int glDtID = glTransactionDtDao.InsertReturnPrimaryKeyID(entityDt);

                lstEntity.Add(entityDt);

                count++;
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glDtDao = new GLTransactionDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            int GLTransactionID = 0;
            string errorMessage = "";
            try
            {
                DetailPage.SaveGLTransactionHd(ctx, ref GLTransactionID, ref errorMessage);
                GLTransactionHd entityHd = glHdDao.Get(GLTransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityHd.TransactionCode == "7281" || entityHd.TransactionCode == "7282" || entityHd.TransactionCode == "7283" || entityHd.TransactionCode == "7284" || entityHd.TransactionCode == "7285" || entityHd.TransactionCode == "7286" || entityHd.TransactionCode == "7287" || entityHd.TransactionCode == "7288" || entityHd.TransactionCode == "7299")
                    {
                        TransactionTypeLock entityLock = entityLockDao.Get(entityHd.TransactionCode);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = entityHd.JournalDate;
                            if (DateNow > DateLock)
                            {
                                List<GLTransactionDt> lstEntityDt = new List<GLTransactionDt>();

                                ControlToEntity(ctx, lstEntityDt);

                                retval = GLTransactionID.ToString();
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                                result = false;
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                        }
                    }
                    else
                    {
                        List<GLTransactionDt> lstEntityDt = new List<GLTransactionDt>();

                        ControlToEntity(ctx, lstEntityDt);

                        retval = GLTransactionID.ToString();
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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

        #endregion
    }
}