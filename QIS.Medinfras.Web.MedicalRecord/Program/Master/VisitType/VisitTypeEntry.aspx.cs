using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class VisitTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.VISIT_TYPE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                int ID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ID.ToString();
                SetControlProperties();
                VisitType entity = BusinessLayer.GetVisitType(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listAdmission = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.ADMISSION_TYPE));
            listAdmission.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboAdmission, listAdmission, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboAdmission, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsSick, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(VisitType entity)
        {
            txtVisitTypeCode.Text = entity.VisitTypeCode;
            txtVisitTypeName.Text = entity.VisitTypeName;
            cboAdmission.Value = entity.GCAdmissionType;
            chkIsSick.Checked = entity.IsSickVisit;
        }

        private void ControlToEntity(VisitType entity)
        {
            entity.VisitTypeCode= txtVisitTypeCode.Text;
            entity.VisitTypeName = txtVisitTypeName.Text;
            entity.GCAdmissionType = cboAdmission.Value.ToString();
            entity.IsSickVisit = chkIsSick.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("VisitTypeCode = '{0}'", txtVisitTypeCode.Text);
            List<VisitType> lst = BusinessLayer.GetVisitTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Visit Type Group with Code " + txtVisitTypeCode.Text + " is already exist!";
            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            VisitTypeDao entityDao = new VisitTypeDao(ctx);
            bool result = false;
            try
            {
                VisitType entity = new VisitType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetVisitTypeMaxID(ctx).ToString();
                ctx.CommitTransaction();
                BridgingToMedinfrasMobileApps(entity, Convert.ToInt32(retval), "001");
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
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
                VisitType entity = BusinessLayer.GetVisitType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVisitType(entity);
                BridgingToMedinfrasMobileApps(entity, entity.VisitTypeID, "002");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void BridgingToMedinfrasMobileApps(VisitType oVisitType, int visitTypeID, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                if (oVisitType != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnVisitTypeMasterChanged(oVisitType, visitTypeID, eventType);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}