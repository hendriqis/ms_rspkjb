<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSurgeryReportCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ViewSurgeryReportCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_viewPreOpAssessmentCtl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#<%=grdProcedureGroupView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdProcedureGroupView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

        registerCollapseExpandHandler();

        $('#leftPanel ul li').first().click();
    });


    //#region Implant
    function oncbpImplantViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdImplantView.ClientID %> tr:eq(1)').click();

            setPaging($("#implantPaging"), pageCount, function (page) {
                cbpImplantView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdImplantView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onCbpProcedureGroupViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

            setPaging($("#procedureGroupPaging"), pageCount, function (page) {
                cbpProcedureGroupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
    }

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
</script>
<style type="text/css">
    #leftPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
    #leftPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
    #leftPanel > ul > li    { list-style-type: none; font-size: 15px; display:list-item; border: 1px solid #fdf5e6!important; padding: 5px 8px; cursor: pointer; background-color:#87CEEB!important; }
    #leftPanel > ul > li.selected { background-color: #ff5722!important; color: White; }   
    .divContent { padding-left: 3px; min-height:490px;} 
</style>
<div style="width:100%;">
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <input type="hidden" id="hdnVisitID" runat="server" value='0' />
    <input type="hidden" id="hdnTestOrderID" runat="server" value='0' />
    <input type="hidden" id="hdnReportID" runat="server" value='0' />
    <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
        <colgroup>
            <col style="width:300px" />
            <col />
        </colgroup>
        <tr>
            <td>
               <div id="lblMedicalNo" runat="server" class="w3-lime w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444; width:100%"></div>
            </td>
            <td>
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("LAPORAN OPERASI")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" title="Data Operasi" class="w3-hover-red">Data Operasi</li>
                        <li contentID="divPage3" title="Diagnosa dan Jenis Tindakan Operasi" class="w3-hover-red">Diagnosa dan Jenis Tindakan Operasi</li>
                        <li contentID="divPage4" title="Team Pelaksana" class="w3-hover-red">Team Pelaksana</li>
                        <li contentID="divPage5" title="Pemasangan Implant" class="w3-hover-red">Pemasangan Implant</li>
                        <li contentID="divPage6" title="Uraian Pemebedahan" class="w3-hover-red">Uraian Pembedahan</li>
                    </ul>     
                </div> 
                <div>
                    <table class="w3-table-all" style="width:100%">
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class=" w3-small"><%=GetLabel("Dibuat Oleh :")%></div></td>
                        </tr>        
                        <tr>
                            <td style="text-align:left"><div id="lblPhysicianName2" runat="server" class="w3-medium"></div></td>
                        </tr>                                                         
                    </table>
                </div>
            </td>
            <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display:none"> 
                    <table style="margin-top:5px; width:100%" cellpadding="0" border="0" cellspacing="0">
                        <colgroup style="width:130px"/>
                            <colgroup style="width:10px; text-align: center"/>
                            <colgroup />
                        <colgroup style="width:130px"/>
                        <tr>
                            <td style="vertical-align:top">
                                <img style="width:110px;height:125px" runat="server" runat="server" id="imgPatientImage" />
                            </td>
                            <td />
                            <td>
                                <table border = "0" cellpadding="0" cellspacing="0">
                                    <colgroup style="width:160px"/>
                                    <colgroup style="width:10px; text-align: center"/>
                                    <colgroup />   
                                    <tr>
                                        <td colspan="3" style="width:100%"><span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold; width:100%"></span></td>
                                    </tr> 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Jenis Kelamin")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblGender" runat="server" style="color:Black"></span></td>
                                    </tr>                                 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Tanggal Lahir (Umur)")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblDateOfBirth" runat="server" style="color:Black"></span></td>
                                    </tr>       
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Tanggal & Jam Registrasi")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><div id="lblRegistrationDateTime" runat="server" style="color:Black"></div></td>
                                    </tr>      
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("No. Registrasi")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><div id="lblRegistrationNo" runat="server" style="color:Black"></div></td>
                                    </tr> 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("DPJP Utama")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPhysician" runat="server" style="color:Black"></span></td>
                                    </tr>                  
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Pembayar")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPayerInformation" runat="server" style="color:Black"></span></td>
                                    </tr>     
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Lokasi Pasien")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPatientLocation" runat="server" style="color:Black"></span></td>
                                    </tr>          
                                    <tr>
                                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel("Diagnosa")%></td>
                                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel(":")%></td>
                                        <td>
                                            <textarea id="lblDiagnosis" runat="server" style="border:0; width:100%; height:120px; background-color: transparent" readonly></textarea>
                                        </td>
                                    </tr>                                                                                                                                                                                                                                                          
                                </table>
                            </td>
                            <td style="vertical-align:top">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divContent w3-animate-left">
                    <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                        <colgroup>
                            <col style="width: 55%" />
                            <col style="width: 45%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="150px" />
                                                    <col width="100px" />
                                                    <col width="150px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblOrderNo">
                                                            <%:GetLabel("Nomor Order")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtTestOrderNo" Width="150px" runat="server" Enabled="false" />
                                                    </td>
                                                </tr> 
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblMandatory">
                                                            <%=GetLabel("Tanggal Laporan")%></label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col width="150px" />
                                                                <col width="150px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtReportDate" Width="120px" runat="server" ReadOnly="true" style="text-align:center" />
                                                                </td>
                                                                <td class="tdLabel" style="padding-left : 5px">
                                                                    <label class="lblMandatory">
                                                                        <%=GetLabel("Jam Laporan")%></label>
                                                                </td>
                                                                <td style="padding-left : 5px">
                                                                    <asp:TextBox ID="txtReportTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblMandatory">
                                                            <%=GetLabel("Tanggal Operasi")%></label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col width="150px" />
                                                                <col width="150px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtStartDate" Width="120px" runat="server" ReadOnly="true" style="text-align:center" />
                                                                </td>
                                                                <td class="tdLabel" style="padding-left : 5px">
                                                                    <label class="lblMandatory">
                                                                        <%=GetLabel("Jam Mulai Operasi")%></label>
                                                                </td>
                                                                <td style="padding-left : 5px">
                                                                    <asp:TextBox ID="txtStartTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel"><%=GetLabel("Lama Operasi") %></td>
                                                    <td><asp:TextBox ID="txtDuration" Width="60px" CssClass="number" runat="server" ReadOnly="true" /> menit</td>                                              
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Operasi Ke-")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border ="0" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblSurgeryNoType" CssClass="rblSurgeryNoType" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" 1 (Pertama)" Value="1" />
                                                                        <asp:ListItem Text=" Re-Do" Value="0" />
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSurgeryNo" Width="50px" runat="server" Style="text-align: right" Enabled="false" />
                                                                </td>                                      
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal" id="lblFamilyRelation">
                                                            <%=GetLabel("Cara Pembiusan")%></label>
                                                    </td>
                                                    <td style="width: 130px">
                                                        <dxe:ASPxComboBox runat="server" ID="cboAnesthesiaType" ClientInstanceName="cboAnesthesiaType"
                                                            Width="100%" Enabled="false" />
                                                    </td>
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal" id="Label4">
                                                            <%=GetLabel("Jenis Pembedahan")%></label>
                                                    </td>
                                                    <td style="width: 130px">
                                                        <dxe:ASPxComboBox runat="server" ID="cboWoundType" ClientInstanceName="cboWoundType"
                                                            Width="100%" Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Antibiotika Profilaksis")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="1" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsUsingProfilaksis" CssClass="rblIsUsingProfilaksis" runat="server" Enabled="false"
                                                                        RepeatDirection="Horizontal" CellPadding="2">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                    <asp:TextBox ID="txtProfilaxis" runat="server" Width="200px" Enabled="false" />
                                                                </td>  
                                                                <td style="padding-left:2px">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Waktu Pemberian")%></label>
                                                                </td>        
                                                                <td style="padding-left:5px">
                                                                    <asp:TextBox ID="txtProfilaxisTime" Width="50px" runat="server" Style="text-align: center" Enabled="false" />
                                                                </td>                                                                                                                                                                                                             
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Komplikasi/Penyulit")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsHasComplexity" CssClass="rblIsHasComplexity" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                    <asp:TextBox ID="txtComplexityRemarks" runat="server" Width="99%" Enabled="false" />
                                                                </td>                                                                                                                                                                                                           
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Perdarahan")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsHemorrhage" CssClass="rblIsHemorrhage" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                    <asp:TextBox ID="txtHemorrhage" runat="server" Width="60px" Style="text-align: right" Enabled="false" /> ml
                                                                </td>                                                                                                                                                                                                           
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Drain")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsBloodDrain" CssClass="rblIsBloodDrain" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                    <asp:TextBox ID="txtOtherBloodDrainType" runat="server" Width="99%" Enabled="false" />
                                                                </td>                                                                                                                                                                                                           
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tampon")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsUsingTampon" CssClass="rblIsUsingTampon" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                    <asp:TextBox ID="txtTamponType" runat="server" Width="99%" Enabled="false" />
                                                                </td>                                                                                                                                                                                                           
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tourniquet")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsUsingTourniquet" CssClass="rblIsUsingTourniquet" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                </td>                                                                                                                                                                                                           
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Transfusi")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsBloodTransfussion" CssClass="rblIsBloodTransfussion" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td>
                                                                    <asp:TextBox ID="txtBloodTransfussion" runat="server" Width="60px" Style="text-align: right" Enabled="false" /> ml
                                                                </td>                                                                                                                                                                                                           
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Pemeriksaan Kultur")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsTestKultur" CssClass="rblIsTestKultur" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>   
                                                                <td>
                                                                    <asp:TextBox ID="txtOtherTestKulturType" runat="server" Width="99%" Enabled="false" />
                                                                </td>                                                                                                                                                                                                        
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Pemeriksaan Sitologi")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsTestCytology" CssClass="rblIsTestCytology" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>  
                                                                <td>
                                                                    <asp:TextBox ID="txtOtherTestCytologyType" runat="server" Width="99%" Enabled="false" />
                                                                </td>                                                                                                                                                                                                       
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Pemeriksaan Jaringan ke PA")%></label>
                                                    </td>
                                                    <td colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <colgroup>
                                                                <col width="100px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblIsSpecimenTest" CssClass="rblIsSpecimenTest" runat="server"
                                                                        RepeatDirection="Horizontal" CellPadding="2" Enabled="false">
                                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                                    </asp:RadioButtonList>
                                                                </td>    
                                                                <td class="tdLabel" style="width: 120px">
                                                                    <label class="lblNormal" id="Label4">
                                                                        <%=GetLabel("Jenis Spesimen")%></label>
                                                                </td>
                                                                <td style="width: 130px">
                                                                    <dxe:ASPxComboBox runat="server" ID="cboSpecimen" ClientInstanceName="cboSpecimen"
                                                                        Width="100%" Enabled="false" />
                                                                </td>                                                                                                                                                                                                    
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align:top">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="divContent w3-animate-left">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="150px" />
                                        <col width="100px" />
                                        <col width="100px" />
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Pre Diagnosis")%></label>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtPreDiagnosis" runat="server" Width="99%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Post Diagnosis")%></label>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtPostDiagnosis" runat="server" Width="99%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <br />
                                            <label class="lblNormal">
                                                <%=GetLabel("Jenis/Tindakan Operasi :")%></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <dxcp:ASPxCallbackPanel ID="cbpProcedureGroupView" runat="server" Width="100%" ClientInstanceName="cbpProcedureGroupView"
                                                ShowLoadingPanel="false" OnCallback="cbpProcedureGroupView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent6" runat="server">
                                                        <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 180px">
                                                            <asp:GridView ID="grdProcedureGroupView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureGroupID") %>" bindingfield="ProcedureGroupID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureGroupCode") %>" bindingfield="ProcedureGroupCode" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureGroupName") %>" bindingfield="ProcedureGroupName" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Jenis Operasi")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div><%#: Eval("ProcedureGroupName")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Kategori Operasi"  DataField="SurgeryClassification" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging" style="display:none">
                                                <div class="wrapperPaging">
                                                    <div id="procedureGroupPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; vertical-align:top">
                                            <label class="lblNormal">
                                                <%=GetLabel("Catatan Jenis Operasi")%></label>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtProcedureGroupRemarks" runat="server" Width="100%" TextMode="Multiline"
                                                Rows="3" ReadOnly="true" />     
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left"> 
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <div>
                                    <dxcp:ASPxCallbackPanel ID="cbpParamedicTeamView" runat="server" Width="100%" ClientInstanceName="cbpParamedicTeamView"
                                        ShowLoadingPanel="false" OnCallback="cbpParamedicTeamView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ oncbpParamedicTeamViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 150px">
                                                    <asp:GridView ID="grdParamedicTeamView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                                    <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Tenaga Medis" 
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" HeaderStyle-Width="150px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data team pelaksana untuk order ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging" style="display:none">
                                        <div class="wrapperPaging">
                                            <div id="paramedicPaging">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Konsultasi Intra Operatif") %></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtReferralSummary" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="5" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage5" class="w3-border divContent w3-animate-left"> 
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col width="100px" />
                                        <col width="100px" />
                                        <col width="100px" />
                                        <col width="100px" />
                                        <col width="100px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td colspan="6">
                                            <asp:CheckBox ID="chkIsUsingImplant" runat="server" Text=" Dilakukan Pemasangan Implant"
                                                Checked="false" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dxcp:ASPxCallbackPanel ID="cbpImplantView" runat="server" Width="100%" ClientInstanceName="cbpImplantView"
                                    ShowLoadingPanel="false" OnCallback="cbpImplantView_Callback">
                                    <ClientSideEvents EndCallback="function(s,e){ oncbpImplantViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                <asp:GridView ID="grdImplantView" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                                <input type="hidden" value="<%#:Eval("ItemName") %>" bindingfield="ItemName" />
                                                                <input type="hidden" value="<%#:Eval("SerialNumber") %>" bindingfield="SerialNumber" />
                                                                <input type="hidden" value="<%#:Eval("cfImplantDate") %>" bindingfield="cfImplantDate" />
                                                                <input type="hidden" value="<%#:Eval("cfReviewDate") %>" bindingfield="cfReviewDate" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="Nama Item" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" HeaderStyle-Width="200px"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="cfImplantDate" HeaderText="Tanggal Pemasangan" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada informasi pemasangan implant untuk tindakan operasi di pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="containerPaging" style="display:none">
                                    <div class="wrapperPaging">
                                        <div id="implantPaging">
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage6" class="divContent w3-animate-left">
                   <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                        <colgroup>
                            <col style="width:150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="lblSurgeryRemarks">
                                    <%=GetLabel("Uraian Pembedahan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="24" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>    
            </td>
        </tr>
    </table>
</div>         