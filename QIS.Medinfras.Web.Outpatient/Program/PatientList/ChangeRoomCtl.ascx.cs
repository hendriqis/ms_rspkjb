using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class ChangeRoomCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnVisitDate.Value = Helper.GetDatePickerValue(lstParam[2]).ToString(Constant.FormatString.DATE_FORMAT);
            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", lstParam[0])).FirstOrDefault();
            if (entityParamedic != null)
            {
                hdnPhysicianID.Value = entityParamedic.ParamedicID.ToString();
                txtPhysicianCode.Text = entityParamedic.ParamedicCode;
                txtPhysicianName.Text = entityParamedic.FullName;
            }
            vHealthcareServiceUnit entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", lstParam[1])).FirstOrDefault();
            if (entityHSU != null)
            {
                hdnHealthcareServiceUnitID.Value = entityHSU.HealthcareServiceUnitID.ToString();
                txtServiceUnitCode.Text = entityHSU.ServiceUnitCode;
                txtServiceUnitName.Text = entityHSU.ServiceUnitName;
            }
            IsAdd = false;

        }

        private void GetSettingParameter()
        {
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool isValid = true;
            bool result = true;

            if (isValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);

                try
                {
                    string filterExpression = string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND VisitDate = '{2}' AND GCVisitStatus = '{3}'", hdnPhysicianID.Value, hdnHealthcareServiceUnitID.Value, hdnVisitDate.Value, Constant.VisitStatus.OPEN);
                    List<ConsultVisit> lstEntity = BusinessLayer.GetConsultVisitList(filterExpression);
                    if (lstEntity.Count > 0)
                    {
                        foreach (ConsultVisit a in lstEntity)
                        {
                            a.RoomID = Convert.ToInt32(hdnRoomID.Value);
                            entityConsultVisitDao.Update(a);
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
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected string GetGenderFemale()
        {
            return Constant.Gender.FEMALE;
        }
    }
}