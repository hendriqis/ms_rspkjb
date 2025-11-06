<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientEducationListCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.PatientEducationListCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $(function () {
        $(".rblEducationTypeStatus input").change(function () {
            $txt = $(this).closest('tr').parent().closest('tr').find('.txtFreeText');
            if ($(this).val() == '1')
                $txt.show();
            else
                $txt.hide();
        });
        $(".rblEducationTypeStatus").each(function () {
            $(this).find('input[checked=checked]').change();
        });

        $('#<%=rblIsPatientFamily.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trFamilyInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trFamilyInfo.ClientID %>').attr("style", "display:none");
            }
        });
    });

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
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" contentIndex="1" title="Informasi Umum" class="w3-hover-red">Informasi Umum</li>
                        <li contentID="divPage2" contentIndex="2" title="Materi Edukasi" class="w3-hover-red">Materi Edukasi</li>
                    </ul>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width:180px"/>
                            <col style="width:150px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam Edukasi")%></label></td>
                            <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                            <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pemberi Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Penerima Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pasien" Value="0" Selected="True" />
                                    <asp:ListItem Text="Keluarga / Lain-lain" Value="1"  />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trFamilyInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Penerima")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:140px"/>
                                        <col style="width:80px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSignature2Name" CssClass="txtSignature2Name" runat="server" Width="100%"  />
                                        </td>
                                        <td class="tdLabel" style="padding-left:5px">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Hubungan")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                Width="99%" ToolTip = "Hubungan dengan Pasien" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>    
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tingkat Pemahaman Awal")%></label></td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCUnderstandingLevel" ClientInstanceName="cboGCUnderstandingLevel"
                                    Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Metode Edukasi")%></label></td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCEducationMethod" ClientInstanceName="cboGCEducationMethod"
                                    Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Material Edukasi")%></label></td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCEducationMaterial" ClientInstanceName="cboGCEducationMaterial"
                                    Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Evaluasi Edukasi")%></label></td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCEducationEvaluation" ClientInstanceName="cboGCEducationEvaluation"
                                    Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                  </table>             
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <div style="width: 99%; height: auto; max-height : 400px; min-height: 300px; border:0px solid #AAA; overflow-y: auto; overflow-x:hidden; padding-left: 10px;">
                        <asp:Repeater ID="rptEducationType" runat="server">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" style="width:98%">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="labelColumn" style="width:250px;vertical-align:top;padding-top:5px">
                                        <input type="hidden" id="hdnGCEducationType" runat="server" value='<%#:Eval("StandardCodeID") %>' />
                                        <%#: Eval("StandardCodeName") %>
                                    </td>
                                    <td>
                                        <div style="padding-left:1px">
                                            <asp:RadioButtonList ID="rblEducationTypeStatus" CssClass="rblEducationTypeStatus" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Ya" Value="1"/>
                                                <asp:ListItem Text=" Tidak" Value="0" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div style="padding-left:5px">
                                            <asp:TextBox ID="txtFreeText" Style="display:none" CssClass="txtFreeText" runat="server" Width="370px" TextMode="Multiline" Rows="2" />
                                        </div>
                                    </td>
                                </tr>                
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
