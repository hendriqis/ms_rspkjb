<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientDataPageList.master" AutoEventWireup="true" 
    CodeBehind="PatientFolderList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientFolderList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientFolderChangePhoto" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbpayorupdate.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Change Photo")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        function addUploadToolbar() {
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-spacing"></li>');
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-item" title="Upload" style="cursor: pointer;"><div class="dxm-content" id="uploadButton">' +
        								'<img class="dxm-image toolbarUpload rotateimg" src="<%=ResolveUrl("~/Libs/Images/FileManager/upload-icon.gif")%>" width="17px" style="margin-top:-3px;">' +
        								'</div></li>');
//            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-item" title="Scan" style="cursor: pointer;"><div class="dxm-content" id="scanButton">' +
//        								'<img class="dxm-image toolbarUpload rotateimg" src="<%=ResolveUrl("~/Libs/Images/FileManager/upload-icon.gif")%>" width="17px" style="margin-top:-3px;">' +
//        								'</div></li>');

            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-spacing"></li>');
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-item" title="Edit File Description" style="cursor: pointer;"><div class="dxm-content" id="btnEditFileDesc">' +
        								'<img class="dxm-image toolbarUpload rotateimg" src="<%=ResolveUrl("~/Libs/Images/FileManager/edit-icon.png")%>" width="17px">' +
        								'</div></li>');
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-spacing"></li>');
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-item" title="Open" style="cursor: pointer;"><div class="dxm-content" id="openButton">' +
        								'<img class="dxm-image toolbarUpload rotateimg" src="<%=ResolveUrl("~/Libs/Images/FileManager/open-icon.png")%>" width="17px">' +
        								'</div></li>');
        }

        $(function () {
            addUploadToolbar();

            $('#uploadButton').click(function () {
                var isAllowUpload = true;
                var folderName = fileManager.GetCurrentFolderPath().split('\\')[1];
                if (folderName == 'BodyDiagram') {
                    isAllowUpload = false;
                    showToast('Warning', 'Cannot Upload Data Into Body Diagram Folder');
                }
                if (isAllowUpload) {
                    var path = $('.dx.dxm-image-l.dxm-gutter').find('input').first().val();
                    path = fileManager.GetCurrentFolderPath();
                    var url = ResolveUrl("~/Program/PatientAdministration/PatientPage/PatientFolder/PatientFolderUploadDocumentCtl.ascx");
                    openUserControlPopup(url, path, 'Upload Patient Document', 800, 400);
                }
            });

            $('#openButton').click(function () {
                var selectedFile = fileManager.GetSelectedFile();
                var path = fileManager.GetCurrentFolderPath();
                if (selectedFile != null) {
                    var fileName = selectedFile.GetFullName();
                    var folderName = fileName.split('\\')[1];
                    var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
                    window.open(url, path + "|" + fileName, "popupWindow", "width=800, height=400,scrollbars=yes");
                }
                else {
                    showToast('Warning', 'Please Select a File');
                }
            });

            //            $('#scanButton').click(function () {
            //                var isAllowUpload = true;
            //                var folderName = fileManager.GetCurrentFolderPath().split('\\')[1];
            //                if (folderName == 'BodyDiagram') {
            //                    isAllowUpload = false;
            //                    showToast('Warning', 'Cannot Upload Document Into Body Diagram Folder');
            //                }
            //                if (isAllowUpload) {
            //                    var path = $('.dx.dxm-image-l.dxm-gutter').find('input').first().val();
            //                    path = fileManager.GetCurrentFolderPath().substring(6);
            //                    var url = ResolveUrl("~/Program/PatientAdministration/PatientPage/PatientFolder/ScanDocumentCtl.ascx");
            //                    openUserControlPopup(url, path, 'Scan Document', 800, 600);
            //                }
            //            });

            $('#btnEditFileDesc').click(function () {
                var selectedFile = fileManager.GetSelectedFile();
                var path = fileManager.GetCurrentFolderPath();
                if (selectedFile != null) {
                    var fileName = selectedFile.GetFullName();
                    var folderName = fileName.split('\\')[1];
                    if (folderName == 'BodyDiagram')
                        showToast('Warning', 'Cannot Edit From Body Diagram Folder');
                    else {
                        var url = ResolveUrl("~/Program/PatientAdministration/PatientPage/PatientFolder/PatientFolderUploadDocumentEditCtl.ascx");
                        openUserControlPopup(url, path + "|" + fileName, 'Edit Document', 800, 400);
                    }
                }
                else {
                    showToast('Warning', 'Please Select a File');
                }
            });

            $('#<%=btnPatientFolderChangePhoto.ClientID %>').click(function () {
                var MRN = parseInt('<%=MRN %>');
                var url = ResolveUrl("~/Libs/Controls/EditPatientPhotoCtl.ascx");
                openUserControlPopup(url, MRN, 'Edit Profile Picture', 300, 430);
            });

            Methods.checkImageError('imgPatientProfilePicture', 'patient', 'hdnPatientGender');
        });

        var currentPatientFolderSelectedFolder = '';
        $('.dxfm-folder').die('click');
        $('.dxfm-folder').live('click', function () {
            currentPatientFolderSelectedFolder = $(this).find('.dxtv-ndTxt').html();

            $('dxWeb_fmDeleteButton').attr('class', 'dxWeb_fmDeleteButtonDisabled');
        });

        function onAfterSavePatientPhoto() {
            var MRN = parseInt('<%=MRN %>');
            var filterExpression = 'MRN = ' + MRN;
            Methods.getObjectValue('GetvPatientList', filterExpression, 'PatientImageUrl', function (result) {
                setPatientPagePicture(result);
            });
        }

        function onBeforeItemRenaming(e) {
            if (e.isFolder) {
                var folderName = e.fullName.split('\\')[1];
                if (folderName == 'BodyDiagram') {
                    showToast('Warning', 'Cannot Rename Body Diagram Folder');
                    e.cancel = true;
                }
                $('#<%=hdnIsFolder.ClientID %>').val('1');
            }
            else
                $('#<%=hdnIsFolder.ClientID %>').val('0');
        }

        function onBeforeItemDeleting(e) {
            if (e.isFolder) {
                var folderName = e.fullName.split('\\')[1];
                if (folderName == 'BodyDiagram') {
                    showToast('Warning', 'Cannot Delete Body Diagram Folder');
                    e.cancel = true;
                }
                $('#<%=hdnIsFolder.ClientID %>').val('1');
            }
            else {
                var folderName = e.fullName.split('\\')[1];
                if (folderName == 'BodyDiagram') {
                    showToast('Warning', 'Cannot Delete Item From Body Diagram Folder');
                    e.cancel = true;
                }
                $('#<%=hdnIsFolder.ClientID %>').val('0');
            }

//            var MRN = parseInt('<%=MRN %>');
//            var fileName = "";
//            var filterexpression = "";
//            Methods.getObject('GetvPatientDiagnosisList', filterexpression, function (result) {
//                if (result != null) {
//                    entityToControl(result);
//                }
//                else {
//                    ResetForm();
//                }
//            });

        }

        function onAfterSaveAddRecordEntryPopup(param) {
            fileManager.Refresh();
        }
    </script>
    <input type="hidden" id="hdnIsFolder" runat="server" value="" />
    <input type="hidden" id="hdnPatientDocumentUrl" runat="server" value="" />
    <input type="hidden" id="hdnMRN" runat="server" value="" />
    <div style="padding:10px;" class="borderBox">
        <dx:ASPxFileManager ID="fileManager" ClientInstanceName="fileManager" runat="server" OnFolderCreating="fileManager_FolderCreating" OnCustomThumbnail="fileManager_CustomThumbnail"
            OnItemDeleting="fileManager_ItemDeleting" OnItemMoving="fileManager_ItemMoving"
            OnItemRenaming="fileManager_ItemRenaming">
            <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf,.tif,.mp4,.docx" />
            <SettingsUpload Enabled="false"/>
            <SettingsToolbar ShowPath="false" />
            <SettingsEditing AllowCreate="true" AllowDelete="true" AllowRename="false" />
            <SettingsPermissions>
                <AccessRules>
                    <dx:FileManagerFolderAccessRule Path="System" Edit="Deny" />
                </AccessRules>
            </SettingsPermissions>
            <ClientSideEvents ItemDeleting="function(s,e){
                onBeforeItemDeleting(e);
            }" ItemRenaming="function(s,e){
                onBeforeItemRenaming(e);
            }" ItemMoving="function(s,e){
                var folderName = e.fullName.split('\\')[1];
                if(folderName == 'BodyDiagram'){
                    showToast('Warning', 'Cannot Move Item From Body Diagram Folder');
                    e.cancel = true;
                }
            }" FolderCreating="function(s,e){
                var folderName = e.fullName.split('\\')[1];
                if(folderName == 'BodyDiagram'){
                    showToast('Warning', 'Cannot Create New Folder Inside Body Diagram Folder');
                    e.cancel = true;
                }
            }" SelectedFileOpened="function(s,e){
                
            }" />
        </dx:ASPxFileManager>
    </div>
</asp:Content>
