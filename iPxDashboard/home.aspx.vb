
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class iPxDashboard_home
    Inherits System.Web.UI.Page


    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If

        countClient()
        countRequest()
    End Sub

    Sub countClient()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT count(businessid) as total FROM  iPx_profile_client where status='Y' and businesstype='26'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            lbltotCLient.Text = oSQLReader.Item("total")
        End If

        oCnct.Close()
    End Sub

    Sub countRequest()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select COUNT(id) as total from iPx_profile_user_signup where status not in ('X','A') and registerfor = '26'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            lblrequest.Text = oSQLReader.Item("total")
        End If

        oCnct.Close()
    End Sub

End Class
