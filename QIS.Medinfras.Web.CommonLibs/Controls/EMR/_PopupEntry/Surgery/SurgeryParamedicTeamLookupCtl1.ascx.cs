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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SurgeryParamedicTeamLookupCtl1 : BaseProcessPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnIsNewRecord.Value = paramInfo[0] == "0" ? "1" : "0";

            string _reportID = paramInfo[2];
            if (_reportID != "0")
            {
                hdnLookupID.Value = paramInfo[0];
                hdnLookupTestOrderID.Value = paramInfo[1];
                hdnLookupSurgeryReportID.Value = _reportID;

                string filterExpression = string.Format("TestOrderID = {0}", hdnLookupTestOrderID.Value);
                vSurgeryTestOrderHd1 entity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression).FirstOrDefault();
                EntityToControl(entity);

                BindGridView(1, true, ref PageCount);
            }
        }

        private void EntityToControl(vSurgeryTestOrderHd1 entity)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtOrderNo.Text = entity.TestOrderNo;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnLookupTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtParamedicTeamRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDtParamedicTeam> lstEntity = BusinessLayer.GetvTestOrderDtParamedicTeamList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCParamedicRole");

            grdLookupParamedicTeamView.DataSource = lstEntity;
            grdLookupParamedicTeamView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
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

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                int reportID = Convert.ToInt32(hdnLookupSurgeryReportID.Value);
                string referenceNo = string.Empty;

                string processResult = "0|Terjadi kesalahan ketika copy jenis tindakan";
                processResult = CopyFromOrder(hdnSelectedID.Value);
                string[] resultInfo = ((string)processResult).Split('|');

                if (resultInfo[0] == "1")
                {
                    result = true;
                }
                else
                {
                    result = false;
                    errMessage = resultInfo[1];
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private string CopyFromOrder(string recordID)
        {
            string result = "0|Terjadi kesalahan ketika copy hasil pemeriksaan";

            if (!string.IsNullOrEmpty(recordID) && recordID != "0")
            {
                List<vTestOrderDtParamedicTeam> list1 = BusinessLayer.GetvTestOrderDtParamedicTeamList(string.Format("VisitID = {0} AND ID IN ({1}) AND IsDeleted = 0 ORDER BY ID", AppSession.RegisteredPatient.VisitID, recordID));
                if (list1.Count > 0)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientSurgeryTeamDao entityDtDao = new PatientSurgeryTeamDao(ctx);

                    try
                    {
                        foreach (vTestOrderDtParamedicTeam item in list1)
                        {
                            PatientSurgeryTeam entity = new PatientSurgeryTeam();
                            entity.PatientSurgeryID = Convert.ToInt32(hdnLookupSurgeryReportID.Value);
                            entity.ParamedicID = item.ParamedicID;
                            entity.GCParamedicRole = item.GCParamedicRole;
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(entity);
                        }

                        ctx.CommitTransaction   ();
                        result = string.Format("1|{0}",string.Empty);
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        result = string.Format("0|{0}",ex.Message);
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
            }
            else
            {
                result = "0|Record ID untuk jenis tindakan operasi tidak boleh kosong";
            }
            return result;
        }
    }
}