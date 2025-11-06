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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationEditBusinessPartnerCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            hdnRegistrationIDCtl.Value = param;

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CUSTOMER_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboRegistrationPayerCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCareCtl, lstClassCare, "ClassName", "ClassID");
            cboControlClassCareCtl.SelectedIndex = 0;

            string filterReg = string.Format("RegistrationID = {0}", param);
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(filterReg).FirstOrDefault();
            txtRegistrationNoCtl.Text = entityReg.RegistrationNo;
            txtMRNCtl.Text = entityReg.MedicalNo;
            hdnMRNCtl.Value = entityReg.MRN.ToString();
            txtPatientNameCtl.Text = entityReg.PatientName;
            hdnClassIDCtl.Value = entityReg.ClassID.ToString();
            hdnChargeClassIDCtl.Value = entityReg.ChargeClassID.ToString();
            hdnDepartmentIDCtl.Value = entityReg.DepartmentID;
            hdnGCCustomerTypeCtl.Value = entityReg.GCCustomerType;
            cboRegistrationPayerCtl.Value = entityReg.GCCustomerType;
            hdnParamedicID.Value = entityReg.ParamedicID.ToString();

            hdnPayerIDCtl.Value = entityReg.BusinessPartnerID.ToString();
            txtPayerCompanyCodeCtl.Text = entityReg.BusinessPartnerCode;
            txtPayerCompanyNameCtl.Text = entityReg.BusinessPartnerName;
            hdnContractIDCtl.Value = entityReg.ContractID.ToString();
            txtContractNoCtl.Text = entityReg.ContractNo;
            hdnCoverageTypeIDCtl.Value = entityReg.CoverageTypeID.ToString();
            txtCoverageTypeCodeCtl.Text = entityReg.CoverageTypeCode;
            txtCoverageTypeNameCtl.Text = entityReg.CoverageTypeName;
            hdnEmployeeIDCtl.Value = entityReg.EmployeeID.ToString();
            txtEmployeeCodeCtl.Text = entityReg.EmployeeCode;
            txtEmployeeNameCtl.Text = entityReg.EmployeeName;
            txtParticipantNoCtl.Text = entityReg.CorporateAccountNo;
            txtCoverageLimitCtl.Text = entityReg.CoverageLimitAmount.ToString(Constant.FormatString.NUMERIC_2);
            chkIsCoverageLimitPerDayCtl.Checked = entityReg.IsCoverageLimitPerDay;

            if (!entityReg.IsControlCoverageLimit)
            {
                trCoverageLimitCtl.Attributes.Add("style", "display:none");
                trCoverageLimitPerDayCtl.Attributes.Add("style", "display:none");
            }
            else if (entityReg.DepartmentID != Constant.Facility.INPATIENT)
            {
                trCoverageLimitPerDayCtl.Attributes.Add("style", "display:none");
            }

            if (!entityReg.IsControlClassCare)
            {
                trControlClassCareCtl.Attributes.Add("style", "display:none");
            }
            else
            {
                cboControlClassCareCtl.Value = entityReg.ControlClassID.ToString();
            }

            if (entityReg.GCCustomerType != Constant.CustomerType.HEALTHCARE)
            {
                trEmployeeCtl.Style.Add("display", "none");
            }


            tblPayerCompanyCtl.Style.Remove("display");
            chkUsingCOBCtl.Attributes.Remove("style");
            if (cboRegistrationPayerCtl.Value.ToString() == Constant.CustomerType.PERSONAL)
            {
                tblPayerCompanyCtl.Style.Add("display", "none");
                chkUsingCOBCtl.Attributes.Add("style", "display:none");
            }

            hdnGCTariffSchemePersonalCtl.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetCustomerTypePersonal()
        {
            return Constant.CustomerType.PERSONAL;
        }

        protected string GetCustomerTypeHealthcare()
        {
            return Constant.CustomerType.HEALTHCARE;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPatientNameCtl, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMRNCtl, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationNoCtl, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(cboRegistrationPayerCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnPayerIDCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPayerCompanyCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPayerCompanyNameCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboControlClassCareCtl, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnContractIDCtl, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtContractNoCtl, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnCoverageTypeIDCtl, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCoverageTypeCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCoverageTypeNameCtl, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnEmployeeIDCtl, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEmployeeCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEmployeeNameCtl, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtParticipantNoCtl, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);

            try
            {
                AuditLog entityAuditLog = new AuditLog();
                Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("IsMainVisit = 1 AND RegistrationID = {0}", hdnRegistrationIDCtl.Value), ctx).FirstOrDefault();

                if (entity.ChargesAmount == 0 || entity.PaymentAmount == 0)
                {
                    if (entity.IsHasAIOPackage && cboRegistrationPayerCtl.Value.ToString() != Constant.CustomerType.PERSONAL)
                    {
                        result = false;
                        errMessage = "Maaf, registrasi " + entity.RegistrationNo + " ini tidak dapat diubah penjamin bayar karena memiliki pengisian paket all-in-one AIO.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);
                        RegistrationPayer entityPayer = null;

                        entity.GCCustomerType = cboRegistrationPayerCtl.Value.ToString();
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                        entityPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationIDCtl.Value), ctx).FirstOrDefault();
                        bool isEntityPayerNull = false;

                        if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entity.BusinessPartnerID = 1;
                            entity.ContractID = null;
                            entity.CoverageTypeID = null;
                            entity.CoverageLimitAmount = 0;
                            entity.IsCoverageLimitPerDay = false;
                            entity.GCTariffScheme = hdnGCTariffSchemePersonalCtl.Value;
                            entity.IsControlClassCare = false;
                            entity.ControlClassID = null;
                            entity.EmployeeID = null;

                            if (entityPayer != null)
                            {
                                entity.IsUsingCOB = false;
                                entityPayer.IsDeleted = true;
                                entityPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                registrationPayerDao.Update(entityPayer);
                            }
                        }
                        else
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entity.BusinessPartnerID = entity.GCCustomerType != Constant.CustomerType.PERSONAL ? Convert.ToInt32(hdnPayerIDCtl.Value) : 1;
                            entity.ContractID = Convert.ToInt32(hdnContractIDCtl.Value);
                            entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeIDCtl.Value);
                            entity.CorporateAccountNo = txtParticipantNoCtl.Text;
                            entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimitCtl.Text);
                            entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDayCtl.Checked;
                            entity.GCTariffScheme = hdnGCTariffSchemeCtl.Value;
                            entity.IsControlClassCare = (hdnIsControlClassCareCtl.Value == "1");

                            if (entity.IsControlClassCare)
                            {
                                entity.ControlClassID = Convert.ToInt32(cboControlClassCareCtl.Value);
                            }
                            else
                            {
                                entity.ControlClassID = null;
                            }

                            if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                            {
                                entity.IsBPJS = true;
                                entity.BPJSAmount = entity.CoverageLimitAmount;
                            }

                            if (hdnEmployeeIDCtl.Value == "" || hdnEmployeeIDCtl.Value == "0")
                            {
                                entity.EmployeeID = null;
                            }
                            else
                            {
                                entity.EmployeeID = Convert.ToInt32(hdnEmployeeIDCtl.Value);
                            }

                            if (entityPayer == null)
                            {
                                entityPayer = new RegistrationPayer();
                                isEntityPayerNull = true;
                            }

                            entityPayer.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);
                            entityPayer.BusinessPartnerID = entity.BusinessPartnerID;
                            entityPayer.IsPrimaryPayer = true;
                            entityPayer.GCCustomerType = entity.GCCustomerType;
                            entityPayer.ContractID = Convert.ToInt32(entity.ContractID);
                            entityPayer.CoverageTypeID = Convert.ToInt32(entity.CoverageTypeID);
                            entityPayer.CorporateAccountNo = entity.CorporateAccountNo;
                            entityPayer.CoverageLimitAmount = entity.CoverageLimitAmount;
                            entityPayer.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;

                            if (isEntityPayerNull)
                            {
                                //entity.IsUsingCOB = true;
                                entityPayer.CreatedBy = AppSession.UserLogin.UserID;
                                registrationPayerDao.Insert(entityPayer);
                            }
                            else
                            {
                                entityPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                registrationPayerDao.Update(entityPayer);
                            }
                        }

                        entity.CorporateAccountNo = txtParticipantNoCtl.Text;

                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            entity.BPJSAmount = entity.CoverageLimitAmount;

                            Patient oPatient = null;
                            if (entity.MRN != null && entity.MRN != 0)
                            {
                                oPatient = patientDao.Get(Convert.ToInt32(entity.MRN));
                            }

                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entity.RegistrationID);
                            if (regBPJS == null)
                            {
                                regBPJS = new RegistrationBPJS();
                                regBPJS.RegistrationID = entity.RegistrationID;
                                if (oPatient != null)
                                {
                                    regBPJS.NoPeserta = oPatient.NHSRegistrationNo != null && oPatient.NHSRegistrationNo != "" ? oPatient.NHSRegistrationNo : "0";
                                    regBPJS.NamaPeserta = oPatient.NamaPesertaBPJS;

                                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entityVisit.HealthcareServiceUnitID), ctx).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(hsu.BPJSPoli))
                                    {
                                        regBPJS.KodePoliklinik = hsu.BPJSPoli.Split('|')[0];
                                        regBPJS.NamaPoliklinik = hsu.BPJSPoli;
                                    }
                                    ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityVisit.ParamedicID), ctx).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(pm.BPJSReferenceInfo))
                                    {
                                        regBPJS.KodeDPJP = pm.BPJSReferenceInfo.Split(';')[1].Split('|')[0];
                                    }
                                }
                                else
                                {
                                    regBPJS.NoPeserta = "0";
                                    regBPJS.NamaPeserta = "";
                                }
                                regBPJS.TanggalRujukan = entity.RegistrationDate;
                                regBPJS.JenisPelayanan = entity.TransactionCode == Constant.TransactionCode.IP_REGISTRATION ? "1" : "2";
                                regBPJS.CreatedBy = AppSession.UserLogin.UserID;
                                registrationBPJSDao.Insert(regBPJS);
                            }
                        }

                        registrationDao.Update(entity);
                        retval = entity.RegistrationNo;

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
                        if (entity.AppointmentID > 0)
                        {
                            Appointment oApm = BusinessLayer.GetAppointmentList(string.Format("AppointmentID='{0}'", entity.AppointmentID), ctx).FirstOrDefault();
                            if (oApm != null)
                            {
                                oApm.BusinessPartnerID = entity.BusinessPartnerID;
                                oApm.ContractID = entity.ContractID;
                                oApm.CoverageTypeID = entity.CoverageTypeID;
                                oApm.GCCustomerType = entity.GCCustomerType;
                                oApm.BusinessPartnerID = entity.BusinessPartnerID;
                                oApm.ContractID = entity.ContractID;
                                oApm.CoverageTypeID = entity.CoverageTypeID;
                                oApm.CorporateAccountNo = entity.CorporateAccountNo;
                                oApm.CoverageLimitAmount = entity.CoverageLimitAmount;
                                oApm.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                                oApm.GCTariffScheme = entity.GCTariffScheme;
                                oApm.IsControlClassCare = entity.IsControlClassCare;
                                oApm.ControlClassID = entity.ControlClassID;
                                oApm.EmployeeID = entity.EmployeeID;
                                appointmentDao.Update(oApm);

                            }
                        }


                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, Registrasi " + entity.RegistrationNo + " ini tidak dapat diubah penjamin bayar karena sudah memiliki transaksi / pembayaran";
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