using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientReferralList : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_REFERRAL;
        }

        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientReferralRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientReferral> lstView = BusinessLayer.GetvPatientReferralList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdView.DataSource = lstView;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            if (hdnIsResponse.Value == "True")
            {
                errMessage = "Maaf, Permintaan sudah diresponse oleh Dokter yang dirujuk, tidak bisa diubah lagi.";
                return false;
            }
            else if (Convert.ToInt32(hdnFromPhysicianID.Value) != AppSession.UserLogin.ParamedicID)
            {
                errMessage = "Maaf, Anda tidak bisa mengubah Catatan Permintaan Konsultasi/Rawat Bersama Dokter lain.";
                return false;
            }
            return true;
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            if (hdnIsResponse.Value == "True")
            {
                errMessage = "Maaf, Permintaan sudah diresponse oleh Dokter yang dirujuk, tidak bisa diubah lagi.";
                return false;
            }
            else if (Convert.ToInt32(hdnFromPhysicianID.Value) != AppSession.UserLogin.ParamedicID)
            {
                errMessage = "Maaf, Anda tidak bisa menghapus Catatan Permintaan Konsultasi/Rawat Bersama Dokter lain.";
                return false;
            }
            return true;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/Planning/Referral/PatientReferralEntryCtl.ascx");
            queryString = "";
            popupWidth = 800;
            popupHeight = 400;
            popupHeaderText = string.Format("Konsultasi / Rawat Bersama : {0} (MRN : {1}, REG : {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/Planning/Referral/PatientReferralEntryCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 800;
                popupHeight = 400;
                popupHeaderText = string.Format("Konsultasi / Rawat Bersama : {0} (MRN : {1}, REG : {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);

                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao referralDao = new PatientReferralDao(ctx);
            ConsultVisitDao cvDao = new ConsultVisitDao(ctx);
            ParamedicTeamDao teamDao = new ParamedicTeamDao(ctx);
            try
            {
                if (hdnID.Value != "")
                {
                    PatientReferral entity = referralDao.Get(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    referralDao.Update(entity);

                    if (entity.GCRefferalType == Constant.ReferralType.KONSULTASI || entity.GCRefferalType == Constant.ReferralType.RAWAT_BERSAMA)
                    {
                        List<ParamedicTeam> entityTeam = BusinessLayer.GetParamedicTeamList(string.Format(
                                                            "RegistrationID = {0} AND ParamedicID = {1} AND GCParamedicRole = '{2}' AND IsDeleted = 0",
                                                            AppSession.RegisteredPatient.RegistrationID, entity.ToPhysicianID, Constant.ParamedicRole.KONSULEN), ctx);
                        if (entityTeam.Count > 0)
                        {
                            ParamedicTeam obj = teamDao.Get(entityTeam.FirstOrDefault().ID);
                            obj.IsDeleted = true;
                            obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            teamDao.Update(obj);
                        }
                    }
                }

                ctx.CommitTransaction();
                return result;
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