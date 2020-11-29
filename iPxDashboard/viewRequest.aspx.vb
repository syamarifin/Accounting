
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class iPxDashboard_viewRequest
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

        For Each row As GridViewRow In GridView1.Rows

            If row.RowIndex = GridView1.SelectedIndex Then
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
            Else
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Click to select this row."
            End If
        Next

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "getcodeApp" Then 'approve
            Dim cod As String
            cod = e.CommandArgument.ToString
            Session("sRequest") = True
            Session("sEmailClient") = cod

            Response.Redirect("UserAccess.aspx")

            
        End If
        If e.CommandName = "getcodeRjc" Then
            Dim cod As String
            cod = e.CommandArgument.ToString
            Session("sEmailClient") = cod
            update("X")
        End If

    End Sub

    Public Sub grid()
        oCnct.Open()

        sSQL = "select * from iPx_profile_user_signup where status not in ('X','A') and registerfor = '26' "
        If Session("sCondition") <> "" Then
            sSQL = sSQL & Session("sCondition")
            Session("sCondition") = ""
            sSQL = sSQL & " order by registerdate desc"
        Else
            sSQL = sSQL & " order by registerdate desc"

        End If
        Dim cmd As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim dt As DataTable = New DataTable()
        cmd.Fill(dt)

        GridView1.DataSource = dt
        GridView1.DataBind()

        oCnct.Close()

    End Sub

    Function update(ByVal status As String) As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPx_profile_user_signup SET status= '" & status & "' where email='" & Session("sEmailClient") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        Session("sMessage") = "Reject Successfull !| ||"
        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "viewRequest.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")

        update = ""
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "Request"

        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            grid()
        End If

    End Sub

    Protected Sub btnfilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilter.Click
        Response.Redirect("queryRequest.aspx")
    End Sub

    Protected Sub btnReject_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReject.ServerClick
        Response.Redirect("viewReject.aspx")
    End Sub
End Class
