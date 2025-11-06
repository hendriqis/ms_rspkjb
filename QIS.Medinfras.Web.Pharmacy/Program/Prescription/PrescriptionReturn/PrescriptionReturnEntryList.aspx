<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="PrescriptionReturnEntryList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionReturnEntryList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl.ascx" TagName="ctlGrdRegOrderPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientOrderPharmacyPrescriptionReturnCtl.ascx" TagName="ctlGrdOrderPatient" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDate.ClientID %>');
            setDatePicker('<%=txtTestOrderDate.ClientID %>');
            $('#<%=txtDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtTestOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtDate.ClientID %>').change(function (evt) {
                onRefreshGrdReg();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGrdReg();
            });

            $('#lblRefreshOrder.lblLink').click(function (evt) {
                onRefreshGrdOrder();
            });

            $('#<%=txtTestOrderDate.ClientID %>').change(function (evt) {
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
                onRefreshGrdReg();
            });
        }
        //#endregion

        function onCboDepartmentValueChanged(evt) {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            if (cboDepartment.GetValue() == 'INPATIENT')
                $('#<%=trTanggal.ClientID %>').attr('style', 'display:none');
            else
                $('#<%=trTanggal.ClientID %>').removeAttr('style');
            onRefreshGrdReg();
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

        //#region Order
        //#region Service Unit Order
        $('#lblServiceUnitOrder.lblLink').live('click', function () {
            var DepartmentID = cboDepartmentOrder.GetValue();
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0 AND IsUsingRegistration = 1";
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
        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;

        var intervalIDReg = window.setInterval(function () {
            onRefreshGrdReg();
        }, interval);

        function onRefreshGrdReg() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalIDReg);
                $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalIDReg = window.setInterval(function () {
                    onRefreshGrdReg();
                }, interval);
            }
        }

        var intervalIDOrder = window.setInterval(function () {
            onRefreshGrdOrder();
        }, interval);

        function onRefreshGrdOrder() {
            if (IsValid(null, 'fsPatientListOrder', 'mpPatientListOrder')) {
                window.clearInterval(intervalIDOrder);
                $('#<%=hdnFilterExpressionQuickSearchOrder.ClientID %>').val(txtSearchViewOrder.GenerateFilterExpression());
                refreshGrdOrderPatient();
                intervalIDOrder = window.setInterval(function () {
                    onRefreshGrdOrder();
                }, interval);
            }
        }

        function onCboDepartmentOrderValueChanged(evt) {
            $('#<%=hdnServiceUnitIDOrder.ClientID %>').val('');
            $('#<%=txtServiceUnitCodeOrder.ClientID %>').val('');
            $('#<%=txtServiceUnitNameOrder.ClientID %>').val('');
            onRefreshGrdOrder();
        }

        function onCboOrderResultTypeValueChanged(evt) {
            onRefreshGrdOrder();
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

        $('#<%=chkIgnoreDateOrder.ClientID %>').die();
        $('#<%=chkIgnoreDateOrder.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtTestOrderDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtTestOrderDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdOrder();
        });

        $('#<%=chkIsIgnoreDate.ClientID %>').die();
        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdReg();
        });
    </script>
    
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <div style="padding:15px;">
        <div class="containerUlTabPage">
           <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerByOrder"><%=GetLabel("Order Retur Resep") %></li>
                <li contentid="containerDaftar"><%=GetLabel("Pendaftaran")%></li>
           </ul>
        </div>

        <div style="padding:2px;display:none;" id="containerDaftar" class="containerOrder">
            <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
            <div class="pageTitle"><%=GetLabel("Daftar Registrasi")%> : <%=GetLabel("Pilih Pasien")%></div>
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
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Farmasi")%></label></td>
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
                                <tr id="trTanggal" runat="server">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                                    <td><asp:TextBox ID="txtDate" Width="120px" runat="server" CssClass="datepicker" />&nbsp;<asp:CheckBox ID="chkIsIgnoreDate" runat="server" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;</td>
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
                        </fieldset>
                        <div style="padding:7px 0 0 3px;font-size:0.95em">
                            <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                        </div>

                        <uc1:ctlGrdRegOrderPatient runat="server" id="grdRegisteredPatient" />
                    </td>
                </tr>
            </table>
        
            <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>

        <div style="padding:2px" id="containerByOrder" class="containerOrder">
            <input type="hidden" id="hdnFilterExpressionOrder" runat="server" value="" />
            <div class="pageTitle"><%=GetLabel("Daftar Order Retur Resep")%> : <%=GetLabel("Pilih Pasien")%></div>
            <table class="tblContentArea" style="width:100%">
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <fieldset id="fsPatientListOrder">  
                            <table class="tblEntryContent" style="width:60%;">
                                <colgroup>
                                    <col style="width:25%"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Farmasi")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnitOrder" ClientInstanceName="cboServiceUnitOrder" Width="100%" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { refreshGrdOrderPatient(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDepartmentOrder" ClientInstanceName="cboDepartmentOrder" Width="100%" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentOrderValueChanged(e); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblServiceUnitOrder"><%=GetLabel("Unit Pelayanan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnServiceUnitIDOrder" runat="server" value="" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtServiceUnitCodeOrder" Width="100%" runat="server" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox ID="txtServiceUnitNameOrder" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                                    <td><asp:TextBox ID="txtTestOrderDate" Width="120px" runat="server" CssClass="datepicker" />&nbsp;<asp:CheckBox ID="chkIgnoreDateOrder" runat="server" Checked="true" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewOrder" ID="txtSearchViewOrder"
                                            Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewOrderSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No.Order" FieldName="PrescriptionReturnOrderNo" />
                                                <qis:QISIntellisenseHint Text="No.Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan Hasil")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType" Width="150px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(e); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div style="padding:7px 0 0 3px;font-size:0.95em">
                            <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefreshOrder">[refresh]</span> <%=GetLabel("setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                        </div>
                        <uc1:ctlGrdOrderPatient runat="server" id="grdOrderPatient" />
                    </td>
                </tr>
            </table>
            <div class="imgLoadingGrdView" id="containerImgLoadingView2" >
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </div>
    </div>
</asp:Content>
