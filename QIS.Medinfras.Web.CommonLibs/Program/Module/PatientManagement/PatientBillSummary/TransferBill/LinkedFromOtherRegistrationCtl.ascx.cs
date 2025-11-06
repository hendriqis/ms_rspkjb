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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class LinkedFromOtherRegistrationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            vRegistration10 entity = BusinessLayer.GetvRegistration10List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            txtRegistrationNoCtl.Text = entity.RegistrationNo;
            txtPatientCtl.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnitCtl.Text = string.Format("{0} || {1}", entity.DepartmentID, entity.ServiceUnitName);
            txtParamedicCtl.Text = entity.ParamedicName;
            txtBusinessPartnerCtl.Text = entity.BusinessPartnerName;
            txtSEPNoCtl.Text = entity.NoSEP;
            hdnMRN.Value = entity.MRN.ToString();

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationLinkedFromRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vRegistrationLinkedFrom> lstEntity = BusinessLayer.GetvRegistrationLinkedFromList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "IsMainLinked DESC, LinkedFromRegistrationID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
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
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            try
            {
                Registration entityRegistration = registrationDao.Get(Convert.ToInt32(hdnLinkedFromRegistrationID.Value));
                if (entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CLOSED)
                {
                    if (!entityRegistration.IsChargesTransfered)
                    {
                        entityRegistration.LinkedToRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationDao.Update(entityRegistration);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        // UPDATE LinkedRegistrationID
                        Registration entityLinkedRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        if (entityLinkedRegistration.LinkedRegistrationID == null)
                        {
                            entityLinkedRegistration.LinkedRegistrationID = entityRegistration.RegistrationID;
                            entityLinkedRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(entityLinkedRegistration);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, data Registrasi dgn nomor <b>" + entityRegistration.RegistrationNo + "</b> transaksi & tagihannya sudah ditransfer.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, data Registrasi dgn nomor <b>" + entityRegistration.RegistrationNo + "</b> statusnya sudah VOID / CLOSED.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            try
            {
                Registration entityRegistration = registrationDao.Get(Convert.ToInt32(hdnLinkedFromRegistrationID.Value));
                if (entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CLOSED)
                {
                    if (!entityRegistration.IsChargesTransfered)
                    {
                        entityRegistration.LinkedToRegistrationID = null;
                        entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationDao.Update(entityRegistration);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        // UPDATE LinkedRegistrationID
                        Registration entityLinkedRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        if (entityLinkedRegistration.LinkedRegistrationID == entityRegistration.RegistrationID)
                        {
                            entityLinkedRegistration.LinkedRegistrationID = null;
                            entityLinkedRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(entityLinkedRegistration);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, data Registrasi dgn nomor <b>" + entityRegistration.RegistrationNo + "</b> transaksi & tagihannya sudah ditransfer.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, data Registrasi dgn nomor <b>" + entityRegistration.RegistrationNo + "</b> statusnya sudah VOID / CLOSED.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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