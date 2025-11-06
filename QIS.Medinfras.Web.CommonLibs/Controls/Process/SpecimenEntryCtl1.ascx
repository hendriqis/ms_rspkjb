<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpecimenEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SpecimenEntryCtl1" %>
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
<script type="text/javascript" id="dxss_NewSurgeryOrderEntryCtl1">
    $(function () {
        setDatePicker('<%=txtSampleDate.ClientID %>');

        $('#<%=txtSampleDate.ClientID %>').datepicker('option', 'maxDate', '0');

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
    });

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }

    //#region Specimen
    function onLedSpecimenLostFocus(led) {
        var specimenID = led.GetValueText();
        $('#<%=hdnEntrySpecimenID.ClientID %>').val(specimenID);
        var filterExp = "SpecimenID = '" + specimenID + "'";
        Methods.getObject("GetSpecimenList", filterExp, function (result) {
            if (result != null) {
                cboContainerType.SetValue(result.GCSpecimenContainerType);
            }
            else {
                cboContainerType.SetValue("");
            }
        });
    }

    function ResetSpecimenEntryControls(s) {
        ledSpecimen.SetValue('');
        cboContainerType.SetValue('');
        $('#<%=txtSpecimenQty.ClientID %>').val('0');
        $('#<%=hdnSpecimenProcessMode.ClientID %>').val('0');
    }

    $('.imgAddSpecimen.imgLink').die('click');
    $('.imgAddSpecimen.imgLink').live('click', function (evt) {
        ResetSpecimenEntryControls();
        $('#<%=hdnSpecimenProcessMode.ClientID %>').val("1");
        $('#trSpecimen').removeAttr('style');
    });

    $('.btnApplySpecimen').click(function () {
        submitSpecimen();
        $('#<%=ledSpecimen.ClientID %>').focus();
        $('#trSpecimen').attr('style', 'display:none');
    });

    $('.btnCancelSpecimen').click(function () {
        ResetSpecimenEntryControls();
        $('#trSpecimen').attr('style', 'display:none');
    });

    $('.imgEditSpecimen.imgLink').die('click');
    $('.imgEditSpecimen.imgLink').live('click', function () {
        $('#<%=hdnSpecimenProcessMode.ClientID %>').val('0');
        SetSpecimenEntityToControl(this);
        $('#trSpecimen').removeAttr('style');
    });

    $('.imgDeleteSpecimen.imgLink').die('click');
    $('.imgDeleteSpecimen.imgLink').live('click', function () {
        $('#<%=hdnOrderDtSpecimenID.ClientID %>').val($(this).attr('recordID'));
       var specimenName = $(this).attr('specimenName')
       var message = "Hapus Jenis sampel <b>'" + specimenName + "'</b> dari order pemeriksaan ?";
        displayConfirmationMessageBox('Sampel Pemeriksaan', message, function (result) {
            if (result) {
                cbpSpecimen.PerformCallback('delete');
            }
        });
    });

    function GetCurrentSelectedSpecimen(s) {
        var $tr = $(s).closest('tr').parent().closest('tr');
        var idx = $('#<%=grdSpecimenView.ClientID %> tr').index($tr);
        $('#<%=grdSpecimenView.ClientID %> tr:eq(' + idx + ')').click();

        $row = $('#<%=grdSpecimenView.ClientID %> tr.selected');
        var selectedObj = {};

        $row.find('input[type=hidden]').each(function () {
            selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });

        return selectedObj;
    }

    function SetSpecimenEntityToControl(param) {
        $('#<%=hdnOrderDtSpecimenID.ClientID %>').val($(param).attr('recordID'));
        ledSpecimen.SetValue($(param).attr('specimenID'));
        cboContainerType.SetValue($(param).attr('containerType'));
        $('#<%=txtSpecimenQty.ClientID %>').val($(param).attr('quantity'));

        $('#<%=hdnEntrySpecimenID.ClientID %>').val($(param).attr('specimenID'));
    }

    function submitSpecimen() {
        if ($('#<%=hdnEntrySpecimenID.ClientID %>').val() != '') {
            if ($('#<%=hdnSpecimenProcessMode.ClientID %>').val() == "1")
                cbpSpecimen.PerformCallback('add');
            else
                cbpSpecimen.PerformCallback('edit');
        }
        else {
            displayErrorMessageBox("ERROR", "Jenis spesimen harus dipilih sebelum diproses !");
        }
    }

    function onCbpSpecimenEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit") {
                $('#<%=hdnSpecimenProcessMode.ClientID %>').val('0');
            }

            $('#<%=hdnTestOrderID.ClientID %>').val(param[2]);

            ResetSpecimenEntryControls();
            cbpSpecimenView.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            displayErrorMessageBox("Sampel Pemeriksaan", 'Error Message : ' + param[2]);
        }
        else
            $('#<%=grdSpecimenView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

</script>
<style type="text/css">
    
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnFromHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnSpecimenTakenID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnSpecimenProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtSpecimenID" value="" />
    <input type="hidden" runat="server" id="hdnEntrySpecimenID" value="" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 22%" />
            <col style="width: 78%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Informasi Pengambilan Sample" class="w3-hover-red">Pengambilan Sample</li>
                        <li contentID="divPage2" title="Catatan Sample" class="w3-hover-red">Catatan Sample</li>
                        <li contentID="divPage3" title="Detail Pemeriksaan" class="w3-hover-red">Detail Pemeriksaan</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Pengambilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSampleDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSampleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pengambil Sampel")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSpecimenTakenName" Width="150px" runat="server" Style="text-align: left" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsUsingExistingSample" Width="100%" runat="server" Text=" Pemeriksaan Tambahan (menggunakan sample yang sudah ada)" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <tr id="trSpecimen" style="display: none">
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                                <colgroup>
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Spesimen")%></label>
                                                    </td>
                                                    <td colspan="3">
                                                        <qis:QISSearchTextBox ID="ledSpecimen" ClientInstanceName="ledSpecimen"
                                                            runat="server" Width="99%" ValueText="SpecimenID" FilterExpression="" DisplayText="SpecimenName"
                                                            MethodName="GetSpecimenList">
                                                            <ClientSideEvents ValueChanged="function(s){ onLedSpecimenLostFocus(s); }" />
                                                            <Columns>
                                                                <qis:QISSearchTextBoxColumn Caption="Nama Spesimen" FieldName="SpecimenName"
                                                                    Description="i.e. Darah" Width="500px" />
                                                            </Columns>
                                                        </qis:QISSearchTextBox>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Jenis Tabung")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ID="cboContainerType" ClientInstanceName="cboContainerType"
                                                            Width="100px">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Jumlah")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSpecimenQty" Width="60px" CssClass="number" runat="server" />
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <img class="btnApplySpecimen imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td>
                                                                    <img class="btnCancelSpecimen imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                                <dxcp:ASPxCallbackPanel ID="cbpSpecimenView" runat="server" Width="100%" ClientInstanceName="cbpSpecimenView"
                                                    ShowLoadingPanel="false" OnCallback="cbpSpecimenView_Callback">
                                                    <ClientSideEvents EndCallback="function(s,e){ onCbpSpecimenViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                                <asp:GridView ID="grdSpecimenView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                            <ItemTemplate>
                                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                <input type="hidden" value="<%#:Eval("SpecimenID") %>" bindingfield="SpecimenID" />
                                                                                <input type="hidden" value="<%#:Eval("SpecimenCode") %>" bindingfield="SpecimenCode" />
                                                                                <input type="hidden" value="<%#:Eval("SpecimenName") %>" bindingfield="SpecimenName" />
                                                                                <input type="hidden" value="<%#:Eval("GCSpecimenContainerType") %>" bindingfield="GCSpecimenContainerType" />
                                                                                <input type="hidden" value="<%#:Eval("cfQuantity") %>" bindingfield="cfQuantity" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                            <HeaderTemplate>
                                                                                <img class="imgAddSpecimen imgLink" title='<%=GetLabel("+ Jenis Spesimen")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                    alt=""/>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img class="imgEditSpecimen imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                alt="" recordID = "<%#:Eval("ID") %>" specimenID = "<%#:Eval("SpecimenID") %>" specimenCode = "<%#:Eval("SpecimenCode") %>" 
                                                                                                specimenName = "<%#:Eval("SpecimenName") %>" containerType = "<%#:Eval("GCSpecimenContainerType") %>" quantity = "<%#:Eval("cfQuantity") %>" />
                                                                                        </td>
                                                                                        <td style="width: 1px">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <img class="imgDeleteSpecimen imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                alt="" recordID = "<%#:Eval("ID") %>" specimenID = "<%#:Eval("SpecimenID") %>" specimenCode = "<%#:Eval("SpecimenCode") %>" 
                                                                                                specimenName = "<%#:Eval("SpecimenName") %>" containerType = "<%#:Eval("GCSpecimenContainerType") %>" quantity = "<%#:Eval("cfQuantity") %>" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <%=GetLabel("Jenis Sampel")%>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div><%#: Eval("SpecimenName")%></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Jenis Tabung"  DataField="SpecimenContainerType" />
                                                                        <asp:BoundField HeaderText="Jumlah Sampel"  DataField="cfQuantity" />
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("Belum ada informasi jenis sampel untuk pasien ini") %>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="SpecimenPaging">
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
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Tambahan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="350px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpPopupDetailView" runat="server" Width="100%" ClientInstanceName="cbpPopupDetailView"
                            ShowLoadingPanel="false" OnCallback="cbpPopupDetailView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 300px">
                                        <asp:GridView ID="grdPopupViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="100px" />
                                                <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="300px" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Catatan Klinis/Order" HeaderStyle-HorizontalAlign="Left" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Belum ada informasi pemeriksaan untuk pasien ini") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                     </div> 
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpSpecimen" runat="server" Width="100%" ClientInstanceName="cbpSpecimen"
        ShowLoadingPanel="false" OnCallback="cbpSpecimen_Callback">
        <ClientSideEvents EndCallback="function(s,e){ onCbpSpecimenEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
