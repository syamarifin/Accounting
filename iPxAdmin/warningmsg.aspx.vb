

Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Threading
Partial Class iPxAdmin_warningmsg
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Session("sMessage") = "||line1|line2|Line3|"

        If Session("sWarningID") = "0" Then
            confirmationstep1.Visible = True
            confirmationfooterokonly.Visible = True
            confirmationfooteryesno.Visible = False
        Else
            confirmationstep1.Visible = True
            confirmationfooterokonly.Visible = False
            confirmationfooteryesno.Visible = True

        End If

    End Sub

    Protected Sub btnOkonly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkonly.Click
        Response.Redirect(Session("sUrlOKONLY"))
    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Response.Redirect(Session("sUrlYES"))
    End Sub

    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Response.Redirect(Session("sUrlNO"))
    End Sub
End Class

