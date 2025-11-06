using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientVisitTypeEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                SetControlProperties();
                ConsultVisitType entity = BusinessLayer.GetConsultVisitType(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(false, false, false));
        }

        private void EntityToControl(ConsultVisitType entity)
        {
            hdnVisitTypeID.Value = Convert.ToString(entity.VisitTypeID);
            vConsultVisitType vCVT = BusinessLayer.GetvConsultVisitTypeList(string.Format("ID = {0}", hdnID.Value))[0];
            txtVisitTypeCode.Text = vCVT.VisitTypeCode;
            txtVisitTypeName.Text = vCVT.VisitTypeName;
        }

        private void ControlToEntity(ConsultVisitType entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            if (hdnVisitTypeID.Value != null)
            {
                vConsultVisitType entityCVT = BusinessLayer.GetvConsultVisitTypeList(string.Format("ID = {0}", hdnID.Value))[0];
                entityCVT.VisitTypeCode = txtVisitTypeCode.Text;
                entityCVT.VisitTypeName = txtVisitTypeName.Text;
            }
            
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ConsultVisitType entity = new ConsultVisitType();
                vRegistration vreg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                entity.VisitID = vreg.VisitID;
                entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertConsultVisitType(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ConsultVisitType entity = BusinessLayer.GetConsultVisitType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateConsultVisitType(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}