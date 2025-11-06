using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class AddMCUPackageCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected bool IsAllowEditPatientVisit = true;
        protected bool IsEditable = true;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] flagParam = param.Split('|');
                if (flagParam.Length > 1)
                {
                    hdnRegistrationID.Value = flagParam[0];
                    IsEditable = false;
                    divContainerAddData.Style.Add("display", "none");
                }
                else hdnRegistrationID.Value = param;
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtMRN.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                hdnDepartmentIDCtlPckg.Value = entity.DepartmentID;

                hdnVisitID.Value = entity.VisitID.ToString();

                BindGridView(1, true, ref PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitItemPackageRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }
            List<vConsultVisitItemPackage> lstEntity = BusinessLayer.GetvConsultVisitItemPackageList(filterExpression, 8, pageIndex, "ID ASC");
            grdPatientVisitTransHd.DataSource = lstEntity;
            grdPatientVisitTransHd.DataBind();
        }

        protected void cbpAddMCUPackage_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), true, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    result = "save|";
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else if (param[0] == "delete")
                {
                    result = "delete|";
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;       
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                ConsultVisit cv = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                Helper.InsertConsultVisitItemPackage(ctx, cv, hdnDepartmentIDCtlPckg.Value, Convert.ToInt32(hdnItemID.Value), 0);
                ctx.CommitTransaction();
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


        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitItemPackageDao entityVisitDao = new ConsultVisitItemPackageDao(ctx);
            try
            {
                ConsultVisitItemPackage entity = BusinessLayer.GetConsultVisitItemPackage(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityVisitDao.Update(entity);
                ctx.CommitTransaction();
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
    }
}