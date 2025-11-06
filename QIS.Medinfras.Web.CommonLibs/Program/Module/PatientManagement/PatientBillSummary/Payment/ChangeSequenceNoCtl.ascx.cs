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
    public partial class ChangeSequenceNoCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPatientPaymentDtID.Value = param;

            string filterPaymentDt = string.Format("PaymentDetailID = {0}", hdnPatientPaymentDtID.Value);
            vPatientPaymentDt entityDt = BusinessLayer.GetvPatientPaymentDtList(filterPaymentDt).FirstOrDefault();
            PatientPaymentDtInfo entityDtInfo = BusinessLayer.GetPatientPaymentDtInfo(entityDt.PaymentDetailID);

            txtPaymentNo.Text = entityDt.PaymentNo;
            txtPaymentDateTime.Text = string.Format("{0} {1}", entityDt.PaymentDateInString, entityDt.PaymentTime);
            txtBusinessPartnerName.Text = entityDt.BusinessPartnerName;
            txtSequenceNo.Text = entityDtInfo.SequenceNo.ToString();

            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            IsAdd = false;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSequenceNo, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao entityDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            try
            {
                PatientPaymentDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnPatientPaymentDtID.Value));
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);

                PatientPaymentDtInfo entityDtInfo = entityDtInfoDao.Get(entityDt.PaymentDetailID);
                entityDtInfo.SequenceNo = Convert.ToInt32(txtSequenceNo.Text);
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