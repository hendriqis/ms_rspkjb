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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RecalculationPatientBillUpdatePayerCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                SetControlProperties();
                hdnRegistrationID.Value = param.Split('|')[0];
                hdnDepartmentID.Value = param.Split('|')[1];
                hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                EntityToControl(entity);

                trPayerCompany.Style.Remove("display");
                trCoverageLimit.Style.Remove("display");
                trCoverageLimitPerDay.Style.Remove("display");
                trCoverageType.Style.Remove("display");
                trParticipant.Style.Remove("display");
                trPayerContract.Style.Remove("display");
                if (cboPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
                {
                    trPayerCompany.Style.Add("display", "none");
                    trCoverageType.Style.Add("display", "none");
                    trParticipant.Style.Add("display", "none");
                    trPayerContract.Style.Add("display", "none");
                    trCoverageLimit.Style.Add("display", "none");
                    trCoverageLimitPerDay.Style.Add("display", "none");
                }
                if (!entity.IsControlCoverageLimit)
                    trCoverageLimit.Style.Add("display", "none");
                if (!entity.IsControlCoverageLimit || entity.DepartmentID != Constant.Facility.INPATIENT)
                    trCoverageLimitPerDay.Style.Add("display", "none");
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.CUSTOMER_TYPE));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPayer, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(cboPayer, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnPayerID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPayerCompanyCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPayerCompanyName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnContractID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtContractNo, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnCoverageTypeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtParticipantNo, new ControlEntrySetting(true, true, false));
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetCustomerContractFilterExpression()
        {
            return string.Format(" AND '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0", DateTime.Now.ToString("yyyyMMdd"));
        }

        private void EntityToControl(vRegistration entity)
        {
            cboPayer.Value = entity.GCCustomerType;
            hdnPayerID.Value = entity.BusinessPartnerID.ToString();
            txtPayerCompanyCode.Text = entity.BusinessPartnerCode;
            txtPayerCompanyName.Text = entity.BusinessPartnerName;
            txtParticipantNo.Text = entity.CorporateAccountNo;
            txtCoverageTypeCode.Text = entity.CoverageTypeCode;
            txtCoverageTypeName.Text = entity.CoverageTypeName;
            txtContractNo.Text = entity.ContractNo;
            hdnMRN.Value = entity.MRN.ToString();
            txtPatientName.Text = entity.PatientName;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtMRN.Text = entity.MedicalNo;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            try
            {
                Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                entity.GCCustomerType = cboPayer.Value.ToString();
                entity.BusinessPartnerID = entity.GCCustomerType != Constant.CustomerType.PERSONAL ? Convert.ToInt32(hdnPayerID.Value) : 1;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                {
                    entity.BusinessPartnerID = 1;
                    entity.ContractID = null;
                    entity.CoverageTypeID = null;
                    entity.CoverageLimitAmount = 0;
                    entity.IsCoverageLimitPerDay = false;
                    entity.GCTariffScheme = hdnGCTariffSchemePersonal.Value;
                }
                else
                {
                    entity.BusinessPartnerID = entity.GCCustomerType != Constant.CustomerType.PERSONAL ? Convert.ToInt32(hdnPayerID.Value) : 1;
                    entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                    entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                    entity.CorporateAccountNo = txtParticipantNo.Text;
                    entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                    entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                    entity.GCTariffScheme = hdnGCTariffScheme.Value;
                }
                entity.CorporateAccountNo = txtParticipantNo.Text;
                registrationDao.Update(entity);

                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}','{2}')", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                foreach (PatientChargesHd entityHd in lstPatientChargesHd)
                {
                    entityHd.IsPendingRecalculated = true;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}