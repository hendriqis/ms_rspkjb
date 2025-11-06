using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingProblemEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_PROBLEM;
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
                NursingProblem entity = BusinessLayer.GetNursingProblem(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtProblemCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProblemCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProblemName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(NursingProblem entity)
        {
            txtProblemCode.Text = entity.ProblemCode;
            txtProblemName.Text = entity.ProblemName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(NursingProblem entity)
        {
            entity.ProblemCode = txtProblemCode.Text;
            entity.ProblemName = txtProblemName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ProblemCode = '{0}'", txtProblemCode.Text);
            List<NursingProblem> lst = BusinessLayer.GetNursingProblemList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Nursing Problem with Code " + txtProblemCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);

            string FilterExpression = string.Format("ProblemCode = '{0}' AND ProblemID != {1} AND IsDeleted = 0", txtProblemCode.Text, ID);
            List<NursingProblem> lst = BusinessLayer.GetNursingProblemList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Nursing Problem with Code " + txtProblemCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                NursingProblem entity = new NursingProblem();
                ControlToEntity(entity);
                entity.ProblemCode = txtProblemCode.Text;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNursingProblem(entity);
                retval = entity.ProblemCode;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                NursingProblem entity = BusinessLayer.GetNursingProblem(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingProblem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}