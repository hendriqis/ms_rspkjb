<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="PatientList.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.PatientList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientListNutritionCtl.ascx" TagName="ctlGrdRegOrderPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientOrderNutritionCtl.ascx" TagName="ctlGrdOrderPatient" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtOrderDate.ClientID %>');
            $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');

            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar') {
                $container = $('#ulTabLabResult').find("[contentid='containerDaftar']");
                $('#ulTabLabResult li.selected').removeAttr('class');
                $container.addClass('selected');
                $('#containerByOrder').hide();
                $('#containerDaftar').show();
            }

            $('#<%=txtOrderDate.ClientID %>').change(function (evt) {
                onRefreshGrdOrder();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                    onRefreshGrdReg();
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
            var filterExpression = "ServiceUnitCode = '" + value + "'";
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

        function onCboDepartmentValueChanged(evt) {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar')
                onRefreshGrdReg();
            else
                onRefreshGrdOrder();
        }

        function ToggleInpatientControl() {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerDaftar') {
                $('#<%=trDate.ClientID %>').attr('style', 'display:none');
                $('#<%=trOrderType.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%=trDate.ClientID %>').removeAttr('style');
                $('#<%=trOrderType.ClientID %>').removeAttr('style');
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
    
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" value="" id="hdnLastContentID" runat="server" />
    <input type="hidden" value="" id="hdnQuickTextOrder" runat="server" />
    <input type="hidden" value="" id="hdnQuickTextReg" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingUDD" runat="server" />
    <div style="padding:15px;">
        <div>
            <fieldset id="fsPatientList">  
                <table class="tblContentArea" style="width:100%;">
                    <colgroup>
                        <col style="width:25%"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Instalasi Gizi")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { refreshGrdRegisteredPatient(); }" />
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
                    <tr id="trOrderType" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Order")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType" Width="250px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(e); }" />
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
                <li class="selected" contentid="containerByOrder"><%=GetLabel("Order Asuhan Gizi") %></li>
                <li contentid="containerDaftar"><%=GetLabel("Pendaftaran")%></li>
           </ul>
        </div>
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <div style="padding:2px;" id="containerByOrder" class="containerOrder">
            <div class="pageTitle"><%=GetLabel("Daftar Order")%> : <%=GetLabel("Pilih Pasien")%></div>
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <uc2:ctlGrdOrderPatient runat="server" id="grdOrderPatient" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding:2px;display:none;" id="containerDaftar" class="containerOrder">
            <div class="pageTitle"><%=GetLabel("Daftar Pasien")%> : <%=GetLabel("Pilih Pasien")%></div>
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
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchViewReg.SetText($('#<%=hdnQuickTextReg.ClientID %>').val());
        });
    </script>
</asp:Content>
