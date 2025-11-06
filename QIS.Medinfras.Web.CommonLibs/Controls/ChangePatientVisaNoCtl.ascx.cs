using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePatientVisaNoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;
            vRegistration oReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value)).FirstOrDefault();

            txtRegNo.Text = oReg.RegistrationNo;
            txtMedicalNo.Text = oReg.MedicalNo;
            txtPatientName.Text = oReg.PatientName;
            txtPatientVisaNumber.Text = oReg.PatientVisaNumber;

        }
        protected void cbpPatientReg_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "update")
            {
                if (OnSaveUpdateRecord(ref errMessage))
                {
                    result += string.Format("success");
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
                
            }

            panel.JSProperties["cpResult"] = result;
        }

        public bool OnSaveUpdateRecord(ref string errMessage)
        {

            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            try {
                Registration oReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value), ctx).FirstOrDefault();
                oReg.PatientVisaNumber = txtPatientVisaNumber.Text;
                entityDao.Update(oReg);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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