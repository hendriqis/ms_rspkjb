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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PayerDetailEntryCtl : BaseEntryPopupCtl
    {
        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                SetControlProperties();
                hdnRegistrationIDCtlEditPayer.Value = param;
                IsAdd = false;
                OnLoadCtlControl();
            }
        }

        protected override void SetControlProperties()
        {
            //List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID like '%{0}%'", Constant.StandardCode.FAMILY_RELATION));
            //lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            //Methods.SetComboBoxField<StandardCode>(cboRelationship, lstSc, "StandardCodeName", "StandardCodeID");
            //cboRelationship.SelectedIndex = 0;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboCtlControlClassCare, lstClassCare, "ClassName", "ClassID");
            Methods.SetComboBoxField<ClassCare>(cboPayerControlClassCare, lstClassCare, "ClassName", "ClassID");
            cboCtlControlClassCare.SelectedIndex = 0;
            cboPayerControlClassCare.SelectedIndex = 0;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID <> '{1}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CUSTOMER_TYPE, Constant.CustomerType.PERSONAL));
            Methods.SetComboBoxField<StandardCode>(cboCtlRegistrationPayer, lstStandardCode, "StandardCodeName", "StandardCodeID");

            SetControlEntrySetting(txtRegNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBusinessPartnerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBusinessPartnerName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEmployeeCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEmployeeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCorporateAccNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccName, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(cboRelationship, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAmount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPayerControlClassCare, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCoverageLimitPerDay, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsGuaranteeLetterExistsCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTelepon, new ControlEntrySetting(false, false, false));
            Helper.SetControlEntrySetting(cboCtlRegistrationPayer, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtCtlPayerCompanyCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtCtlCoverageTypeCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtCtlContractNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        private void EntityToControl(vRegistration entity)
        {
            txtRegNo.Text = entity.RegistrationNo;
            hdnCtlMRN.Value = entity.MedicalNo;

            BusinessPartners entityBusinessPartner = BusinessLayer.GetBusinessPartners(entity.BusinessPartnerID);
            txtBusinessPartnerCode.Text = entityBusinessPartner.BusinessPartnerCode;
            txtBusinessPartnerName.Text = entityBusinessPartner.BusinessPartnerName;

            if (entity.GCCustomerType != Constant.CustomerType.HEALTHCARE)
                trEmployee.Style.Add("display", "none");
            else
            {
                txtEmployeeCode.Text = entity.EmployeeCode;
                txtEmployeeName.Text = entity.EmployeeName;
            }
            txtEmail.Text = entity.ContactPersonEmail;
            txtTelepon.Text = entity.ContactPersonMobileNo;
            txtCorporateAccNo.Text = entity.CorporateAccountNo;
            txtCorporateAccName.Text = entity.CorporateAccountName;
            txtCoverageTypeCode.Text = entity.CoverageTypeCode;
            txtCoverageTypeName.Text = entity.CoverageTypeName;
            if (entity.ContractID > 0)
            {
                CustomerContract entityCustContract = BusinessLayer.GetCustomerContract(Convert.ToInt32(entity.ContractID));
                txtContractPeriodStart.Text = entityCustContract.StartDateInString;
                txtContractPeriodEnd.Text = entityCustContract.EndDateInString;
                if (entityCustContract.IsControlCoverageLimit == true)
                {
                    hdnIsControlCovLimit.Value = entityCustContract.IsControlCoverageLimit.ToString();
                    txtAmount.Text = entity.CoverageLimitAmount.ToString();
                    if (entity.DepartmentID != Constant.Facility.INPATIENT)
                        trCoverageLimitPerDay.Style.Add("display", "none");
                    chkIsCoverageLimitPerDay.Checked = entity.IsCoverageLimitPerDay;
                }
                else
                {
                    trCoverageLimit.Style.Add("display", "none");
                    hdnIsControlCovLimit.Value = entityCustContract.IsControlCoverageLimit.ToString();
                }
                if (entityCustContract.IsControlClassCare)
                    cboPayerControlClassCare.Value = entity.ControlClassID.ToString();
                else
                    trControlClassCare.Style.Add("display", "none");
                hdnIsControlClassCare.Value = entityCustContract.IsControlClassCare ? "1" : "0";
                divContractSummary.InnerHtml = entityCustContract.ContractSummary;
                chkIsGuaranteeLetterExistsCtl.Checked = entity.IsGuaranteeLetterExists;
            }
        }


        private void ControlToEntity(Registration entity)
        {
            entity.CorporateAccountNo = txtCorporateAccNo.Text;
            entity.CorporateAccountName = txtCorporateAccName.Text;
            //entity.GCCorporateAccountRelation = Helper.GetComboBoxValue(cboRelationship, true);
            if (hdnIsControlCovLimit.Value == "True")
            {
                entity.CoverageLimitAmount = Convert.ToDecimal(txtAmount.Text);
                if (chkIsCoverageLimitPerDay.Checked) entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
            }
            if (hdnIsControlClassCare.Value == "1")
            {
                entity.IsControlClassCare = true;
                if (cboPayerControlClassCare.Value != null)
                {
                    string classCare = cboPayerControlClassCare.Value.ToString();
                    if (!string.IsNullOrEmpty(classCare) && classCare != "0")
                    {
                        entity.ControlClassID = Convert.ToInt32(cboPayerControlClassCare.Value);
                    }
                    else
                    {
                        entity.ControlClassID = null;
                    }
                }
                else
                {
                    entity.ControlClassID = null;
                }
            }
            else
            {
                entity.ControlClassID = null;
            }
        }


        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvRegistrationPayerList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationIDCtlEditPayer.Value));
            grdView.DataBind();
        }

        protected override void OnLoadCtlControl()
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationIDCtlEditPayer.Value))[0];
            EntityToControl(entity);
            BindGridView();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";
            bool isPrimaryChange = false;
            if (param == "save")
            {
                if (hdnRegistrationPayerID.Value.ToString() != "")
                {
                    if (OnSaveGridEditRecord(ref errMessage, ref isPrimaryChange))
                    {
                        result += "success|";
                        if (isPrimaryChange) result += "primaryChange";
                    }
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveGridAddRecord(ref errMessage, ref isPrimaryChange))
                    {
                        result += "success|";
                        if (isPrimaryChange) result += "primaryChange";
                    }
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            RegistrationPayerDao entityPayerDao = new RegistrationPayerDao(ctx);
            try
            {
                Registration entity = entityDao.Get(Convert.ToInt32(hdnRegistrationIDCtlEditPayer.Value));

                List<PatientPaymentHd> lstPayment = BusinessLayer.GetPatientPaymentHdList(string.Format(
                                                        "RegistrationID = '{0}' AND GCTransactionStatus != '{1}' AND GCPaymentType != '{2}'",
                                                        hdnRegistrationIDCtlEditPayer.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT));
                if (lstPayment.Count() == 0)
                {
                    ControlToEntity(entity);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<RegistrationPayer> lstRegPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationIDCtlEditPayer.Value), ctx);
                    if (lstRegPayer.Where(t => Convert.ToInt32(t.IsPrimaryPayer) == 0).ToList().Count() > 0)
                    {
                        entity.IsUsingCOB = true;
                    }

                    RegistrationPayer regPayer = lstRegPayer.Where(t => t.RegistrationID == entity.RegistrationID && t.BusinessPartnerID == entity.BusinessPartnerID).FirstOrDefault();
                    if (regPayer != null)
                    {
                        regPayer.IsGuaranteeLetterExists = chkIsGuaranteeLetterExistsCtl.Checked;
                        regPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPayerDao.Update(regPayer);
                    }

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    result = true;
                    ctx.CommitTransaction();
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


        private void ControlToEntity(RegistrationPayer entity)
        {
            entity.BusinessPartnerID = Convert.ToInt32(hdnCtlPayerCompanyID.Value);
            entity.ContractID = Convert.ToInt32(hdnCtlContractID.Value);
            entity.IsGuaranteeLetterExists = chkIsGuaranteeLetterExistsCtlAdd.Checked;

            if (hdnCtlIsControlClassCare.Value == "1")
            {
                if (cboCtlControlClassCare.Value != null)
                {
                    string classCare = cboCtlControlClassCare.Value.ToString();
                    if (!string.IsNullOrEmpty(classCare) && classCare != "0")
                    {
                        entity.ControlClassID = Convert.ToInt32(cboCtlControlClassCare.Value);
                    }
                    else
                    {
                        entity.ControlClassID = null;
                    }
                }
                else
                {
                    entity.ControlClassID = null;
                }
            }
            else
            {
                entity.ControlClassID = null;
            }

            entity.CorporateAccountName = txtCorporateAccountName.Text;
            entity.CorporateAccountNo = txtCorporateAccountNo.Text;
            entity.CoverageLimitAmount = Convert.ToDecimal(txtCtlCoverageLimitAmount.Text);
            entity.CoverageTypeID = Convert.ToInt32(hdnCtlCoverageTypeID.Value);
            entity.GCCustomerType = cboCtlRegistrationPayer.Value.ToString();
            entity.IsCoverageLimitPerDay = chkCtlCoverageLimit.Checked;
            entity.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtlEditPayer.Value);
        }

        private bool OnSaveGridAddRecord(ref string errMessage, ref bool isPrimaryChange)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            try
            {
                Registration entityReg = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtlEditPayer.Value));

                List<PatientPaymentHd> lstPayment = BusinessLayer.GetPatientPaymentHdList(string.Format(
                                                        "RegistrationID = '{0}' AND GCTransactionStatus != '{1}' AND GCPaymentType != '{2}'",
                                                        hdnRegistrationIDCtlEditPayer.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT));

                if (lstPayment.Count() == 0)
                {
                    if (entityReg.BusinessPartnerID == 1)
                    {
                        result = false;
                        errMessage = "Maaf, tidak dapat menambah data pembayar dari registrasi " + entityReg.RegistrationNo + " karena penjamin utama adalah PERSONAL.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        if (entityReg.IsHasAIOPackage && cboCtlRegistrationPayer.Value.ToString() != Constant.CustomerType.PERSONAL)
                        {
                            result = false;
                            errMessage = "Maaf, Registrasi " + entityReg.RegistrationNo + " ini tidak dapat diubah penjamin bayar karena memiliki pengisian paket all-in-one AIO.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            RegistrationPayer entity = new RegistrationPayer();
                            ControlToEntity(entity);
                            if (chkIsPrimary.Checked)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<RegistrationPayer> lstRegPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationIDCtlEditPayer.Value), ctx);
                                if (lstRegPayer.Count() == 0)
                                {
                                    entity.IsPrimaryPayer = chkIsPrimary.Checked;
                                }
                                else
                                {
                                    //jika tambah baru ada yang di centang
                                    if (chkIsPrimary.Checked == true)
                                    {
                                        foreach (RegistrationPayer row in lstRegPayer)
                                        {
                                            row.IsPrimaryPayer = false;
                                            registrationPayerDao.Update(row);
                                        }
                                    }
                                    entity.IsPrimaryPayer = chkIsPrimary.Checked;

                                }
                            }
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            registrationPayerDao.Insert(entity);

                            if (cboCtlRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS)
                            {
                                entityReg.IsBPJS = true;
                                entityReg.BPJSAmount = Convert.ToDecimal(txtCtlCoverageLimitAmount.Text);

                                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("IsMainVisit = 1 AND RegistrationID = {0}", entityReg.RegistrationID), ctx).FirstOrDefault();
                                Patient oPatient = null;
                                if (entityReg.MRN != null && entityReg.MRN != 0)
                                {
                                    oPatient = patientDao.Get(Convert.ToInt32(entityReg.MRN));
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
                                    regBPJS.TanggalRujukan = entityReg.RegistrationDate;
                                    regBPJS.JenisPelayanan = entityReg.TransactionCode == Constant.TransactionCode.IP_REGISTRATION ? "1" : "2";
                                    regBPJS.CreatedBy = AppSession.UserLogin.UserID;
                                    registrationBPJSDao.Insert(regBPJS);
                                }
                            }

                            if (entity.IsPrimaryPayer)
                            {
                                isPrimaryChange = true;

                                entityReg.BusinessPartnerID = entity.BusinessPartnerID;
                                entityReg.ContractID = entity.ContractID;
                                entityReg.CoverageTypeID = entity.CoverageTypeID;
                                entityReg.CorporateAccountNo = entity.CorporateAccountNo;
                                entityReg.CoverageLimitAmount = entity.CoverageLimitAmount;
                                entityReg.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                                entityReg.GCTariffScheme = hdnCtlGCTariffScheme.Value;
                                entityReg.IsControlClassCare = (hdnCtlIsControlClassCare.Value == "1");

                                if (hdnCtlIsControlClassCare.Value == "1")
                                {
                                    entityReg.ControlClassID = entity.ControlClassID;
                                }
                                else
                                {
                                    entityReg.ControlClassID = null;
                                }

                                entityReg.EmployeeID = null;
                            }

                            entityReg.IsUsingCOB = true;
                            entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(entityReg);

                            ctx.CommitTransaction();
                        }
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, tidak dapat menambah data pembayar dari registrasi " + entityReg.RegistrationNo + " karena sudah memiliki Pembayaran.";
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
            return result;
        }

        private bool OnSaveGridEditRecord(ref string errMessage, ref bool isPrimaryChange)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            try
            {
                Registration entityReg = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtlEditPayer.Value));
                RegistrationPayer entity = BusinessLayer.GetRegistrationPayer(Convert.ToInt32(hdnRegistrationPayerID.Value));

                List<PatientPaymentHd> lstPayment = BusinessLayer.GetPatientPaymentHdList(string.Format(
                                                        "RegistrationID = '{0}' AND GCTransactionStatus != '{1}' AND GCPaymentType != '{2}'",
                                                        hdnRegistrationIDCtlEditPayer.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT));
                if (lstPayment.Count() == 0)
                {
                    if (entityReg.IsHasAIOPackage && cboCtlRegistrationPayer.Value.ToString() != Constant.CustomerType.PERSONAL)
                    {
                        result = false;
                        errMessage = "Maaf, Registrasi " + entityReg.RegistrationNo + " ini tidak dapat diubah penjamin bayar karena memiliki pengisian paket all-in-one AIO.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        ControlToEntity(entity);
                        if (chkIsPrimary.Checked)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<RegistrationPayer> lstRegPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationIDCtlEditPayer.Value), ctx);
                            if (lstRegPayer.Count() == 0)
                            {
                                entity.IsPrimaryPayer = chkIsPrimary.Checked;
                            }
                            else
                            {
                                //jika tambah baru ada yang di centang
                                if (chkIsPrimary.Checked == true)
                                {
                                    foreach (RegistrationPayer row in lstRegPayer)
                                    {
                                        row.IsPrimaryPayer = false;
                                        registrationPayerDao.Update(row);
                                    }
                                }
                                entity.IsPrimaryPayer = chkIsPrimary.Checked;
                            }


                        }
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationPayerDao.Update(entity);

                        if (cboCtlRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS)
                        {
                            entityReg.IsBPJS = true;
                            entityReg.BPJSAmount = Convert.ToDecimal(txtCtlCoverageLimitAmount.Text);

                            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("IsMainVisit = 1 AND RegistrationID = {0}", entityReg.RegistrationID), ctx).FirstOrDefault();
                            Patient oPatient = null;
                            if (entityReg.MRN != null && entityReg.MRN != 0)
                            {
                                oPatient = patientDao.Get(Convert.ToInt32(entityReg.MRN));
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
                                regBPJS.TanggalRujukan = entityReg.RegistrationDate;
                                regBPJS.JenisPelayanan = entityReg.TransactionCode == Constant.TransactionCode.IP_REGISTRATION ? "1" : "2";
                                regBPJS.CreatedBy = AppSession.UserLogin.UserID;
                                registrationBPJSDao.Insert(regBPJS);
                            }
                        }

                        if (entity.IsPrimaryPayer)
                        {
                            isPrimaryChange = true;

                            entityReg.BusinessPartnerID = entity.BusinessPartnerID;
                            entityReg.ContractID = entity.ContractID;
                            entityReg.CoverageTypeID = entity.CoverageTypeID;
                            entityReg.CorporateAccountNo = entity.CorporateAccountNo;
                            entityReg.CoverageLimitAmount = entity.CoverageLimitAmount;
                            entityReg.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                            entityReg.GCTariffScheme = hdnCtlGCTariffScheme.Value;
                            entityReg.IsControlClassCare = (hdnCtlIsControlClassCare.Value == "1");

                            if (hdnCtlIsControlClassCare.Value == "1")
                            {
                                entityReg.ControlClassID = entity.ControlClassID;
                            }
                            else
                            {
                                entityReg.ControlClassID = null;
                            }

                            entityReg.EmployeeID = null;
                        }

                        entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationDao.Update(entityReg);
                        ctx.CommitTransaction();
                        result = true;
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, tidak dapat mengubah data pembayar dari registrasi " + entityReg.RegistrationNo + " karena sudah memiliki Pembayaran.";
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            try
            {
                RegistrationPayer entity = BusinessLayer.GetRegistrationPayer(Convert.ToInt32(hdnRegistrationPayerID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                registrationPayerDao.Update(entity);

                Registration entityReg = BusinessLayer.GetRegistration(entity.RegistrationID);
                List<RegistrationPayer> lstRegPayer = BusinessLayer.GetRegistrationPayerList(string.Format(
                                                            "RegistrationID = {0} AND BusinessPartnerID != {1} AND IsPrimaryPayer = 0 AND IsDeleted = 0",
                                                            entityReg.RegistrationID, entity.BusinessPartnerID
                                                        ), ctx);
                if (lstRegPayer.Count() == 0 || lstRegPayer.Count() == 1)
                {
                    entityReg.IsUsingCOB = false;
                    entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entityReg);
                }
                ctx.CommitTransaction();
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