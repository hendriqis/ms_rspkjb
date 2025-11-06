<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="LaboratoryTestResultHistoryList.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryTestResultHistoryList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientResultCtl.ascx" TagName="ctlGrdPatientResult" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region Inisialisasi
        $(function () {
            //#region Medical No
            $('#lblMRN.lblLink').click(function () {
                openSearchDialog('patient', "", function (value) {
                    $('#<%=txtMRN.ClientID %>').val(value);
                    ontxtMRNChanged(value);
                });
            });

            $('#<%=txtMRN.ClientID %>').change(function () {
                ontxtMRNChanged($(this).val());
            });

            function ontxtMRNChanged(value) {
                var filterExpression = "MedicalNo = '" + value + "'";
                Methods.getObject('GetvPatientList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMRN.ClientID %>').val(result.MRN);
                        $('#<%=txtMRN.ClientID %>').val(result.MedicalNo);
                        $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                    }
                    else {
                        $('#<%=hdnMRN.ClientID %>').val('');
                        $('#<%=txtMRN.ClientID %>').val('');
                        $('#<%=txtPatientName.ClientID %>').val('');
                    }
                    onRefreshGridView();
                });
            }

            //#endregion

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });

            $('#<%=txtPatientName.ClientID %>').change(function () {
                onRefreshGridView();
            });

            setDatePicker('<%=txtTransactionDateFrom.ClientID %>');
            setDatePicker('<%=txtTransactionDateTo.ClientID %>');

            $('#<%=txtTransactionDateFrom.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#<%=txtTransactionDateTo.ClientID %>').change(function () {
                onRefreshGridView();
            });
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                refreshGrdResultPatient();
            }
        }
        //#endregion

        function onCboOrderResultTypeValueChanged() {
            onRefreshGridView();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });
        //#endregion

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding:15px">
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server"/>
        <div class="pageTitle"><%=GetMenuCaption()%> : <%=GetLabel("Pilih Pasien")%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsPatientList"> 
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblMRN"><%=GetLabel("No Rekam Medis")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnMRN" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtMRN" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtMRNPatientName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                                <td><asp:TextBox ID="txtPatientName" Width="200px" runat="server"/></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan Hasil")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType" Width="150px" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Pemeriksaan")%></label></td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:140px"/>
                                            <col style="width:5%"/>
                                            <col style="width:140px"/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtTransactionDateFrom" Width="100px" runat="server" CssClass="datepicker"/></td>
                                            <td>- </td>
                                            <td><asp:TextBox ID="txtTransactionDateTo" Width="100px" runat="server" CssClass="datepicker"/></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                    </div>
                    <uc1:ctlGrdPatientResult runat="server" id="grdPatientResult" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
