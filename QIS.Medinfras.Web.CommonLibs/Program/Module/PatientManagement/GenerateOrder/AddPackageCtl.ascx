<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPackageCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AddPackageCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitctl">
    $('#lblPatientVisitAddData').die('click');
    $('#lblPatientVisitAddData').live('click', function () {
        $('#containerMCUPackage').show();
    });

    $('#ulTabPackageMCU li').click(function () {
        $('#ulTabPackageMCU li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
        lastContentID = $contentID;
    });

    function getItemMasterCtlFilterExpression() {
        var filterExpression = "ItemID IN (SELECT ItemID FROM vItemService WHERE GCItemType = 'X001^007' AND IsPackageItem = 1) AND IsDeleted = 0";
        filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM vConsultVisitItemPackage WHERE isDeleted = 0 AND RegistrationID = " + $('#<%=hdnRegistrationID.ClientID %>').val() + ")";
        return filterExpression;
    }

    $('#btnSaveCtl').click(function (evt) {

        if ($('#<%=txtItemCode.ClientID %>').val() == '' && $('#<%=txtItemName.ClientID %>').val() == '') {
            showToast('Warning', 'Isi Paket MCU Terlebih Dahulu');
        }
        else {
            cbpAddMCUPackage.PerformCallback('save');
            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName.ClientID %>').val('');
        }
    });

    $('#btnCancelCtl').die('click');
    $('#btnCancelCtl').live('click', function () {
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#containerMCUPackage').hide();
    });

    $('.imgPatientVisitDelete.imgLink').die('click');
    $('.imgPatientVisitDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                var id = $row.find('.ID').val();
                $('#<%=hdnID.ClientID %>').val(id);
                cbpAddMCUPackage.PerformCallback('delete');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
            }
        });
    });

    function onCbpPatientVisitTransHdEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
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
        $('#containerMCUPackage').hide();
        hideLoadingPanel();
    }

    $('#<%=lblItemMCU.ClientID %>.lblLink').live('click', function () {
        var filterExpression = getItemMasterCtlFilterExpression();
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemMasterCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemMasterCodeChanged($(this).val());
    });

    function onTxtItemMasterCodeChanged(value) {
        var filterExpression = getItemMasterCtlFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
            }
            else 
            {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
            }
        });
    }
    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');

    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdPatientVisitTransHd.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpAddMCUPackage.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnDepartmentIDCtlPckg" />
    <input type="hidden" value="" runat="server" id="hdnID" />
    <div class="pageTitle">
        <%=GetLabel("Kunjungan Pasien")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 110px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Registrasi")%>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRegistrationNo" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="containerUlTabPage">
                    <ul class="ulTabPage" id="ulTabPackageMCU">
                        <li class="selected" contentid="containerStandardPackage">
                            <%=GetLabel("Standard Paket") %></li>
                        <%--<li contentid="containerItemPackage">
                            <%=GetLabel("Tambah Item") %></li>--%>
                    </ul>
                </div>
                <div id="containerStandardPackage" class="containerTransDt">
                    <div id="containerMCUPackage" style="margin-top: 10px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Entry Paket MCU")%></div>
                        <fieldset id="fsPatientVisit" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 110px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" runat="server" id="lblItemMCU">
                                            <%:GetLabel("Paket MCU")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnItemID" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtItemName" Width="90%" ReadOnly="true" runat="server" />
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
                                                    <input type="button" id="btnSaveCtl" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnCancelCtl" value='<%= GetLabel("Batal")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpAddMCUPackage" runat="server" Width="100%" ClientInstanceName="cbpAddMCUPackage"
                        ShowLoadingPanel="false" OnCallback="cbpAddMCUPackage_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientVisitTransHdEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                    margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdPatientVisitTransHd" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class='imgPatientVisitDelete <%#: Eval("IsMainPackage").ToString() == "True" || IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>'
                                                        title='<%=GetLabel("Delete")%>' src='<%# Eval("IsMainPackage").ToString() == "True" || IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kode Paket")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="divItemCode">
                                                        <%#: Eval("ItemCode")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="300px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Nama Paket")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="divItemName1">
                                                        <%#: Eval("ItemName1")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
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
                    <div style="text-align: center" id="divContainerAddData" runat="server">
                        <span class="lblLink" id="lblPatientVisitAddData">
                            <%= GetLabel("Add Data")%></span>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right;">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
