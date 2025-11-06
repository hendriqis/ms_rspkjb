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
    public partial class ChiefComplaintEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.CHIEF_COMPLAINT;
        }

        protected override void InitializeDataControl()
        {
            Helper.SetControlEntrySetting(txtChiefComplaint, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtLocation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboOnset, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtOnset, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboProvocation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtProvocation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboQuality, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtQuality, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboSeverity, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtSeverity, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboTime, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtTime, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRelievedBy, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRelievedBy, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkAutoAnamnesis, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkAlloAnamnesis, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");

            LoadEntity();
        }

        private void LoadEntity()
        {
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            List<ChiefComplaint> lstChiefComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = '{0}'", AppSession.RegisteredPatient.VisitID));

            if (entityVisit.StartServiceDate == null || entityVisit.StartServiceTime == "")
            {
                txtServiceDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
            else
            {
                txtServiceDate.Text = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = entityVisit.StartServiceTime;
            }

            hdnDepartmentID.Value = entityVisit.DepartmentID;

            tblRecordStatus.Visible = lstChiefComplaint.Count > 0;

            if (lstChiefComplaint.Count > 0)
            {
                ChiefComplaint entity = lstChiefComplaint.FirstOrDefault();
                hdnID.Value = entity.ID.ToString();
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ONSET, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.RELIEVED_BY, Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            if (hdnID.Value == "")
            {
                cboOnset.SelectedIndex = cboOnset.Items.Count - 1;
                cboProvocation.SelectedIndex = cboProvocation.Items.Count - 1;
                cboQuality.SelectedIndex = cboQuality.Items.Count - 1;
                cboSeverity.SelectedIndex = cboSeverity.Items.Count - 1;
                cboTime.SelectedIndex = cboTime.Items.Count - 1;
                cboRelievedBy.SelectedIndex = cboRelievedBy.Items.Count - 1;
            }
        }

        private void EntityToControl(ChiefComplaint entity)
        {
            txtChiefComplaint.Text = entity.ChiefComplaintText;
            chkAutoAnamnesis.Checked = entity.IsAutoAnamnesis;
            chkAlloAnamnesis.Checked = entity.IsAlloAnamnesis;
            txtLocation.Text = entity.Location;
            cboOnset.Value = entity.GCOnset;
            cboProvocation.Value = entity.GCProvocation;
            cboQuality.Value = entity.GCQuality;
            cboSeverity.Value = entity.GCSeverity;
            cboTime.Value = entity.GCCourse;
            cboRelievedBy.Value = entity.GCRelieved;
            txtOnset.Text = entity.Onset;
            txtProvocation.Text = entity.Provocation;
            txtQuality.Text = entity.Quality;
            txtSeverity.Text = entity.Severity;
            txtTime.Text = entity.CourseTiming;
            txtRelievedBy.Text = entity.RelievedBy;

            UpdateRecordStatus(entity);
        }

        private void UpdateRecordStatus(ChiefComplaint entity)
        {
            if (entity.LastUpdatedBy != null)
            {
                vUser userInfo = BusinessLayer.GetvUserList(string.Format("UserID = {0}", entity.LastUpdatedBy)).FirstOrDefault();
                if (userInfo != null)
                {
                    lblLastUpdatedDate.InnerText = string.Format("{0} {1}",entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT), entity.LastUpdatedDate.ToString(Constant.FormatString.TIME_FORMAT));
                    lblLastUpdatedBy.InnerText = userInfo.FullName;
                }
            }
            else
            {
                vUser userInfo = BusinessLayer.GetvUserList(string.Format("UserID = {0}", entity.CreatedBy)).FirstOrDefault();
                if (userInfo != null)
                {
                    lblLastUpdatedDate.InnerText = string.Format("{0} {1}", entity.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT), entity.CreatedDate.ToString(Constant.FormatString.TIME_FORMAT));
                    lblLastUpdatedBy.InnerText = userInfo.FullName;
                }
            }
        }

        private void ControlToEntity(ChiefComplaint entity)
        {
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.Location = txtLocation.Text;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);

            if (cboQuality.Value == null)
                entity.GCQuality = "";
            else
                entity.GCQuality = cboQuality.Value.ToString();
            if (cboOnset.Value == null)
                entity.GCOnset = "";
            else
                entity.GCOnset = cboOnset.Value.ToString();
            if (cboRelievedBy.Value == null)
                entity.GCRelieved = "";
            else
                entity.GCRelieved = cboRelievedBy.Value.ToString();
            if (cboSeverity.Value == null)
                entity.GCSeverity = "";
            else
                entity.GCSeverity = cboSeverity.Value.ToString();
            if (cboProvocation.Value == null)
                entity.GCProvocation = "";
            else
                entity.GCProvocation = cboProvocation.Value.ToString();
            if (cboTime.Value == null)
                entity.GCCourse = "";
            else
                entity.GCCourse = cboTime.Value.ToString();
            entity.Quality = txtQuality.Text;
            entity.Onset = txtOnset.Text;
            entity.RelievedBy = txtRelievedBy.Text;
            entity.Severity = txtSeverity.Text;
            entity.Provocation = txtProvocation.Text;
            entity.CourseTiming = txtTime.Text;
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                if (String.IsNullOrEmpty(entityConsultVisit.StartServiceTime))
                {
                    entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityConsultVisit.StartServiceTime = txtServiceTime.Text;
                    entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));                    
                }
                entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                consultVisitDao.Update(entityConsultVisit);
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                if (hdnID.Value != "0" && hdnID.Value != "")
                {
                    bool result = true;
                    IDbContext ctx = DbFactory.Configure(true);
                    ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
                    try
                    {
                        if (string.IsNullOrEmpty(txtChiefComplaint.Text))
                        {
                            result = false;
                            message = "Chief Complaint should not be empty.";
                            ctx.Close();
                            return result;
                        }
                        ChiefComplaint entity = chiefComplaintDao.Get(Convert.ToInt32(hdnID.Value));
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        chiefComplaintDao.Update(entity);
                        message = hdnID.Value;

                        UpdateConsultVisitRegistration(ctx);
                        ctx.CommitTransaction();

                        hdnIsSaved.Value = "1";
                        UpdateRecordStatus(entity);
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        message = ex.Message;
                        hdnIsSaved.Value = "0";
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
                    ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
                    try
                    {
                        ChiefComplaint entity = new ChiefComplaint();

                        ControlToEntity(entity);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        chiefComplaintDao.Insert(entity);
                        hdnID.Value = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();
                        message = hdnID.Value;

                        UpdateConsultVisitRegistration(ctx);
                        ctx.CommitTransaction();

                        hdnIsSaved.Value = "1";

                        UpdateRecordStatus(entity);
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        message = ex.Message;
                        hdnIsSaved.Value = "0";
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadEntity();
        }
    }
}
