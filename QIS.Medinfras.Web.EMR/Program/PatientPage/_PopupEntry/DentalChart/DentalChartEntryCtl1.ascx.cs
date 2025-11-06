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

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class DentalChartEntryCtl1 : BaseEntryPopupCtl
    {
        protected string SpecialtyID = "";
        public override void InitializeDataControl(string queryString)
        {
            string[] param = queryString.Split('|');
            if (param[0] == "add")
            {
                IsAdd = true;
                hdnToothID.Value = string.Format("X044^0{0}", Convert.ToInt32(param[1]).ToString("00"));
                SetControlProperties();
                hdnToothSurfaces.Value = "";
            }
            else
            {
                IsAdd = false;
                int id = Convert.ToInt32(param[1]);
                hdnPatientDentalID.Value = id.ToString();
                PatientDental entity = BusinessLayer.GetPatientDental(id);
                hdnToothID.Value = entity.GCTooth;
                SetControlProperties();
                EntityToControl(entity);
            }
            cboToothProblem.Focus();
        }

        protected void SetControlProperties()
        {
            SpecialtyID = AppSession.RegisteredPatient.SpecialtyID;
            ledProcedure.FilterExpression = string.Format("SpecialtyID = '{0}'", AppSession.RegisteredPatient.SpecialtyID);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') OR StandardCodeID = '{3}' AND IsDeleted = 0", Constant.StandardCode.TOOTH_PROBLEM, Constant.StandardCode.TOOTH_STATUS, Constant.StandardCode.TOOTH_SURFACES, hdnToothID.Value));
            lstSc.Insert(0, new StandardCode { StandardCodeName = "", StandardCodeID = "" });

            Methods.SetComboBoxField<StandardCode>(cboToothProblem, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TOOTH_PROBLEM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCurrentStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TOOTH_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboNextStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TOOTH_STATUS || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            txtTooth.Text = lstSc.FirstOrDefault(sc => sc.StandardCodeID == hdnToothID.Value).StandardCodeName;

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = userLoginParamedic.ToString();
            }

            string imgUrl = "";
            string userMap = "";
            string type = ""; // 1 : Graham, 2 :

            int toothNumberPrefix = Convert.ToInt32(hdnToothID.Value.Substring(6, 1));
            int lastDigitToothNumber = Convert.ToInt32(hdnToothID.Value.Substring(7));
            if (toothNumberPrefix == 1 || toothNumberPrefix == 5) // Upper Right
            {
                if (lastDigitToothNumber < 4)
                {
                    mapToothTop2.Attributes.Add("val", "X047^007"); //Labial
                    mapToothLeft2.Attributes.Add("val", "X047^004"); //Distal
                    mapToothRight2.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothBottom2.Attributes.Add("val", "X047^008"); //Palatal
                    mapToothCenter2.Attributes.Add("val", "X047^001"); //Incisal
                }
                else // Gigi Graham
                {
                    mapToothTop.Attributes.Add("val", "X047^005"); //Buccal
                    mapToothLeft.Attributes.Add("val", "X047^004"); //Distal
                    mapToothRight.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothBottom.Attributes.Add("val", "X047^008"); //Palatal
                    mapToothCenter.Attributes.Add("val", "X047^002"); //Occlusal
                }
            }
            else if (toothNumberPrefix == 2 || toothNumberPrefix == 6) // Upper Left
            {
                if (lastDigitToothNumber < 4)
                {
                    mapToothTop2.Attributes.Add("val", "X047^007"); //Labial
                    mapToothLeft2.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothRight2.Attributes.Add("val", "X047^004"); //Distal
                    mapToothBottom2.Attributes.Add("val", "X047^008"); //Palatal
                    mapToothCenter2.Attributes.Add("val", "X047^001"); //Incisal
                }
                else // Gigi Graham
                {
                    mapToothTop.Attributes.Add("val", "X047^005"); //Buccal
                    mapToothLeft.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothRight.Attributes.Add("val", "X047^004"); //Distal
                    mapToothBottom.Attributes.Add("val", "X047^008"); //Palatal
                    mapToothCenter.Attributes.Add("val", "X047^002"); //Occlusal
                }
            }
            else if (toothNumberPrefix == 3 || toothNumberPrefix == 7) // Lower Left
            {
                if (lastDigitToothNumber < 4)
                {
                    mapToothTop2.Attributes.Add("val", "X047^009"); //Lingual
                    mapToothLeft2.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothRight2.Attributes.Add("val", "X047^004"); //Distal
                    mapToothBottom2.Attributes.Add("val", "X047^007"); //Labial
                    mapToothCenter2.Attributes.Add("val", "X047^001"); //Incisal
                }
                else // Gigi Graham
                {
                    mapToothTop.Attributes.Add("val", "X047^009"); //Lingual
                    mapToothLeft.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothRight.Attributes.Add("val", "X047^004"); //Distal
                    mapToothBottom.Attributes.Add("val", "X047^005"); //Buccal
                    mapToothCenter.Attributes.Add("val", "X047^002"); //Occlusal
                }
            }
            else if (toothNumberPrefix == 4 || toothNumberPrefix == 8) // Lower Right
            {
                if (lastDigitToothNumber < 4)
                {
                    mapToothTop2.Attributes.Add("val", "X047^009"); //Lingual
                    mapToothLeft2.Attributes.Add("val", "X047^004"); //Distal
                    mapToothRight2.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothBottom2.Attributes.Add("val", "X047^007"); //Labial
                    mapToothCenter2.Attributes.Add("val", "X047^001"); //Incisal
                }
                else // Gigi Graham
                {
                    mapToothTop.Attributes.Add("val", "X047^009"); //Lingual
                    mapToothLeft.Attributes.Add("val", "X047^004"); //Distal
                    mapToothRight.Attributes.Add("val", "X047^003"); //Mesial
                    mapToothBottom.Attributes.Add("val", "X047^005"); //Buccal
                    mapToothCenter.Attributes.Add("val", "X047^002"); //Occlusal
                }
            }
            if (lastDigitToothNumber > 3)
            {
                type = "1";
                imgUrl = "tooth_entry.png";
                userMap = "#maptooth";
            }
            else
            {
                type = "2";
                imgUrl = "tooth_entry2.png";
                userMap = "#maptooth2";
            }


            imgToothSurface.Src = string.Format("{0}{1}", Page.ResolveUrl("~/Libs/Images/Medical/"), imgUrl);
            imgToothSurface.Attributes.Add("usemap", userMap);
            imgToothSurface.Attributes.Add("type", type);
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTreatmentDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTreatmentTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(txtTooth, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboToothProblem, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCurrentStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboNextStatus, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNextDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PatientDental entity)
        {
            txtTreatmentDate.Text = entity.TreatmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTreatmentTime.Text = entity.TreatmentTime;
            cboToothProblem.Value = entity.GCToothProblem;
            cboCurrentStatus.Value = entity.GCToothStatus;
            //ledProcedure.Value = entity.ProcedureID;
            hdnProcedureID.Value = entity.ProcedureID;
            cboNextStatus.Value = entity.NextGCToothStatus;
            if (entity.NextDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtNextDate.Text = entity.NextDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNotes.Text = entity.Remarks;
            hdnToothSurfaces.Value = entity.ToothSurfaces;
        }

        private void ControlToEntity(PatientDental entity)
        {
            entity.TreatmentDate = Helper.GetDatePickerValue(txtTreatmentDate);
            entity.TreatmentTime = txtTreatmentTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.GCTooth = hdnToothID.Value;
            entity.GCToothProblem = cboToothProblem.Value.ToString();
            entity.GCToothStatus = cboCurrentStatus.Value.ToString();
            //entity.ProcedureID = hdnProcedureID.Value;
            if (cboNextStatus.Value != null && cboNextStatus.Value.ToString() != "")
                entity.NextGCToothStatus = cboNextStatus.Value.ToString();
            else
                entity.NextGCToothStatus = null;
            entity.ToothSurfaces = hdnToothSurfaces.Value;
            entity.NextDate = Helper.GetDatePickerValue(txtNextDate);
            entity.Remarks = txtNotes.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientDentalDao entityDao = new PatientDentalDao(ctx);
            PatientProcedureDao entityProcedureDao = new PatientProcedureDao(ctx);
            try
            {
                PatientDental entity = new PatientDental();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entity.ID = BusinessLayer.GetPatientDentalMaxID(ctx);

                if (hdnProcedureID.Value != "0" && !string.IsNullOrEmpty(hdnProcedureID.Value))
                {
                    PatientProcedure entityProcedure = new PatientProcedure();
                    entityProcedure.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityProcedure.ReferenceID = entity.ID;
                    entityProcedure.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entityProcedure.ProcedureDate = entity.TreatmentDate;
                    entityProcedure.ProcedureTime = entity.TreatmentTime;
                    entityProcedure.ProcedureID = entity.ProcedureID;
                    entityProcedure.Remarks = entity.Remarks;
                    entityProcedure.IsCreatedBySystem = true;
                    entityProcedure.CreatedBy = AppSession.UserLogin.UserID;
                    entityProcedureDao.Insert(entityProcedure); 
                }

                ctx.CommitTransaction();

                retVal = "DentalChart";
            }
            catch (Exception ex)
            {
                retVal = "DentalChart";
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientDentalDao entityDao = new PatientDentalDao(ctx);
            PatientProcedureDao entityProcedureDao = new PatientProcedureDao(ctx);
            try
            {
                Int32 ID = Convert.ToInt32(hdnPatientDentalID.Value);
                PatientDental entity = entityDao.Get(ID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                if (hdnProcedureID.Value != "0" && !string.IsNullOrEmpty(hdnProcedureID.Value))
                {
                    PatientProcedure entityProcedure = BusinessLayer.GetPatientProcedureList(String.Format("ReferenceID = {0}", ID), ctx)[0];
                    entityProcedure.ProcedureDate = entity.TreatmentDate;
                    entityProcedure.ProcedureTime = entity.TreatmentTime;
                    entityProcedure.ProcedureID = entity.ProcedureID;
                    entityProcedure.Remarks = entity.Remarks;
                    entityProcedure.IsCreatedBySystem = true;
                    entityProcedure.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityProcedureDao.Update(entityProcedure); 
                }

                ctx.CommitTransaction();

                retVal = "DentalChart";
            }
            catch (Exception ex)
            {
                retVal = "DentalChart";
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
}