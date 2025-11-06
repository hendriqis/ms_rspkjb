<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingProblemQuickPicksCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingProblemQuickPicksCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<style type="text/css">
    .trSelectedItem {background-color: #ecf0f1 !important;}
</style>

<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
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
            cbpPopup.PerformCallback('refresh');
        }
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
        setDatePicker('<%=txtProblemDate.ClientID %>');
        $('#<%=txtProblemDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtProblemDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtProblemDate.ClientID %>').val(getDateNowDatePickerFormat());
    });

    function onBeforeSaveRecordEntryPopup() {
        //        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
                displayErrorMessageBox("Masalah Keperawatan", "Belum ada item masalah keperawatan yang dipilih");
                return false;
        }
        //        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberName = [];
        var lstSelectedMemberRemarks = [];
        var lstSelectedMemberIsCITO = [];

        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {

            var key = $(this).find('.keyField').val();
            var name = $(this).find('.ItemName').html();

            lstSelectedMember.push(key);
            lstSelectedMemberName.push(name);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberName.ClientID %>').val(lstSelectedMemberName.join('^'));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var defaultStartDate = $('#<%=txtProblemDate.ClientID %>').val();
            var defaultStartTime = '08:00';

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ProblemName}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ProblemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $($newTr);
            $newTr.insertBefore($('#trFooter'));
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

    $('#<%=rblItemSource.ClientID %> input').change(function () {
        getCheckedMember();
        cbpPopup.PerformCallback('refresh');
    });

    function onAfterSaveRecordPatientPageEntry(value) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

</script>

<div style="padding:10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ProblemID}' />
            </td>
            <td class="ItemName">${ProblemName}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberName" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRemarks" runat="server" value="" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:40%"/>
            <col style="width:60%"/>
        </colgroup>
        <tr>
            <td style="padding:2px;vertical-align:top">
                <h4><%=GetLabel("Masalah Keperawatan:")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width:120px"/>
                        <col style="width:100px"/>
                        <col style="width:250px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="lblItemSource"><%=GetLabel("Pilih dari")%></label></td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblItemSource" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Master" Value="1" Selected="True" />
                                <asp:ListItem Text="Hasil Pengkajian" Value="2"  />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                                ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                                    EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdView_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ProblemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="cfProblemName" HeaderText="Masalah Keperawatan" ItemStyle-CssClass="tdItemName1" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada data masalah keperawatan")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingPopup"></div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding:2px;vertical-align:top">
                <h4><%=GetLabel("Masalah Keperawatan : Yang dipilih")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Pengkajian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProblemDate" runat="server" Width="120px" CssClass="datepicker" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProblemTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" />
                         </td>
                    </tr>
                </table>
                <div style="height:400px; overflow-y:scroll;padding-top:5px">
                    <fieldset id="fsDrugsQuickPicks">
                        <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                            <tr id="trHeader2">
                                <th style="width: 20px">
                                    &nbsp;
                                </th>
                                <th align="left">
                                    <%=GetLabel("Masalah Keperawatan")%>
                                </th>
                            </tr>
                            <tr id="trFooter">
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
        
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div> 
</div>