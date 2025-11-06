<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportParameterEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ReportParameterEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#lblFilterParameter').attr('class', 'lblLink lblMandatory');
        $('#<%=txtFilterParameterCode.ClientID %>').removeAttr('readonly');

        $('#<%=hdnFilterParameterID.ClientID %>').val('');
        $('#<%=txtFilterParameterCode.ClientID %>').val('');
        $('#<%=txtFilterParameterName.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('');

        $('#<%=hdnIsAdd.ClientID %>').val('1');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var filterParameterID = $row.find('.hdnFilterParameterID').val();
            $('#<%=hdnFilterParameterID.ClientID %>').val(filterParameterID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#lblFilterParameter').attr('class', 'lblDisabled');
        $('#<%=txtFilterParameterCode.ClientID %>').attr('readonly', 'readonly');

        $row = $(this).closest('tr');
        var filterParameterID = $row.find('.hdnFilterParameterID').val();
        var filterParameterCode = $row.find('.hdnFilterParameterCode').val();
        var filterParameterName = $row.find('.tdFilterParameterName').html();
        var displayOrder = $row.find('.tdDisplayOrder').html();
        
        $('#<%=hdnFilterParameterID.ClientID %>').val(filterParameterID);
        $('#<%=txtFilterParameterCode.ClientID %>').val(filterParameterCode);
        $('#<%=txtFilterParameterName.ClientID %>').val(filterParameterName);
        $('#<%=txtDisplayOrder.ClientID %>').val(displayOrder);

        $('#<%=hdnIsAdd.ClientID %>').val('0');

        $('#containerPopupEntryData').show();
    });

    //#region Filter Parameter
    function getFilterParameterFilterExpression() {
        var filterExpression = 'FilterParameterID NOT IN (SELECT FilterParameterID FROM ReportParameter WHERE ReportID = ' + $('#<%=hdnReportID.ClientID %>').val() + ') AND IsDeleted = 0';
        return filterExpression;
    }

    $('#lblFilterParameter.lblLink').die('click');
    $('#lblFilterParameter.lblLink').live('click', function () {
        openSearchDialog('filterparameter', getFilterParameterFilterExpression(), function (value) {
            $('#<%=txtFilterParameterCode.ClientID %>').val(value);
            onTxtFilterParameterCodeChanged(value);
        });
    });

    $('#<%=txtFilterParameterCode.ClientID %>').change(function () {
        onTxtFilterParameterCodeChanged($(this).val());
    });

    function onTxtFilterParameterCodeChanged(value) {
        var filterExpression = getFilterParameterFilterExpression() + " AND FilterParameterCode = '" + value + "'";
        Methods.getObject('GetFilterParameterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnFilterParameterID.ClientID %>').val(result.FilterParameterID);
                $('#<%=txtFilterParameterName.ClientID %>').val(result.FilterParameterName);
            }
            else {
                $('#<%=hdnFilterParameterID.ClientID %>').val('');
                $('#<%=txtFilterParameterName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnReportID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Report Parameter")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Report")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHeaderText" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr> 
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnIsAdd" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblFilterParameter"><%=GetLabel("Filter Parameter")%></label></td>
                                <td>
                                    <input type="hidden" value="" id="hdnFilterParameterID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtFilterParameterCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtFilterParameterName" ReadOnly="true" CssClass="required" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Display Order")%></label></td>
                                <td><asp:TextBox ID="txtDisplayOrder" runat="server" Width="100px" CssClass="required number" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
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
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" class="hdnFilterParameterID" value="<%#: Eval("FilterParameterID")%>" />
                                                <input type="hidden" class="hdnFilterParameterCode" value="<%#: Eval("FilterParameterCode")%>" />
                                                <input type="hidden" class="hdnDisplayOrder" value="<%#: Eval("DisplayOrder")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FilterParameterName" HeaderText="Filter Parameter" ItemStyle-CssClass="tdFilterParameterName" />
                                        <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" ItemStyle-CssClass="tdDisplayOrder" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

