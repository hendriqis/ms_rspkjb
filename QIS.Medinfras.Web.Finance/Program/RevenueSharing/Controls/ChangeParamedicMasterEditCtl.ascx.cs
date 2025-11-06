using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangeParamedicMasterEditCtl : BaseProcessPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnParamedicID.Value = param;

            GetSettingParameter();
            SetControlProperties();

            vParamedicMaster entity = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", hdnParamedicID.Value)).FirstOrDefault();
            EntityToControl(entity);

        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format(
                                    "ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
                                    Constant.StandardCode.TITLE,
                                    Constant.StandardCode.SUFFIX,
                                    Constant.StandardCode.GENDER)
                                );
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(false, false, true));

            #region Personal Data
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInitial, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true));
            #endregion
        }
        
        private void EntityToControl(vParamedicMaster entity)
        {
            txtParamedicCode.Text = entity.ParamedicCode;

            #region Personal Data
            cboTitle.Value = entity.GCTitle;
            txtFirstName.Text = entity.FirstName;
            txtMiddleName.Text = entity.MiddleName;
            txtFamilyName.Text = entity.LastName;
            cboSuffix.Value = entity.GCSuffix;
            txtInitial.Text = entity.Initial;
            cboGender.Value = entity.GCGender;
            #endregion
        }

        private void ControlToEntity(ParamedicMaster entity)
        {
            #region Personal Data
            if (cboTitle.Value != null && cboTitle.Value.ToString() != "")
                entity.GCTitle = cboTitle.Value.ToString();
            else
                entity.GCTitle = null;

            entity.FirstName = txtFirstName.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.LastName = txtFamilyName.Text;
            if (cboSuffix.Value != null && cboSuffix.Value.ToString() != "")
                entity.GCSuffix = cboSuffix.Value.ToString();
            else
                entity.GCSuffix = null;

            entity.Initial = txtInitial.Text;
            if (cboGender.Value != null && cboGender.Value.ToString() != "")
                entity.GCGender = cboGender.Value.ToString();
            else
                entity.GCGender = null;

            string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
            string title = cboTitle.Value == null ? "" : cboTitle.Text;
            entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            entity.FullName = Helper.GenerateFullName(entity.Name, title, suffix);
            #endregion
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicMasterDao entityDao = new ParamedicMasterDao(ctx);
            try
            {
                if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                {
                    ParamedicMaster entity = entityDao.Get(Convert.ToInt32(hdnParamedicID.Value));
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();

                    BridgingToMedinfrasMobileApps(entity, entity.ParamedicID, "002");
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, data master Dokter/Paramedis tidak ditemukan.";
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

        private void BridgingToMedinfrasMobileApps(ParamedicMaster oParamedicMaster, int paramedicID, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (oParamedicMaster != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnParamedicMasterChanged(oParamedicMaster, paramedicID, eventType);
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