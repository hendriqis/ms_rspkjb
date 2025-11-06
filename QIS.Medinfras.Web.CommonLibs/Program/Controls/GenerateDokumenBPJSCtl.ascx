<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateDokumenBPJSCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GenerateDokumenBPJSCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_SuratPenagihan">

    $(function () {
        hideLoadingPanel();
    });


    function onCbpReportProcessEndCallback(s) {

        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == "fail") {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            } else {
             
                showToast('Process Success');
            }
        }

        pcRightPanelContent.Hide();

    }


    $('#chkSelectAllDetail').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedDetail input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
      
    });

    function getCheckedData() {
        var lstSelectedReportID = $('#<%=hdnSelectedReportID.ClientID %>').val().split(',');
        var lstSelectedReportCode = $('#<%=hdnSelectedReportCode.ClientID %>').val().split(',');
      
        var result = '';
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var DtID = $tr.find('.keyField').val();
                var hdnReportCode = $tr.find('.hdnReportCode').val();
                result += hdnReportCode + "^";

            }

        });

        if (result != "") {
            result = result.substring(0, result.length - 1);
            if (result != "") {
                $('#<%=hdnReportCode.ClientID %>').val(result);
            } else {
                $('#<%=hdnReportCode.ClientID %>').val("");
            }

        } else {
            $('#<%=hdnReportCode.ClientID %>').val("");
        }
        
    }

    $('#<%=btnProcess.ClientID %>').click(function () {
        getCheckedData();
        var ReportCode = $('#<%=hdnReportCode.ClientID %>').val();
        if (ReportCode != "") {
            cbpReportProcess.PerformCallback('save');
        } else {
            showToast('Silahkan dipilih dahulu report code yang akan diproses.');
        }
       
    });
</script>
<input type="hidden" runat="server" id="hdnRegistrationID" value="0" />
<input type="hidden" runat="server" id="hdnMenuIDCtl" value="0" />
<input type="hidden" runat="server" id="hdnReportCode" value="" />
<input type="hidden" runat="server" id="hdnSepNo" value="" />
<input type="hidden" runat="server" id="hdnNoPeserta" value="" />
<input type="hidden" runat="server" id="hdnRegistrationNo" value="" />
<div style="padding: 10px;">
     <div>
        <table>
            <tbody>
                <tr>
                    <th>Registration No</th>
                    <td><asp:TextBox runat="server" ID="txtRegistrationNo" Width="150px" ReadOnly=true /></td>
                </tr>
                 <tr>
                    <th>Nomor Peserta</th>
                    <td><asp:TextBox runat="server" ID="txtPeserta" Width="150px" ReadOnly=true /></td>
                </tr>
                 <tr>
                    <th>Nomor SEP</th>
                    <td><asp:TextBox runat="server" ID="txtSepNo" Width="150px" ReadOnly=true /></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="button" id="btnProcess" runat="server" value="Process" class="btn btn-w3" /></td>
                </tr>
            </tbody>
        </table>
     </div>
     <div>
     <dxcp:ASPxCallbackPanel ID="cbpReportProcess" runat="server" Width="100%" ClientInstanceName="cbpReportProcess"
                    ShowLoadingPanel="false" OnCallback="cbpReportProcess_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpReportProcessEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlDocumentNotesGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                 <input type="hidden" id="hdnSelectedReportID" runat="server" value="0" />
                                    <input type="hidden" id="hdnSelectedReportCode" runat="server" value="0" />
                                <asp:GridView ID="grdReportMaster" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                     
                                  
                                      <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                            <center> <input id="chkSelectAllDetail" type="checkbox" />
                                            </center>
                                           
                                           </HeaderTemplate>
                                            <ItemTemplate>
                                               <asp:CheckBox ID="chkIsSelectedDetail" runat="server" CssClass="chkIsSelectedDetail" />
                                                <input type="hidden" value="<%#:Eval("ReportID") %>" bindingfield="ID" class="hdnReportID" />
                                                <input type="hidden" value="<%#:Eval("ReportCode") %>" bindingfield="MenuCode" class="hdnReportCode" />
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:BoundField DataField="ReportCode" HeaderText="Report Code" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ReportTitle1" HeaderText="Report Name" HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingDocumentNote">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
     </div>
</div>
