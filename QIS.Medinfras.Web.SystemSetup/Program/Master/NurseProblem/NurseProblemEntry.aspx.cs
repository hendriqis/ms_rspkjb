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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class NurseProblemEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.NURSING_PROBLEM;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                Int32 problemID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = problemID.ToString();
                NursingProblem entity = BusinessLayer.GetNursingProblem(Convert.ToInt32(problemID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtNurseProblemCode.Focus();
        }
       
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNurseProblemCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNursingProblemName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(NursingProblem entity)
        {
            txtNurseProblemCode.Text = entity.ProblemCode;
            txtNursingProblemName.Text = entity.ProblemName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(NursingProblem entity)
        {
            entity.ProblemCode = txtNurseProblemCode.Text;
            entity.ProblemName = txtNursingProblemName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ProblemCode = '{0}'", txtNurseProblemCode.Text);
            List<NursingProblem> lst = BusinessLayer.GetNursingProblemList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Nurse Problem with Code " + txtNurseProblemCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }
        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 NurseProblemID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("ProblemCode = '{0}'  AND ProblemID != {1}", txtNurseProblemCode.Text, NurseProblemID);
            List<NursingProblem> lst = BusinessLayer.GetNursingProblemList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Nurse Problem with Code " + txtNurseProblemCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingProblemDao entityDao = new NursingProblemDao(ctx);
            bool result = false;
            try
            {
                NursingProblem entity = new NursingProblem();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingProblemMaxID(ctx).ToString();
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
                NursingProblem entity = BusinessLayer.GetNursingProblem(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingProblem(entity);
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