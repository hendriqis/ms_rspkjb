using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class OperationalTimeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.OPERATIONAL_TIME_HOUR;
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
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                OperationalTime entity = BusinessLayer.GetOperationalTime(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtOperationalTimeCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOperationalTimeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOperationalTimeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDisplayColor, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDisplayColorPicker, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStart1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEnd1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStart2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEnd2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStart3, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEnd3, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStart4, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEnd4, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStart5, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEnd5, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(OperationalTime entity)
        {
            txtOperationalTimeCode.Text = entity.OperationalTimeCode;
            txtOperationalTimeName.Text = entity.OperationalTimeName;
            txtDisplayColor.Text = txtDisplayColorPicker.Text = entity.DisplayColor;
            txtStart1.Text = entity.StartTime1;
            txtEnd1.Text = entity.EndTime1;
            txtStart2.Text = entity.StartTime2;
            txtEnd2.Text = entity.EndTime2;
            txtStart3.Text = entity.StartTime3;
            txtEnd3.Text = entity.EndTime3;
            txtStart4.Text = entity.StartTime4;
            txtEnd4.Text = entity.EndTime4;
            txtStart5.Text = entity.StartTime5;
            txtEnd5.Text = entity.EndTime5;
        }

        private void ControlToEntity(OperationalTime entity)
        {
            entity.OperationalTimeCode = txtOperationalTimeCode.Text;
            entity.OperationalTimeName = txtOperationalTimeName.Text;
            entity.DisplayColor = txtDisplayColor.Text;
            entity.StartTime1 = txtStart1.Text;
            entity.EndTime1 = txtEnd1.Text;
            entity.StartTime2 = txtStart2.Text;
            entity.EndTime2 = txtEnd2.Text;
            entity.StartTime3 = txtStart3.Text;
            entity.EndTime3 = txtEnd3.Text;
            entity.StartTime4 = txtStart4.Text;
            entity.EndTime4 = txtEnd4.Text;
            entity.StartTime5 = txtStart5.Text;
            entity.EndTime5 = txtEnd5.Text;
        }

        protected bool checkValidateSlotTime() {
            bool result = true;
            if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text != "" && txtStart4.Text != "" && txtStart5.Text != "")
            {
                DateTime StartTime1 = DateTime.Parse("08-01-2012" + ' ' + txtStart1.Text);
                DateTime StartTime2 = DateTime.Parse("08-01-2012" + ' ' + txtStart2.Text);
                DateTime StartTime3 = DateTime.Parse("08-01-2012" + ' ' + txtStart3.Text);
                DateTime StartTime4 = DateTime.Parse("08-01-2012" + ' ' + txtStart4.Text);
                DateTime StartTime5 = DateTime.Parse("08-01-2012" + ' ' + txtStart5.Text);

                DateTime EndTime1 = DateTime.Parse("08-01-2012" + ' ' + txtEnd1.Text);
                DateTime EndTime2 = DateTime.Parse("08-01-2012" + ' ' + txtEnd2.Text);
                DateTime EndTime3 = DateTime.Parse("08-01-2012" + ' ' + txtEnd3.Text);
                DateTime EndTime4 = DateTime.Parse("08-01-2012" + ' ' + txtEnd4.Text);
                DateTime EndTime5 = DateTime.Parse("08-01-2012" + ' ' + txtEnd5.Text);


                if (EndTime1.TimeOfDay > StartTime1.TimeOfDay)
                {
                    if (StartTime2.TimeOfDay > EndTime1.TimeOfDay)
                    {
                        if (EndTime2.TimeOfDay > StartTime2.TimeOfDay)
                        {
                            if (StartTime3.TimeOfDay > EndTime2.TimeOfDay)
                            {
                                if (EndTime3.TimeOfDay > StartTime3.TimeOfDay)
                                {
                                    if (StartTime4.TimeOfDay > EndTime3.TimeOfDay)
                                    {
                                        if (EndTime4.TimeOfDay > StartTime4.TimeOfDay)
                                        {
                                            if (StartTime5.TimeOfDay > EndTime4.TimeOfDay)
                                            {
                                                if (EndTime5.TimeOfDay > StartTime5.TimeOfDay)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                result = false;
                                            }
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                       result = false;
                                    }
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text != "" && txtStart4.Text != "" && txtStart5.Text == "")
            {
                DateTime StartTime1 = DateTime.Parse("08-01-2012" + ' ' + txtStart1.Text);
                DateTime StartTime2 = DateTime.Parse("08-01-2012" + ' ' + txtStart2.Text);
                DateTime StartTime3 = DateTime.Parse("08-01-2012" + ' ' + txtStart3.Text);
                DateTime StartTime4 = DateTime.Parse("08-01-2012" + ' ' + txtStart4.Text);

                DateTime EndTime1 = DateTime.Parse("08-01-2012" + ' ' + txtEnd1.Text);
                DateTime EndTime2 = DateTime.Parse("08-01-2012" + ' ' + txtEnd2.Text);
                DateTime EndTime3 = DateTime.Parse("08-01-2012" + ' ' + txtEnd3.Text);
                DateTime EndTime4 = DateTime.Parse("08-01-2012" + ' ' + txtEnd4.Text);


                if (EndTime1.TimeOfDay > StartTime1.TimeOfDay)
                {
                    if (StartTime2.TimeOfDay > EndTime1.TimeOfDay)
                    {
                        if (EndTime2.TimeOfDay > StartTime2.TimeOfDay)
                        {
                            if (StartTime3.TimeOfDay > EndTime2.TimeOfDay)
                            {
                                if (EndTime3.TimeOfDay > StartTime3.TimeOfDay)
                                {
                                    if (StartTime4.TimeOfDay > EndTime3.TimeOfDay)
                                    {
                                        if (EndTime4.TimeOfDay > StartTime4.TimeOfDay)
                                        {
                                            result = true;
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text != "" && txtStart4.Text == "" && txtStart5.Text == "")
            {
                DateTime StartTime1 = DateTime.Parse("08-01-2012" + ' ' + txtStart1.Text);
                DateTime StartTime2 = DateTime.Parse("08-01-2012" + ' ' + txtStart2.Text);
                DateTime StartTime3 = DateTime.Parse("08-01-2012" + ' ' + txtStart3.Text);

                DateTime EndTime1 = DateTime.Parse("08-01-2012" + ' ' + txtEnd1.Text);
                DateTime EndTime2 = DateTime.Parse("08-01-2012" + ' ' + txtEnd2.Text);
                DateTime EndTime3 = DateTime.Parse("08-01-2012" + ' ' + txtEnd3.Text);

                if (EndTime1.TimeOfDay > StartTime1.TimeOfDay)
                {
                    if (StartTime2.TimeOfDay > EndTime1.TimeOfDay)
                    {
                        if (EndTime2.TimeOfDay > StartTime2.TimeOfDay)
                        {
                            if (StartTime3.TimeOfDay > EndTime2.TimeOfDay)
                            {
                                if (EndTime3.TimeOfDay > StartTime3.TimeOfDay)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text == "" && txtStart4.Text == "" && txtStart5.Text == "")
            {
                DateTime StartTime1 = DateTime.Parse("08-01-2012" + ' ' + txtStart1.Text);
                DateTime StartTime2 = DateTime.Parse("08-01-2012" + ' ' + txtStart2.Text);

                DateTime EndTime1 = DateTime.Parse("08-01-2012" + ' ' + txtEnd1.Text);
                DateTime EndTime2 = DateTime.Parse("08-01-2012" + ' ' + txtEnd2.Text);

                if (EndTime1.TimeOfDay > StartTime1.TimeOfDay)
                {
                    if (StartTime2.TimeOfDay > EndTime1.TimeOfDay)
                    {
                        if (EndTime2.TimeOfDay > StartTime2.TimeOfDay)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else if (txtStart1.Text != "" && txtStart2.Text == "" && txtStart3.Text == "" && txtStart4.Text == "" && txtStart5.Text == "")
            {
                DateTime StartTime1 = DateTime.Parse("08-01-2012" + ' ' + txtStart1.Text);

                DateTime EndTime1 = DateTime.Parse("08-01-2012" + ' ' + txtEnd1.Text);

                if (EndTime1.TimeOfDay > StartTime1.TimeOfDay)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("OperationalTimeCode = '{0}'", txtOperationalTimeCode.Text);
            List<OperationalTime> lst = BusinessLayer.GetOperationalTimeList(FilterExpression);

            if (lst.Count > 0)
            {
                errMessage = " Operational Time with Code " + txtOperationalTimeCode.Text + " is already exist!";
            }

            if (!checkValidateSlotTime())
                errMessage = "Operational Time Overlap Between Session";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("OperationalTimeCode = '{0}' AND OperationalTimeID != {1}", txtOperationalTimeCode.Text, ID);
            List<OperationalTime> lst = BusinessLayer.GetOperationalTimeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Operational Time with Code " + txtOperationalTimeCode.Text + " is already exist!";

            if (!checkValidateSlotTime())
                errMessage = "Operational Time Overlap Between Session";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            OperationalTimeDao entityDao = new OperationalTimeDao(ctx);
            bool result = false;
            try
            {
                OperationalTime entity = new OperationalTime();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetOperationalTimeMaxID(ctx).ToString();
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
                OperationalTime entity = BusinessLayer.GetOperationalTime(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateOperationalTime(entity);
                BridgingToMedinfrasMobileApps(entity, entity.OperationalTimeID, "002");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void BridgingToMedinfrasMobileApps(OperationalTime oOperationalTime, int operationalTimeID, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (oOperationalTime != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnOperationalTimeMasterChanged(oOperationalTime, operationalTimeID, eventType);
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