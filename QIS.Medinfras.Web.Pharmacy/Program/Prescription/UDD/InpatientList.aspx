<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="InpatientList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.InpatientList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientListUDDCtl.ascx" TagName="ctlGrdRegOrderPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientOrderUDDCtl.ascx" TagName="ctlGrdOrderPatient" TagPrefix="uc2" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientNotificationListUDDCtl.ascx" TagName="ctlGrdPatientNotificationList" TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
    </script>
    
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" value="" id="hdnLastContentID" runat="server" />
    <input type="hidden" value="" id="hdnQuickTextOrder" runat="server" />
    <input type="hidden" value="" id="hdnQuickTextReg" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingUDD" runat="server" />
    <input type="hidden" id="hdnRegistrationPayer" runat="server" value="" />
    <div style="padding:5px;">
        <div>
            <fieldset id="fsPatientList">  
                <table class="tblContentArea" style="width:100%;">
                    <colgroup>
                        <col style="width:25%"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Farmasi")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
                        <td>
                            <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:120px"/>
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
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tipe Penjamin")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboRegistrationPayer" ClientInstanceName="cboRegistrationPayer" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboPayerValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg" ID="txtSearchViewReg"
                                Width="100%" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                    <qis:QISIntellisenseHint Text="No.Bed" FieldName="BedCode" />
                                    <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                    <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                    <tr id="trDate" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                        <td><asp:TextBox ID="txtOrderDate" Width="120px" runat="server" CssClass="datepicker" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status Perawatan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboCheckinStatus" ClientInstanceName="cboCheckinStatus" Width="250px"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboCheckedInValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trPrescriptionType" runat="server">
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Jenis Order")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                Width="250px" runat="server" >
                                <ClientSideEvents ValueChanged="function(s,e) { onCboPrescriptionTypeValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trOrderType" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Proses Order")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType" Width="250px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(e); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trOrderStatus" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Konfirmasi Order")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboOrderStatusType" ClientInstanceName="cboOrderStatusType" Width="250px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboOrderStatusTypeValueChanged(e); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trSortBy" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" style="text-decoration:underline">
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
                        <td colspan="2">
                            <div style="padding:7px 0 10px 3px;font-size:0.95em">
                                <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div class="containerUlTabPage">
           <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerByOrder"><%=GetLabel("Daftar Order") %></li>
                <li contentid="containerCPPT"><%=GetLabel("Notifikasi via CPPT")%></li>
                <li contentid="containerDaftar"><%=GetLabel("Daftar Pasien Rawat Inap")%></li>
           </ul>
        </div>
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <div style="padding:2px;" id="containerByOrder" class="containerOrder">
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <uc2:ctlGrdOrderPatient runat="server" id="grdOrderPatient" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding:2px;display:none;" id="containerDaftar" class="containerOrder">
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <uc1:ctlGrdRegOrderPatient runat="server" id="grdRegisteredPatient" />
                    </td>
                </tr>
            </table>
        
            <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
        <div style="padding:2px;display:none;" id="containerCPPT" class="containerOrder">
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <uc3:ctlGrdPatientNotificationList runat="server" id="ctlGrdPatientNotificationList" />
                    </td>
                </tr>
            </table>
        
            <div class="imgLoadingGrdView" id="Div2" >
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchViewReg.SetText($('#<%=hdnQuickTextReg.ClientID %>').val());

            setDatePicker('<%=txtOrderDate.ClientID %>');
            $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar') {
                $container = $('#ulTabLabResult').find("[contentid='containerDaftar']");
                $('#ulTabLabResult li.selected').removeAttr('class');
                $container.addClass('selected');
                $('#containerByOrder').hide();
                $('#containerCPPT').hide();
                $('#containerDaftar').show();

                onRefreshGrdReg();
            }
            else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT') {
                $container = $('#ulTabLabResult').find("[contentid='containerCPPT']");
                $('#ulTabLabResult li.selected').removeAttr('class');
                $container.addClass('selected');
                $('#containerByOrder').hide();
                $('#containerCPPT').show();
                $('#containerDaftar').hide();

                onRefreshGrdCPPT();
            }
            else {
                $container = $('#ulTabLabResult').find("[contentid='containerByOrder']");
                $('#ulTabLabResult li.selected').removeAttr('class');
                $container.addClass('selected');
                $('#containerByOrder').show();
                $('#containerCPPT').hide();
                $('#containerDaftar').hide();

                onRefreshGrdOrder();
            }

            $('#<%=txtOrderDate.ClientID %>').change(function (evt) {
                onRefreshGrdOrder();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                    onRefreshGrdReg();
                else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT')
                    onRefreshGrdCPPT();
                else
                    onRefreshGrdOrder();
            });

            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerOrder').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnLastContentID.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                ToggleInpatientControl();
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                    onRefreshGrdReg();
                else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT')
                    onRefreshGrdCPPT();
                else
                    onRefreshGrdOrder();
            });

            cboDepartment.SetValue(Constant.Facility.Inpatient);
            ToggleInpatientControl()
        });

        //#region Registration
        //#region Service Unit Registration
        $('#lblServiceUnit.lblLink').live('click', function () {
            var DepartmentID = cboDepartment.GetValue();
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0 AND IsUsingRegistration = 1";
            openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                    onRefreshGrdReg();
                else
                    onRefreshGrdOrder();
            });
        }
        //#endregion

        function onCboCheckedInValueChanged() {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                onRefreshGrdReg();
            else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT')
                onRefreshGrdCPPT();
            else
                onRefreshGrdOrder();
        }

        function onCboPrescriptionTypeValueChanged() {
            onRefreshGrdOrder();
        }

        function onCboSortByValueChanged() {
            onRefreshGrdOrder();
        }

        function onCboPayerValueChanged() {
            $('#<%=hdnRegistrationPayer.ClientID %>').val(cboRegistrationPayer.GetValue());

            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                onRefreshGrdReg();
            else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT')
                onRefreshGrdCPPT();
            else
                onRefreshGrdOrder();
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;

        var intervalGrid = window.setInterval(function () {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                onRefreshGrdReg();
            else
                onRefreshGrdOrder();
        }, interval);

        function onRefreshGrdReg() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalGrid);
                if (txtSearchViewReg != null)
                    $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalGrid = window.setInterval(function () {
                    onRefreshGrdReg();
                }, interval);
            }
        }

        function onRefreshGrdCPPT() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalGrid);
                if (txtSearchViewReg != null)
                    $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                refreshPatientGrid();
                intervalGrid = window.setInterval(function () {
                    onRefreshGrdCPPT();
                }, interval);
            }
        }

        function onRefreshGrdOrder() {
            if (IsValid(null, 'fsPatientList', 'mpPatientListOrder')) {
                window.clearInterval(intervalGrid);
                if (txtSearchViewReg != null)
                    $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                refreshGrdOrderPatient();
                intervalIDOrder = window.setInterval(function () {
                    onRefreshGrdOrder();
                }, interval);
            }
        }

        function onCboOrderResultTypeValueChanged(evt) {
            onRefreshGrdOrder();
        }

        function onCboOrderStatusTypeValueChanged(evt) {
            onRefreshGrdOrder();
        }

        function onCboServiceUnitChanged(evt) {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                onRefreshGrdReg();
            else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT')
                onRefreshGrdCPPT();
            else
                onRefreshGrdOrder();
        }

        function onCboDepartmentValueChanged(evt) {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                onRefreshGrdReg();
            else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT')
                onRefreshGrdCPPT();
            else
                onRefreshGrdOrder();
        }

        function ToggleInpatientControl() {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar') {
                $('#<%=trDate.ClientID %>').attr('style', 'display:none');
                $('#<%=trPrescriptionType.ClientID %>').attr('style', 'display:none');
                $('#<%=trOrderType.ClientID %>').attr('style', 'display:none');
                $('#<%=trOrderStatus.ClientID %>').attr('style', 'display:none');
                $('#<%=trSortBy.ClientID %>').attr('style', 'display:none');
            }
            else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerCPPT') {
                $('#<%=trDate.ClientID %>').attr('style', 'display:none');
                $('#<%=trPrescriptionType.ClientID %>').attr('style', 'display:none');
                $('#<%=trOrderType.ClientID %>').attr('style', 'display:none');
                $('#<%=trOrderStatus.ClientID %>').attr('style', 'display:none');
                $('#<%=trSortBy.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%=trDate.ClientID %>').removeAttr('style');
                $('#<%=trPrescriptionType.ClientID %>').removeAttr('style');
                $('#<%=trOrderType.ClientID %>').removeAttr('style');
                $('#<%=trOrderStatus.ClientID %>').removeAttr('style');
                $('#<%=trSortBy.ClientID %>').removeAttr('style');
            }
        }

        function onTxtSearchViewRegSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                    onRefreshGrdReg();
                else
                    onRefreshGrdOrder();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }
        //#endregion
    </script>
</asp:Content>
