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
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryRecalculationBillUpdatePayerCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnRegistrationID.Value = param.Split('|')[0];
                hdnDepartmentID.Value = param.Split('|')[1];
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];

                SetControlProperties();

                EntityToControl(entity);
                
                trPayerCompany.Style.Remove("display");
                trCoverageLimit.Style.Remove("display");
                trCoverageLimitPerDay.Style.Remove("display");
                trCoverageType.Style.Remove("display");
                trParticipant.Style.Remove("display");
                trPayerContract.Style.Remove("display");
                trGuaranteeLetterExists.Style.Remove("display");

                if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                {
                    trPayerCompany.Style.Add("display", "none");
                    trCoverageType.Style.Add("display", "none");
                    trParticipant.Style.Add("display", "none");
                    trPayerContract.Style.Add("display", "none");
                    trCoverageLimit.Style.Add("display", "none");
                    trCoverageLimitPerDay.Style.Add("display", "none");
                    trGuaranteeLetterExists.Style.Add("display", "none");
                }
                if (entity.GCCustomerType != Constant.CustomerType.HEALTHCARE)
                    trEmployee.Style.Add("display", "none");

                if (!entity.IsControlClassCare)
                    trControlClassCare.Style.Add("display", "none");

                if (!entity.IsControlCoverageLimit)
                {
                    trCoverageLimit.Style.Add("display", "none");
                }
                else
                {
                    if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        trCoverageLimitPerDay.Style.Add("display", "none");
                    }
                    else
                    {
                        if (!entity.IsControlCoverageLimit || entity.DepartmentID != Constant.Facility.INPATIENT)
                            trCoverageLimitPerDay.Style.Add("display", "none");
                    }
                }

                hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;
            }
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CUSTOMER_TYPE));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPayer, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<ClassCare> lstClassCare1 = BusinessLayer.GetClassCareList("IsDeleted = 0  AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare1, "ClassName", "ClassID");
            cboControlClassCare.Value = hdnChargeClassID.Value;
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
            SetControlEntrySetting(cboControlClassCare, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnContractID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtContractNo, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnCoverageTypeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnEmployeeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEmployeeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEmployeeName, new ControlEntrySetting(false, false, true));

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
            hdnContractID.Value = entity.ContractID.ToString();
            txtContractNo.Text = entity.ContractNo;
            hdnMRN.Value = entity.MRN.ToString();
            txtPatientName.Text = entity.PatientName;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtMRN.Text = entity.MedicalNo;
            txtCoverageLimit.Text = entity.CoverageLimitAmount.ToString();
            chkIsCoverageLimitPerDay.Checked = entity.IsCoverageLimitPerDay;
            hdnContractID.Value = entity.ContractID.ToString();
            hdnCoverageTypeID.Value = entity.CoverageTypeID.ToString();
            hdnEmployeeID.Value = entity.EmployeeID.ToString();
            txtEmployeeCode.Text = entity.EmployeeCode;
            txtEmployeeName.Text = entity.EmployeeName;

            hdnChargeClassID.Value = entity.ChargeClassID.ToString();
            txtChargeClass.Text = entity.ChargeClassName;

            chkIsGuaranteeLetterExists.Checked = entity.IsGuaranteeLetterExists;

            hdnIsControlClassCare.Value = entity.IsControlClassCare ? "1" : "0";
            if (entity.IsControlClassCare)
                cboControlClassCare.Value = entity.ControlClassID.ToString();

            if (entity.GCCustomerType != Constant.CustomerType.PERSONAL && hdnContractID.Value != "0")
            {
                string filterExpression = string.Format("BusinessPartnerID = {0}{1}", hdnPayerID.Value, GetCustomerContractFilterExpression());
                vCustomerContract obj = BusinessLayer.GetvCustomerContractList(filterExpression).FirstOrDefault();
                if (obj != null)
                {
                    hdnContractCoverageCount.Value = obj.ContractCoverageCount.ToString();
                    filterExpression = string.Format("ContractID = {0} AND MRN = {1}", hdnContractID.Value, hdnMRN.Value);
                    hdnContractCoverageMemberCount.Value = BusinessLayer.GetContractCoverageMemberRowCount(filterExpression).ToString();
                }
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            try
            {
                string oldPayer = "", newPayer = "", isChangePayer = "0";

                AuditLog entityAuditLog = new AuditLog();
                Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                
                oldPayer = entity.BusinessPartnerID.ToString();
                entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);
                RegistrationPayer entityPayer = null;

                string payerType = cboPayer.Value.ToString();

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<PatientPaymentHd> lstPayment = BusinessLayer.GetPatientPaymentHdList(string.Format(
                                                        "RegistrationID = '{0}' AND GCTransactionStatus != '{1}' AND GCPaymentType != '{2}'",
                                                        hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT), ctx);

                if (lstPayment.Count() == 0)
                {
                    if (entity.IsHasAIOPackage && payerType != Constant.CustomerType.PERSONAL)
                    {
                        result = false;
                        errMessage = "Maaf, registrasi " + entity.RegistrationNo + " ini tidak dapat diubah penjamin bayar karena memiliki pengisian paket all-in-one AIO.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        entity.GCCustomerType = payerType;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                        if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entity.BusinessPartnerID = 1;
                            entity.ContractID = null;
                            entity.CoverageTypeID = null;
                            entity.CoverageLimitAmount = 0;
                            entity.IsCoverageLimitPerDay = false;
                            entity.GCTariffScheme = hdnGCTariffSchemePersonal.Value;
                            entity.IsControlClassCare = false;
                            entity.ControlClassID = null;
                            entity.EmployeeID = null;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<RegistrationPayer> lstRegPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value), ctx);
                            if (lstRegPayer.Count() > 0)
                            {
                                entity.IsUsingCOB = false;
                                foreach (RegistrationPayer regPayer in lstRegPayer)
                                {
                                    regPayer.IsDeleted = true;
                                    regPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    registrationPayerDao.Update(regPayer);
                                }
                            }
                        }
                        else
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entity.BusinessPartnerID = entity.GCCustomerType != Constant.CustomerType.PERSONAL ? Convert.ToInt32(hdnPayerID.Value) : 1;
                            entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                            entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                            entity.CorporateAccountNo = txtParticipantNo.Text;
                            entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                            entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                            entity.GCTariffScheme = hdnGCTariffScheme.Value;
                            entity.IsControlClassCare = (hdnIsControlClassCare.Value == "1");
                            if (entity.IsControlClassCare)
                            {
                                entity.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                            }
                            else
                            {
                                entity.ControlClassID = null;
                            }
                            if (hdnEmployeeID.Value == "" || hdnEmployeeID.Value == "0")
                            {
                                entity.EmployeeID = null;
                            }
                            else
                            {
                                entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value), ctx).FirstOrDefault();

                            if (entityPayer == null)
                            {
                                entityPayer = new RegistrationPayer();
                                entityPayer.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                entityPayer.BusinessPartnerID = entity.BusinessPartnerID;
                                entityPayer.IsPrimaryPayer = true;
                                entityPayer.GCCustomerType = entity.GCCustomerType;
                                entityPayer.ContractID = Convert.ToInt32(entity.ContractID);
                                entityPayer.CoverageTypeID = Convert.ToInt32(entity.CoverageTypeID);
                                entityPayer.CorporateAccountNo = entity.CorporateAccountNo;
                                entityPayer.CoverageLimitAmount = entity.CoverageLimitAmount;
                                entityPayer.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                                entityPayer.IsGuaranteeLetterExists = chkIsGuaranteeLetterExists.Checked;
                                //entity.IsUsingCOB = true; // ditutup oleh RN 20190613 karena perubahan data penjamin di file ini, bukan sbg COB tapi sbg mainpayer
                                entityPayer.CreatedBy = AppSession.UserLogin.UserID;
                                registrationPayerDao.Insert(entityPayer);
                            }
                            else
                            {
                                entityPayer.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                entityPayer.BusinessPartnerID = entity.BusinessPartnerID;
                                entityPayer.IsPrimaryPayer = true;
                                entityPayer.GCCustomerType = entity.GCCustomerType;
                                entityPayer.ContractID = Convert.ToInt32(entity.ContractID);
                                entityPayer.CoverageTypeID = Convert.ToInt32(entity.CoverageTypeID);
                                entityPayer.CorporateAccountNo = entity.CorporateAccountNo;
                                entityPayer.CoverageLimitAmount = entity.CoverageLimitAmount;
                                entityPayer.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                                entityPayer.IsGuaranteeLetterExists = chkIsGuaranteeLetterExists.Checked;
                                entityPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                registrationPayerDao.Update(entityPayer);
                            }
                        }
                        entity.CorporateAccountNo = txtParticipantNo.Text;

                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            entity.IsBPJS = true;
                            entity.BPJSAmount = entity.CoverageLimitAmount;

                            Patient oPatient = null;
                            if (entity.MRN != null && entity.MRN != 0)
                            {
                                oPatient = patientDao.Get(Convert.ToInt32(entity.MRN));
                            }

                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entity.RegistrationID);
                            ParamedicMaster oParamediMaster = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID='{0}'", AppSession.RegisteredPatient.ParamedicID)).FirstOrDefault();
                            string KodeDPJP = string.Empty;
                            if (oParamediMaster != null)
                            {
                                if (!String.IsNullOrEmpty(oParamediMaster.BPJSReferenceInfo))
                                {
                                    string[] bpjsInfo = oParamediMaster.BPJSReferenceInfo.Split(';');
                                    string[] hfisInfo = bpjsInfo[1].Split('|');
                                    KodeDPJP = hfisInfo[0];
                                }
                            }
                            if (regBPJS == null)
                            {
                                regBPJS = new RegistrationBPJS();
                                regBPJS.RegistrationID = entity.RegistrationID;
                                if (oPatient != null)
                                {
                                    regBPJS.NoPeserta = oPatient.NHSRegistrationNo != null && oPatient.NHSRegistrationNo != "" ? oPatient.NHSRegistrationNo : "0";
                                    regBPJS.NamaPeserta = oPatient.NamaPesertaBPJS;
                                }
                                else
                                {
                                    regBPJS.NoPeserta = "0";
                                    regBPJS.NamaPeserta = "";
                                }
                                regBPJS.KodeDPJP = KodeDPJP; 
                                regBPJS.TanggalRujukan = entity.RegistrationDate;
                                regBPJS.JenisPelayanan = entity.TransactionCode == Constant.TransactionCode.IP_REGISTRATION ? "1" : "2";
                                if (string.IsNullOrEmpty(regBPJS.NoSuratKontrol))
                                {
                                    regBPJS.NoSuratKontrol = BusinessLayer.GenerateNoSuratKontrolBPJS(entity.RegistrationDate, ctx);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                }
                                regBPJS.CreatedBy = AppSession.UserLogin.UserID;
                                registrationBPJSDao.Insert(regBPJS);
                            }
                            else
                            {
                                RegistrationBPJS RegBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);
                                if (RegBPJS != null)
                                {
                                    RegBPJS.KodeDPJP = KodeDPJP;
                                   
                                    if (oPatient != null)
                                    {
                                        RegBPJS.NoPeserta = oPatient.NHSRegistrationNo != null && oPatient.NHSRegistrationNo != "" ? oPatient.NHSRegistrationNo : "0";
                                        RegBPJS.NamaPeserta = oPatient.NamaPesertaBPJS;
                                    }
                                    else
                                    {
                                        RegBPJS.NoPeserta = "0";
                                        RegBPJS.NamaPeserta = "";
                                    }
                                    registrationBPJSDao.Update(RegBPJS);
                                }
                            }
                        }

                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationDao.Update(entity);
                        newPayer = entity.BusinessPartnerID.ToString();

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                        entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                        entityAuditLog.LogDate = DateTime.Now;
                        entityAuditLog.TransactionID = entity.RegistrationID;
                        entityAuditLogDao.Insert(entityAuditLog);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}','{2}')", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                        foreach (PatientChargesHd entityHd in lstPatientChargesHd)
                        {
                            entityHd.IsPendingRecalculated = true;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entityHd);
                        }

                        if (newPayer != oldPayer)
                        {
                            isChangePayer = "1";
                        }

                        if (isChangePayer == "1")
                        {
                            if (AppSession.IsBridgingToQueue)
                            {
                                //If Bridging to Queue - Send Information
                                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.LABORATORY || hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                                {
                                    try
                                    {
                                        //VisitInfo visitInfo = new VisitInfo();
                                        //visitInfo = ConvertVisitToDTO(entityVisit);
                                        //PatientData patientInfo = ConvertPatientToDTO(entityPatient);
                                        APIMessageLog entityAPILog = new APIMessageLog()
                                        {
                                            MessageDateTime = DateTime.Now,
                                            Recipient = Constant.BridgingVendor.QUEUE,
                                            Sender = Constant.BridgingVendor.HIS,
                                            IsSuccess = true
                                        };

                                        QueueService oService = new QueueService();
                                        string apiResult = oService.BAR_P01(AppSession.UserLogin.HealthcareID, entity);
                                        string[] apiResultInfo = apiResult.Split('|');
                                        if (apiResultInfo[0] == "0")
                                        {
                                            entityAPILog.IsSuccess = false;
                                            entityAPILog.MessageText = apiResultInfo[1];
                                            entityAPILog.Response = apiResultInfo[1];
                                            Exception ex = new Exception(apiResultInfo[1]);
                                            Helper.InsertErrorLog(ex);
                                        }
                                        else
                                        {
                                            entityAPILog.MessageText = apiResultInfo[1];
                                        }

                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    }
                                    catch (Exception ex)
                                    {
                                        Helper.InsertErrorLog(ex);
                                    }
                                }
                            }
                        }
                        retval = isChangePayer;
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, registrasi " + entity.RegistrationNo + " tidak dapat mengubah data pembayar karena sudah memiliki Pembayaran.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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
            return result;
        }
    }
}