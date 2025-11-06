<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="LaboratoryVerificationResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryVerificationResultDetail" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Process")%></div></li>
    <li id="btnDecline" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div><%=GetLabel("Decline")%></div></li>
    <li id="btnProcessResultBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">    
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnProcessResultBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Worklist/LaboratoryVerificationResult/LaboratoryVerificationResultList.aspx');
            });

            $('#<%=btnDecline.ClientID %>').click(function () {
                onCustomButtonClick('decline');
            });

            $('#<%=btnProcess.ClientID %>').click(function () {
                onCustomButtonClick('process');
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == 'decline')
                $('#<%=btnProcessResultBack.ClientID %>').click();
            else if (type == 'process') {
                showToast('Process Success', 'Proses Verifikasi Sudah Berhasil Dilakukan', function () {
                    $('#<%=btnProcessResultBack.ClientID %>').click();
                });
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                getCheckedMember();
                cbpView.PerformCallback('refresh');
            }
        }

        $('#chkSelectAllResult').die('change');
        $('#chkSelectAllResult').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });
            
    </script> 
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedItem" runat="server" value="" />
    <input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
    <input type="hidden" value="" id="hdnLabResultID" runat="server"/>  

    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Laboratory Test Result Verification")%></div>
        </div>
        <table class="tblContentArea">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" align="center"><input id="chkSelectAllResult" type="checkbox" /></th>
                                                <th style="width:100%" align="center"><%=GetLabel("Pelayanan")%></th>
                                                <th align="center"><%=GetLabel("Fraction")%></th>
                                                <th align="center"><%=GetLabel("Nilai Hasil(Metric)")%></th>                                   
                                                <th align="center"><%=GetLabel("Nilai Hasil(International)")%></th>
                                                <th align="center"><%=GetLabel("Hasil Text")%></th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="6">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" align="center"><input id="chkSelectAllResult" type="checkbox" /></th>
                                                <th style="width:30%" align="center"><%=GetLabel("Pelayanan")%></th>
                                                <th align="center"><%=GetLabel("Fraction")%></th>
                                                <th align="center"><%=GetLabel("Nilai Hasil(Metric)")%></th>                                   
                                                <th align="center"><%=GetLabel("Nilai Hasil(International)")%></th>
                                                <th align="center"><%=GetLabel("Hasil Text")%></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%# Eval("CustomID")%>' />
                                            </td>
                                            <td><div <%# Eval("IsNormal").ToString() == "False" ? "class='isAbnormalColor'" : "" %>><%# Eval("ItemName1") %></div></td>
                                            <td><div <%# Eval("IsNormal").ToString() == "False" ? "class='isAbnormalColor'" : "" %>><%# Eval("FractionName1") %></div></td>
                                            <td align="right"><div <%# Eval("IsNormal").ToString() == "False" ? "class='isAbnormalColor'" : "" %>><%# Eval("MetricResultValue") %></div></td>
                                            <td align="right"><div <%# Eval("IsNormal").ToString() == "False" ? "class='isAbnormalColor'" : "" %>><%# Eval("InternationalResultValue") %></div></td>
                                            <td><div <%# Eval("IsNormal").ToString() == "False" ? "class='isAbnormalColor'" : "" %>><%# Eval("TextValue") %></div></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel> 
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>  
    </div>
</asp:Content>