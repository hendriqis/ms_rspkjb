<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueueEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.QueueEditCtl" %>
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
        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        return result;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == "update") {
            if (param[1] == "success") {
                showToast("Success!", "Update antrian berhasil");
            }
            else {
                showToast("Failed!", param[2]);
            }
        }
    }

    $('.btnUpdateQueue').die('click');
    $('.btnUpdateQueue').live('click', function () {
        $tr = $(this).closest('tr').parent().closest('tr');
        var visitID = $tr.find('.hdnVisitID').val();
        var session = $(this).closest('tr').find('.divSelectedSession').html();
        cbpView.PerformCallback('update|' + visitID + "|" + session);
    });

    function onCboSessionChanged(s) {
        $tr = $(s.GetInputElement()).closest('tr').parent().closest('tr');
        $tr.find('.divSelectedSession').html(s.GetValue());
    }

    $('.imgPrint.imgLink').die('click');
    $('.imgPrint.imgLink').live('click', function () {
        $tr = $(this).closest('tr').parent().closest('tr');
        var visitID = $tr.find('.hdnVisitID').val();
        cbpView.PerformCallback('print|' + visitID);
    });

</script>
<input type="hidden" id="hdnListRujukan" runat="server" />
<input type="hidden" id="hdnAsalRujukan" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnRegistrationID" runat="server" />
<input type="hidden" id="hdnIsBridgingToGateway" runat="server" />
<input type="hidden" id="hdnProviderGatewayService" runat="server" />
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
                                <%=GetLabel("No. Registrasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRegistrationNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Rekam Medis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtMedicalNo" runat="server" Width="99%" ReadOnly="true" />
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Klinik" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="VisitTypeName" HeaderText="Kunjungan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="250px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Session" HeaderText="Sesi" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="QueueNo" HeaderText="Antrian" HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ReferenceQueueNo" HeaderText="Antrian Lama" HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <table width="200px" cellpadding="2px" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <div style="display: none" class="divSelectedSession" id="divSelectedSession" runat=server>
                                                                </div>
                                                                <dxe:ASPxComboBox CssClass="cboSession" ID="cboSession" runat="server" Width="120px" >
                                                                <ClientSideEvents ValueChanged="function(s,e){ onCboSessionChanged(s); }" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td>
                                                                <input type="button" class="btnUpdateQueue w3-btn w3-hover-blue" value='<%=GetLabel("Ambil Antrian") %>' style="width: 100%;background-color:Navy;color:White;" />
                                                            </td>
                                                            <td>
                                                                <img class="imgPrint imgLink"
                                                    title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                    alt="" />
                                                            </td>
                                                            <input type="hidden" value='<%#:Eval("VisitID") %>' class="hdnVisitID" />
                                                        </tr>
                                                    </table>
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
    </table>
</div>
