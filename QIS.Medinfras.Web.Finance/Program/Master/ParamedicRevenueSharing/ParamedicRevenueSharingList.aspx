<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ParamedicRevenueSharingList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ParamedicRevenueSharingList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>


<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnEditRecord">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" />
        <div>
            <%=GetLabel("Edit")%>
        </div>
    </li>
    <li runat="server" id="btnSaveRecord" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" />
        <div>
            <%=GetLabel("Save")%>
        </div>
    </li>
    <li runat="server" id="btnCancelRecord" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" />
        <div>
            <%=GetLabel("Cancel")%>
        </div>
    </li>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'>
    </script>

    <script type="text/javascript">
        //#region Button
        $('#<%=btnEditRecord.ClientID %>').click(function () {
            if (IsValid(null, 'fsPatientList', 'mpPatientEntry')) {
                $('#<%=btnEditRecord.ClientID %>').hide();
                $('#<%=btnSaveRecord.ClientID %>').show();
                $('#<%=btnCancelRecord.ClientID %>').show();

                $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
                    var keyField = $(this).find('.keyField').html();
                    var clientInstanceName = 'cboRevenue' + keyField;
                    var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
                    cbo.SetEnabled(true);
                });

                $('#lblItem').attr('class', 'lblDisabled')
                $('#lblPhysician').attr('class', 'lblDisabled')
                cboHealthCare.SetEnabled(false);
                $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
            }
        });

        $('#<%=btnSaveRecord.ClientID %>').click(function () {
            var result = '';
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
                if (result != "")
                    result += "|";
                var id = $(this).find('.hdnGCParamedicRole').val();
                var keyField = $(this).find('.keyField').html();
                var clientInstanceName = 'cboRevenue' + keyField;
                var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
                result += id + ';' + cbo.GetValue();
            });
            $('#<%=hdnResult.ClientID %>').val(result);
            $('#<%=btnEditRecord.ClientID %>').show();
            $('#<%=btnSaveRecord.ClientID %>').hide();
            $('#<%=btnCancelRecord.ClientID %>').hide();

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
                var keyField = $(this).find('.keyField').html();
                var clientInstanceName = 'cboRevenue' + keyField;
                var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
                cbo.SetEnabled(false);
            });

            $('#lblItem').attr('class', 'lblLink')
            $('#lblPhysician').attr('class', 'lblLink')
            cboHealthCare.SetEnabled(true);
            $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtPhysicianCode.ClientID %>').removeAttr('readonly');

            onCustomButtonClick('save');
        });

        function onAfterCustomClickSuccess() {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=btnCancelRecord.ClientID %>').click(function () {

            $('#<%=btnEditRecord.ClientID %>').show();
            $('#<%=btnSaveRecord.ClientID %>').hide();
            $('#<%=btnCancelRecord.ClientID %>').hide();

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
                var keyField = $(this).find('.keyField').html();
                var clientInstanceName = 'cboRevenue' + keyField;
                var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
                cbo.SetEnabled(false);
            });

            $('#lblItem').attr('class', 'lblLink');
            $('#lblPhysician').attr('class', 'lblLink');

            cboHealthCare.SetEnabled(true);
            $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtPhysicianCode.ClientID %>').removeAttr('readonly');

            cbpView.PerformCallback('refresh');
        });
        //#endregion

        //#region Item
        function onGetItemFilterExpression() {
            var filterExpression = "<%:OnGetItemFilterExpression() %>";
            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('item', onGetItemFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemCodeChanged($(this).val());
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = onGetItemFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion
        
        //#region Paramedic
        function onGetPhysicianFilterExpression() {
            var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ParamedicMaster WHERE HealthcareID = '" + cboHealthCare.GetValue() + "') AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        $('.lnkClass').live('click', function () {
            var id = $(this).closest('tr').find('.hdnRowID').val();
            var url = ResolveUrl("~/Program/Master/ParamedicRevenueSharing/ParamedicRevenueSharingCtl.ascx");
            openUserControlPopup(url, id, 'Class Care', 800, 600);
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onCboHealthCareChanged() {
            $('#<%=txtPhysicianCode.ClientID %>').val('');
            $('#<%=hdnPhysicianID.ClientID %>').val('');
            $('#<%=txtPhysicianName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        function onRefreshGridView() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }     

    </script>

    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server"  />
    <input type="hidden" value="" id="hdnRevenue" runat="server" />
    <input type="hidden" value="" id="hdnRow" runat="server" />
    <input type="hidden" value="" id="hdnResult" runat="server" />

    <div style="position: relative;">
        <div class="pageTitle"><%=GetLabel("Jasa Medis per Dokter")%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsPatientList">  
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>                            
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Rumah Sakit")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboHealthCare" ClientInstanceName="cboHealthCare" Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboHealthCareChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblItem"><%=GetLabel("Item")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemCode" CssClass="required"  Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblPhysician"><%=GetLabel("Dokter / Paramedis")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtPhysicianCode" Width="100%" CssClass="" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
               </td>
            </tr>
        </table>
        
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="cfStandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                <asp:BoundField DataField="StandardCodeName" HeaderText="Peran Dokter" HeaderStyle-Width="180px" />
                                <asp:TemplateField HeaderText="Jasa Medis">
                                    <ItemTemplate>
                                        <input type="hidden" runat="server" id="hdnGCParamedicRole" class="hdnGCParamedicRole" value='<%#:Eval("StandardCodeID") %>' />
                                        <input type="hidden" runat="server" id="hdnRowID" class="hdnRowID" />
                                        <dxe:ASPxComboBox ID="cboRevenue" ClientEnabled="false" ClientInstanceName='<%#: "cboRevenue" + DataBinder.Eval(Container.DataItem, "cfStandardCodeID")%>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Kelas" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <a id="lnkClass" runat="server" class="lnkClass">Class</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>