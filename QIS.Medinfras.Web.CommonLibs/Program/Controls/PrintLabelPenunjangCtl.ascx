<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintLabelPenunjangCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrintLabelPenunjangCtl" %>
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


    

    function getCheckedData() {
        var lstSelectedReportID = $('#<%=hdnSelectedID.ClientID %>').val().split(','); 
        var result = '';
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var DtID = $tr.find('.keyField').val();
            }

        });

        if (result != "") {
            result = result.substring(0, result.length - 1);
            if (result != "") {
                $('#<%=hdnSelectedID.ClientID %>').val(result);
            } else {
                $('#<%=hdnSelectedID.ClientID %>').val("");
            }

        } else {
            $('#<%=hdnSelectedID.ClientID %>').val("");
        }
        
    }
    $('.btnPrint').live('click', function () {
        var id = $(this).attr('data-id');
        var hdnSelectedID = $('#<%=hdnSelectedID.ClientID %>').val(id);
        if (hdnSelectedID != "") {
            cbpPrintProcess.PerformCallback('print');
             
        } else {
            showToast('Silahkan dipilih dahulu yang akan diprint.');
        }
    });
   
    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedDate = "";
        $('.chkIsSelectedDetail input:checked').each(function () {
            var id = $(this).closest('tr').find('.keyField').html();
            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }
            tempSelectedID += id;
           
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            alert(tempSelectedID);
            return true;
        }
        else return false;
    }
    function onCbpPrintProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'print') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            cbpPrintProcess.PerformCallback('refresh');
        }
        hideLoadingPanel();
    }
</script>
<input type="hidden" runat="server" id="hdnPatientChargesIDLabelCtl" value="0" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" /> 
<div style="padding: 10px;">
     <div>
     <dxcp:ASPxCallbackPanel ID="cbpPrintProcess" runat="server" Width="100%" ClientInstanceName="cbpPrintProcess"
                    ShowLoadingPanel="false" OnCallback="cbpPrintProcess_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPrintProcessEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlDocumentNotesGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                 <input type="hidden" id="hdnSelectedReportID" runat="server" value="0" />
                                    <input type="hidden" id="hdnSelectedReportCode" runat="server" value="0" />
                                <asp:GridView ID="grdViewLabel" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                     <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                      <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                           
                                            <ItemTemplate>
                                               <input type="button" value="PRINT LABEL" class="btnPrint" data-id="<%#:Eval("ID") %>" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="keyField" class="hdnID" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" class="hdnItemName1" />
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                       
                                        
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
