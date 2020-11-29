Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxAdmin_licenseextention
    Inherits System.Web.UI.Page

    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ' removecookie()
        'Response.Redirect(Session("sURILICENSEEEXT"))
        Response.Redirect("logon.aspx")
    End Sub

    Protected Sub btnPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPayment.Click
        Dim nAmt As Double
        Dim nRoom As Integer
        Dim cIpx As New iPxClass
        'Dim cClientCurrency As String
        ' baca jumlah kamar yang active
        nRoom = 1
        'sSQL = "SELECT isnull(COUNT(roomno),0) AS Expr1 FROM iPxPMS_cfg_roomno WHERE (businessid = '" & Session("sBusinessID") & "') AND (isactive = 'Y') "
        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)
        'oSQLReader = oSQLCmd.ExecuteReader
        'If oSQLReader.Read Then
        '    nRoom = oSQLReader.Item("Expr1")
        'End If
        'oSQLReader.Close()
        oCnct = Nothing
        'cClientCurrency = cIpx.GetClientCurrency(Session("sBusinessID"))
        Session("sProgramName") = DropDownList1.SelectedValue
        Session("sPackage") = DropDownList2.SelectedValue
        Session("sPayfor") = "License Extention"
        Session("sQty") = 1
        Select Case DropDownList1.SelectedValue
            Case "Starter"
                'If cClientCurrency.Trim = "IDR" Then
                nAmt = 30000 * nRoom
                'Else
                'nAmt = 30 * nRoom
                'End If
                Session("sProgram") = "01"

                Session("sRegisteredPackage") = "1"
            Case "Business"
                'If cClientCurrency.Trim = "IDR" Then
                nAmt = 40000 * nRoom
                'Else
                'nAmt = 40 * nRoom
                'End If
                Session("sProgram") = "02"
                Session("sRegisteredPackage") = "2"
            Case "Professional"
                'If cClientCurrency.Trim = "IDR" Then
                nAmt = 50000 * nRoom
                'Else
                'nAmt = 50 * nRoom
                'End If
                Session("sProgram") = "03"
                Session("sRegisteredPackage") = "3"
        End Select

        If DropDownList2.SelectedValue = "Monthly" Then
            Session("cPyMode") = "M"
        Else
            nAmt = nAmt * 11
            Session("cPyMode") = "Y"
        End If

        Session("sPaymentCode") = "R"  ' Site License
        Session("nPackageQTY") = 1
        Session("nAmt") = nAmt
        Session("sUrlOKONLY") = "signin.aspx"

        'Response.Redirect("payment.aspx")
        Response.Redirect("paymentmode.aspx")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblSite.Text = "For " & Session("sUserName")

        'If Session("sURILICENSEEEXT") = "" Then
        '    Session("sURILICENSEEEXT") = Request.UrlReferrer.AbsoluteUri

        'End If
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
End Class