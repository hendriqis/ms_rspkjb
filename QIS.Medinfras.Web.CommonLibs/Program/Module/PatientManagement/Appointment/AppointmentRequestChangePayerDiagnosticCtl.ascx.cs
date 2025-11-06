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
    public partial class AppointmentRequestChangePayerDiagnosticCtl : BaseEntryPopupCtl
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

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CUSTOMER_TYPE, Constant.CustomerType.PERSONAL));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPayer, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPayer.Value = Constant.CustomerType.INSURANCE;
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
            }
            else
            {
                cboPayer.Value = Constant.CustomerType.INSURANCE;
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
                    AppointmentRequest entity = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(hdnAppointmentRequestID.Value));
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

                    retval = "1";

                    ctx.CommitTransaction();
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