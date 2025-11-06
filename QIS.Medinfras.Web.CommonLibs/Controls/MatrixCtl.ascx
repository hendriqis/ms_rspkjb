<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MatrixCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MatrixCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunititementryctl">
    function addMatrixAvailableFilterRow(){
        $trHeader = $('#<%=grdAvailable.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td class='keyField'></td><td></td></tr>");
        $input = $("<input type='text' id='txtMatrixFilterAvailable' style='width:100%' />").val($('#<%=hdnAvailableSearchText.ClientID %>').val());
        $trFilter.find('td').last().append($input);
        $trFilter.insertAfter($trHeader);
    }

    function addMatrixSelectedFilterRow(){
        $trHeader = $('#<%=grdSelected.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td class='keyField'></td><td></td></tr>");
        $input = $("<input type='text' id='txtMatrixFilterSelected' style='width:100%' />").val($('#<%=hdnSelectedSearchText.ClientID %>').val());
        $trFilter.find('td').last().append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#chkSelectAvailable').die('change');
    $('#chkSelectAvailable').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkAvailable').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $('#chkSelectSelected').die('change');
    $('#chkSelectSelected').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $('#txtMatrixFilterSelected').die('keypress');
    $('#txtMatrixFilterSelected').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            $('#<%=hdnSelectedSearchText.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpMatrixSelected.PerformCallback('refresh');
        }
    });

    $('#txtMatrixFilterAvailable').die('keypress');
    $('#txtMatrixFilterAvailable').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            $('#<%=hdnAvailableSearchText.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpMatrixAvailable.PerformCallback('refresh');
        }
    });

    $(function () {
        $('#btnMatrixMoveRightAll').click(function () {
            cbpMatrixProcess.PerformCallback('rightAll');
        });
        $('#btnMatrixMoveRight').click(function () {
            cbpMatrixProcess.PerformCallback('right|' + getCheckedAvailable());
        });
        $('#btnMatrixMoveLeft').click(function () {
            cbpMatrixProcess.PerformCallback('left|' + getCheckedSelected());
        });
        $('#btnMatrixMoveLeftAll').click(function () {
            cbpMatrixProcess.PerformCallback('leftAll');
        });
        $('#btnSaveMatrix').click(function () {
            cbpMatrixProcess.PerformCallback('save');
        });

        if ($('#<%=lblHeader2.ClientID %>').html() == '') {
            $('#trMatrixHeader2').remove();
        }

        addMatrixAvailableFilterRow();
        addMatrixSelectedFilterRow();
    });

    function onCbpMatrixProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|')
        if(param[0] == 'save'){
            if (param[1] == 'success') {
                var type = $('#<%=hdnParam.ClientID %>').val().split('|')[0];
                if (typeof onAfterSaveMatrixCtl == 'function')
                    onAfterSaveMatrixCtl(type);
                pcRightPanelContent.Hide();
            }
            else
                showToast('Save Failed', 'Error Message : ' + param[2]);
        }
        else{
            cbpMatrixAvailable.PerformCallback('refresh');
            cbpMatrixSelected.PerformCallback('refresh');
        }
    }

    //#region Paging Available
    var pageCountAvailable = parseInt('<%=PageCountAvailable %>');
    function getCheckedAvailable(){
        var result = '';
        $('#<%=grdAvailable.ClientID %> .chkAvailable input:checked').each(function () {
            if(result != '')
                result += ';';
            result += $(this).closest('tr').find('.keyField').html();
        });
        return result;
    }

    $(function () {
        setPaging($("#pagingMatrixAvailable"), pageCountAvailable, function(page){
            cbpMatrixAvailable.PerformCallback('changepage|' + page + '|' + getCheckedAvailable());
        }, 8);
    });

    function onCbpMatrixAvailableEndCallback(s) {
        $('#containerImgLoadingMatrixAvailable').hide();

        var param = s.cpResult.split('|');
        if(param[0] == 'refresh'){
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdAvailable.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingMatrixAvailable"), pageCount, function(page){
                cbpMatrixAvailable.PerformCallback('changepage|' + page + '|' + getCheckedAvailable());
            }, 8);
            $('#<%=hdnCheckedAvailable.ClientID %>').val('');
        }
        else{
            $('#<%=hdnCheckedAvailable.ClientID %>').val(s.cpCheckedAvailable);
            $('#<%=grdAvailable.ClientID %> tr:eq(1)').click();
        }

        addMatrixAvailableFilterRow();
    }
    //#endregion

    //#region Paging Selected
    var pageCountSelected = parseInt('<%=PageCountSelected %>');
    function getCheckedSelected(){
        var result = '';
        $('#<%=grdSelected.ClientID %> .chkSelected input:checked').each(function () {
            if(result != '')
                result += ';';
            result += $(this).closest('tr').find('.keyField').html();
        });
        return result;
    }

    $(function () {
        setPaging($("#pagingMatrixSelected"), pageCountSelected, function(page){
            cbpMatrixSelected.PerformCallback('changepage|' + page + '|' + getCheckedSelected());
        }, 8);
    });

    function onCbpMatrixSelectedEndCallback(s) {
        $('#containerImgLoadingMatrixSelected').hide();

        var param = s.cpResult.split('|');
        if(param[0] == 'refresh'){
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdSelected.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingMatrixSelected"), pageCount, function(page){
                cbpMatrixSelected.PerformCallback('changepage|' + page + '|' + getCheckedSelected());
            }, 8);
            $('#<%=hdnCheckedSelected.ClientID %>').val('');
        }
        else{
            $('#<%=hdnCheckedSelected.ClientID %>').val(s.cpCheckedSelected);
            $('#<%=grdSelected.ClientID %> tr:eq(1)').click();
        }

        addMatrixSelectedFilterRow();
    }
    //#endregion
    
