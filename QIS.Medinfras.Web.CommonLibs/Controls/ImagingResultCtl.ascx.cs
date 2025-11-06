using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ImagingResultCtl : BaseViewPopupCtl
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
                    List<ImagingResultHd> lstHd = BusinessLayer.GetImagingResultHdList(string.Format("ChargeTransactionID = {0}", hdnTransactionID.Value));
                    if (lstHd.Count > 0)
                    {
                        ImagingResultHd imagingHd = lstHd.FirstOrDefault();
                        hdnID.Value = imagingHd.ID.ToString();
                        EntityToControl(oChargesHd, imagingHd);
                    }
                }
            }
        }

        private void EntityToControl(vPatientChargesHd chargesHd, ImagingResultHd entity)
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
            List<vImagingResultDt> lstEntity = BusinessLayer.GetvImagingResultDtList(string.Format("ID = {0}", hdnID.Value));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}