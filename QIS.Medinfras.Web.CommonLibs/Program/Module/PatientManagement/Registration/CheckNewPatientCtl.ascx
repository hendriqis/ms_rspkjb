<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckNewPatientCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CheckNewPatientCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_checknewpatientctl">
    $(function () {
        setDatePicker('<%=txtSearchDOB.ClientID %>');
        $('#<%=txtSearchDOB.ClientID %>').datepicker('option', 'maxDate', '0');

        $('#<%:txtSearchPhoneNo.ClientID %>').keyup(function () {
            this.value = this.value.replace(/[^0-9\.]/g, '');
        });

        $('#btnCheckPatient').click(function () {
            var patientName = $('#<%:txtSearchName.ClientID %>').val();
            if (patientName != "") {
                cbpPatient.PerformCallback();
            } else {
                var messageBody = "Isi pencarian nama pasien terlebih dahulu.";
                displayMessageBox('SILAHKAN COBA LAGI', messageBody);
            }
        });

        $('#btnCheckClose').click(function () {
            var param = 'cekDuplikat' + '|' + $('#<%:txtSearchName.ClientID %>').val() + '|' + $('#<%:txtSearchAddress.ClientID %>').val() + '|' + $('#<%:txtSearchPhoneNo.ClientID %>').val() + '|' + $('#<%:txtSearchNIK.ClientID %>').val() + '|' + $('#<%:txtSearchMotherName.ClientID %>').val();
            if ($('#<%:hdnMotherParameter.ClientID %>').val() != '') {
                param = $('#<%:hdnMotherParameter.ClientID %>').val();
            }
            pcRightPanelContent.Hide();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/PatientEntryCtl.ascx");
            openUserControlPopup(url, param, 'Data Pasien', 1100, 550);
        });

    });

    function onCbpPatientEndCallback(evt) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnMotherParameter" value="0" runat="server" />
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <table class="tblContentArea">
        <tr>
         <h4 class="h4">
                    <%=GetLabel("Data Pasien")%></h4>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 1000px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 500px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchName" Width="100%" runat="server" CssClass="test" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Lahir")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchDOB" Width="108px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("NIK")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchNIK" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Alamat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchAddress" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No HP")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchPhoneNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Ibu")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchMotherName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>

                <input type="button" value='<%= GetLabel("Cari")%>' id="btnCheckPatient" style="width: 100px" />
                <input type="button" value='<%= GetLabel("Lanjut")%>' id="btnCheckClose" style="width: 100px" />
                <dxcp:ASPxCallbackPanel ID="cbpPatient" runat="server" Width="100%" ClientInstanceName="cbpPatient"
                    ShowLoadingPanel="false" OnCallback="cbpPatient_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdPatient" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" >
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SSN" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="No KTP" HeaderStyle-Width="120px" >
                                        <HeaderStyle HorizontalAlign="Left" Width="120px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NHSRegistrationNo" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderText="No BPJS" 
                                            HeaderStyle-Width="120px" >
                                        <HeaderStyle HorizontalAlign="Left" Width="120px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DateOfBirthInString" HeaderText="Tanggal Lahir" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" >
                                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StreetName" HeaderStyle-HorizontalAlign="Left" 
                                            HeaderText="Alamat" >
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PhoneNo1" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="No Telp" HeaderStyle-Width="100px" >
                                        <HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MobilePhoneNo1" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="No HP" HeaderStyle-Width="100px" >
                                        <HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MotherName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Nama Ibu" HeaderStyle-Width="100px" >
                                        <HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada data pasien tersebut")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
