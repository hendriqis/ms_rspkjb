<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PhysicianBalanceInformation.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PhysicianBalanceInformation" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'>
    </script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=txtDateFrom.ClientID %>').change(function () {
                var start = $('#<%=txtDateFrom.ClientID %>').val();
                var end = $('#<%=txtDateTo.ClientID %>').val();

                $('#<%=txtDateFrom.ClientID %>').val(validateDateFromTo(start, end));                
            });

            $('#<%=txtDateTo.ClientID %>').change(function () {
                var start = $('#<%=txtDateFrom.ClientID %>').val();
                var end = $('#<%=txtDateTo.ClientID %>').val();

                $('#<%=txtDateTo.ClientID %>').val(validateDateToFrom(start, end));                
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });
        });

        function onCboDepartmentChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnParamDepartmentID.ClientID %>').val(value);            
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle()) %></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Refresh")%></div></li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">       
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
    <input type="hidden" id="hdnParamDepartmentID" runat="server" />
    <table class="tblEntryContent" style="width: 650px;">
        <colgroup>
            <col style="width: 200px" />
            <col style="width: 600px" />
        </colgroup>
        <tr>
            <td>
                <label>
                    <%=GetLabel("Tanggal Transaksi") %></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                        </td>
                        <td>
                            s/d
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("Department Transaksi") %></label>
            </td>
            <td colspan="3">
                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="50%"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){onCbpViewEndCallback(s)}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RowNumber" HeaderText="No. Urut" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Registrasi") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Department : ")%>
                                                        <%#:Eval("DepartmentID") %>
                                                    </div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("No Registrasi : ")%>
                                                        <%#:Eval("RegistrationNo") %>
                                                    </div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Pasien : ")%>
                                                        (<%#:Eval("MedicalNo") %>)
                                                        <%#:Eval("PatientName") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BusinessPartnerName" HeaderText="Penjamin Bayar" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Transaksi") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("No Transaksi : ")%>
                                                        <%#:Eval("TransactionNo") %>
                                                    </div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Tanggal Transaksi : ")%>
                                                        <%#:Eval("TransactionDateInString") %>
                                                    </div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Item : ")%>
                                                        (<%#:Eval("ItemCode") %>)
                                                        <%#:Eval("ItemName1") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Jasa Dokter") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: right; font-size: 0.95em">
                                                        <%#:Eval("SharingAmount","{0:N2}")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
