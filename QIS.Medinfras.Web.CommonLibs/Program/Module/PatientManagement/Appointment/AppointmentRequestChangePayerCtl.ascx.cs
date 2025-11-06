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
    public partial class AppointmentRequestChangePayerCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = true;
                hdnProcessType.Value = param.Split('|')[0];
                hdnAppointmentRequestID.Value = param.Split('|')[1];

                vAppointmentRequest entity = null;
                if (hdnProcessType.Value == "single")
                {
                    entity = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID = {0}", hdnAppointmentRequestID.Value)).FirstOrDefault();
                }

                SetControlProperties();

                EntityToControl(hdnProcessType.Value == "single" ? entity : null);

                hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;
            }
        }
        protected string GetCustomerTypePersonal()
        {
            return Constant.CustomerType.PERSONAL;
        }
        protected string GetCustomerTypeHealthcare()
        {
            return Constant.CustomerType.HEALTHCARE;
        }
        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CUSTOMER_TYPE));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPayer, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPayer.Value = Constant.CustomerType.PERSONAL;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare, "ClassName", "ClassID");
            cboControlClassCare.SelectedIndex = 0;

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

          
            
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetCustomerContractFilterExpression()
        {
            return string.Format(" AND '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0", DateTime.Now.ToString("yyyyMMdd"));
        }

        private void EntityToControl(vAppointmentRequest entity)
        {
            if (entity != null)
            {
                cboPayer.Value = entity.GCCustomerType;
                hdnPayerID.Value = entity.BusinessPartnerID.ToString();
                txtPayerCompanyCode.Text = entity.BusinessPartnerCode;
                txtPayerCompanyName.Text = entity.BusinessPartnerName;

                hdnPayerID.Value = entity.BusinessPartnerID.ToString();
                txtPayerCompanyCode.Text = entity.BusinessPartnerCode;
                txtPayerCompanyName.Text = entity.BusinessPartnerName;
                hdnContractID.Value = entity.ContractID.ToString();
                txtContractNo.Text = entity.ContractNo;
                hdnCoverageTypeID.Value = entity.CoverageTypeID.ToString();
                txtCoverageTypeCode.Text = entity.CoverageTypeCode;
                txtCoverageTypeName.Text = entity.CoverageTypeName;
                hdnEmployeeID.Value = entity.EmployeeID.ToString();
                txtEmployeeCode.Text = entity.EmployeeCode;
                txtEmployeeName.Text = entity.EmployeeName;
                 
                txtCoverageLimit.Text = entity.CoverageLimitAmount.ToString("N");
                chkIsCoverageLimitPerDay.Checked = entity.IsCoverageLimitPerDay;
                 
                Int32 ContractID = Convert.ToInt32(hdnContractID.Value);
                CustomerContract entityContract = BusinessLayer.GetCustomerContract(Convert.ToInt32(ContractID));
                if (entityContract != null)
                {
                    txtContractPeriod.Text = entityContract.EndDate.ToString(Constant.FormatString.DATE_FORMAT);
                }

                if (!entity.IsControlCoverageLimit)
                {
                    trCoverageLimit.Attributes.Add("style", "display:none");
                    trCoverageLimitPerDay.Attributes.Add("style", "display:none");
                }
                else if (entity.DepartmentID != Constant.Facility.INPATIENT)
                    trCoverageLimitPerDay.Attributes.Add("style", "display:none");

                if (!entity.IsControlClassCare)
                    trControlClassCare.Attributes.Add("style", "display:none");
                else
                    cboControlClassCare.Value = entity.ControlClassID.ToString();

                 

                if (entity.IsControlCoverageLimit) { 
                    trCoverageLimit.Attributes.Remove("style");
                }
                else
                {
                    trCoverageLimit.Attributes.Add("style", "display:none");
                     trCoverageLimitPerDay.Attributes.Add("style", "display:none");
                     txtCoverageLimit.Attributes.Add("style", "display:none");

                }

            }
            else
            {
                cboPayer.Value = Constant.CustomerType.PERSONAL;
                hdnPayerID.Value = "1";
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentRequestDao entityDao = new AppointmentRequestDao(ctx);
            try
            {
                if (hdnProcessType.Value == "single")
                {
                    bool isValid = true; 

                    AppointmentRequest entity = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(hdnAppointmentRequestID.Value));
                    entity.GCCustomerType = cboPayer.Value.ToString();
                    if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                    {
                        entity.BusinessPartnerID = 1;
                    }
                    else
                    {
                       
                        if ( string.IsNullOrEmpty(txtContractNo.Text) || string.IsNullOrEmpty(txtCoverageTypeCode.Text) || string.IsNullOrEmpty(txtPayerCompanyCode.Text) )
                        {
                            retval = "0";
                            errMessage = "Mohon dilengkapi untuk pengisian penjamin bayar";
                            isValid = false;
                        }
                        else {
                            entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                            entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                            entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                            if (!string.IsNullOrEmpty(txtCoverageLimit.Text))
                            {
                                entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                            }
                            else {
                                entity.CoverageLimitAmount = 0;
                            }

                            entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                            entity.GCTariffScheme = hdnGCTariffScheme.Value;
                            entity.IsControlClassCare = (hdnIsControlClassCare.Value == "1");
                            if (entity.IsControlClassCare)
                            {
                                entity.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                            }
                            else {
                                entity.ControlClassID = null;
                            }

                            if (hdnEmployeeID.Value == "" || hdnEmployeeID.Value == "0")
                            {
                                entity.EmployeeID = null;
                            }
                            else {
                                entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);
                            }
                            
                        }
                    }

                    if (isValid)
                    {
                        entityDao.Update(entity);
                        retval = "1";
                        ctx.CommitTransaction();
                    }
                    else {
                        retval = "0";
                    }
                    
                }
                else if (hdnProcessType.Value == "multi")
                {
                    List<AppointmentRequest> lstEntity = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentRequestID IN ({0})", hdnAppointmentRequestID.Value));
                    if (lstEntity.Count > 0)
                    {
                        foreach (AppointmentRequest entity in lstEntity)
                        {
                            entity.GCCustomerType = cboPayer.Value.ToString();
                            if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                            {
                                entity.BusinessPartnerID = 1;
                            }
                            else
                            {
                                entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                            }
                            entityDao.Update(entity);
                        }
                        retval = "1";
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Data Permintaan Perjanjian tidak ditemukan";
                        ctx.RollBackTransaction();
                    }
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