using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ProcedureEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PROCEDURE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID.ToString();
                Procedures entity = BusinessLayer.GetProcedures(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtProcedureCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProcedureCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProcedureName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Procedures entity)
        {
            txtProcedureCode.Text = entity.ProcedureID;
            txtProcedureName.Text = entity.ProcedureName;
        }

        private void ControlToEntity(Procedures entity)
        {
            entity.ProcedureID = txtProcedureCode.Text;
            entity.ProcedureName = txtProcedureName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ProcedureID = '{0}'", txtProcedureCode.Text);
            List<Procedures> lst = BusinessLayer.GetProceduresList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Visit Type with Code " + txtProcedureCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Procedures entity = new Procedures();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertProcedures(entity);
                retval = entity.ProcedureID;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Procedures entity = BusinessLayer.GetProcedures(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateProcedures(entity);
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