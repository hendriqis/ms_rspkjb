using System;
using System.Collections.Generic;
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
    public partial class SetBPJSClaimTypeCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPatientPaymentIDCtl.Value = param;

            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "ParameterCode IN ('{0}')",
                                                                        Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS //0
                                                                    ));
            hdnSetvarCustomerTypeBPJSCtl.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue;

            string filterPaymentDt = string.Format("PaymentID = {0} AND GCCustomerType = '{1}' AND IsDeleted = 0", hdnPatientPaymentIDCtl.Value, hdnSetvarCustomerTypeBPJSCtl.Value);
            vPatientPaymentDt entityDt = BusinessLayer.GetvPatientPaymentDtList(filterPaymentDt).FirstOrDefault();
            PatientPaymentDtInfo entityDtInfo = BusinessLayer.GetPatientPaymentDtInfo(entityDt.PaymentDetailID);

            hdnPatientPaymentDtIDCtl.Value = entityDt.PaymentDetailID.ToString();
            txtPaymentNo.Text = entityDt.PaymentNo;
            txtPaymentDateTime.Text = string.Format("{0} {1}", entityDt.PaymentDateInString, entityDt.PaymentTime);
            txtBusinessPartnerName.Text = entityDt.BusinessPartnerName;
            txtSequenceNo.Text = entityDtInfo.SequenceNo.ToString();


            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                                            "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.BPJS_CLAIM_TYPE //0
                                        ));
            lstSc.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboBPJSClaimType, lstSc, "StandardCodeName", "StandardCodeID");

            cboBPJSClaimType.Value = entityDtInfo.GCBPJSClaimType;

            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            IsAdd = false;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboBPJSClaimType, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao entityDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            try
            {
                PatientPaymentDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnPatientPaymentDtIDCtl.Value));
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);

                PatientPaymentDtInfo entityDtInfo = entityDtInfoDao.Get(entityDt.PaymentDetailID);
                if (cboBPJSClaimType.Value != null && cboBPJSClaimType.Value != "")
                {
                    entityDtInfo.GCBPJSClaimType = cboBPJSClaimType.Value.ToString();
                }
                else
                {
                    entityDtInfo.GCBPJSClaimType = null;
                }
                entityDtInfoDao.Update(entityDtInfo);

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