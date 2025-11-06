<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="PatientIndicatorSummary.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientIndicatorSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <colgroup>
            <col width="40%" />
            <col width="20%" />
            <col width="40%" />
        </colgroup>
        <tr>
            <td>
                <asp:ListView runat="server" ID="lvwViewVS" OnItemDataBound="lvwViewVS_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all"
                            style="font-size: 0.9em">
                            <tr>
                                <th rowspan="2" align="left">
                                    <%=GetLabel("VITAL SIGN")%>
                                </th>
                                <th colspan="3" align="center">
                                    <%=GetLabel("SUMMARY")%>
                                </th>
                                <th colspan="2" align="center">
                                    <%=GetLabel("LAST")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MIN")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MAX")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("AVERAGE")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("VALUE")%>
                                </th>
                                <th style="width: 70px" align="center">
                                    <%=GetLabel("DATE")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="5">
                                    <%=GetLabel("There is no record to display")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all"
                            style="font-size: 0.9em">
                            <tr>
                                <th rowspan="2" align="left">
                                    <%=GetLabel("VITAL SIGN")%>
                                </th>
                                <th colspan="3" align="center">
                                    <%=GetLabel("SUMMARY")%>
                                </th>
                                <th colspan="2" align="center">
                                    <%=GetLabel("LAST")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MIN")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MAX")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("AVERAGE")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("VALUE")%>
                                </th>
                                <th style="width: 70px" align="center">
                                    <%=GetLabel("DATE")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdItemName">
                                <label class="lblItemName">
                                    <%#: Eval("VitalSignLabel")%></label>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtVSMinValue" Width="55px" runat="server" value="0" CssClass="number txtMinValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtVSMaxValue" Width="55px" runat="server" value="0" CssClass="number txtMaxValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtVSAverageValue" Width="55px" runat="server" value="0" CssClass="number txtAverageValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtVSLastValue" Width="55px" runat="server" value="0" CssClass="number txtAverageValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtVSLastDate" Width="70px" runat="server" value="" CssClass="date txtLastDate" style="text-align:center"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </td>
            <td>&nbsp</td>
            <td style="padding-left: 10px; vertical-align: top">
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all"
                            style="font-size: 0.9em">
                            <tr>
                                <th rowspan="2" align="left">
                                    <%=GetLabel("LABORATORY")%>
                                </th>
                                <th colspan="3" align="center">
                                    <%=GetLabel("SUMMARY")%>
                                </th>
                                <th colspan="2" align="center">
                                    <%=GetLabel("LAST")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MIN")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MAX")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("AVERAGE")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("VALUE")%>
                                </th>
                                <th style="width: 70px" align="center">
                                    <%=GetLabel("DATE")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="5">
                                    <%=GetLabel("There is no record to display")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all"
                            style="font-size: 0.9em">
                            <tr>
                                <th rowspan="2" align="left">
                                    <%=GetLabel("LABORATORY")%>
                                </th>
                                <th colspan="3" align="center">
                                    <%=GetLabel("SUMMARY")%>
                                </th>
                                <th colspan="2" align="center">
                                    <%=GetLabel("LAST")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MIN")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("MAX")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("AVERAGE")%>
                                </th>
                                <th style="width: 55px" align="center">
                                    <%=GetLabel("VALUE")%>
                                </th>
                                <th style="width: 70px" align="center">
                                    <%=GetLabel("DATE")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdItemName">
                                <label class="lblItemName">
                                    <%#: Eval("FractionName1")%></label>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtMinValue" Width="55px" runat="server" value="0" CssClass="number txtMinValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtMaxValue" Width="55px" runat="server" value="0" CssClass="number txtMaxValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtAverageValue" Width="55px" runat="server" value="0" CssClass="number txtAverageValue"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtLastValue" Width="55px" runat="server" value="" CssClass="number txtLastDate"
                                    ReadOnly="true" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtLastDate" Width="70px" runat="server" value="" CssClass="date txtLastDate" style="text-align:center"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>
</asp:Content>
