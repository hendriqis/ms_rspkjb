<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalRecordFormContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MedicalRecordFormContentCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        setHtmlEditor();
    });
</script>
<input type="hidden" id="hdnFormID" value="" runat="server" />
<input type="hidden" id="hdnFormName" value="" runat="server" />
<input type="hidden" id="hdnFormContent" value="" runat="server" />
<div style="height:445px">
    <table class="tblContentArea">
        <colgroup>
            <col width="60%" />
            <col width="40%" />
        </colgroup>
        <tr valign="top">
            <td>
                <table width="100%">
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Form Rekam Medis")%></label></td>
                        <td><asp:TextBox ID="txtFormInfo" Width="100%" runat="server" ReadOnly="true" /></td>      
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                    <asp:TextBox TextMode="MultiLine" Width="100%" Height="400px" ID="txtContent" runat="server" CssClass="htmlEditor" />
            </td>
        </tr>
    </table>
</div>

