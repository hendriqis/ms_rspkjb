<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BPJSReferralInformation.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BPJSReferralInformation" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_Referralctl">
    $("#btnSearchFaskes").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtKeyword.ClientID %>').val() == '')
            alert("Kata Kunci Harus Di Isi");
        else {
            var keyword = $('#<%=txtKeyword.ClientID %>').val();
            BPJSService.getBPJSReferralInformation(keyword, function (result) {
                try {
                    var obj = jQuery.parseJSON(result);
                    if (obj.metadata.code == "200") {
                        $('#<%=hdnReferralList.ClientID %>').val(result);
                        cbpView.PerformCallback();
                    }
                    else {
                        alert(obj.metadata.message);
                        $('#<%=hdnReferralList.ClientID %>').val('');
                    }
                } catch (err) {
                    $('#<%=hdnReferralList.ClientID %>').val('');
                    alert(err);
                }
            });
        }
    });
</script>
<input type="hidden" id="hdnReferralList" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <div class="pageTitle">
        <%=GetLabel("List Rujukan Faskes/RS")%></div>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kata Kunci :")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKeyword" runat="server" Width="100%" />
                        </td>
                        <td>
                            <input type="button" id="btnSearchFaskes" value='<%= GetLabel("Data Peserta")%>' />
                        </td>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlBPJSReferralInformation" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <columns>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="80px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kode Cabang")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("kdCabang")%>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="100px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kode Provider")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("kdProvider")%>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="100px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Nama Cabang")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("nmCabang")%>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="50px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Nama Provider")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("nmProvider")%>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </columns>
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
