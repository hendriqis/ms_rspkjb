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

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class LaboratoryTestResultEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnTransactionID.Value = param;
                string filterExpression = string.Format("TransactionID = {0}",hdnTransactionID.Value);
                vPatientChargesHd oChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oChargesHd != null)
                {
                    TestOrderHd testOrderHd = BusinessLayer.GetTestOrderHd(oChargesHd.TestOrderID);
                    List<vLaboratoryResultDt> lstLaboratoryHd = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0}", hdnTransactionID.Value));
                    if (lstLaboratoryHd.Count > 0)
                    {
                        vLaboratoryResultDt laboratoryHd = lstLaboratoryHd.FirstOrDefault();
                        hdnID.Value = laboratoryHd.ID.ToString();
                        EntityToControl(oChargesHd,laboratoryHd);
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