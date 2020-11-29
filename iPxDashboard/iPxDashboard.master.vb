
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class iPxDashboard_iPxDashboard
    Inherits System.Web.UI.MasterPage

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' System.Threading.Thread.Sleep(100)
        lblUser.Text = Session("sName")

        countRequest()
        countUserAccess()

        'Dim RQ, UA As Integer

        'RQ = lblrequest.Text
        'UA = lbluseraccess.Text
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

    Sub countUserAccess()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "select count(businessid) as total  from iPx_profile_client WHERE status='Y' and businesstype='26'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            lbluseraccess.Text = oSQLReader.Item("total")
        End If

        oCnct.Close()
    End Sub


End Class

