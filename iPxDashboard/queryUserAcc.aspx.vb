

Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Threading
Partial Class iPxDashboard_queryUserAcc
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If

        If Not Page.IsPostBack Then

        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopUser", "clearModal()", True)
        End If
    End Sub


    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click

        Select Case ddStatus.SelectedValue
            Case "0" 'all account
                Session("sCondition") = ""
            Case "N" 'status
                Session("sCondition") = " where iPx_profile_client.status='Y' "
            Case "I" 'in-status
                Session("sCondition") = " where iPx_profile_client.status='N' "

        End Select
     
        If txtbusinesname.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_client.businessname like '%" & txtbusinesname.Text.Trim & "%') "
        End If

        If txtaddress.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_client.address like '%" & txtaddress.Text.Trim & "%') "
        End If

        If txtphone.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_client.phone like '%" & txtphone.Text.Trim & "%') "
        End If

        If txtemail.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_client.email like '%" & txtemail.Text.Trim & "%') "
        End If

        

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopUser", "hideModal()", True)

        Response.Redirect("viewUserAcc.aspx?sCondition=" & sSQL)
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Session("sCondition") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopUser", "hideModal()", True)
        Response.Redirect("viewUserAcc.aspx?sCondition=")
    End Sub


End Class
