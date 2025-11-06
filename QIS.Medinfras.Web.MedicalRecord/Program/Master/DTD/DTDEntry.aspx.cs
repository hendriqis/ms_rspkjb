using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class DTDEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.DTD;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                DTD entity = BusinessLayer.GetDTD(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtDTDCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDTDCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDTDName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDTDLabel, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(DTD entity)
        {
            txtDTDCode.Text = entity.DTDNo;
            txtDTDName.Text = entity.DTDName;
            txtDTDLabel.Text = entity.DTDLabel;
        }

        private void ControlToEntity(DTD entity)
        {
            entity.DTDNo = txtDTDCode.Text;
            entity.DTDName = txtDTDName.Text;
            entity.DTDLabel = txtDTDLabel.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("DTDNo = '{0}'", txtDTDCode.Text);
            List<DTD> lst = BusinessLayer.GetDTDList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " DTD with Code " + txtDTDCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                DTD entity = new DTD();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertDTD(entity);
                retval = entity.DTDNo;
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
                DTD entity = BusinessLayer.GetDTD(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDTD(entity);
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