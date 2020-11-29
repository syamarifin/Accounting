Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAccesUser
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='34'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddOp "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddOp").ToString = "Y" Then
                lbAddAcc.Enabled = True
            Else
                lbAddAcc.Enabled = False
            End If
        Else
            lbAddAcc.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub ListUserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT b.description,a.*, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='35'and x.active='Y' and x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "') as EditOp "
        sSQL += "FROM iPx_profile_client_userid as a "
        sSQL += "INNER JOIN iPx_profile_client_usergroup as b ON b.usergroup=a.usergroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "  "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.userid asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvUserAcces.DataSource = dt
                    gvUserAcces.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvUserAcces.DataSource = dt
                    gvUserAcces.DataBind()
                    gvUserAcces.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Option") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            Session("sCondition") = ""
            ListUserAcces()
        End If
        UserAcces()
    End Sub

    Protected Sub lbAddAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddAcc.Click
        Session("sNewCode") = True
        Session("sCode") = ""
        Response.Redirect("iPxAccesUserAdd.aspx")
    End Sub

    Protected Sub gvUserAcces_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUserAcces.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sCode") = e.CommandArgument
            Session("sNewCode") = False
            Response.Redirect("iPxAccesUserAdd.aspx")
        End If
    End Sub
End Class
