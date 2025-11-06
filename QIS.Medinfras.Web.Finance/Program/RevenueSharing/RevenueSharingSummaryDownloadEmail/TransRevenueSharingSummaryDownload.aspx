<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="TransRevenueSharingSummaryDownload.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TransRevenueSharingSummaryDownload" %>

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
    <li id="btnDownload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download")%></div>
    </li>
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

        //#region Download & SentEmail

        $('#<%=btnDownload.ClientID %>').live('click', function () {
            onCustomButtonClick('download');
        });

        $('#<%=btnSendEmail.ClientID %>').live('click', function () {
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (paramedicID != '') {
                showToastConfirmation('Apakah yakin akan proses kirim email ke dokter yang terpilih ?', function (result) {
                    if (result) {
                        onCustomButtonClick('email');
                    }
                });
            }
            else {
                displayErrorMessageBox('Warning', 'Pilih dokter terlebih dahulu.');
            }
        });

        function downloadRevenueSharingDocument(stringparam) {
            var baseName = "Slip_Honor_Dokter_Periode_";
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();
            var paramedicCode = $('#<%=txtParamedicCode.ClientID %>').val();

            var fileName = baseName + start + "_Sampai_" + end;
            if (paramedicCode != '') {
                fileName += '_' + paramedicCode;
            }
            fileName += '.pdf';

            var link = document.createElement("a");
            link.href = 'data:application/pdf;base64,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }

        function readURL(input) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=hdnRevenueSharingUploadedFile.ClientID %>').val(e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                if (retval != "Download Failed") {
                    downloadRevenueSharingDocument(retval);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            }
            else {
                if (retval == '') {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');                
                }
            }
        }

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

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnRevenueSharingUploadedFile" runat="server" value="" />
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="RSSummaryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Tanggal Rekap")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("cfRSSummaryDateInString")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nomor Rekap")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("RSSummaryNo")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Dokter")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: left">
                                                        (<%#: Eval("ParamedicCode")%>) <%#: Eval("ParamedicName")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nama Bank")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("BankName")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="right"
                                                ItemStyle-HorizontalAlign="right">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Jumlah Penyesuaian")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfTotalAdjustmentAmountInString")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="right"
                                                ItemStyle-HorizontalAlign="right">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Jumlah Jasa Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfTotalRevenueSharingAmountInString")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Dibuat Oleh")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("CreatedByName")%>
                                                        <BR>
                                                       <%#: Eval("cfCreatedDateInString2")%>
                                                    </div>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
