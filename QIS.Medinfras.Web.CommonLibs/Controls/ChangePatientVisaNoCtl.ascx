<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePatientVisaNoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangePatientVisaNoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_nursingnotesperdate">

    $(function () {
        hideLoadingPanel();
    });
     $('#<%=btnsave.ClientID %>').live('click', function () {

         cbpPatientReg.PerformCallback('update');
    });

     function onCbpPatientRegViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'update') {
            if (param[1] == 'fail') {
                showToast('update Failed', 'Error Message : ' + param[2]);
            }
            else {
                showToast('Update Success');
            }
        }

        pcRightPanelContent.Hide();
          
    }
</script>
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnsave" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbSave.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        
        <table>
             
             <tr>
               <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Registration No.")%></label>
                </td>
                <td>
                     <asp:TextBox ID="txtRegNo" runat="server" Enabled="false" Width="250px"  />
                </td>
            </tr>
            <tr>
               <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Medical No.")%></label>
                </td>
                <td>
                     <asp:TextBox ID="txtMedicalNo" runat="server" Enabled="false" Width="250px"   />
                </td>
            </tr>
            <tr>
               <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Nama Pasien")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientName" runat="server"   Enabled="false" Width="250px"  />
                </td>
            </tr>
            <tr>
               <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Nomor Visa")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientVisaNumber" runat="server"  Width="250px"  />
                </td>
            </tr>
            
                
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPatientReg" runat="server" Width="100%" ClientInstanceName="cbpPatientReg"
            ShowLoadingPanel="false" OnCallback="cbpPatientReg_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    hideLoadingPanel();
                    onCbpPatientRegViewEndCallback(s);
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
