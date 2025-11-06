<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="MCUGenerateOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUGenerateOrderList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientVisitGenerateOrderMCUCtl.ascx"
    TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });
        });

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

        function onCboResultTypeValueChanged() {
            onRefreshGridView();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        function onBeforeLoadRightPanelContent(code) {
            return "";
        }

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRegistrationDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRegistrationDate.ClientID %>').removeAttr('readonly');
            onRefreshGridView();
        });

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%>
            :
            <%=GetLabel("Pilih Pasien")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Registrasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                </td>
                            </tr>
                            <tr id="trIsShowRegistrationStatus" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Registrasi")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboResultType" ClientInstanceName="cboResultType" Width="150px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(e); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="2">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Penjamin Bayar" FieldName="BusinessPartnerName" />
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
                    <uc1:ctlGrdRegisteredPatient runat="server" ID="grdRegisteredPatient" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
