<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="BedChargesProcess.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.BedChargesProcess" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessClosedJobBed" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Toolbar/job_bed_open.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Close Job Bed")%></div>
    </li>
    <li id="btnProcessReopenJobBed" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Toolbar/job_bed_closed.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Reopen Job Bed")%></div>
    </li>
    <li id="btnHistoryJobBed" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("History Job Bed")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setCustomButtonEnabled();
        });

        function setCustomButtonEnabled() {
            if ($('#<%=hdnIsJobBedClosed.ClientID %>').val() == "0") {
                $('#<%=btnProcessClosedJobBed.ClientID %>').show();
            } else {
                $('#<%=btnProcessClosedJobBed.ClientID %>').hide();
            }

            if ($('#<%=hdnIsJobBedClosed.ClientID %>').val() == "0" && $('#<%=hdnIsJobBedReopen.ClientID %>').val() == "0") {
                $('#<%=btnProcessReopenJobBed.ClientID %>').hide();
            } else {
                if ($('#<%=hdnIsJobBedReopen.ClientID %>').val() == "0") {
                    $('#<%=btnProcessReopenJobBed.ClientID %>').show();
                } else {
                    $('#<%=btnProcessReopenJobBed.ClientID %>').hide();
                }
            }
        }

        $('#<%=btnProcessClosedJobBed.ClientID %>').live('click', function () {
            onCustomButtonClick('closejobbed');
        });

        $('#<%=btnProcessReopenJobBed.ClientID %>').live('click', function () {
            var url = ResolveUrl('~/Program/BedChargesProcess/BedChargesProcessReopenCtl.ascx');
            var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
            openUserControlPopup(url, regID, 'Reopen Job Bed', 600, 200);
        });

        $('#<%=btnHistoryJobBed.ClientID %>').live('click', function () {
            var url = ResolveUrl("~/Libs/Program/Information/InfoHistoryJobBedRegistrationCtl.ascx");
            var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
            openUserControlPopup(url, regID, 'History Job Bed', 900, 500);
        });

        $('.btnSave').live('click', function () {
            $tr = $(this).closest('tr');
            var ID = $tr.find('.ID').val();
            cbpView.PerformCallback("save|" + ID);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'success') {
                    cbpView.PerformCallback('refresh');
                } else {
                    displayErrorMessageBox('Proses gagal', 'Error Message : ' + param[2]);
                    cbpView.PerformCallback('refresh');
                }
            }
        }

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRightPanelContent(code, value, isAdd) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (registrationID != '' || registrationID != '0') {
                if (code == 'IP-00207') {
                    filterExpression.text = registrationID;
                    return true;
                } else {
                    errMessage.text = "ERROR";
                    return false;
                }
            } else {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnIsJobBedClosed" runat="server" />
    <input type="hidden" value="" id="hdnIsJobBedReopen" runat="server" />
    <input type="hidden" value="" id="hdnBedChargesTimeRounding" runat="server" />
    <input type="hidden" value="" id="hdnBedChargesTypeDate" runat="server" />
    <input type="hidden" value="" id="hdnBedChargesInDay" runat="server" />
    <input type="hidden" value="" id="hdnBedChargesHealthcareServiceUnit" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="width: 50%">
                </td>
                <td style="width: 50%">
                    <table id="tblInfoWarning" runat="server">
                        <tr>
                            <td style="vertical-align: top" class="blink-alert">
                                <img height="60" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' class="blink-alert" />
                            </td>
                            <td style="vertical-align: middle">
                                <label class="lblWarning" id="lblBedChargesTimeRounding" runat="server"></label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 300px">
                                                            <%= GetLabel("Dari")%>
                                                        </th>
                                                        <th style="width: 300px">
                                                            <%= GetLabel("Ke")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("Jumlah Jam")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Jumlah Nilai")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("No Transaksi")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("Proses")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="10">
                                                            <%=GetLabel("No data to display.") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 300px">
                                                            <%= GetLabel("Dari")%>
                                                        </th>
                                                        <th style="width: 300px">
                                                            <%= GetLabel("Ke")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("Jumlah Jam")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Jumlah Nilai")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("No Transaksi")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("Proses")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server">
                                                    <td align="left">
                                                        <input type="hidden" class="ID" value='<%#:Eval("ID") %>' />
                                                        <div>
                                                            <b>
                                                                <%#: Eval("FromServiceUnitName")%></b>
                                                            <br />
                                                            <label style="font-style: italic">
                                                                <%:GetLabel("Ruang - Tempat Tidur : ")%></label>
                                                            <%#: Eval("FromRoomName")%>
                                                            ||
                                                            <%#: Eval("FromBedCode")%>
                                                            <br />
                                                            <label style="font-style: italic">
                                                                <%:GetLabel("Kelas - Kelas Tagihan : ")%></label><%#: Eval("FromClassName")%>
                                                            ||
                                                            <%#: Eval("FromChargeClassName")%>
                                                            <br />
                                                            <label>
                                                                <b>
                                                                    <%#: Eval("cfFromDateTimeInString")%></b>
                                                            </label>
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ToServiceUnitName")%></b>
                                                            <br />
                                                            <label style="font-style: italic">
                                                                <%:GetLabel("Ruang - Tempat Tidur : ")%></label>
                                                            <%#: Eval("ToRoomName")%>
                                                            ||
                                                            <%#: Eval("ToBedCode")%>
                                                            <br />
                                                            <label style="font-style: italic">
                                                                <%:GetLabel("Kelas - Kelas Tagihan : ")%></label><%#: Eval("ToClassName")%>
                                                            ||
                                                            <%#: Eval("ToChargeClassName")%>
                                                            <br />
                                                            <label>
                                                                <b>
                                                                    <%#: Eval("cfToDateTimeInString")%></b>
                                                            </label>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("NumberOfHour")%>
                                                        </div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("cfChargesAmountInString")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("TransactionNo")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <input type="button" id="btnSave" class="btnSave" value="Simpan" runat="server" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
