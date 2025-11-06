using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingAdjustmentEntry : BasePageTrx
    {
        List<StandardCode> lstFormulaType = null;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_ADJUSTMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string OnGetRevenueSharingFilterExpression()
        {
            return string.Format("ParamedicID = {0} AND GCTransactionStatus = '{1}'",AppSession.ParamedicID,Constant.TransactionStatus.OPEN);
        }

        protected string OnGetRevenueSharingGroupSc() 
        {
            return Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtProcessedDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP, Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE));
            Methods.SetRadioButtonListField(rblAdjustment, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField(cboAdjustmentTypeAdd, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboAdjustmentTypeMin, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "2").ToList(), "StandardCodeName", "StandardCodeID");

            rblAdjustment.SelectedValue = OnGetRevenueSharingGroupSc();

            Helper.SetControlEntrySetting(hdnRSTransactionID, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnRevenueSharingNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRevenueSharingNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRevenueSharingNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboAdjustmentTypeAdd, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboAdjustmentTypeMin, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtAdjustmentAmount, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(chkRevenueSharingFee, new ControlEntrySetting(false, false, false), "mpEntryPopup");

            BindGridDetail();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            int RSTransactionID ;

            RSTransactionID = hdnRSTransactionID.Value != "" ? Convert.ToInt32(hdnRSTransactionID.Value) : 0;

            filterExpression = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSTransactionID = {1} AND GCRSAdjustmentGroup = '{2}'", AppSession.ParamedicID, RSTransactionID, rblAdjustment.SelectedValue);

            List<vTransRevenueSharingAdj> lstEntity = BusinessLayer.GetvTransRevenueSharingAdjList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void ControlToEntity(TransRevenueSharingAdj entity) 
        {
            entity.RSTransactionID = Convert.ToInt32(hdnRSTransactionID.Value);
            entity.GCRSAdjustmentGroup = rblAdjustment.SelectedValue;
            if (rblAdjustment.SelectedValue == OnGetRevenueSharingGroupSc())
            {
                entity.GCRSAdjustmentType = cboAdjustmentTypeAdd.Value.ToString();
            }
            else 
            {
                entity.GCRSAdjustmentType = cboAdjustmentTypeMin.Value.ToString();
            }
            entity.AdjustmentAmount = Convert.ToDecimal(txtAdjustmentAmount.Text); 
            entity.Remarks = txtRemarks.Text.ToString();
            entity.IsTaxed = chkRevenueSharingFee.Checked;
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
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
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            //BindGridDetail();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                TransRevenueSharingAdj entity = new TransRevenueSharingAdj();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertTransRevenueSharingAdj(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                TransRevenueSharingAdj entity = BusinessLayer.GetTransRevenueSharingAdj(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTransRevenueSharingAdj(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                TransRevenueSharingAdj entity = BusinessLayer.GetTransRevenueSharingAdj(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTransRevenueSharingAdj(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            try
            {
                int RSTransactionID = Convert.ToInt32(hdnRSTransactionID.Value);

                retval = BusinessLayer.GenerateRevenueSharingAdjustment(
                                            RSTransactionID,
                                            AppSession.UserLogin.UserID
                                        );
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}