<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpecimenInfoCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SpecimenInfoCtl1" %>
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
<script type="text/javascript" id="dxss_NewSurgeryOrderEntryCtl1">
    $(function () {
        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();
    });

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }
</script>
<style type="text/css">
    
</style>
<div>
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

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 22%" />
            <col style="width: 78%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Informasi Pengambilan Sampel" class="w3-hover-red">Pengambilan Sampel</li>
                        <li contentID="divPage2" title="Informasi Pengiriman Sampel" class="w3-hover-red">Pengiriman Sampel</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Pengambilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSampleDate" Width="120px" runat="server" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSampleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"  ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dilakukan oleh")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsUsingExistingSample" Width="100%" runat="server" Text=" Pemeriksaan Tambahan (menggunakan sample yang sudah ada)" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <tr>
                                        <td>
                                             <div style="position: relative;">
                                                <dxcp:ASPxCallbackPanel ID="cbpSpecimenView" runat="server" Width="100%" ClientInstanceName="cbpSpecimenView"
                                                    ShowLoadingPanel="false" OnCallback="cbpSpecimenView_Callback">
                                                    <ClientSideEvents EndCallback="function(s,e){ onCbpSpecimenViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage6">
                                                                <asp:GridView ID="grdSpecimenView" runat="server" CssClass="grdSelected "
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                            <ItemTemplate>
                                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                <input type="hidden" value="<%#:Eval("SpecimenID") %>" bindingfield="SpecimenID" />
                                                                                <input type="hidden" value="<%#:Eval("SpecimenCode") %>" bindingfield="SpecimenCode" />
                                                                                <input type="hidden" value="<%#:Eval("SpecimenName") %>" bindingfield="SpecimenName" />
                                                                                <input type="hidden" value="<%#:Eval("GCSpecimenContainerType") %>" bindingfield="GCSpecimenContainerType" />
                                                                                <input type="hidden" value="<%#:Eval("cfQuantity") %>" bindingfield="cfQuantity" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <%=GetLabel("Jenis Sampel")%>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div><%#: Eval("SpecimenName")%></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Jenis Tabung"  DataField="SpecimenContainerType" />
                                                                        <asp:BoundField HeaderText="Jumlah Sampel"  DataField="cfQuantity" />
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("Belum ada informasi jenis sampel untuk pasien ini") %>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="SpecimenPaging">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                           
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Pengambilan Sampel") %></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="150px" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsDelivery" Width="100%" runat="server" Text=" Sudah dilakukan pengiriman sampel" Checked="true" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Pengiriman")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeliveryDate" Width="120px" runat="server"  ReadOnly="true"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeliveryTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dikirim/Diantar oleh")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedic2" Width="100%" runat="server" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
</div>
