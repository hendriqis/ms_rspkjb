using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CreditCardFeeFractionEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnCreditCardID.Value = param;
            vCreditCard entity = BusinessLayer.GetvCreditCardList(String.Format("CreditCardID = {0}", Convert.ToInt32(hdnCreditCardID.Value))).FirstOrDefault();
            txtEDCMachineCode.Text = entity.EDCMachineCode;
            txtEDCMachineName.Text = entity.EDCMachineName;
            hdnDefaultDateNow.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEffectiveDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);
            txtEffectiveDate.Attributes.Add("validationgroup", "mpEntryPopup");
            txtSurchargeFee.Attributes.Add("validationgroup", "mpEntryPopup");
            txtMDRFee.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnCreditCardID.Value != "")
            {
                filterExpression = string.Format("CreditCardID = {0}", hdnCreditCardID.Value);
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvEDCMachineDetailRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vEDCMachineDetail> lstEntity = BusinessLayer.GetvEDCMachineDetailList(filterExpression, 8, pageIndex, "CreditCardID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";

            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(EDCCardFee entity)
        {
            entity.CreditCardID = Convert.ToInt32(hdnCreditCardID.Value);
            entity.EffectiveDate = Helper.GetDatePickerValue(txtEffectiveDate);
            entity.SurchargeFee = Convert.ToDecimal(txtSurchargeFee.Text);
            entity.MDRFee = Convert.ToDecimal(txtMDRFee.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                EDCCardFeeDao entityDao = new EDCCardFeeDao();
                EDCCardFee entity = new EDCCardFee();
                ControlToEntity(entity);                
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                EDCCardFee entity = BusinessLayer.GetEDCCardFee(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateEDCCardFee(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                EDCCardFee entity = BusinessLayer.GetEDCCardFee(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateEDCCardFee(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}