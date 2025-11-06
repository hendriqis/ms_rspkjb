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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EstimatedChargesQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrx DetailPage
        {
            get { return (BasePageTrx)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            //Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            hdnParam.Value = param;
            string[] paramSplit = param.Split('|');
            hdnTransactionIDCtl.Value = paramSplit[0];
            hdnCustomerTypeCtl.Value = paramSplit[1];
            hdnBusinessPartnerIDCtl.Value = paramSplit[2];
            hdnRegistrationIDCtl.Value = paramSplit[3];
            hdnCoverageTypeIDCtl.Value = paramSplit[5];
            hdnTransactionDateCtl.Value = paramSplit[6];
            hdnHealthcareServiceUnitIDCtl.Value = paramSplit[7];
            hdnTransactionNoCtl.Value = paramSplit[8];

            string filterClass = string.Format("IsDeleted = 0 AND ClassID = '{0}'", paramSplit[4]);
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(filterClass);
            Methods.SetComboBoxField(cboServiceChargeClassIDCtl, lstClassCare, "ClassName", "ClassID");
            cboServiceChargeClassIDCtl.SelectedIndex = 0;
            cboServiceChargeClassIDCtl.Enabled = false;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE, Constant.StandardCode.CUSTOMER_TYPE));
            Methods.SetComboBoxField(cboItemType, lstSC.Where(p => p.ParentID == Constant.StandardCode.ITEM_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboItemType.SelectedIndex = 0;
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnItemGroupID.Value == "")
                filterExpression += string.Format("GCItemType = '{0}' AND (ItemName1 LIKE '%{1}%' OR ItemCode LIKE '%{1}%') AND IsDeleted = 0 AND GCItemStatus != '{2}'", cboItemType.Value, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);
            else
                filterExpression += string.Format("GCItemType = '{0}' AND (ItemName1 LIKE '%{1}%' OR ItemCode LIKE '%{1}%') AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0 AND GCItemStatus != '{3}'", cboItemType.Value, hdnFilterItem.Value, hdnItemGroupID.Value, Constant.ItemStatus.IN_ACTIVE);
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ItemMaster entity = e.Row.DataItem as ItemMaster;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnTransactionIDCtl.Value != "0" && hdnTransactionIDCtl.Value != "")
            {
                List<EstimatedChargesDt> lstItemID = BusinessLayer.GetEstimatedChargesDtList(string.Format("EstimatedChargesHdID = {0} AND IsDeleted = 0", hdnTransactionIDCtl.Value));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (EstimatedChargesDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetItemMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<ItemMaster> lstEntity = BusinessLayer.GetItemMasterList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
            EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberPrice = hdnSelectedMemberPrice.Value.Split(',');
                string[] lstSelectedMemberPriceComp1 = hdnSelectedMemberPriceComp1.Value.Split(',');
                string[] lstSelectedMemberPriceComp2 = hdnSelectedMemberPriceComp2.Value.Split(',');
                string[] lstSelectedMemberPriceComp3 = hdnSelectedMemberPriceComp3.Value.Split(',');
                string[] lstSelectedMemberPatientAmount = hdnSelectedMemberPatientAmount.Value.Split(',');
                string[] lstSelectedMemberPayerAmount = hdnSelectedMemberPayerAmount.Value.Split(',');
                string[] lstSelectedMemberLineAmount = hdnSelectedMemberLineAmount.Value.Split(',');

                if (!String.IsNullOrEmpty(hdnTransactionIDCtl.Value) && hdnTransactionIDCtl.Value != "0")
                {
                    int ct = 0;
                    retval = hdnTransactionNoCtl.Value;
                    foreach (String itemID in lstSelectedMember)
                    {
                        EstimatedChargesDt entityDt = new EstimatedChargesDt();
                        entityDt.EstimatedChargesHdID = Convert.ToInt32(hdnTransactionIDCtl.Value);
                        entityDt.ItemID = Convert.ToInt32(itemID);
                        entityDt.Qty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.Tariff = Convert.ToDecimal(lstSelectedMemberPrice[ct]);
                        entityDt.TariffComp1 = Convert.ToDecimal(lstSelectedMemberPriceComp1[ct]);
                        entityDt.TariffComp2 = Convert.ToDecimal(lstSelectedMemberPriceComp2[ct]);
                        entityDt.TariffComp3 = Convert.ToDecimal(lstSelectedMemberPriceComp3[ct]);
                        entityDt.PatientAmount = Convert.ToDecimal(lstSelectedMemberPatientAmount[ct]);
                        entityDt.PayerAmount = Convert.ToDecimal(lstSelectedMemberPayerAmount[ct]);
                        entityDt.LineAmount = Convert.ToDecimal(lstSelectedMemberLineAmount[ct]);
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                        ct++;
                    }
                }
                else
                {
                    EstimatedChargesHd entity = new EstimatedChargesHd();
                    entity.TransactionDate = DateTime.Now;
                    entity.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ESTIMATED_CHARGES, entity.TransactionDate, ctx);
                    entity.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);
                    entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value);
                    entity.GCCustomerType = hdnCustomerTypeCtl.Value;
                    entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCtl.Value);

                    if (!String.IsNullOrEmpty(hdnCoverageTypeIDCtl.Value))
                    {
                        entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeIDCtl.Value);
                    }
                    
                    entity.ClassID = Convert.ToInt32(cboServiceChargeClassIDCtl.Value);
                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int id = entityHdDao.InsertReturnPrimaryKeyID(entity);
                    retval = entity.TransactionNo;

                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        EstimatedChargesDt entityDt = new EstimatedChargesDt();
                        entityDt.EstimatedChargesHdID = id;
                        entityDt.ItemID = Convert.ToInt32(itemID);
                        entityDt.Qty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.Tariff = Convert.ToDecimal(lstSelectedMemberPrice[ct]);
                        entityDt.TariffComp1 = Convert.ToDecimal(lstSelectedMemberPriceComp1[ct]);
                        entityDt.TariffComp2 = Convert.ToDecimal(lstSelectedMemberPriceComp2[ct]);
                        entityDt.TariffComp3 = Convert.ToDecimal(lstSelectedMemberPriceComp3[ct]);
                        entityDt.PatientAmount = Convert.ToDecimal(lstSelectedMemberPatientAmount[ct]);
                        entityDt.PayerAmount = Convert.ToDecimal(lstSelectedMemberPayerAmount[ct]);
                        entityDt.LineAmount = Convert.ToDecimal(lstSelectedMemberLineAmount[ct]);
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                        ct++;
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
    }
}