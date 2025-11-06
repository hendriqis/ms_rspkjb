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
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientDocumentListCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] paramSplit = param.Split('|');
            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", paramSplit[1]));
            hdnVisitIDCtl.Value = paramSplit[2];
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDocumentType = '0270^CD'", hdnVisitIDCtl.Value); //untuk cardio

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }
            List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "DocumentDate");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientDocument obj = (vPatientDocument)e.Row.DataItem;
            }
        }

        protected void cbpViewPatientList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "delete")
                {
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            Boolean result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDocumentDao entityDao = new PatientDocumentDao(ctx);
            try
            {
                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnIDCtl.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                if (entity.CreatedBy > 0 || entity.GCDocumentType != Constant.DocumentType.DIAGNOSTIC_IMAGING)
                {
                    Patient oPatient = BusinessLayer.GetPatient(entity.MRN);
                    if (oPatient != null)
                    {
                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\{1}", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), entity.FileName);
                        path = path.Replace("#MRN", oPatient.MedicalNo);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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