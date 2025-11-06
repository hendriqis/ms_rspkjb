<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="iCareInformationCtl.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.iCareInformationCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.btnGetICare').die('click');
            $('.btnGetICare').live('click', function () {
                if ($('#<%=hdnIsBridgingToiCare.ClientID %>').val() == "1") {
                    $row = $(this).closest('tr').parent().closest('tr');
                    var entity = rowToObject($row);
                    var noKartu = entity.NHSRegistrationNo;
                    var nik = entity.SSN;
                    cbpView.PerformCallback('icare' + '|' + nik + '|' + noKartu);
                }
                else {
                    displayErrorMessageBox("WARNING", "Konfigurasi Bridging i-Care sedang non-aktif");
                }
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'icare') {
                if (param[1] == 'failed') {
                    displayErrorMessageBox("Save Failed", 'Error Message : ' + param[2]);
                }
                else {
                    var viewerUrl = param[2];
                    window.open(viewerUrl, "popupWindow", "width=1000, height=1000,scrollbars=yes");
                }
                    
            }
        }
    </script>
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianHFIS" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToiCare" runat="server" />
    <div style="position: relative; padding-top: 10px">
        <table style="width:30% ; padding-bottom: 10px" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tdLabel">
                    <%=GetLabel("Kode HFIS Dokter") %>
                </td>
                <td>
                    <asp:TextBox ID="txtPhysicianHFISCode" Width="50px" runat="server" ReadOnly=true/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <%=GetLabel("Nama HFIS Dokter") %>
                </td>
                <td>
                    <asp:TextBox ID="txtPhysicianHFISName" Width="300px" runat="server" ReadOnly=true/>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage" Style="height: 550px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                            <Columns>
                                <asp:BoundField DataField="MedicalNo" HeaderText="No. Rekam Medis" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PatientName" HeaderText="Nama" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IdentityNoType" HeaderText="Tipe Kartu" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SSN" HeaderText="NIK" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NHSRegistrationNo" HeaderText="No. Kartu BPJS" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DateOfBirthInString" HeaderText="Tgl. Lahir" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Gender" HeaderText="Jenis Kelamin" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <div>
                                                        <input type="button" class="btnGetICare" value='<%=GetLabel("Get Data I-Care") %>' />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <input type="hidden" value="<%#:Eval("NHSRegistrationNo") %>" bindingfield="NHSRegistrationNo" />
                                        <input type="hidden" value="<%#:Eval("SSN") %>" bindingfield="SSN" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan perawat untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
