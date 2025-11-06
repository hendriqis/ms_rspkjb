﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class FetalMeasurementEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.FETAL_MEASUREMENT;
        }

        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<FetalMeasurementHd> oList = BusinessLayer.GetFetalMeasurementHdList(filterExpression);

            if (oList.Count > 0)
            {
                hdnID.Value = oList[0].ID.ToString();
                FetalMeasurementDt oDetail = BusinessLayer.GetFetalMeasurementDt(oList[0].ID, 1);
                EntityToControl(oList[0],oDetail);
            }
            else
            {
                txtMeasurementDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMeasurementTime.Text = AppSession.RegisteredPatient.VisitTime;
                txtPregnancyWeek.Text = CalculatePregnancyWeek();
                hdnID.Value = "0";
                txtAC.Text = "0";
                txtBPD.Text = "0";
                txtHL.Text = "0";
                txtHC.Text = "0";
                txtFL.Text = "0";
                txtEFW.Text = "0";
                txtOFD.Text = "0";
                txtCRL.Text = "0";
                txtFHR.Text = "0";
                txtGS.Text = "0";
            }
        }

        private string CalculatePregnancyWeek()
        {
            string result = "0";
            //Get Last Menstrual Period
            string filterExpression = string.Format("MRN = {0} AND IsBorn=0 ORDER BY ID DESC", AppSession.RegisteredPatient.MRN);
            vAntenatalRecord oAntenatal = BusinessLayer.GetvAntenatalRecordList(filterExpression).FirstOrDefault();

            if (oAntenatal != null)
            {
                hdnPregnancyNo.Value = oAntenatal.PregnancyNo.ToString();
                DateTime lmpDate = oAntenatal.LMP;
                DateTime currentDate = Helper.GetDatePickerValue(txtMeasurementDate.Text);
                result = Math.Ceiling((currentDate.Subtract(lmpDate).TotalDays / 7)).ToString();
            }
            return result;
        }

        protected override void SetControlProperties()
        {
            Helper.SetControlEntrySetting(txtPregnancyWeek, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(ddlFetusNo, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtBPD, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAC, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHL, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHC, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFL, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEFW, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtOFD, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtCRL, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFHR, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtGS, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            ddlFetusNo.Items.Add(new ListItem { Text = "1", Value = "1" });
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<FetalMeasurementHd> oList = BusinessLayer.GetFetalMeasurementHdList(filterExpression);
            if (oList.Count > 1)
            {
                for (int i = 2; i < oList.Count - 1; i++)
                {
                    ddlFetusNo.Items.Add(new ListItem { Text = i.ToString(), Value = i.ToString() });
                }
            }
            ddlFetusNo.SelectedIndex = 0;
        }

        private void EntityToControl(FetalMeasurementHd oHeader, FetalMeasurementDt oDetail)
        {
            txtMeasurementDate.Text = oHeader.MeasurementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtMeasurementTime.Text = oHeader.MeasurementTime;
            hdnPregnancyNo.Value = oHeader.PregnancyNo.ToString();
            txtPregnancyWeek.Text = oHeader.PregnancyWeek.ToString();
            ddlFetusNo.SelectedValue = oDetail.FetusNo.ToString();
            txtBPD.Text = oDetail.BPD.ToString();
            txtAC.Text = oDetail.AC.ToString();
            txtHL.Text = oDetail.HL.ToString();
            txtHC.Text = oDetail.HC.ToString();
            txtFL.Text = oDetail.FL.ToString();
            txtEFW.Text = oDetail.EFW.ToString();
            txtOFD.Text = oDetail.OFD.ToString();
            txtCRL.Text = oDetail.CRL.ToString();
            txtFHR.Text = oDetail.FHR.ToString();
            txtGS.Text = oDetail.GS.ToString();
        }

        private void ControlToEntity(FetalMeasurementHd oHeader, FetalMeasurementDt oDetail)
        {
            oHeader.MeasurementDate = Helper.GetDatePickerValue(txtMeasurementDate);
            oHeader.MeasurementTime = txtMeasurementTime.Text;
            oHeader.PregnancyNo = Convert.ToInt16(hdnPregnancyNo.Value);
            oHeader.PregnancyWeek = Convert.ToInt16(txtPregnancyWeek.Text);

            oDetail.ID = Convert.ToInt32(hdnID.Value);
            oDetail.FetusNo = Convert.ToInt16(ddlFetusNo.SelectedValue);
            oDetail.AC = Convert.ToDecimal(txtAC.Text);
            oDetail.BPD = Convert.ToDecimal(txtBPD.Text);
            oDetail.HL = Convert.ToDecimal(txtHL.Text);
            oDetail.HC = Convert.ToDecimal(txtHC.Text);
            oDetail.FL = Convert.ToDecimal(txtFL.Text);
            oDetail.EFW = Convert.ToDecimal(txtEFW.Text);
            oDetail.OFD = Convert.ToDecimal(txtOFD.Text);
            oDetail.CRL = Convert.ToDecimal(txtCRL.Text);
            oDetail.FHR = Convert.ToDecimal(txtFHR.Text);
            oDetail.GS = Convert.ToDecimal(txtGS.Text);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                if (hdnID.Value == "0")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    FetalMeasurementHdDao entityDao = new FetalMeasurementHdDao(ctx);
                    FetalMeasurementDtDao entityDtDao = new FetalMeasurementDtDao(ctx);
                    try
                    {
                        FetalMeasurementHd entityHd = new FetalMeasurementHd();
                        FetalMeasurementDt entityDt = new FetalMeasurementDt();
                        ControlToEntity(entityHd, entityDt);
                        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityHd.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entityHd);

                        entityHd.ID = BusinessLayer.GetFetalMeasurementHdMaxID(ctx);

                        entityDt.ID = entityHd.ID;
                        entityDtDao.Insert(entityDt);

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        errMessage = ex.Message;
                        result = false;
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
                    IDbContext ctx = DbFactory.Configure(true);
                    FetalMeasurementHdDao entityHdDao = new FetalMeasurementHdDao(ctx);
                    FetalMeasurementDtDao entityDtDao = new FetalMeasurementDtDao(ctx);
                    try
                    {
                        FetalMeasurementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnID.Value));
                        FetalMeasurementDt entityDt = BusinessLayer.GetFetalMeasurementDt(entityHd.ID,1);
                        List<VitalSignDt> lstNewEntityDt = new List<VitalSignDt>();

                        ControlToEntity(entityHd, entityDt);
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;

                        entityHdDao.Update(entityHd);

                        entityDt.ID = entityHd.ID;
                        entityDtDao.Update(entityDt);

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        errMessage = ex.Message;
                        result = false;
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
