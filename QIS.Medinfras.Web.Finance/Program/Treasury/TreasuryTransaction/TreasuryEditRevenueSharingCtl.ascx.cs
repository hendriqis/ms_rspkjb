using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TreasuryEditRevenueSharingCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] filter = param.Split('|');
            hdnGLTransactionIDedit.Value = filter[0];
            hdnTransactionDtIDedit.Value = filter[1];

            string filterBinding = string.Format("GLTransactionID = {0} AND TransactionDtID = {1}", hdnGLTransactionIDedit.Value, hdnTransactionDtIDedit.Value);
            vGLTransactionDt entity = BusinessLayer.GetvGLTransactionDtList(filterBinding).FirstOrDefault();

            EntityToControl(entity);
            IsAdd = false;
        }

        protected override void OnControlEntrySetting()
        {
        }

        private void EntityToControl(vGLTransactionDt entity)
        {
            txtGLAccountNo.Text = string.Format("{0} {1}", entity.GLAccountNo, entity.GLAccountName);
            txtSegment.Text = entity.cfSegmentNo;
            txtSupplierPaymentNo.Text = entity.ReferenceNo;
            txtDebitAmount.Text = entity.DebitAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtCreditAmount.Text = entity.CreditAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(GLTransactionDt entity)
        {
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao entityHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                GLTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnTransactionDtIDedit.Value));
                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    GLTransactionHd entityHd = entityHdDao.Get(entityDt.GLTransactionID);
                    if (entityHd.TransactionCode == "7281" || entityHd.TransactionCode == "7282" || entityHd.TransactionCode == "7283" || entityHd.TransactionCode == "7284" || entityHd.TransactionCode == "7285" || entityHd.TransactionCode == "7286" || entityHd.TransactionCode == "7287" || entityHd.TransactionCode == "7288" || entityHd.TransactionCode == "7299")
                    {
                        TransactionTypeLock entityLock = entityLockDao.Get(entityHd.TransactionCode);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = entityHd.JournalDate;
                            if (DateNow > DateLock)
                            {
                                ControlToEntity(entityDt);
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                                retval = entityDt.TransactionDtID.ToString();
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
                        else
                        {
                            ControlToEntity(entityDt);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                            retval = entityDt.TransactionDtID.ToString();
                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        retval = entityDt.TransactionDtID.ToString();
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    errMessage = "Data tidak dapat diubah karena sudah diproses.";
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = false;
                errMessage = ex.Message;
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