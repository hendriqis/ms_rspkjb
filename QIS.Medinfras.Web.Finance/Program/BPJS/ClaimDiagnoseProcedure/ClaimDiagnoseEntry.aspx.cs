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
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ClaimDiagnoseEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.DIAGNOSE_PROCEDURE_CLAIM;
        }

        #region List
        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(1);

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            ParamedicMaster entity = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);
            hdnDefaultParamedicCode.Value = entity.ParamedicCode;
            hdnDefaultParamedicName.Value = entity.FullName;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            hdnIsMainDiagnosisExists.Value = lstEntity.Where(a => a.GCDiagnoseTypeClaim == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList().Count() > 0 ? "1" : "0";
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
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            txtClaimDiagnosisDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtClaimDiagnosisTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboClaimStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDiagnoseTypeClaim, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected string GetDefaultDiagnosisType()
        {
            return Constant.DiagnoseType.MAIN_DIAGNOSIS;
        }

        protected string GetDefaultDifferentialDiagnosisStatus()
        {
            return Constant.DifferentialDiagnosisStatus.CONFIRMED;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboDiagnoseTypeClaim, new ControlEntrySetting(true, true, true));

            if (hdnIsMainDiagnosisExists.Value == "0")
            {
                cboDiagnoseTypeClaim.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS;
            }
            else
            {
                cboDiagnoseTypeClaim.Value = Constant.DiagnoseType.COMPLICATION;
            }

            SetControlEntrySetting(txtClaimDiagnosisDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClaimDiagnosisTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClaimDiagnosisCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClaimDiagnosisName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtv5DiagnosaID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtv5DiagnosaName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtv6DiagnosaID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtv6DiagnosaName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboClaimStatus, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            entity.GCDiagnoseTypeClaim = cboDiagnoseTypeClaim.Value.ToString();

            entity.ClaimDiagnosisBy = AppSession.UserLogin.UserID;
            entity.ClaimDiagnosisDate = Helper.GetDatePickerValue(txtClaimDiagnosisDate);
            entity.ClaimDiagnosisTime = txtClaimDiagnosisTime.Text;
            entity.ClaimDiagnosisID = txtClaimDiagnosisCode.Text;
            entity.ClaimDiagnosisText = txtClaimDiagnosisText.Text;
            entity.ClaimINADiagnoseID = txtv6DiagnosaID.Text;
            entity.ClaimINADiagnoseText = txtv6DiagnosaName.Text;
        }

        private bool IsValidToSave(ref string errMessage, bool IsAddMode)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCDiagnoseTypeClaim = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS);
            if (!String.IsNullOrEmpty(hdnEntryID.Value))
            {
                filterExpression += string.Format(" AND ID != '{0}'", hdnEntryID.Value);
            }
            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
            if (lstEntity.Count > 0)
            {
                //Validate one episode should only have one main diagnose
                if (cboDiagnoseTypeClaim.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                {
                    errMessage = "Dalam satu episode keperawatan/kunjungan pasien hanya boleh ada 1 diagnosa utama klaim.";
                    return false;
                }
            }
            return true;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao oPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
            try
            {
                if (!IsValidToSave(ref errMessage, true))
                {
                    result = false;
                }

                if (result)
                {
                    PatientDiagnosis entity = new PatientDiagnosis();
                    ControlToEntity(entity);
                    entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                    entity.IsIgnorePhysicianDiagnose = true;
                    entity.GCDiagnoseType = entity.GCDiagnoseTypeClaim;
                    entity.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                    entity.VisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    oPatientDiagnosisDao.Insert(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao oPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
            try
            {
                if (!IsValidToSave(ref errMessage, true))
                {
                    result = false;
                }

                if (result)
                {
                    PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnEntryID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Update(entity);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao oPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
            try
            {
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnEntryID.Value));
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(entity.DiagnoseID) && string.IsNullOrEmpty(entity.FinalDiagnosisID) && string.IsNullOrEmpty(entity.DiagnosisText) && string.IsNullOrEmpty(entity.FinalDiagnosisText))
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Update(entity);
                    }
                    else
                    {
                        entity.GCDiagnoseTypeClaim = null;
                        entity.ClaimDiagnosisID = null;
                        entity.ClaimDiagnosisText = null;
                        entity.ClaimINADiagnoseID = null;
                        entity.ClaimINADiagnoseText = null;
                        entity.ClaimDiagnosisBy = null;
                        entity.ClaimDiagnosisDate = null;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Update(entity);
                    }


                }
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
        #endregion
    }
}