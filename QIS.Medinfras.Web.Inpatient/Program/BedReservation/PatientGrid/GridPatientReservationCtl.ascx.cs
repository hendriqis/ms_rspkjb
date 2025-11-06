using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientReservationCtl : System.Web.UI.UserControl
    {
        protected int pageCountReservasi = 1;
        protected int currPageReservasi = 1;
        public void InitializeControl()
        {
            BindGridView(currPageReservasi, true, ref pageCountReservasi);
        }

        protected void cbpViewReservasi_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePagePatientOrder)Page).LoadAllWords();
            int pageCountReservasi = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                //if (param[0] == "transfer")
                //{
                //    vBedReservation entityBedReservation = BusinessLayer.GetvBedReservationList(string.Format("ReservationID = {0} AND GCReservationStatus = '{1}'", hdnNewReservationID.Value, Constant.Bed_Reservation_Status.PROPOSED)).FirstOrDefault();
                //    vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", entityBedReservation.RegistrationID)).FirstOrDefault();
                //    IDbContext ctx = DbFactory.Configure(true);
                //    PatientTransferDao entityDao = new PatientTransferDao(ctx);
                //    BedDao entityBedDao = new BedDao(ctx);
                //    ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
                //    ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);
                //    try
                //    {
                //        PatientTransfer patientTransfer = BusinessLayer.GetPatientTransferList(String.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", entityBedReservation.RegistrationID, Constant.PatientTransferStatus.OPEN)).FirstOrDefault();
                //        if (patientTransfer == null)
                //        {
                //            if (entityBedReservation.BedID != 0 || entityBedReservation.BedID != null)
                //            {
                //                PatientTransfer entity = new PatientTransfer();
                //                entity.ReservationID = entityBedReservation.ReservationID;
                //                entity.ToBedID = Convert.ToInt32(entityBedReservation.BedID);
                //                entity.ToClassID = Convert.ToInt32(entityBedReservation.ClassID);
                //                entity.ToChargeClassID = Convert.ToInt32(entityBedReservation.ChargeClassID);
                //                entity.ToHealthcareServiceUnitID = Convert.ToInt32(entityBedReservation.HealthcareServiceUnitID);
                //                entity.ToParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                //                entity.ToRoomID = Convert.ToInt32(entityBedReservation.RoomID);
                //                entity.ToSpecialtyID = entityVisit.SpecialtyID;
                //                entity.GCPatientTransferType = Constant.PatientTransferType.REGISTRATION;
                //                String date = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                //                entity.TransferDate = Convert.ToDateTime(date);
                //                entity.TransferTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                //                entity.Remarks = "";

                //                entity.RegistrationID = Convert.ToInt32(entityVisit.RegistrationID);
                //                entity.FromBedID = Convert.ToInt32(entityVisit.BedID);
                //                entity.FromClassID = Convert.ToInt32(entityVisit.ClassID);
                //                entity.FromChargeClassID = Convert.ToInt32(entityVisit.ChargeClassID);
                //                entity.FromHealthcareServiceUnitID = Convert.ToInt32(entityVisit.HealthcareServiceUnitID);
                //                entity.FromParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                //                entity.FromRoomID = Convert.ToInt32(entityVisit.RoomID);
                //                entity.FromSpecialtyID = entityVisit.SpecialtyID;
                //                entity.GCPatientTransferStatus = Constant.PatientTransferStatus.OPEN;

                //                entity.CreatedBy = AppSession.UserLogin.UserID;

                //                List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1})", entity.FromBedID, entity.ToBedID), ctx);
                //                Bed fromBed = lstBed.FirstOrDefault(p => p.BedID == entity.FromBedID);
                //                Bed toBed = lstBed.FirstOrDefault(p => p.BedID == entity.ToBedID);

                //                fromBed.GCBedStatus = Constant.BedStatus.WAIT_TO_BE_TRANSFERRED;
                //                toBed.GCBedStatus = Constant.BedStatus.BOOKED;
                //                toBed.RegistrationID = Convert.ToInt32(entityVisit.RegistrationID);

                //                fromBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                //                toBed.LastUpdatedBy = AppSession.UserLogin.UserID;

                //                entityDao.Insert(entity);
                //                entityBedDao.Update(fromBed);
                //                entityBedDao.Update(toBed);

                //                ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID), ctx).FirstOrDefault();
                //                entityConsultVisit.ParamedicID = entity.ToParamedicID;
                //                entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                //                entityConsultVisitDao.Update(entityConsultVisit);

                //                ctx.CommitTransaction();
                //            }
                //            else
                //            {
                //                //result1 = false;
                //                //errMessage = "Pilih Tempat Tidur Terlebih Dahulu";
                //                //ctx.RollBackTransaction(); 
                //            }
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}
                //else
                //{
                    if (param[0] == "changepage")
                    {
                        BindGridView(Convert.ToInt32(param[1]), false, ref pageCountReservasi);
                        result = "changepage";
                    }
                    else // refresh
                    {

                        BindGridView(1, true, ref pageCountReservasi);
                        result = "refresh|" + pageCountReservasi;
                    }
            //    }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePagePatientOrder)Page).GetFilterExpressionTestOrder();
            string sortBy = ((BasePagePatientOrder)Page).GetSortingTestOrder();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBedReservation1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vBedReservation1> lstEntity = BusinessLayer.GetvBedReservation1List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, sortBy);
            lvwViewReservasi.DataSource = lstEntity;
            lvwViewReservasi.DataBind();
        }

        protected void lvwViewReservasi_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //vTestOrderHdVisit entity = (vTestOrderHdVisit)e.Item.DataItem;
                //HtmlGenericControl spnProcessed = e.Item.FindControl("spnProcessed") as HtmlGenericControl;
                //if(entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                //    spnProcessed.Style.Add("display", "none");
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePagePatientOrder)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDtReservasi_Click(object sender, EventArgs e)
        {
            String ReservationID = hdnNewReservationID.Value;
            ((BasePagePatientOrder)Page).OnGrdRowClick(ReservationID);
        }
    }
}