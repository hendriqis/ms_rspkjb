<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloodBankOrderEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BloodBankOrderEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_BloodBankOrderEntryCtl1">
    $(function () {
        setDatePicker('<%=txtOrderDate.ClientID %>');
        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        //#region Room

        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = '';

            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";

            return filterExpression;
        }
        //#endregion

        $('#<%=rblGCSourceType.ClientID %> input').change(function () {
            if ($(this).val() == "X533^001") {
                $('#<%=trPaymentTypeInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trPaymentTypeInfo.ClientID %>').attr("style", "display:none");
            }
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

        $('#lblPhysicianNoteID').removeClass('lblLink');

    });

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }

    //#region Detail Region
    $('.imgAddDetail.imgLink').die('click');
    $('.imgAddDetail.imgLink').live('click', function (evt) {
        ResetDetailEntryControls();
        $('#<%=hdnDetailProcessMode.ClientID %>').val("1");
        $('#trDetail').removeAttr('style');
    });

    $('.imgEditDetail.imgLink').die('click');
    $('.imgEditDetail.imgLink').live('click', function () {
        $('#<%=hdnDetailProcessMode.ClientID %>').val('0');
        SetDetailEntityToControl(this);
        $('#trDetail').removeAttr('style');
    });

    $('.imgDeleteDetail.imgLink').die('click');
    $('.imgDeleteDetail.imgLink').live('click', function () {
        $('#<%=hdnDetailRecordID.ClientID %>').val($(this).attr('recordID'));
        var itemName = $(this).attr('itemName')
        var message = "Hapus Jenis darah <b>'" + itemName + "'</b> dari order permintaan ?";
        displayConfirmationMessageBox('Order Bank Darah', message, function (result) {
            if (result) {
                cbpDetail.PerformCallback('delete');
            }
        });
    });

    function SetDetailEntityToControl(param) {
        $('#<%=hdnDetailRecordID.ClientID %>').val($(param).attr('recordID'));
        cboBloodComponentType.SetValue($(param).attr('itemID'));
        $('#<%=txtOrderQty.ClientID %>').val($(param).attr('quantity'));

        $('#<%=hdnDetailRecordID.ClientID %>').val($(param).attr('recordID'));
    }

    $('.btnApplyDetail').click(function () {
        if (cboBloodType.GetValue() != null && cboBloodType.GetValue() != "" && cboBloodType.GetValue() != "X009^X" && cboRhesus.GetValue() != null && cboRhesus.GetValue() != "") {
            submitDetail();
            ResetDetailEntryControls();
        } else {
            alert("Harap pilih Golongan Darah dan Rhesusnya terlebih dahulu.");
        }
    });

    $('.btnCancelDetail').click(function () {
        ResetDetailEntryControls();
        $('#trDetail').attr('style', 'display:none');
    });

    function ResetDetailEntryControls(s) {
        cboBloodComponentType.SetValue('');
        $('#<%=txtOrderQty.ClientID %>').val('1');
        $('#<%=hdnDetailProcessMode.ClientID %>').val('1');
    }

    function submitDetail() {
        if (cboBloodComponentType.GetValue() != '') {
            if ($('#<%=hdnDetailProcessMode.ClientID %>').val() == "1")
                cbpDetail.PerformCallback('add');
            else
                cbpDetail.PerformCallback('edit');
        }
        else {
            displayErrorMessageBox("ERROR", "Jenis darah harus dipilih sebelum diproses !");
        }
    }

    function onCbpDetailEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit") {
                $('#<%=hdnDetailProcessMode.ClientID %>').val('0');
            }

            $('#<%=hdnTestOrderIDCtlEntry.ClientID %>').val(param[2]);

            ResetDetailEntryControls();
            cbpDetailView.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            displayErrorMessageBox("Jenis Darah", 'Error Message : ' + param[2]);
        }
        else
            $('#<%=grdDetailView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<style type="text/css">
    
</style>
<div>
    <input type="hidden" runat="server" id="hdnParameterRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnParameterVisitID" value="" />
    <input type="hidden" runat="server" id="hdnParameterParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderIDCtlEntry" value="0" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnBloodBankHSUID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" runat="server" id="hdnDetailProcessMode" value="1" />
    <input type="hidden" runat="server" id="hdnDetailRecordID" value="" />
    <input type="hidden" runat="server" id="hdnIsRemarksCopyFromDiagnose" />
    <input type="hidden" runat="server" id="hdnDefaultDiagnosa" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Data Order" class="w3-hover-red">Data Order</li>
                        <li contentid="divPage2" title="Jenis Darah/Komponen" class="w3-hover-red">Jenis Produk Darah</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Sumber/Asal Darah") %></label>
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rblGCSourceType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" PMI" Value="X533^001" />
                                    <asp:ListItem Text=" Persediaan BDRS" Value="X533^002" />
                                    <asp:ListItem Text=" Pendonor" Value="X533^003" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Penyimpanan") %></label>
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rblGCUsageType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Langsung digunakan" Value="X534^001" />
                                    <asp:ListItem Text=" Dititipkan di BDRS" Value="X534^002" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trPaymentTypeInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Pembayaran (Jika PMI)") %></label>
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rblGCPaymentType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Dibayar langsung di PMI" Value="X535^001" />
                                    <asp:ListItem Text=" Tagihan Pasien di Rumah Sakit" Value="X535^002" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Catatan Klinis/Diagnosa") %></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Riwayat Transfusi") %></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtMedicalHistory" runat="server" Width="99%" TextMode="Multiline"
                                    Height="250px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Golongan Darah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboBloodType" ClientInstanceName="cboBloodType"
                                    Width="125px" ToolTip="Golongan Darah">
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <label class="lblMandatory">
                                                <%=GetLabel("Rhesus")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboRhesus" ClientInstanceName="cboRhesus" Width="100px"
                                                ToolTip="Rhesus">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <tr id="trDetail" style="display: none">
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                <colgroup>
                                                    <col style="width: 120px" />
                                                    <col style="width: 150px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblMandatory">
                                                            <%=GetLabel("Produk Darah")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <dxe:ASPxComboBox runat="server" ID="cboBloodComponentType" ClientInstanceName="cboBloodComponentType"
                                                            Width="99%" ToolTip="Jenis Darah/Komponen">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Jumlah")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOrderQty" Width="60px" CssClass="number" runat="server" />
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <img class="btnApplyDetail imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td>
                                                                    <img class="btnCancelDetail imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="position: relative;">
                                                <dxcp:ASPxCallbackPanel ID="cbpDetailView" runat="server" Width="100%" ClientInstanceName="cbpDetailView"
                                                    ShowLoadingPanel="false" OnCallback="cbpDetailView_Callback">
                                                    <ClientSideEvents EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                                <asp:GridView ID="grdDetailView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                            <HeaderTemplate>
                                                                                <img class="imgAddDetail imgLink" title='<%=GetLabel("+ Jenis/Komponen Darah")%>'
                                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>' alt="" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img class="imgEditDetail imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                alt="" recordid="<%#:Eval("ID") %>" itemid="<%#:Eval("ItemID") %>" itemname="<%#:Eval("ItemName1") %>"
                                                                                                quantity="<%#:Eval("cfQuantity") %>" />
                                                                                        </td>
                                                                                        <td style="width: 1px">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <img class="imgDeleteDetail imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                alt="" recordid="<%#:Eval("ID") %>" itemid="<%#:Eval("ItemID") %>" itemname="<%#:Eval("ItemName1") %>"
                                                                                                quantity="<%#:Eval("cfQuantity") %>" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <%=GetLabel("Jenis/Komponen Darah")%>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div>
                                                                                    <%#: Eval("ItemName1")%></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Jumlah" DataField="cfQuantity" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("Belum ada informasi jenis/komponen darah untuk order untuk pasien ini") %>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="detailPaging">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpDetail" runat="server" Width="100%" ClientInstanceName="cbpDetail"
        ShowLoadingPanel="false" OnCallback="cbpDetail_Callback">
        <ClientSideEvents EndCallback="function(s,e){onCbpDetailEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
