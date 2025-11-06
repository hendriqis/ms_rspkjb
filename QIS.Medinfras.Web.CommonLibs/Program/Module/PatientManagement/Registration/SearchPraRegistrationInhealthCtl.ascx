<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPraRegistrationInhealthCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SearchPraRegistrationInhealthCtl" %>
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
        setDatePicker('<%:txtDate.ClientID %>');
        $('#<%:txtDate.ClientID %>').datepicker('option', 'maxDate', 0);

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

    function onAfterSaveAddRecordEntryPopup(param) {
        $('#hdnRightPanelContentCode').val('searchPraRegistration');
        return $('#<%=txtQuery.ClientID %>').val(param);
    }

    $("#btnSearchPraRegistrasi").on("click", function (e) {
        e.preventDefault();
        cbpView.PerformCallback();
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
            showToast("Data Pra Registrasi", "Hanya boleh pilih 1 (satu) nomor pra registrasi untuk setiap kunjungan !");
            return false;
        }
        else if (count == 0) {
            showToast("Data Pra Registrasi", "Belum ada nomor pra registrasi yang dipilih !");
            return false;
        }
        else
        {
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
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
                                <%=GetLabel("Tanggal")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDate" Width="100px" runat="server" Style="text-align: center" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pencarian")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtQuery" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" id="btnSearchPraRegistrasi" value='<%= GetLabel("Cari Data")%>' />
                        </td>
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
                                            <asp:BoundField DataField="PRAREGISTRATIONNUMBER" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PRAREGISTRATIONNUMBER" HeaderText="No. Pra Registrasi" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NOKAPESERTA" HeaderText="No. Peserta" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NMPST" HeaderText="Nama Peserta" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfTanggalKunjungan" HeaderText="Tanggal Kunjungan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfTanggalLahir" HeaderText="Tgl. Lahir" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NOHP" HeaderText="No. HP" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="TujuanPoli" HeaderText="Tujuan Poli" HeaderStyle-HorizontalAlign="Left"
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
