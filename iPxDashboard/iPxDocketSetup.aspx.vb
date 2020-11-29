Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports ExecuteQuery
Partial Class iPxDashboard_iPxDocketSetup

    Inherits System.Web.UI.Page
    'execute query
    'Dim execute As New ExecuteQuery

    'definisi string koneksi dan buka koneksi 
    Dim Cmd As SqlCommand
    Dim Rd As SqlDataReader
    Dim dt As New DataTable
    Dim queryResult As Integer
    'definisi string koneksi dan buka koneksi 
    Dim strCn As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim cn As SqlConnection = New SqlConnection(strCn)
    Dim sSQL As String

    Public Function ExecuteQuery(ByVal cmd As SqlCommand, ByVal action As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
        Using con As New SqlConnection(conString)
            cmd.Connection = con
            Select Case action
                Case "SELECT"
                    Using sda As New SqlDataAdapter()
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            Return dt
                        End Using
                    End Using
                Case "UPDATE"
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                    Exit Select
            End Select
            Return Nothing
        End Using
    End Function

    Sub reportBusinessName()
        Dim cmd As New SqlCommand("SELECT  * FROM iPx_profile_client where businesstype='26' ")
        ddlBusinessName.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlBusinessName.DataTextField = "businessname"
        ddlBusinessName.DataValueField = "businessid"
        ddlBusinessName.DataBind()
        ddlBusinessName.Items.Insert(0, "")
    End Sub
    Public Sub clear()
        txtReportID.Enabled = True
        txtReportID.Focus()
        txtReportID.Text = ""

        txtDescription.Text = ""
    End Sub
    Public Sub disabled()
        txtReportID.Enabled = False


    End Sub
    Public Sub grid()
        cn.Open()

        Dim cmd As SqlDataAdapter = New SqlDataAdapter("Select businessname,code,description,fileName From iPxAcctDocket_Report as a LEFT JOIN iPx_profile_client as b ON b.businessid=a.businessid ", cn)
        Dim dt As DataTable = New DataTable()
        cmd.Fill(dt)

        GridView1.DataSource = dt
        GridView1.DataBind()

        cn.Close()

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
        Call grid()
    End Sub
    Public Sub selectrow()


        txtReportID.Text = GridView1.SelectedRow.Cells(2).Text
        Call showData()
    End Sub
    Public Sub showData()
        cn.Open()
        sSQL = "Select * From iPxAcctDocket_Report where code='" & txtReportID.Text & "'"
        Cmd = New SqlCommand(sSQL, cn)
        Rd = Cmd.ExecuteReader
        If Rd.Read Then
            'Call dsblPrm()
            Dim business As String = Rd.Item("businessid")
            ddlGrpID.SelectedValue = Rd.Item("Grp")
            txtDescription.Text = Rd.Item("description")
            'ddlGrpID.Text = Rd.Item("groupid")
            cn.Close()
            Call reportBusinessName()
            ddlBusinessName.SelectedValue = business.Trim
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data is already exist!');", True)

        Else
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data is not found!');", True)
            'txtDescription.Text = ""
            'txtCurrency.Text = ""
            txtReportID.Focus()
            cn.Close()
        End If


    End Sub
    Public Sub saveData()
        cn.Open()
        Dim cek As String
        If chkIsactive.Checked Then
            cek = "Y"

        Else
            cek = "N"

        End If

        Dim fileName As String = System.IO.Path.GetFileName(FileUpload1.FileName)
        FileUpload1.SaveAs(Server.MapPath("~/iPxReportFile/") & fileName)

        sSQL = "insert into iPxAcctDocket_Report (businessid,code,Grp,Description,fileName,isActive)values ('" & ddlBusinessName.SelectedValue & "','" & txtReportID.Text & "','" & ddlGrpID.SelectedValue & "','" & txtDescription.Text & "','" & fileName & "','" & cek & "') "
        Cmd = New SqlCommand(sSQL, cn)
        Cmd.ExecuteNonQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been saved!');", True)
        clear()
        'dsblPrm()
        cn.Close()
    End Sub
    Public Sub update()
        cn.Open()
        Dim cek As String
        If chkIsactive.Checked Then
            cek = "Y"

        Else
            cek = "N"

        End If

        Cmd = New SqlCommand("Update iPxAcctDocket_Report Set description='" & txtDescription.Text & "',Grp='" & ddlGrpID.SelectedValue & "',Active='" & cek & "' Where code='" & txtReportID.Text & "' and businessid='" & ddlBusinessName.SelectedValue & "'", cn)
        Cmd.ExecuteNonQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Update Successful!');", True)
        cn.Close()
    End Sub
    Public Sub checkUpdate()
        cn.Open()

        Cmd = New SqlCommand("Select *  From iPxAcctDocket_Report where code='" & txtReportID.Text & "' and businessid='" & ddlBusinessName.SelectedValue & "'", cn)
        Rd = Cmd.ExecuteReader
        If Rd.HasRows Then
            cn.Close()
            Call update()
            Dim OpenFileobj, FSOobj, FilePath

            FilePath = Server.MapPath("~/iPxReportFile/" & GridView1.SelectedRow.Cells(3).Text)
            FSOobj = Server.CreateObject("Scripting.FileSystemObject")
            If FSOobj.fileExists(FilePath) Then

                Response.Write(FSOobj.DeleteFile(FilePath))

            End If
            FSOobj = Nothing

            Dim fileName As String = System.IO.Path.GetFileName(FileUpload1.FileName)
            FileUpload1.SaveAs(Server.MapPath("~/iPxReportFile/") & fileName)
        Else
            cn.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert(' Already Exist!');", True)
        End If
    End Sub

    Public Sub delete()
        cn.Open()

        Cmd = New SqlCommand("Delete from iPxAcctDocket_Report Where  code='" & txtReportID.Text & "' and businessid='" & ddlBusinessName.SelectedValue & "'", cn)
        Cmd.ExecuteNonQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Delete Successful!');", True)
        Call clear()
        cn.Close()

        Dim OpenFileobj, FSOobj, FilePath

        FilePath = Server.MapPath("~/iPxReportFile/" & GridView1.SelectedRow.Cells(3).Text)
        FSOobj = Server.CreateObject("Scripting.FileSystemObject")
        If FSOobj.fileExists(FilePath) Then

            Response.Write(FSOobj.DeleteFile(FilePath))

        End If
        FSOobj = Nothing
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'If Session("sUserCode") = "" Then
        '    Response.Redirect("signin.aspx")
        'End If

        If Not IsPostBack Then
            Call reportBusinessName()
            Call grid()
            'Call dsblPrm()
            'Call enblP4()
            'rbP5.Checked = True
            imgDelete.Attributes("onclick") = "if(!confirm('Do you want to delete ?')){ return false; };"
        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Call selectrow()

        'Call enblP4()
        'pnlGrid.Visible = False
        'pnlHotelProperty.Visible = True
    End Sub


    Sub enabledBtn()
        imgSave.Enabled = True
        imgCancel.Enabled = True
        imgDelete.Enabled = True
    End Sub


    Protected Sub txtRateItemID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReportID.TextChanged
        Call showData()
        Call enabledBtn()
    End Sub

    Protected Sub ddlBusinessName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBusinessName.SelectedIndexChanged
        'reportBusinessName()
        grid()
    End Sub

    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgSave.Click
        cn.Open()

        Cmd = New SqlCommand("Select *  From iPxAcctDocket_Report where  code='" & txtReportID.Text & "' and businessid='" & ddlBusinessName.SelectedValue & "' ", cn)
        Rd = Cmd.ExecuteReader

        If Rd.HasRows Then
            cn.Close()

            Call checkUpdate()
            Call grid()


        Else
            cn.Close()
            If txtDescription.Text.Length = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Rate Item ID cannot null!');", True)

            Else
                Call saveData()
                Call grid()

            End If

        End If
    End Sub

    Protected Sub imgCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgCancel.Click
        Call clear()
        'Call dsblPrm()
        'rbP5.Checked = True
    End Sub

    Protected Sub imgDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgDelete.Click
        If txtDescription.Text.Length = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('no data is deleted!!, preview the data that you want to delete.');", True)
        Else
            Call delete()
            Call grid()
        End If
    End Sub

    Protected Sub imgNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgNew.Click
        pnlHotelProperty.Visible = True
        Call clear()
        Call enabledBtn()
        'Call dsblPrm()
        'rbP5.Checked = True
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click

    End Sub
End Class