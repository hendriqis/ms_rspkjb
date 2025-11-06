<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientFolderUploadDocumentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientFolderUploadDocumentCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>

<script id="dxss_PatientFolderUploadDocument" type="text/javascript">
    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
        var fileName = $('#<%=FileUpload1.ClientID %>').val().replace("C:\\fakepath\\", "");
        $('#txtUploadFile').val(fileName);

        //fileName = fileName.split('.')[0];
        $('#<%=txtFileName1.ClientID %>').val(fileName);
        $('#<%=txtRename1.ClientID %>').val(fileName);
        if ($('#previewImage').width() > $('#previewImage').height())
            $('#previewImage').width('150px');
        else
            $('#previewImage').height('150px');
    });

    function readURL (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                $('#previewImage').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
        else
            $('#previewImage').attr('src', input.value);
    }

    $('#btnUploadFile').click(function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    setDatePicker('<%=txtDocumentDate.ClientID %>');

    function onBeforeSaveRecord(errMessage) {
        var file = $('#<%=FileUpload1.ClientID %>').val();
        var reg = /(.*?)\.(jpg|jpeg|png|txt|doc|docx|pdf|JPG|JPEG|PNG|TXT|DOC|DOCX|PDF|DCM|MP4)$/;
        if (!file.match(reg)) {
            displayErrorMessageBox('MEDINFRAS', "Maaf, format file tidak sesuai");
            return false;
        }
        return true;
    }

    function onBeforeSaveRecordEntryPopup(errMessage) {
        var file = $('#<%=FileUpload1.ClientID %>').val();
        var reg = /(.*?)\.(jpg|jpeg|png|txt|doc|docx|pdf|JPG|JPEG|PNG|TXT|DOC|DOCX|PDF|DCM|MP4)$/;
        if (!file.match(reg)) {
            displayErrorMessageBox('MEDINFRAS', "Maaf, format file tidak sesuai");
            return false;
        }
        return true;
    }

</script>
<input type="hidden" id="hdnParam" runat="server" value="" />
<input type="hidden" id="hdnFolder" runat="server" value="" />
<table width="100%">
    <colgroup>
        <col width="50%" />
        <col width="50%" />
    </colgroup>
    <tr valign="top">
        <td>
            <table width="100%">
                <colgroup>
                    <col width="150px" />
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Type") %></label></td>
                    <td><dx:ASPxComboBox runat="server" ID="cboGCDocumentType" ClientInstanceName="cboGCDocumentType" Width="100%" /></td>  
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Name") %></label></td>
                    <td><asp:TextBox ID="txtDocumentName1" runat="server" Width="100%" /></td>  
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Date") %></label></td>
                    <td><asp:TextBox ID="txtDocumentDate" runat="server" Width="120px" CssClass="datepicker" /></td>  
                </tr>
                <tr valign="top">
                    <td class="tdLabel" style="padding-top:5px" valign="top"><label class="lblNormal"><%=GetLabel("Notes") %></label></td>
                    <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="5" /></td>  
                </tr>
            </table>
        </td>
        <td>
            <table width="100%">
                <colgroup>
                    <col width="150px" />
                </colgroup>
                <tr>
                    <td rowspan="4" style="height:150px; width:150px;border:1px solid ActiveBorder;" align="center">
                        <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                        <img src="" id="previewImage" width="150px" height="150px" />
                    </td>
                    <td>
                        <div style="display:none">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <table cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width:180px"/>
                                <col style="width:30px"/>
                            </colgroup>
                            <tr>
                                <td><input type="text" id="txtUploadFile" style="width:97%" readonly="readonly"/></td>
                                <td style="padding:3px;padding-bottom:5px"><input type="button" id="btnUploadFile" value="Browse"/></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>          
                    <td><%=GetLabel("Allowed extension") %> : jpg,jpeg,png,txt,doc,docx,pdf,dcm,mp4.</td>
                </tr>
                <tr>                                
                    <td><%=GetLabel("Maximum upload size") %> : 10MB</td>
                </tr>
                <tr><td>&nbsp</td></tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Uploaded File") %></label></td>
                    <td><asp:TextBox ID="txtFileName1" Width="150px" runat="server" ReadOnly="true" /></td>    
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rename File To") %></label></td>
                    <td><asp:TextBox ID="txtRename1" Width="150px" runat="server" /></td>    
                </tr>
            </table>
        </td>
    </tr>
</table>