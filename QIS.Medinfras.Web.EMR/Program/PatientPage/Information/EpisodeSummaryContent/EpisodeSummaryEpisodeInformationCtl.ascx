<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryEpisodeInformationCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryEpisodeInformationCtl" %>

<h3 class="headerContent">Episode Information</h3>
<style type="text/css">
.warnaTeks
{
    color:#AD3400;
}
.warnaHeader
{
    color:#064E73;
}
</style>
<div style="max-height:380px;overflow-y:auto">
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup style="width:150px"/>
        <colgroup style="width:20px"/>
        <tr>
            <td class="warnaHeader">Registration Date/Time</td>
            <td class="warnaHeader">:</td>
            <td><div id="lblRegistrationDateTime" runat="server"></div></td>
        </tr>
        <tr>
            <td class="warnaHeader">LOS</td>
            <td class="warnaHeader">:</td>
            <td><div id="lblLOS" runat="server"></div></td>
        </tr>
        <tr>
            <td class="warnaHeader">Unit Name</td>
            <td class="warnaHeader">:</td>
            <td><div id="lblUnitName" runat="server"></div></td>
        </tr>
        <tr>
            <td class="warnaHeader">Visit Type</td>
            <td class="warnaHeader">:</td>
            <td><div id="lblVisitType" runat="server"></div></td>
        </tr>
        <tr>
            <td class="warnaHeader">Purpose</td>
            <td class="warnaHeader">:</td>
            <td><div id="lblPurpose" runat="server"></div></td>
        </tr>
        <tr>
            <td class="warnaHeader">Chief Complaint</td>
            <td class="warnaHeader">:</td>
            <td><table cellpadding="0" cellspacing="0">
                <asp:Repeater ID="rptChiefComplaint" runat="server">
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <li style="list-style-type:none;"><%# Eval("ChiefComplaintText")%></li>
                                <div>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:80px" class="labelColumn">Quality</td>
                                        <td style="width:5px">:</td>
                                        <td style="width:110px" class="warnaTeks"><%# Eval("DisplayQuality")%></td>
                                        <td style="width:65px" class="labelColumn">Severity</td>
                                        <td style="width:5px">:</td>
                                        <td class="warnaTeks"><%# Eval("DisplaySeverity")%></td>
                                    </tr>
                                    <tr>
                                        <td class="labelColumn">Provocation</td>
                                        <td>:</td>
                                        <td class="warnaTeks"><%# Eval("DisplayProvocation")%></td>
                                        <td class="labelColumn">Time</td>
                                        <td>:</td>
                                        <td class="warnaTeks"><%# Eval("DisplayCourse")%></td>
                                    </tr>
                                    <tr>
                                        <td class="labelColumn">Relieved By</td>
                                        <td>:</td>
                                        <td class="warnaTeks"><%# Eval("DisplayRelieved")%></td>
                                    </tr>
                                </table>
                            </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="warnaHeader">Allergies</td>
            <td class="warnaHeader">:</td>
            <td>
                <asp:Repeater ID="rptAllergies" runat="server">
                    <HeaderTemplate>
                        <ul style="margin:0;">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li style="list-style-type:none; display: inline;">
                            <table cellpadding="0" cellspacing="0" style="margin-bottom:10px;">
                                <tr>
                                    <td style="width:95px" class="labelColumn">Allergen Type</td>
                                    <td style="width:5px">:</td>
                                    <td style="width:100px" class="warnaTeks"><%# Eval("AllergenType")%></td>
                                    <td style="width:90px" class="labelColumn">Allergy Source</td>
                                    <td style="width:5px">:</td>
                                    <td class="warnaTeks"><%# Eval("AllergySource")%></td>         
                                </tr>
                                <tr>
                                    <td class="labelColumn">Allergen Name</td>
                                    <td>:</td>
                                    <td class="warnaTeks"><%# Eval("Allergen")%></td>
                                    <td class="labelColumn">Severity</td>
                                    <td>:</td>
                                    <td class="warnaTeks"><%# Eval("AllergySeverity")%></td>
                                </tr>
                                <tr>
                                    <td class="labelColumn">Known Date</td>
                                    <td>:</td>
                                    <td class="warnaTeks"> <%# Eval("DisplayDate")%></td>
                                    <td class="labelColumn">Reaction</td>
                                    <td>:</td>
                                    <td class="warnaTeks"><%# Eval("Reaction")%></td>
                                </tr>
                            </table>  
                        </li>     
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
</div>
