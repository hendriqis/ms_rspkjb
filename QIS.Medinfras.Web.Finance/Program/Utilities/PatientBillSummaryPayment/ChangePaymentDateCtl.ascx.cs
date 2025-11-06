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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangePaymentDateCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnPaymentIDCtl.Value = param;

            string filterExpression = string.Format("PaymentID = {0}", hdnPaymentIDCtl.Value);
            vPatientPaymentHd entityHd = BusinessLayer.GetvPatientPaymentHdList(filterExpression).FirstOrDefault();

            txtPaymentNo.Text = entityHd.PaymentNo;
            txtPaymentDate.Text = entityHd.PaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentTime.Text = entityHd.PaymentTime;
            txtRegistrationNo.Text = entityHd.RegistrationNo;
            txtPatientInfomation.Text = string.Format("{0} - {1}", entityHd.MedicalNo, entityHd.PatientName);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPaymentTime, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            try
            {
                PatientPaymentHd entityDao = entityHdDao.Get(Convert.ToInt32(hdnPaymentIDCtl.Value));
                entityDao.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
                entityDao.PaymentTime = txtPaymentTime.Text;
                entityDao.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityDao);

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