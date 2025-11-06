<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="ProcessBedReservationList.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.ProcessBedReservationList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Program/BedReservation/PatientGrid/GridPatientTitipanCtl.ascx"
    TagName="ctlGrdPatientTitipan" TagPrefix="uc1" %>
<%@ Register Src="~/Program/BedReservation/PatientGrid/GridPatientReservationCtl.ascx"
    TagName="ctlGrdPatientReservation" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerOrder').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });

        //#region Titipan
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridViewTitipan();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridViewTitipan();
        }, interval);

        function onRefreshGridViewTitipan() {
            $('#<%=hdncboServiceUnitTitipan.ClientID %>').val(cboServiceUnitTitipan.GetValue());
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearchTitipan.ClientID %>').val(txtSearchViewTitipan.GenerateFilterExpression());
                refreshGrdPasienTitipan();
                intervalID = window.setInterval(function () {
                    onRefreshGridViewTitipan();
                }, interval);
            }
        }

        function onTxtSearchViewTitipanSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridViewTitipan();
            }, 0);
        }

        function onCboServiceUnitTitipanChanged(s) {
            onRefreshGridViewTitipan();
        }
        //endRegion

        //#region Reservasi
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridViewReservasi();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridViewReservasi();
        }, interval);

        function onRefreshGridViewReservasi() {
            $('#<%=hdncboClassRequest.ClientID %>').val(cboClassRequest.GetValue());
            $('#<%=hdnSortBy.ClientID %>').val(cboSortBy.GetValue());
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearchReservasi.ClientID %>').val(txtSearchViewReservasi.GenerateFilterExpression());
                refreshGrdPasienReservasi();
                intervalID = window.setInterval(function () {
                    onRefreshGridViewReservasi();
                }, interval);
            }
        }

        function onTxtSearchViewReservasiSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridViewReservasi();
            }, 0);
        }

        function oncboClassRequestChanged(s) {
            onRefreshGridViewReservasi();
        }

        function onCboSortByValueChanged(s) {
            onRefreshGridViewReservasi();
        }
        //endRegion

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchTitipan" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReservasi" runat="server" />
    <input type="hidden" value="" id="hdncboServiceUnitTitipan" runat="server" />
    <input type="hidden" value="" id="hdncboClassRequest" runat="server" />
    <input type="hidden" value="" id="hdnSortBy" runat="server" />
    <div style="padding: 15px;">
        <div class="containerUlTabPage">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerTitipan">
                    <%=GetLabel("Titipan")%></li>
                <li contentid="containerReservasi">
                    <%=GetLabel("Reservasi")%></li>
            </ul>
        </div>
        <div style="padding: 2px;" id="containerTitipan" class="containerOrder">
            <div class="pageTitle">
                <%=GetLabel("Pasien Titipan")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="Fieldset1">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Ruang Perawatan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnitTitipan" ClientInstanceName="cboServiceUnitTitipan"
                                            Width="350px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitTitipanChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewTitipan"
                                            ID="txtSearchViewTitipan" Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewTitipanSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                        <uc1:ctlGrdPatientTitipan runat="server" ID="CtlGrdPatientTitipan" />
                    </td>
                </tr>
            </table>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
        <div style="padding: 2px; display: none;" id="containerReservasi" class="containerOrder">
            <div class="pageTitle">
                <%=GetLabel("Pasien Reservasi")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="Fieldset2">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kelas Permintaan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboClassRequest" ClientInstanceName="cboClassRequest"
                                            Width="350px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { oncboClassRequestChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Urut Berdasarkan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboSortBy" ClientInstanceName="cboSortBy"
                                            Width="150px" runat="server" BackColor="Pink">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboSortByValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReservasi"
                                            ID="txtSearchViewReservasi" Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewReservasiSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No Reservasi" FieldName="ReservationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="Span1">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                        <uc1:ctlGrdPatientReservation runat="server" ID="CtlGrdPatientReservasi" />
                    </td>
                </tr>
            </table>
            <div class="imgLoadingGrdView" id="Div2">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
    </div>
</asp:Content>
