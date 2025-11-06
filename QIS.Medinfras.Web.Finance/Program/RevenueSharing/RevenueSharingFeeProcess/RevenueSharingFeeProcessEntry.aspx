<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingFeeProcessEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingFeeProcessEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPreview" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Preview")%>
        </div>
    </li>
    <li id="btnApprove" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" />
        <div>
            <%=GetLabel("Approve")%>
        </div>
    </li>
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" />
        <div>
            <%=GetLabel("Void")%>
        </div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=lvwView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlGridView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            //#region Paramedic
            function onGetParamedicFilterExpression() {
                return "<%=OnGetParamedicFilterExpression() %>";
            }

            $('#lblParamedic.lblLink').click(function () {
                openSearchDialog('paramedic', 'IsDeleted = 0 AND IsHasRevenueSharing = 1', function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    onTxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
                onTxtParamedicCodeChanged($(this).val());
            });

            function onTxtParamedicCodeChanged(value) {
                var filterExpression = onGetParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.FullName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID %>').val('');
                        $('#<%=txtParamedicCode.ClientID %>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
                });
            }
        });
        //#endregion

        $(function () {
            var pageCount = parseInt('<%=PageCount %>');
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'approve') {
                if (param[1] == 'fail') {
                    showToast('Proses Perhitungan Pajak Jasa Medis Gagal', 'Error Message : ' + param[2]);
                }
                else {
                    cbpView.PerformCallback('refresh');
                }
            }
            cbpView.PerformCallback('refresh');
        }

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

        function onRefreshGridView() {
            var status = cboStatus.GetValue();
            if (status == 'OPEN') {
                $('#<%=btnApprove.ClientID %>').show();
                $('#<%=btnVoidByReason.ClientID %>').hide();
            } else if (status == 'APPROVE') {
                $('#<%=btnApprove.ClientID %>').hide();
                $('#<%=btnVoidByReason.ClientID %>').show();
            } else {
                $('#<%=btnApprove.ClientID %>').show();
                $('#<%=btnVoidByReason.ClientID %>').show();
            }

            cbpView.PerformCallback('refresh');
        }

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            $('.chkSelectAll input').prop('checked', false);
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getFeeRevenueSharing() {
            var lstSelectedFeeRevenueSharing = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var result = '';
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var idx = lstSelectedFeeRevenueSharing.indexOf(key);
                    if (idx < 0) {
                        lstSelectedFeeRevenueSharing.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstSelectedFeeRevenueSharing.indexOf(key);
                    if (idx > -1) {
                        lstSelectedFeeRevenueSharing.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedFeeRevenueSharing.join(',').trim());
        }

        $('#<%=btnPreview.ClientID %>').click(function () {
            onRefreshGridView();
        });

        $('#<%=btnApprove.ClientID %>').live('click', function () {
            getFeeRevenueSharing();
            $('#<%=hdnSelectedMemberValue.ClientID %>').val($('#<%=hdnSelectedMember.ClientID %>').val().substring(1));
            cbpProcess.PerformCallback('approve');
            onRefreshGridView();
        });

        $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
            getFeeRevenueSharing();
            $('#<%=hdnSelectedMemberValue.ClientID %>').val($('#<%=hdnSelectedMember.ClientID %>').val().substring(1));
            cbpProcess.PerformCallback('void');
            onRefreshGridView();
        });

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberValue" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <fieldset id="fsList" style="margin: 0">
        <table class="tblEntryContent">
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Proses") %></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 145px" />
                            <col style="width: 3px" />
                            <col style="width: 145px" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                            </td>
                            <td>
                                <%=GetLabel("s/d") %>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" id="lblParamedic">
                        <%=GetLabel("Dokter / Paramedis") %></label>
                </td>
                <td>
                    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="50%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="lblNormal lblLink" id="Label1">
                        <%=GetLabel("Status") %></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <dxe:ASPxComboBox ID="cboStatus" ClientInstanceName="cboStatus" Width="200px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tblContentArea">
            <tr>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center" id="thSelectAll">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Kode Dokter")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Nama Dokter")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Akumulasi Jasa Bruto")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Dasar PPH")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21 (s/d Bulan Lalu)")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21 (Bulan Ini)")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21 Extra")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Status")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="16">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdRevenueSharingFee grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center" id="thSelectAll">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Kode Dokter")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Nama Dokter")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Akumulasi Jasa Bruto")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Dasar PPH")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21 (s/d Bulan Lalu)")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21 (Bulan Ini)")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PPH Pasal 21 Extra")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Status")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ParamedicID")%>' />
                                                </td>
                                                <td>
                                                    <%#: Eval("ParamedicCode")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("ParamedicName")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("AccumulationRevenueSharing", "{0:N}")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("BasePPh", "{0:N}")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("PPhPasal21", "{0:N}")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("PPhPasal21PreviousMonth", "{0:N}")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("PPhPasal21CurrentMonth", "{0:N}")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("PPhPasal21Extra", "{0:N}")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("IsPosting")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
