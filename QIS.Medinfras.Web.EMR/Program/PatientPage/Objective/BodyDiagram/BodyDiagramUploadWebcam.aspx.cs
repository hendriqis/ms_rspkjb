using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class BodyDiagramUploadWebcam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.InputStream.Length > 0)
            {
                try
                {
                    System.Drawing.Image img = Bitmap.FromStream(Request.InputStream);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Convert Image to byte[]
                        img.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        Session["bodyDiagramUploadWebcam"] = base64String;
                        //return base64String;
                    }
                }
                catch
                {
                }
                finally
                {
                    Response.Clear();
                }
            }
        }
    }
}