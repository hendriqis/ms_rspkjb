<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="ServiceOrderAIOList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ServiceOrderAIOList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl.ascx" TagName="ctlGrdRegOrderPatient"
    TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridServiceOrderCtl.ascx" TagName="ctlGrdOrderPatient"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDate.ClientID %>');
            setDatePicker('<%=txtTestOrderDate.ClientID %>');
            $('#<%=txtDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtTestOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtDate.ClientID %>').change(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    onRefreshGrdReg();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    onRefreshGrdReg();
            });

            $('#lblRefreshOrder.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsPatientListOrder', 'mpPatientListOrder'))
                    onRefreshGrdOrder();
            });

            $('#<%=txtTestOrderDate.ClientID %>').change(function (evt) {
                if (IsValid(evt, 'fsPatientListOrder', 'mpPatientListOrder'))
                    onRefreshGrdOrder();
            });

            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerOrder').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });

        $('#<%=chkIsPreviousEpisodePatientOrder.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatientOrder.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtTestOrderDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtTestOrderDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdOrder();
        });

        $('#<%=chkIsPreviousEpisodePatientReg.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatientReg.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdReg();
        });

        //#region Service Unit Registration
        $('#lblServiceUnit.lblLink').live('click', function () {
            var DepartmentID = cboDepartment.GetValue();
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "IsUsingRegistration = 1 AND DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0";
            openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
                $('#<%=txtFromServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtFromServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFromServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtFromServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnFromServiceUnitID.ClientID %>').val('');
                    $('#<%=txtFromServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtFromServiceUnitName.ClientID %>').val('');
                }
                onRefreshGrdReg();
            });

            //#endregion
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        //#region Service Unit Order
        $('#lblServiceUnitOrder.lblLink').live('click', function () {
            var DepartmentID = cboDepartmentOrder.GetValue();
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "IsUsingRegistration = 1 AND DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0";
            openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
                $('#<%=txtServiceUnitCodeOrder.ClientID %>').val(value);
                onTxtServiceUnitCodeOrderChanged(value);
            });
        });

        $('#<%=txtServiceUnitCodeOrder.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeOrderChanged($(this).val());
        });

        function onTxtServiceUnitCodeOrderChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitIDOrder.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitNameOrder.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitIDOrder.ClientID %>').val('');
                    $('#<%=txtServiceUnitCodeOrder.ClientID %>').val('');
                    $('#<%=txtServiceUnitNameOrder.ClientID %>').val('');
                }
                onRefreshGrdOrder();
            });
        }
        //#endregion

        var intervalIDOrder = window.setInterval(function () {
            onRefreshGrdOrder();
        }, interval);

        function onRefreshGrdOrder() {
            if (IsValid(null, 'fsPatientListOrder', 'mpPatientListOrder')) {
                $('#<%=hdnFilterExpressionQuickSearchOrder.ClientID %>').val(txtSearchViewOrder.GenerateFilterExpression());
                window.clearInterval(intervalIDOrder);
                refreshGrdOrderPatient();
                intervalIDOrder = window.setInterval(function () {
                    onRefreshGrdOrder();
                }, interval);
            }
        }

        var intervalIDReg = window.setInterval(function () {
            onRefreshGrdReg();
        }, interval);

        function onRefreshGrdReg() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                // window.clearInterval(intervalIDReg);
                refreshGrdRegisteredPatient();
                intervalIDReg = window.setInterval(function () {
                    onRefreshGrdReg();
                }, interval);
            }
        }

        function onCboDepartmentValueChanged(evt) {
            var cboValue = cboDepartment.GetValue();
            if (cboValue == Constant.Facility.INPATIENT)
                $('#trRegistrationDate').attr('style', 'display:none');
            else
                $('#trRegistrationDate').removeAttr('style');

            $('#<%=hdnFromServiceUnitID.ClientID %>').val('');
            $('#<%=txtFromServiceUnitCode.ClientID %>').val('');
            $('#<%=txtFromServiceUnitName.ClientID %>').val('');
            if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                onRefreshGrdReg();
        }

        function onCboDepartmentOrderValueChanged(evt) {
            $('#<%=hdnServiceUnitIDOrder.ClientID %>').val('');
            $('#<%=txtServiceUnitCodeOrder.ClientID %>').val('');
            $('#<%=txtServiceUnitNameOrder.ClientID %>').val('');
            if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                onRefreshGrdOrder();
        }

        function onCboOrderResultTypeValueChanged(evt) {
            if (IsValid(evt, 'fsPatientListOrder', 'mpPatientListOrder'))
                onRefreshGrdOrder();
        }

        function onCboToServiceUnitOrderValueChanged() {
            if (IsValid(null, 'fsPatientListOrder', 'mpPatientListOrder'))
                onRefreshGrdOrder();
        }

        function onCboToServiceUnitValueChanged() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                onRefreshGrdReg();
        }

        function onTxtSearchViewOrderSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrdOrder();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        function onTxtSearchViewRegSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrdReg();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" id="hdnSetVarMenuRealisasi" runat="server" value="" />
    <div style="padding: 15px;">
        <div class="containerUlTabPage">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerByOrder">
                    <%=GetLabel("Realisasi Order AIO")%></li>
            </ul>
        </div>
        <div style="padding: 2px; display: none" id="containerDaftar" class="containerOrder">
            <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
            <input type="hidden" id="hdnOrderType" runat="server" />
            <div class="pageTitle">
                <%=GetLabel("Pendaftaran Pasien")%>
                :
                <%=GetLabel("Pilih Pasien")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientList">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 150px" />

                                    <col />
                                </colgroup>
                                <tr id="trServiceUnit" runat="server">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Order Tujuan")%></label>
                                    </td>
                                    <td colspan="2">
                                        <input type="hidden" id="hdnToServiceUnitID" value="" runat="server" />
                                        <dxe:ASPxComboBox ID="cboToServiceUnit" ClientInstanceName="cboToServiceUnit" runat="server"
                                            Width="350px">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboToServiceUnitValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Asal Pasien")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="350px"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblServiceUnit">
                                            <%=GetLabel("Unit Pelayanan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnFromServiceUnitID" runat="server" value="" />
                                        <asp:TextBox ID="txtFromServiceUnitCode" Width="120px" runat="server" />
                                        <asp:TextBox ID="txtFromServiceUnitName" ReadOnly="true" Width="350px" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDate" Width="120px" runat="server" CssClass="datepicker" />
                                        <asp:CheckBox ID="chkIsPreviousEpisodePatientReg" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg"
                                            ID="txtSearchViewReg" Width="350px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
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
                        <uc1:ctlGrdRegOrderPatient runat="server" ID="grdRegisteredPatient" />
                    </td>
                </tr>
            </table>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
        <div style="padding: 2px;" id="containerByOrder" class="containerOrder">
            <input type="hidden" id="hdnFilterExpressionOrder" runat="server" value="" />
            <div class="pageTitle">
                <%=GetLabel("Job Order Entry")%>
                :
                <%=GetLabel("Pilih Pasien")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientListOrder">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr id="trServiceUnitOrder" runat="server">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Order Tujuan")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboToServiceUnitOrder" ClientInstanceName="cboToServiceUnitOrder"
                                            runat="server" Width="350px">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboToServiceUnitOrderValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Asal Pasien")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboDepartmentOrder" ClientInstanceName="cboDepartmentOrder"
                                            Width="350px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentOrderValueChanged(e); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblServiceUnitOrder">
                                            <%=GetLabel("Unit Pelayanan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnServiceUnitIDOrder" runat="server" value="" />
                                        <asp:TextBox ID="txtServiceUnitCodeOrder" Width="120px" runat="server" />
                                        <asp:TextBox ID="txtServiceUnitNameOrder" ReadOnly="true" Width="350px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTestOrderDate" Width="120px" runat="server" CssClass="datepicker" />
                                        <asp:CheckBox ID="chkIsPreviousEpisodePatientOrder" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewOrder"
                                            ID="txtSearchViewOrder" Width="350px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewOrderSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Order" FieldName="ServiceOrderNo" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No Transaksi" FieldName="ChargesTransactionNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tampilan Hasil")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType"
                                            Width="350px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(e); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefreshOrder">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                        <uc1:ctlGrdOrderPatient runat="server" ID="grdOrderPatient" />
                    </td>
                </tr>
            </table>
            <div class="imgLoadingGrdView" id="containerImgLoadingView2">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
    </div>
</asp:Content>
