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
    public partial class PsychiatryStatusEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PSYCHIATRY_STATUS;
        }

        protected override void InitializeDataControl()
        {
            Helper.SetControlEntrySetting(txtGeneralAppearance, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboSpeechContact, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtSpeechContactText, new ControlEntrySetting(true, true,false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboConsciousness, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtConsciousness, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            Helper.SetControlEntrySetting(cboTimeOrientation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtTimeOrientation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboPlaceOrientation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtPlaceOrientation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboPersonOrientation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtPersonOrientation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboMoodyLevel, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtMoodyLevel, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboThoughtProcessForm, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtThoughtProcessForm, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboThoughtProcessFlow, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtThoughtProcessFlow, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboThoughtProcessContent, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtThoughtProcessContent, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            Helper.SetControlEntrySetting(cboPerception, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtPerception, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboVolition, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtVolition, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboPsychomotor, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtPsychomotor, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}",AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            List<PsychiatryStatus> lstStatus = BusinessLayer.GetPsychiatryStatusList(string.Format("VisitID = '{0}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));

            if (entityVisit.StartServiceDate == null || entityVisit.StartServiceTime == "")
            {
                txtStatusDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStatusTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
            else
            {
                txtStatusDate.Text = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStatusTime.Text = entityVisit.StartServiceTime;
            }

            hdnDepartmentID.Value = entityVisit.DepartmentID;
            
            if (lstStatus.Count > 0)
            {
                PsychiatryStatus entity = lstStatus.FirstOrDefault();
                hdnID.Value = entity.ID.ToString();
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                txtStatusDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStatusTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') AND IsActive = 1 AND IsDeleted = 0", 
                Constant.StandardCode.SPEECH_CONTACT, Constant.StandardCode.CONSCIOUSNESS, Constant.StandardCode.ORIENTATION_TIME,
                Constant.StandardCode.ORIENTATION_PLACE, Constant.StandardCode.ORIENTATION_PERSON, Constant.StandardCode.MOODY_LEVEL,
                Constant.StandardCode.THOUGHT_PROCESS_FORM, Constant.StandardCode.THOUGHT_PROCESS_FLOW, Constant.StandardCode.THOUGHT_PROCESS_CONTENT,
                Constant.StandardCode.PERCEPTION,Constant.StandardCode.VOLITION,Constant.StandardCode.PSYCHMOTOR);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboSpeechContact, lstSc.Where(p => p.ParentID == Constant.StandardCode.SPEECH_CONTACT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboConsciousness, lstSc.Where(p => p.ParentID == Constant.StandardCode.CONSCIOUSNESS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboTimeOrientation, lstSc.Where(p => p.ParentID == Constant.StandardCode.ORIENTATION_TIME || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboPlaceOrientation, lstSc.Where(p => p.ParentID == Constant.StandardCode.ORIENTATION_PLACE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboPersonOrientation, lstSc.Where(p => p.ParentID == Constant.StandardCode.ORIENTATION_PERSON || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboMoodyLevel, lstSc.Where(p => p.ParentID == Constant.StandardCode.MOODY_LEVEL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboThoughtProcessForm, lstSc.Where(p => p.ParentID == Constant.StandardCode.THOUGHT_PROCESS_FORM || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboThoughtProcessFlow, lstSc.Where(p => p.ParentID == Constant.StandardCode.THOUGHT_PROCESS_FLOW || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboThoughtProcessContent, lstSc.Where(p => p.ParentID == Constant.StandardCode.THOUGHT_PROCESS_CONTENT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboPerception, lstSc.Where(p => p.ParentID == Constant.StandardCode.PERCEPTION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboVolition, lstSc.Where(p => p.ParentID == Constant.StandardCode.VOLITION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboPsychomotor, lstSc.Where(p => p.ParentID == Constant.StandardCode.PSYCHMOTOR || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            if (hdnID.Value == "")
            {
                cboSpeechContact.SelectedIndex = cboSpeechContact.Items.Count - 1;
                cboConsciousness.SelectedIndex = cboConsciousness.Items.Count - 1;
                cboTimeOrientation.SelectedIndex = cboTimeOrientation.Items.Count - 1;
                cboPlaceOrientation.SelectedIndex = cboPlaceOrientation.Items.Count - 1;
                cboPersonOrientation.SelectedIndex = cboPersonOrientation.Items.Count - 1;
                cboMoodyLevel.SelectedIndex = cboMoodyLevel.Items.Count - 1;
                cboThoughtProcessForm.SelectedIndex = cboThoughtProcessForm.Items.Count - 1;
                cboThoughtProcessFlow.SelectedIndex = cboThoughtProcessFlow.Items.Count - 1;
                cboThoughtProcessContent.SelectedIndex = cboThoughtProcessContent.Items.Count - 1;
                cboPerception.SelectedIndex = cboPerception.Items.Count - 1;
                cboVolition.SelectedIndex = cboVolition.Items.Count - 1;
                cboPsychomotor.SelectedIndex = cboPsychomotor.Items.Count - 1; 
            }
        }

        private void EntityToControl(PsychiatryStatus entity)
        {
            txtGeneralAppearance.Text = entity.GeneralAppearance;
            cboSpeechContact.Value = entity.GCSpeechContact;
            txtSpeechContactText.Text = entity.SpeechContactText;
            cboConsciousness.Value = entity.GCConsciousness;
            txtConsciousness.Text = entity.ConsciousnessOther;
            cboTimeOrientation.Value = entity.GCOrientationTime;
            txtTimeOrientation.Text = entity.OtherOrientationTime;
            cboPlaceOrientation.Value = entity.GCOrientationPlace;
            txtPlaceOrientation.Text = entity.OtherOrientationPlace;
            cboPersonOrientation.Value = entity.GCOrientationPerson;
            txtPersonOrientation.Text = entity.OtherOrientationPerson;
            cboMoodyLevel.Value = entity.GCMoodyLevel;
            txtMoodyLevel.Text = entity.OtherMoodyLevel;
            cboThoughtProcessForm.Value = entity.GCThoughtProcessForm;
            txtThoughtProcessForm.Text = entity.OtherThoughtProcess;
            cboThoughtProcessFlow.Value = entity.GCThoughtProcessFlow;
            txtThoughtProcessFlow.Text = entity.OtherThoughtProcessFlow;
            cboThoughtProcessContent.Value = entity.GCThoughtProcessContent;
            txtThoughtProcessContent.Text = entity.OtherThoughtProcessContent;
            cboPerception.Value = entity.GCPerception;
            txtPerception.Text = entity.OtherPerception;            
            cboVolition.Text = entity.GCVolition;
            txtVolition.Text = entity.OtherVolition;
            cboPsychomotor.Text = entity.GCPsychmotor;
            txtPsychomotor.Text = entity.OtherPsychmotor;
        }

        private void ControlToEntity(PsychiatryStatus entity)
        {
            entity.GeneralAppearance = txtGeneralAppearance.Text;
           
            if (cboSpeechContact.Value == null || string.IsNullOrEmpty(cboSpeechContact.Value.ToString()))
                entity.GCSpeechContact = null;
            else
                entity.GCSpeechContact = cboSpeechContact.Value.ToString();
            entity.SpeechContactText = txtSpeechContactText.Text;

            if (cboConsciousness.Value == null || string.IsNullOrEmpty(cboConsciousness.Value.ToString()))
                entity.GCConsciousness = null;
            else
                entity.GCConsciousness = cboConsciousness.Value.ToString();
            entity.ConsciousnessOther = txtConsciousness.Text;

            if (cboTimeOrientation.Value == null || string.IsNullOrEmpty(cboTimeOrientation.Value.ToString()))
                entity.GCOrientationTime = null;
            else
                entity.GCOrientationTime = cboTimeOrientation.Value.ToString();
            entity.OtherOrientationTime = txtTimeOrientation.Text;

            if (cboPlaceOrientation.Value == null || string.IsNullOrEmpty(cboPlaceOrientation.Value.ToString()))
                entity.GCOrientationPlace = null;
            else
                entity.GCOrientationPlace = cboPlaceOrientation.Value.ToString();
            entity.OtherOrientationPlace = txtPlaceOrientation.Text;

            if (cboPersonOrientation.Value == null || string.IsNullOrEmpty(cboPersonOrientation.Value.ToString()))
                entity.GCOrientationPerson = null;
            else
                entity.GCOrientationPerson = cboPersonOrientation.Value.ToString();
            entity.OtherOrientationPerson = txtPersonOrientation.Text;

            if (cboMoodyLevel.Value == null || string.IsNullOrEmpty(cboMoodyLevel.Value.ToString()))
                entity.GCMoodyLevel = null;
            else
                entity.GCMoodyLevel = cboMoodyLevel.Value.ToString();
            entity.OtherMoodyLevel = txtMoodyLevel.Text;

            if (cboThoughtProcessForm.Value == null || string.IsNullOrEmpty(cboThoughtProcessForm.Value.ToString()))
                entity.GCThoughtProcessForm = null;
            else
                entity.GCThoughtProcessForm = cboThoughtProcessForm.Value.ToString();
            entity.OtherThoughtProcess = txtThoughtProcessForm.Text;

            if (cboThoughtProcessFlow.Value == null || string.IsNullOrEmpty(cboThoughtProcessFlow.Value.ToString()))
                entity.GCThoughtProcessFlow = null;
            else
                entity.GCThoughtProcessFlow = cboThoughtProcessFlow.Value.ToString();
            entity.OtherThoughtProcessFlow = txtThoughtProcessFlow.Text;

            if (cboThoughtProcessContent.Value == null || string.IsNullOrEmpty(cboThoughtProcessContent.Value.ToString()))
                entity.GCThoughtProcessContent = null;
            else
                entity.GCThoughtProcessContent = cboThoughtProcessContent.Value.ToString();
            entity.OtherThoughtProcessContent = txtThoughtProcessContent.Text;

            if (cboPerception.Value == null || string.IsNullOrEmpty(cboPerception.Value.ToString()))
                entity.GCPerception = null;
            else
                entity.GCPerception = cboPerception.Value.ToString();
            entity.OtherPerception = txtPerception.Text;

            if (cboVolition.Value == null || string.IsNullOrEmpty(cboVolition.Value.ToString()))
                entity.GCVolition = null;
            else
                entity.GCVolition = cboVolition.Value.ToString();
            entity.OtherVolition = txtVolition.Text;

            if (cboPsychomotor.Value == null || string.IsNullOrEmpty(cboPsychomotor.Value.ToString()))
                entity.GCPsychmotor = null;
            else
                entity.GCPsychmotor = cboPsychomotor.Value.ToString();
            entity.OtherPsychmotor = txtPsychomotor.Text;
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                if (hdnID.Value != "")
                {
                    bool result = true;
                    IDbContext ctx = DbFactory.Configure(true);
                    PsychiatryStatusDao statusDao = new PsychiatryStatusDao(ctx);
                    try
                    {
                        if (string.IsNullOrEmpty(txtGeneralAppearance.Text))
                        {
                            result = false;
                            message = "General Appearance should not be empty.";
                            ctx.Close();
                            return result;
                        }
                        PsychiatryStatus entity = statusDao.Get(Convert.ToInt32(hdnID.Value));
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        statusDao.Update(entity);
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
                    PsychiatryStatusDao statusDao = new PsychiatryStatusDao(ctx);
                    try
                    {
                        PsychiatryStatus entity = new PsychiatryStatus();

                        ControlToEntity(entity);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        statusDao.Insert(entity);
                        hdnID.Value = BusinessLayer.GetPsychiatryStatusMaxID(ctx).ToString();
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
