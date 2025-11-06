<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="PatientFolderDetail2.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientFolderDetail2" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">   
    <script type="text/javascript">
        $('#imgBackPatientFolderDt').live('click', function () {
            document.location = document.referrer;
        });
    </script>

    <div class="pageTitle" style="height:33px; margin-bottom: 10px;">
        <img class="imgLink" id="imgBackPatientFolderDt" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" style="float:left;" title="<%=GetLabel("Back")%>" />
        <div style="margin-left: 40px;font-size: 1.1em"><%=GetLabel("Patient Folder")%></div>
    </div>
    <div style="height: 80px;">
        <img src='<%= ResolveUrl("~/Libs/Images/patient_male.png")%>' alt="" width="55px" style="float:left;" />
        <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0" >
            <col style="width:100px"/>
            <col style="width:145px"/>
            <col style="width:100px"/>
            <col style="width:180px"/>
            <col style="width:100px"/>
            <col style="width:240px"/>
            <tr style="font-size:1.2em">
                <td colspan="3" style="padding-left:20px"><label id="lblPatientName" runat="server"></label></td>
                <td colspan="2"><label id="lblRegistrationNo" runat="server"></label></td>
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
    <div style="padding:10px;" class="borderBox">
        <dx:ASPxFileManager ID="fileManager" runat="server" OnFolderCreating="fileManager_FolderCreating" OnCustomThumbnail="fileManager_CustomThumbnail"
            OnItemDeleting="fileManager_ItemDeleting" OnItemMoving="fileManager_ItemMoving"
            OnItemRenaming="fileManager_ItemRenaming" OnFileUploading="fileManager_FileUploading">
            <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf" />
            <SettingsToolbar ShowPath="false" />
            <SettingsEditing AllowCreate="true" AllowDelete="true" AllowMove="true" AllowRename="true" />
            <SettingsPermissions>
                <AccessRules>
                    <dx:FileManagerFolderAccessRule Path="System" Edit="Deny" />
                </AccessRules>
            </SettingsPermissions>
        </dx:ASPxFileManager>
    </div>
</asp:Content>
