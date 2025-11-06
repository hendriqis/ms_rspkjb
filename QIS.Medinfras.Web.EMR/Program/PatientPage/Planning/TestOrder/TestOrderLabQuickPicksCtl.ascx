<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderLabQuickPicksCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.TestOrderLabQuickPicksCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_testorderquickpicksctl">
    $(function () {
        hideLoadingPanel();
    });

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
            }
            else {
                $('#<%=txtDiagnoseID.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var result = '';
        $('.chkIsSelected input:checked').each(function () {
            var key = $(this).closest('.divContainerLabItem').find('.hdnItemID').val();
            lstSelectedMember.push(key);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    function onBeforeSaveRecordEntryPopup() {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '' && $('#<%=txtRemarks.ClientID %>').val() != '')
            return true;
        else {
            if ($('#<%=txtRemarks.ClientID %>').val() == '') {
                showToast("Order Pemeriksaan","Catatan Klinis harus disertakan untuk kebutuhan akreditasi");
                return false;
            }
            else {
                showToast("Order Pemeriksaan", "Belum ada item pemeriksaan yang dipilih");
                return false;
            }
        }
    }
   
    $('#lnkDetailItem').die('click');
    $('#lnkDetailItem').live('click', function () {
        var msg = $(this).parent().find('.preconditionNotes').val();
        showToast('Kondisi sebelum test dilakukan', msg);
    });
</script>

<div style="padding:10px; height: 450px; overflow-x:hidden; overflow-y:scroll">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" value="" id="hdnTestOrderEntryQuickPicksTestOrderID" />

    <style type="text/css">
        div#multicolumn {
            -moz-column-count: 3;
            -moz-column-gap: 10px;
            -webkit-column-count: 3;
            -webkit-column-gap: 10px;
            column-count: 3;
            column-gap: 10px;
        }
        
        #lnkDetailItem
        {            
            color:Black;
            font-weight:bold;
            text-decoration:none;
        }
        
        .redAsterik
        {            
            color:Red;
        }
    </style>

    <table>
        <colgroup>
            <col style="width:150px"/>
            <col style="width:400px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblLink" id="lblDiagnose"><%=GetLabel("Diagnosis")%></label></td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:100px"/>
                        <col style="width:3px"/>
                        <col style="width:300px"/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td><asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                <%=GetLabel("Catatan Klinis") %>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                    Rows="2" />
            </td>
        </tr>
    </table>
    <table style="width:100%">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <div id="multicolumn">
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                            EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:Repeater ID="rptView" runat="server" OnItemDataBound="rptView_ItemDataBound">
                                        <ItemTemplate>
                                            <h4><%#Eval("ItemGroupName1") %></h4>
                                            <asp:Repeater ID="rptDetail" runat="server">
                                                <ItemTemplate>
                                                    <div class="divContainerLabItem">
                                                        <input type="hidden" class="hdnItemID" value='<%#Eval("ItemID") %>' />
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <%# Eval("PreconditionNotes").ToString() == "" ? 
                                                                Eval("ItemName1") : 
                                                                "<input class=\"preconditionNotes\" type=\"hidden\" value=\""+Eval("PreconditionNotes")+"\">"+
                                                                "<a href=\"#\" id=\"lnkDetailItem\">"+Eval("ItemName1")+"</a>"+"<span class=\"redAsterik\">*</span>"
                                                        %>
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
