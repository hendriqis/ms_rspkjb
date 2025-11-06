<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderTemplateOrderCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderTemplateOrderCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_testordertemplatesctl">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td></tr>");

        $input = $("<input type='text' id='txtFilterItem' style='width:100%;height:20px' />").val($('#<%=hdnFilterItem.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtFilterItem').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItem.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpViewPopup.PerformCallback('refresh');
        }
    });

    $('#tblSelectedItem .txtQty').live('change', function () {
        $row = $(this).closest('tr');
        totalPayer = 0;
        totalPatient = 0;
        grandTotal = 0;
        $('#tblSelectedItem tr').each(function () {
            if ($(this).find('.keyField').val() != undefined) {
                calculateTariffEstimation($(this));
            }
        });
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            lstSelectedMember.push(key);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopUp"), pageCount, function (page) {
            getCheckedMember();
            cbpViewPopup.PerformCallback('changepage|' + page);
        });

        //#region Template Order
        $('#lblTestTemplate.lblLink').click(function () {
            var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
            openSearchDialog('templatepanel', filterExpression, function (value) {
                $('#<%=txtTestTemplateCode.ClientID %>').val(value);
                onTxtTestTemplateCode(value);
            });
        });

        $('#<%=txtTestTemplateCode.ClientID %>').change(function () {
            onTxtTestTemplateCode($(this).val());
        });

        function onTxtTestTemplateCode(value) {
            var filterExpression = "TestTemplateCode = '" + value + "'";
            Methods.getObject('GetTestTemplateHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnTestTemplateID.ClientID %>').val(result.TestTemplateID);
                    $('#<%=txtTestTemplateName.ClientID %>').val(result.TestTemplateName);
                }
                else {
                    $('#<%=hdnTestTemplateID.ClientID %>').val('');
                    $('#<%=txtTestTemplateCode.ClientID %>').val('');
                    $('#<%=txtTestTemplateName.ClientID %>').val('');
                }
                getCheckedMember();
                cbpViewPopup.PerformCallback('refresh');
            });
        }
        //#endregion

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
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopUp"), pageCount, function (page) {
                getCheckedMember();
                cbpViewPopup.PerformCallback('changepage|' + page);
            });
        }
        else { }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $($newTr);
            $newTr.insertAfter($('#trHeader2'));
        }
        else {
            var id = $(this).closest('tr').find('.keyField').html();
            $('#tblSelectedItem tr').each(function () {
                if ($(this).find('.keyField').val() == id) {
                    $(this).remove();
                }
            });
        }
    });

    $('#tblSelectedItem .chkIsSelected2').die('change');
    $('#tblSelectedItem .chkIsSelected2').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var id = $selectedTr.find('.keyField').val();
            var isFound = false;
            $('#<%=grdView.ClientID %> tr').each(function () {
                if (id == $(this).find('.keyField').html()) {
                    $(this).find('.chkIsSelected').find('input').prop('checked', false);
                    isFound = true;
                }
            });
            if (!isFound) {
                var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });
</script>

<div style="padding:10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
            </td>
            <td>${ItemName1}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsLabUnit" runat="server" value="0" />

    <table>
        <colgroup>
            <col style="width:150px"/>
            <col style="width:400px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblTestTemplate"><%=GetLabel("Template Order")%></label></td>
            <td>
                <input type="hidden" id="hdnTestTemplateID" value="0" runat="server" />
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:120px"/>
                        <col style="width:450px"/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtTestTemplateCode" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtTestTemplateName" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblLink" id="lblDiagnose"><%=GetLabel("Diagnosa")%></label></td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:120px"/>
                        <col style="width:450px"/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width:100%">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                        EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan yang tersedia :" ItemStyle-CssClass="tdItemName1" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopUp"></div>
                    </div>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                    <tr id="trHeader2">
                        <th style="width:40px">&nbsp;</th>
                        <th align="left"><%=GetLabel("Pelayanan yang telah dipilih :")%></th>  
                    </tr>
                </table> 
            </td>
        </tr>
    </table>
        
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div> 
</div>
