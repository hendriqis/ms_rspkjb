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
    public partial class NursingOutcomeMasterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_OUTCOME_MASTER;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                NursingOutcome entity = BusinessLayer.GetNursingOutcome(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtNursingOutcomeCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNursingOutcomeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNursingOutcomeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtResult, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDescription, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NursingOutcome entity)
        {
            txtNursingOutcomeCode.Text = entity.NursingOutcomeCode;
            txtNursingOutcomeName.Text = entity.NursingOutcomeText;
            txtResult.Text = entity.NursingOutcomeResult;
            txtDescription.Text = entity.Remarks;
        }

        private void ControlToEntity(NursingOutcome entity)
        {
            entity.NursingOutcomeCode = txtNursingOutcomeCode.Text;
            entity.NursingOutcomeText = txtNursingOutcomeName.Text;
            entity.NursingOutcomeResult = txtResult.Text;
            entity.Remarks = txtDescription.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("NursingOutcomeCode = '{0}'", txtNursingOutcomeCode.Text);
            List<NursingOutcome> lst = BusinessLayer.GetNursingOutcomeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kode Luaran Asuhan Keperawatan dengan " + txtNursingOutcomeCode.Text + " sudah digunakan!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);

            string FilterExpression = string.Format("NursingOutcomeCode = '{0}' AND NursingOutcomeID != {1} AND IsDeleted = 0", txtNursingOutcomeCode.Text, ID);
            List<NursingOutcome> lst = BusinessLayer.GetNursingOutcomeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kode Luaran Asuhan Keperawatan dengan " + txtNursingOutcomeCode.Text + " sudah digunakan!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingOutcomeDao entityDao = new NursingOutcomeDao(ctx);
            bool result = false;
            try
            {
                NursingOutcome entity = new NursingOutcome();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingOutcomeMaxID(ctx).ToString();
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
                NursingOutcome entity = BusinessLayer.GetNursingOutcome(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingOutcome(entity);
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