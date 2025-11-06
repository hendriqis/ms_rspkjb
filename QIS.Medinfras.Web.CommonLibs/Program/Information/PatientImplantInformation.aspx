<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="PatientImplantInformation.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientImplantInformation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <style type="text/css">
        .LvColor
        {
            background-color: Silver !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtTransactionDate.ClientID %>');

            //            $('#<%=txtTransactionDate.ClientID %>').change(function (evt) {
            //                onRefreshGridView();
            //            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                //                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                //                intervalID = window.setInterval(function () {
                //                    onRefreshGridView();
                //                }, interval);
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            var gender = $('.hdnPatientGender').val();
            Methods.checkImageError('imgPatientImage', 'patient', gender);
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var gender = $('.hdnPatientGender').val();
            Methods.checkImageError('imgPatientImage', 'patient', gender);
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else if(param[0] == 'print') {
                cbpView.PerformCallback('refresh');
            }
        }
        //#endregion

        function refreshGrdRegisteredPatient() {

            cbpView.PerformCallback('refresh');
        }

        function onGetPhysicianFilterExpression() {
            var filterExpression = "IsDeleted = 0";
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

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtTransactionDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtTransactionDate.ClientID %>').removeAttr('readonly');
            //            onRefreshGridView();
        });
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentID" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=HttpUtility.HtmlEncode(GetMenuCaption())%>
        </div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Transaksi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactionDate" Width="120px" runat="server" CssClass="datepicker" />
                                    <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPhysician">
                                        <%=GetLabel("Dokter / Tenaga Medis")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                    <table style="width: 350px" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianCode" Width="80px" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="98%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Dokter" FieldName="ParamedicName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("Setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("Menit")%>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyfield" style="display: none">
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Informasi Pasien")%>
                                                    </th>
                                                    <th style="width: 50px">
                                                        <%=GetLabel("Tanggal Transaksi")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Nama Dokter")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Nama Item")%>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Nomor Seri")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="12">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyfield" style="display: none">
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Informasi Pasien")%>
                                                    </th>
                                                    <th style="width: 50px">
                                                        <%=GetLabel("Tanggal Transaksi")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Nama Dokter")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Nama Item")%>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Nomor Seri")%>
                                                    </th>
                                                </tr>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trItem">
                                                <td class="keyfield" style="display: none">
                                                    <%#: Eval("ID") %>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        (<%#: Eval("MedicalNo") %>)
                                                        <%#: Eval("PatientName") %>                                                        
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <%#: Eval("cfTransactionDate") %>
                                                    </div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("ParamedicName") %></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("ItemName")%></div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <%#: Eval("SerialNumber") %></div>
                                                </td>                                                
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
