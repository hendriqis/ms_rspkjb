<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientUseDetailDrugMSReturnCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientUseDetailDrugMSReturnCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_ptdrgmsctl">
    function onLoadDrugMSReturn() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblDrugMSReturnQuickPick').show();
        }
        else {
            $('#lblDrugMSReturnQuickPick').hide();
        }

        $('#btnDrugMSReturnSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxDrugMSReturn', 'mpTrxDrugMSReturn'))
                cbpDrugMSReturn.PerformCallback('save');
            return false;
        });

        $('#btnDrugMSReturnCancel').click(function () {
            $('#containerEntryDrugMSReturn').hide();
        });

        $('#lblDrugMSReturnQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/DrugsReturnQuickPicksCtl.ascx');
                var visitID = getVisitID();
                var locationID = getLocationID();
                var transactionID = getTransactionHdID();
                var departmentID = getDepartmentID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = visitID + '|' + locationID + '|' + transactionID + '|' + departmentID + '|' + serviceUnitID + '|' + registrationID + '|' + isAccompany;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });
    }

    $('.imgDrugMSReturnApprove.imgLink').die('click');
    $('.imgDrugMSReturnApprove.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpDrugMSReturn.PerformCallback('approve|' + obj.ID);
    });

    $('.imgDrugMSReturnVoid.imgLink').die('click');
    $('.imgDrugMSReturnVoid.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpDrugMSReturn.PerformCallback('void|' + obj.ID);
    });

    $('.imgDrugMSReturnDelete.imgLink').die('click');
    $('.imgDrugMSReturnDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpDrugMSReturn.PerformCallback(param);
        });
    });

    function onCbpDrugMSReturnEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'void') {
            if (param[1] == 'fail')
                showToast('Void Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Approve Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnIsEditable" runat="server" value="" />

<input type="hidden" id="hdnDrugMSReturnTransactionDtID" runat="server" value="" />
<div id="containerEntryDrugMSReturn" style="margin-top:4px;display:none;">
    <div class="pageTitle"><%=GetLabel("Tambah Atau Ubah Data")%></div>
    <fieldset id="fsTrxDrugMSReturn" style="margin:0"> 
        <table class="tblEntryDetail">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width:100px"/>
                            <col style="width:200px"/>
                            <col style="width:200px"/>
                            <col style="width:200px"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Lokasi")%></label></td>
                            <td colspan="3"><asp:TextBox ID="txtDrugMSReturnLocationName" runat="server" Width="100%" ReadOnly="true" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Obat")%></label></td>
                            <td colspan="3">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtDrugMSReturnItemCode" ReadOnly="true" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtDrugMSReturnItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Digunakan")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Satuan")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSReturnQtyUsed" Width="100%" ReadOnly="true" CssClass="number" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSReturnQtyUoM" Width="100%" ReadOnly="true" CssClass="number" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Jumlah")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Konversi")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah Satuan Kecil")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSReturnBaseQty" ReadOnly="true" CssClass="number" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSReturnConversion" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <input type="button" id="btnDrugMSReturnSave" value='<%= GetLabel("Save")%>' />
                            </td>
                            <td>
                                <input type="button" id="btnDrugMSReturnCancel" value='<%= GetLabel("Cancel")%>' />
                            </td>
                        </tr>
                    </table>
                    <img style="float:left;margin-right: 10px;" src='<%= ResolveUrl("~/Libs/Images/Button/info.png")%>' alt='' />
                    <label class="lblInfo">Obat dan Alkes yang di-Save akan langsung di-Approve.</label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>

<dxcp:ASPxCallbackPanel ID="cbpDrugMSReturn" runat="server" Width="100%" ClientInstanceName="cbpDrugMSReturn"
    ShowLoadingPanel="false" OnCallback="cbpDrugMSReturn_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpDrugMSReturnEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                <input type="hidden" id="hdnDrugMSReturnAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnDrugMSReturnAllTotalPayer" runat="server" value="" />
                <asp:ListView ID="lvwDrugMSReturn" runat="server">
                    <LayoutTemplate>                                
                        <table id="tblView" runat="server" class="grdDrugMSReturn grdNormal notAllowSelect" cellspacing="0" rules="all" >
                            <tr> 
                                <th style="width:80px" rowspan="2"></th>
                                <th rowspan="2">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th colspan="2" style="width:200px"><%=GetLabel("JUMLAH")%></th>
                                <th rowspan="2" style="width:230px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Petugas")%>
                                    </div>
                                </th>                            
                            </tr>
                            <tr>                                
                                <th style="width:100px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Digunakan")%>
                                    </div>
                                </th>
                                <th style="width:100px">
                                    <div style="text-align:left;padding-right:3px">
                                        <%=GetLabel("Satuan")%>
                                    </div>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:24px">
                                                <img class="imgDrugMSReturnVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%> title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>' alt="" />
                                                <img class="imgDrugMSReturnApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%> title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>' alt="" />
                                                <img class="imgDrugMSReturnVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%> title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>' alt="" />
                                            </td>
                                            <td style="width:1px">&nbsp;</td>
                                            <td style="width:24px"><img class="imgDrugMSReturnDelete imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%> title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                    <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />                                    
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px">
                                    <div><%#: Eval("ItemName1")%></div>          
                                    <div><span style="font-style:italic"><%#: Eval("ItemCode") %> </span></div>                                                                                      
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("UsedQuantity")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;">
                                    <div><%#: Eval("ItemUnit")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding-right:3px;text-align:right;">
                                    <div><%#: Eval("CreatedByFullName")%></div>
                                    <div><%#: Eval("CreatedDateInString")%></div>                                                 
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center">
                    <span class="lblLink" id="lblDrugMSReturnQuickPick"><%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>