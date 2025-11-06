using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for UploadService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class UploadService : System.Web.Services.WebService
    {
        #region Body Diagram
        [WebMethod(EnableSession = true)]
        public bool UploadBodyDiagram(DateTime observationDate, string observationTime, string imageData, int idx, string listSymbol, string customSrc, int paramedicID, string testOrderID = "0")
        {
            bool result = true;

            string path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISPatientBodyDiagramImagePath.Replace('/', '\\');

            path = path.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = String.Format("{0}_{1}.png", AppSession.RegisteredPatient.VisitID, (idx + 1));
            FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            byte[] data = Convert.FromBase64String(imageData);
            bw.Write(data);
            bw.Close();

            //format listImageSymbol : "diagramID|symbol1|symbol2|....;
            //Format symbol-n: id;left;top;symbolCode;notes
            string[] param = listSymbol.Split('|');
            int diagramID = Convert.ToInt32(param[0]);
            if (diagramID < 0)
            {
                path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISBodyDiagramImagePath.Replace('/', '\\') + "\\Custom\\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                bw = new BinaryWriter(fs);

                data = Convert.FromBase64String(customSrc);
                bw.Write(data);
                bw.Close();
            }
            IDbContext ctx = DbFactory.Configure(true);
            PatientBodyDiagramHdDao patientHdDao = new PatientBodyDiagramHdDao(ctx);
            PatientBodyDiagramDtDao patientDtDao = new PatientBodyDiagramDtDao(ctx);

            string errMessage = "";
            try
            {
                PatientBodyDiagramHd patientHd = new PatientBodyDiagramHd();
                patientHd.VisitID = AppSession.RegisteredPatient.VisitID;
                if (diagramID > -1)
                    patientHd.DiagramID = diagramID;
                else
                {
                    patientHd.DiagramID = null;
                    patientHd.CustomFileName = fileName;
                }
                patientHd.FileName = fileName;
                patientHd.ParamedicID = paramedicID;
                patientHd.ObservationDate = observationDate;
                patientHd.ObservationTime = observationTime;
                if (testOrderID != "0" && testOrderID != "")
                {
                    if (testOrderID.Contains(';'))
                    {
                        string[] linkedId = testOrderID.Split(';');
                        patientHd.PatientChargesDtID = Convert.ToInt32(linkedId[0]);
                    }
                    else
                    {
                        patientHd.TestOrderID = Convert.ToInt32(testOrderID);
                    }

                }
                patientHd.CreatedBy = AppSession.UserLogin.UserID;
                patientHdDao.Insert(patientHd);
                patientHd.ID = BusinessLayer.GetPatientBodyDiagramHdMaxID(ctx);

                for (int i = 1; i < param.Length; ++i)
                {
                    string[] symbol = param[i].Split(';');

                    PatientBodyDiagramDt patientDt = new PatientBodyDiagramDt();
                    patientDt.ID = patientHd.ID;
                    patientDt.GCBodyDiagramSymbol = string.Format("{0}^{1}", Constant.StandardCode.BODY_DIAGRAM_SYMBOL, symbol[0]);
                    patientDt.LeftMargin = Convert.ToDecimal(symbol[1]);
                    patientDt.TopMargin = Convert.ToDecimal(symbol[2]);
                    patientDt.SymbolCode = symbol[3];
                    patientDt.Remarks = symbol[4];
                    patientDt.CreatedBy = AppSession.UserLogin.UserID;
                    patientDtDao.Insert(patientDt);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            if (!result)
                throw new Exception(errMessage);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public bool UploadBodyDiagramPhoto(string image)
        {
            bool result = true;

            try
            {
                Session["bodyDiagramUploadWebcam"] = image;
            }
            catch
            {
            }
            
            return result;
        }

        [WebMethod(EnableSession = true)]
        public bool UploadFreeDrawImage(string image)
        {
            bool result = true;

            try
            {
                Session["freeDrawImage"] = image;
            }
            catch
            {
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public bool UpdateBodyDiagram(int ID, string imageData, string listSymbol)
        {
            bool result = true;

            PatientBodyDiagramHd patientHd = BusinessLayer.GetPatientBodyDiagramHd(ID);

            string path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISPatientBodyDiagramImagePath.Replace('/', '\\');

            path = path.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = patientHd.FileName;

            FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            byte[] data = Convert.FromBase64String(imageData);
            bw.Write(data);
            bw.Close();

            //format listImageSymbol : "|symbol1|symbol2|....;
            //Format symbol-n: id;left;top;symbolCode;notes
            string[] param = listSymbol.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            PatientBodyDiagramHdDao patientHdDao = new PatientBodyDiagramHdDao(ctx);
            PatientBodyDiagramDtDao patientDtDao = new PatientBodyDiagramDtDao(ctx);
            try
            {
                patientHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                patientHdDao.Update(patientHd);

                string filterExpression = String.Format("ID = {0} AND IsDeleted = 0", ID);
                List<PatientBodyDiagramDt> listPatientDtOld = BusinessLayer.GetPatientBodyDiagramDtList(filterExpression, ctx);
                foreach (PatientBodyDiagramDt patientDtOld in listPatientDtOld)
                {
                    patientDtOld.IsDeleted = true;
                }

                for (int i = 1; i < param.Length; ++i)
                {
                    string[] symbol = param[i].Split(';');
                    string symbolCode = symbol[3];

                    PatientBodyDiagramDt patientDt = listPatientDtOld.FirstOrDefault(p => p.SymbolCode == symbolCode);
                    //Update
                    if (patientDt != null)
                    {
                        patientDt.LeftMargin = Convert.ToDecimal(symbol[1]);
                        patientDt.TopMargin = Convert.ToDecimal(symbol[2]);
                        patientDt.Remarks = symbol[4];
                        patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDt.IsDeleted = false;
                        patientDtDao.Update(patientDt);
                    }
                    //Insert
                    else
                    {
                        patientDt = new PatientBodyDiagramDt();
                        patientDt.ID = ID;
                        patientDt.GCBodyDiagramSymbol = string.Format("{0}^{1}", Constant.StandardCode.BODY_DIAGRAM_SYMBOL, symbol[0]);
                        patientDt.LeftMargin = Convert.ToDecimal(symbol[1]);
                        patientDt.TopMargin = Convert.ToDecimal(symbol[2]);
                        patientDt.SymbolCode = symbol[3];
                        patientDt.Remarks = symbol[4];
                        patientDt.CreatedBy = AppSession.UserLogin.UserID;
                        patientDtDao.Insert(patientDt);
                    }
                }
                foreach (PatientBodyDiagramDt patientDtOld in listPatientDtOld)
                {
                    //Delete
                    if (patientDtOld.IsDeleted)
                    {
                        patientDtOld.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDtDao.Update(patientDtOld);
                    }
                }
                ctx.CommitTransaction();
            }
            catch
            {
                ctx.RollBackTransaction();
                result = false;
                //throw new Exception(ex.Message);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region Patient Photo
        [WebMethod(EnableSession = true)]
        public void UploadPatientPhoto(string imageData, int MRN)
        {
            Patient entity = BusinessLayer.GetPatient(MRN);

            string path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISPatientImagePath.Replace('/', '\\');
            path = path.Replace("#MRN", entity.MedicalNo);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = String.Format("{0}.jpg", entity.MedicalNo);
            FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            byte[] data = Convert.FromBase64String(imageData);
            bw.Write(data);
            bw.Close();

            entity.PictureFileName = fileName;
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            BusinessLayer.UpdatePatient(entity);
        }

        [WebMethod(EnableSession = true)]
        public void UploadGuestPhoto(string imageData, int GuestID)
        {
            Guest entity = BusinessLayer.GetGuest(GuestID);

            string path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISGuestImagePath.Replace('/', '\\');
            path = path.Replace("#GUESTNO", entity.GuestNo);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = String.Format("{0}.jpg", entity.GuestNo);
            FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            byte[] data = Convert.FromBase64String(imageData);
            bw.Write(data);
            bw.Close();

            entity.PictureFileName = fileName;
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            BusinessLayer.UpdateGuest(entity);
        }
        #endregion

        [WebMethod()]
        public void UploadChartImage(string imageData)
        {
            string fileName = String.Format("D:\\upload\\{0}.png", "chart");
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            byte[] data = Convert.FromBase64String(imageData);

            bw.Write(data);
            bw.Close();
        }
    }
}
