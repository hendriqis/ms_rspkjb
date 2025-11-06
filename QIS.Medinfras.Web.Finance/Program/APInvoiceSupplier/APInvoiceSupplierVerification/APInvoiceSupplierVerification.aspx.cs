using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierVerification : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_INVOICE_SUPPLIER_VERIFICATION;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Diverifikasi" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diverifikasi" });
            lstVariable.Add(new Variable { Code = "3", Value = "Belum Bayar" });
            lstVariable.Add(new Variable { Code = "4", Value = "Sudah Bayar" });
            lstVariable.Add(new Variable { Code = "5", Value = "Sudah Verifikasi dan Belum Bayar" });
            lstVariable.Add(new Variable { Code = "6", Value = "Sudah Verifikasi dan Sudah Bayar" });
            Methods.SetComboBoxField<Variable>(cboStatus, lstVariable, "Value", "Code");
            cboStatus.Value = "1";

            txtDueDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            Helper.SetControlEntrySetting(txtDueDate, new ControlEntrySetting(true, true, false), "mpInvoiceList");

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("DueDate <= '{0}' AND GCTransactionStatus NOT IN ('{1}', '{2}')", Helper.GetDatePickerValue(txtDueDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN);

            string status = cboStatus.Value.ToString();
            switch (status)
            {
                case "1": filterExpression += string.Format(" AND IsVerified = 0"); break;
                case "2": filterExpression += string.Format(" AND IsVerified = 1"); break;
                case "3": filterExpression += string.Format(" AND NumberOfPayment = 0"); break;
                case "4": filterExpression += string.Format(" AND NumberOfPayment > 0"); break;
                case "5": filterExpression += string.Format(" AND NumberOfPayment = 0 AND IsVerified = 1"); break;
                case "6": filterExpression += string.Format(" AND NumberOfPayment > 0 AND IsVerified = 1"); break;
            }
            filterExpression += string.Format(" AND BusinessPartnerID = {0}", AppSession.BusinessPartnerID);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseInvoiceHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPurchaseInvoiceHd> lstEntity = BusinessLayer.GetvPurchaseInvoiceHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseInvoiceHd entity = e.Item.DataItem as vPurchaseInvoiceHd;
                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember != null)
                {
                    if (lstSelectedMember.Contains(entity.PurchaseInvoiceID.ToString()))
                        chkIsSelected.Checked = true;
                    else
                        chkIsSelected.Checked = false;
                }
                CheckBox chkVerified = e.Item.FindControl("chkVerified") as CheckBox;
                chkVerified.Checked = entity.IsVerified;
                CheckBox chkBayar = e.Item.FindControl("chkBayar") as CheckBox;
                if (entity.NumberOfPayment > 0)
                {
                    chkBayar.Checked = true;
                    chkIsSelected.Visible = false;
                }
                else
                    chkBayar.Checked = false;
            }
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "process")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseInvoiceHdDao pinvoiceDao = new PurchaseInvoiceHdDao(ctx);
                List<PurchaseInvoiceHd> lstPurchaseInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(String.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx); 
                try
                {
                    foreach (PurchaseInvoiceHd entity in lstPurchaseInvoiceHd) 
                    {
                        if (!entity.IsVerified)
                        {
                            entity.IsVerified = true;
                            entity.VerifiedBy = AppSession.UserLogin.UserID;
                            entity.VerifiedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.LastUpdatedDate = DateTime.Now;
                            pinvoiceDao.Update(entity);
                        }
                    }
                    
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else if(type == "decline")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseInvoiceHdDao pinvoiceDao = new PurchaseInvoiceHdDao(ctx);
                List<PurchaseInvoiceHd> lstPurchaseInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(String.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx); 
                try
                {
                    List<PurchaseInvoiceHd> lstPurchaseInvoiceHdProcessed = lstPurchaseInvoiceHd.Where(p => p.NumberOfPayment > 0).ToList();
                    if (lstPurchaseInvoiceHdProcessed.Count < 1)
                    {
                        foreach (PurchaseInvoiceHd entity in lstPurchaseInvoiceHd)
                        {
                            entity.IsVerified = false;
                            entity.VerifiedBy = null;
                            entity.VerifiedDate = new DateTime(1900, 1, 1);
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.LastUpdatedDate = DateTime.Now;
                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            pinvoiceDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = string.Format("No {0} Sudah Dibayar. Tidak Bisa Dibatalkan", String.Join(",", lstPurchaseInvoiceHdProcessed.Select(p => String.Format("<b>{0}</b>", p.PurchaseInvoiceNo))));
                        result = false;
                    }                    
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }

            return result;
        }
    }
}