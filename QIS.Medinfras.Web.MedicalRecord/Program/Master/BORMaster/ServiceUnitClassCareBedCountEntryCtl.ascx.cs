using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ServiceUnitClassCareBedCountEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnServiceUnitID.Value = param;

            string filterEntity = string.Format("ServiceUnitID = {0} ORDER BY LastUpdatedDate DESC, StartingDate DESC", param);
            vServiceUnitClassCareBedCount entity = BusinessLayer.GetvServiceUnitClassCareBedCountList(filterEntity).LastOrDefault();
            if (entity != null)
            {
                txtServiceUnitName.Text = entity.ServiceUnitName;
                hdnClassID.Value = entity.ClassID.ToString();
                txtClassCode.Text = entity.ClassCode;
                txtClassName.Text = entity.ClassName;
                txtStartingDate.Text = entity.StartingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtBedCount.Text = entity.BedCount.ToString();
                txtRemarks.Text = entity.Remarks;
            }
            else
            {
                ServiceUnitMaster sums = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(param));
                txtServiceUnitName.Text = sums.ServiceUnitName;
                txtStartingDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtBedCount.Text = "0";
                txtRemarks.Text = "";
            }

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStartingDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBedCount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitClassCareBedCountDao entityDao = new ServiceUnitClassCareBedCountDao(ctx);
            try
            {
                string filterData = string.Format("ServiceUnitID = {0} AND ClassID = {1} AND StartingDate = '{2}'", hdnServiceUnitID.Value, hdnClassID.Value, Helper.GetDatePickerValue(txtStartingDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
                List<ServiceUnitClassCareBedCount> lstEntity = BusinessLayer.GetServiceUnitClassCareBedCountList(filterData, ctx);
                if (lstEntity.Count() > 0)
                {
                    ServiceUnitClassCareBedCount entity = lstEntity.LastOrDefault();
                    entity.StartingDate = Helper.GetDatePickerValue(txtStartingDate.Text);
                    entity.BedCount = Convert.ToInt32(txtBedCount.Text);
                    entity.Remarks = txtRemarks.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }
                else
                {
                    ServiceUnitClassCareBedCount entity = new ServiceUnitClassCareBedCount();
                    entity.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
                    entity.ClassID = Convert.ToInt32(hdnClassID.Value);
                    entity.StartingDate = Helper.GetDatePickerValue(txtStartingDate.Text);
                    entity.BedCount = Convert.ToInt32(txtBedCount.Text);
                    entity.Remarks = txtRemarks.Text;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    entityDao.Insert(entity);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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