Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxInvoiceFormat
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim cIpx As New iPxClass
    Dim sSQL As String
    Dim oSQLReader As SqlDataReader
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='35'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddOp "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddOp").ToString = "Y" Then
                lbSave.Enabled = True
            Else
                lbSave.Enabled = False
            End If
        Else
            lbSave.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub InvoiceFormat()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Invoice_Format "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbHeader.Text = oSQLReader.Item("header1").ToString
            tbFooter1.Text = oSQLReader.Item("footer1").ToString
            tbFooter2.Text = oSQLReader.Item("footer2").ToString
            tbBank.Text = oSQLReader.Item("bank").ToString
            tbNoReq.Text = oSQLReader.Item("noReq").ToString
            lblHeader.Text = Replace(tbHeader.Text, vbLf, "<br/>")
            lblFooter1.Text = Replace(tbFooter1.Text, vbLf, "<br/>")
            lblFooter2.Text = Replace(tbFooter2.Text, vbLf, "<br/>")
            lblBank.Text = tbBank.Text
            lblNoReq.Text = tbNoReq.Text
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveFormat()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO iPxAcctAR_Invoice_Format(businessid,id,header1,bank,noReq,footer1,footer2) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','F1','" & Replace(tbHeader.Text, "'", "''") & "','" & Replace(tbBank.Text, "'", "''") & "','" & Replace(tbNoReq.Text, "'", "''") & "',"
        sSQL += "'" & Replace(tbFooter1.Text, "'", "''") & "','" & Replace(tbFooter2.Text, "'", "''") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data save successfully !');document.getElementById('Buttonx').click()", True)
        InvoiceFormat()

    End Sub
    Sub updateFormat()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Invoice_Format SET header1='" & Replace(tbHeader.Text, "'", "''") & "',bank='" & Replace(tbBank.Text, "'", "''") & "', "
        sSQL += "noReq='" & Replace(tbNoReq.Text, "'", "''") & "',footer1='" & Replace(tbFooter1.Text, "'", "''") & "', "
        sSQL += "footer2='" & Replace(tbFooter2.Text, "'", "''") & "' "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('updating successfully !');document.getElementById('Buttonx').click()", True)
        InvoiceFormat()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            InvoiceFormat()
            tbBank.Visible = False
            tbFooter1.Visible = False
            tbFooter2.Visible = False
            tbHeader.Visible = False
            tbNoReq.Visible = False
            lbSave.Text = "<i class='fa fa-edit'></i> Edit"
        End If
        UserAcces()
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If lbSave.Text = "<i class='fa fa-edit'></i> Edit" Then
            tbBank.Visible = True
            tbFooter1.Visible = True
            tbFooter2.Visible = True
            tbHeader.Visible = True
            tbNoReq.Visible = True
            lblBank.Visible = False
            lblFooter1.Visible = False
            lblFooter2.Visible = False
            lblHeader.Visible = False
            lblNoReq.Visible = False
            lbSave.Text = "<i class='fa fa-save'></i> Save"
        ElseIf lbSave.Text = "<i class='fa fa-save'></i> Save" Then
            If tbHeader.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Format Header!');", True)
                tbHeader.Focus()
            ElseIf tbFooter1.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Format Footer 1!');", True)
                tbFooter1.Focus()
            ElseIf tbBank.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Bank Name!');", True)
                tbBank.Focus()
            ElseIf tbNoReq.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter No Req!');", True)
                tbNoReq.Focus()
            ElseIf tbFooter2.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Format Footer 2!');", True)
                tbFooter2.Focus()
            Else
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT id FROM iPxAcctAR_Invoice_Format WHERE businessid ='" & Session("sBusinessID") & "'"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    updateFormat()
                Else
                    oSQLReader.Close()
                    saveFormat()
                End If
            End If
            tbBank.Visible = False
            tbFooter1.Visible = False
            tbFooter2.Visible = False
            tbHeader.Visible = False
            tbNoReq.Visible = False
            lblBank.Visible = True
            lblFooter1.Visible = True
            lblFooter2.Visible = True
            lblHeader.Visible = True
            lblNoReq.Visible = True
            lbSave.Text = "<i class='fa fa-edit'></i> Edit"
        End If
    End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        If lbSave.Text = "<i class='fa fa-edit'></i> Edit" Then
            Response.Redirect("home.aspx")
        ElseIf lbSave.Text = "<i class='fa fa-save'></i> Save" Then
            tbBank.Visible = False
            tbFooter1.Visible = False
            tbFooter2.Visible = False
            tbHeader.Visible = False
            tbNoReq.Visible = False
            lblBank.Visible = True
            lblFooter1.Visible = True
            lblFooter2.Visible = True
            lblHeader.Visible = True
            lblNoReq.Visible = True
            lbSave.Text = "<i class='fa fa-edit'></i> Edit"
        End If
    End Sub
End Class
