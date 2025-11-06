using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Laboratory.Program.LaboratoryOrder
{
    public partial class SendOrderToLISCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnTransactionID.Value = param;
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                        Constant.SettingParameter.LB_LIS_PROVIDER //1
                                                                    ), ctx);

                string lisProvider = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.LB_LIS_PROVIDER).FirstOrDefault().ParameterValue;

                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                switch (lisProvider)
                {
                    case Constant.LIS_PROVIDER.HCLAB:
                        BusinessLayer.SendToLISInterfaceDB(transactionID, ctx);
                        break;
                    case Constant.LIS_PROVIDER.GRACIA:
                        BusinessLayer.SendToLISInterfaceDB_GRACIA(transactionID, ctx);
                        break;
                }
                result = true;
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