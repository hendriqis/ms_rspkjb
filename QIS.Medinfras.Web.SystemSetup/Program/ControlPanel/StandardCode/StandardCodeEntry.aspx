<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" CodeBehind="StandardCodeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.StandardCodeEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Standard Code")%></div>--%>
    
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Standard Code ID")%></label></td>
                        <td>
                            <asp:TextBox ID="txtStandardCodeParentID" Width="100px" runat="server" />
                            ^
                            <asp:TextBox ID="txtStandardCodeChildID" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Standard Code Name")%></label></td>
                        <td><asp:TextBox ID="txtStandardCodeName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Tag Property")%></label></td>
                        <td><asp:TextBox ID="txtTagProperty" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("FHIR Reference")%></label></td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width:120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFHIRReferenceCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFHIRReferenceName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="margin-top:5px"><label><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtNotes" Width="300px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Is Mapping COA")%></label></td>
                        <td><asp:CheckBox ID="chkIsMapping" runat="server" Checked="false" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>