using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class LaboratoryResultCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string filterExpressionDt = "";
            if (param != "")
            {
                List<SettingParameter> setvar = BusinessLayer.GetSettingParameterList(String.Format("ParameterCode IN ('{0}')",
                        Constant.SettingParameter.LB_IS_PREVIEW_RESULT_AFTER_PROPOSED_RESULT
                        ));

                hdnIsShowResultAfterProposed.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_IS_PREVIEW_RESULT_AFTER_PROPOSED_RESULT).FirstOrDefault().ParameterValue;
                hdnTransactionID.Value = param;
                string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
                vPatientChargesHd oChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oChargesHd != null)
                {
                    //hdnTransactionID.Value = oChargesHd.TransactionID.ToString();
                    TestOrderHd testOrderHd = BusinessLayer.GetTestOrderHd(oChargesHd.TestOrderID);
                    if (hdnIsShowResultAfterProposed.Value == "1")
                    {
                        filterExpressionDt = string.Format("ChargeTransactionID = {0} AND ResultGCTransactionStatus NOT IN ('{1}','{2}')", hdnTransactionID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
                    }
                    else
                    {
                        filterExpressionDt = string.Format("ChargeTransactionID = {0}", hdnTransactionID.Value);
                    }

                    List<vLaboratoryResultDt> lstLaboratoryHd = BusinessLayer.GetvLaboratoryResultDtList(filterExpressionDt);
                    if (lstLaboratoryHd.Count > 0)
                    {
                        vLaboratoryResultDt laboratoryHd = lstLaboratoryHd.FirstOrDefault();
                        hdnID.Value = laboratoryHd.ID.ToString();
                        EntityToControl(oChargesHd, laboratoryHd);
                    }

                    if (testOrderHd != null)
                    {
                        txtRemarksHd.Text = testOrderHd.Remarks;
                    }
                    else
                    {
                        txtRemarksHd.Text = oChargesHd.Remarks;
                    }
                }
            }
        }

        private void EntityToControl(vPatientChargesHd chargesHd, vLaboratoryResultDt entity)
        {
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            txtOrderNumber.Text = chargesHd.TestOrderNo;
            txtTestOrderDate.Text = chargesHd.TestOrderDateInString;
            txtTestOrderTime.Text = chargesHd.TestOrderTime;

            if (!String.IsNullOrEmpty(chargesHd.TestOrderInfo))
            {
                txtOrderedBy.Text = chargesHd.TestOrderInfo.Split(';')[3];
            }
            else
            {
                txtOrderedBy.Text = "";
            }

            List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ID = {0} ORDER BY ItemDisplayOrder,FractionDisplayOrder", hdnID.Value));

            grdView.DataSource = lstLabEntity;
            grdView.DataBind();
        }
    }
}