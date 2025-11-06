using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientDiagnosisList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.DIFFERENTIAL_DIAGNOSIS;
        }

        #region List
        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (lstEntity.Count > 0)
                grdView.SelectedIndex = 0;
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/Assesment/Diagnosis/DiagnosisEntryCtl.ascx");
            queryString = string.Format("{0}|{1}", "0", grdView.Rows.Count);
            popupWidth = 600;
            popupHeight = 420;
            popupHeaderText = "Patient Diagnosis";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/Assesment/Diagnosis/DiagnosisEntryCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 600;
                popupHeight = 420;
                popupHeaderText = "Patient Diagnosis";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            //if (hdnID.Value != "")
            //{
            //    PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnID.Value));
            //    entity.IsDeleted = true;
            //    entity.LastUpdatedDate = DateTime.Now;
            //    BusinessLayer.UpdatePatientDiagnosis(entity);
            //    return true;
            //}
            return false;
        }
        #endregion

        //#region Entry
        //protected override void SetControlProperties()
        //{
        //    txtDifferentialDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        //    txtDifferentialTime.Text = AppSession.RegisteredPatient.VisitTime;

        //    List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
        //    Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
        //    hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

        //    if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
        //    {
        //        int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
        //        cboPhysician.ClientEnabled = false;
        //        hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
        //    }

        //    String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
        //    List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
        //    Methods.SetComboBoxField<StandardCode>(cboStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
        //    Methods.SetComboBoxField<StandardCode>(cboDiagnoseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        //}

        //protected override void OnControlEntrySetting()
        //{
        //    SetControlEntrySetting(txtDifferentialDate, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(txtDifferentialTime, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(cboDiagnoseType, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(ledMorphology, new ControlEntrySetting(true, true, false));
        //    //SetControlEntrySetting(ddlMonth, new ControlEntrySetting(true, true, false));

        //    SetControlEntrySetting(cboStatus, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(chkIsFollowUp, new ControlEntrySetting(true, true, false));
        //    SetControlEntrySetting(chkIsChronic, new ControlEntrySetting(true, true, false));
        //}

        //protected override bool OnSaveAddRecord(ref string errMessage)
        //{
        //    try
        //    {
        //        PatientDiagnosis entity = new PatientDiagnosis();
        //        ControlToEntity(entity);
        //        entity.VisitID = AppSession.RegisteredPatient.VisitID;
        //        entity.CreatedBy = AppSession.UserLogin.UserID;
        //        BusinessLayer.InsertPatientDiagnosis(entity);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        errMessage = ex.Message;
        //        return false;
        //    }
        //}
        //#endregion
    }
}