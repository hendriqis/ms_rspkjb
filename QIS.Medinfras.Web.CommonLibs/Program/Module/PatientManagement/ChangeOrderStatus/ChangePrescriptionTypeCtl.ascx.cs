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
using System.Net;
using Newtonsoft.Json;
using System.IO;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePrescriptionTypeCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionID.Value = paramInfo[0];

            PatientChargesHd chargesHD = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
            txtTransactionNo.Text = chargesHD.TransactionNo;
            PrescriptionOrderHd order = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(chargesHD.PrescriptionOrderID));
            hdnPrescriptionID.Value = chargesHD.PrescriptionOrderID.ToString();
            txtPrescriptionType.Text = BusinessLayer.GetStandardCode(order.GCPrescriptionType).StandardCodeName;

            String filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            IsAdd = false;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                PrescriptionOrderHd order = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionID.Value));
                order.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                order.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(order);
                ctx.CommitTransaction();
                return result;
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