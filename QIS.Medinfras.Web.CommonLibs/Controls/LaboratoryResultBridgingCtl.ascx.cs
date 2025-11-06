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
    public partial class LaboratoryResultBridgingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnTransactionID.Value = param;
                string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
                vPatientChargesHd oChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oChargesHd != null)
                {
                    //                    hdnTransactionID.Value = oChargesHd.TransactionID.ToString();
                    TestOrderHd testOrderHd = BusinessLayer.GetTestOrderHd(oChargesHd.TestOrderID);
                    List<LaboratoryResultHd> lstLaboratoryHd = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", hdnTransactionID.Value));
                    if (lstLaboratoryHd.Count > 0)
                    {
                        LaboratoryResultHd laboratoryHd = lstLaboratoryHd.FirstOrDefault();
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

        private void EntityToControl(vPatientChargesHd chargesHd, LaboratoryResultHd entity)
        {
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            txtOrderNumber.Text = chargesHd.TestOrderNo;
            txtTestOrderDate.Text = chargesHd.TestOrderDateInString;
            txtTestOrderTime.Text = chargesHd.TestOrderTime;

            if (!String.IsNullOrEmpty(chargesHd.TestOrderInfo))
            {
                txtOrderedBy.Text = chargesHd.TestOrderInfo.Split(';')[1];
            }
            else
            {
                txtOrderedBy.Text = "";
            }

            List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ID = {0} ORDER BY  FractionDisplaySequence", hdnID.Value));

            grdView.DataSource = lstLabEntity;
            grdView.DataBind();
        }

    }
}