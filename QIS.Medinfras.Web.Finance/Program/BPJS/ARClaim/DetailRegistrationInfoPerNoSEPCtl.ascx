<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailRegistrationInfoPerNoSEPCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.DetailRegistrationInfoPerNoSEPCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_DetailRegistrationInfoPerNoSEPCtl">

</script>
<div id="containerPopup">
    <input type="hidden" id="hdnNoSEPCtl" runat="server" value="" />
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
                            <%=GetLabel("No SEP")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoSEP" Width="170px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetailInfo" runat="server" Width="100%" ClientInstanceName="cbpProcessDetailInfo"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetailInfo_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpProcessDetailInfoEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th>
                                                        <%=GetLabel("Registration Information")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Patient")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Payment Information")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Payment Amount")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("AR Information")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("AR Claimed Amount")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("AR Payment Amount")%>
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
                                                    <th>
                                                        <%=GetLabel("Registration Information")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Patient")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Payment Information")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Payment Amount")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("AR Information")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("AR Claimed Amount")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("AR Payment Amount")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <div>
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("RegistrationNo") %></label></div>
                                                    <div>
                                                        <label>
                                                            <%#: Eval("cfRegistrationDateInString") %></label></div>
                                                    <div>
                                                        <label style="font-style: italic; font-size: x-small">
                                                            <%#: Eval("DepartmentID") %></label></div>
                                                    <div>
                                                        <label style="font-style: italic; font-size: x-small">
                                                            <%#: Eval("ServiceUnitName") %></label></div>
                                                    <div>
                                                        <label style="font-style: italic; font-size: x-small">
                                                            <%#: Eval("BusinessPartnerName") %></label></div>
                                                    <div>
                                                        <label>
                                                            <%=GetLabel("Status = ")%><b><%#: Eval("RegistrationStatus") %></b></label></div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <label style="font-style: italic">
                                                            <%#: Eval("MedicalNo") %></label></div>
                                                    <div>
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("PatientName") %></label></div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("PaymentNo") %></label></div>
                                                    <div>
                                                        <label>
                                                            <%#: Eval("cfPaymentDateInString") %></label></div>
                                                    <div>
                                                        <label style="font-style: italic; font-size: x-small">
                                                            <%#: Eval("PaymenBusinessPartnerName") %></label></div>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfPaymentAmountInString")%>
                                                </td>
                                                <td>
                                                    <div>
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("ARInvoiceNo") %></label></div>
                                                    <div>
                                                        <label>
                                                            <%#: Eval("cfARInvoiceDateInString") %></label></div>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfARClaimedAmountInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfARPaymentAmountInString")%>
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
</div>
