

Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Threading
Partial Class iPxDashboard_queryRequest
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    Dim cipx As New iPxClass




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
        '        SELECT        TOP (200) id, hotelname, noofroom, contactperson, mobile, email, password, address, country, city, website, registerfor, registerdate, status, clerknotes, guestnotes, 
        '                         approvalnotes, approvaldate
        'FROM            iPx_profile_user_signup

        If txtbusinesname.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.hotelname like '%" & txtbusinesname.Text.Trim & "%') "
        End If

        If txtaddress.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.address like '%" & txtaddress.Text.Trim & "%') "
        End If

        If txtmobile.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.mobile like '%" & txtmobile.Text.Trim & "%') "
        End If

        If txtemail.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.email like '%" & txtemail.Text.Trim & "%') "
        End If

        If txtwebsite.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.website like '%" & txtwebsite.Text.Trim & "%') "
        End If

        If txtPerUntl.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.registerdate <= '" & cipx.Date2IsoDate(txtPerUntl.Text.Trim) & "') "
        End If

        If txtPerFrom.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " AND (iPx_profile_user_signup.registerdate >= '" & cipx.Date2IsoDate(txtPerFrom.Text.Trim) & "') "
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopUser", "hideModal()", True)

        Response.Redirect("viewRequest.aspx?sCondition=" & sSQL)
    End Sub

    Protected Sub btncal2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncal2.Click
        Calendar2.Visible = True
    End Sub

    Protected Sub btncal3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncal3.Click
        Calendar3.Visible = True
    End Sub


    Protected Sub Calendar2_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Calendar2.SelectionChanged
        txtPerFrom.Text = Format(Calendar2.SelectedDate, "dd/MM/yyyy")
        Calendar2.Visible = False
    End Sub

    Protected Sub Calendar3_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Calendar3.SelectionChanged
        txtPerUntl.Text = Format(Calendar3.SelectedDate, "dd/MM/yyyy")
        Calendar3.Visible = False
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Session("sCondition") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopUser", "hideModal()", True)
        Response.Redirect("viewRequest.aspx?sCondition=")
    End Sub


End Class
