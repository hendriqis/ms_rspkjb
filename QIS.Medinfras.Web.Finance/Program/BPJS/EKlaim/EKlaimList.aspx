<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="EKlaimList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.EKlaimList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Program/BPJS/EKlaim/GridEKlaimPatientCtl.ascx" TagName="ctlGrdEKlaimPatient"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtSearchTanggalSEPFrom.ClientID %>');
            $('#<%=txtSearchTanggalSEPFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            setDatePicker('<%=txtSearchTanggalSEPTo.ClientID %>');
            $('#<%=txtSearchTanggalSEPTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtSearchTanggalPulangFrom.ClientID %>');
            $('#<%=txtSearchTanggalPulangFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            setDatePicker('<%=txtSearchTanggalPulangTo.ClientID %>');
            $('#<%=txtSearchTanggalPulangTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        $('#<%=chkAbaikanTanggalPulang.ClientID %>').die();
        $('#<%=chkAbaikanTanggalPulang.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtSearchTanggalPulangFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtSearchTanggalPulangTo.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=txtSearchTanggalPulangFrom.ClientID %>').removeAttr('readonly');
                $('#<%=txtSearchTanggalPulangTo.ClientID %>').removeAttr('readonly');
            }
        });

        $('#<%=chkAbaikanTanggalSEP.ClientID %>').die();
        $('#<%=chkAbaikanTanggalSEP.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtSearchTanggalSEPFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtSearchTanggalSEPTo.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=txtSearchTanggalSEPFrom.ClientID %>').removeAttr('readonly');
                $('#<%=txtSearchTanggalSEPTo.ClientID %>').removeAttr('readonly');
            }
        });

        $("#btnRefresh").live('click', function (e) {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            onRefreshGridView();
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                refreshGrdEKlaimPatient();
            } else {
                $('#<%=hdnQuickText.ClientID %>').val('');
            }
        };

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                onRefreshGridView();
            }, 0);
        }

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" value="" id="hdnIsShowRegistrationWithLinkedTo" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetLabel("Pencarian Data")%></div>
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientListOrder">
                        <table>
                            <colgroup>
                                <col style="width: 180px" />
                                <col style="width: 150px" />
                                <col style="width: 50px" />
                                <col style="width: 150px" />
                                <col style="width: 200px" />
                                <col style="width: 50px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblNormal" style="width: 150px">
                                        <%=GetLabel("Jenis Rawat / No SEP")%></label>
                                </td>
                                <td align="left">
                                    <dxe:ASPxComboBox ID="cboJenisRawat" ClientInstanceName="cboJenisRawat" runat="server"
                                        Style="width: 150px">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboFilterNoSEP" ClientInstanceName="cboFilterNoSEP" runat="server"
                                        Style="width: 150px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label style="width: 150px">
                                        <%=GetLabel("Kelas SEP")%></label>
                                </td>
                                <td align="left">
                                    <dxe:ASPxComboBox ID="cboKelasSEP" ClientInstanceName="cboKelasSEP" runat="server"
                                        Style="width: 150px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trTanggalSEP" runat="server">
                                <td>
                                    <label class="lblNormal" style="width: 150px">
                                        <%=GetLabel("Tanggal SEP")%></label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearchTanggalSEPFrom" runat="server" CssClass="datepicker" Style="width: 120px" />
                                </td>
                                <td align="center">
                                    <label style="width: 30px">
                                        <%=GetLabel("s/d")%></label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearchTanggalSEPTo" runat="server" CssClass="datepicker" Style="width: 120px" />
                                </td>
                                <td align="left">
                                    <asp:CheckBox ID="chkAbaikanTanggalSEP" runat="server" /><%:GetLabel(" Abaikan Tanggal SEP")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label style="width: 150px">
                                        <%=GetLabel("Jenis Tanggal Pulang")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboJenisTanggalPulang" ClientInstanceName="cboJenisTanggalPulang" runat="server"
                                        Style="width: 150px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label style="width: 150px">
                                        <%=GetLabel("Tanggal Pulang")%></label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearchTanggalPulangFrom" runat="server" CssClass="datepicker"
                                        Style="width: 120px" />
                                </td>
                                <td align="center">
                                    <label style="width: 30px">
                                        <%=GetLabel("s/d")%></label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearchTanggalPulangTo" runat="server" CssClass="datepicker" Style="width: 120px" />
                                </td>
                                <td align="left">
                                    <asp:CheckBox ID="chkAbaikanTanggalPulang" runat="server" /><%:GetLabel(" Abaikan Tanggal Pulang")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="5">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView" Width="100%"
                                        Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No SEP" FieldName="NoSEP" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No RM E-Klaim" FieldName="EKlaimMedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Registasi" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label style="width: 150px">
                                        <%=GetLabel("Urut Berdasarkan")%></label>
                                </td>
                                <td align="left" colspan="3">
                                    <dxe:ASPxComboBox ID="cboSortByCustom" ClientInstanceName="cboSortByCustom" runat="server"
                                        Style="width: 100%">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" style="width: 120px" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large"
                                        value='<%= GetLabel("R e f r e s h")%>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <uc1:ctlGrdEKlaimPatient runat="server" ID="grdEKlaimPatient" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
        });
    </script>
</asp:Content>
