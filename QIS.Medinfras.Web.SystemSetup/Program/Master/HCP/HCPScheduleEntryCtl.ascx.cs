using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class HCPScheduleEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParamedicID.Value = par[0];
            hdnHealthcareServiceUnitID.Value = par[1];

            vServiceUnitParamedic entity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value))[0];
            txtParamedicName.Text = entity.ParamedicName;
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnit.Text = entity.ServiceUnitName;

            List<CSchedule> listSchedule = GetListSchedule();
            List<OperationalTime> lstOperationalTimeType = BusinessLayer.GetOperationalTimeList("IsDeleted = 0");
            lstOperationalTimeType.Insert(0, new OperationalTime { OperationalTimeID = 0 });
            foreach (CSchedule schedule in listSchedule)
            {
                schedule.chk.Checked = false;
                schedule.ddl.Enabled = false;
                Methods.SetComboBoxField<OperationalTime>(schedule.ddl, lstOperationalTimeType, "Time", "OperationalTimeID");
                schedule.ddl.SelectedIndex = 0;
                schedule.ddl.Attributes.Add("validationgroup", "mpEntryPopup");
            }

            List<ParamedicSchedule> listParamedicSchedule = BusinessLayer.GetParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value));
            foreach (ParamedicSchedule paramedicSchedule in listParamedicSchedule)
            {
                CSchedule schedule = (CSchedule)listSchedule[paramedicSchedule.DayNumber - 1];
                schedule.chk.Checked = true;
                schedule.ddl.Enabled = true;
                schedule.ddl.SelectedValue = paramedicSchedule.OperationalTimeID.ToString();
                Helper.AddCssClass(schedule.ddl, "required");
            }
        }

        public class CSchedule
        {
            public DropDownList ddl { get; set; }
            public CheckBox chk { get; set; }
        }

        private List<CSchedule> GetListSchedule()
        {
            List<CSchedule> listOperationalTime = new List<CSchedule>();

            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime1, chk = chkOperationalTime1 });
            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime2, chk = chkOperationalTime2 });
            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime3, chk = chkOperationalTime3 });
            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime4, chk = chkOperationalTime4 });
            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime5, chk = chkOperationalTime5 });
            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime6, chk = chkOperationalTime6 });
            listOperationalTime.Add(new CSchedule { ddl = ddlOperationalTime7, chk = chkOperationalTime7 });

            return listOperationalTime;
        }

        protected void cbpScheduleProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (OnSaveRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDao entityDao = new ParamedicScheduleDao(ctx);
            try
            {
                int HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                int ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                List<ParamedicSchedule> listParamedicSchedule = BusinessLayer.GetParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", HealthcareServiceUnitID, ParamedicID), ctx);
                byte dayNumber = 1;
                List<CSchedule> listSchedule = GetListSchedule();
                foreach (CSchedule schedule in listSchedule)
                {
                    string value = Request.Form[schedule.ddl.UniqueID];
                    if (schedule.chk.Checked && value != "0")
                    {
                        ParamedicSchedule entity = listParamedicSchedule.FirstOrDefault(p => p.DayNumber == dayNumber);
                        if (entity == null)
                        {
                            entity = new ParamedicSchedule();
                            entity.HealthcareServiceUnitID = HealthcareServiceUnitID;
                            entity.ParamedicID = ParamedicID;
                            entity.DayNumber = dayNumber;
                            entity.OperationalTimeID = Convert.ToInt32(value);
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entity);
                        }
                        else
                        {
                            entity = entityDao.Get(HealthcareServiceUnitID, ParamedicID, dayNumber);
                            entity.OperationalTimeID = Convert.ToInt32(Request.Form[schedule.ddl.UniqueID]);
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }
                    }
                    else
                    {
                        ParamedicSchedule entity = listParamedicSchedule.FirstOrDefault(p => p.DayNumber == dayNumber);
                        if (entity != null)
                        {
                            entityDao.Delete(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.DayNumber);
                        }
                    }
                    dayNumber++;
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}