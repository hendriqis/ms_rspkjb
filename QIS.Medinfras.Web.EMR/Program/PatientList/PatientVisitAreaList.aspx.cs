using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientVisitAreaList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_IN_MY_AREA;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected String GetServiceUnitUserRoleFilterParameter()
        {
            return String.Format("{0};{1}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            //List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID IN ('{0}','{1}') AND IsActive = 1", Constant.Facility.EMERGENCY, Constant.Facility.INPATIENT));
            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID IN ('{0}') AND IsActive = 1", Constant.Facility.EMERGENCY));
            Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
            cboPatientFrom.ReadOnly = true;
            cboPatientFrom.SelectedIndex = 0;
            txtRealisationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
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

        public String GetFilterExpression()
        {
            string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}','{4}')",
                                                            Constant.VisitStatus.CANCELLED,
                                                            Constant.VisitStatus.CLOSED,
                                                            Constant.VisitStatus.OPEN,
                                                            Constant.VisitStatus.PHYSICIAN_DISCHARGE,
                                                            Constant.VisitStatus.DISCHARGED
                                                        );

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else
            {
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, cboPatientFrom.Value.ToString(), "");
                string lstHealtcareServiceUnitID = String.Join(",", lstServiceUnit.Select(p => p.HealthcareServiceUnitID));

                filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0})", lstHealtcareServiceUnitID);
            }

            if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);

            if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit16RowCountByFieldName(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            string orderExpression = "RegistrationNo ASC";
            if (cboPatientFrom.Value.ToString() == Constant.Facility.EMERGENCY)
            {
                orderExpression = "RegistrationNo DESC";
            }

            List<vConsultVisit16> lstEntity = BusinessLayer.GetvConsultVisit16List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, orderExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit16 entity = e.Item.DataItem as vConsultVisit16;
                HtmlTableCell tdWaitingTime = e.Item.FindControl("tdWaitingTime") as HtmlTableCell;
                if (entity.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    if (!String.IsNullOrEmpty(entity.TriageColor))
                    {
                        tdWaitingTime.Style.Add("background-color", entity.TriageColor);
                    }
                }
                tdWaitingTime.InnerHtml = string.Format("{0}", entity.cfWaitingTime);

                //if (entity.GCVisitStatus == Constant.VisitStatus.RECEIVING_TREATMENT)
                //{
                //    HtmlGenericControl divChiefComplaint = e.Item.FindControl("divChiefComplaint") as HtmlGenericControl;
                //    vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
                //    divChiefComplaint.InnerHtml = oChiefComplaint != null ? "X" : "";

                //    HtmlGenericControl divDiagnosis = e.Item.FindControl("divDiagnosis") as HtmlGenericControl;
                //    vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
                //    divDiagnosis.InnerHtml = oDiagnosis != null ? "X" : "";
                //}

                if (entity.DepartmentID != Constant.Facility.INPATIENT)
                {
                    HtmlTableCell tdServiceFlag = e.Item.FindControl("tdServiceFlag") as HtmlTableCell;
                    if (tdServiceFlag != null)
                    {
                        if (entity.GCVisitStatus != Constant.VisitStatus.OPEN && entity.GCVisitStatus != Constant.VisitStatus.CHECKED_IN)
                        {
                            tdServiceFlag.Style.Add("background-color", "#192a56");
                        }
                    }
                }

                if (entity.DepartmentID != Constant.Facility.EMERGENCY)
                {
                    HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                    imgPatientSatisfactionLevelImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/PatientStatus_{0}.png", entity.PatientSatisfactionLevel));
                }
            }
        }


        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionNo.Value))[0];
                int paramedicID = entity.ParamedicID;

                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.GCGender = entity.GCGender;
                pt.RegistrationID = entity.RegistrationID;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.ParamedicID = Convert.ToInt32(paramedicID);
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.StartServiceDate = entity.StartServiceDate;
                pt.StartServiceTime = entity.StartServiceTime;
                pt.GCCustomerType = entity.GCCustomerType;
                pt.BusinessPartnerID = entity.BusinessPartnerID;

                AppSession.RegisteredPatient = pt;

                Response.Redirect("~/Program/PatientPage/PatientDataView.aspx");
            }
        }

        protected void cbpPatientTransfer_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(hdnTransactionNo.Value);
            result = "transfer" + "|" + hdnTransactionNo.Value + "|";

            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            PatientTransferHistoryDao transferHistoryDao = new PatientTransferHistoryDao(ctx);
            ParamedicMasterDao paramedicDao = new ParamedicMasterDao(ctx);
            RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);

            try
            {
                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionNo.Value), ctx).FirstOrDefault();
                int paramedicID = entity.ParamedicID;

                //Update Registered Physician and Visit Status : Receiving Treatment
                ConsultVisit oVisit = consultVisitDao.Get(entity.VisitID);
                oVisit.ParamedicID = AppSession.UserLogin.ParamedicID;
                if (oVisit.GCVisitStatus == Constant.VisitStatus.OPEN || oVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
                {
                    oVisit.StartServiceDate = DateTime.Now.Date;
                    oVisit.StartServiceTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                    oVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                }
                oVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                consultVisitDao.Update(oVisit);

                //Create Patient Take-over history
                PatientTransferHistory oHistory = new PatientTransferHistory();
                oHistory.VisitID = entity.VisitID;
                oHistory.FromParamedicID = entity.ParamedicID;
                oHistory.ToParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                oHistory.GCTransferReason = Constant.ERPatientTransferStatus.PATIENT_IN_MY_AREA;
                oHistory.OtherReasonText = string.Format("Picks from Patient in My Area List on {0}", DateTime.Now.ToString());
                oHistory.CreatedBy = AppSession.UserLogin.UserID;
                oHistory.CreatedDate = DateTime.Now;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                transferHistoryDao.Insert(oHistory);

                RegistrationBPJS regBPJS = entityRegistrationBPJSDao.Get(entity.RegistrationID);
                if (regBPJS != null)
                {
                    ParamedicMaster entityParamedic = paramedicDao.Get(Convert.ToInt32(oVisit.ParamedicID));
                    if (entityParamedic != null)
                    {
                        if (entityParamedic.BPJSReferenceInfo != null && entityParamedic.BPJSReferenceInfo != "")
                        {
                            string[] bpjsInfo = entityParamedic.BPJSReferenceInfo.Split(';');
                            string[] hfisInfo = bpjsInfo[1].Split('|');
                            regBPJS.KodeDPJP = hfisInfo[0];
                        }
                    }

                    regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityRegistrationBPJSDao.Update(regBPJS);
                }

                ctx.CommitTransaction();
                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }
    }
}