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
    public partial class ChangeTransactionPhysicianCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string filter = string.Format("TransactionID = {0}", param);
            vPatientChargesHd1 chargesHd = BusinessLayer.GetvPatientChargesHd1List(filter).FirstOrDefault();

            hdnTransactionIDCtlCP.Value = chargesHd.TransactionID.ToString();
            hdnTransactionHealthcareServiceUnitIDCtlCP.Value = chargesHd.HealthcareServiceUnitID.ToString();
            hdnRegistrationIDCtlCP.Value = chargesHd.RegistrationID.ToString();
            hdnVisitIDCtlCP.Value = chargesHd.VisitID.ToString();

            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtTransactionDate.Text = chargesHd.TransactionDateInString;
            txtTransactionTime.Text = chargesHd.TransactionTime;
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnTransactionHealthcareServiceUnitIDCtlCP.Value);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                string filter = string.Format("TransactionID = {0}", hdnTransactionIDCtlCP.Value);
                PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHdList(filter).FirstOrDefault();

                if (chargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", hdnTransactionIDCtlCP.Value, Constant.TransactionStatus.VOID);
                    List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterDt);

                    foreach (PatientChargesDt entityDt in lstDt)
                    {
                        entityDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }

                    retval = chargesHd.TransactionNo;

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi " + chargesHd.TransactionNo + " tidak dapat diubah karena sudah diproses.";
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