</script>
<input type="hidden" value="" runat="server" id="hdnCheckedAvailable" />
<input type="hidden" value="" runat="server" id="hdnCheckedSelected" />
<input type="hidden" value="" runat="server" id="hdnParam" />


<input type="hidden" value="" runat="server" id="hdnAvailableSearchText" />
<input type="hidden" value="" runat="server" id="hdnSelectedSearchText" />

<table class="tblEntryContent" style="width:70%">
    <colgroup>
        <col style="width:160px"/>
        <col/>
    </colgroup>
    <tr>
        <td class="tdLabel"><label class="lblNormal" id="lblHeader" runat="server"></label></td>
        <td colspan="2"><asp:TextBox ID="txtHeader" ReadOnly="true" Width="100%" runat="server" /></td>
    </tr>  
    <tr id="trMatrixHeader2">
        <td class="tdLabel"><label class="lblNormal" id="lblHeader2" runat="server" ></label></td>
        <td colspan="2"><asp:TextBox ID="txtHeader2" ReadOnly="true" Width="100%" runat="server" /></td>
    </tr>  
</table>

<table style="width:100%">
    <colgroup>
        <col width="45%" />
        <col />
        <col width="45%" />
    </colgroup>
    <tr>
        <td valign="top">
            <h4 style="display:none"><%=GetLabel("Available")%></h4>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpMatrixAvailable" runat="server" Width="100%" ClientInstanceName="cbpMatrixAvailable"
                    ShowLoadingPanel="false" OnCallback="cbpMatrixAvailable_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMatrixAvailable').show(); }"
                        EndCallback="function(s,e){ onCbpMatrixAvailableEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height: 272px;overflow-y:auto; ">
                                <asp:GridView ID="grdAvailable" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAvailable" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" Checked='<%# DataBinder.Eval(Container, "DataItem.IsChecked") %>' runat="server" CssClass="chkAvailable" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ID" ItemStyle-CssClass="keyField" HeaderStyle-CssClass="keyField"  />
                                        <asp:TemplateField HeaderText = "Item" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate> 
                                                <asp:Label ID="lblItemName1" Text = '<%# DataBinder.Eval(Container, "DataItem.Name") %>' ToolTip = '<%# DataBinder.Eval(Container, "DataItem.ToolTip") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingMatrixAvailable">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingMatrixAvailable"></div>
                    </div>
                </div> 
            </div>
        </td>
        <td align="center">
            <input type="button" value=">>" id="btnMatrixMoveRightAll" class="btnMatrixMoveButton w3-btn w3-green w3-hover-blue" style="width:70px"/> <br /><br />
            <input type="button" value=">" id="btnMatrixMoveRight" class="btnMatrixMoveButton w3-btn w3-green w3-hover-blue" style="width:70px;margin-bottom:2px;"/> <br />
            <input type="button" value="<" id="btnMatrixMoveLeft" class="btnMatrixMoveButton w3-btn w3-red w3-hover-blue" style="width:70px"/> <br /><br />
            <input type="button" value="<<" id="btnMatrixMoveLeftAll" class="btnMatrixMoveButton w3-btn w3-red w3-hover-blue" style="width:70px"/>
        </td>
        <td valign="top">
            <h4 style="display:none"><%=GetLabel("Selected")%></h4>
            <div style="position: relative;">
                <input type="hidden" value="" runat="server" id="Hidden1" />
                <input type="hidden" value="" runat="server" id="Hidden2" />
                <dxcp:ASPxCallbackPanel ID="cbpMatrixSelected" runat="server" Width="100%" ClientInstanceName="cbpMatrixSelected"
                    ShowLoadingPanel="false" OnCallback="cbpMatrixSelected_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMatrixSelected').show(); }"
                        EndCallback="function(s,e){ onCbpMatrixSelectedEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height: 272px;overflow-y:auto;">
                                <asp:GridView ID="grdSelected" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectSelected" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" Checked='<%# DataBinder.Eval(Container, "DataItem.IsChecked") %>' runat="server" CssClass="chkSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ID" ItemStyle-CssClass="keyField" HeaderStyle-CssClass="keyField"  />
                                        <asp:TemplateField HeaderText = "Item" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate> 
                                                <asp:Label ID="lblItemName2" Text = '<%# DataBinder.Eval(Container, "DataItem.Name") %>' ToolTip = '<%# DataBinder.Eval(Container, "DataItem.ToolTip") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingMatrixSelected">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingMatrixSelected"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>
<dxcp:ASPxCallbackPanel ID="cbpMatrixProcess" runat="server" Width="100%" ClientInstanceName="cbpMatrixProcess"
    ShowLoadingPanel="false" OnCallback="cbpMatrixProcess_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpMatrixProcessEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>
<div style="width:100%;text-align:center">
    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
        <tr>
            <td><input type="button" value='<%= GetLabel("Save")%>'  class="btnCRUDButton w3-btn w3-hover-blue" style="width:90px" id="btnSaveMatrix" /></td>
            <td><input type="button" value='<%= GetLabel("Close")%>' class="btnCRUDButton w3-btn w3-hover-blue" style="width:90px" onclick="pcRightPanelContent.Hide();" /></td>
        </tr>
    </table>
</div>

