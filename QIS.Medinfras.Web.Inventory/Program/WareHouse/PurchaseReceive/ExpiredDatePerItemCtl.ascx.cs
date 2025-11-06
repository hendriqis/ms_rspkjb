using System;
using System.Collections.Generic;
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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ExpiredDatePerItemCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;
            
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID,
                            Constant.SettingParameter.IM_USE_MAX_EXPIRED_DATE,
                            Constant.SettingParameter.IM_MAX_EXPIRED_DATE);
            List<SettingParameterDt> oParamList = BusinessLayer.GetSettingParameterDtList(filterExpression);

            hdnIsUseMaxExpiredDate.Value = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_USE_MAX_EXPIRED_DATE).FirstOrDefault().ParameterValue;
            hdnMaxExpiredDate.Value = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_MAX_EXPIRED_DATE).FirstOrDefault().ParameterValue;

            BindGridView();
            Helper.SetControlEntrySetting(txtBatchNumber, new ControlEntrySetting(true, true, true), "mpTrxCtlPopup");
            Helper.SetControlEntrySetting(txtExpiredDate, new ControlEntrySetting(true, true, true), "mpTrxCtlPopup");
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxCtlPopup");
        }

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            //if (grdView.Rows.Count < 1)
            //    BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
                filterExpression = string.Format("ID = {0}", hdnID.Value);
            
            List<PurchaseReceiveDtExpired> lstEntity = BusinessLayer.GetPurchaseReceiveDtExpiredList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }       

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "" && hdnBatchNumber.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnSaveDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    vPurchaseOrderDt entity = e.Row.DataItem as vPurchaseOrderDt;
            //    TextBox txtReceivedItem = e.Row.FindControl("txtReceivedItem") as TextBox;
            //    TextBox txtUnitPrice = e.Row.FindControl("txtUnitPrice") as TextBox;
            //    txtReceivedItem.Text = (entity.Quantity - entity.ReceivedQuantity).ToString();
            //    txtUnitPrice.Text = entity.UnitPrice.ToString();
            //}
        }

        protected bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveDtExpiredDao entityDao = new PurchaseReceiveDtExpiredDao(ctx);
            try
            {
                PurchaseReceiveDtExpired entity = new PurchaseReceiveDtExpired();
                if (hdnIsUseMaxExpiredDate.Value == "1")
                {
                    if (Helper.GetDatePickerValue(hdnMaxExpiredDate.Value) >= Helper.GetDatePickerValue(txtExpiredDate.Text))
                    {
                        entity.ID = Convert.ToInt32(hdnID.Value);
                        entity.BatchNumber = txtBatchNumber.Text;
                        entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
                        entity.Quantity = Convert.ToInt32(txtQuantity.Text);
                        entityDao.Insert(entity);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tanggal kadaluarsa melebihi batas.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    entity.ID = Convert.ToInt32(hdnID.Value);
                    entity.BatchNumber = txtBatchNumber.Text;
                    entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
                    entity.Quantity = Convert.ToInt32(txtQuantity.Text);
                    entityDao.Insert(entity);
                    ctx.CommitTransaction();
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

        protected bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveDtExpiredDao entityDao = new PurchaseReceiveDtExpiredDao(ctx);
            try
            {
                PurchaseReceiveDtExpired entity = BusinessLayer.GetPurchaseReceiveDtExpired(Convert.ToInt32(hdnID.Value), Request.Form[txtBatchNumber.UniqueID]);

                if (hdnIsUseMaxExpiredDate.Value == "1")
                {
                    if (Helper.GetDatePickerValue(hdnMaxExpiredDate.Value) >= Helper.GetDatePickerValue(txtExpiredDate.Text))
                    {
                        entity.Quantity = Convert.ToInt32(txtQuantity.Text);
                        entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tanggal kadaluarsa melebihi batas.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    entity.Quantity = Convert.ToInt32(txtQuantity.Text);
                    entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
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

        protected bool OnSaveDeleteRecord(ref string errMessage) 
        {
            bool result = true;
            try
            {
                BusinessLayer.DeletePurchaseReceiveDtExpired(Convert.ToInt32(hdnID.Value), Request.Form[txtBatchNumber.UniqueID]);
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }
    }
}