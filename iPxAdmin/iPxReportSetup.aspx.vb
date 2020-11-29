Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports ExecuteQuery
Partial Class iPXADMIN_iPxReportSetup

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

    Sub reportGRp()
        Dim cmd As New SqlCommand("SELECT  * FROM CFG_ReportGRP where moduleID='" & ddlModul.Text.Trim & "' ")
        ddlGrpID.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlGrpID.DataTextField = "description"
        ddlGrpID.DataValueField = "groupID"
        ddlGrpID.DataBind()
    End Sub
    Public Sub clear()
        txtReportID.Enabled = True
        txtReportID.Focus()
        txtReportID.Text = ""

        txtDescription.Text = ""


        txtp1.Text = ""
    End Sub
    Public Sub disabled()
        txtReportID.Enabled = False


    End Sub
    Public Sub grid()
        cn.Open()

        Dim cmd As SqlDataAdapter = New SqlDataAdapter("Select reportID,description,reportname From CFG_ReportList where moduleID='" & ddlModul.Text.Trim & "' ", cn)
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


        txtReportID.Text = GridView1.SelectedRow.Cells(1).Text

        Call showData()
    End Sub
    Public Sub showData()
        cn.Open()

        Cmd = New SqlCommand("Select *From CFG_ReportList where reportid='" & txtReportID.Text & "' and moduleID='" & ddlModul.Text.Trim & "'", cn)
        Rd = Cmd.ExecuteReader
        If Rd.Read Then
            Call dsblPrm()
            ddlGrpID.Text = Rd.Item("groupid")
            txtReportID.Text = Rd.Item("reportid")
            txtDescription.Text = Rd.Item("description")
            ddlGrpID.Text = Rd.Item("groupid")

            txtp1.Text = Rd.Item("prm1label")
            txtQueryP1.Text = Rd.Item("prm1query")
            txtp2.Text = Rd.Item("prm2label")
            txtQueryP2.Text = Rd.Item("prm2query")
            txtp3.Text = Rd.Item("prm3label")
            txtQueryP3.Text = Rd.Item("prm3query")
            txtp4.Text = Rd.Item("prm4label")
            txtQueryP4.Text = Rd.Item("prm4query")

            Dim type1, type2, type3, type4 As String
            type1 = Rd.Item("prm1type")
            type2 = Rd.Item("prm2type")
            type3 = Rd.Item("prm3type")
            type4 = Rd.Item("prm4type")

            If type1 <> " " Then
                ddlp1.Text = type1
                Call enblP1()
            End If

            If type2 <> " " Then
                ddlp2.Text = type2
                Call enblP2()
            End If

            If type3 <> " " Then
                ddlp3.Text = type3
                Call enblP3()
            End If

            If type4 <> " " Then
                ddlp4.Text = type4
                Call enblP4()
            End If

            If type1 = "C" Then
                txtQueryP1.Enabled = True
            End If
            If type2 = "C" Then
                txtQueryP2.Enabled = True
            End If
            If type3 = "C" Then
                txtQueryP3.Enabled = True
            End If
            If type4 = "C" Then
                txtQueryP4.Enabled = True
            End If
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data is already exist!');", True)

        Else
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data is not found!');", True)
            'txtDescription.Text = ""
            'txtCurrency.Text = ""
            txtReportID.Focus()
        End If

        cn.Close()
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


        Cmd = New SqlCommand("insert into cfg_ReportList (moduleID,groupID,reportID,subgroupid,description,reportname,reportfile,component,prm1type,prm1label,prm1query,prm2type,prm2label,prm2query,prm3type,prm3label,prm3query,prm4type,prm4label,prm4query,Active)values ('" & ddlModul.Text & "','" & ddlGrpID.Text & "','" & txtReportID.Text & "','','" & txtDescription.Text & "','" & fileName & "','" & fileName & "','','" & ddlp1.Text & "','" & txtp1.Text & "','" & txtQueryP1.Text & "','" & ddlp2.Text & "','" & txtp2.Text & "','" & txtQueryP2.Text & "','" & ddlp3.Text & "','" & txtp3.Text & "','" & txtQueryP3.Text & "','" & ddlp4.Text & "','" & txtp4.Text & "','" & txtQueryP4.Text & "','" & cek & "') ", cn)
        Cmd.ExecuteNonQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been saved!');", True)
        clear()
        dsblPrm()
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

        Cmd = New SqlCommand("Update cfg_ReportList Set description='" & txtDescription.Text & "',prm1type='" & ddlp1.Text & "',prm1label='" & txtp1.Text & "',prm1query='" & txtQueryP1.Text & "',prm2type='" & ddlp2.Text & "',prm2label='" & txtp2.Text & "',prm2query='" & txtQueryP2.Text & "',prm3type='" & ddlp3.Text & "',prm3label='" & txtp3.Text & "',prm3query='" & txtQueryP3.Text & "',prm4type='" & ddlp4.Text & "',prm4label='" & txtp4.Text & "',prm4query='" & txtQueryP4.Text & "',Active='" & cek & "' Where reportid='" & txtReportID.Text & "' and moduleID='" & ddlModul.Text.Trim & "'", cn)
        Cmd.ExecuteNonQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Update Successful!');", True)
        cn.Close()
    End Sub
    Public Sub checkUpdate()
        cn.Open()

        Cmd = New SqlCommand("Select *  From cfg_ReportList where reportid='" & txtReportID.Text & "' and moduleID='" & ddlModul.Text.Trim & "'", cn)
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

        Cmd = New SqlCommand("Delete from cfg_ReportList Where  reportid='" & txtReportID.Text & "' and moduleID='" & ddlModul.Text.Trim & "'", cn)
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
        If Session("sPossition") <> "0" Then
            Session("sMessage") = "Sorry, Your not access !| ||"
            Session("sWarningID") = "0"
            Session("sUrlOKONLY") = "iPxCrmHome.aspx"
            Session("sUrlYES") = "http://www.thepyxis.net"
            Session("sUrlNO") = "http://www.thepyxis.net"
            Response.Redirect("warningmsg.aspx")
        Else
            If Not IsPostBack Then
                Call reportGRp()
                Call grid()
                Call dsblPrm()
                'Call enblP4()
                rbP5.Checked = True
                imgDelete.Attributes("onclick") = "if(!confirm('Do you want to delete ?')){ return false; };"
            End If
        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Call selectrow()

        Call enblP4()
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
    Sub enblP1()
        ddlp1.Enabled = True
        txtp1.Enabled = True
    End Sub
    Sub enblP2()
        ddlp1.Enabled = True
        txtp1.Enabled = True
        ddlp2.Enabled = True
        txtp2.Enabled = True

    End Sub
    Sub enblP3()
        ddlp1.Enabled = True
        txtp1.Enabled = True
        ddlp2.Enabled = True
        txtp2.Enabled = True
        ddlp3.Enabled = True
        txtp3.Enabled = True
    End Sub
    Sub enblP4()
        ddlp1.Enabled = True
        txtp1.Enabled = True
        ddlp2.Enabled = True
        txtp2.Enabled = True
        ddlp3.Enabled = True
        txtp3.Enabled = True
        ddlp4.Enabled = True
        txtp4.Enabled = True
    End Sub
    Sub dsblPrm()
        ddlp1.Enabled = False
        txtp1.Enabled = False
        ddlp2.Enabled = False
        txtp2.Enabled = False
        ddlp3.Enabled = False
        txtp3.Enabled = False
        ddlp4.Enabled = False
        txtp4.Enabled = False
        txtQueryP1.Enabled = False
        txtQueryP2.Enabled = False
        txtQueryP3.Enabled = False
        txtQueryP4.Enabled = False
        ddlp1.Text = ""
        ddlp2.Text = ""
        ddlp3.Text = ""
        ddlp4.Text = ""
        ddlp1.Text = ""
        ddlp2.Text = ""
        ddlp3.Text = ""
        ddlp4.Text = ""
        txtp1.Text = ""
        txtp2.Text = ""
        txtp3.Text = ""
        txtp4.Text = ""
        txtQueryP1.Text = ""
        txtQueryP2.Text = ""
        txtQueryP3.Text = ""
        txtQueryP4.Text = ""


    End Sub

    Protected Sub rbP1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbP1.CheckedChanged
        Call dsblPrm()
        Call enblP1()
    End Sub

    Protected Sub rbP2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbP2.CheckedChanged
        Call dsblPrm()
        Call enblP2()
    End Sub

    Protected Sub rbP3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbP3.CheckedChanged
        Call dsblPrm()
        Call enblP3()
    End Sub

    Protected Sub rbP4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbP4.CheckedChanged
        Call dsblPrm()
        Call enblP4()
    End Sub

    Protected Sub rbP5_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbP5.CheckedChanged
        Call dsblPrm()
    End Sub

    Protected Sub ddlp1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlP1.SelectedIndexChanged
        If ddlp1.Text = "C" Then
            txtQueryP1.Enabled = True
        Else
            txtQueryP1.Enabled = False
        End If
    End Sub

    Protected Sub ddlp2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlP2.SelectedIndexChanged
        If ddlp2.Text = "C" Then
            txtQueryP2.Enabled = True
        Else
            txtQueryP2.Enabled = False
        End If
    End Sub

    Protected Sub ddlp3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlP3.SelectedIndexChanged
        If ddlp3.Text = "C" Then
            txtQueryP3.Enabled = True
        Else
            txtQueryP3.Enabled = False
        End If
    End Sub

    Protected Sub ddlp4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlP4.SelectedIndexChanged
        If ddlp4.Text = "C" Then
            txtQueryP4.Enabled = True
        Else
            txtQueryP4.Enabled = False
        End If
    End Sub

    Protected Sub ddlModul_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlModul.SelectedIndexChanged
        reportGRp()
        grid()
    End Sub

    Protected Sub imgSave_Click(sender As Object, e As EventArgs) Handles imgSave.Click
        cn.Open()

        Cmd = New SqlCommand("Select *  From cfg_ReportList where  reportID='" & txtReportID.Text & "' and moduleID='" & ddlModul.Text.Trim & "' ", cn)
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

    Protected Sub imgCancel_Click(sender As Object, e As EventArgs) Handles imgCancel.Click
        Call clear()
        Call dsblPrm()
        rbP5.Checked = True
    End Sub

    Protected Sub imgDelete_Click(sender As Object, e As EventArgs) Handles imgDelete.Click
        If txtDescription.Text.Length = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('no data is deleted!!, preview the data that you want to delete.');", True)
        Else
            Call delete()
            Call grid()
        End If
    End Sub

    Protected Sub imgNew_Click(sender As Object, e As EventArgs) Handles imgNew.Click
        pnlHotelProperty.Visible = True
        Call clear()
        Call enabledBtn()
        Call dsblPrm()
        rbP5.Checked = True
    End Sub

    Protected Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click

    End Sub
End Class