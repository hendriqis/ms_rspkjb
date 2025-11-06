<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendPGxTestOrderCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SendPGxTestOrderCtl" %>
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
        return $('#<%=txtTransactionNo.ClientID %>').val(param);
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
        if (count == 0) {
            displayMessageBox("Send PGx Test Order", "Belum ada item pemeriksaan yang dipilih !");
            return false;
        }
        else {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == '0') {
            displayMessageBox("Send PGx Test Order", "Error Message : <br/><span style='color:red'>" + param[1] + "</span>");
        }
        else {
            displayMessageBox("Send PGx Test Order", "SUCCESS : <br/><span>" + "Order pemeriksaan berhasil dikirim" + "</span>");
        }
    }
</script>
<input type="hidden" id="hdnTransactionID" runat="server" />
<input type="hidden" id="hdnTestOrderID" runat="server" />
<input type="hidden" id="hdnListRujukan" runat="server" />
<input type="hidden" id="hdnAsalRujukan" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea" style="width:100%;">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="180px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Transaksi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"  style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Klinis")%></label>
                        </td>
                        <td colspan="2" style="vertical-align:top">
                            <asp:TextBox ID="txtClinicalNotes" runat="server" Width="420px" TextMode="Multiline" Rows="5" Height="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="vertical-align:top">
                            <div>
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                                                margin-right: auto; position: relative; font-size: 0.95em;">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Kode ")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through; color:red;font-style:italic'":"" %>>
                                                                    <%#: Eval("ItemCode")%> | <%#: Eval("OldItemCode")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Nama Pemeriksaan")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through; color:red;font-style:italic'":"" %>>
                                                                    <%#: Eval("ItemName1")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DetailReferenceNo" HeaderText = "Reference No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="80px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("STATUS")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div>
                                                                    <%#: Eval("LISBridgingStatus")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
                    <tr>
                        <td class="tdLabel"  style="vertical-align:top" colspan="3">
                            <label class="lblNormal">
                                Pasien berisiko atau memiliki penyakit berikut :</label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-left:10px">
                            <asp:CheckBoxList id="chkListRisk" 
                                       CellPadding="5"
                                       CellSpacing="5"
                                       RepeatColumns="3"
                                       RepeatDirection="Horizontal"
                                       TextAlign="Right"
                                       runat="server">
 
                                     <asp:ListItem Value="T2DM"> T2DM</asp:ListItem>
                                     <asp:ListItem Value="HYPERTENSION"> Hypertension</asp:ListItem>
                                     <asp:ListItem Value="ISCHAEMIC_HEART_DISEASE"> Ischaemic Heart Disease</asp:ListItem>
                                     <asp:ListItem Value="STROKE"> Stroke</asp:ListItem>
                                     <asp:ListItem Value="MAJOR_DEPRESIVE_DISORDER"> Major Depressive Disorder</asp:ListItem>
                                     <asp:ListItem Value="GOUT"> Gout</asp:ListItem>
                                     <asp:ListItem Value="RHEUMATOID_ARTHRITIS"> Rheumatoid Arthritis</asp:ListItem>
                                     <asp:ListItem Value="ANXIETY"> Anxiety</asp:ListItem>
                                     <asp:ListItem Value="HYPERLIPIDEMIA"> Hyperlipidemia</asp:ListItem>
                                  </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"  style="vertical-align:top" colspan="3">
                            <asp:CheckBox ID="chkIsHasFamilyMemberWithPGxTest" Checked="false" runat="server" /> Memiliki Anggota Keluarga yang melakukan Pemeriksaan PGx Test
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"  style="vertical-align:top" colspan="3">
                            <asp:CheckBox ID="chkIsInterestedInADR" Checked="false" runat="server" /> Pasien tertarik untuk memvalidasi potensi reaksi merugikan yang mereka alami
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
