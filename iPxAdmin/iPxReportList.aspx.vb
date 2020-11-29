Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
'Imports ExecuteQuery
Partial Class iPxAdmin_iPxReportList
    Inherits System.Web.UI.Page
    'execute query
    ' Dim execute As New ExecuteQuery

    'definisi string koneksi dan buka koneksi 
    Dim Cmd As SqlCommand
    Dim Rd As SqlDataReader
    Dim dt As New DataTable
    Dim queryResult As Integer
    'definisi string koneksi dan buka koneksi 
    Dim strCn As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim cn As SqlConnection = New SqlConnection(strCn)
    Dim ciPx As New iPxClass
    Public Function ExecuteQueryFO(ByVal cmd As SqlCommand, ByVal action As String) As DataTable
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



    Sub rptGrp()
        Dim cmd As New SqlCommand("SELECT  * FROM iPx_cfg_ReportGRP where moduleID='" & Session("sProgram") & "' ")
        ddlGrpID.DataSource = ExecuteQueryFO(cmd, "SELECT")
        ddlGrpID.DataTextField = "description"
        ddlGrpID.DataValueField = "groupID"
        ddlGrpID.DataBind()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "getReport" Then
            Dim rptID As String
            rptID = e.CommandArgument.ToString
            Session("reportID") = rptID
            clearSession()
            'Call selectrow()
            'pnlGrid.Visible = False
            'pnlHotelProperty.Visible = True
            Call clearTextbox()
            Call showData()

            btnPreview.Enabled = True

        End If
    End Sub


    Public Sub grid()
        cn.Open()
        Dim sSQL As String = "SELECT  * FROM iPx_cfg_ReportList where (businessid IS NULL or businessid='" & Session("sBusinessID") & "') and moduleID='" & Session("sProgram") & "' and groupID='" & ddlGrpID.Text & "' order by description "
        Dim cmd As SqlDataAdapter = New SqlDataAdapter(sSQL, cn)
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


        'Call propertyID()
        Call showData()

    End Sub

    Public Sub showData()
        cn.Open()

        'Cmd = New SqlCommand("Select *From iPx_cfg_ReportList where  reportID='" & GridView1.SelectedRow.Cells(2).Text & "'", cn)
        Cmd = New SqlCommand("Select *From iPx_cfg_ReportList where moduleID='" & Session("sProgram") & "' and reportID='" & Session("reportID") & "'", cn)
        Rd = Cmd.ExecuteReader
        If Rd.Read Then
            'label
            Dim p1lbl, p2lbl, p3lbl, p4lbl As String


            p1lbl = Rd.Item("prm1label")
            p2lbl = Rd.Item("prm2label")
            p3lbl = Rd.Item("prm3label")
            p4lbl = Rd.Item("prm4label")


            'type
            Dim p1type, p2type, p3type, p4type As String

            p1type = Rd.Item("prm1Type")
            p2type = Rd.Item("prm2Type")
            p3type = Rd.Item("prm3Type")
            p4type = Rd.Item("prm4Type")

            'query
            Dim p1query, p2query, p3query, p4query As String

            p1query = Rd.Item("prm1query")
            p2query = Rd.Item("prm2query")
            p3query = Rd.Item("prm3query")
            p4query = Rd.Item("prm4query")


            'prm1
            If p1type = "T" Then
                txtP1.Visible = True
            Else
                txtP1.Visible = False
            End If

            If p1type = "C" Then
                ddlP1.Visible = True
                Dim cmd As New SqlCommand("" & p1query & " where businessid='" & Session("sBusinessID") & "' ")
                ddlP1.DataSource = ExecuteQueryFO(cmd, "SELECT")
                ddlP1.DataTextField = "data"
                ddlP1.DataValueField = "id"
                ddlP1.Items.Insert(0, "select")
                ddlP1.DataBind()
            Else
                ddlP1.Visible = False
            End If

            If p1type = "D" Then
                txtDateP1.Visible = True
                imgP1.Visible = True
                txtDateP1.Text = Date.Today.ToString("dd/MM/yyyy")
            Else
                txtDateP1.Visible = False
                imgP1.Visible = False
            End If

            'prm2
            If p2type = "T" Then
                txtP2.Visible = True
            Else
                txtP2.Visible = False
            End If

            If p2type = "C" Then
                ddlP2.Visible = True
                Dim cmd As New SqlCommand("" & p2query & " ")
                ddlP2.DataSource = ExecuteQueryFO(cmd, "SELECT")
                ddlP2.DataTextField = "data"
                ddlP2.DataValueField = "id"
                ddlP2.Items.Insert(0, "")
                ddlP2.DataBind()
            Else
                ddlP2.Visible = False
            End If

            If p2type = "D" Then
                txtDateP2.Visible = True
                imgP2.Visible = True
                txtDateP2.Text = Date.Today.ToString("dd/MM/yyyy")
            Else
                txtDateP2.Visible = False
                imgP2.Visible = False
            End If

            'prm3
            If p3type = "T" Then
                txtP3.Visible = True
            Else
                txtP3.Visible = False
            End If

            If p3type = "C" Then
                ddlP3.Visible = True
                Dim cmd As New SqlCommand("" & p3query & "  ")
                ddlP3.DataSource = ExecuteQueryFO(cmd, "SELECT")
                ddlP3.DataTextField = "data"
                ddlP3.DataValueField = "id"
                ddlP3.Items.Insert(0, "")
                ddlP3.DataBind()
            Else
                ddlP3.Visible = False
            End If

            If p3type = "D" Then
                txtDateP3.Visible = True
                imgP3.Visible = True
                txtDateP3.Text = Date.Today.ToString("dd/MM/yyyy")
            Else
                txtDateP3.Visible = False
                imgP3.Visible = False
            End If

            'prm4
            If p4type = "T" Then
                txtP4.Visible = True

            Else
                txtP4.Visible = False
            End If

            If p4type = "C" Then
                ddlP4.Visible = True
                Dim cmd As New SqlCommand("" & p4query & " ")
                ddlP4.DataSource = ExecuteQueryFO(cmd, "SELECT")
                ddlP4.DataTextField = "data"
                ddlP4.DataValueField = "id"
                ddlP4.Items.Insert(0, "")
                ddlP4.DataBind()
            Else
                ddlP4.Visible = False
            End If

            If p4type = "D" Then
                txtDateP4.Visible = True
                imgP4.Visible = True
                txtDateP4.Text = Date.Today.ToString("dd/MM/yyyy")
            Else
                txtDateP4.Visible = False
                imgP4.Visible = False
            End If



            lblP1.Visible = True
            lblP2.Visible = True
            lblP3.Visible = True
            lblP4.Visible = True

            lblP1.Text = p1lbl
            lblP2.Text = p2lbl
            lblP3.Text = p3lbl
            lblP4.Text = p4lbl



            Session("filename") = Rd.Item("reportName")

            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data is already exist!');", True)

        Else
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data is not found!');", True)
            'txtDescription.Text = ""
            'txtCurrency.Text = ""

        End If

        cn.Close()
    End Sub



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If

        If Not IsPostBack Then

            'If ciPx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "RP") <> True Then

            '    Session("sMessage") = "Sorry, you dont have access in this module |"
            '    Session("sMemberid") = ""
            '    Session("sWarningID") = "0"
            '    Session("sUrlOKONLY") = "home.aspx"
            '    Session("sUrlYES") = "http://www.thepyxis.net"
            '    Session("sUrlNO") = "http://www.thepyxis.net"
            '    Response.Redirect("warningmsg.aspx")
            'End If

            Session("sProgram") = "02"
            Session("rptGrp") = "01"
            Session("reportID") = "01"
            Session("sUserOperator") = Session("sUserCode")
            Call rptGrp()
            Call grid()
            Call showData()
            Call visiblePrm()
            btnPreview.Enabled = False



        End If
        ScriptManager.RegisterStartupScript(Page, [GetType](), "closeAlret", "<script>closeAlret()</script>", False)
    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        clearSession()
        'Call selectrow()
        'pnlGrid.Visible = False
        'pnlHotelProperty.Visible = True
        Call clearTextbox()
        Call showData()


    End Sub






    Sub clearTextbox()
        txtP1.Text = ""
        txtP2.Text = ""
        txtP3.Text = ""
        txtP4.Text = ""



        txtDateP1.Text = ""
        txtDateP2.Text = ""
        txtDateP3.Text = ""
        txtDateP4.Text = ""

        lblP1.Text = ""
        lblP2.Text = ""
        lblP3.Text = ""
        lblP4.Text = ""
    End Sub
    Sub visiblePrm()
        txtP1.Visible = False
        txtP2.Visible = False
        txtP3.Visible = False
        txtP4.Visible = False

        ddlP1.Visible = False
        ddlP2.Visible = False
        ddlP3.Visible = False
        ddlP4.Visible = False

        txtDateP1.Visible = False
        txtDateP2.Visible = False
        txtDateP3.Visible = False
        txtDateP4.Visible = False

        lblP1.Visible = False
        lblP2.Visible = False
        lblP3.Visible = False
        lblP4.Visible = False

        imgP1.Visible = False
        imgP2.Visible = False
        imgP3.Visible = False
        imgP4.Visible = False
    End Sub

    Sub preview()
        cn.Open()

        Cmd = New SqlCommand("Select *From iPx_cfg_ReportList where  reportID='" & GridView1.SelectedRow.Cells(2).Text & "'", cn)
        Rd = Cmd.ExecuteReader
        If Rd.Read Then
            Dim fileName As String

            fileName = Rd.Item("reportName")


        End If
        cn.Close()
    End Sub
    Protected Sub ddlGrpID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrpID.SelectedIndexChanged
        Call visiblePrm()

        Call grid()
        btnPreview.Enabled = False

    End Sub
    Sub passingPrm()
        Dim cIpx As New iPxClass

        If txtP1.Visible = True Then
            Session("P1") = txtP1.Text
        End If
        If txtP2.Visible = True Then
            Session("P2") = txtP2.Text
        End If
        If txtP3.Visible = True Then
            Session("P3") = txtP3.Text
        End If
        If txtP4.Visible = True Then
            Session("P4") = txtP4.Text
        End If

        If ddlP1.Visible = True Then
            Session("P1") = ddlP1.Text
        End If
        If ddlP2.Visible = True Then
            Session("P2") = ddlP2.Text
        End If
        If ddlP3.Visible = True Then
            Session("P3") = ddlP3.Text
        End If
        If ddlP4.Visible = True Then
            Session("P4") = ddlP4.Text
        End If

        If txtDateP1.Visible = True Then
            Session("P1") = cIpx.Date2IsoDate(txtDateP1.Text)
        End If
        If txtDateP2.Visible = True Then
            Session("P2") = cIpx.Date2IsoDate(txtDateP2.Text)
        End If
        If txtDateP3.Visible = True Then
            Session("P3") = cIpx.Date2IsoDate(txtDateP3.Text)
        End If
        If txtDateP4.Visible = True Then
            Session("P4") = cIpx.Date2IsoDate(txtDateP4.Text)
        End If




        'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Pop", "openModal();", True)

        Response.Redirect("iPxReportView.aspx")
        'popReport.Show()

    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        If txtP1.Visible = True And txtP1.Text.Length = 0 Or txtP2.Visible = True And txtP2.Text.Length = 0 Or txtP3.Visible = True And txtP3.Text.Length = 0 Or txtP4.Visible = True And txtP4.Text.Length = 0 Or txtDateP1.Visible = True And txtDateP1.Text.Length = 0 Or txtDateP2.Visible = True And txtDateP2.Text.Length = 0 Or txtDateP3.Visible = True And txtDateP3.Text.Length = 0 Or txtDateP4.Visible = True And txtDateP4.Text.Length = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data cannot null!');", True)
        Else
            Call passingPrm()
        End If

    End Sub
    Sub clearSession()

        Session("P1") = ""
        Session("P2") = ""
        Session("P3") = ""
        Session("P4") = ""
    End Sub






    'Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
    '    Response.Redirect(Session("sUrlback"))
    'End Sub

    
End Class