using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseOrderTermEntryCtl : BaseViewPopupCtl
    {
        protected bool IsEditable = true;

        public override void InitializeDataControl(string param)
        {
            hdnPurchaseOrderIDCtl.Value = param;

            string filterPO = string.Format("PurchaseOrderID = {0}", Convert.ToInt32(hdnPurchaseOrderIDCtl.Value));
            vPurchaseOrderHd orderHd = BusinessLayer.GetvPurchaseOrderHdList(filterPO).FirstOrDefault();
            txtPurchaseOrderNoCtl.Text = orderHd.PurchaseOrderNo;
            txtSupplierCtl.Text = string.Format("{0} ({1})", orderHd.BusinessPartnerName, orderHd.BusinessPartnerCode);
            txtTotalOrderSaldoCtl.Text = orderHd.cfTransactionAmountOnTermInString;

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'  AND IsDeleted = 0", Constant.StandardCode.PPH_TYPE));
            listStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPPHType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PPH_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboPPHType.SelectedIndex = 0;

            List<Variable> lstFilterBy = new List<Variable>();
            lstFilterBy.Add(new Variable { Code = "0", Value = "" });
            lstFilterBy.Add(new Variable { Code = "1", Value = "Non Void" });
            lstFilterBy.Add(new Variable { Code = "2", Value = "Approved" });
            lstFilterBy.Add(new Variable { Code = "3", Value = "Open" });
            lstFilterBy.Add(new Variable { Code = "4", Value = "Void" });
            Methods.SetComboBoxField<Variable>(cboFilterBy, lstFilterBy, "Value", "Code");
            cboFilterBy.Value = "1";

            BindGridView();

        }

        #region Void Entity
        private bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);

            try
            {
                string filterPOTerm = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus = '{1}'", hdnPurchaseOrderIDCtl.Value, Constant.TransactionStatus.OPEN);
                List<PurchaseOrderTerm> lstTerm = BusinessLayer.GetPurchaseOrderTermList(filterPOTerm, ctx);
                foreach (PurchaseOrderTerm poTerm in lstTerm)
                {
                    poTerm.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    poTerm.GCVoidReason = Constant.DeleteReason.OTHER;
                    poTerm.VoidReason = "VOID from button Void All";
                    poTerm.VoidDate = DateTime.Now;
                    poTerm.VoidBy = AppSession.UserLogin.UserID;
                    poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTermDao.Update(poTerm);
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
        #endregion

        #region Approved Entity
        private bool OnApprovedRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);

            try
            {
                string filterPOTerm = string.Format("PurchaseOrderID = {0}", hdnPurchaseOrderIDCtl.Value);
                List<PurchaseOrderTerm> lstTerm = BusinessLayer.GetPurchaseOrderTermList(filterPOTerm, ctx);
                foreach (PurchaseOrderTerm poTerm in lstTerm)
                {
                    if (poTerm.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        poTerm.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityTermDao.Update(poTerm);
                    }
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
        #endregion

        #region Process
        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int poID = Convert.ToInt32(hdnPurchaseOrderIDCtl.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref poID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage, poID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "reopen")
            {
                if (OnReopenEntityDt(ref errMessage, poID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "approved")
            {
                if (OnApprovedRecord(ref errMessage))
                {
                    result += "success";
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += "success";
                }
                else result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPOID"] = poID.ToString();
        }

        private void ControlToEntity(PurchaseOrderTerm entityTerm)
        {
            entityTerm.TermRemarks = txtTermRemarks.Text;
            entityTerm.TermConditions = txtTermCondition.Text;
            entityTerm.TermInPercentage = hdnIsTermInPercentage.Value == "1" ? true : false;
            entityTerm.TermPercentage = Convert.ToDecimal(hdnTermPercentage.Value);
            entityTerm.TermAmount = Convert.ToDecimal(hdnTermAmount.Value);
            entityTerm.IsPurchaseReceiveRequired = hdnIsPurchaseReceiveRequired.Value == "1" ? true : false;
            entityTerm.GCPPHType = cboPPHType.Value.ToString();
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int testOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);
            int counter = Convert.ToInt32(txtTermCounter.Text);

            try
            {
                for (int i = 1; i <= counter; i++)
                {
                    PurchaseOrderTerm entityTerm = new PurchaseOrderTerm();
                    entityTerm.PurchaseOrderID = Convert.ToInt32(hdnPurchaseOrderIDCtl.Value);
                    entityTerm.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    ControlToEntity(entityTerm);
                    entityTerm.TermCounter = i;
                    entityTerm.CreatedBy = AppSession.UserLogin.UserID;
                    entityTermDao.Insert(entityTerm);
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);

            try
            {
                PurchaseOrderTerm entityTerm = entityTermDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityTerm.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityTerm);
                    entityTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTermDao.Update(entityTerm);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, detail termin ini sudah diproses. Silahkan refresh popup ulang.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);

            try
            {
                PurchaseOrderTerm entityTerm = entityTermDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityTerm.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityTerm.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityTerm.GCVoidReason = Constant.DeleteReason.OTHER;
                    entityTerm.VoidReason = "VOID from Detail Term";
                    entityTerm.VoidDate = DateTime.Now;
                    entityTerm.VoidBy = AppSession.UserLogin.UserID;
                    entityTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTermDao.Update(entityTerm);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, detail termin ini sudah diproses. Silahkan refresh popup ulang.";
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

        private bool OnReopenEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);

            try
            {
                PurchaseOrderTerm entityTerm = entityTermDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityTerm.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                {
                    entityTerm.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTermDao.Update(entityTerm);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, detail termin ini sudah diproses. Silahkan refresh popup ulang.";
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

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseOrderIDCtl.Value != "")
            {
                filterExpression = string.Format("PurchaseOrderID = {0}", hdnPurchaseOrderIDCtl.Value);
            }

            if (cboFilterBy.Value != null)
            {
                if (cboFilterBy.Value.ToString() == "1")
                {
                    filterExpression += string.Format(" AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);
                }
                else if (cboFilterBy.Value.ToString() == "2")
                {
                    filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.APPROVED);
                }
                else if (cboFilterBy.Value.ToString() == "3")
                {
                    filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.OPEN);
                }
                else if (cboFilterBy.Value.ToString() == "4")
                {
                    filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.VOID);
                }
            }

            List<vPurchaseOrderTerm> lstEntity = BusinessLayer.GetvPurchaseOrderTermList(filterExpression);
            grdViewTerm.DataSource = lstEntity;
            grdViewTerm.DataBind();
        }

        protected void grdViewTerm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseOrderTerm entity = e.Row.DataItem as vPurchaseOrderTerm;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

    }
}