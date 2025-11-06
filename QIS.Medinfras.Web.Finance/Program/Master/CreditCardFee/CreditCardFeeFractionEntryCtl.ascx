<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditCardFeeFractionEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.CreditCardFeeFractionEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_CreditCardFeeFractionEntryCtl">
    $(function () {
        setDatePicker('<%=txtEffectiveDate.ClientID %>');
        $('#<%=txtEffectiveDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtEffectiveDate.ClientID %>').val($('#<%:hdnDefaultDateNow.ClientID %>').val());
        $('#<%=txtSurchargeFee.ClientID %>').val('');
        $('#<%=txtMDRFee.ClientID %>').val('');
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnCreditCardID.ClientID %>').val('');
        $('#<%=txtEffectiveDate.ClientID %>').val('');
        $('#<%=txtSurchargeFee.ClientID %>').val('');
        $('#<%=txtMDRFee.ClientID %>').val('');
        $('#containerPopupEntryData').hide();
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {

        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnCreditCardID.ClientID %>').val(entity.CreditCardID);
        $('#<%=txtEffectiveDate.ClientID %>').val(entity.cfEffectiveDateInStringDatePickerFormat);
        $('#<%=txtSurchargeFee.ClientID %>').val(entity.SurchargeFee);
        $('#<%=txtMDRFee.ClientID %>').val(entity.MDRFee);
        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#<%=hdnID.ClientID %>').val('');
                $('#<%=txtEffectiveDate.ClientID %>').val('');
                $('#<%=txtSurchargeFee.ClientID %>').val('');
                $('#<%=txtMDRFee.ClientID %>').val('');
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();        
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[2]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    $('#<%=txtSurchargeFee.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        var value = parseDecimal($(this).attr('hiddenVal'));
        $('#<%=txtSurchargeFee.ClientID %>').val(value).trigger('changeValue');
    });

    $('#<%=txtMDRFee.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        var value = parseDecimal($(this).attr('hiddenVal'));
        $('#<%=txtMDRFee.ClientID %>').val(value).trigger('changeValue');
    });
</script>
<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnDefaultDateNow" value="" runat="server"/>
    <input type="hidden" id="hdnCreditCardID" value="" runat="server"/>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:95%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:100px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Mesin EDC")%></label></td>
                        <td><asp:TextBox ID="txtEDCMachineCode" ReadOnly="true" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtEDCMachineName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col style="width:400px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Effective Date")%></label></td>
                                <td><asp:TextBox ID="txtEffectiveDate" CssClass="datepicker required" runat="server" Width="100px" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Surcharge Fee (%)")%></label></td>
                                <td><asp:TextBox ID="txtSurchargeFee" CssClass="required number" runat="server" Width="100px" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("MDR Fee (%)")%></label></td>
                                <td><asp:TextBox ID="txtMDRFee" CssClass="required number" runat="server" Width="100px" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />

                                                <input type="hidden" value="<%#: Eval("ID")%>" bindingfield = "ID"/>
                                                <input type="hidden" value="<%#: Eval("CreditCardID")%>" bindingfield = "CreditCardID"/>
                                                <input type="hidden" value="<%#: Eval("EffectiveDate")%>" bindingfield = "EffectiveDate"/>
                                                <input type="hidden" value="<%#: Eval("cfEffectiveDateInStringDatePickerFormat")%>" bindingfield = "cfEffectiveDateInStringDatePickerFormat"/>
                                                <input type="hidden" value="<%#: Eval("SurchargeFee")%>" bindingfield= "SurchargeFee"/>
                                                <input type="hidden" value="<%#: Eval("MDRFee")%>" bindingfield= "MDRFee" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfEffectiveDateInString" ItemStyle-CssClass="tdEffectiveDate" HeaderText="Effective Date" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="SurchargeFee" ItemStyle-CssClass="tdSurchargeFee" HeaderText="Surcharge Fee (%)"  HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MDRFee" ItemStyle-CssClass="tdMDRFee" HeaderText="MDR Fee (%)" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data tidak tersedia")%>
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
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

