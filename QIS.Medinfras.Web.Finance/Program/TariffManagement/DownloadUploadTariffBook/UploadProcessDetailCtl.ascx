<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadProcessDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.UploadProcessDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_UploadProcessDetailCtl">

    function onBeforeSaveRecord(errMessage) {
        return true;
    }
    function onAfterSaveAddRecordEntryPopup(param) {

        cbpProcessDetail.PerformCallback('refresh');
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden; position:relative" id="containerPopup">
    <input type="hidden" id="hdnBookIDCtl" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 120px" />
            <col />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetailCtl" runat="server" Width="100%" ClientInstanceName="cbpProcessDetailCtl"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetailCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Jenis Item")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Item")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Kelas")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff Comp 1")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff Comp 2")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff Comp 3")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Informasi Upload")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="20">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Jenis Item")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Item")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Kelas")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff Comp 1")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff Comp 2")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Tariff Comp 3")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Informasi Upload")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <%#: Eval("ItemType")%>
                                            </td>
                                            <td align="left">
                                                <i><%#: Eval("ItemCode")%></i> <br />
                                                <b><%#: Eval("ItemName1") %></b>
                                            </td>
                                            <td align="left">
                                                <i><%#: Eval("ClassCode")%></i><br />
                                                <b><%#: Eval("ClassName") %></b>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfProposedTariff") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfProposedTariffComp1") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfProposedTariffComp2") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfProposedTariffComp3") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("CreatedByFullName") %> <br />
                                                <%#: Eval("cfCreatedDateInString") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
