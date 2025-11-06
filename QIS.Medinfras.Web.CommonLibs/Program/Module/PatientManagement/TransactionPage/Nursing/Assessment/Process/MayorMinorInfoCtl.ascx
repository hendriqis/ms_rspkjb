<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MayorMinorInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MayorMinorInfoCtl" %>
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
<input type="hidden" id="hdnTransactionIDCtl" runat="server" />
<input type="hidden" id="hdnNursingDiagnoseIDCtl" runat="server" />
<div style="height: 200px; overflow-y: auto">
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
                                <%=GetLabel("No. Asuhan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNursingTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td colspan="2">
                <h4 style="text-align: center">
                    <%=GetLabel("Persentase Data Mayor")%></h4>
            </td>
        </tr>
        <tr>
            <td>
                <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                    <b>
                        <%=GetLabel("Subjective")%></b>
                </div>
                <div runat="server" id="divSubjectiveMayorCtl" style="text-align: center; font-weight: bold;" />
            </td>
            <td>
                <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                    <b>
                        <%=GetLabel("Objective")%></b>
                </div>
                <div runat="server" id="divObjectiveMayorCtl" style="text-align: center; font-weight: bold;" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h4 style="text-align: center">
                    <%=GetLabel("Persentase Data Minor")%></h4>
            </td>
        </tr>
        <tr>
            <td>
                <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                    <b>
                        <%=GetLabel("Subjective")%></b>
                </div>
                <div runat="server" id="divSubjectiveMinorCtl" style="text-align: center; font-weight: bold;" />
            </td>
            <td>
                <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                    <b>
                        <%=GetLabel("Objective")%></b>
                </div>
                <div runat="server" id="divObjectiveMinorCtl" style="text-align: center; font-weight: bold;" />
            </td>
        </tr>
    </table>
</div>
