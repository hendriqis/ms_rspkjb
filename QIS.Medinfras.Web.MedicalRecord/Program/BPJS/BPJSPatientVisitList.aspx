<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="BPJSPatientVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.BPJSPatientVisitList" %>

<%@ Register Src="~/libs/Controls/PatientGrid/GridBPJSPatientMRCtl.ascx" TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromRegistrationDate.ClientID %>');
            $('#<%=txtFromRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToRegistrationDate.ClientID %>');
            $('#<%=txtToRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtFromRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#<%=txtToRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            //#region Service Unit
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
                    onRefreshGridView();
                });
            }
            //#endregion
        });

        function onCboPatientFromValueChanged() {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            onRefreshGridView();
        }
        
        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                refreshGrdRegisteredPatient();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        function onCboResultTypeValueChanged() {
            onRefreshGridView();
        }
       
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var logDate = Methods.DatePickerToDateFormat($('#<%=txtFromRegistrationDate.ClientID %>').val());
            if (logDate == '' || logDate == '0') {
                errMessage.text = 'Please Select Date First!';
                return false;
            }
            else {
                filterExpression.text = "ActualVisitDate = '" + logDate + "'";
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <div style="padding: 15px">
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <div class="pageTitle"><%=GetMenuCaption()%></div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:50%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:145px"/>
                                        <col style="width:3px"/>
                                        <col style="width:145px"/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtFromRegistrationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                        <td><%=GetLabel("s/d") %></td>
                                        <td><asp:TextBox ID="txtToRegistrationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server" Width="100%">
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
                                        <td><asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="99%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView" Width="435px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Pendaftaran")%></label></td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus" Width="150px" runat="server">
                                                <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan Hasil")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboResultType" ClientInstanceName="cboResultType" Width="150px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("setiap")%>
                        <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
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
