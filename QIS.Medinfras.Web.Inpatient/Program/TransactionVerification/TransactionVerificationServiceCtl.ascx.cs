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
    public partial class TransactionVerificationServiceCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt> lstChargesDt)
        {
            //lstSelectedMember = hdnSelectedServiceMember.Value.Split(',');

            lvwService.DataSource = lstChargesDt;
            lvwService.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void VerifyProcess(IDbContext ctx, String[] lstSelectedMember, String[] lstUnselectedMember, String transactionID)
        {
            PatientChargesHdDao patientHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientDtDao = new PatientChargesDtDao(ctx);
            string filterExpression = String.Format("TransactionID = {0} AND IsDeleted = 0", transactionID);
            List<PatientChargesDt> lstPatientDt = BusinessLayer.GetPatientChargesDtList(filterExpression, ctx);
            foreach (PatientChargesDt patientDt in lstPatientDt)
            {

                if (lstSelectedMember.Contains(patientDt.ID.ToString()) && patientDt.IsVerified == false)
                {
                    patientDt.IsVerified = true;
                    patientDt.VerifiedBy = AppSession.UserLogin.UserID;
                    patientDt.VerifiedDate = DateTime.Now;
                    patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientDt.LastUpdatedDate = DateTime.Now;
                    patientDtDao.Update(patientDt);
                }
                if (lstUnselectedMember.Contains(patientDt.ID.ToString()) && patientDt.IsVerified == true)
                {
                    patientDt.IsVerified = false;
                    patientDt.VerifiedBy = null;
                    patientDt.VerifiedDate = new DateTime(1900, 1, 1);
                    patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientDt.LastUpdatedDate = DateTime.Now;
                    patientDtDao.Update(patientDt);
                }
            }
            PatientChargesHd entityHd = patientHdDao.Get(Convert.ToInt32(transactionID));
            if (lstPatientDt.Where(p => p.IsVerified == false).Count() > 0)
                entityHd.IsVerified = false;
            else
                entityHd.IsVerified = true;
            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
            patientHdDao.Update(entityHd);
        }
    }
}