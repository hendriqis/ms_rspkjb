<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RLReportColCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RLReportColCtl" %>


<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
 <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>



<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
     var isChangeTagField = false;
     function onCbpViewEndCallback() {
         $('#containerImgLoadingView').hide();
         isChangeTagField = false;
         $('.txtValue').change(function () {
             isChangeTagField = true;
         });
     }
</script>

<div style="height:440px; overflow-y:auto; overflow-x:hidden">
<input type="hidden" id="hdnReportID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Kolom Laporan RL")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%" />
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:120%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Laporan RL")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHeaderText" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="Index" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />                                
                                        <asp:BoundField DataField="RLReportID" HeaderText="Nama Kolom" HeaderStyle-Width="200px" />
                                        <asp:TemplateField>
                                            <HeaderTemplate><%=GetLabel("Nilai") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValue" CssClass="txtValue" runat="server" Width="100%" Text='<%#:Eval("Value") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>    
           </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>