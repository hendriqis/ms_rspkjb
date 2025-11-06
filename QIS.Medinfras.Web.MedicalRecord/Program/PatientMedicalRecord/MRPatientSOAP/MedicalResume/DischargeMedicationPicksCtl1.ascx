<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DischargeMedicationPicksCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.DischargeMedicationPicksCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<style type="text/css">
    .trSelectedItem {background-color: #ecf0f1 !important;}
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
                $('#<%=hdnOrderID.ClientID %>').val($(this).find('.keyField').html());

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

<div style="padding:3px;">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnPopupVisitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedVisitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedItem" runat="server" value="" />
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
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                        EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" CssClass="pnlContainerGridPatientPage">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-top:20px; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PrescriptionOrderNo" HeaderStyle-CssClass="hiddenColumn prescriptionOrderNo" ItemStyle-CssClass="hiddenColumn prescriptionOrderNo" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <%=GetLabel("Daftar Resep Pulang")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <div>
                                                                <%#: Eval("cfPrescriptionDateText")%>,
                                                                <%#: Eval("PrescriptionTime") %>,
                                                                <%#: Eval("PrescriptionOrderNo")%>
                                                            </div>
                                                            <div style="width: 250px; float: left">
                                                                <%#: Eval("ParamedicName") %>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi pelayanan resep pulang untuk pasien")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>        
                <div class="imgLoadingGrdView" id="containerImgLoadingView1" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div> 
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div>
            </td>
            <td style="padding:2px;vertical-align:top">
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width:120px"/>
                        <col style="width:100px"/>
                        <col style="width:250px"/>
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
                                    <asp:GridView ID="grdPopupViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="keyField" value='<%#:Eval("PrescriptionOrderDetailID")%>' />
                                                    <input type="hidden" class="itemName" value='<%#:Eval("DrugName")%>' />
                                                    <input type="hidden" class="signaRule" value='<%#:Eval("cfConsumeMethod2")%>' />
                                                    <input type="hidden" class="route" value='<%#:Eval("Route")%>' />
                                                    <input type="hidden" class="medicationAdministration" value='<%#:Eval("MedicationAdministration")%>' />
                                                    <input type="hidden" class="dispenseQty" value='<%#:Eval("DispenseQtyInString")%>' />
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Item Name")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("DrugName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Frekuensi" >
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCDosingFrequency")%>' class="hdnGCItemUnit" />
                                                    <input type="number" value='<%#:Eval("Frequency")%>' class="txtFrequency" min="0" value="0" style="width:40px; text-align: right" readonly="readonly"/> <%#:Eval("DosingFrequency") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Dosis" >
                                                <ItemTemplate>
                                                    <input type="text" value='<%#:Eval("cfNumberOfDosage")%>' class="txtQty" style="width:40px; text-align: right" readonly="readonly"  /> <%#:Eval("DosingUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Diresepkan" >
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                                    <input type="number" value='<%#:Eval("DispenseQty")%>' class="txtDispenseQty" min="0" value="0" style="width:40px; text-align: right" readonly="readonly" /> <%#:Eval("ItemUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi resep pulang untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
<%--                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt1"></div>
                        </div>
                    </div> --%>
                </div>
            </td>
        </tr>
    </table>
</div>