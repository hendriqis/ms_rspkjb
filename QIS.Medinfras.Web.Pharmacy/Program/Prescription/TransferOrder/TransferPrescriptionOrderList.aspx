<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="TransferPrescriptionOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.TransferPrescriptionOrderList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/CheckGridPatientOrderPharmacyCtl.ascx"
    TagName="ctlGrdOrderPatient" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnTransferOrder">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtTestOrderDate.ClientID %>');
            $('#<%=txtTestOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');


            $('#<%=txtTestOrderDate.ClientID %>').change(function (evt) {
                onRefreshGrdOrder();
            });

            $('#<%=btnTransferOrder.ClientID %>').click(function () {
                var param = getCheckedRow();
                if (param == '') {
                    showToast('Warning', 'Order Resep yang ingin ditransfer belum dipilih');
                }
                else if (cboToServiceUnitOrder.GetText() == '') {
                    showToast('Warning', 'Tujuan Lokasi Farmasi belum dipilih');
                }
                else {
                    $('#<%=hdnParam.ClientID %>').val(param);
                    if (IsValid(null, 'fsPatientListOrder', 'mpPatientEntry')) {
                        var message = "Lanjutkan proses transfer order dari <b>" + cboServiceUnitOrder.GetText() + " </b> ke <b>" + cboToServiceUnitOrder.GetText() + "</b> ?";
                        showToastConfirmation(message, function (result) {
                            if (result) {
                                onCustomButtonClick('transfer');
                            }
                        });
                    }
                }
            });
        });

        $('.lblPrescriptionOrderNo.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $tr.find('.keyField').html();
            alert(id);
            event.preventDefault();
        });

        $('.imgOtherDispensaryOrder').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $tr.find('.keyField').html();
            alert('Informasi Order');
            event.preventDefault();
        });

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

        function onRefreshGrdOrder() {
            if (IsValid(null, 'fsPatientListOrder', 'mpPatientListOrder')) {
                $('#<%=hdnFilterExpressionQuickSearchOrder.ClientID %>').val(txtSearchViewOrder.GenerateFilterExpression());
                refreshGrdOrderPatient();
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

        $('#lblRefreshOrder.lblLink').live('click', function () {
            onRefreshGrdOrder();
        });

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

        function onAfterCustomClickSuccess(type) {
            var message = "Proses transfer order dari <b>" + cboServiceUnitOrder.GetText() + " </b> ke <b>" + cboToServiceUnitOrder.GetText() + "</b> ?";
            showToast("MEDINFRAS", 'PROSES BERHASIL : ' + message);
            onRefreshGrdOrder();
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;

        var intervalIDReg = window.setInterval(function () {
            onRefreshGrdReg();
        }, interval);
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnQuickTextOrder" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingUDD" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <div>
            <input type="hidden" id="hdnFilterExpressionOrder" runat="server" value="" />
            <div class="pageTitle">
                <%=GetLabel("Daftar Order Resep")%>
                :
                <%=GetLabel("Pilih Order yang ingin ditransfer")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientListOrder">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="padding: 20px">
                                        <table style="width: 100%">
                                            <colgroup width="70px" />
                                            <colgroup />
                                            <tr>
                                                <td>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/warning.png")%>' alt="" height="65px" width="65px" />
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <h4 style="background-color: transparent; color: red; font-weight: bold">
                                                        <%=GetLabel("PERINGATAN : Proses tidak bisa dibatalkan")%></h4>
                                                    <%=GetLabel("Harap lakukan konfirmasi kepada lokasi asal order sebelum dilakukan proses transfer karena bisa terjadi order sedang dikerjakan")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Farmasi")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnitOrder" ClientInstanceName="cboServiceUnitOrder"
                                            Width="100%" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { refreshGrdOrderPatient(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Asal Pasien")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDepartmentOrder" ClientInstanceName="cboDepartmentOrder"
                                            Width="100%" runat="server">
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
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtServiceUnitCodeOrder" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtServiceUnitNameOrder" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTestOrderDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewOrder"
                                            ID="txtSearchViewOrder" Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewOrderSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Order" FieldName="PrescriptionOrderNo" />
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
                                            <%=GetLabel("Transfer ke Farmasi")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboToServiceUnitOrder" ClientInstanceName="cboToServiceUnitOrder"
                                            Width="100%" runat="server">
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
    <script type="text/javascript">
        $(function () {
            txtSearchViewOrder.SetText($('#<%=hdnQuickTextOrder.ClientID %>').val());
        });
    </script>
</asp:Content>
