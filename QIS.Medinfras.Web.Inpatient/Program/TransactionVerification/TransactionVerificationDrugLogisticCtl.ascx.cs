using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class TransactionVerificationDrugLogisticCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt> lstChargesDt)
        {
            //lstSelectedMember = hdnSelectedServiceMember.Value.Split(',');

            lvwDrugLogistic.DataSource = lstChargesDt;
            lvwDrugLogistic.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwDrugLogistic.FindControl("tdDrugMSTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwDrugLogistic.FindControl("tdDrugMSTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwDrugLogistic.FindControl("tdDrugMSTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N");
            }
        }

        protected void lvwDrugLogistic_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientChargesDt entity = e.Item.DataItem as vPatientChargesDt;
                CheckBox chkIsSelectedDrugLogistic = e.Item.FindControl("chkIsSelectedDrugLogistic") as CheckBox;
                chkIsSelectedDrugLogistic.Checked = entity.IsVerified;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void VerifyProcess(IDbContext ctx)
        {
            PatientChargesDtDao patientDtDao = new PatientChargesDtDao(ctx);
            foreach (ListViewDataItem item in lvwDrugLogistic.Items)
            {
                CheckBox chkIsSelected = (CheckBox)item.FindControl("chkIsSelectedDrugLogistic");
                HtmlInputHidden hdnID = (HtmlInputHidden)item.FindControl("keyFieldDrugLogistic");
                string filterExpression = String.Format("ID = {0} AND IsDeleted = 0", hdnID.Value);
                List<PatientChargesDt> lstpatientDt = BusinessLayer.GetPatientChargesDtList(filterExpression, ctx);

                foreach (PatientChargesDt patientDt in lstpatientDt)
                {
                    if (chkIsSelected.Checked)
                    {
                        patientDt.IsVerified = true;
                        patientDt.VerifiedBy = AppSession.UserLogin.UserID;
                        patientDt.VerifiedDate = DateTime.Now;
                        patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDt.LastUpdatedDate = DateTime.Now;
                        patientDtDao.Update(patientDt);
                    }
                    else
                    {
                        patientDt.IsVerified = false;
                        patientDt.VerifiedBy = null;
                        patientDt.VerifiedDate = new DateTime(1900, 1, 1);
                        patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDt.LastUpdatedDate = DateTime.Now;
                        patientDtDao.Update(patientDt);
                    }

                }
            }
        }
    }
}