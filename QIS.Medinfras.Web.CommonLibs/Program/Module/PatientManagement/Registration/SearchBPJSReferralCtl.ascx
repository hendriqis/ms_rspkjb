<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBPJSReferralCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SearchBPJSReferralCtl" %>
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
</style>
<script type="text/javascript" id="dxss_Referralctl">
    $(function () {
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnSelectedID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                $(this).addClass('selected');
            }
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();
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
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        return result;
    }

    function onAfterProcessPopupEntry(param) {
        $('#hdnRightPanelContentCode').val('getDataRujukan');
        return $('#<%=txtNoPeserta.ClientID %>').val(param);
    }

    $("#btnSearchPeserta").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNoPeserta.ClientID %>').val() == '')
            showToast("Data Rujukan", "Nomor Kartu Peserta harus diisi!");
        else {
            var noPeserta = $('#<%=txtNoPeserta.ClientID %>').val();
            var asalRujukan = 2;
            BPJSService.getRujukanList(noPeserta, asalRujukan, function (result) {
                try {
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        $('#<%=hdnListRujukan.ClientID %>').val(resultInfo[1]);
                    }
                    else {
                        showToast("Data Rujukan", resultInfo[2]);
                        $('#<%=hdnListRujukan.ClientID %>').val('');
                    }
                    cbpView.PerformCallback();
                } catch (err) {
                    $('#<%=hdnListRujukan.ClientID %>').val('');
                    showToast("Data Rujukan BPJS", err);
                }
            });
        }
    });

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

    function getSelectedItem() {
        var tempSelectedID = "";
        var count = 0;
        $('.grdView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }
            tempSelectedID += id;
            count += 1;
        });
        if (count > 1) {
            showToast("Data Rujukan BPJS", "Hanya boleh pilih 1 (satu) nomor rujukan untuk setiap kunjungan !");
            return false;
        }
        else if (count == 0) {
            showToast("Data Rujukan BPJS", "Belum ada nomor rujukan yang dipilih !");
            return false;
        }
        else
        {
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == '0') {
            showToast("Data Rujukan BPJS", "Error Message : <br/><span style='color:red'>" + param[1] + "</span>");
        }
    }
</script>
<input type="hidden" id="hdnListRujukan" runat="server" />
<input type="hidden" id="hdnAsalRujukan" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnNoPeserta" runat="server" />
<input type="hidden" id="hdnIsBridgingBPJSVClaimVersion" runat="server" />
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Kartu")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoPeserta" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <%--<td>
                            <input type="button" id="btnSearchPeserta" value='<%= GetLabel("Cari Data Rujukan")%>' />
                        </td>--%>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="noKunjungan" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="tglKunjungan" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="noKunjungan" HeaderText="No. Kunjungan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="namaPelayanan" HeaderText="Jenis Pelayanan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="namaPoli" HeaderText="Poli Rujukan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfDiagnosa" HeaderText="Diagnosa" HeaderStyle-Width="250px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="keluhan" HeaderText="Keluhan" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
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
