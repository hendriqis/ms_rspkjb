<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="CencusInformation.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.CencusInformation" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Refresh")%></div></li>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <style type="text/css">
        .statusHeader
        {
            padding:1px;
            background-color:#09abd2;
            color:Black;
            border:1px solid gray;
        }  
    
        .PatientIN
        {
            color: Blue;
        }  
    
        .PatientOUT
        {
            color: Red;
        }  
    </style>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtLogDate.ClientID %>');

            $('#<%=txtLogDate.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    onRefreshGridView();
                }
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var date =  $('#<%:txtLogDate.ClientID %>').val();
            var ServiceUnit = cboServiceUnit.GetValue()
            var YYYY = date.substring(6, 10);
            var MM = date.substring(3, 5);
            var DD = date.substring(0, 2);
            var dateALL = YYYY + '-' + MM + '-' + DD;

            if (dateALL == '') {
                errMessage.text = 'Please Select Date First!';
                return false;
            }
            else {
                filterExpression.text = "LogDate = '" + dateALL + "'|" + ServiceUnit + "|" + dateALL;
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnLogDate" runat="server" />
    <div style="padding:=5px">        
        <table width="100%">
            <colgroup>
                <col width="60%" />
                <col width="40%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td><label class="lblNormal"><%=GetLabel("Tanggal Sensus") %></label></td>
                            <td><asp:TextBox runat="server" ID="txtLogDate" CssClass="datepicker" Width="120px" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Ruang Perawatan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="350px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top">
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
                                                    <th rowspan="3" align="left" style="font-weight:bold"><%=GetLabel("KELAS PERAWATAN")%></th>
                                                    <th rowspan="3" style="width:70px;font-weight:bold"><%=GetLabel("PASIEN AWAL")%></th>
                                                    <th colspan="2" style="font-weight:bold"><%=GetLabel("MASUK")%></th>
                                                    <th colspan="4" style="font-weight:bold"><%=GetLabel("KELUAR")%></th>
                                                    <th rowspan="3" style="width:70px;font-weight:bold"><%=GetLabel("SISA PASIEN")%></th>                     
                                                </tr>
                                                <tr>  
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Baru")%></th>
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Pindahan")%></th>
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Dipindahkan")%></th>
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Keluar")%></th>
                                                    <th colspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Meninggal")%></th>                     
                                                </tr>
                                                <tr>
                                                    <th style="width:50px"><%=GetLabel("< 48 jam")%></th>
                                                    <th style="width:50px"><%=GetLabel("> 48 jam")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Tidak ada informasi sensus harian di ruang perawatan ini")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th rowspan="3" align="left" style="font-weight:bold"><%=GetLabel("KELAS PERAWATAN")%></th>
                                                    <th rowspan="3" style="width:70px;font-weight:bold"><%=GetLabel("PASIEN AWAL")%></th>
                                                    <th colspan="2" style="font-weight:bold"><%=GetLabel("MASUK")%></th>
                                                    <th colspan="4" style="font-weight:bold"><%=GetLabel("KELUAR")%></th>
                                                    <th rowspan="3" style="width:70px;font-weight:bold"><%=GetLabel("SISA PASIEN")%></th>                     
                                                </tr>
                                                <tr>  
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Baru")%></th>
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Pindahan")%></th>
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Dipindahkan")%></th>
                                                    <th rowspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Keluar")%></th>
                                                    <th colspan="2" style="width:70px;font-weight:bold"><%=GetLabel("Pasien Meninggal")%></th>                     
                                                </tr>
                                                <tr>
                                                    <th style="width:70px;font-weight:bold"><%=GetLabel("< 48 jam")%></th>
                                                    <th style="width:70px;font-weight:bold"><%=GetLabel("> 48 jam")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>                    
                                                <tr id="trFooter" class="trFooter" runat="server">
                                                    <td style="font-weight:bold"><%=GetLabel("TOTAL") %></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalPatientBegin" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalAdmission" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalTransferIN" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalTransferOUT" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalDischarged" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalDied1" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalDied2" runat="server"></td>
                                                    <td style="font-weight:bold" align="right" id="tdTotalStay" runat="server"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="background-color:#EAEAEA; font-weight:bold"><%#:Eval("ClassName") %></td>
                                                <td align="right" style="background-color:#EAEAEA; font-weight:bold"><%#:Eval("cfPatientBegin")%></td>
                                                <td align="right" style="background-color:white"><%#:Eval("cfAdmission")%></td>
                                                <td align="right" style="background-color:white"><%#:Eval("cfTransferIN")%></td>
                                                <td align="right" style="background-color:white"><%#:Eval("cfTransferOUT")%></td>
                                                <td align="right" style="background-color:white"><%#:Eval("cfDischarged")%></td>
                                                <td align="right" style="background-color:white"><%#:Eval("cfDied1")%></td>
                                                <td align="right" style="background-color:white"><%#:Eval("cfDied2")%></td>
                                                <td align="right" style="background-color:#EAEAEA; font-weight:bold"><%#:Eval("cfStay")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
                <td style="vertical-align:top">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <div class="statusHeader">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tdlabel" style="width : 120px; padding-left: 3px">
                                                <label class="lblNormal" >
                                                    <%=GetLabel("Status") %></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboGCATDStatus" Width="400px">
                                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>  
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
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
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                            <HeaderTemplate>
                                                                <div><%=GetLabel("Ruangan") %></div>
                                                                <div><%=GetLabel("Kelas") %></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div><%#:Eval("RoomCode")%></div>
                                                                <div><%#:Eval("ClassName")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div><%=GetLabel("Nama Pasien") %></div>
                                                                <div><span><%=GetLabel("No. RM") %>, <%=GetLabel("No. Registrasi") %></span></div>
                                                                <div><%=GetLabel("Dokter / Paramedis") %></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div><label class="lblNormal" id="lblPatientName"><%#:Eval("PatientName")%></label></div>
                                                                <div><%#:Eval("MedicalNo")%>, <%#:Eval("RegistrationNo")%></div>
                                                                <div style="color:Maroon"><%#:Eval("ParamedicName")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SpecialtyName" HeaderText="Spesialisasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                            <HeaderTemplate>
                                                                <div><%=GetLabel("Lokasi Asal/Tujuan") %></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("cfLocationReference")%>
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
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
