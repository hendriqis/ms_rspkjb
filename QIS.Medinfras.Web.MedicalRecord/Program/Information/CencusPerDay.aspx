<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="CencusPerDay.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CencusPerDay" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%--<%@ Register Src="~/Program/Information/PivotOptionCtl.ascx" TagName="PivotOptionCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PeriodeCtl.ascx" TagName="PeriodeCtl" TagPrefix="uc2" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtLogDate.ClientID %>');

            $('#<%=txtLogDate.ClientID %>').change(function () {
                onRefreshGridView();
            });

        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onRefreshGridView() {
            cbpView.PerformCallback('refresh');
            cbpView2.PerformCallback();
        }
    </script>
    <div style="padding:=15px">
        <table width="100%">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Ruang Perawatan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td><label class="lblNormal"><%=GetLabel("Tanggal") %></label></td>
                            <td><asp:TextBox runat="server" ID="txtLogDate" CssClass="datepicker" Width="120px" /></td>
                        </tr>
                        <tr>
                            <td><label class="lblNormal"><%=GetLabel("Status") %></label></td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCATDStatus" Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No. Rekam Medis" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama" ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="ClassName" HeaderText="Kelas" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="RoomName" HeaderText="Ruangan" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="SpecialtyName" HeaderText="Spesialisasi" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <div style="font-size:20px; border-bottom: 1px solid #AAA"><%=GetLabel("Rekapitulasi")%></div>
                    <dxcp:ASPxCallbackPanel ID="cbpView2" runat="server" Width="100%" ClientInstanceName="cbpView2" ShowLoadingPanel="false" OnCallback="cbpView2_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpView2EndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th rowspan="3"><%=GetLabel("Spesialisasi")%></th>
                                                    <th rowspan="3" style="width:50px"><%=GetLabel("Pasien Awal")%></th>
                                                    <th colspan="2"><%=GetLabel("Pasien Masuk")%></th>
                                                    <th colspan="8"><%=GetLabel("Pasien Keluar")%></th>
                                                    <th rowspan="3" style="width:50px"><%=GetLabel("Sisa Pasien")%></th>                     
                                                </tr>
                                                <tr>  
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Pindahan")%></th>
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Baru")%></th>
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Dipindahkan")%></th>
                                                    <th colspan="6"><%=GetLabel("Pasien Keluar Hidup")%></th>
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Meninggal")%></th>                     
                                                </tr>
                                                <tr>
                                                    <th style="width:50px"><%=GetLabel("VVIP")%></th>
                                                    <th style="width:50px"><%=GetLabel("VIP")%></th>
                                                    <th style="width:50px"><%=GetLabel("I")%></th>
                                                    <th style="width:50px"><%=GetLabel("II")%></th>
                                                    <th style="width:50px"><%=GetLabel("III")%></th>
                                                    <th style="width:50px"><%=GetLabel("Non Kls")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th rowspan="3"><%=GetLabel("Spesialisasi")%></th>
                                                    <th rowspan="3" style="width:50px"><%=GetLabel("Pasien Awal")%></th>
                                                    <th colspan="2"><%=GetLabel("Pasien Masuk")%></th>
                                                    <th colspan="8"><%=GetLabel("Pasien Keluar")%></th>
                                                    <th rowspan="3" style="width:50px"><%=GetLabel("Sisa Pasien")%></th>                     
                                                </tr>
                                                <tr>  
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Pindahan")%></th>
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Baru")%></th>
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Dipindahkan")%></th>
                                                    <th colspan="6"><%=GetLabel("Pasien Keluar Hidup")%></th>
                                                    <th rowspan="2" style="width:50px"><%=GetLabel("Pasien Meninggal")%></th>                     
                                                </tr>
                                                <tr>
                                                    <th style="width:50px"><%=GetLabel("VVIP")%></th>
                                                    <th style="width:50px"><%=GetLabel("VIP")%></th>
                                                    <th style="width:50px"><%=GetLabel("I")%></th>
                                                    <th style="width:50px"><%=GetLabel("II")%></th>
                                                    <th style="width:50px"><%=GetLabel("III")%></th>
                                                    <th style="width:50px"><%=GetLabel("Non Kls")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>                    
                                                <tr id="trFooter" class="trFooter" runat="server">
                                                    <td><%=GetLabel("Total") %></td>
                                                    <td align="right" id="tdTotalPatientBegin" runat="server"></td>
                                                    <td align="right" id="tdTotalTransferIN" runat="server"></td>
                                                    <td align="right" id="tdTotalAdmission" runat="server"></td>
                                                    <td align="right" id="tdTotalTransferOUT" runat="server"></td>
                                                    <td align="right" id="tdTotalDischargeVVIP" runat="server"></td>
                                                    <td align="right" id="tdTotalDischargeVIP" runat="server"></td>
                                                    <td align="right" id="tdTotalDischargeI" runat="server"></td>
                                                    <td align="right" id="tdTotalDischargeII" runat="server"></td>
                                                    <td align="right" id="tdTotalDischargeIII" runat="server"></td>
                                                    <td align="right" id="tdTotalDischargeNonKelas" runat="server"></td>
                                                    <td align="right" id="tdTotalDied" runat="server"></td>
                                                    <td align="right" id="tdTotalStay" runat="server"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#:Eval("SpecialtyName") %></td>
                                                <td align="right"><%#:Eval("cfPatientBegin")%></td>
                                                <td align="right"><%#:Eval("cfTransferIN")%></td>
                                                <td align="right"><%#:Eval("cfAdmission")%></td>
                                                <td align="right"><%#:Eval("cfTransferOUT")%></td>
                                                <td align="right"><%#:Eval("cfDischargeVVIP")%></td>
                                                <td align="right"><%#:Eval("cfDischargeVIP")%></td>
                                                <td align="right"><%#:Eval("cfDischargeI")%></td>
                                                <td align="right"><%#:Eval("cfDischargeII")%></td>
                                                <td align="right"><%#:Eval("cfDischargeIII")%></td>
                                                <td align="right"><%#:Eval("cfDischargeNonKelas")%></td>
                                                <td align="right"><%#:Eval("cfDied")%></td>
                                                <td align="right"><%#:Eval("cfStay")%></td>
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
    </div>
</asp:Content>
