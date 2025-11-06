<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDiagnosisHistoryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientDiagnosisHistoryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
    .important
    {
        background-color: #c23616;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_Referralctl">
    $(function () {
        $('#<%=grdDiagnosisSummaryView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdDiagnosisSummaryView.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnSelectedID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                $(this).addClass('selected');
            }
        });
        $('#<%=grdDiagnosisSummaryView.ClientID %> tr:eq(1)').click();
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else
        {
            return true;
        }
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedDiagnosisID.ClientID %>').val();
        return result;
    }

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
        }
        else {
            $cell.removeClass('highlight');
        }
    });


    $('.chkIsMainDiagnosis input').die('change');
    $('.chkIsMainDiagnosis input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            var count = 0;
            $('.grdSelected .chkIsMainDiagnosis input:checked').each(function () {
                count += 1;
            });
            if (count > 1) {
                displayMessageBox("Salin Diagnosa", "Untuk Diagnosa utama hanya boleh 1 (satu) dalam 1 (satu) kunjungan.");
                $(this).prop('checked', false);
                $cell.removeClass('important');
            }
            else {
                $cell.addClass('important');            
            }
        }
        else {
            $cell.removeClass('important');
        }
    });

    function getSelectedItem() {
        var lstSelectedDiagnosisID = [];
        var lstSelectedDiagnosisName = [];
        var lstSelectedDiagnosisText = [];
        var lstSelectedMainDiagnosis = [];
        var count = 0;
        $('.grdSelected .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var diagnosisID = $(this).closest('tr').find('.keyField').html();
            var diagnosisName = $(this).closest('tr').find('.diagnosisName').html();
            var diagnosisText = $(this).closest('tr').find('.diagnosisText').html();

            var isMainDiagnosis = '0';
            if ($tr.find('.chkIsMainDiagnosis input').is(':checked')) {
                isMainDiagnosis = '1';
            }

            lstSelectedDiagnosisID.push(diagnosisID);
            lstSelectedDiagnosisName.push(diagnosisName);
            lstSelectedDiagnosisText.push(diagnosisText);
            lstSelectedMainDiagnosis.push(isMainDiagnosis);
            count += 1;
        });

        if (count == 0) {
            var messageBody = "Belum ada item yang dipilih.";
            displayMessageBox('Lookup : Riwayat Diagnosa', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedDiagnosisID.ClientID %>').val(lstSelectedDiagnosisID.join('|'));
            $('#<%=hdnSelectedDiagnosisName.ClientID %>').val(lstSelectedDiagnosisName.join('|'));
            $('#<%=hdnSelectedDiagnosisText.ClientID %>').val(lstSelectedDiagnosisText.join('|'));
            $('#<%=hdnSelectedMainDiagnosis.ClientID %>').val(lstSelectedMainDiagnosis.join('|'));
            return true;
        }
    }

    function oncbpLookupViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onRefreshDiagnosisGrid == 'function') {
            onRefreshDiagnosisGrid();
        }
    }
</script>
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnPopupVisitID" runat="server" value="" />
<input type="hidden" id="hdnPopupMRN" runat="server" value="" />
<input type="hidden" id="hdnPopupVisitDate" runat="server" value="" />
<input type="hidden" id="hdnPopupVisitTime" runat="server" value="" />
<input type="hidden" id="hdnSelectedDiagnosisID" runat="server" value="" />
<input type="hidden" id="hdnSelectedDiagnosisName" runat="server" value="" />
<input type="hidden" id="hdnSelectedDiagnosisText" runat="server" value="" />
<input type="hidden" id="hdnSelectedMainDiagnosis" runat="server" value="" />

<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpLookupView" runat="server" Width="100%" ClientInstanceName="cbpLookupView"
                        ShowLoadingPanel="false" OnCallback="cbpLookupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpLookupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdDiagnosisSummaryView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px" HeaderText="Diagnosa Utama">
                                                <ItemTemplate>
                                                     <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsMainDiagnosis" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DiagnoseID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="DiagnosisText" HeaderStyle-CssClass="diagnosisText" ItemStyle-CssClass="diagnosisText" HeaderText="Diagnosa (Dokter)" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DiagnoseID" HeaderStyle-CssClass="diagnosisID" ItemStyle-CssClass="diagnosisID" HeaderText="Kode Diagnosa (ICD)" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ICDDiagnosisName" HeaderStyle-CssClass="diagnosisName" ItemStyle-CssClass="diagnosisName" HeaderText="Nama Diagnosa (ICD)" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Jumlah Kunjungan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" HeaderStyle-CssClass="noOfVisit" ItemStyle-CssClass="noOfVisit">
                                                <ItemTemplate>
                                                    <label class="lblNoOfVisit lblLink">
                                                        <%#:Eval("cfNoOfVisit", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi diagnosa untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
