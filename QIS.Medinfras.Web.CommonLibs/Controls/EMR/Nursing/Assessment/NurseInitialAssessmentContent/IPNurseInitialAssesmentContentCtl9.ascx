<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPNurseInitialAssesmentContentCtl9.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.IPNurseInitialAssesmentContentCtl9" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl9">
    $(function () {
        registerCollapseExpandHandler();
    });
</script>

<style type="text/css">
    .warnaTeks { color:#AD3400;}
    .warnaHeader { color:#064E73;}    
    li  { list-style-type:none; font-size: 14px;list-style-position:inside;margin:0;padding:0; }   
    .divContent { font-size:14px;}
    .divNotAvailableContent { margin-left:25px; font-size:11px; font-style:italic; color:red}
</style>

<h4 class="w3-blue h4expanded">
    <%=GetLabel("Keluhan Utama Pasien")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>        
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal dan Waktu")%></label>
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("DPJP")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Keluhan Utama")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtChiefComplaint" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </div>
</div>

<h4 class="w3-blue h4expanded">
    <%=GetLabel("Keluhan lain yang menyertai")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>        
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Keluhan lain yang menyertai")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtHPISummary" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    &nbsp;
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <td>
                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Autoanamnesis" Checked="false" Enabled = "false" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                Checked="false" Enabled = "false" />
                        </td>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>

