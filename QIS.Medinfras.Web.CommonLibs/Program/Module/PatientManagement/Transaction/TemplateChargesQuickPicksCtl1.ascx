<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateChargesQuickPicksCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplateChargesQuickPicksCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_testorderquickpicksctl">
    $(function () {
        hideLoadingPanel();
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != "") {
            if ($('#<%=hdnParamedicID.ClientID %>').val() != "") {
                return true;
            } else {
                errMessage.text = 'Pilih Dokter/Paramedis terlebih dahulu.';
                return false;
            }
        }
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
    }

    //#region Template
    $('#lblTemplate.lblLink').live('click', function () {
        var filter = "IsDeleted = 0 AND HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "'";
        openSearchDialog('chargestemplatehd', filter, function (value) {
            $('#<%=txtTemplateCode.ClientID %>').val(value);
            onTxtTemplateCodeChanged(value);
        });
    });

    $('#<%=txtTemplateCode.ClientID %>').live('change', function () {
        onTxtTemplateCodeChanged($(this).val());
    });

    function onTxtTemplateCodeChanged(value) {
        var filterExpression = "ChargesTemplateCode = '" + value + "' AND IsDeleted = 0 AND HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "'";
        Methods.getObject('GetvChargesTemplateHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnChargesTemplateID.ClientID %>').val(result.ChargesTemplateID);
                $('#<%=txtTemplateName.ClientID %>').val(result.ChargesTemplateName);
                cbpViewPopup.PerformCallback('refresh');
            }
            else {
                $('#<%=hdnChargesTemplateID.ClientID %>').val('');
                $('#<%=txtTemplateCode.ClientID %>').val('');
                $('#<%=txtTemplateName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function getCheckedMember() {
        ////        var lstSelectedMember = [];
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        var result = '';
        $('.chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                var key = $(this).closest('.divContainerLabItem').find('.hdnItemID').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                    result = true;
                }
            }
            else {
                var key = $(this).closest('.divContainerLabItem').find('.hdnItemID').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx > -1) {
                    lstSelectedMember.splice(idx, 1);
                }
            }
        });

            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    $('#lnkDetailItem').die('click');
    $('#lnkDetailItem').live('click', function () {
        var msg = $(this).parent().find('.preconditionNotes').val();
        showToast('Kondisi sebelum test dilakukan', msg);
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshControl();
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onRefreshControl(filterExpression) {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        getCheckedMember();
        cbpViewPopup.PerformCallback('refresh');
    }

    //#region Physician
    $('#lblQuickPicksPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtParamedicCode.ClientID %>').val(value);
            onTxtQuickPicksPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
        onTxtQuickPicksPhysicianCodeChanged($(this).val());
    });

    function onTxtQuickPicksPhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<div style="padding: 10px; height: 450px; overflow-x: hidden; overflow-y: scroll">
    <input type="hidden" id="hdnChargesTemplateID" runat="server" value="0" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsAccompany" runat="server" value="" />
    <input type="hidden" id="hdnIsOnlyBPJS" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnIsBPJSRegistration" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnIsOnlyBPJSItem" runat="server" value="" />
    <input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnListItemBefore" runat="server" value="0" />
    <style type="text/css">
        div#multicolumn
        {
            -moz-column-count: 3;
            -moz-column-gap: 10px;
            -webkit-column-count: 3;
            -webkit-column-gap: 10px;
            column-count: 3;
            column-gap: 10px;
        }
        .redAsterik
        {
            color: Red;
        }
    </style>
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 500px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblTemplate">
                    <%=GetLabel("Template")%></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 3px" />
                        <col style="width: 250px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblQuickPicksPhysician">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnParamedicID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 3px" />
                        <col style="width: 600px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtParamedicName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td>
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="100%" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                        <qis:QISIntellisenseHint Text="Kode Item" FieldName="ItemCode" />
                    </IntellisenseHints>
                </qis:QISIntellisenseTextBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="multicolumn">
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){;showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:Repeater ID="rptView" runat="server" OnItemDataBound="rptView_ItemDataBound">
                                        <ItemTemplate>
                                            <h4>
                                                <%#Eval("StandardCodeName") %></h4>
                                            <asp:Repeater ID="rptDetail" OnItemDataBound="rptDetail_ItemDataBound" runat="server">
                                                <ItemTemplate>
                                                    <div class="divContainerLabItem">
                                                        <input type="hidden" class="hdnItemID" value='<%#Eval("ItemID") %>' />
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <%# Eval("ItemName1")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
