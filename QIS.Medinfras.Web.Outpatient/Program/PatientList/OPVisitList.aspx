<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="OPVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.OPVisitList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl.ascx" TagName="ctlGrdRegOrderPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientOrderCtl.ascx" TagName="ctlGrdOrderPatient" TagPrefix="uc1" %>

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


            //#region Attr Order
            setDatePicker('<%=txtOrderDate.ClientID %>');
            $('#<%=txtOrderDate.ClientID %>').change(function () {
                onRefreshGrdOrder();
            });
            $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            function getHealthcareServiceUnitOrderFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFromOrder.GetValue() + "' AND IsDeleted = 0"; ;
                return filterExpression;
            }

            $('#lblRefreshOrder.lblLink').click(function () {
                onRefreshGrdOrder();
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
                var filterExpression = getHealthcareServiceUnitOrderFilterExpression() + " AND ServiceUnitCode = '" + value + "' AND IsDeleted = 0";
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

            //#region Attr Registrasi
            setDatePicker('<%=txtRealisationDate.ClientID %>');
            $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtRealisationDate.ClientID %>').change(function () {
                onRefreshGrdReg();
            });
            $('#lblRefresh.lblLink').click(function () {
                onRefreshGrdReg();
            });

            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "' AND IsDeleted = 0"; ;
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
                    onRefreshGrdReg();
                });
            }
            //#endregion
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        //#region Order

        $('#<%=chkIsPreviousEpisodePatientOrder.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatientOrder.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtOrderDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtOrderDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdOrder();
        });

        function onCboPatientFromOrderValueChanged() {
            $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
            onRefreshGrdOrder();
        }

        function oncboClinicOrderValueChanged() {
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

        //#region Registration

        $('#<%=chkIsPreviousEpisodePatientReg.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatientReg.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRealisationDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRealisationDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdReg();
        });

        function onCboPatientFromValueChanged() {
            var cboValue = cboPatientFrom.GetValue();
            if (cboValue == Constant.Facility.INPATIENT)
                $('#trRegistrationDate').attr('style', 'display:none');
            else
                $('#trRegistrationDate').removeAttr('style');

            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            onRefreshGrdReg();
        }
        function oncboClinicValueChanged() {
            onRefreshGrdReg();
        }

        var intervalIDReg = window.setInterval(function () {
            onRefreshGrdReg();
        }, interval);

        function onRefreshGrdReg() {
            if (IsValid(null, 'fsPatientListReg', 'mpPatientList')) {
                window.clearInterval(intervalIDReg);
                $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalIDReg = window.setInterval(function () {
                    onRefreshGrdReg();
                }, interval);
            }
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
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnImagingID" runat="server" value="" />
    <input type="hidden" id="hdnLabID" runat="server" value="" />
    <div style="padding:15px">
        <div class="containerUlTabPage" style="margin-bottom:3px;">
           <ul class="ulTabPage" id="ulTabLabResult">
                <li contentid="containerByOrder" style="display:none"><%=GetLabel("Order Pemeriksaan")%></li>
                <li class="selected" contentid="containerDaftar"><%=GetLabel("Pendaftaran Pasien")%></li>
           </ul>
        </div>
        <div id="containerByOrder" class="containerOrder" style="display:none">
            <div class="pageTitle"><%=GetLabel("Job Order Entry")%> : <%=GetLabel("Pilih Pasien")%></div>
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
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Klinik")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboClinicOrder" ClientInstanceName="cboClinicOrder" runat="server" Width="350px">
                                            <ClientSideEvents ValueChanged="function(s,e) { oncboClinicOrderValueChanged(); }"/>
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
                                <tr id="trOrderDate">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                                    <td><asp:TextBox ID="txtOrderDate" Width="120px" runat="server" CssClass="datepicker"/>
                                        <asp:CheckBox ID="chkIsPreviousEpisodePatientOrder" runat="server" Text="Abaikan Tanggal" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewOrder" ID="txtSearchViewOrder"
                                            Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewOrderSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Order" FieldName="TestOrderNo" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan Hasil")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType" Width="150px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(); }" />
                                        </dxe:ASPxComboBox>
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
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle"><%=GetLabel("Pendaftaran Pasien")%> : <%=GetLabel("Pilih Pasien")%></div>
            <fieldset id="fsPatientListReg"> 
                <table class="tblContentArea" style="width:100%">
                    <tr>
                        <td style="padding:5px;vertical-align:top">
                            <table class="tblEntryContent" style="width:60%;">
                                <colgroup>
                                    <col style="width:25%"/>
                                    <col/>
                                </colgroup>
                                <tr id="trServiceUnitOrder" runat="server">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Klinik")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboClinic" ClientInstanceName="cboClinic" runat="server" Width="350px">
                                            <ClientSideEvents ValueChanged="function(s,e) { oncboClinicValueChanged(); }"/>
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
                                    <td class="tdLabel"><label class="lblLink lblNormal" id="lblServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
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
                                                <td><asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                                    <td><asp:TextBox ID="txtRealisationDate" Width="120px" runat="server" CssClass="datepicker"/>
                                        <asp:CheckBox ID="chkIsPreviousEpisodePatientReg" runat="server" Text="Abaikan Tanggal" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg" ID="txtSearchViewReg"
                                            Width="300px" Watermark="Search">
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
                            <div style="padding:7px 0 0 3px;font-size:0.95em">
                                <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%> 
                            </div>
                            <uc1:ctlGrdRegOrderPatient runat="server" id="grdRegisteredPatient" />
                        </td>
                    </tr>
            
                </table>
            </fieldset>
        </div>
    </div>
    
</asp:Content>
