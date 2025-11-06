using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SurveyDataEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.MasterSurvey;
        }

        protected String GetPageTitle()
        {
            string filterMenu = string.Format("MenuCode = '{0}'", OnGetMenuCode());
            MenuMaster menu = BusinessLayer.GetMenuMasterList(filterMenu).FirstOrDefault();

            return menu.MenuCaption;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            if (param.Length > 1)
            {
                IsAdd = false;
                Int32 SurveyID = Convert.ToInt32(param[1]);
                hdnID.Value = SurveyID.ToString();
                DashboardSurvey entity = BusinessLayer.GetDashboardSurvey(Convert.ToInt32(SurveyID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            
            txtSurveyCode.Focus();
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSurveyCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSurveyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSurveyNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(DashboardSurvey entity)
        {
            txtSurveyCode.Text = entity.SurveyCode;
            txtSurveyName.Text = entity.SurveyName;
            txtSurveyNotes.Text = entity.SurveyNotes;

            if (!String.IsNullOrEmpty(entity.SurveyCode))
            {
                if (entity.SurveyCode.Contains("|"))
                {
                    string[] surveyInfo = entity.SurveyCode.Split('|');
                    txtSurveyCode.Text = surveyInfo[0];
                    txtSurveyName.Text = surveyInfo[1];
                    txtSurveyNotes.Text = surveyInfo[2];
                }
            }
            else
            {
                txtSurveyCode.Text = string.Empty;
                txtSurveyName.Text = string.Empty;
                txtSurveyNotes.Text = string.Empty;
            }
        }

        private void ControlToEntity(DashboardSurvey entity)
        {
            entity.SurveyCode = txtSurveyCode.Text;
            entity.SurveyName = txtSurveyName.Text;
            entity.SurveyNotes = txtSurveyNotes.Text;

            if (!string.IsNullOrEmpty(txtSurveyCode.Text))
            {
                entity.SurveyCode = string.Format("{0}", txtSurveyCode.Text);
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SurveyCode = '{0}'", txtSurveyCode.Text);
            List<DashboardSurvey> lst = BusinessLayer.GetDashboardSurveyList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Survey with Code " + txtSurveyCode.Text + " already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("SurveyCode = '{0}' AND SurveyID != {1}", txtSurveyCode.Text, ID);
            List<DashboardSurvey> lst = BusinessLayer.GetDashboardSurveyList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Survey with Code " + txtSurveyCode.Text + " already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            DashboardSurveyDao entityDao = new DashboardSurveyDao(ctx);
            bool result = false;
            try
            {
                DashboardSurvey entity = new DashboardSurvey();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
                ctx.CommitTransaction();
                result = true;
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                DashboardSurvey entity = BusinessLayer.GetDashboardSurvey(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDashboardSurvey(entity);
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