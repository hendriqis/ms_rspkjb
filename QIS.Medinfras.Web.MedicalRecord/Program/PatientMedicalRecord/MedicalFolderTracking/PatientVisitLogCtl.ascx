<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientVisitLogCtl.ascx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientVisitLogCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_patientvisitlogctl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, null, 1);
    });

    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpEntryPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnMRN" runat="server" />
    <input type="hidden" id="hdnFromDate" runat="server" />
    <input type="hidden" id="hdnToDate" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="VisitDateInString" HeaderText="Tanggal" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="VisitTime" HeaderText="Jam" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="No. Registrasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"  >
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td align="left"><%#: Eval("RegistrationNo")%></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"><%#: Eval("PatientName")%></td>
                                                    </tr>
                                                </table>  
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" >
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td align="left"><%#: Eval("cfPatientLocation")%></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"><%#: Eval("ParamedicName")%></td>
                                                    </tr>
                                                </table>  
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </td>
        </tr>
    </table>
</div>