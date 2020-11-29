Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Data

Partial Class iPxDashboard_createvoucher
    Inherits System.Web.UI.Page
    Dim sSql As String
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
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

    Protected Sub rbAllList_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAllList.CheckedChanged
        iPxCNCT.SelectCommand = "SELECT      * FROM         iPx_general_voucher "


    End Sub

    Protected Sub rbUsedList_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbUsedList.CheckedChanged
        iPxCNCT.SelectCommand = "SELECT      * FROM         iPx_general_voucher where status='X'"
    End Sub

    Protected Sub rbActiveList_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbActiveList.CheckedChanged
        iPxCNCT.SelectCommand = "SELECT      * FROM         iPx_general_voucher where status='A'"

    End Sub

    Protected Sub btnGenerateVcr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateVcr.Click

        Select Case DropDownList1.SelectedValue
            Case "Starter"

                Session("sProgram") = "P1"

                Session("sRegisteredPackage") = "LC"
            Case "Business"

                Session("sProgram") = "P3"
                Session("sRegisteredPackage") = "LC"
            Case "Professional"

                Session("sProgram") = "P4"
                Session("sRegisteredPackage") = "LC"
        End Select

        If DropDownList2.SelectedValue = "Monthly" Then
            Session("cPyMode") = "M1"
        Else

            Session("cPyMode") = "Y1"
        End If
        random()
        insert()
        GridView1.DataBind()
        Session("sMessage") = "Voucher " & DropDownList1.SelectedValue & " - " & DropDownList2.SelectedValue & "  Has Been Created ! |"

        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "createvoucher.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")
    End Sub
    Sub insert()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSql, oCnct)
        sSql = "insert into iPx_general_voucher ( voucherno, description, status, createdby, usedby ) values( '" & Session("sProgram") & Session("sRegisteredPackage") & Session("cPyMode") & Session("sRandom") & "', '" & DropDownList1.SelectedValue & " - " & DropDownList2.SelectedValue & "', 'A','" & Session("sName") & "','-' ) "
        oSQLCmd.CommandText = sSql
        oSQLCmd.ExecuteNonQuery()
        oCnct.Close()
    End Sub
    Sub random()
        Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        Dim numbers As String = "1234567890"

        Dim characters As String = numbers

        characters += Convert.ToString(alphabets) & numbers

        Dim length As Integer = Integer.Parse(10)
        Dim otp As String = String.Empty
        For i As Integer = 0 To length - 1
            Dim character As String = String.Empty
            Do
                Dim index As Integer = New Random().Next(0, characters.Length)
                character = characters.ToCharArray()(index).ToString()
            Loop While otp.IndexOf(character) <> -1
            otp += character
        Next
        Session("sRandom") = otp
    End Sub

    Protected Sub rbMonthly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbMonthly.CheckedChanged
        iPxCNCT.SelectCommand = "SELECT      * FROM         iPx_general_voucher where status='A' "

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "Create Voucher"
        If Not Page.IsPostBack Then
            iPxCNCT.SelectCommand = "SELECT      * FROM         iPx_general_voucher where status='A'"

        End If
    End Sub
End Class
