Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxAdmin_logon
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim sActivationCode As String

    Protected Sub submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit.Click
        checking()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sUserCode") = "" Then
            Response.Redirect("signin.aspx")
        End If
        Session("sDateWorkAR") = ""
        Session("sDateWorkARRec") = ""
        Session("sDateWorkGL") = ""
    End Sub

    Sub checking()
        oCnct.Open()
        ' businessid, usergroup, userid, username, password, name, neverexpired, registereddate, expiredafter, islocked, userimage


        sSQL = "Select * From iPx_profile_client_userid Where businessid = '" & ddProperty.SelectedValue & "' AND usercode='" & txtUserLogon.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            If (Trim(oSQLReader.Item("usercode")) = Trim(txtUserLogon.Text)) Then 'usercode
                If (Trim(oSQLReader.Item("password")) = Trim(txtPassLogon.Text)) Then 'password

                    If oSQLReader.Item("islocked") = "Y" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Username Is Locked !');document.getElementById('Buttonx').click()", True)
                    Else
                        Session("sUserCode") = oSQLReader.Item("usercode").ToString.Trim
                        Session("sUserLogon") = oSQLReader.Item("usercode").ToString.Trim 'create name userLogon
                        Session("sBusinessID") = ddProperty.SelectedValue
                        CheckValidation()
                        oSQLReader.Close()
                        cekGrpCust()
                        Response.Redirect("home.aspx")
                    End If
                    'End If


                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Password Is Wrong !');document.getElementById('Buttonx').click()", True)

                End If

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Username Not Registered !');document.getElementById('Buttonx').click()", True)
            End If


        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Property Is Wrong !');document.getElementById('Buttonx').click()", True)

        End If
        oCnct.Close()
    End Sub

    Sub removecookie()
        'Fetch the Cookie using its Key.
        Dim userCookie As HttpCookie = Request.Cookies("user")
        Dim passCookie As HttpCookie = Request.Cookies("pass")
        'Set the Expiry date to past date.
        If userCookie Is Nothing Then

        Else
            userCookie.Expires = DateTime.Now.AddDays(-1)
            passCookie.Expires = DateTime.Now.AddDays(-1)
            'Update the Cookie in Browser.
            Response.Cookies.Add(userCookie)
            Response.Cookies.Add(passCookie)
        End If
    End Sub

    Private Sub CheckValidation()
        oSQLReader.Close()
        Dim sSql As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()

        End If

        sSql = "Select isnull(max(validity),'1900/01/01') as validity FROM iPx_profile_clientprogram WHERE (businessid = '" & Session("sBusinessID") & "') "
        oSQLCmd = New SqlCommand(sSql, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then
            If Format(DateValue(oSQLReader.Item("validity")), "yyyy/MM/dd") >= Format(Now, "yyyy/MM/dd") Then
                'OK
                Session("sValidity") = oSQLReader.Item("validity")
                Session("sValidUntil") = DateValue(oSQLReader.Item("validity"))

            Else
                'EXPIRED
                Session("sURILICENSEEEXT") = ""

                Response.Redirect("licenseextention.aspx")
            End If

        End If


        oCnct.Close()
    End Sub

    Sub cekGrpCust()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CustomerGrp WHERE arGroup = 'CC' and businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()

        Else
            oSQLReader.Close()
            saveGrpCust()
        End If
        oCnct.Close()
    End Sub
    Sub saveGrpCust()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAR_Cfg_CustomerGrp(businessid,arGroup,Description,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','CC','Credit Card','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Protected Sub btnsignout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsignout.Click
        removecookie()
        Response.Redirect("signin.aspx")
    End Sub
End Class
