<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="BedReservationInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.BedReservationInformation" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Controls/PatientGrid/GridBedReservationCtl.ascx" TagName="ctlgrdInpatientReservation"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtReservationDate.ClientID %>');
            $('#<%=txtReservationDate.ClientID %>').datepicker('option', 'maxDate');
            $('#<%=txtReservationDate.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });

            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });

        function onCboChanged() {
            onRefreshGridView();
        }
        
        function onCboClassCareChanged() {
            onRefreshGridView();
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        $('#<%=rblFilterSum.ClientID %>').live('change', function () {
            var filter = $('#<%=rblFilterSum.ClientID %>').find(":checked").val();
            if (filter == "filterClassSum") {
                $('#<%:trServiceUnitSum.ClientID %>').attr('style', 'display:none');
                $('#<%:trClassSum.ClientID %>').removeAttr('style');
                cbpViewSum.PerformCallback('refresh');
            } else {
                $('#<%:trServiceUnitSum.ClientID %>').removeAttr('style');
                $('#<%:trClassSum.ClientID %>').attr('style', 'display:none');
                cbpViewSum.PerformCallback('refresh');
            }
        });

        function onCboBedPicksWardSumValueChanged() {
            cbpViewSum.PerformCallback('refresh');
        }

        function onCboClassPicksSumValueChanged() {
            cbpViewSum.PerformCallback('refresh');
        }

        function onRefreshGridViewSum() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                cbpViewSum.PerformCallback('refresh');
            }
        }

        function onCbpViewSumEndCallback() {
            hideLoadingPanel();
        }
         
    </script>
    <style type="text/css">
        .ulBed
        {
            margin: 0;
            padding: 0;
        }
        .ulBed li
        {
            display: inline-block;
            border-radius: 5px;
            list-style-type: none;
            width: 275px;
            height: 135px;
            margin: 0 3px;
            padding: 5px;
        }
        
        .ulFooter li
        {
            display: inline-block;
            border-radius: 2px;
            list-style-type: none;
            min-width: 75px;
            height: 15px;
            margin: 0 10px;
            padding: 5px;
            font-size: 11px;
        }
        .genderStyle
        {
            font-size: 11px;
        }
        
        .fontCustom
        {
            font-size: 12px;
        }
        
        .trGenderM
        {
            background-color: blue;
        }
        .trGenderF
        {
            background-color: #FF69B4;
        }
        .liBedStatusU
        {
            background-color: #A1A4A6;
        }
        .liBedStatusW
        {
            background-color: #DEEC83;
        }
        .liBedStatusH
        {
            background-color: #B3A360;
        }
        .liBedStatusI
        {
            background-color: #F8C299;
        }
        .liBedStatusO
        {
            background-color: #4ac5e3;
        }
        .liBedStatusCo
        {
            background-color: #E7B4DE;
        }
        .liBedStatusB
        {
            background-color: #f1f262;
        }
        .liBedStatusOM
        {
            background-color: #4ac5e3;
        }
        .liBedStatusOF
        {
            background-color: #ffbdde;
        }
        
        .ulTabPage
        {
            margin: 0;
            padding: 0;
        }
        .ulTabPage li
        {
            list-style-type: none;
            width: auto;
            height: auto;
            margin: 0 10px;
            padding: 2px;
            overflow: hidden;
        }
        .TabContent
        {
            background-color: #F8C299;
        }
    </style>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div class="containerUlTabPage" style="margin-bottom: 3px;padding:3px">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="containerReservation" class="selected">
                <%=GetLabel("Informasi Reservasi")%></li>
            <li contentid="containerSummary">
                <%=GetLabel("Informasi Rekap Ruangan")%></li>
        </ul>
    </div>
    <div id="containerReservation" class="containerInfo" style="padding: 15px">
        <div class="pageTitle">
            <%=GetLabel("Informasi Reservasi")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 10%" />
                                <col style="width: 90%" />
                            </colgroup>
                            <tr>
                                <td width="200px">
                                    <%=GetLabel("Ruang Perawatan")%>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" Width="300px" ClientInstanceName="cboKlinik"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trReservationDate">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Reservasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReservationDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas Perawatan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboClassCare" Width="300px" ClientInstanceName="cboClassCare"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboClassCareChanged(); }" />
                                    </dxe:ASPxComboBox>
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
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Register" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Alamat" FieldName="PatientAddress" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                    </fieldset>
                    <uc1:ctlgrdInpatientReservation runat="server" ID="grdInpatientReservation" />
                </td>
            </tr>
        </table>
    </div>
    <div id="containerSummary" class="containerInfo" style="display: none; padding:15px">
            <div class="pageTitle">
                <%=GetLabel("Informasi Rekap Ruangan")%></div>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col width="120px" />
                </colgroup>
                <tr>
                    <td colspan="2">
                        <div id="divShow1" class="divShow">
                            <table class="tblContentArea" style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top" colspan="2">
                                        <fieldset id="Fieldset2">
                                            <table class="tblEntryContent">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <%=GetLabel("Filter")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblFilterSum" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Per Ruang" Value="filterRoomSum" Selected="True" />
                                                            <asp:ListItem Text="Per Kelas" Value="filterClassSum" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="trServiceUnitSum" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Ruang Perawatan")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboBedPicksWardSum" ClientInstanceName="cboBedPicksWardSum"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboBedPicksWardSumValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trClassSum" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Kelas")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClassPicksSum" ClientInstanceName="cboClassPicksSum" runat="server"
                                                            Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClassPicksSumValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewSum" runat="server" Width="100%" ClientInstanceName="cbpViewSum"
                                            OnCallback="cbpViewSum_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewSumEndCallback(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server">
                                                    <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                                        margin-right: auto; position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                                        <table id="grdView" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                                            <asp:ListView ID="lvwView" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblViewSum" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th style="width: 251px" rowspan="2" align="center">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 150px" rowspan="2" align="center">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th colspan="6" align="center">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                            <th style="width: 100px" rowspan="2" align="center">
                                                                                <%=GetLabel("TOTAL BED")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Dibooking")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Sedang Dibersihkan")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Tutup")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Menunggu Transfer")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Terisi")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Kosong")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="9">
                                                                                <%=GetLabel("No Data To Display") %>
                                                                            </td>
                                                                        </tr>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblViewSum" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th style="width: 251px" rowspan="2" align="center">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 150px" rowspan="2" align="center">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th colspan="6" align="center">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                            <th style="width: 100px" rowspan="2" align="center">
                                                                                <%=GetLabel("TOTAL BED")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Dibooking")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Sedang Dibersihkan")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Tutup")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Menunggu Transfer")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Terisi")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Kosong")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <%#: Eval("RoomName")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("ClassName")%>
                                                                        </td>
                                                                        <%--                                                                        <td align="center">
                                                                            <%#:Eval("BedBooking", "{0, 0:N2}")%>
                                                                        </td>--%>
                                                                        <td align="center">
                                                                            <label>
                                                                                <%#:Eval("BedBooking", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("BedCleaned", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("BedClosed", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <label>
                                                                                <%#:Eval("BedWaiting", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <label>
                                                                                <%#:Eval("BedOccupied", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("BedEmpty", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("TotalBed", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <input type="hidden" value='<%#:Eval("RoomID") %>' class="hdnRoomIDSumm" />
                                                                        <input type="hidden" value='<%#:Eval("ClassID") %>' class="hdnClassID" />
                                                                        <input type="hidden" value='<%#:Eval("BedOccupied") %>' class="hdnTotalOccupied" />
                                                                        <input type="hidden" value='<%#:Eval("BedWaiting") %>' class="hdnTotalWaiting" />
                                                                        <input type="hidden" value='<%#:Eval("BedBooking") %>' class="hdnTotalBooking" />
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
