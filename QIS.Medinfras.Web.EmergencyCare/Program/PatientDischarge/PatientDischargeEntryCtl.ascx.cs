using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class PatientDischargeEntryCtl : BaseEntryPopupCtl
    {
        protected string RegistrationDateTime = "";

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnBedID.Value = param;
                Bed entityBed = BusinessLayer.GetBedList(string.Format("BedID = {0}", hdnBedID.Value)).FirstOrDefault();
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", entityBed.RegistrationID))[0];

                EntityToControl(entity);
            }
        }

        private void EntityToControl(vRegistration3 entity)
        {
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtRegistrationDateTime.Text = entity.RegistrationNo;
            txtPatientInfo.Text = entity.PatientName;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtRoomName.Text = entity.RoomName;
            //hdnBedID.Value = Convert.ToString(entity.BedID);
            txtBedCode.Text = entity.BedCode;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BedDao entityBedDao = new BedDao(ctx);
            try
            {
                Bed entityBed = BusinessLayer.GetBed(Convert.ToInt32(hdnBedID.Value));
                entityBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                entityBed.RegistrationID = null;

                entityBedDao.Update(entityBed);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                retval = "disposisi";
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = "Pasien tidak memiliki tempat tidur";
                Helper.InsertErrorLog(ex);
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