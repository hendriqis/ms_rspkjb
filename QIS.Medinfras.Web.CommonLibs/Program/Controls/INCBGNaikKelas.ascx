<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="INCBGNaikKelas.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.INCBGNaikKelas" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">

    

    $('#btnRegistrationJknCancel').click(function () {
        $('#containerPatientVisitNotesEntryData').hide();
    });
    
    $('#btnRegistrationJknSave').click(function (evt) {
        if (IsValid(evt, 'fsPatientVisitNotes', 'mpPatientVisitNotes')) {
            cbpRegistrationJkn.PerformCallback('save');
        }
        else {
            return false;
            
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
 
        var filterExpression = "ID = '" + entity.ID + "'";
        Methods.getObject('GetvRegistrationJKNList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtClassCode.ClientID %>').val(result.ClassCode);
                $('#<%=txtClassName.ClientID %>').val(result.ClassName);
                $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                $('#<%=hdnID.ClientID %>').val(result.ID);
                $('#<%=txtNilaiHakKelas.ClientID %>').val(result.NilaiHakKelas);
                $('#<%:chkIuran.ClientID %>').prop('checked',result.IsNilaiIur);  
            }
        });

    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpRegistrationJkn.PerformCallback('delete');
            }
        });
    });
    function ResetForm() {
        $('#<%=hdnID.ClientID %>').val(0);
        $('#<%=txtNilaiHakKelas.ClientID %>').val("");
        $('#<%=txtClassCode.ClientID %>').val("");
        $('#<%=txtClassName.ClientID %>').val("");
        $('#<%=hdnClassID.ClientID %>').val(0);
        $('#<%:chkIuran.ClientID %>').val(false);
    } 
    function onRegistrationJknEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPatientVisitNotesEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
        ResetForm();
    }

    $('#lblClassCare.lblLink').live('click', function () {
        var hdnRegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var filterexpression = "IsDeleted=0 AND IsBPJSClass=1";

        if (!$('#<%:chkIuran.ClientID %>').is(':checked')) {
            filterexpression += " AND ClassID NOT IN (SELECT ClassID FROM RegistrationJKN WHERE RegistrationID = " + hdnRegistrationID + " AND IsDeleted=0)";
        }
        openSearchDialog('classcare', filterexpression, function (value) {
            $('#<%=txtMRN.ClientID %>').val(value);
            onClasscareChanged(value);
        });
    });
    function onClasscareChanged(value) {

        var filterExpression = "ClassCode = '" + value + "' AND IsDeleted=0 AND IsBPJSClass=1";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtClassCode.ClientID %>').val(result.ClassCode);
                $('#<%=txtClassName.ClientID %>').val(result.ClassName);
                $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
            }
       });
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="0" id="hdnID" runat="server" />
                            
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="160px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                 <div id="containerRegistrationJKNEntryData">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnVisitNoteID" runat="server" value="" />
                    <fieldset id="fsPatientVisitNotes" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 300px" />
                            </colgroup>
                             <tr>
                                <td class="tdLabel">
                                  
                                </td>
                                <td>
                                   <asp:CheckBox ID="chkIuran"   runat="server" /> <%=GetLabel("Iuran Max 75% INACBG")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdlabel">
                                    <label class="lblMandatory lblLink" id="lblClassCare" >
                                        <%=GetLabel("Kelas Perawatan")%></label>
                                </td>
                                <td> <input type="hidden" runat="server" id="hdnClassID" /> 
                                                <asp:TextBox ID="txtClassCode" Width="50px" CssClass="required" runat="server" /> 
                                              <asp:TextBox ID="txtClassName" Width="200px" CssClass="required" runat="server" readonly/></td>
                                </td>
                                 
                            </tr>
                            
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory" >
                                        <%=GetLabel("Nilai Kelas Kelas")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNilaiHakKelas" Width="50%" CssClass="required" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnRegistrationJknSave" value='<%= GetLabel("Simpan")%>' onclick="btnSave()" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnRegistrationJknCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpRegistrationJkn" runat="server" Width="100%" ClientInstanceName="cbpRegistrationJkn"
                    ShowLoadingPanel="false" OnCallback="cbpRegistrationJkn_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onRegistrationJknEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdVisitNotes" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ClassID") %>" bindingfield="ClassID" />
                                                <input type="hidden" value="<%#:Eval("NilaiHakKelas") %>" bindingfield="NilaiHakKelas" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="100px" DataField="RegistrationNo" HeaderText="Nomor Registrasi" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="100px" DataField="ClassName" HeaderText="Kelas" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="80px" DataField="cfNilaiHakKelas" HeaderText="Nilai Hak Kelas" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderStyle-Width="80px" DataField="cfIsNilaiIur" HeaderText="Iuran Max 75% INACBG "/>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
