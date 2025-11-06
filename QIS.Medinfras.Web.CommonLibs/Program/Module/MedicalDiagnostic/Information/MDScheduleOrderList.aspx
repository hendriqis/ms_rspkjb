<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="MDScheduleOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDScheduleOrderList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientOrderCtl.ascx" TagName="ctlGrdOrderPatient" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Attr Order
            setDatePicker('<%=txtOrderDate.ClientID %>');
            $('#<%=txtOrderDate.ClientID %>').change(function () {
                onRefreshGrdOrder();
            });
            $('#<%=txtOrderDate.ClientID %>').datepicker('option', '', '0');

            function getHealthcareServiceUnitOrderFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFromOrder.GetValue() + "'"; ;
                return filterExpression;
            }

            function getBedServiceUnitOrderFilterExpression() {
                var filterExpression = "IsDeleted = 0 AND ServiceUnitCode LIKE '%" + $('#<%=txtServiceUnitOrderCode.ClientID %>').val() + "%'";
                return filterExpression;
            }


            $('#lblRefreshOrder.lblLink').click(function () {
                onRefreshGrdOrder();
            });

            //#region ServiceUnitOrder
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
                        $('#<%=hdnServiceUnitOrderID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitOrderCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitOrderName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
                        $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
                    }
                    onRefreshGrdOrder();
                });
            }
            //#endregion

            //#region Patient From Order

            $('#lblBed.lblLink').click(function () {
                openSearchDialog('bed', getBedServiceUnitOrderFilterExpression(), function (value) {
                    $('#<%=txtBedCode.ClientID %>').val(value);
                    onTxtBedCodeChanged(value);
                });
            });

            $('#<%=txtBedCode.ClientID %>').change(function () {
                onTxtBedCodeChanged($(this).val());
            });

            function onTxtBedCodeChanged(value) {
                var filterExpression = getBedServiceUnitOrderFilterExpression() + " AND BedCode = '" + value + "'";
                Methods.getObject('GetvBedList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBedID.ClientID %>').val(result.BedID);
                        $('#<%=txtBedCode.ClientID %>').val(result.BedCode);
                    }
                    else {
                        $('#<%=hdnBedID.ClientID %>').val('');
                        $('#<%=txtBedCode.ClientID %>').val('');
                    }
                    onRefreshGrdOrder();
                });
            }
            //#endregion
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        //#region Order
        function onCboPatientFromOrderValueChanged() {
            $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderName.ClientID %>').val('');

            if (cboPatientFromOrder.GetValue() == "INPATIENT") {
                $('.trBed').show();
            }
            else {
                $('.trBed').hide();
            }

            onRefreshGrdOrder();
        }

        function onCboMedicalDiagnosticOrderValueChanged() {
            onRefreshGrdOrder();
        }

        function onCboOrderResultTypeValueChanged() {
            onRefreshGrdOrder();
        }

        var intervalIDOrder = window.setInterval(function () {
            onRefreshGrdOrder();
        }, interval);

        function onRefreshGrdOrder() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalIDOrder);
                $('#<%=hdnFilterExpressionQuickSearchOrder.ClientID %>').val(txtSearchViewOrder.GenerateFilterExpression());
                refreshGrdOrderPatient();
                intervalIDOrder = window.setInterval(function () {
                    onRefreshGrdOrder();
                }, interval);
            }
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
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnImagingID" runat="server" value="" />
    <input type="hidden" id="hdnLabID" runat="server" value="" />
    <div style="padding:15px">
        <div id="containerByOrder" class="containerOrder">
            <div class="pageTitle"><%=GetLabel("Jadwal Pelayanan")%></div>
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <fieldset id="fsPatientList"> 
                            <table class="tblEntryContent" style="width:60%;">
                                <colgroup>
                                    <col style="width:25%"/>
                                    <col/>
                                </colgroup>
                                <tr id="trServiceUnit" runat="server">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Penunjang Medis")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboMedicalDiagnosticOrder" ClientInstanceName="cboMedicalDiagnosticOrder" runat="server" Width="350px">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboMedicalDiagnosticOrderValueChanged(); }"/>
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
                                    <td class="tdLabel"><label class="lblLink lblNormal" id="lblServiceUnitOrder"><%=GetLabel("Unit Pelayanan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnServiceUnitOrderID" value="" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtServiceUnitOrderCode" Width="100%" runat="server" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox ID="txtServiceUnitOrderName" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="trBed" style="display:none">
                                    <td class="tdLabel"><label class="lblLink lblNormal" id="lblBed" ><%=GetLabel("Tempat Tidur")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnBedID" value="" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtBedCode" Width="60%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trOrderDate">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                                    <td><asp:TextBox ID="txtOrderDate" Width="120px" runat="server" CssClass="datepicker"/></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewOrder" ID="txtSearchViewOrder"
                                            Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewOrderSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="No Order" FieldName="TestOrderNo" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div style="padding:7px 0 0 3px;font-size:0.95em">
                            <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefreshOrder">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                        </div>
                        <uc1:ctlGrdOrderPatient runat="server" id="grdOrderedPatient" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    
</asp:Content>
