<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="MedicalFolderStatusEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MedicalFolderStatusEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnOrderListBack" runat="server" crudmode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
    <li id="btnProcessSaveRecord" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div><%=GetLabel("Proses")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onLoad() {
            $('#<%=btnProcessSaveRecord.ClientID %>').click(function () {
                var param = '';
                $('.chkIsExist input').each(function () {
                    var id = $(this).closest('tr').find('.keyField').html();
                    $isComplete = $(this).closest('tr').find('.chkIsCompleted').find('input');
                    if (param != '') {
                        param += '|';
                    }
                    param += id + ',';
                    if ($(this).is(':checked'))
                        param += '1';
                    else
                        param += '0';
                    param += ','
                    if ($isComplete.is(':checked'))
                        param += '1';
                    else
                        param += '0';
                    param += ','
                    $cboType = $(this).closest('tr').find('.cboNotesType');
                    if ($cboType.val() != "") {
                        param += $cboType.val();
                    }
                    param += ','
                    $txtRemarks = $(this).closest('tr').find('.txtRemarks');
                    if ($txtRemarks.val() != "") {
                        param += $txtRemarks.val();
                    }
                });
                $('#<%=hdnParam.ClientID %>').val(param);
                onCustomButtonClick('approve');
            });
            $('.chkIsExist input').each(function () {
                $(this).change();
            });
        }

        $('#<%=btnOrderListBack.ClientID %>').click(function () {
            showLoadingPanel();
            document.location = ResolveUrl('~/Program/PatientList/VisitList.aspx?id=mfs');
        });

        $('.chkIsExist input').live('change', function () {
            $tr = $(this).closest('tr');
            $chkHdID = $(this).closest('tr').find('.chkIsCompleted').find('input');
            $txtRemarks = $tr.find('.txtRemarks');
            $cboType = $tr.find('.cboNotesType');
            if ($(this).is(':checked')) {
                $chkHdID.removeAttr("disabled");
            }
            else {
                $chkHdID.attr("disabled", true);
                $chkHdID.prop('checked', false);
                $txtRemarks.attr('readonly', 'readonly');
                $cboType.attr("disabled", true);
            }
        });

        $('.chkIsCompleted input').live('change', function () {
            $tr = $(this).closest('tr');
            $txtRemarks = $tr.find('.txtRemarks');
            $cboType = $tr.find('.cboNotesType');
            if ($(this).is(':checked')) {
                $txtRemarks.attr('readonly', 'readonly');
                $cboType.attr("disabled", true);
            }
            else {
                $txtRemarks.removeAttr('readonly');
                $cboType.attr("disabled", false);
            }
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCboDepartmentChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            $('.chkIsExist input').each(function () {
                $(this).change();
            });
        }


        $(function () {
            $("#<%=rblCheckAll.ClientID %> input").change(function () {
                var value = $(this).val();
                if (value == 1) {
                    $(".chkIsExist input").each(function () {
                        $(this).prop('checked', true);
                    });
                    $(".chkIsCompleted input").each(function () {
                        $(this).attr("disabled", false);
                        $(this).prop('checked', true);
                    });
                    $(".txtRemarks").each(function () {
                        $(this).attr('readonly', 'readonly');
                    });
                    $(".cboNotesType").each(function () {
                        $(this).attr("disabled", true);
                    });
                }
                else if (value == 2) {
                    $(".chkIsExist input").each(function () {
                        $(this).prop('checked', true);
                    });
                    $(".chkIsCompleted input").each(function () {
                        $(this).attr("disabled", false);
                        $(this).prop('checked', false);
                    });
                    $(".txtRemarks").each(function () {
                        $(this).removeAttr('readonly');
                    });
                    $(".cboNotesType").each(function () {
                        $(this).attr("disabled", false);
                    });
                }
                else if (value == 3) {
                    $(".chkIsExist input").each(function () {
                        $(this).prop('checked', false);
                    });
                    $(".chkIsCompleted input").each(function () {
                        $(this).attr("disabled", true);
                        $(this).prop('checked', false);
                    });
                    $(".txtRemarks").each(function () {
                        $(this).attr('readonly', 'readonly');
                    });
                    $(".cboNotesType").each(function () {
                        $(this).attr("disabled", true);
                    });
                }
            });
        });

        function cboMedicalFileStatusChanged() {

            var value = cboMedicalFileStatus.GetValue();
            cboReason.SetValue('<%=GetReasonBerkasKurangLengkap()%>');
            if (value == '<%=GetMedicalFileStatusReturn()%>') {
                trReason.style.display = "table-row";
            } else {
                trReason.style.display = "none";
                trRemarks.style.display = "none";
            }
        }

        function cboReasonChanged() {
            var value = cboReason.GetValue();
            if (value == '<%=GetReasonOther()%>') {
                trRemarks.style.display = "table-row";
            } else {
                trRemarks.style.display = "none";
            }
        }
    </script>

    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnCheckedIsExist" runat="server" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 15%" />
                        <col />
                    </colgroup>
                    <tr id="trServiceUnit" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jenis Berkas")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFolderType" Width="300px" ClientInstanceName="cboFolderType"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RadioButtonList ID="rblCheckAll" CssClass="rblCheckAll" runat="server"
                                RepeatDirection="Vertical">
                                <asp:ListItem Text="Centang Semua (Ada dan Terisi)" Value="1" />
                                <asp:ListItem Text="Centang Semua (Ada dan Tidak Terisi)" Value="2" />
                                <asp:ListItem Text="Centang Semua (Tidak Ada dan Tidak Terisi)" Value="3" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table width="90%">
                    <colgroup>
                        <col style="width:50%"/>
                    </colgroup>
                    <tr>
                        <td valign="top"><%=GetLabel("Status Rekam Medis")  %></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboMedicalFileStatus" ClientInstanceName="cboMedicalFileStatus" Width="120px">
                                <ClientSideEvents ValueChanged="function(s,e){cboMedicalFileStatusChanged()}" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td><%=GetLabel("Pembawa Berkas")%></td>
                        <td><asp:TextBox runat="server" ID="txtTransporterName" Width="120px" /></td>
                    </tr>
                    <tr id="trReason" style="display:none;">
                        <td><%=GetLabel("Alasan Pengembalian") %></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboReason" ClientInstanceName="cboReason" Width="120px">
                                <ClientSideEvents ValueChanged="function(s,e){cboReasonChanged()}" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trRemarks" style="display:none;">
                        <td valign="top"><%=GetLabel("Catatan")%></td>
                        <td id="tdRemarks" runat="server">
                            <asp:TextBox runat="server" ID="txtRemarks" Width="100%" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="FormID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="FormCode" HeaderText="Kode Berkas" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="FormName" HeaderStyle-CssClass="formName" ItemStyle-CssClass="formName" HeaderText="Nama Berkas" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left"/>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Ada">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsExist" CssClass="chkIsExist" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Terisi">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsCompleted" CssClass="chkIsCompleted" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Jenis Catatan">                                               
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="cboMRStatusNotes" CssClass="cboNotesType" Width="100%"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Catatan Tambahan">                                               
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" CssClass="txtRemarks" Width="100%" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
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
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
