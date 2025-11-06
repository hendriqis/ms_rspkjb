using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Globalization;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingProblemQuickPicksCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;

            hdnParam.Value = param;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            txtProblemDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProblemTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (rblItemSource.SelectedValue == "2")
            {                
                // From History
                string problemID = GetProblemCodeFromAssessment();
                if (!string.IsNullOrEmpty(problemID))
                {
                    filterExpression += string.Format(" AND ProblemID IN ({0})", problemID); 
                }
            }

            return filterExpression;
        }

        private string GetProblemCodeFromAssessment()
        {
            string result = string.Empty;
            return result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    vItemBalanceQuickPick1 entity = e.Row.DataItem as vItemBalanceQuickPick1;
            //    CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
            //    if (lstSelectedMember.Contains(entity.ItemID.ToString()))
            //        chkIsSelected.Checked = true;
            //    System.Drawing.Color foreColor = System.Drawing.Color.Black;
            //    if (entity.QuantityEND == 0)
            //        foreColor = System.Drawing.Color.Red;
            //    e.Row.Cells[2].ForeColor = foreColor;
            //    e.Row.Cells[3].ForeColor = foreColor;
            //}
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format(" ProblemName LIKE '%{0}%' AND IsDeleted = 0", hdnFilterItem.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetNursingProblemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<NursingProblem> lstEntity = BusinessLayer.GetNursingProblemList(filterExpression, 10, pageIndex, "ProblemName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = false;

            IDbContext ctx = DbFactory.Configure(true);
            NursingPatientProblemDao entityDao = new NursingPatientProblemDao(ctx);

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberName = hdnSelectedMemberName.Value.Split('^');

                NursingPatientProblem entity;

                if (hdnID.Value == "" || hdnID.Value == "0")
                {
                    #region Nursing Patient Problem

                    foreach (String itemID in lstSelectedMember)
                    {
                        entity = new NursingPatientProblem();
                        entity.ProblemDate = Helper.GetDatePickerValue(txtProblemDate);
                        entity.ProblemTime = txtProblemTime.Text;
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.ProblemID = Convert.ToInt32(itemID);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }

                    #endregion

                    result = true;
                }

                if (result == true)
                {
                    retval = string.Format("{0}|{1}",AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.RegistrationNo);
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void ControlToEntity(NursingPatientProblem entity)
        {
            entity.ProblemDate = Helper.GetDatePickerValue(txtProblemDate);
            entity.ProblemTime = txtProblemTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
        }
    }
}