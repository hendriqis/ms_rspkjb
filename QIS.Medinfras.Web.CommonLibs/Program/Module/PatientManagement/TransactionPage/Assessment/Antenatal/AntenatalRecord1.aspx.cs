﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AntenatalRecord1 : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_ANTENATAL_RECORD;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_ANTENATAL_RECORD;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.ANTENATAL_RECORD;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ANTENATAL_RECORD;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.ANTENATAL_RECORD;
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.ANTENATAL_RECORD;
                    default: return Constant.MenuCode.EmergencyCare.ANTENATAL_RECORD;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string filterExpression = string.Format("MRN = {0} AND IsBorn = 0 ORDER BY ID DESC", AppSession.RegisteredPatient.MRN);
            vAntenatalRecord oAntenatal = BusinessLayer.GetvAntenatalRecordList(filterExpression).FirstOrDefault();
            if (oAntenatal != null)
            {
                hdnID.Value = oAntenatal.ID.ToString();
                EntityToControl(oAntenatal);
            }
            else
            {
                hdnID.Value = "0";
                txtPregnancyNo.Text = "1";
                txtLMP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEDB.Text = DateTime.Now.AddDays(280).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtGravida.Text = "0";
                txtPara.Text = "0";
                txtAbortion.Text = "0";
                txtLife.Text = "0";
                txtMenstrualHistory.Text = "";
                txtMedicalHistory.Text = "";
            }
        }

        protected override void SetControlProperties()
        {
            Helper.SetControlEntrySetting(txtPregnancyNo, new ControlEntrySetting(true, true,  true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtMenstrualHistory, new ControlEntrySetting(true, true, false,string.Empty), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtGravida, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtPara, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAbortion, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtLife, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtLMP, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEDB, new ControlEntrySetting(true, true, true, DateTime.Now.AddDays(280).ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpPatientStatus");
        }

        private void EntityToControl(vAntenatalRecord oAntenatal)
        {
            txtPregnancyNo.Text = oAntenatal.PregnancyNo.ToString();
            txtLMP.Text = oAntenatal.LMP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEDB.Text = oAntenatal.EDB.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtGravida.Text = oAntenatal.Gravida.ToString();
            txtPara.Text = oAntenatal.Para.ToString();
            txtAbortion.Text = oAntenatal.Abortion.ToString();
            txtLife.Text = oAntenatal.Life.ToString();
            txtMenstrualHistory.Text = oAntenatal.MenstrualHistory;
            txtMedicalHistory.Text = oAntenatal.MedicalHistory;
        }

        private void ControlToEntity(Antenatal oAntenatal)
        {
            oAntenatal.PregnancyNo = Convert.ToInt16(txtPregnancyNo.Text);
            oAntenatal.LMP = Helper.GetDatePickerValue(txtLMP);
            oAntenatal.EDB = Helper.GetDatePickerValue(txtEDB);
            oAntenatal.Gravida = Convert.ToInt16(txtGravida.Text);
            oAntenatal.Para = Convert.ToInt16(txtPara.Text);
            oAntenatal.Abortion = Convert.ToInt16(txtAbortion.Text);
            oAntenatal.Life = Convert.ToInt16(txtLife.Text);
            oAntenatal.MenstrualHistory =  txtMenstrualHistory.Text;
            oAntenatal.MedicalHistory = txtMedicalHistory.Text;
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                if (hdnID.Value != "0")
                {
                    bool result = true;
                    IDbContext ctx = DbFactory.Configure(true);
                    AntenatalDao antenatalDao = new AntenatalDao(ctx);
                    try
                    {
                        Antenatal entity = antenatalDao.Get(Convert.ToInt32(hdnID.Value));
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        antenatalDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        message = ex.Message;
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    return result;
                }
                else
                {
                    bool result = true;
                    IDbContext ctx = DbFactory.Configure(true);
                    AntenatalDao antenatalDao = new AntenatalDao(ctx); try
                    {
                        Antenatal entity = new Antenatal();
                        ControlToEntity(entity);
                        entity.MRN = AppSession.RegisteredPatient.MRN;
                        entity.FirstVisitID = AppSession.RegisteredPatient.VisitID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        antenatalDao.Insert(entity);
                        hdnID.Value = BusinessLayer.GetAntenatalMaxID(ctx).ToString();
                        message = hdnID.Value;
                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        message = ex.Message;
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    return result;
                }
            }
            return true;
        }
    }
}
