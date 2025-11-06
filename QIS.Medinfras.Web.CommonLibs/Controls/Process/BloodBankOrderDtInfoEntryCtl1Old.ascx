<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloodBankOrderDtInfoEntryCtl1Old.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BloodBankOrderDtInfoEntryCtl1Old" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
<div>
<script type="text/javascript" id="dxss_BloodBankOrderDtInfoEntryCtl1">
    setDatePicker('<%=txtExpiredDate.ClientID %>');
    $('#<%=txtExpiredDate.ClientID %>').datepicker('option', 'minDate', '0');

    $(function () {
        $('#<%=grdItemList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdItemList.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnItemID.ClientID %>').val($(this).find('.itemID').html());
            //alert('refresh detail');
            //cbpViewDetail1.PerformCallback('refresh');
            //            if ($('#cbpDetailView1').length > 0) {
            //                alert('Element exists!');
            //            } else {
            //                alert('Element does not exist!');
            //            }
        });
        $('#<%=grdItemList.ClientID %> tr:eq(1)').click();
    });

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }

    //#region Detail Info
    function ResetDetailEntryControls(s) {
        cboPackingQuality.SetValue('');
        $('#<%=hdnSpecimenProcessMode.ClientID %>').val('0');
    }

    $('.imgAddDetail.imgLink').die('click');
    $('.imgAddDetail.imgLink').live('click', function (evt) {
        ResetDetailEntryControls();
        $('#<%=hdnSpecimenProcessMode.ClientID %>').val("1");
        $('#trDetailInfo').removeAttr('style');
    });

    $('.btnApplySpecimen').click(function () {
        submitSpecimen();
        $('#trDetailInfo').attr('style', 'display:none');
    });

    $('.btnCancelDetail').click(function () {
        ResetDetailEntryControls();
        $('#trDetailInfo').attr('style', 'display:none');
    });

    $('.imgEditDetail.imgLink').die('click');
    $('.imgEditDetail.imgLink').live('click', function () {
        $('#<%=hdnSpecimenProcessMode.ClientID %>').val('0');
        SetSpecimenEntityToControl(this);
        $('#trDetailInfo').removeAttr('style');
    });

    $('.imgDeleteDetail.imgLink').die('click');
    $('.imgDeleteDetail.imgLink').live('click', function () {
        $('#<%=hdnOrderDtSpecimenID.ClientID %>').val($(this).attr('recordID'));
        var specimenName = $(this).attr('specimenName')
        var message = "Hapus Jenis sampel <b>'" + specimenName + "'</b> dari order pemeriksaan ?";
        displayConfirmationMessageBox('Sampel Pemeriksaan', message, function (result) {
            if (result) {
                cbpSpecimen.PerformCallback('delete');
            }
        });
    });

    function GetCurrentSelectedSpecimen(s) {
        var $tr = $(s).closest('tr').parent().closest('tr');
        var idx = $('#<%=grdDetailView.ClientID %> tr').index($tr);
        $('#<%=grdDetailView.ClientID %> tr:eq(' + idx + ')').click();

        $row = $('#<%=grdDetailView.ClientID %> tr.selected');
        var selectedObj = {};

        $row.find('input[type=hidden]').each(function () {
            selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });

        return selectedObj;
    }

    function SetSpecimenEntityToControl(param) {
        $('#<%=hdnOrderDtSpecimenID.ClientID %>').val($(param).attr('recordID'));
        cboContainerType.SetValue($(param).attr('containerType'));

        $('#<%=hdnEntrySpecimenID.ClientID %>').val($(param).attr('specimenID'));
    }

    function submitSpecimen() {
        if ($('#<%=hdnEntrySpecimenID.ClientID %>').val() != '') {
            if ($('#<%=hdnSpecimenProcessMode.ClientID %>').val() == "1")
                cbpSpecimen.PerformCallback('add');
            else
                cbpSpecimen.PerformCallback('edit');
        }
        else {
            displayErrorMessageBox("ERROR", "Jenis spesimen harus dipilih sebelum diproses !");
        }
    }

    function onCbpSpecimenEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit") {
                $('#<%=hdnSpecimenProcessMode.ClientID %>').val('0');
            }

            $('#<%=hdnTestOrderID.ClientID %>').val(param[2]);

            ResetDetailEntryControls();
            cbpDetailView1.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            displayErrorMessageBox("Sampel Pemeriksaan", 'Error Message : ' + param[2]);
        }
        else
            $('#<%=grdDetailView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

</script>

    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnFromHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnSpecimenProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtSpecimenID" value="" />
    <input type="hidden" runat="server" id="hdnEntrySpecimenID" value="" />
    <input type="hidden" runat="server" id="hdnItemID" value="" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
        </colgroup>
        <tr>
            <td style="vertical-align:top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpItemList" runat="server" Width="100%" ClientInstanceName="cbpItemList"
                        ShowLoadingPanel="false" OnCallback="cbpItemList_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                            EndCallback="function(s,e){ oncbpItemListEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="panItemList" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdItemList" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="hiddenColumn itemID" ItemStyle-CssClass="hiddenColumn itemID" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Jenis Darah" HeaderStyle-CssClass="ItemName" ItemStyle-CssClass="ItemName" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfQuantity" HeaderText="Jumlah" HeaderStyle-CssClass="ItemQty" ItemStyle-CssClass="ItemQty" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada jenis darah yang tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerHdImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingHd"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td style="vertical-align:top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                <tr id="trDetailInfo">
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col width="180px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("No. Kantong Darah")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBloodBagNo" Width="120px" CssClass="number" runat="server" />
                                                </td>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Expired Date")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExpiredDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kualitas Kantong")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboPackingQuality" ClientInstanceName="cboPackingQuality"
                                                        Width="150px">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kualitas Darah")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboBloodQuality" ClientInstanceName="cboBloodQuality"
                                                        Width="150px">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <img class="btnApplySpecimen imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td>
                                                                <img class="btnCancelDetail imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpDetailView1" runat="server" Width="100%" ClientInstanceName="cbpDetailView1"
                                                ShowLoadingPanel="false" OnCallback="cbpDetailView1_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ oncbpDetailViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent6" runat="server">
                                                        <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdDetailView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("LabelNo") %>" bindingfield="LabelNo" />
                                                                            <input type="hidden" value="<%#:Eval("GCPackingQuality") %>" bindingfield="GCPackingQuality" />
                                                                            <input type="hidden" value="<%#:Eval("GCBloodQuality") %>" bindingfield="GCBloodQuality" />
                                                                            <input type="hidden" value="<%#:Eval("ExpiredDate") %>" bindingfield="ExpiredDate" />
                                                                            <input type="hidden" value="<%#:Eval("cfExpiredDate") %>" bindingfield="cfExpiredDate" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddDetail imgLink" title='<%=GetLabel("+ Detail Kantong")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt=""/>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditDetail imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" gcPackingQuality = "<%#:Eval("GCPackingQuality") %>" gcBloodQuality = "<%#:Eval("gcBloodQuality") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteDetail imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" gcPackingQuality = "<%#:Eval("GCPackingQuality") %>" gcBloodQuality = "<%#:Eval("gcBloodQuality") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("No. Kantong Darah")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div><%#: Eval("LabelNo")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Kondisi Kemasan"  DataField="PackingQuality" />
                                                                    <asp:BoundField HeaderText="Kualitas Darah"  DataField="BloodQuality" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada informasi kantong darah untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="detailViewPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                           
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpSpecimen" runat="server" Width="100%" ClientInstanceName="cbpSpecimen"
        ShowLoadingPanel="false" OnCallback="cbpSpecimen_Callback">
        <ClientSideEvents EndCallback="function(s,e){ onCbpSpecimenEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
