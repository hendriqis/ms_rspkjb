using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillDiscountEntryCtl : BaseEntryPopupCtl
    {        
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                SetControlProperties();
                hdnPatientBillingID.Value = param;
                vPatientBill entity = BusinessLayer.GetvPatientBillList(string.Format("PatientBillingID = {0}", param))[0];
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DISCOUNT_REASON));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDiscountReason, lstSc, "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(vPatientBill entity)
        {
            cboDiscountReason.Value = entity.GCDiscountReason;

            txtTotalPatientAmount.Text = entity.TotalPatientAmount.ToString();
            txtPatientDiscountPercentage.Text = entity.PatientDiscountInPercentage.ToString();
            txtPatientDiscountAmount.Text = entity.PatientDiscountAmount.ToString();
            txtTotalPatient.Text = entity.TotalPatient.ToString();

            txtTotalPayerAmount.Text = entity.TotalPayerAmount.ToString();
            txtPayerDiscountPercentage.Text = entity.PayerDiscountInPercentage.ToString();
            txtPayerDiscountAmount.Text = entity.PayerDiscountAmount.ToString();
            txtTotalPayer.Text = entity.TotalPayer.ToString();
        }

        private void ControlToEntity(PatientBill entity)
        {
            if (cboDiscountReason.Value != null && cboDiscountReason.Value.ToString() != "")
                entity.GCDiscountReason = cboDiscountReason.Value.ToString();
            else
                entity.GCDiscountReason = null;
            entity.PatientDiscountAmount = Convert.ToDecimal(txtPatientDiscountAmount.Text);
            entity.PayerDiscountAmount = Convert.ToDecimal(txtPayerDiscountAmount.Text);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PatientBill entity = BusinessLayer.GetPatientBill(Convert.ToInt32(hdnPatientBillingID.Value));

                string oldBillStatus = entity.GCTransactionStatus;

                ControlToEntity(entity);

                if (oldBillStatus != entity.GCTransactionStatus)
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                }

                BusinessLayer.UpdatePatientBill(entity);
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