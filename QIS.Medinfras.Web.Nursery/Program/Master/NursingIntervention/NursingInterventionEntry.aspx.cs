using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingInterventionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_INTERVENTION;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                NursingIntervention entity = BusinessLayer.GetNursingIntervention(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtNursingInterventionCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNursingInterventionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNursingInterventionName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDescription, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NursingIntervention entity)
        {
            txtNursingInterventionCode.Text = entity.NurseInterventionCode;
            txtNursingInterventionName.Text = entity.NurseInterventionName;
            txtDescription.Text = entity.Description;
        }

        private void ControlToEntity(NursingIntervention entity)
        {
            entity.NurseInterventionCode = txtNursingInterventionCode.Text;
            entity.NurseInterventionName = txtNursingInterventionName.Text;
            entity.Description = txtDescription.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("NurseInterventionCode = '{0}'", txtNursingInterventionCode.Text);
            List<NursingIntervention> lst = BusinessLayer.GetNursingInterventionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Nursing Problem with Code " + txtNursingInterventionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);

            string FilterExpression = string.Format("NurseInterventionCode = '{0}' AND NurseInterventionID != {1} AND IsDeleted = 0", txtNursingInterventionCode.Text, ID);
            List<NursingIntervention> lst = BusinessLayer.GetNursingInterventionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Nursing Problem with Code " + txtNursingInterventionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingInterventionDao entityDao = new NursingInterventionDao(ctx);
            bool result = false;
            try
            {
                NursingIntervention entity = new NursingIntervention();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingDiagnoseMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                NursingIntervention entity = BusinessLayer.GetNursingIntervention(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingIntervention(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}