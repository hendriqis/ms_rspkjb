<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="MrnMergeHistory.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MrnMergeHistory" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=GetMenuCaption()%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                //                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
                //                intervalID = window.setInterval(function () {
                //                    onRefreshGridView();
                //                }, interval);
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        $('#<%=chkIsIgnoreDate.ClientID %>').die();
        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDateFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtDateTo.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=txtDateFrom.ClientID %>').removeAttr('readonly');
                $('#<%=txtDateTo.ClientID %>').removeAttr('readonly');
            }
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
                    $('#<%=lvwView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div style="padding: =5px">
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
                            <td>
                                <label>
                                    <%=GetLabel("Tanggal Proses") %></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td >
                                            <asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false"
                                                Text=" Abaikan Tanggal" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="No RM Asal" FieldName="FromMedicalNo" />
                                        <qis:QISIntellisenseHint Text="Nama Pasien Asal" FieldName="FromFullName" />
                                        <qis:QISIntellisenseHint Text="No RM Tujuan" FieldName="ToMedicalNo" />
                                        <qis:QISIntellisenseHint Text="Nama Pasien Tujuan" FieldName="ToFullName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top" colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th colspan="4" style="font-weight: bold">
                                                        <%=GetLabel("ASAL")%>
                                                    </th>
                                                    <th colspan="4" style="font-weight: bold">
                                                        <%=GetLabel("TUJUAN")%>
                                                    </th>
                                                    <th colspan="2" style="width: 70px; font-weight: bold">
                                                        <%=GetLabel("DIBUAT")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th rowspan="2" style="width: 50px; font-weight: bold">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 250px; font-weight: bold">
                                                        <%=GetLabel("Nama Lengkap")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 30px; font-weight: bold">
                                                        <%=GetLabel("Tanggal Lahir")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; font-weight: bold">
                                                        <%=GetLabel("Jenis Kelamin")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px; font-weight: bold">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 250px; font-weight: bold">
                                                        <%=GetLabel("Nama Lengkap")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 35px; font-weight: bold">
                                                        <%=GetLabel("Tanggal Lahir")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; font-weight: bold">
                                                        <%=GetLabel("Jenis Kelamin")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 100px; font-weight: bold">
                                                        <%=GetLabel("Oleh")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; font-weight: bold">
                                                        <%=GetLabel("Tanggal")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("Tidak Ada Informasi Penggabungan No Rekam Medis")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th colspan="4" style="font-weight: bold">
                                                        <%=GetLabel("ASAL")%>
                                                    </th>
                                                    <th colspan="4" style="font-weight: bold">
                                                        <%=GetLabel("TUJUAN")%>
                                                    </th>
                                                    <th colspan="2" style="width: 70px; font-weight: bold">
                                                        <%=GetLabel("DIBUAT")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th rowspan="2" style="width: 50px; font-weight: bold">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 250px; font-weight: bold">
                                                        <%=GetLabel("Nama Lengkap")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 35px; font-weight: bold">
                                                        <%=GetLabel("Tanggal Lahir")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; font-weight: bold">
                                                        <%=GetLabel("Jenis Kelamin")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px; font-weight: bold">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 250px; font-weight: bold">
                                                        <%=GetLabel("Nama Lengkap")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 35px; font-weight: bold">
                                                        <%=GetLabel("Tanggal Lahir")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; font-weight: bold">
                                                        <%=GetLabel("Jenis Kelamin")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 100px; font-weight: bold">
                                                        <%=GetLabel("Oleh")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; font-weight: bold">
                                                        <%=GetLabel("Tanggal")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("FromMedicalNo") %>
                                                </td>
                                                <td align="left" style="background-color: white">
                                                    <%#:Eval("FromFullName")%>
                                                </td>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("cfFromDateOfBirthInString")%>
                                                </td>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("FromGender")%>
                                                </td>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("ToMedicalNo")%>
                                                </td>
                                                <td align="left" style="background-color: white">
                                                    <%#:Eval("ToFullName")%>
                                                </td>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("cfToDateOfBirthInString")%>
                                                </td>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("ToGender")%>
                                                </td>
                                                <td align="left" style="background-color: white">
                                                    <%#:Eval("CreatedByName")%>
                                                </td>
                                                <td align="center" style="background-color: white">
                                                    <%#:Eval("cfCreatedDateInString")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
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
    </div>
</asp:Content>
