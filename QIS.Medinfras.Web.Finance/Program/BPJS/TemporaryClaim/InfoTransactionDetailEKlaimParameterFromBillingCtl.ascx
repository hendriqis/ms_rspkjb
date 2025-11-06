<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoTransactionDetailEKlaimParameterFromBillingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InfoTransactionDetailEKlaimParameterFromBillingCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_InfoTransactionDetailEKlaimParameterFromBillingCtl">

</script>
<div id="containerPopup">
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left">
                <table>
                    <colgroup>
                        <col style="width: 140px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <%=GetLabel("Registration No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="170px" runat="server" ReadOnly="true"
                                Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("SEP No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSEPNo" Width="170px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Patient")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatient" Width="350px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <table width="600px" runat="server" cellspacing="0" rules="all">
                    <colgroup>
                        <col style="width: 170px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <b>
                                <%=GetLabel("Amount")%></b>
                        </td>
                        <td style="text-align: right">
                            <b>
                                <%=GetLabel("Patient")%></b>
                        </td>
                        <td style="text-align: right">
                            <b>
                                <%=GetLabel("Payer")%></b>
                        </td>
                        <td style="text-align: right">
                            <b>
                                <%=GetLabel("Total")%></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Seluruhnya")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatientAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPayerAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalLineAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right; font-weight: bold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Obat (Ditagihkan)")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatientObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPayerObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalLineObatDitagihkanAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right; font-weight: bold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Tanpa Obat (Ditagihkan)")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatientTanpaObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPayerTanpaObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalLineTanpaObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right; font-weight: bold" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("E-Klaim Parameter Code")%>
                                                    </th>
                                                    <th style="width: 150px" align="left">
                                                        <%=GetLabel("E-Klaim Parameter Name")%>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("Item Code")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Item Name")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Patient Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Payer Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Line Amount")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("E-Klaim Parameter Code")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("E-Klaim Parameter Name")%>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("Item Code")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Item Name")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Patient Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Payer Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Line Amount")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <input type="hidden" class="EKlaimParameterID" id="EKlaimParameterID" runat="server"
                                                        value='<%#: Eval("EKlaimParameterID")%>' />
                                                    <%#: Eval("EKlaimParameterCode")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("EKlaimParameterName")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemCode")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemName1")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfPatientAmountInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfPayerAmountInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfLineAmountInString")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging" style="display: none">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
