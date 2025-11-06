using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangeRegistrationPayerDetail : BasePageTrx
    {

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CHANGE_REGISTRATION_PAYER;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnVisitID.Value = Page.Request.QueryString["id"];

                List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                            AppSession.UserLogin.HealthcareID, //0
                                                                            Constant.SettingParameter.FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE, //1
                                                                            Constant.SettingParameter.FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM, //2
                                                                            Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO //3
                                                                        ));

                hdnIsFinalisasiKlaimAfterARInvoice.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE).ParameterValue;
                hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM).ParameterValue;
                hdnIsGrouperAmountClaimDefaultZero.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO).FirstOrDefault().ParameterValue;

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnRegistrationNo.Value = entity.RegistrationNo;
                EntityToControl(entity);

                List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1 AND StandardCodeID != '{1}'",
                                                                                                Constant.StandardCode.CUSTOMER_TYPE, Constant.CustomerType.PERSONAL));
                lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                Methods.SetComboBoxField<StandardCode>(cboPayer, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

                List<ClassCare> lstClassCare1 = BusinessLayer.GetClassCareList("IsDeleted = 0  AND IsUsedInChargeClass = 1");
                Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare1, "ClassName", "ClassID");
                cboControlClassCare.SelectedIndex = 0;

                if (entity.GCCustomerType != Constant.CustomerType.HEALTHCARE)
                {
                    trEmployee.Style.Add("display", "none");
                }

            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WITH(NOLOCK) WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetCustomerContractFilterExpression()
        {
            return string.Format(" AND '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0", DateTime.Now.ToString("yyyyMMdd"));
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);

            hdnMRN.Value = entity.MRN.ToString();

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            hdnGCCustomerTypeOri.Value = entityReg.GCCustomerType;
            txtPayerOri.Text = entityReg.CustomerType;
            hdnPayerIDOri.Value = entityReg.BusinessPartnerID.ToString();
            txtPayerCompanyCodeOri.Text = entityReg.BusinessPartnerCode;
            txtPayerCompanyNameOri.Text = entityReg.BusinessPartnerName;
            txtContractNoOri.Text = entityReg.ContractNo;
            txtCoverageTypeCodeOri.Text = entityReg.CoverageTypeCode;
            txtCoverageTypeNameOri.Text = entityReg.CoverageTypeName;
            txtEmployeeCodeOri.Text = entityReg.EmployeeCode;
            txtEmployeeNameOri.Text = entityReg.EmployeeName;
            txtParticipantNoOri.Text = entityReg.CorporateAccountNo;
            txtChargeClassOri.Text = entityReg.ChargeClassName;
            if (entityReg.ControlClassID != null && entityReg.ControlClassID != 0)
            {
                ClassCare ccc = BusinessLayer.GetClassCare(entityReg.ControlClassID);
                txtControlClassCareOri.Text = ccc.ClassName;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            RegistrationBPJSInfoDao registrationBPJSInfoDao = new RegistrationBPJSInfoDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            PatientPaymentDtDao patientPaymentDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao patientPaymentDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);

            if (type == "changepayer")
            {
                try
                {
                    Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                    RegistrationBPJS entityRegBPJS = registrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationID.Value));

                    string filterRegBPJS = string.Format("RegistrationID = {0}", entity.RegistrationID);
                    List<RegistrationBPJSInfo> entityRegBPJSInfoList = BusinessLayer.GetRegistrationBPJSInfoList(filterRegBPJS, ctx);

                    RegistrationPayer entityPayer = null;
                    if (hdnPayerID.Value != "" && hdnPayerID.Value != "0")
                    {
                        string oldCustType = entity.GCCustomerType;
                        string newCustType = cboPayer.Value.ToString();

                        entity.GCCustomerType = cboPayer.Value.ToString();
                        entity.BusinessPartnerID = entity.GCCustomerType != Constant.CustomerType.PERSONAL ? Convert.ToInt32(hdnPayerID.Value) : 1;
                        entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                        entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                        entity.CorporateAccountNo = txtParticipantNo.Text;
                        entity.GCTariffScheme = hdnGCTariffScheme.Value;
                        entity.IsControlClassCare = (hdnIsControlClassCare.Value == "1");
                        if (entity.IsControlClassCare && cboControlClassCare.Value != null)
                        {
                            entity.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                        }
                        else
                        {
                            entity.ControlClassID = null;
                        }
                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            entity.BPJSAmount = entity.CoverageLimitAmount;
                        }
                        if (hdnEmployeeID.Value == "" || hdnEmployeeID.Value == "0")
                        {
                            entity.EmployeeID = null;
                        }
                        else
                        {
                            entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);
                        }
                        entity.CorporateAccountNo = txtParticipantNo.Text;

                        entityPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value), ctx).FirstOrDefault();
                        bool isEntityPayerNull = false;
                        if (entityPayer == null)
                        {
                            entityPayer = new RegistrationPayer();
                            isEntityPayerNull = true;
                        }
                        entityPayer.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        entityPayer.BusinessPartnerID = entity.BusinessPartnerID;
                        entityPayer.IsPrimaryPayer = true;
                        entityPayer.GCCustomerType = entity.GCCustomerType;
                        entityPayer.ContractID = Convert.ToInt32(entity.ContractID);
                        entityPayer.CoverageTypeID = Convert.ToInt32(entity.CoverageTypeID);
                        entityPayer.CorporateAccountNo = entity.CorporateAccountNo;
                        entityPayer.CoverageLimitAmount = entity.CoverageLimitAmount;
                        entityPayer.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                        entityPayer.ControlClassID = entity.ControlClassID;
                        if (isEntityPayerNull)
                        {
                            entityPayer.CreatedBy = AppSession.UserLogin.UserID;
                            registrationPayerDao.Insert(entityPayer);
                        }
                        else
                        {
                            entityPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationPayerDao.Update(entityPayer);
                        }

                        string filterPayment = string.Format("IsDeleted = 0 AND PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WITH(NOLOCK) WHERE RegistrationID = {0} AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}' )",
                                                    entity.RegistrationID, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER);
                        List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(filterPayment, ctx);
                        foreach (PatientPaymentDt paymentDtCheck in lstPaymentDt)
                        {
                            vCustomerLineDt entityCustPayment = BusinessLayer.GetvCustomerLineDtList(string.Format("CustomerLineID IN (SELECT CustomerLineID FROM Customer WITH(NOLOCK) WHERE BusinessPartnerID = {0})", paymentDtCheck.BusinessPartnerID)).FirstOrDefault();
                            vCustomerLineDt entityCustReg = BusinessLayer.GetvCustomerLineDtList(string.Format("CustomerLineID IN (SELECT CustomerLineID FROM Customer WITH(NOLOCK) WHERE BusinessPartnerID = {0})", entityPayer.BusinessPartnerID)).FirstOrDefault();

                            if (entityCustPayment.ARInProcess == entityCustReg.ARInProcess)
                            {
                                foreach (PatientPaymentDt paymentDt in lstPaymentDt)
                                {
                                    paymentDt.BusinessPartnerID = entity.BusinessPartnerID;
                                    paymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientPaymentDtDao.Update(paymentDt);

                                    PatientPaymentDtInfo paymentDtInfo = patientPaymentDtInfoDao.Get(paymentDt.PaymentDetailID);
                                    if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                    {
                                        if (paymentDtInfo != null)
                                        {
                                            if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "0" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value == "1")
                                            {
                                                paymentDtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                paymentDtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                                paymentDtInfo.ClaimDate = DateTime.Now;

                                                paymentDtInfo.GCFinalStatus = Constant.FinalStatus.OPEN;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    paymentDtInfo.GrouperAmountClaim = paymentDt.PaymentAmount;
                                                    paymentDtInfo.GrouperAmountFinal = paymentDt.PaymentAmount;
                                                }
                                                else
                                                {
                                                    paymentDtInfo.GrouperAmountClaim = 0;
                                                    paymentDtInfo.GrouperAmountFinal = 0;
                                                }
                                            }
                                            else if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "1" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value == "1")
                                            {
                                                paymentDtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                paymentDtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                                paymentDtInfo.ClaimDate = DateTime.Now;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    paymentDtInfo.GrouperAmountClaim = paymentDt.PaymentAmount;
                                                    paymentDtInfo.GrouperAmountFinal = paymentDt.PaymentAmount;
                                                }
                                                else
                                                {
                                                    paymentDtInfo.GrouperAmountClaim = 0;
                                                    paymentDtInfo.GrouperAmountFinal = 0;
                                                }
                                            }
                                            else
                                            {
                                                paymentDtInfo.GCClaimStatus = Constant.ClaimStatus.OPEN;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    paymentDtInfo.GrouperAmountClaim = paymentDt.PaymentAmount;
                                                    paymentDtInfo.GrouperAmountFinal = paymentDt.PaymentAmount;
                                                }
                                                else
                                                {
                                                    paymentDtInfo.GrouperAmountClaim = 0;
                                                    paymentDtInfo.GrouperAmountFinal = 0;
                                                }
                                            }

                                            patientPaymentDtInfoDao.Update(paymentDtInfo);
                                        }
                                    }
                                    else
                                    {
                                        if (paymentDtInfo != null)
                                        {
                                            if (entityRegBPJSInfoList.Count() > 0)
                                            {
                                                RegistrationBPJSInfo regBpjsInfo = entityRegBPJSInfoList.FirstOrDefault();
                                                regBpjsInfo.IsBPJS = false;
                                                registrationBPJSInfoDao.Update(regBpjsInfo);

                                                entity.IsBPJS = false;
                                            }

                                            paymentDtInfo.GrouperAmountClaim = paymentDt.PaymentAmount;
                                            paymentDtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                            paymentDtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                            paymentDtInfo.ClaimDate = DateTime.Now;
                                            paymentDtInfo.GrouperAmountFinal = paymentDt.PaymentAmount;
                                            paymentDtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                            paymentDtInfo.FinalBy = AppSession.UserLogin.UserID;
                                            paymentDtInfo.FinalDate = DateTime.Now;
                                            patientPaymentDtInfoDao.Update(paymentDtInfo);
                                        }
                                    }
                                }

                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                registrationDao.Update(entity);

                                string filterGLTrans = string.Format("IsDeleted = 0 AND GCItemDetailStatus != '{0}' AND ReferenceNo LIKE '%{1}%' AND BusinessPartnerID != 0 AND GLTransactionID IN (SELECT GLTransactionID FROM GLTransactionHd WITH(NOLOCK) WHERE GCTransactionStatus != '{0}' AND TransactionCode = '7202')",
                                                                    Constant.TransactionStatus.VOID, hdnRegistrationNo.Value);
                                List<GLTransactionDt> lstGLTrans = BusinessLayer.GetGLTransactionDtList(filterGLTrans, ctx);

                                if (lstGLTrans.Count > 0)
                                {
                                    foreach (GLTransactionDt glTransactionDt in lstGLTrans)
                                    {
                                        Customer entityCust = BusinessLayer.GetCustomerList(string.Format("BusinessPartnerID = {0}", entity.BusinessPartnerID)).FirstOrDefault();
                                        glTransactionDt.BusinessPartnerID = entity.BusinessPartnerID;
                                        glTransactionDt.CustomerGroupID = entityCust.CustomerGroupID;
                                        glTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        glTransactionDtDao.Update(glTransactionDt);
                                    }
                                }

                                string newPayer = string.Format("({0}) {1}", txtPayerCompanyCode.Text, Convert.ToString(Request.Form[txtPayerCompanyName.UniqueID]));
                                retval = newPayer;
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = string.Format("Tidak dapat melakukan perubahan Penjamin Bayar, dikarenakan <b>COA Piutang dalam Proses</b> antara Penjamin Awal dengan Penjamin Akhir berbeda.");
                                ctx.RollBackTransaction();
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Pilih PENJAMIN AKHIR terlebih dahulu.";
                        ctx.RollBackTransaction();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }

            return result;
        }
    }
}