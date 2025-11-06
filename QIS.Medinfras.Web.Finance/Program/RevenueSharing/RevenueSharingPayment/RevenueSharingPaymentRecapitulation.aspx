<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingPaymentRecapitulation.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingPaymentRecapitulation" %>

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
    <li id="btnPrint" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print")%></div>
    </li>
    <li id="btnSendEmail" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><div>
            <%=GetLabel("Send Email")%></div>
    </li>
    <li id="btnDownload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download Excel")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodeFrom.ClientID %>');
            setDatePicker('<%=txtPeriodeTo.ClientID %>');
        }

        $('#btnRefresh').live('click', function () {
            cbpProcessDetail.PerformCallback('refresh');
        });

        $('#<%=btnPrint.ClientID %>').live('click', function () {
            var reportCode = $('#<%=hdnReportCode.ClientID %>').val();

            var txtPeriodeFrom = $('#<%=txtPeriodeFrom.ClientID %>').val();
            var periodeFrom = txtPeriodeFrom.split('-');
            var oPeriodeFrom = periodeFrom[2] + "" + periodeFrom[1] + "" + periodeFrom[0];

            var txtPeriodeTo = $('#<%=txtPeriodeTo.ClientID %>').val();
            var periodeTo = txtPeriodeTo.split('-');
            var oPeriodeTo = periodeTo[2] + "" + periodeTo[1] + "" + periodeTo[0];

            var oPaymentDate = oPeriodeFrom + ";" + oPeriodeTo;

            var oParamedicID = "0";
            if ($('#<%=hdnParamedicID.ClientID %>').val() != "" && $('#<%=hdnParamedicID.ClientID %>').val() != null) {
                oParamedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            }

            var oReportParameter = oPaymentDate + "," + oParamedicID;
            openReportViewer(reportCode, oReportParameter);
        });

        $('#<%=btnSendEmail.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah yakin akan proses kirim email ke dokter yang terpilih ?', function (result) {
                if (result) {
                    onCustomButtonClick('email');
                }
            });
        });

        //#region Download & Upload

        $('#<%=btnDownload.ClientID %>').live('click', function () {
            onCustomButtonClick('download');
        });

        function downloadBPJSDocument(stringparam) {
            var fileName = $('#<%=hdnFileName.ClientID %>').val();

            var link = document.createElement("a");
            link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }

        //#endregion

        //#region Paramedic Master
        function onGetParamedicMasterFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblParamedic.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetParamedicMasterFilterExpression(), function (value) {
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

        function onCbpProcessDetailEndCallback() {
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                downloadBPJSDocument(retval);
            }

            cbpProcessDetail.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnReportCode" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 60%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td>
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 150px" />
                                <col style="width: 30px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblMandatory">
                                        <%=GetLabel("Periode Pembayaran JasMed") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPeriodeFrom" CssClass="datepicker" Style="width: 130px" />
                                </td>
                                <td align="center">
                                    <label>
                                        <%=GetLabel("s/d")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPeriodeTo" CssClass="datepicker" Style="width: 130px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblMandatory lblLink" id="lblParamedic">
                                        <%=GetLabel("Dokter / Paramedis")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
                                    <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                                </td>
                                <td colspan="3">
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
                <td colspan="2">
                    <h4>
                        <%=GetLabel("Data Pembayaran Jasa Medis")%></h4>
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Pembayaran")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Rekap")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Bukti")%>
                                                        </th>
                                                        <th align="center" style="width: 120px">
                                                            <%=GetLabel("Department")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Nama / Kode Jasa Medis")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Transaksi")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Pembayaran")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Pembayaran")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Rekap")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Bukti")%>
                                                        </th>
                                                        <th align="center" style="width: 120px">
                                                            <%=GetLabel("Department")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Nama / Kode Jasa Medis")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Transaksi")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Pembayaran")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RSPaymentNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfRSPaymentDateInString") %></label>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RSSummaryNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfRSSummaryDateInString") %></label>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RevenueSharingNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfProcessedDateInString") %></label>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%#: Eval("DepartmentID") %></label>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RevenueSharingName") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("RevenueSharingCode") %></label>
                                                    </td>
                                                    <td align="right">
                                                        <label class="lblNormal" style="color: Purple">
                                                            <%#: Eval("cfRevenueSharingAmountInString") %></label>
                                                    </td>
                                                    <td align="right">
                                                        <label class="lblNormal" style="color: Red">
                                                            <%#: Eval("cfPaymentAmountInString") %></label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
