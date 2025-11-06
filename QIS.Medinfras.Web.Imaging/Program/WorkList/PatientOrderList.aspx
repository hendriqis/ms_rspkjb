<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="PatientOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.WorkList.PatientOrderList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl.ascx" TagName="ctlGrdRegOrderPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientOrderCtl.ascx" TagName="ctlGrdOrderPatient" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">

        var realDate;
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');

        $(function () {
            $('#containerDaftar').filter(':visible').hide();

            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerOrder').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });


            //#region Attr Order
            setDatePicker('<%=txtOrderDate.ClientID %>');
            $('#<%=txtOrderDate.ClientID %>').change(function () {
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    cbpViewOrder.PerformCallback('refresh');
            });
            $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
            
            function getHealthcareServiceUnitOrderFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFromOrder.GetValue() + "'"; ;
                return filterExpression;
            }

            $('#lblRefreshOrder.lblLink').click(function () {
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    cbpViewOrder.PerformCallback('refresh');
            });

            $('#lblServiceUnitOrder.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitOrderFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitOrderCode.ClientID %>').val(value);
                    onTxtServiceUnitOrderCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitOrderCode.ClientID %>').change(function () {
                onTxtServiceUnitOrderCodeChanged($(this).val());
            });

            function onTxtServiceUnitOrderCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitOrderFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitOrderCode.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitOrderCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitOrderName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitOrderCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
                    }
                    if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                        cbpViewOrder.PerformCallback('refresh');
                });
            }
            //#endregion

            //#region Attr Registrasi

            $('#<%=txtServiceUnitOrderName.ClientID %>').attr("readonly", "readonly");
            $('#<%=txtServiceUnitName.ClientID %>').attr("readonly", "readonly");

            setDatePicker('<%=txtRealisationDate.ClientID %>');
            $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtRealisationDate.ClientID %>').change(function () {
                if (IsValid(null, 'fsPatientListReg', 'mpPatientList'))
                    cbpView.PerformCallback('refresh');
            });
            $('#lblRefresh.lblLink').click(function () {
                if (IsValid(null, 'fsPatientListReg', 'mpPatientList'))
                    cbpView.PerformCallback('refresh');
            });


            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "'"; ;
                return filterExpression;
            }


            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                    if (IsValid(null, 'fsPatientListReg', 'mpPatientList'))
                        cbpView.PerformCallback('refresh');
                });
            }


            //#endregion
        });

        //#region callback
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onCboPatientFromValueChanged() {
            if (IsValid(null, 'fsPatientListReg', 'mpPatientList')) {
                cbpView.PerformCallback('refresh');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }
        }

        function onCboPatientFromOrderValueChanged() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                cbpViewOrder.PerformCallback('refresh');
                $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
                $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
            }
        }

        function onCboMedicSupportOrderValueChanged() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
            cbpViewOrder.PerformCallback('refresh');
        }

        function onCboMedicSupportValueChanged() {
            if (IsValid(null, 'fsPatientListReg', 'mpPatientList'))
                cbpView.PerformCallback('refresh');
        }

        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

    </script>
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div style="padding:15px">
        <div class="containerUlTabPage" style="margin-bottom:3px;">
           <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerByOrder"><%=GetLabel("Order Pemeriksaan") %></li>
                <li contentid="containerDaftar"><%=GetLabel("Pendaftaran Pemeriksaan")%></li>
           </ul>
        </div>
        <div id="containerByOrder" class="containerOrder">
        <div class="pageTitle"><%=GetLabel("Work List : Daftar Order Pemeriksaan")%></div>
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
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Order")%></label></td>
                                <td><asp:TextBox ID="txtOrderDate" Width="120px" runat="server" CssClass="datepicker"/></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Penunjang Medis")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicSupportOrder" ClientInstanceName="cboMedicSupportOrder" runat="server" Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboMedicSupportOrderValueChanged(); }"/>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFromOrder" ClientInstanceName="cboPatientFromOrder" runat="server" Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromOrderValueChanged(); }"/>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblNormal" id="lblServiceUnitOrder"><%=GetLabel("Service Unit")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitOrderCode" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtServiceUnitOrderCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtServiceUnitOrderName" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </fieldset>
                        <div style="padding:7px 0 0 3px;font-size:0.95em">
                            <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefreshOrder">[refresh]</span> <%=GetLabel("Setiap 10 Menit")%>
                        </div>
                        <uc1:ctlGrdOrderPatient runat="server" id="grdOrderedPatient" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle"><%=GetLabel("Work List : Daftar Pendaftaran Pemeriksaan")%></div>
            <fieldset id="fsPatientListReg"> 
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Rencana Realisasi")%></label></td>
                                <td><asp:TextBox ID="txtRealisationDate" Width="120px" runat="server" CssClass="datepicker"/></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Penunjang Medis")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicSupport" ClientInstanceName="cboMedicSupport" runat="server" Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboMedicSupportValueChanged(); }"/>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server" Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }"/>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblNormal" id="lblServiceUnit"><%=GetLabel("Service Unit")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div style="padding:7px 0 0 3px;font-size:0.95em">
                            <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap 10 Menit")%>
                        </div>
                        <uc1:ctlGrdRegOrderPatient runat="server" id="grdRegisteredPatient" />
                    </td>
                </tr>
            
            </table>
            </fieldset>
        </div>
    </div>
    
</asp:Content>
