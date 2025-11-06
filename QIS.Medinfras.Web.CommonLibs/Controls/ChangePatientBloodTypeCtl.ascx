<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePatientBloodTypeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangePatientBloodTypeCtl" %>
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
         
         cbpPatientBlood.PerformCallback('update');
    });

    function onCbpViewEndCallback(s) {
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
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 80px" />
                <col />
            </colgroup>
            <tr>
               <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Medical No.")%></label>
                </td>
                <td>
                     <asp:TextBox ID="txtMedicalNo" runat="server" Enabled="false" />
                </td>
            </tr>
            <tr>
               <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Nama Pasien")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientName" runat="server"   Enabled="false"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Golongan Darah")%></label>
                </td>
                  <td>
                                            <dxe:ASPxComboBox ID="cboBloodType" ClientInstanceName="cboBloodType" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboBloodRhesus" ClientInstanceName="cboBloodRhesus" Width="100%"
                                                runat="server" />
                                        </td>
               </tr>
                
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPatientBlood" runat="server" Width="100%" ClientInstanceName="cbpPatientBlood"
            ShowLoadingPanel="false" OnCallback="cbpPatientBlood_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    hideLoadingPanel();
                    onCbpViewEndCallback(s);
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
