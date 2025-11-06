using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationEditResultDeliveryPlanCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            hdnRegistrationIDCtl.Value = param;
            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationIDCtl.Value));
            
            EntityToControl(entity);
        }
        private void EntityToControl(Registration entity)
        {
            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.RESULT_DELIVERY_PLAN);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboResultDeliveryPlanEdit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.RESULT_DELIVERY_PLAN || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            string filterReg = string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value);
            vRegistration reg = BusinessLayer.GetvRegistrationList(filterReg).FirstOrDefault();
            txtRegistrationNo.Text = reg.RegistrationNo;
            txtNoRM.Text = reg.MedicalNo;
            txtPatient.Text = reg.PatientName;
            if (reg.GCResultDeliveryPlan != null && reg.GCResultDeliveryPlan != "")
            {
                if (reg.GCResultDeliveryPlan != Constant.ResultDeliveryPlan.OTHERS)
                {
                    txtResultDeliveryPlanOthersEdit.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    txtResultDeliveryPlanOthersEdit.Attributes.Remove("readonly");
                }
                cboResultDeliveryPlanEdit.Value = reg.GCResultDeliveryPlan;
                txtResultDeliveryPlanOthersEdit.Text = reg.ResultDeliveryPlanOthers;
            }
            else
            {
                cboResultDeliveryPlanEdit.Text = "";
                txtResultDeliveryPlanOthersEdit.Text = "";
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtResultDeliveryPlanOthersEdit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboResultDeliveryPlanEdit, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);

            try
            {
                Registration reg = entityRegistrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (cboResultDeliveryPlanEdit.Value != null)
                {
                    reg.GCResultDeliveryPlan = cboResultDeliveryPlanEdit.Value.ToString();
                }
                else
                {
                    reg.GCResultDeliveryPlan = null;
                }

                reg.ResultDeliveryPlanOthers = txtResultDeliveryPlanOthersEdit.Text;
                reg.LastUpdatedBy = AppSession.UserLogin.UserID;

                retval = reg.RegistrationNo;
                entityRegistrationDao.Update(reg);

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