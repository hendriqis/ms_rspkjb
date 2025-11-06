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
    public partial class NursingIndicatorMasterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_INDICATOR_MASTER;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                NursingIndicator entity = BusinessLayer.GetNursingIndicator(Convert.ToInt32(ID));
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
            SetControlEntrySetting(txtScale1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtScale2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtScale3, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtScale4, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtScale5, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(NursingIndicator entity)
        {
            txtNursingOutcomeCode.Text = entity.NursingIndicatorCode;
            txtNursingOutcomeName.Text = entity.NursingIndicatorText;
            txtScale1.Text = entity.Scale1Text;
            txtScale2.Text = entity.Scale2Text;
            txtScale3.Text = entity.Scale3Text;
            txtScale4.Text = entity.Scale4Text;
            txtScale5.Text = entity.Scale5Text;
        }

        private void ControlToEntity(NursingIndicator entity)
        {
            entity.NursingIndicatorCode = txtNursingOutcomeCode.Text;
            entity.NursingIndicatorText = txtNursingOutcomeName.Text;
            entity.Scale1Text = txtScale1.Text;
            entity.Scale2Text = txtScale2.Text;
            entity.Scale3Text = txtScale3.Text;
            entity.Scale4Text = txtScale4.Text;
            entity.Scale5Text = txtScale5.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("NursingIndicatorCode = '{0}'", txtNursingOutcomeCode.Text);
            List<NursingIndicator> lst = BusinessLayer.GetNursingIndicatorList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kode Kriteria Hasil Asuhan Keperawatan dengan " + txtNursingOutcomeCode.Text + " sudah digunakan!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);

            string FilterExpression = string.Format("NursingIndicatorCode = '{0}' AND NursingIndicatorID != {1} AND IsDeleted = 0", txtNursingOutcomeCode.Text, ID);
            List<NursingIndicator> lst = BusinessLayer.GetNursingIndicatorList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kode Kriteria Hasil Asuhan Keperawatan dengan " + txtNursingOutcomeCode.Text + " sudah digunakan!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingIndicatorDao entityDao = new NursingIndicatorDao(ctx);
            bool result = false;
            try
            {
                NursingIndicator entity = new NursingIndicator();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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
                NursingIndicator entity = BusinessLayer.GetNursingIndicator(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingIndicator(entity);
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