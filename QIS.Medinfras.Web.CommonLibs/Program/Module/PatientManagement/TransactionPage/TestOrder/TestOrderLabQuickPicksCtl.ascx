<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderLabQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderLabQuickPicksCtl" %>
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

        if ($('#<%=txtRemarks.ClientID %>').val() == '') {
            displayErrorMessageBox("Order Pemeriksaan", "Catatan Klinis harus disertakan untuk kebutuhan akreditasi");
            return false;
        }

        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            displayErrorMessageBox("Order Pemeriksaan", "Harus ada item pemeriksaan yang dipilih sebelum disimapan");
            return false;
        }
    }
    //#region Diagnose
    $('#lblDiagnose.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
            $('#<%=txtDiagnoseID.ClientID %>').val(value);
            onTxtDiagnoseIDChanged(value);
        });
    });

    $('#<%=txtDiagnoseID.ClientID %>').live('change', function () {
        onTxtDiagnoseIDChanged($(this).val());
    });

    function onTxtDiagnoseIDChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                if ($('#<%=txtRemarks.ClientID %>').val() == '') {
                    $('#<%=txtRemarks.ClientID %>').val(result.DiagnoseName);
                }
            }
            else {
                $('#<%=txtDiagnoseID.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function getCheckedMember() {
        ////        var lstSelectedMember = [];
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        var result = '';
        $('.chkIsSelected input:checked').each(function () {
            var key = $(this).closest('.divContainerLabItem').find('.hdnItemID').val();
            if (lstSelectedMember.indexOf(key) == -1) {
                lstSelectedMember.push(key);
            }
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    $('#lnkDetailItem').die('click');
    $('#lnkDetailItem').live('click', function () {
        var msg = $(this).parent().find('.preconditionNotes').val();
        var itemName = $(this).parent().find('.itemName').val();
        var title = "Syarat dan Kondisi Pemeriksaan (" + itemName + ")";
        displayMessageBox(title, msg);
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
    function onGetPhysicianExecutorFilterExpression() {
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID= '" + $('#<%=hdnHealthcareServiceUnitIDCTL.ClientID %>').val() + "') AND IsDeleted = 0 AND IsAvailable = 1";
        return filterExpression;
    }
    $('#lblExecutorParamedic.lblLink').die('click');
    $('#lblExecutorParamedic.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianExecutorFilterExpression(), function (value) {
            $('#<%=txtExecutorParamedicCode.ClientID %>').val(value);
            onTxtServicePhysicianCodeChanged(value);
        });
    });

    $('#<%=txtExecutorParamedicCode.ClientID %>').live('change', function () {
        onTxtServicePhysicianCodeChanged($(this).val());
    });

    function onTxtServicePhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianExecutorFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnExecutorParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtExecutorParamedicName.ClientID %>').val(result.ParamedicName);

            }
            else {
                $('#<%=hdnExecutorParamedicID.ClientID %>').val('');
                $('#<%=txtExecutorParamedicCode.ClientID %>').val('');
                $('#<%=txtExecutorParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<style type="text/css">
    div#multicolumn
    {
        -moz-column-count: 3;
        -moz-column-gap: 10px;
        -webkit-column-count: 3;
        -webkit-column-gap: 10px;
        -column-count: 3;
        -column-gap: 10px;
        width:1000px;
    }
    .redAsterik
    {
        color: Red;
    }
</style>
<div style="padding: 10px; height: 35px">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCTL" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsOnlyBPJSItem" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnIsBPJSRegistration" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnIsPATest" value="" runat="server" />
    <input type="hidden" id="hdnIsLabUnit" value="" runat="server" />
    <input type="hidden" id="hdnOrderHanyaItemPemeriksaanQPCtl" runat="server" value="" />
    <input type="hidden" id="hdnMCUItemTambahanProposeCtl" runat="server" value="0" />
    <input type="hidden" id="hdnRadiotheraphyUnitID" runat="server" value="0" />
    <input type="hidden" id="hdnGCSubItemType" runat="server" value="" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 120px" />
            <col style="width: 250px" />
            <col style="width: 10px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr id="trPhysicanExecuter" runat="server" style="display:none;">
             <td class="tdLabel">
                <label class="lblLink  lblMandatory" id="lblExecutorParamedic">
                    <%=GetLabel("Dokter Pelaksana")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnExecutorParamedicID" runat="server" value="0"/>
                <asp:TextBox ID="txtExecutorParamedicCode" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtExecutorParamedicName" ReadOnly="true" Width="100%" runat="server" />
            </td>
            
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblDiagnose">
                    <%=GetLabel("Diagnosa")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" />
            </td>
            <td />
            <td style="vertical-align:top">
                <label class="lblMandatory" id="lblRemarks">
                    <%=GetLabel("Catatan Klinis")%></label>
            </td>
            <td colspan="2" rowspan="2">
                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                    Rows="2" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td colspan="2">
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
        <tr>
            <td colspan="6">
                <div style="height: 350px; overflow-y: scroll; margin-top:5px">
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 100%" />
                        </colgroup>
                        <tr>
                            <td style="padding: 5px; vertical-align: top">
                                <div>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){;showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:Repeater ID="rptView" runat="server" OnItemDataBound="rptView_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div id="divGroupHeader" runat="server" style="text-align: center; font-size:larger"><%#Eval("ItemGroupName1") %></div>
                                                            <div id="divGroupDetail" runat="server">
                                                                <asp:DataList ID="rptDetail" runat="server" RepeatColumns="4" RepeatDirection="Vertical">
                                                                    <ItemTemplate>
                                                                        <div class="divContainerLabItem" style="width: 260px">
                                                                            <input type="hidden" class="hdnItemID" value='<%#Eval("ItemID") %>' />
                                                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                                            <%# Eval("PreconditionNotes").ToString() == "" ? 
                                                                                Eval("ItemName1") : 
                                                                                "<input class=\"preconditionNotes\" type=\"hidden\" value=\""+Eval("PreconditionNotes")+"\">"+
                                                                                "<input class=\"itemName\" type=\"hidden\" value=\""+Eval("ItemName1")+"\">"+                                                                
                                                                                "<a href=\"#\" id=\"lnkDetailItem\">"+Eval("ItemName1")+"</a>"+"<span class=\"redAsterik\">*</span>"
                                                                            %>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
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
            </td>
        </tr>
    </table>
</div>
