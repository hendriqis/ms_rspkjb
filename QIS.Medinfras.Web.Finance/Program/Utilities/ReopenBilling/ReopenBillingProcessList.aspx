<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ReopenBillingProcessList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ReopenBillingProcessList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnReopenBilling" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Re-open Billing")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        }

        $('#<%=btnReopenBilling.ClientID %>').live('click', function () {
            var errMessage = { text: '' };
            getCheckedMember(errMessage);
            if (errMessage.text == '') {
                if ($('#<%=hdnSelectedRegistrationID.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Registration First');
                }
                else {
                    onCustomButtonClick('reopenbilling');
                }
            }
        });

        function onCboDepartmentChanged() {
            $('#<%=hdnFilterHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
        }

        //#region Healthcare Service Unit
        function getHealthcareServiceUnitFilterExpression() {
            var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboDepartment.GetValue() + "'"; ;
            return filterExpression;
        }

        $('#lblServiceUnit.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFilterHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnFilterHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#btnRefresh').live('click', function (evt) {
            onRefreshGridView();
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
            }
        }

        function onAfterCustomClickSuccess(type) {
            showToast('INFORMATION', 'Process re-open billing success !', function () {
                onRefreshGridView();
            });
            setCustomToolbarVisibility();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember(errMessage) {
            var lstRegistrationID = $('#<%=hdnSelectedRegistrationID.ClientID %>').val().split(',');
            $('.grdView .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.hdnKeyField').val().trim();
                    var idx = lstRegistrationID.indexOf(key);
                    if (idx < 0) {
                        lstRegistrationID.push(key);
                    }
                }
                else {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.hdnKeyField').val().trim();
                    var idx = lstRegistrationID.indexOf(key);
                    if (idx > -1) {
                        lstRegistrationID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedRegistrationID.ClientID %>').val(lstRegistrationID.join(','));
        }

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnFilterHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRegistrationID" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr id="trRegistrationDate" runat="server">
                            <td>
                                <%=GetLabel("Tanggal Pendaftaran") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr id="trDepartment" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Department")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                    Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblServiceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td colspan="2">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td colspan="2">
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="400px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 10px" align="center">
                                                </th>
                                                <th style="width: 300px">
                                                    <%=GetLabel("No. Registrasi") %>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Registrasi") %>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Pasien") %>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 10px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 300px">
                                                    <%=GetLabel("No. Registrasi") %>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Registrasi") %>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Pasien") %>
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
                                            </td>
                                            <td class="tdRoleID">
                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                <b>
                                                    <%#: Eval("RegistrationNo")%></b>
                                                <br />
                                                <i>
                                                    <%#: Eval("ParamedicName")%></i>
                                                <br />
                                                <%=GetLabel("Tgl Registrasi : ") %><%#: Eval("cfRegistrationDateInString")%>
                                                <br />
                                                <%=GetLabel("Tgl Tutup Billing : ") %><%#: Eval("cfBillingClosedDateInString")%>
                                                <br />
                                                <%=GetLabel("Tgl Buka Billing : ") %><%#: Eval("cfBillingReopenDateInString")%>
                                            </td>
                                            <td>
                                                <div>
                                                    <b>
                                                        <%#: Eval("ServiceUnitName")%></b>
                                                    <br />
                                                    <%=GetLabel("Ruang & Bed : ") %><%#: Eval("RoomName")%>
                                                    <%#: Eval("BedCode")%>
                                                </div>
                                            </td>
                                            <td>
                                                <b>
                                                    <%#: Eval("PatientName")%></b>
                                                <br />
                                                <i>
                                                    <%#: Eval("MedicalNo")%></i>
                                                <br />
                                                <%=GetLabel("DOB : ") %><%#: Eval("cfDateOfBirthInString")%><%=GetLabel(" (") %><i><%#: Eval("Gender")%></i><%=GetLabel(")") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</asp:Content>
