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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ProcedureEntry1Ctl : BasePagePatientPageEntryCtl
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
        }


        protected void cboPopupLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
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
            string filterExpression = string.Format("ProcedureID NOT IN (SELECT ProcedureID FROM PatientProcedure WHERE VisitID = {0} AND IsDeleted = 0) AND ProcedureName LIKE '%{1}%' AND  IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnFilterItem.Value);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetProceduresRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<Procedures> lstEntity = BusinessLayer.GetProceduresList(filterExpression, 10, pageIndex, "ProcedureName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao entityDao = new PatientProcedureDao(ctx);

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');

                if (AppSession.RegisteredPatient != null && AppSession.RegisteredPatient.VisitID != 0)
                {
                    foreach (string item in lstSelectedMember)
                    {
                        PatientProcedure entity = new PatientProcedure();
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = (int)AppSession.UserLogin.ParamedicID;
                        entity.ProcedureDate = DateTime.Now.Date;
                        entity.ProcedureTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        entity.ProcedureID = item;
                        entity.IsCreatedBySystem = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                }
                else
                {
                    result = false;
                    retval = "0";
                    errMessage = "Invalid Patient Visit Information";
                }

                
                if (result == true)
                {
                    retval = AppSession.RegisteredPatient.VisitID.ToString();
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
    }
}