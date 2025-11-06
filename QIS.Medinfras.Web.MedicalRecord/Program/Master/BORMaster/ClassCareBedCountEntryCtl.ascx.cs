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
    public partial class ClassCareBedCountEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnClassID.Value = param;

            string filterEntity = string.Format("ClassID = {0} ORDER BY LastUpdatedDate DESC, StartingDate DESC", param);
            vClassCareBedCount entity = BusinessLayer.GetvClassCareBedCountList(filterEntity).LastOrDefault();
            if (entity != null)
            {
                txtClassName.Text = entity.ClassName;
                txtStartingDate.Text = entity.StartingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtBedCount.Text = entity.BedCount.ToString();
                txtRemarks.Text = entity.Remarks;
            }
            else
            {
                ClassCare cc = BusinessLayer.GetClassCare(Convert.ToInt32(param));
                txtClassName.Text = cc.ClassName;
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
            ClassCareBedCountDao entityDao = new ClassCareBedCountDao(ctx);
            try
            {
                string filterData = string.Format("ClassID = {0} AND StartingDate = '{1}'", hdnClassID.Value, Helper.GetDatePickerValue(txtStartingDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
                List<ClassCareBedCount> lstEntity = BusinessLayer.GetClassCareBedCountList(filterData, ctx);
                if (lstEntity.Count() > 0)
                {
                    ClassCareBedCount entity = lstEntity.LastOrDefault();
                    entity.StartingDate = Helper.GetDatePickerValue(txtStartingDate.Text);
                    entity.BedCount = Convert.ToInt32(txtBedCount.Text);
                    entity.Remarks = txtRemarks.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }
                else
                {
                    ClassCareBedCount entity = new ClassCareBedCount();
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