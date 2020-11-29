<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxReportList.aspx.vb" Inherits="iPxAdmin_iPxReportList" title="Alcor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .footer {
           position: fixed;
           left: 0;
           bottom: 0;
           width: 100%;
           background-color:Transparent;
           color: white;
           text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="row">
  <div class="col-lg-6">
   <div class="text-bold">
<asp:Label ID="Label1" runat="server" Text="Group" ></asp:Label>
    </div>
   
<div >
<asp:DropDownList ID="ddlGrpID" runat="server" AutoPostBack="True"  CssClass=" form-control"  >
</asp:DropDownList>

     
</div>
<hr />
   <asp:GridView ID="GridView1" CssClass="table table-bordered" GridLines="None" AutoGenerateColumns="false"  runat="server" AllowPaging="True" AllowSorting="True"  OnPageIndexChanging="OnPaging"   >
                                        <Columns>
                                             <asp:templatefield HeaderText="Select">
        <itemtemplate>
            <asp:linkbutton width="100px" cssclass="btn btn-success " data- id="deleteitem" CommandName="getReport" CommandArgument='<%# Eval("reportID")%>' runat="server"><span class="fa fa-check"></span> Select</asp:linkbutton>
        </itemtemplate>
    </asp:templatefield>
    
    <asp:TemplateField HeaderText="Balance">
        <ItemTemplate>
            <asp:Label ID="lblDescription"  runat="server"  Text='<%# Eval("description").ToString()%>'></asp:Label>
        </ItemTemplate>
   
     </asp:TemplateField>
                                        </Columns>
                                      <PagerStyle HorizontalAlign = "Center" BorderColor="White" Font-Bold="true"  CssClass = "pagination-ys" />
                                        <SelectedRowStyle Font-Bold="True" Font-Italic="True" />
                                 
                                        <AlternatingRowStyle CssClass="alt" />
                                    </asp:GridView>
  
</div>
<br />
<br />
<br />
<br />
<br />

 <div class="col-lg-6">
     <asp:Panel ID="pnlPRM"  runat="server">
   <table class="table table-bordered">
   <tr>
<td>

   
    <asp:Panel runat="server" ID="pnlList" >




    
   
                      
<br />
                                             

   </asp:Panel>                 


    <asp:Panel runat="server" ID="pnlParam"  >
<div class=" text-bold">
                                   <asp:Label ID="lblP1" runat="server"></asp:Label>
                                 </div>
<div >
                                <asp:TextBox ID="txtP1" runat="server" Visible="False" CssClass="form-control"></asp:TextBox>
                                <asp:DropDownList ID="ddlP1" runat="server"  Visible="False" CssClass="form-control">
                                </asp:DropDownList>
    <div class="input-group">
                                <asp:TextBox ID="txtDateP1" Enabled="false" runat="server"  Visible="False" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtDateP1_CalendarExtender" runat="server" 
                                    Format="dd/MM/yyyy" PopupButtonID="imgP1" TargetControlID="txtDateP1">
                                </cc1:CalendarExtender>
        <span class="input-group-btn">
                             
                   <asp:LinkButton ID="imgP1" CssClass=" btn btn-success" runat="server"><span class="fa fa-calendar"></span></asp:LinkButton>

            </span>
        </div>
                            </div>
                                           
<div class=" text-bold">
                                   <asp:Label ID="lblP2" runat="server"></asp:Label>
                                 </div>

<div >
                                <asp:TextBox ID="txtP2" runat="server"  Visible="False"  CssClass="form-control"></asp:TextBox>
                                <asp:DropDownList ID="ddlP2" runat="server"  Visible="False" CssClass="form-control">
                                </asp:DropDownList>
    <div class="input-group">
                                <asp:TextBox ID="txtDateP2" Enabled="false" runat="server" Visible="False" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtDateP2_CalendarExtender" runat="server" 
                                    Format="dd/MM/yyyy" PopupButtonID="imgP2" TargetControlID="txtDateP2">
                                </cc1:CalendarExtender>
    <span class="input-group-btn">                          
     <asp:LinkButton ID="imgP2" CssClass=" btn btn-success" runat="server"><span class="fa fa-calendar"></span></asp:LinkButton>
        </span>
        </div>
                            </div>
                     

<div class="text-bold">
                                   <asp:Label ID="lblP3" runat="server"></asp:Label>
                                 </div>
<div>
                                <asp:TextBox ID="txtP3" runat="server"  Visible="False"  CssClass="form-control"></asp:TextBox>
                                <asp:DropDownList ID="ddlP3" runat="server" Visible="False" CssClass="form-control">
                                </asp:DropDownList>
    <div class="input-group">
                                <asp:TextBox ID="txtDateP3" Enabled="false" runat="server"  Visible="False" CssClass="form-control"></asp:TextBox>

                                <cc1:CalendarExtender ID="txtDateP3_CalendarExtender" runat="server" 
                                    Format="dd/MM/yyyy" PopupButtonID="imgP3" TargetControlID="txtDateP3">
                                </cc1:CalendarExtender>
              <span class="input-group-btn">               
    <asp:LinkButton ID="imgP3" CssClass=" btn btn-success" runat="server"><span class="fa fa-calendar"></span></asp:LinkButton>
                  </span>
        </div>
                            </div>
                  
                      
<div class="text-bold">
                                   <asp:Label ID="lblP4" runat="server"></asp:Label>
                                 </div>
<div >
                                <asp:TextBox ID="txtP4" runat="server" Visible="False" CssClass="form-control"></asp:TextBox>
                                <asp:DropDownList ID="ddlP4" runat="server" Visible="False" CssClass="form-control">
                                </asp:DropDownList>
    <div class="input-group">
                                <asp:TextBox ID="txtDateP4" Enabled="false" runat="server"  Visible="False" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtDateP4_CalendarExtender" runat="server" 
                                    Format="dd/MM/yyyy" PopupButtonID="imgP4" TargetControlID="txtDateP4">
                                </cc1:CalendarExtender>
                                <span class="input-group-btn">  

         <asp:LinkButton ID="imgP4" CssClass=" btn btn-success" runat="server"><span class="fa fa-calendar"></span></asp:LinkButton>
                                    </span>
        </div>
                            </div>
                   
<div >
    <br />
<asp:Button ID="btnPreview" runat="server"  Text="Preview"  class="btn btn-success btn-block" />
    <hr />

</div>

        </asp:Panel>


    
<asp:LinkButton ID="lnkFake" runat="server"  Text="" />
<cc1:ModalPopupExtender ID="popReport" runat="server" 
                                            BackgroundCssClass="modalBackground" PopupControlID="pnlRpt" 
                                            TargetControlID="lnkFake" Drag="True" >
                                        </cc1:ModalPopupExtender>
<asp:Panel ID="pnlRpt" runat="server"   CssClass="modal-dialog modal-lg"  style = "display:none" ScrollBars="None">
                         <div class="modal-content">
                             <div class="modal-header">
                                      
                                 <asp:LinkButton ID="btnClose" CssClass="close" runat="server">&times;</asp:LinkButton>
     </div>
                          <div class="modal-body" style="height: 500px">
                                    <iframe id="frameeditexpanse" frameborder="0" src="iPxReportView.aspx" class="panel panel-default" width="100%" height="100%"   scrolling="auto" >
                                    </iframe>
                 
                        </div>

                         
                        </asp:Panel>  
   

   
                        </td>
                        </tr>
                        </table> 
                            </asp:Panel>
    </div>
  </div>
</asp:Content>

<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="Server">
    <div class="footer">
        <asp:linkbutton width="150px" cssclass="btn btn-default" PostBackUrl="~/iPxAdmin/home.aspx"  data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
    </div>
</asp:content>