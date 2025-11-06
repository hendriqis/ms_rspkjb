<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EklaimPatientDocumentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.PatientPage.EklaimPatientDocumentCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<script id="dxss_tempclaimuploaddocument" type="text/javascript">
    $(function () {
        setDatePicker('<%=txtDocumentDate.ClientID %>');
        $('#<%=txtDocumentDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });
    setDatePicker('<%=txtDocumentDate.ClientID %>');

       $('#<%=btnSend.ClientID %>').live('click', function () {
       
        cbpProcess.PerformCallback('uploadFileEklaim');
    });
    function onCbpProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == "uploadFileEklaim") {
            if (param[1] == 'success') {
                var respInfo = JSON.parse(param[2]);
                if (respInfo.metadata.code != "200") {
                    alert(respInfo.metadata.message);
                } else {
                    alert(respInfo.metadata.message);
                    
                }
            }
            else 
            {
                showToast('Failed', 'Error Message : ' + param[2]);
            }
        }  
    }
</script>

<input type="hidden" id="hdnParam" runat="server" value="" />
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnRegistraionID" runat="server" value="" />
<input type="hidden" id="hdnMedicalNo" runat="server" value="" />

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
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("No SEP") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtSep" runat="server" Width="120px" CssClass="datepicker" /></td>  
                </tr>
                 <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("No Peserta BPJS") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtNoPeserta" runat="server" Width="120px" CssClass="datepicker" /></td>  
                </tr>
                 <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtFullname" runat="server" Width="120px" CssClass="datepicker" /></td>  
                </tr>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Date") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox ID="txtDocumentDate" runat="server" Width="120px" CssClass="datepicker" /></td>  
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Document Type") %></label></td>
                    <td><dx:ASPxComboBox runat="server" ID="cboGCDocumentType" ClientInstanceName="cboGCDocumentType" Width="300px" /></td>  
                </tr>
                 <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("File Class E-klaim") %></label></td>
                    <td><dx:ASPxComboBox runat="server" ID="cboFileClass" ClientInstanceName="cboFileClass" Width="300px"  /></td>  
                </tr>
                 
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("File to upload") %></label></td>
                    <td style="padding-left:1px"><asp:TextBox  type="text" runat="server" id="txtFileName" Width="300px"  /></td> 
                </tr>
                <tr>
                    <td></td>
                    <td><input type="button" id="btnSend" runat="server" value="Upload" /></td>
                </tr>
            </table>
        </td>
    </tr>
    
</table>

    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>