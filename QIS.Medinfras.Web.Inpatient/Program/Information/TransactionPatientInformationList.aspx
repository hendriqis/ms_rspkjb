<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="TransactionPatientInformationList.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionPatientInformationList" %>

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

        $('.lblRegistrationNo').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            var url = ResolveUrl("~/Program/Information/TransactionPatientInformationDetailCtl.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });

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
            }, 0);
        }
        //#endregion

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

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetLabel("Informasi Pasien Dirawat")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width:140px"/>
                            </colgroup>
                            <tr>
                                <td><%=GetLabel("Ruang Perawatan")%></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" ClientInstanceName="cboKlinik"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Register" FieldName="MRN" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
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
                            EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th rowspan="2"><%=GetLabel("Informasi Registrasi") %></th>
                                                    <th colspan="3"><%=GetLabel("Order Penunjang Medis") %></th>
                                                    <th colspan="2"><%=GetLabel("Transaksi") %></th>
                                                </tr>
                                                <tr>
                                                    <th><%=GetLabel("Lab") %></th>
                                                    <th><%=GetLabel("Radiologi") %></th>
                                                    <th><%=GetLabel("Other") %></th>
                                                    <th><%=GetLabel("Charges") %></th>
                                                    <th><%=GetLabel("Billing") %></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th rowspan="2"><%=GetLabel("Informasi Registrasi") %></th>
                                                    <th colspan="3"><%=GetLabel("Order Penunjang Medis") %></th>
                                                    <th colspan="2"><%=GetLabel("Transaksi") %></th>
                                                </tr>
                                                <tr>
                                                    <th><%=GetLabel("Lab") %></th>
                                                    <th><%=GetLabel("Radiologi") %></th>
                                                    <th><%=GetLabel("Other") %></th>
                                                    <th><%=GetLabel("Charges") %></th>
                                                    <th><%=GetLabel("Billing") %></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="tdRoleID">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                                <a class="lblLink lblRegistrationNo"><%#: Eval("RegistrationNo")%></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><%#: Eval("PatientName")%></td>
                                                            <td><%#: Eval("MedicalNo")%></td>
                                                        </tr>
                                                        <tr>
                                                            <td><%#: Eval("ServiceUnitName")%></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="tdRoleID" align="right"><%#: Eval("Lab")%></td>
                                                <td class="tdRoleID" align="right"><%#: Eval("Radiologi")%></td>
                                                <td class="tdRoleID" align="right"><%#: Eval("Other")%></td>
                                                <td class="tdRoleID" align="right"><%#: Eval("Charges")%></td>
                                                <td class="tdRoleID" align="right"><%#: Eval("Billing")%></td>
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
