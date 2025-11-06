<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferralCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ReferralCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_Referralctl">
    $('#lblReferalAddData').die('click');
    $('#lblReferalAddData').live('click', function () {
        $('#<%=hdnIDCtl.ClientID %>').val('');
        $('#<%=txtReferrerCodeCtl.ClientID %>').val('');
        $('#<%=txtKodeExternalCtl.ClientID %>').val('');
        $('#<%=txtReferrerNameCtl.ClientID %>').val('');
        $('#<%=txtShortNameReferrerCtl.ClientID %>').val('');
        $('#<%=txtContactReferrerCtl.ClientID %>').val('');
        $('#<%=txtTelephoneNoCtl.ClientID %>').val('');
        cboReferrerGroupCtl.SetSelectedIndex(0);
        $('#<%=txtZipCodeCtl.ClientID %>').val('');
        $('#<%=txtCountyCtl.ClientID %>').val('');
        $('#<%=txtDistrictCtl.ClientID %>').val('');
        $('#<%=txtCityCtl.ClientID %>').val('');
        $('#<%=txtAddressCtl.ClientID %>').val('');
        $('#<%=txtProvinceCodeCtl.ClientID %>').val('');
        $('#<%=txtProvinceNameCtl.ClientID %>').val('');
        $('#<%=hdnZipCodeCtl.ClientID %>').val('');
        $('#containerReferrerEntryDataCtl').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIDCtl.ClientID %>').val(entity.ID);

            var filterExpression = "ReferrerID = " + entity.ID;
            Methods.getObject('GetRegistrationList', filterExpression, function (result) {
                if (result != null) {
                    showToastConfirmation("Maaf, Rujukan Ini Sudah Pernah Digunakan. Tekan Lanjut untuk Menghapus", function (resultbtn) {
                        if (resultbtn) {
                            cbpView.PerformCallback('delete');
                        } else {
                            cbpView.PerformCallback('refresh');
                        }
                    })
                } else {
                    cbpView.PerformCallback('delete');
                }
            })
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnIDCtl.ClientID %>').val(entity.ID);
        $('#<%=txtReferrerCodeCtl.ClientID %>').val(entity.BusinessPartnerCode);
        $('#<%=txtKodeExternalCtl.ClientID %>').val(entity.CommCode);
        $('#<%=txtReferrerNameCtl.ClientID %>').val(entity.BusinessPartnerName);
        $('#<%=txtShortNameReferrerCtl.ClientID %>').val(entity.ShortName);
        $('#<%=txtContactReferrerCtl.ClientID %>').val(entity.ContactPerson);
        $('#<%=txtTelephoneNoCtl.ClientID %>').val(entity.PhoneNo1);
        cboReferrerGroupCtl.SetValue(entity.GCReferrerGroup);
        $('#<%=txtZipCodeCtl.ClientID %>').val(entity.ZipCode);
        $('#<%=txtCountyCtl.ClientID %>').val(entity.County);
        $('#<%=txtDistrictCtl.ClientID %>').val(entity.District);
        $('#<%=txtCityCtl.ClientID %>').val(entity.City);
        $('#<%=txtAddressCtl.ClientID %>').val(entity.StreetName);
        $('#<%=txtProvinceCodeCtl.ClientID %>').val(entity.GCState.split('^')[1]);
        $('#<%=txtProvinceNameCtl.ClientID %>').val(entity.ProvinceName);
        $('#<%=hdnZipCodeCtl.ClientID %>').val(entity.ZipCodeId);
        $('#containerReferrerEntryDataCtl').show();
    });

    $('#btnReferrerCancelCtl').die('click');
    $('#btnReferrerCancelCtl').live('click', function () {
        $('#containerReferrerEntryDataCtl').hide();
    });

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                cbpView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerReferrerEntryDataCtl').hide();
            }
        }
        else if (param[0] == 'delete') {
        if (param[1] == 'fail') {
            showToast('Delete Failed', 'Error Message : ' + param[2]);
            cbpView.PerformCallback('refresh');
        }
        else {
            var pageCount = parseInt(param[2]);
            setPagingDetailItem(pageCount);
        }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    $('#btnReferrerSaveCtl').click(function (evt) {
        if (IsValid(evt, 'fsReferrer', 'mpReferrer'))
            cbpView.PerformCallback('save');
        return false;
    });

    //#region Zip Code
    $('#lblZipCode.lblLink').click(function () {
        openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
            onTxtZipCodeChanged(value);
        });
    });

    $('#<%=txtZipCodeCtl.ClientID %>').change(function () {
        onTxtZipCodeChangedValue($(this).val());
    });

    function onTxtZipCodeChanged(value) {
        if (value != '') {
            var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeCtl.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeCtl.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityCtl.ClientID %>').val(result.City);
                    $('#<%=txtCountyCtl.ClientID %>').val(result.County);
                    $('#<%=txtDistrictCtl.ClientID %>').val(result.District);
                    $('#<%=txtProvinceCodeCtl.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceNameCtl.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeCtl.ClientID %>').val('');
                    $('#<%=txtZipCodeCtl.ClientID %>').val('');
                    $('#<%=txtCityCtl.ClientID %>').val('');
                    $('#<%=txtCountyCtl.ClientID %>').val('');
                    $('#<%=txtDistrictCtl.ClientID %>').val('');
                    $('#<%=txtProvinceCodeCtl.ClientID %>').val('');
                    $('#<%=txtProvinceNameCtl.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeCtl.ClientID %>').val('');
            $('#<%=txtZipCodeCtl.ClientID %>').val('');
            $('#<%=txtCityCtl.ClientID %>').val('');
            $('#<%=txtCountyCtl.ClientID %>').val('');
            $('#<%=txtDistrictCtl.ClientID %>').val('');
            $('#<%=txtProvinceCodeCtl.ClientID %>').val('');
            $('#<%=txtProvinceNameCtl.ClientID %>').val('');
        }
    }

    function onTxtZipCodeChangedValue(value) {
        if (value != '') {
            var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeCtl.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeCtl.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityCtl.ClientID %>').val(result.City);
                    $('#<%=txtCountyCtl.ClientID %>').val(result.County);
                    $('#<%=txtDistrictCtl.ClientID %>').val(result.District);
                    $('#<%=txtProvinceCodeCtl.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceNameCtl.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeCtl.ClientID %>').val('');
                    $('#<%=txtZipCodeCtl.ClientID %>').val('');
                    $('#<%=txtCityCtl.ClientID %>').val('');
                    $('#<%=txtCountyCtl.ClientID %>').val('');
                    $('#<%=txtDistrictCtl.ClientID %>').val('');
                    $('#<%=txtProvinceCodeCtl.ClientID %>').val('');
                    $('#<%=txtProvinceNameCtl.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeCtl.ClientID %>').val('');
            $('#<%=txtZipCodeCtl.ClientID %>').val('');
            $('#<%=txtCityCtl.ClientID %>').val('');
            $('#<%=txtCountyCtl.ClientID %>').val('');
            $('#<%=txtDistrictCtl.ClientID %>').val('');
            $('#<%=txtProvinceCodeCtl.ClientID %>').val('');
            $('#<%=txtProvinceNameCtl.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Province
    function onGetSCProvinceFilterExpression() {
        var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
        return filterExpression;
    }

    $('#lblProvince.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
            $('#<%=txtProvinceCodeCtl.ClientID %>').val(value);
            onTxtProvinceCodeChanged(value);
        });
    });

    $('#<%=txtProvinceCodeCtl.ClientID %>').change(function () {
        onTxtProvinceCodeChanged($(this).val());
    });

    function onTxtProvinceCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtProvinceNameCtl.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtProvinceCodeCtl.ClientID %>').val('');
                $('#<%=txtProvinceNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Kode Provider Rujukan Inhealth
    var isBridging = $('#<%=hdnIsBridgingToInhealth.ClientID %>').val();
    if (isBridging == "1") {
        $('#<%:trInhealthCodeReferrer.ClientID %>').removeAttr('style');
    }
    else {
        $('#<%:trInhealthCodeReferrer.ClientID %>').attr('style', 'display:none');
    }

    $('#lblInhealthProviderRujukan.lblLink').click(function () {
        openSearchDialog('vinhealthreferenceproviderrujukan', '', function (value) {
            $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').val(value);
            onTxtProviderRujukanCodeChanged(value);
        });
    });

    $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').change(function () {
        onTxtProviderRujukanCodeChanged($(this).val());
    });

    function onTxtProviderRujukanCodeChanged(value) {
        var filterExpression = "ObjectCode = '" + value + "'";
        Methods.getObject('GetvInhealthReferenceProviderRujukanList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').val(result.ObjectCode);
                $('#<%=txtNamaInhealthProviderRujukan.ClientID %>').val(result.ObjectName);
                $('#<%=txtReferrerNameCtl.ClientID %>').val(result.ObjectName);
            }
            else {
                $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').val('');
                $('#<%=txtNamaInhealthProviderRujukan.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncboReferrerSearchCodeValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        cbpView.PerformCallback('refresh');
    }

</script>
<div style="height: 440px; overflow-y: auto">
    <div class="pageTitle"><%=GetLabel("Rujukan")%></div>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containerReferrerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnIDCtl" runat="server" value="" />
                    <fieldset id="fsReferrer" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 50%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="2">
                                    <h4>
                                        <%=GetLabel("Data Rujukan")%></h4>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="lblReferrerCode">
                                                    <%=GetLabel("Kode Perujuk")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReferrerCodeCtl" Width="50%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trInhealthCodeReferrer" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblInhealthProviderRujukan">
                                                    <%=GetLabel("Rujukan Inhealth")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" runat="server" id="hdnIsBridgingToInhealth" value="" />
                                                <asp:TextBox ID="txtKodeInhealthProviderRujukan" Width="30%" ReadOnly="false" runat="server" />
                                                <asp:TextBox ID="txtNamaInhealthProviderRujukan" Width="65%" ReadOnly="false" runat="server" />  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="lblKodeExternal">
                                                    <%=GetLabel("Kode External")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtKodeExternalCtl" Width="50%" ReadOnly="false" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Nama Perujuk")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReferrerNameCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Nama Singkat")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShortNameReferrerCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Contact Person")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactReferrerCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Kelompok Perujuk")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboReferrerGroupCtl" ClientInstanceName="cboReferrerGroupCtl"
                                                    Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("No Telepon")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelephoneNoCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jalan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddressCtl" Width="100%" runat="server" TextMode="MultiLine"
                                                    Rows="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblZipCode">
                                                    <%=GetLabel("Kode Pos")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" runat="server" id="hdnZipCodeCtl" value="" />
                                                <asp:TextBox ID="txtZipCodeCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kelurahan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountyCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kecamatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDistrictCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kota")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCityCtl" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblProvince">
                                                    <%=GetLabel("Provinsi")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceCodeCtl" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceNameCtl" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnReferrerSaveCtl" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnReferrerCancelCtl" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            <table>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Kelompok Perujuk")%>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboReferrerSearchCode" ClientInstanceName="cboReferrerSearchCode"
                                Width="220%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { oncboReferrerSearchCodeValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <img class="imgLink imgEdit" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-right: 2px;" />
                                                    <img class="imgLink imgDelete" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerCode") %>" bindingfield="BusinessPartnerCode" />
                                                    <input type="hidden" value="<%#:Eval("CommCode") %>" bindingfield="CommCode" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerName") %>" bindingfield="BusinessPartnerName" />
                                                    <input type="hidden" value="<%#:Eval("ShortName") %>" bindingfield="ShortName" />
                                                    <input type="hidden" value="<%#:Eval("ContactPerson") %>" bindingfield="ContactPerson" />
                                                    <input type="hidden" value="<%#:Eval("GCReferrerGroup") %>" bindingfield="GCReferrerGroup" />
                                                    <input type="hidden" value="<%#:Eval("StreetName") %>" bindingfield="StreetName" />
                                                    <input type="hidden" value="<%#:Eval("District") %>" bindingfield="District" />
                                                    <input type="hidden" value="<%#:Eval("City") %>" bindingfield="City" />
                                                    <input type="hidden" value="<%#:Eval("County") %>" bindingfield="County" />
                                                    <input type="hidden" value="<%#:Eval("GCState") %>" bindingfield="GCState" />
                                                    <input type="hidden" value="<%#:Eval("PhoneNo1") %>" bindingfield="PhoneNo1" />
                                                    <input type="hidden" value="<%#:Eval("ZipCode") %>" bindingfield="ZipCode" />
                                                    <input type="hidden" value="<%#:Eval("ZipCodeId") %>" bindingfield="ZipCodeId" />
                                                    <input type="hidden" value="<%#:Eval("ProvinceName") %>" bindingfield="ProvinceName" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="20%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kode Perujuk")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("BusinessPartnerCode")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="70%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Nama Perujuk")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("BusinessPartnerName")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblReferalAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
