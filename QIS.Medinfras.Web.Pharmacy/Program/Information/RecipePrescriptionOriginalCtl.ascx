<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecipePrescriptionOriginalCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.RecipePrescriptionOriginalCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<style type="text/css">
    .trSelectedItem
    {
        background-color: #ecf0f1 !important;
    }
</style>
<script type="text/javascript" id="dxss_dischargedrugsquickpicksCtl1">
    function onBeforeProcess(param) {
        if (!getCheckedMember()) {
            return false;
        }
        else {
            return true;
        }
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function getCheckedMember() {
        var lstSelectedMember = [];

        var result = '';
        var count = 0;

        $('.grdSelected .chkIsSelected input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').val();
            var itemName = $tr.find('.itemName').val();
            var signaRule = $tr.find('.signaRule').val();
            var route = $tr.find('.route').val();
            var dispenseQty = $tr.find('.dispenseQty').val();
            var medicationAdministration = $tr.find('.medicationAdministration').val();
            var medicationLineText = itemName + ";" + signaRule + " " + medicationAdministration + " (" + route + ")" + "; Jumlah: " + dispenseQty;

            lstSelectedMember.push(medicationLineText);

            count += 1;
        });

        if (count == 0) {
            var messageBody = "Belum ada item yang dipilih.";
            displayMessageBox('Lookup : Riwayat Pengobatan', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedItem.ClientID %>').val(lstSelectedMember.join('|'));
            return true;
        }
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        hideLoadingPanel();

        setPaging($("#pagingPopup"), pageCount, function (page) {
            RefreshGrid(true, page);
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnHistoryID.ClientID %>').val($(this).find('.keyField').html());

                if (typeof (grdHistory) != 'undefined' && typeof (cbpPopupViewDt) != 'undefined')
                    cbpPopupViewDt.PerformCallback('refresh');
                else
                    window.setTimeout("cbpPopupViewDt.PerformCallback('refresh');", 100);
            }
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function RefreshGridDetail(mode, pageNo) {
        getCheckedMember();
    }

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                RefreshGrid(true, page);
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion

    //#region Paging Dt
    function onCbpPopupViewDtEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPopupViewDt.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt1"), pageCount, function (page) {
                cbpPopupViewDt.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPopupViewDt.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterLookUpDischargePrescription == 'function') {
            onAfterLookUpDischargePrescription(param);
        }
    }
</script>
<div style="padding: 3px;">
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnHistoryID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedItem" runat="server" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 2px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" CssClass="pnlContainerGridPatientPage">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="HistoryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="hiddenColumn prescriptionOrderID"
                                            ItemStyle-CssClass="hiddenColumn" />
                                        <asp:BoundField DataField="PrescriptionOrderNo" HeaderStyle-CssClass="hiddenColumn prescriptionOrderNo"
                                            ItemStyle-CssClass="hiddenColumn prescriptionOrderNo" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <%=GetLabel("Daftar Order Resep")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <div>
                                                                <%#: Eval("cfPrescriptionDateText")%>,
                                                                <%#: Eval("PrescriptionTime") %>
                                                            </div>
                                                            <div>
                                                                <%#: Eval("PrescriptionOrderNo")%>
                                                            </div>
                                                            <div style="width: 250px; float: left">
                                                                <%#: Eval("ParamedicName") %>
                                                            </div>
                                                            <div style="width: 250px; float: left; font-style: italic">
                                                                <%#: Eval("cfSendOrderDateTime") %>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada pengiriman resep dari EMR untuk nomor order ini.")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 2px; vertical-align: top">
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                        <col />
                    </colgroup>
                </table>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupViewDt" runat="server" Width="100%" ClientInstanceName="cbpPopupViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpPopupViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdPopupViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                                ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemName">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Drug Name")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="font-weight: bold">
                                                            <%#: Eval("cfMedicationName")%></span>
                                                    </div>
                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                        <%#: Eval("cfCompoundDetail")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Frequency" HeaderText="Frequency" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                            <asp:BoundField DataField="DosingFrequency" HeaderText="Timeline" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" Visible="false" />
                                            <asp:BoundField DataField="cfNumberOfDosage" HeaderText="Dose" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                            <asp:BoundField DataField="DosingUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" Visible="false" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Signa")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("cfConsumeMethod2")%></div>
                                                    <div>
                                                        <%#: Eval("MedicationAdministration")%></div>
                                                    <div>
                                                        <%#: Eval("Route")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StartDateInDatePickerFormat" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" Visible="false" />
                                            <asp:BoundField DataField="DispenseQtyInString" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTakenQty" HeaderText="Taken" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada pengiriman resep dari EMR untuk nomor order ini.")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
