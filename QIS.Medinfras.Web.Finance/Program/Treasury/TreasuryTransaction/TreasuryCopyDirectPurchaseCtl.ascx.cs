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
    public partial class TreasuryCopyDirectPurchaseCtl : BaseEntryPopupCtl
    {
        private TreasuryTransactionEntry DetailPage
        {
            get { return (TreasuryTransactionEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');
            hdnGLTransactionIDctl.Value = paramList[0];
            hdnGLAccountTreasuryIDctl.Value = paramList[1];
            hdnTreasuryTypectl.Value = paramList[2];
            hdnDepartmentIDCtl.Value = paramList[3];
            hdnServiceUnitIDCtl.Value = paramList[4];
            hdnBusinessPartnerIDCtl.Value = paramList[5];
            hdnCashFlowTypeIDCtl.Value = paramList[6];
            hdnCOADirectPurchaseCtl.Value = paramList[7];

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
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND GLTransactionID IS NULL AND GLTransactionDtID IS NULL", Constant.TransactionStatus.APPROVED);

            List<vCopyDirectPurchaseHd> lstEntity = BusinessLayer.GetvCopyDirectPurchaseHdList(filterExpression, int.MaxValue, 1, "PurchaseDate, DirectPurchaseNo");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
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
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);

            int count = 0;
            List<String> lstSelectedDirectPurchaseID = hdnSelectedDirectPurchaseID.Value.Split(',').ToList();
            List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split(',').ToList();
            lstSelectedDirectPurchaseID.RemoveAt(0);
            lstSelectedRemarks.RemoveAt(0);

            string filterExpression = string.Format("DirectPurchaseID IN ({0})", hdnSelectedDirectPurchaseID.Value.Substring(1));
            List<vDirectPurchaseHd> lstHd = BusinessLayer.GetvDirectPurchaseHdList(filterExpression, ctx);

            GLTransactionHd treasuryHd = glTransactionHdDao.Get(Convert.ToInt32(hdnGLTransactionIDctl.Value));

            foreach (vDirectPurchaseHd entity in lstHd)
            {
                DirectPurchaseHd directPurchaseHd = directPurchaseHdDao.Get(entity.DirectPurchaseID);

                BusinessPartners bp = businessPartnersDao.Get(directPurchaseHd.BusinessPartnerID);

                GLTransactionDt entityDt = new GLTransactionDt();
                entityDt.ReferenceNo = directPurchaseHd.DirectPurchaseNo;
                entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;

                entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeIDCtl.Value);

                if (directPurchaseHd.BusinessPartnerID != null && directPurchaseHd.BusinessPartnerID != 0)
                {
                    entityDt.BusinessPartnerID = directPurchaseHd.BusinessPartnerID;
                }
                else
                {
                    if (hdnBusinessPartnerIDCtl.Value != "" && hdnBusinessPartnerIDCtl.Value != null)
                    {
                        entityDt.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCtl.Value);
                    }
                }

                entityDt.DepartmentID = hdnDepartmentIDCtl.Value;
                if (hdnServiceUnitIDCtl.Value != "" && hdnServiceUnitIDCtl.Value != null)
                {
                    entityDt.ServiceUnitID = Convert.ToInt32(hdnServiceUnitIDCtl.Value);
                }

                entityDt.GLAccount = hdnCOADirectPurchaseCtl.Value != null && hdnCOADirectPurchaseCtl.Value != "" ? Convert.ToInt32(hdnCOADirectPurchaseCtl.Value) : 0;

                if (hdnTreasuryTypectl.Value == Constant.TreasuryType.PENERIMAAN)
                {
                    entityDt.Position = "K";
                    entityDt.DebitAmount = 0;
                    entityDt.CreditAmount = entity.cfTotalDirectPurchase;
                }
                else
                {
                    entityDt.Position = "D";
                    entityDt.DebitAmount = entity.cfTotalDirectPurchase;
                    entityDt.CreditAmount = 0;
                }

                int displayOrder = Convert.ToInt16(hdnDisplayOrderTemp.Value) + count + 1;
                entityDt.DisplayOrder = Convert.ToInt16(displayOrder);

                string remarks = lstSelectedRemarks[count];
                entityDt.Remarks = string.Format("{0}|{1}|{2}|{3}",
                                                    directPurchaseHd.DirectPurchaseNo,
                                                    entity.BusinessPartnerName,
                                                    directPurchaseHd.PurchaseDate.ToString(Constant.FormatString.DATE_FORMAT),
                                                    remarks);
                entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                entityDt.IsReferenceNoGeneratedBySystem = true;

                entityDt.GLTransactionID = treasuryHd.GLTransactionID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                int glDtID = glTransactionDtDao.InsertReturnPrimaryKeyID(entityDt);

                directPurchaseHd.GLTransactionID = treasuryHd.GLTransactionID;
                directPurchaseHd.GLTransactionDtID = glDtID;
                directPurchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                directPurchaseHdDao.Update(directPurchaseHd);
                
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