<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeParamedicMasterEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ChangeParamedicMasterEditCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChangeParamedicMasterEditCtl">
    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnParamedicID" value="" runat="server" />
<input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
<div style="height: 300px; overflow-y: auto">
    <fieldset id="fsEntryPopup" style="margin: 0">
        <table border="0">
            <tr>
                <td style="width: 100%">
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 35%" />
                                <col style="width: 65%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Kode Dokter / Paramedis")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtParamedicCode" Width="100px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Gelar Depan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboTitle" Width="110px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nama Dokter / Paramedis")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFirstName" Width="100%" runat="server" placeholder="First Name" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMiddleName" Width="100%" runat="server" placeholder="Middle Name" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Nama Belakang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFamilyName" Width="100%" runat="server" placeholder="Last Name"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Gelar Belakang")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboSuffix" Width="110px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Inisial")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInitial" Width="100px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jenis Kelamin")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboGender" Width="50%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
