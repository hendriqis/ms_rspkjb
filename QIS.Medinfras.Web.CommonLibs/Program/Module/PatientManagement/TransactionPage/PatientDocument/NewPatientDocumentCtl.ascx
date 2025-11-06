<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewPatientDocumentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.NewPatientDocumentCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>

<script id="dxss_PatientFolderUploadDocument" type="text/javascript">
    $(function () {
        setDatePicker('<%=txtDocumentDate.ClientID %>');
        $('#<%=txtDocumentDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    });

    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
        var fileName = $('#<%=FileUpload1.ClientID %>').val().replace("C:\\fakepath\\", "");
        var name = fileName.substring(0, fileName.lastIndexOf('.'));
        var extension = fileName.substring(fileName.lastIndexOf('.') + 1);
        var fileNameExtention = name.replaceAll(".", "_") + '.' + extension;

        $('#txtUploadFile').val(fileName);

        //fileName = fileName.split('.')[0];
        $('#<%=txtFileName1.ClientID %>').val(name);
        $('#<%=txtRename1.ClientID %>').val(name);
        $('#<%=txtExtention.ClientID %>').val(extension);

        if ($('#<%=imgPreview.ClientID %>').width() > $('#<%=imgPreview.ClientID %>').height())
            $('#<%=imgPreview.ClientID %>').width('150px');
        else
            $('#<%=imgPreview.ClientID %>').height('150px');
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                $('#<%=imgPreview.ClientID %>').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
        else
            $('#<%=imgPreview.ClientID %>').attr('src', input.value);
    }

    $('#btnUploadFile').click(function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    setDatePicker('<%=txtDocumentDate.ClientID %>');
</script>

<input type="hidden" id="hdnParam" runat="server" value="" />
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnIsAdd" runat="server" value="" />
<table width="100%">
    <colgroup>
        <col />
    </colgroup>
    <tr valign="top">
        <td>
            <table width="100%" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="150px" />
                    <col width="295px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Date") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtDocumentDate" runat="server" Width="120px" CssClass="datepicker" /></td>  
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Type") %></label></td>
                    <td><dx:ASPxComboBox runat="server" ID="cboGCDocumentType" ClientInstanceName="cboGCDocumentType" Width="300px" /></td>  
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Name") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtDocumentName" runat="server" Width="295px" /></td>  
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("File Type") %></label></td>
                    <td><dx:ASPxComboBox runat="server" ID="cboFileType" ClientInstanceName="cboFileType" Width="300px" /></td>  
                </tr>
                <tr id="trUploadFile" runat="server" >
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("File to upload") %></label></td>
                    <td style="padding-left:1px"><input type="text" id="txtUploadFile" style="width:295px" readonly="readonly"/></td>
                    <td style="padding-left:1px"><input type="button" id="btnUploadFile" value="Choose File"/></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rename File To") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtRename1" Width="295px" runat="server" /></td> 
                    <td style="padding-left:1px"><asp:TextBox ID="txtExtention" Width="77px" runat="server" /></td>
                </tr>
                <tr valign="top">
                    <td class="tdLabel" style="padding-top:5px" valign="top"><label class="lblNormal"><%=GetLabel("Notes") %></label></td>
                    <td style="padding-left:1px" colspan="2"><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>  
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <colgroup>
                    <col width="150px" />
                </colgroup>
                <tr>
                    <td></td>
                    <td rowspan="4" style="height:150px; width:150px;border:1px solid ActiveBorder;" align="center">
                        <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                        <img src="" runat="server" id="imgPreview" width="150" height="150" />
                        <div style="display:none">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                    </td>
                    <td style="vertical-align:top">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:TextBox ID="txtFileName1" Width="100%" runat="server" ReadOnly="true" /></td>    
                            </tr>
                            <tr>          
                                <td><%=GetLabel("Allowed extension") %> : jpg,jpeg,png,txt,doc,docx,pdf,dcm,mp4.</td>
                            </tr>
                            <tr>                                
                                <td><%=GetLabel("Maximum upload size") %> : 10MB</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
