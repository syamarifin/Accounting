<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master"  AutoEventWireup="false" CodeFile="Broadcast.aspx.vb" Inherits="iPxAdmin_Broadcast"  title="Alcor Loyalty Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    
    <div class="col-sm-4">
    <asp:DropDownList class="form-control padding-horizontal-15" id="drptemplate" AutoPostBack="true"  runat="server"></asp:DropDownList>
    </div>
    
    <div class="col-sm-3">
		<asp:textbox class="form-control padding-horizontal-15" id="txttemplate" placeholder="New Template"  runat="server"></asp:textbox>
    </div>
    
    <div class="col-sm-3">
       <asp:linkbutton width="80px" cssclass="btn btn-default " data- id="btnAddTemplate" runat="server"><span class="fa fa-plus"></span> Add</asp:linkbutton>
    
    </div>
    
    <div class="col-sm-2">
       <asp:linkbutton width="130px" cssclass="btn btn-primary " data- id="btnConfig" runat="server"><span class="fa fa-gear"></span> Configuration</asp:linkbutton>
	
<br />
<br />
	</div> 
	
	<div class="col-sm-12" >
       <div class="col-sm-2">
     <asp:RadioButton id="rbContent" CssClass="radio" GroupName="radiofilter2" Value="1"  Text="Content"   Checked="true" AutoPostBack="true"  runat="server" >
       </asp:RadioButton>
        <asp:Label ID="Label7" runat="server" ></asp:Label>
     </div>
     <div class="col-sm-2" >
     <asp:RadioButton id="rbImgContent" CssClass="radio" GroupName="radiofilter2" Value="0" Text="Image Content"  runat="server" AutoPostBack="true"  >
        </asp:RadioButton> 
         <asp:Label ID="Label9" runat="server"  ></asp:Label>
     </div>  
     </div>
	
	

    <table class="table"  >
     <tr >
        <td style="width:15%">
            <label for="" text-left">Subject</label>
        </td>
        <td  style="width:30%">
		<asp:textbox class="form-control padding-horizontal-15" id="txtSubject" placeholder="Email Subject" MaxLength="60" runat="server"></asp:textbox>
           
        </td>
        <td  style="width:50%">
	             
        </td>
        <td  style="width:5%">
        </td>
   
    </tr>
    
    <tr >
        <td style="width:15%">
            <label for="" text-left">Header Image</label>
        </td>
        <td  style="width:30%">
            <asp:FileUpload class="form-control padding-horizontal-15"  ID="up_header" runat="server" />
        </td>
        <td  style="width:50%">
	        <asp:Image ID="imgHeader" runat="server" Height="80px" />            
        </td>
        <td  style="width:5%">
       <asp:linkbutton cssclass="btn btn-default " data- id="delHDR" runat="server"><span class="fa fa-trash"></span></asp:linkbutton>
       </td>
   
    </tr>
    <asp:Panel ID="Panel2"  runat="server">
    <tr >
        <td style="width:15%">
            <label for="" text-left">Content Image</label>
        </td>
        <td  style="width:30%">
            <asp:FileUpload class="form-control padding-horizontal-15"  ID="up_content" runat="server" />
        </td>
        <td  style="width:50%">
	        <asp:Image ID="imgContent" runat="server" Height="100%" />	                   
        </td>
         <td  style="width:5%">
       <asp:linkbutton cssclass="btn btn-default " data- id="delCNTN" runat="server"><span class="fa fa-trash"></span></asp:linkbutton>
       </td>
   
    </tr>
    
    </asp:Panel>
    
      <asp:Panel ID="Panel1" runat="server">
     <td style="width:15%">
            <label for="" text-left">Content Text</label>
        </td>
     <td>
     </td>
       <td>
      <asp:TextBox ID="txtContentEmail" TextMode="MultiLine" Width="100%" Height="400px" runat="server"></asp:TextBox>    
 
   
   </td>
    </asp:Panel>
     <tr >
        <td style="width:15%">
            <label for="" text-left">Footer Image</label>
        </td>
        <td  style="width:30%">
            <asp:FileUpload class="form-control padding-horizontal-15"  ID="up_footer" runat="server" />		            
        </td>
        <td  style="width:50%">
	        <asp:Image ID="imgFooter" runat="server" Height="80px"  />     	                   
        </td>
         <td  style="width:5%">
       <asp:linkbutton  cssclass="btn btn-default " data- id="delFTR" runat="server"><span class="fa fa-trash"></span></asp:linkbutton>
       </td>
   
    </tr>
    
    </table>	
	
	<div class="col-sm-6">
       <asp:linkbutton width="150px" BackColor="LightGray"  cssclass="btn btn-default" data- id="btnsave" runat="server"><span class="fa fa-save"></span> Save </asp:linkbutton>
    <br />
    <br />
    </div>
    <asp:Label ID="Label5" runat="server" ></asp:Label>
	
    <div class="col-sm-12 right" >
    <hr />
    <h3>Send to</h3>
    </div>
   
	
	
    
    
     <table  style="text-align:left" >
     <tr>
     
      <td style="width:5%">
        
        </td>
        
        <td style="width:10%">
        <asp:RadioButton id="radAll" CssClass="radio" GroupName="radiofilter" Value="0" Text="All"   runat="server" Checked="true" AutoPostBack="true"  >
        </asp:RadioButton>
        </td>
        
        <td style="width:12%">
         <asp:RadioButton id="radMember" CssClass="radio" GroupName="radiofilter" Value="1"  runat="server" Text="Member"  AutoPostBack="true"  >
       </asp:RadioButton>
        </td>
        
        <td style="width:15%">
        <asp:RadioButton id="radMemActv" CssClass="radio" GroupName="radiofilter" Value="2"  runat="server" Text="Member Active"  AutoPostBack="true"  >
       </asp:RadioButton>
        </td>
        
        <td style="width:20%">
         <asp:RadioButton id="radProsCst" CssClass="radio" GroupName="radiofilter" Value="3" Text="Prospect Customer"  runat="server" AutoPostBack="true"   >
       </asp:RadioButton>
        </td>
        
        <td style="width:15%">
         <asp:RadioButton id="radBirth" CssClass="radio" GroupName="radiofilter" Value="4" Text="Birthday"  runat="server" AutoPostBack="true"   >
       </asp:RadioButton>
        </td>
        
        <td style="width:25%">
          <asp:CheckBox ID="chkconfirm" runat="server" Text="  I Agree" />
          <asp:LinkButton ID="btnSendEmail" Width="150px" class="btn btn-primary" runat="server"><span class="fa fa-paper-plane"></span> Send Email</asp:LinkButton>
      
        </td>
     
     </tr>
     </table>
    
    

    <div class="col-sm-12"  style="text-align:left">     
      <br />        
       
        <div class="panel-group">
    <div class="panel panel-default">
      <div class="panel-heading">
        <h4 class="panel-title">
          <a data-toggle="collapse" href="#collapse1">Click here to display list of recipients</a>
        </h4>
      </div>
      <div id="collapse1" class="panel-collapse collapse">
        <div class="panel-body">
        <%--content...........................--%>
             <asp:GridView  style="overflow:auto;" width="100%" class="center table" Font-Size="8" runat="server" 
        CssClass="table" AllowPaging="false"   ShowHeader="true" HeaderStyle-BackColor="#cccccc"
        AutoGenerateColumns="false" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        ForeColor="Black" GridLines="None"  ID="GridView1"   >
           <Columns>
               <asp:BoundField DataField="email" HeaderText="Email" 
                  SortExpression="email"  />
               <asp:BoundField DataField="firstname" HeaderText="First Name" 
                  SortExpression="firstname"  />
               <asp:BoundField DataField="lastname" HeaderText="Last Name" 
                  SortExpression="lastname"  />
               <asp:BoundField DataField="title" HeaderText="Tittle" 
                  SortExpression="title"  />
          </Columns>
          <EmptyDataTemplate>
                    <div class="col-sm-12">
                        <table class="table">
                            <tr style="background-color:#cccccc;">
                              <th>Email</th>
                              <th>First Name</th>
                              <th>Last Name</th>
                              <th>Tittle</th>
                              
                            </tr>
                            <tr style="background-color:#f8f8f8;">
                              <td colspan="13" align="center">No record</td>
                            </tr>
                         </table>
                     </div>
                </EmptyDataTemplate>
          
          
        </asp:GridView>
        
        </div>
       
      </div>
    </div>
  </div>
</div>
    


</asp:Content>