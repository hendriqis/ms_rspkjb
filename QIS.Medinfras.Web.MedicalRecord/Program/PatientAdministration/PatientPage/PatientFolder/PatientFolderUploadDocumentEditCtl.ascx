<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientFolderUploadDocumentEditCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientFolderUploadDocumentEditCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>

<script id="dxss_PatientFolderUploadDocument" type="text/javascript">
    setDatePicker('<%=txtDocumentDate.ClientID %>');
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnParam" runat="server" value="" />
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
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("File Name") %></label></td>
                    <td><asp:TextBox ID="txtFileName" ReadOnly="true" Width="100%" runat="server" /></td>    
                </tr>
                <tr valign="top">
                    <td class="tdLabel" style="padding-top:5px" valign="top"><label class="lblNormal"><%=GetLabel("Notes") %></label></td>
                    <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="5" /></td>  
                </tr>
            </table>
        </td>
        <td>
            <table>
                <colgroup>
                    <col width="150px" />
                </colgroup>
                <tr>
                    <td rowspan="4" style="height:150px; width:150px;border:1px solid ActiveBorder;" align="center">
                        <img src="" runat="server" id="previewImage" width="150" height="150" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>