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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TermEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TERM;
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
                Term entity = BusinessLayer.GetTerm(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtTermCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTermCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTermName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTermDay, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Term entity)
        {
            txtTermCode.Text = entity.TermCode;
            txtTermName.Text = entity.TermName;
            txtTermDay.Text = entity.TermDay.ToString();
        }

        private void ControlToEntity(Term entity)
        {
            entity.TermCode = txtTermCode.Text;
            entity.TermName = txtTermName.Text;
            entity.TermDay = Convert.ToInt16(txtTermDay.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TermCode = '{0}'", txtTermCode.Text);
            List<Term> lst = BusinessLayer.GetTermList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Term with Code " + txtTermCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TermCode = '{0}' AND TermID != {1}", txtTermCode.Text, hdnID.Value);
            List<Term> lst = BusinessLayer.GetTermList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Term with Code " + txtTermCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TermDao entityDao = new TermDao(ctx);
            bool result = false;
            try
            {
                Term entity = new Term();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetTermMaxID(ctx).ToString();
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
                Term entity = BusinessLayer.GetTerm(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTerm(entity);
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