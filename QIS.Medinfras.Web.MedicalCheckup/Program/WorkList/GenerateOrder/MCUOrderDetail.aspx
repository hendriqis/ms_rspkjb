<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="MCUOrderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUOrderDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionTestOrderCtl2.ascx"
    TagName="OrderCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnClinicTransactionBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerateOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnMCUOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Informasi Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table style="float: right" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnConsultVisitItemPackageID" runat="server" />
    <input type="hidden" value="" id="hdnMCUPackageListItemID" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingRegistrationParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnIsGenerateOrderAutoProposed" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultItemIDMCUPackage" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnFromHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDFrom" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDTo" runat="server" />
    <input type="hidden" value="" id="hdnListKey" runat="server" />
    <input type="hidden" value="" id="hdnListDetailItemID" runat="server" />
    <input type="hidden" value="" id="hdnListParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnListIsConfirm" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="0" id="hdnIsItemTambahanPropose" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasAIOPackage" runat="server" />
    <input type="hidden" value="0" id="hdnClassID" runat="server" />
    <input type="hidden" value="0" id="hdnIsEndingAmountRoundingTo1" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                showContent(contentID);
            });

            function showContent(contentID) {
                var i, x, tablinks;
                x = document.getElementsByClassName("divPageNavPanelContent");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(contentID).style.display = "block";
            }
            //#endregion
        });

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'orderPenunjang') {
                return $('#<%:hdnFromHealthcareServiceUnitID.ClientID %>').val();
            }
        }

        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            return filterExpression;
        }

        function getCheckedMember() {
            var listKey = [];
            var listDetailItemID = [];
            var listParamedicID = [];
            var listIsConfirm = [];
            $('.grdMCUOrder tr:gt(0)').each(function () {
                $tr = $(this).closest('tr');
                var key = $tr.find('.hdnKey').val();
                var detailItemID = $tr.find('.hdnDetailItemID').val();
                var paramedicID = $tr.find('.hdnParamedicID').val();
                var isConfirm = '0';
                if ($tr.find('.chkIsConfirm input').is(':checked')) {
                    isConfirm = '1';
                };
                listKey.push(key);
                listDetailItemID.push(detailItemID);
                listParamedicID.push(paramedicID);
                listIsConfirm.push(isConfirm);
            });
            $('#<%=hdnListKey.ClientID %>').val(listKey.join('|'));
            $('#<%=hdnListDetailItemID.ClientID %>').val(listDetailItemID.join('|'));
            $('#<%=hdnListParamedicID.ClientID %>').val(listParamedicID.join('|'));
            $('#<%=hdnListIsConfirm.ClientID %>').val(listIsConfirm.join('|'));
        }

        function oncboServiceUnitValueChanged(s) {
            $('#ulTabMCUTransaction li.selected').removeClass('selected');
            $('#ulTabMCUTransaction li:eq(' + cboServiceUnit.GetSelectedIndex() + ')').addClass('selected');

            var healthcareServiceUnitIDFrom = $('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val();
            var healthcareServiceUnitIDTo = cboServiceUnit.GetValue();
            if ($('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val() != cboServiceUnit.GetValue()) {
                $('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val(cboServiceUnit.GetValue());
            }
            $('#<%=hdnHealthcareServiceUnitIDFrom.ClientID %>').val(healthcareServiceUnitIDFrom);
            $('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val(healthcareServiceUnitIDTo);
            getCheckedMember();
            cbpView.PerformCallback('changeTab');

        }

        function onLoad() {
            $('#leftPageNavPanel ul li').first().addClass('selected');
            $('#ulTabMCUTransaction li:first-child').addClass('selected');

            $('#ulTabMCUTransaction li').click(function () {
                var healthcareServiceUnitIDFrom = $('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val();
                var healthcareServiceUnitIDTo = $(this).find('.hsuID').val();
                $('#<%=hdnHealthcareServiceUnitIDFrom.ClientID %>').val(healthcareServiceUnitIDFrom);
                $('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val(healthcareServiceUnitIDTo);
                cboServiceUnit.SetValue(healthcareServiceUnitIDTo);
                $('#ulTabMCUTransaction li.selected').removeClass('selected');
                $(this).addClass('selected');
                getCheckedMember();
                cbpView.PerformCallback('changeTab');
            });

            $('#<%=btnClinicTransactionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl("~/Program/PatientList/MCUGenerateOrderList.aspx");
            });

            $('#<%=btnGenerateOrder.ClientID %>').click(function () {
                var healthcareServiceUnitIDFrom = $('#<%=hdnHealthcareServiceUnitIDTo.ClientID %>').val();
                $('#<%=hdnHealthcareServiceUnitIDFrom.ClientID %>').val(healthcareServiceUnitIDFrom);
                getCheckedMember();

                var allowContinue = "1";
                var filterDetailItem = "ItemID IN (SELECT isd.DetailItemID FROM ItemServiceDt isd WITH(NOLOCK) WHERE isd.ItemID = " + $('#<%=hdnMCUPackageListItemID.ClientID %>').val() + ") AND (GCItemStatus = 'X181^999' OR IsDeleted = 1)";
                Methods.getObject('GetItemMasterList', filterDetailItem, function (resultIM) {
                    if (resultIM != null) {
                        var itemMessageConfirm = "Detail paket yang bernama item " + resultIM.ItemName1 + " (" + resultIM.ItemCode + ") statusnya non-aktif atau sudah dihapus dari master detail item nya, lanjutkan proses ?";
                        var resultConfirm = confirm(itemMessageConfirm);
                        if (resultConfirm == true) {
                            allowContinue = "1";
                        } else {
                            allowContinue = "0";
                        }
                    }
                });

                if (allowContinue == "1") {
                    var filterOrderOutstanding = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCTransactionStatus = '" + Constant.TransactionStatus.OPEN + "'";
                    Methods.getObject('GetTestOrderHdList', filterOrderOutstanding, function (result) {
                        if (result != null) {
                            showToast('Warning', 'Masih ada Order Penunjang Medis di Task yang belum di-Proposed.');
                        }
                        else {
                            cbpView.PerformCallback('generate');
                        }
                    });
                }
            });

            $('#<%=btnMCUOrder.ClientID %>').click(function () {
                var RegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val() + '|generate';
                var url = ResolveUrl("~/Program/Registration/AddMCUPackageCtl.ascx");
                if (RegistrationID != "" && RegistrationID != "0") {
                    openUserControlPopup(url, RegistrationID, 'Medical Checkup Order', 800, 600);
                }
                else showToast(' Failed', 'Error Message : ' + param[2]);
            });

            $('.lblParamedicName.lblLink').live('click', function () {
                var healthcareServiceUnitID = $(this).closest('tr').find('.hdnHealthareServiceUnitID').val();
                $td = $(this).parent();
                var paramedicID = $td.children('.hdnParamedicID').val();
                openSearchDialog('paramedic', 'IsDeleted = 0 AND IsAvailable = 1', function (value) {
                    onTxtParamedicChanged(value, $td);
                });
            });

            function onTxtParamedicChanged(value, $td) {
                var filterExpression = "IsDeleted = 0 AND IsAvailable = 1 AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $td.find('.lblParamedicName').html(result.ParamedicName);
                        $td.find('.hdnParamedicID').val(result.ParamedicID);
                    }
                });
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                if (param[1] == 'generate') {
                    $('#<%=btnClinicTransactionBack.ClientID %>').trigger('click');
                }
                if (param[1] == 'saveParamedic') {
                    $('#<%:hdnParamedicIDPerUnit.ClientID %>').val('');
                    $('#<%:hdnParamedicCodePerUnit.ClientID %>').val('');
                    $('#<%:txtParamedicCodePerUnit.ClientID %>').val('');
                    $('#<%:hdnParamedicNamePerUnit.ClientID %>').val('');
                    $('#<%:txtParamedicNamePerUnit.ClientID %>').val('');
                }
            }
            else {
                $('#ulTabMCUTransaction li.selected').removeClass('selected');
                $('#ulTabMCUTransaction li:first-child').addClass('selected');
                showToast('Error', param[1]);
            }
        }

        //#region Physician
        function onGetParamedicFilterExpression() {
            var healthcareServiceUnitID = cboServiceUnit.GetValue();
            var filterExpression = 'IsDeleted = 0 AND IsAvailable = 1';
            return filterExpression;
        }

        $('#<%:lblParamedicPerUnit.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetParamedicFilterExpression(), function (value) {
                $('#<%:txtParamedicCodePerUnit.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        });

        $('#<%:txtParamedicCodePerUnit.ClientID %>').live('change', function () {
            onTxtParamedicCodeChanged($(this).val());
        });

        function onTxtParamedicCodeChanged(value) {
            var filterExpression = onGetParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnParamedicIDPerUnit.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtParamedicCodePerUnit.ClientID %>').val(result.ParamedicCode);
                    $('#<%:hdnParamedicCodePerUnit.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtParamedicNamePerUnit.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnParamedicNamePerUnit.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%:hdnParamedicIDPerUnit.ClientID %>').val('');
                    $('#<%:hdnParamedicCodePerUnit.ClientID %>').val('');
                    $('#<%:txtParamedicCodePerUnit.ClientID %>').val('');
                    $('#<%:hdnParamedicNamePerUnit.ClientID %>').val('');
                    $('#<%:txtParamedicNamePerUnit.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#btnSaveParamedicChange').live('click', function () {
            var paramedic = $('#<%:txtParamedicCodePerUnit.ClientID %>').val();
            if (paramedic != '') {
                showToastConfirmation("Apakah yakin akan merubah Dokter ?", function (resultbtn) {
                    if (resultbtn) {
                        cbpView.PerformCallback('saveParamedic');
                    }
                })
            }
            else {
                showToast('Save Failed', 'Pilih dokter terlebih dahulu');
            }
        });
    </script>
    <style>
        .containerUlTabPage
        {
            height: auto !important;
        }
    </style>
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 15%" />
                <col style="width: 85%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentid="divPageGenerateOrder" title="Generate Order" class="w3-hover-red">
                                <%=GetLabel("Generate Order")%></li>
                            <li contentid="divPageOrderDiagnostic" title="Order Penunjang Medis" class="w3-hover-red">
                                <%=GetLabel("Order Penunjang Medis")%></li>
                        </ul>
                    </div>
                </td>
                <td colspan="2">
                    <div id="divPageGenerateOrder" class="w3-border divPageNavPanelContent w3-animate-left">
                        <div>
                            <table>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblServiceUnit">
                                            <%=GetLabel("Unit Pelayanan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { oncboServiceUnitValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblParamedicPerUnit" runat="server">
                                            <%=GetLabel("Dokter")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 600px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCodePerUnit" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicNamePerUnit" Width="100%" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="blink-alert" align="left" colspan="2">
                                        Proses Simpan akan mengubah dokter untuk seluruh unit pelayanan yang dipilih
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 245px" />
                                                <col />
                                            </colgroup>
                                            <tr align="right">
                                                <td>
                                                </td>
                                                <td>
                                                    <input type="button" id="btnSaveParamedicChange" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="containerUlTabPage">
                            <asp:Repeater ID="rptHSU" runat="server">
                                <HeaderTemplate>
                                    <ul class="ulTabPage" id="ulTabMCUTransaction">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" class="hsuID" />
                                        <%#: Eval("ServiceUnitName")%>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div>
                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                            position: relative; font-size: 0.95em;">
                                            <input type="hidden" id="hdnParamedicIDPerUnit" value="" runat="server" />
                                            <input type="hidden" id="hdnParamedicCodePerUnit" value="" runat="server" />
                                            <input type="hidden" id="hdnParamedicNamePerUnit" value="" runat="server" />
                                            <input type="hidden" value="" id="hdnNumberOfItems" runat="server" />
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th class="keyField">
                                                                &nbsp;
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Kode Item")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Nama Item")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Unit Pelayanan")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Dokter/Paramedis")%>
                                                            </th>
                                                            <th style="width: 60px" align="center">
                                                                <%=GetLabel("Dikerjakan")%>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="20">
                                                                <%=GetLabel("Tidak ada Order untuk unit ini.")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th class="keyField">
                                                                &nbsp;
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Kode Item")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Nama Item")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Unit Pelayanan")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Dokter/Paramedis")%>
                                                            </th>
                                                            <th style="width: 60px" align="center">
                                                                <%=GetLabel("Dikerjakan")%>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="keyField">
                                                            <%#: Eval("DetailItemID")%>
                                                        </td>
                                                        <td align="left">
                                                            <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                            <input type="hidden" value="" class="hdnDetailItemID" id="hdnDetailItemID" runat="server" />
                                                            <%#: Eval("DetailItemCode")%>
                                                        </td>
                                                        <td align="left">
                                                            <%#: Eval("DetailItemName1")%>
                                                        </td>
                                                        <td align="left">
                                                            <%#: Eval("ServiceUnitName")%>
                                                            - <span style="font-style: italic">
                                                                <%#:Eval("DepartmentName") %></span>
                                                        </td>
                                                        <td>
                                                            <input type="hidden" value="" class="hdnParamedicID" id="hdnParamedicID" runat="server" />
                                                            <label class="lblParamedicName lblLink" runat="server" id="lblParamedicName">
                                                            </label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkIsConfirm" runat="server" CssClass="chkIsConfirm" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div id="divPageOrderDiagnostic" class="w3-border divPageNavPanelContent w3-animate-left"
                        style='display: none'>
                        <uc1:OrderCtl ID="ctlOrder" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
