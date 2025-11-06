<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="TransRevenueSharingSummaryDownload1.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TransRevenueSharingSummaryDownload1" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnSendEmail" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><div>
            <%=GetLabel("Send Email")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        }

        $('#<%=txtPeriodFrom.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodFrom.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtPeriodTo.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodTo.ClientID %>').val(validateDateToFrom(start, end));
        });

        //#region SentEmail
        $('#<%=btnSendEmail.ClientID %>').live('click', function () {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Harap Pilih Data Terlebih Dahulu');
            }
            else {
                showToastConfirmation('Apakah yakin akan proses kirim email ?', function (result) {
                    if (result) {
                        onCustomButtonClick('email');
                    }
                });
            }
        });
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });
        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('.grdEmail .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html().trim();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        //#region Paramedic Master
        function onGetParamedicMasterFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND IsHasRevenueSharing = 1";
            return filterExpression;
        }

        $('#lblParamedic.lblLink').live('click', function () {
            openSearchDialog('paramedicRevenueSharing', onGetParamedicMasterFilterExpression(), function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                ontxtParamedicCodeChanged(value);
            });
        });

        $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
            ontxtParamedicCodeChanged($(this).val());
        });

        function ontxtParamedicCodeChanged(value) {
            var filterExpression = onGetParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
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
            });
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            var id = obj.hdnRSSummaryID;
            var isAllowPrint = true;
            var reportCode = $('#<%=hdnReportCode.ClientID %>').val();
            filterExpression = "RSSummaryID = '" + id + "'";
            openReportViewer(reportCode, filterExpression);
        });
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnRevenueSharingUploadedFile" runat="server" value="" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnConf" runat="server" />
    <input type="hidden" value="" id="hdnTemplateEmail" runat="server" />
    <input type="hidden" value="" id="hdnReportCode" runat="server" />
    <input type="hidden" value="" id="hdnListCCEmail" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 40%" />
                <col style="width: 60%" />
            </colgroup>
            <tr>
                <td>
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 35%" />
                                <col style="width: 15%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Periode Rekap Jasa Medis") %></label>
                                </td>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                            </td>
                                            <td style="width: 30px; text-align: center">
                                                s/d
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal lblLink" id="lblParamedic">
                                        <%=GetLabel("Dokter / Paramedis")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
                                    <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                    overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdEmail grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Tanggal Rekap")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Nomor Rekap")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Informasi Dokter")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Jumlah Penyesuaian")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Jumlah Jasa Medis")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Dibuat Oleh")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Terakhir Dikirim")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PRINT")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="11">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdEmail grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                    </th>
                                                    <th style="width: 20px; text-align: center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Tanggal Rekap")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Nomor Rekap")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Informasi Dokter")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Jumlah Penyesuaian")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Jumlah Jasa Medis")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Dibuat Oleh")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Terakhir Dikirim")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("PRINT")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("RSSummaryID")%>
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("cfRSSummaryDateInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <input type="hidden" class="hdnRSSummaryID" value='<%#: Eval("RSSummaryID") %>'
                                                        bindingfield="hdnRSSummaryID" />
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("RSSummaryNo")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        (<%#: Eval("ParamedicCode")%>)
                                                        <%#: Eval("ParamedicName")%>
                                                        <br>
                                                        <%#: Eval("BankName")%>
                                                        <br>
                                                        <%#: Eval("EmailAddress")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfTotalAdjustmentAmountInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfTotalRevenueSharingAmountInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("CreatedByName")%>
                                                        <br>
                                                        <%#: Eval("cfCreatedDateInString2")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                       <%#: Eval("LastSentByName")%>
                                                        <br>
                                                        <%#: Eval("cfLastSentDateInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="text-align: center">
                                                        <img class="imgPrint imgLink" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                            alt="" style="margin-right: 2px" /></div>
                                                </td>
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
