<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NTNutritionistAssessmentCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.NTNutritionistAssessmentCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });
        registerCollapseExpandHandler();

        $('#leftPanel ul li').first().click();
    });

    function onCbpVitalSignViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
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
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value='0' />
    <input type="hidden" id="hdnNutritionAssessmentID" runat="server" value='0' />

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
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("ASESMEN AWAL PASIEN (GIZI)")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" title="Riwayat Gizi" class="w3-hover-red">Riwayat Gizi</li>
                        <li contentID="divPage3" title="Antropometri" class="w3-hover-red">Antropometri</li>
                        <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>
                        <li contentID="divPage5" title="Biokimia" class="w3-hover-red">Biokimia</li>
                        <li contentID="divPage6" title="Klinik/Fisik" class="w3-hover-red">Klinik/Fisik</li>
                        <li contentID="divPage7" title="Riwayat Personal" class="w3-hover-red">Riwayat Personal</li>
                        <li contentID="divPage8" title="Diagnosa Gizi" class="w3-hover-red">Diagnosa Gizi</li>
                        <li contentID="divPage9" title="Intervensi Gizi" class="w3-hover-red">Intervensi Gizi</li>
                        <li contentID="divPage10" title="Monitoring dan Evaluasi" class="w3-hover-red">Monitoring dan Evaluasi</li>
                    </ul>     
                </div> 
                <div>
                    <table class="w3-table-all" style="width:100%">
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class=" w3-small"><%=GetLabel("Dikaji Oleh :")%></div></td>
                        </tr>        
                        <tr>
                            <td style="text-align:left"><div id="lblAssessmentParamedicName" runat="server" class="w3-medium"></div></td>
                        </tr>                                                      
                    </table>
                </div>
            </td>
            <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display:none"> 
                    <table style="margin-top:5px; width:100%" cellpadding="0" cellspacing="0">
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
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <colgroup>
                            <col width="100%" />
                            <col width="100%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("Riwayat Gizi")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height: 400px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
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
                                                                <asp:TextBox ID="txtTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                                    ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Dikaji Oleh")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Gizi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNutritionHistory" Width="100%" runat="server" TextMode="MultiLine"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <td>
                                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Autoanamnesis" Checked="false"
                                                                Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                                                Checked="false" Enabled="false" />
                                                        </td>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>  
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display:none">                    
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="width: 150px; vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Antropometri")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAntropometricNotes" runat="server" Width="99%" TextMode="Multiline" Rows="6" ReadOnly="true"/>
                            </td>
                        </tr>
                    </table>     
                </div>    
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display:none">
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                            ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage4">
                                        <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdVitalSignView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <h3><%=GetLabel("Tanda Vital dan Indikator Lainnya")%></h3> 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                            <br />
                                                            <span style="font-style:italic">
                                                                <%#: Eval("Remarks") %>
                                                            </span>
                                                            <br />
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 350px;">
                                                                        <strong>
                                                                            <div style="width: 110px; float: left;" class="labelColumn">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                            <div style="width: 20px; float: left;">
                                                                                :</div>
                                                                        </strong>
                                                                        <div style="float: left;">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <br style="clear: both" />
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada pemeriksaan tanda vital") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="vitalSignPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage5" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                    <colgroup>
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                            <label class="lblNormal" id="lblBiochemistryNotes">
                                                <%=GetLabel("Biokimia") %>
                                            </label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBiochemistryNotes" runat="server" Width="98%" TextMode="Multiline"
                                                Rows="10" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display:none">
                   <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Klinik/Fisik") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicalNotes" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>  
                <div id="divPage7" class="w3-border divContent w3-animate-left" style="display:none"> 
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Riwayat Personal") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalHistory" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>    
                <div id="divPage8" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="lblNutritionProblem" runat="server">
                                    <%=GetLabel("Masalah") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProblem" runat="server" Width="98%" TextMode="Multiline" Rows="3" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label8">
                                    <%=GetLabel("Etiologi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEtiology" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label9">
                                    <%=GetLabel("Symptom") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSymptom" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage9" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Tujuan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInterventionPurpose" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label5">
                                    <%=GetLabel("Pemberian/Penentuan Diet") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInterventionDiet" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label4">
                                    <%=GetLabel("Edukasi/Konseling") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInterventionEducation" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Kolaborasi/Rujukan Gizi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInterventionCollaboration" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage10" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label6">
                                    <%=GetLabel("Monitoring") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMonitoring" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Evaluasi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEvaluation" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>         