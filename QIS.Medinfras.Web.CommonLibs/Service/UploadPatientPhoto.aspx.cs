using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    public partial class UploadPatientPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.InputStream.Length > 0)
            {
                try
                {
                    int MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                    Patient entity = BusinessLayer.GetPatient(MRN);

                    string path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISPatientImagePath.Replace('/', '\\');
                    path = path.Replace("#MRN", entity.MedicalNo);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string pictureFileName = string.Format("{0}.jpg", entity.MedicalNo);
                    System.Drawing.Image img = Bitmap.FromStream(Request.InputStream);
                    img.Save(string.Format("{0}{1}", path, pictureFileName), ImageFormat.Jpeg);

                    entity.PictureFileName = pictureFileName;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatient(entity);
                    Response.Clear();
                    //Response.Write(string.Format("http://192.168.90.4/ApplicationData/Data/{0}", fileName));
                }
                catch
                {
                    Response.Clear();
                    //Response.Write(string.Format("http://192.168.90.4/ApplicationData/Data/book.jpg"));
                }
            }
        }
    }
}