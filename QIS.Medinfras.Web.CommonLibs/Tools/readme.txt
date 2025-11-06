Ketika membuat project baru: 
1. Buat folder SharedLibs. Add As Link file-file dari CommonLibs (contoh ada di outpatient, inpatient)
2. Add as link connectionstring.config
   Properties ConnectionString.config -> Copy To Output Directory -> Copy if newer
   Properties web project nya: 
   - build events -> Post-build event command line:
     COPY "$(TargetDir)*.config" "$(ProjectDir)"
3. Copy web.config dari inpatient / outpatient ke project baru
4. Di properties project -> web -> Servers -> Use Local IIS Web server
   Project Url: http://localhost/msdev/[Module Name]
5. Add reference:
   - QIS.Data.Core
   - QIS.Medinfras.Data.Service
   - QIS.Medinfras.Report
   - QIS.Medinfras.Web.Common
   - QIS.Medinfras.Web.CustomControl
   - DevExpress.Web.v11.1
6. Di inetmgr, msdev/[Module Name]: Add virtual directory:
   - alias = libs 
   - physical path : ambil path QIS.Medinfras.Web.CommonLibs

