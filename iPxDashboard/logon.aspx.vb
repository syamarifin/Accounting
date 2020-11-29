
Partial Class iPxDashboard_logon
    Inherits System.Web.UI.Page

    Protected Sub submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit.Click
        Session("sBusinessID") = ddProperty.SelectedValue
        Response.Redirect("HOME.ASPX")
    End Sub
End Class
