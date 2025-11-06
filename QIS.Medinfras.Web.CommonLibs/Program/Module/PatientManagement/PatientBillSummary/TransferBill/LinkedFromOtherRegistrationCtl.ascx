<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkedFromOtherRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.LinkedFromOtherRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_linkedfromotherregistrationctl">

    //#region From Registration No
    function getFromRegistrationNoFilterExpression() {
        var filterExpression = "RegistrationID != " + $('#<%:hdnRegistrationID.ClientID %>').val();

        filterExpression += " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.OPEN + "','" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";

        return filterExpression;
    }

    $('#<%:lblFromRegistrationNo.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('registrationlinkedto', getFromRegistrationNoFilterExpression(), function (value) {
            $('#<%:txtLinkedFromRegistrationNo.ClientID %>').val(value);
            ontxtLinkedFromRegistrationNoChanged(value);
        });
    });

    $('#<%:txtLinkedFromRegistrationNo.ClientID %>').live('change', function () {
        ontxtLinkedFromRegistrationNoChanged($(this).val());
    });

    function ontxtLinkedFromRegistrationNoChanged(value) {
        var filterExpression = getFromRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
        Methods.getObject('GetvRegistration10List', filterExpression, function (result) {
            if (result != null) {
                var oRegistrationID = result.RegistrationID;
                var oRegistrationNo = result.RegistrationNo;
                var oPatient = "(" + result.MedicalNo + ") " + result.PatientName;
                var oServiceUnit = result.DepartmentID + " || " + result.ServiceUnitName;
                var oParamedic = result.ParamedicName;
                var oBusinessPartner = result.BusinessPartnerName;
                var oNoSEP = result.NoSEP;
                var mrn = $('#<%:hdnMRN.ClientID %>').val();
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();

                $('#<%:hdnLinkedFromRegistrationID.ClientID %>').val(oRegistrationID);
                $('#<%:txtLinkedFromRegistrationNo.ClientID %>').val(oRegistrationNo);
                $('#<%:txtLinkedFromPatientCtl.ClientID %>').val(oPatient);
                $('#<%:txtLinkedFromServiceUnitCtl.ClientID %>').val(oServiceUnit);
                $('#<%:txtLinkedFromParamedicCtl.ClientID %>').val(oParamedic);
                $('#<%:txtLinkedFromBusinessPartnerCtl.ClientID %>').val(oBusinessPartner);
                $('#<%:txtLinkedFromSEPNoCtl.ClientID %>').val(oNoSEP);
            }
            else {
                $('#<%:hdnLinkedFromRegistrationID.ClientID %>').val('');
                $('#<%:txtLinkedFromRegistrationNo.ClientID %>').val('');
                $('#<%:txtLinkedFromPatientCtl.ClientID %>').val('');
                $('#<%:txtLinkedFromServiceUnitCtl.ClientID %>').val('');
                $('#<%:txtLinkedFromParamedicCtl.ClientID %>').val('');
                $('#<%:txtLinkedFromBusinessPartnerCtl.ClientID %>').val('');
                $('#<%:txtLinkedFromSEPNoCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%:hdnLinkedFromRegistrationID.ClientID %>').val('');
        $('#<%:txtLinkedFromRegistrationNo.ClientID %>').val('');
        $('#<%:txtLinkedFromPatientCtl.ClientID %>').val('');
        $('#<%:txtLinkedFromServiceUnitCtl.ClientID %>').val('');
        $('#<%:txtLinkedFromParamedicCtl.ClientID %>').val('');
        $('#<%:txtLinkedFromBusinessPartnerCtl.ClientID %>').val('');
        $('#<%:txtLinkedFromSEPNoCtl.ClientID %>').val('');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        if (confirm("Anda yakin akan menghapus link dari registrasi nomor " + entity.LinkedFromRegistrationNo + "?")) {
            $('#<%=hdnLinkedFromRegistrationID.ClientID %>').val(entity.LinkedFromRegistrationID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 7);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblEntryPopupAddData').click();
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
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
            $('.grdPopup tr:eq(1)').click();
        }

        $('#containerPopupEntryData').hide();
        hideLoadingPanel();
        addItemFilterRow();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion
    
</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden;">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnMRN" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 80%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNoCtl" ReadOnly="true" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter / Tenaga Medis")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtParamedicCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Penjamin Bayar")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBusinessPartnerCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. SEP")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSEPNoCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="2">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 180px" />
                                            <col />
                                        </colgroup>
                                        <tr id="trAdmissionRegistrationNo" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblMandatory lblLink" id="lblFromRegistrationNo" runat="server">
                                                    <%:GetLabel("No Registrasi Asal")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnLinkedFromRegistrationID" value="" runat="server" />
                                                <asp:TextBox ID="txtLinkedFromRegistrationNo" Width="200px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Pasien Asal")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLinkedFromPatientCtl" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Unit Pelayanan Asal")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLinkedFromServiceUnitCtl" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Dokter / Tenaga Medis Asal")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLinkedFromParamedicCtl" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Penjamin Bayar Asal")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLinkedFromBusinessPartnerCtl" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No. SEP Asal")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLinkedFromSEPNoCtl" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%=GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%=GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewPopup').show();}"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s);}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class='imgDelete <%#: Convert.ToInt32(Eval("CountTransfer")) > 0 ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Delete")%>' src='<%# Convert.ToInt32(Eval("CountTransfer")) > 0 ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("LinkedFromRegistrationID") %>" bindingfield="LinkedFromRegistrationID" />
                                                <input type="hidden" value="<%#:Eval("LinkedFromRegistrationNo") %>" bindingfield="LinkedFromRegistrationNo" />
                                                <input type="hidden" value="<%#:Eval("CountTransfer") %>" bindingfield="CountTransfer" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderText="Registrasi"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label style="font-size: small; font-style: italic">
                                                    <b>
                                                        <%#: Eval("LinkedFromRegistrationNo") %></b></label><br />
                                                <label style="font-size: x-small">
                                                    <%#: Eval("cfLinkedFromRegistrationDateInString")%></label><br />
                                                <label style="font-size: x-small">
                                                    <%#: Eval("LinkedFromRegistrationTime")%></label><br />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderText="Unit Registrasi"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label style="font-size: x-small">
                                                    <%#: Eval("LinkedFromDepartmentID")%></label><br />
                                                <label style="font-size: small; font-style: italic">
                                                    <%#: Eval("LinkedFromServiceUnitName") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Pasien"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label style="font-size: x-small">
                                                    <%#: Eval("LinkedFromMedicalNo")%></label><br />
                                                <label style="font-size: small; font-style: italic">
                                                    <%#: Eval("LinkedFromPatientName") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
