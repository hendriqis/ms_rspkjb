<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="TransactionPatientInformationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPatientInformationList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region listview
        function onCboChanged() {
            onRefreshGridView();
        }

        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            }); 
            
            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        $('.lblRegistrationNo').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

//        function onRefreshGridView() {
//            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
//                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
//                cbpView.PerformCallback('refresh');
//            }
//        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            if (pageCount > 100) pageCount = 100;
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetLabel("Informasi Pending Transaksi Pasien ")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width:140px"/>
                            </colgroup>
                            <tr  id="trServiceUnitName" runat="server">
                                <td><%=GetServiceUnitLabel()%></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" ClientInstanceName="cboKlinik"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trRegistrationDate" runat="server">
                                <td><%=GetLabel("Tanggal") %></td>
                                <td><asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                        </div>
                    </fieldset>

                </td>
            </tr>
            <tr>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th rowspan="2"><%=GetLabel("Informasi Registrasi") %></th>
                                                    <th rowspan="2" style="width: 350px"><%=GetLabel("Informasi Pasien") %></th>
                                                    <th colspan="2"><%=GetLabel("Outstanding Resep")%></th>
                                                    <th colspan="3"><%=GetLabel("Outstanding Order Penunjang Medis")%></th>
                                                    <th colspan="2"><%=GetLabel("Outstanding Transaksi")%></th>
                                                    <th colspan="3"><%=GetLabel("Outstanding Posting")%></th>
                                                </tr>
                                                <tr>
                                                    <th style="width:65px"><%=GetLabel("Resep") %></th>
                                                    <th style="width:65px"><%=GetLabel("Retur Resep") %></th>
                                                    <th style="width:65px"><%=GetLabel("Lab") %></th>
                                                    <th style="width:65px"><%=GetLabel("Radiologi") %></th>
                                                    <th style="width:65px"><%=GetLabel("Other") %></th>
                                                    <th style="width:65px"><%=GetLabel("Transaksi") %></th>
                                                    <th style="width:65px"><%=GetLabel("Bill") %></th>
                                                    <th style="width:65px"><%=GetLabel("Obat") %></th>
                                                    <th style="width:65px"><%=GetLabel("Alkes") %></th>
                                                    <th style="width:65px"><%=GetLabel("Barang Umum") %></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="13">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th rowspan="2"><%=GetLabel("Informasi Registrasi") %></th>
                                                    <th rowspan="2" style="width: 350px"><%=GetLabel("Informasi Pasien") %></th>
                                                    <th colspan="2"><%=GetLabel("Outstanding Resep")%></th>
                                                    <th colspan="3"><%=GetLabel("Outstanding Order Penunjang Medis")%></th>
                                                    <th colspan="2"><%=GetLabel("Outstanding Transaksi")%></th>
                                                    <th colspan="3"><%=GetLabel("Outstanding Posting")%></th>
                                                </tr>
                                                <tr>
                                                    <th style="width:65px"><%=GetLabel("Resep") %></th>
                                                    <th style="width:65px"><%=GetLabel("Retur Resep") %></th>
                                                    <th style="width:65px"><%=GetLabel("Lab") %></th>
                                                    <th style="width:65px"><%=GetLabel("Radiologi") %></th>
                                                    <th style="width:65px"><%=GetLabel("Other") %></th>
                                                    <th style="width:65px"><%=GetLabel("Transaksi") %></th>
                                                    <th style="width:65px"><%=GetLabel("Bill") %></th>
                                                    <th style="width:65px"><%=GetLabel("Obat") %></th>
                                                    <th style="width:65px"><%=GetLabel("Alkes") %></th>
                                                    <th style="width:65px"><%=GetLabel("Barang Umum") %></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                    <a class="lblLink lblRegistrationNo"><%#: Eval("RegistrationNo")%></a>
                                                    <div><%#: Eval("ServiceUnitName")%></div>
                                                </td>
                                                <td><%#: Eval("PatientName")%> (<%#: Eval("MedicalNo")%>)</td>
                                                <td align="right"><%#: Eval("PrescriptionOrder")%></td>
                                                <td align="right"><%#: Eval("PrescriptionReturnOrder")%></td>
                                                <td align="right"><%#: Eval("LaboratoriumOrder")%></td>
                                                <td align="right"><%#: Eval("RadiologiOrder")%></td>
                                                <td align="right"><%#: Eval("OtherOrder")%></td>
                                                <td align="right"><%#: Eval("Charges")%></td>
                                                <td align="right"><%#: Eval("Billing")%></td>
                                                <td align="right"><%#: Eval("OutstandingPostingObat")%></td>
                                                <td align="right"><%#: Eval("OutstandingPostingAlkes")%></td>
                                                <td align="right"><%#: Eval("OutstandingPostingBarangUmum")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
