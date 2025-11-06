<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.master" AutoEventWireup="true" 
    CodeBehind="PatientFolderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientFolderDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">   
    <script type="text/javascript">
        $('#imgBackPatientFolderDt').live('click', function () {
            document.location = document.referrer;
        });

        function addUploadToolbar() {
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-spacing"></li>');
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-item" title="Upload" style="cursor: pointer;"><div class="dxm-content" id="uploadButton">' +
        								'<img class="dxm-image toolbarUpload rotateimg" src="<%=ResolveUrl("~/Libs/Images/FileManager/upload-icon.gif")%>" width="17px" style="margin-top:-3px;">' +
        								'</div></li>');

            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-spacing"></li>');
            $('.dx.dxm-image-l.dxm-gutter').append('<li class="dxm-item" title="Edit File Description" style="cursor: pointer;"><div class="dxm-content" id="btnEditFileDesc">' +
        								'<img class="dxm-image toolbarUpload rotateimg" src="<%=ResolveUrl("~/Libs/Images/FileManager/upload-icon.gif")%>" width="17px" style="margin-top:-3px;">' +
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
                    path = fileManager.GetCurrentFolderPath().substring(6);
                    var url = ResolveUrl("~/Program/PatientFolder/PatientFolderUploadDocumentCtl.ascx");
                    openUserControlPopup(url, path, 'Upload Document', 800, 400);
                }
            });

            $('#btnEditFileDesc').click(function () {
                var selectedFile = fileManager.GetSelectedFile();
                if (selectedFile != null) {
                    var fileName = selectedFile.GetFullName();
                    var folderName = fileName.split('\\')[1];
                    if (folderName == 'BodyDiagram')
                        showToast('Warning', 'Cannot Edit From Body Diagram Folder');
                    else {
                        fileName = fileName.substring(6);
                        var url = ResolveUrl("~/Program/PatientFolder/PatientFolderUploadDocumentEditCtl.ascx");
                        openUserControlPopup(url, fileName, 'Edit Document', 800, 400);
                    }
                }
                else {
                    showToast('Warning', 'Please Select a File');
                }
            });

            $('#<%=imgPatientProfilePicture.ClientID %>, #divShowEditProfilePicture').hover(function () {
                $('#divShowEditProfilePicture').show();
            }, function () {
                $('#divShowEditProfilePicture').hide();
            });

            $('#divShowEditProfilePicture').click(function () {
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
                $('#<%=imgPatientProfilePicture.ClientID %>').attr('src', result);
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
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            fileManager.Refresh();
        }
    </script>
    <style type="text/css">
        #divShowEditProfilePicture          { padding:0 3px; cursor: pointer; display:none;background-color:White; font-size: 11px; width: 100px; border:1px solid #EAEAEA; position:absolute;top:44%;left:44%; }
        #divShowEditProfilePicture:hover    { border: 1px solid #B7B7B7; opacity: 0.8; }
        .imgPatientProfilePicture           { cursor: pointer; }
        
        .dxm-item:hover                     { background-color: #CBCBCB; border:1px solid #888888; } 
        
        body                                { background-color: White; }
    </style>
    <input type="hidden" id="hdnIsFolder" runat="server" value="" />
    <div style="height: 80px;">
        <div style="float:left;margin:5px 0 0 5px;position: relative;">
            <div id="divShowEditProfilePicture"><%=GetLabel("Edit Profile Picture")%></div>
            <img id="imgPatientProfilePicture" class="imgPatientProfilePicture" runat="server" src='' alt="" width="55" height="65" />
            <input type="hidden" id="hdnPatientGender" runat="server" class="hdnPatientGender" />
        </div>
        <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0" >
            <col style="width:100px"/>
            <col style="width:145px"/>
            <col style="width:100px"/>
            <col style="width:180px"/>
            <col style="width:100px"/>
            <col style="width:240px"/>
            <tr style="font-size:1.2em">
                <td colspan="3" style="padding-left:20px"><label id="lblPatientName" runat="server"></label></td>
            </tr>
            <tr>
                <td class="tdPatientBannerLabel"><%=GetLabel("MRN")%></td>
                <td><label id="lblMRN" runat="server"></label></td>
                <td class="tdPatientBannerLabel"><%=GetLabel("DOB")%></td>
                <td><label id="lblDOB" runat="server"></label></td>
            </tr>
            <tr>
                <td class="tdPatientBannerLabel"><%=GetLabel("Allergy")%></td>
                <td><label id="lblAllergy" runat="server"></label></td>
                <td class="tdPatientBannerLabel"><%=GetLabel("Age")%></td>
                <td><label id="lblPatientAge" runat="server"></label></td>
            </tr>
            <tr>
                <td class="tdPatientBannerLabel"><%=GetLabel("Note")%></td>
                <td><label id="lblNote" runat="server"></label></td>
                <td class="tdPatientBannerLabel"><%=GetLabel("Gender")%></td>
                <td><label id="lblGender" runat="server"></label></td>
            </tr>
        </table>
    </div>

    <div class="pageTitle" style="height:33px; margin-bottom: 10px;">
        <img class="imgLink" id="imgBackPatientFolderDt" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" style="float:left;" title="<%=GetLabel("Back")%>" />
        <div style="margin-left: 40px;font-size: 1.1em"><%=GetLabel("Patient Folder")%></div>
    </div>
    <div style="padding:10px;" class="borderBox">
        <dx:ASPxFileManager ID="fileManager" ClientInstanceName="fileManager" runat="server" OnFolderCreating="fileManager_FolderCreating" OnCustomThumbnail="fileManager_CustomThumbnail"
            OnItemDeleting="fileManager_ItemDeleting" OnItemMoving="fileManager_ItemMoving"
            OnItemRenaming="fileManager_ItemRenaming">
            <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf" />
            <SettingsUpload Enabled="false"/>
            <SettingsToolbar ShowPath="false" />
            <SettingsEditing AllowCreate="true" AllowDelete="true" AllowRename="true" />
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
                    alert('Cannot Move Item From Body Diagram Folder');
                    e.cancel = true;
                }
            }" FolderCreating="function(s,e){
                var folderName = e.fullName.split('\\')[1];
                if(folderName == 'BodyDiagram'){
                    alert('Cannot Create New Folder Inside Body Diagram Folder');
                    e.cancel = true;
                }
            }" SelectedFileOpened="function(s,e){
                alert('test');
            }" />
        </dx:ASPxFileManager>
    </div>
</asp:Content>
