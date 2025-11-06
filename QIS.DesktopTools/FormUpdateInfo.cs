using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
 
namespace QIS.DesktopTools
{
    public partial class FormUpdateInfo : Form
    {
        private WebClient webClient = null;
        string rootPath = Application.StartupPath;
        string patchFile = "update.zip";
        string folderPatch = "";
 
        public FormUpdateInfo()
        {
            InitializeComponent();

            //remove all file
            string[] patcholdFile = Directory.GetFiles(rootPath, "*.bak", SearchOption.TopDirectoryOnly);
            foreach (string file in patcholdFile)
            {
                if (File.Exists(file))
                {
                    FileInfo fInfo = new FileInfo(file);
                    fInfo.Attributes = FileAttributes.Normal;
                    fInfo.Delete();
                }
            }

            GetUpdateAPP(); 
           
        }
        private void GetUpdateAPP()
        {
            string URL = System.Configuration.ConfigurationManager.AppSettings["URL_UPDATE"];
            string remoteVersionURL = URL + "/" + patchFile;
            
            WebClient webClient = new WebClient();
            Console.WriteLine("Checking for updates...");
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadFileAsync(new Uri(remoteVersionURL), string.Format("{0}\\{1}", rootPath, patchFile));
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string URL = System.Configuration.ConfigurationManager.AppSettings["URL_UPDATE"];
            string remoteVersionURL = URL +"/" + patchFile;
            WebClient webClient = new WebClient();
            Console.WriteLine("Checking for updates...");
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadFileAsync(new Uri(remoteVersionURL), string.Format("{0}\\{1}", rootPath, patchFile));
           
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try {

               
                
                string zipFile = string.Format("{0}\\{1}", rootPath, patchFile);
                string folderPatch = string.Format("{0}\\patch", rootPath);
                if (!Directory.Exists(folderPatch)) 
                {
                    Directory.CreateDirectory(folderPatch);
                }
                ExtractZipFile(zipFile, folderPatch);
                Application.Exit();


                System.Diagnostics.Process.Start(Application.ExecutablePath);

            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

         

        private void ExtractZipFile(string zipFileLocation, string destination)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipFileLocation))
            {
                zip.ExtractAll(destination);
                
                string[] patchFile = Directory.GetFiles(destination);
                foreach (string file in patchFile)
                {
                    string fileName = Path.GetFileName(file);

                    string originalFile = string.Format("{0}\\{1}", destination, fileName);
                    string fileToReplace = string.Format("{0}\\{1}", rootPath, fileName);
                    string backUpOfFileToReplace = string.Format("{0}\\{1}.bak", rootPath, fileName);

                    if (File.Exists(originalFile) && (File.Exists(fileToReplace)))
                    {
                        // Replace the file.    
                        ReplaceFile(originalFile, fileToReplace, backUpOfFileToReplace);
                        
                    }
                    else
                    {
                        Console.WriteLine("Either the file {0} or {1} doesn't " + "exist.", originalFile, fileToReplace);
                    }
                }
            }

            if (File.Exists(zipFileLocation))
            {
                File.Delete(zipFileLocation);
            }
          

        }

        // Move a file into another file, delete the original, and create a backup of the replaced file.    
        public static void ReplaceFile(string fileToMoveAndDelete, string fileToReplace, string backupOfFileToReplace)
        {
            // Create a new FileInfo object.    
            FileInfo fInfo = new FileInfo(fileToMoveAndDelete);
            // replace the file.    
            fInfo.Replace(fileToReplace, backupOfFileToReplace, false);
            
        }

        private void FormUpdateInfo_Load(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();  
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 50 milliseconds.  
                Thread.Sleep(20);
                // Report progress.  
                backgroundWorker1.ReportProgress(i);
            }  
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar   
            _ProgressBar.Value = e.ProgressPercentage;
            // Set the text.  
            this.Text = string.Format("process update {0}%", e.ProgressPercentage.ToString());  
        }  
    }
}
