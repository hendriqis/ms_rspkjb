using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientAccompanyEntryCtl : BaseViewPopupCtl
    {

        protected int serviceUnitUserCount = 0;
        protected bool IsAllowEditPatientVisit = true;
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;
            serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            vRegistration header = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
            txtRegistrationNo.Text = header.RegistrationNo;
            txtPatientName.Text = String.Format("({0}) {1}", header.MedicalNo ,header.PatientName);
            hdnGCRegistrationStatus.Value = header.GCRegistrationStatus;
            hdnMRN.Value = header.MRN.ToString();

            if (header.GCRegistrationStatus == Constant.VisitStatus.CLOSED || header.GCRegistrationStatus == Constant.VisitStatus.CANCELLED) {
                IsAllowEditPatientVisit = false;
            }

            if (!IsAllowEditPatientVisit)
                divContainerAddData.Style.Add("display", "none");

            Helper.SetControlEntrySetting(cboRelationship, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            txtClassCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtClassName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtServiceUnitCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtServiceUnitName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtRoomCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtRoomName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtBedCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtChargeClassCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtChargeClassName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtContactName.Attributes.Add("validationgroup", "mpEntryPopup");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID like '%{0}%'", Constant.StandardCode.FAMILY_RELATION));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboRelationship, lstSc, "StandardCodeName", "StandardCodeID");
            cboRelationship.SelectedIndex = 0;
        }

        protected string GetServiceUnitUserParameter()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("RegistrationID = {0} AND IsDeleted = 0 AND (RegistrationID = bedRegistrationID OR bedRegistrationID IS NULL)", hdnRegistrationID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAccompanyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientAccompany> lstEntity = BusinessLayer.GetvPatientAccompanyList(filterExpression, 8, pageIndex, "PatientAccompanyName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientAccompany entity)
        {
            if (chkIsFamily.Checked)
            {
                entity.FamilyID = Convert.ToInt32(hdnFamilyID.Value);
            }
            else
            {
                entity.PatientAccompanyName = txtContactName.Text;
                entity.GCRelationShip = Convert.ToString(cboRelationship.Value);
            }

            entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);
            entity.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
            entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
            entity.BedID = Convert.ToInt32(hdnBedID.Value);
            entity.PhoneNo = txtTelephoneNo.Text;
            entity.MobilePhoneNo = txtMobilePhone.Text;
            entity.Remarks = txtRemarks.Text;
            entity.CreatedDate = DateTime.Now;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAccompanyDao entityDao = new PatientAccompanyDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);

            try
            {
                PatientAccompany entity = new PatientAccompany();
                Bed Bedentity = new Bed();
                ControlToEntity(entity);
                
                string accompanyNo = "", counterstr = "";
                int counter = 0;
                Registration entityReg = BusinessLayer.GetRegistration(entity.RegistrationID);
                int regLength = entityReg.RegistrationNo.Length;
                string filterPA = string.Format("PatientAccompanyNo LIKE '{0}%' ORDER BY PatientAccompanyNo DESC", entityReg.RegistrationNo);
                PatientAccompany lstPA = BusinessLayer.GetPatientAccompanyList(string.Format(filterPA)).FirstOrDefault();
                if (lstPA != null)
                {
                    int startIndex = regLength + 1;
                    string nocounter = lstPA.PatientAccompanyNo.Substring(startIndex, 2);
                    counter = Convert.ToInt32(nocounter) + 1;
                }
                else
                {
                    counter += 1;
                }

                if (counter < 10)
                {
                    counterstr = string.Format("0{0}", counter);
                }
                else
                {
                    counterstr = counter.ToString();
                }
                accompanyNo = string.Format("{0}-{1}", entityReg.RegistrationNo, counterstr);
                entity.PatientAccompanyNo = accompanyNo;

                //entity.PatientAccompanyNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES, entity.CreatedDate);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;

                Bedentity = BusinessLayer.GetBed(Convert.ToInt32(entity.BedID));

                if (Bedentity.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                {
                    Bedentity.IsPatientAccompany = true;
                    Bedentity.GCBedStatus = Constant.BedStatus.OCCUPIED;
                    Bedentity.RegistrationID = entity.RegistrationID;
                    Bedentity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDao.Insert(entity);
                    entityBedDao.Update(Bedentity);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Maaf tempat tidur ini tidak dapat digunakan. Silahkan pilih tempat tidur lain.";
                    result = false;
                    ctx.RollBackTransaction();
                }
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

        //private bool OnSaveEditRecord(ref string errMessage)
        //{
        //}

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAccompanyDao entityDao = new PatientAccompanyDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);

            try
            {
                PatientAccompany entity = new PatientAccompany();
                entity = BusinessLayer.GetPatientAccompany(Convert.ToInt32(hdnID.Value));
                Bed Bedentity = new Bed();
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;

                Bedentity = BusinessLayer.GetBed(Convert.ToInt32(entity.BedID));
                Bedentity.IsPatientAccompany = false;
                Bedentity.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                Bedentity.RegistrationID = null;

                entityDao.Update(entity);
                entityBedDao.Update(Bedentity);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
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
}