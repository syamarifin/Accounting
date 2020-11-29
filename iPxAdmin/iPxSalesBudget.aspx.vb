Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxSalesBudget
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forMonth, forYear, grpBudget As String
    Dim a As Integer
    Dim dateStart As Date
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='31'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as SaveBudget "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("SaveBudget").ToString = "Y" Then
                btnSavegrid.Enabled = True
            Else
                btnSavegrid.Enabled = False
            End If
        Else
            btnSavegrid.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showdata_dropdownGrpBudget()
        dlBudgetGrp.Items.Insert(0, "Revenue Budget")
        dlBudgetGrp.Items.Insert(1, "Segment Market Budget")
        dlBudgetGrp.Items.Insert(2, "Source of Booking Budget")
        dlBudgetGrp.Items.Insert(3, "Sales Person Budget")
    End Sub
    Sub cekBudget()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        If dlBudgetGrp.SelectedIndex = 0 Then
            grpBudget = "01"
        ElseIf dlBudgetGrp.SelectedIndex = 1 Then
            grpBudget = "02"
        ElseIf dlBudgetGrp.SelectedIndex = 2 Then
            grpBudget = "03"
        ElseIf dlBudgetGrp.SelectedIndex = 3 Then
            grpBudget = "04"
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT Code FROM iPxAcctSales_Budget WHERE Grp_Bud='" & grpBudget & "' and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            ListBudget()
        Else
            oSQLReader.Close()
            saveGrpBudget(Session("sBusinessID"), dlFOLink.SelectedValue, dlBudgetGrp.SelectedIndex)
            ListBudget()
        End If
    End Sub
    Public Function saveGrpBudget(ByVal businessid As String, ByVal businessidfo As String, ByVal budgetgroup As String) As Boolean
        
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If budgetgroup = 0 Then
            sSQL = "SELECT poscode FROM iPxPMS_cfg_pos where businessid ='" & businessidfo & "' and revenuegroup<>'SY' "
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader
            While oSQLReader.Read
                'oSQLReader.Close()
                saveBudget(businessid, businessidfo, oSQLReader.Item("poscode"), budgetgroup)
            End While
            oCnct.Close()
        ElseIf budgetgroup = 1 Then
            sSQL = "SELECT id FROM iPxPMS_cfg_mktsegment where businessid ='" & businessidfo & "' "
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader
            While oSQLReader.Read
                'oSQLReader.Close()
                saveBudget(businessid, businessidfo, oSQLReader.Item("id"), budgetgroup)
            End While
            oCnct.Close()
        ElseIf budgetgroup = 2 Then
            sSQL = "SELECT id FROM iPxPMS_cfg_sob where businessid ='" & businessidfo & "' "
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader
            While oSQLReader.Read
                'oSQLReader.Close()
                saveBudget(businessid, businessidfo, oSQLReader.Item("id"), budgetgroup)
            End While
            oCnct.Close()
        ElseIf budgetgroup = 3 Then
            sSQL = "SELECT id FROM iPxPMS_cfg_sales where businessid ='" & businessidfo & "' "
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader
            While oSQLReader.Read
                'oSQLReader.Close()
                saveBudget(businessid, businessidfo, oSQLReader.Item("id"), budgetgroup)
            End While
            oCnct.Close()
        End If
    End Function
    Public Function saveBudget(ByVal businessid As String, ByVal businessidfo As String, ByVal poscode As String, ByVal budgetgroup As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        'Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        If budgetgroup = 0 Then
            grpBudget = "01"
        ElseIf budgetgroup = 1 Then
            grpBudget = "02"
        ElseIf budgetgroup = 2 Then
            grpBudget = "03"
        ElseIf budgetgroup = 3 Then
            grpBudget = "04"
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcctSales_Budget(businessid,date,Grp_Bud,Code,PctOcc_Bud,Revenue_Bud,pmsID) "
        sSQL = sSQL & "VALUES ('" & businessid & "','" & forMonth & "-" & forYear & "','" & grpBudget & "'"
        sSQL = sSQL & ",'" & poscode & "','0','0'"
        sSQL = sSQL & ",'" & businessidfo & "') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Sub ListBudget()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If dlBudgetGrp.SelectedIndex = 0 Then
            grpBudget = "01"
            sSQL = "SELECT 1 as numb, a.*, (b.revenuegroup) as RevGrp, c.businessname, b.description FROM iPxAcctSales_Budget AS a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b on b.poscode=a.Code and a.pmsID=b.businessid "
            sSQL += "INNER JOIN iPx_profile_client as c on c.businessid = a.pmsID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and Grp_Bud='" & grpBudget & "' "
            sSQL += "and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "' and b.revenuegroup='RO' "
            sSQL += "UNION "
            sSQL += "SELECT 2 as numb, a.*, (b.revenuegroup) as RevGrp, c.businessname, b.description FROM iPxAcctSales_Budget AS a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b on b.poscode=a.Code and a.pmsID=b.businessid "
            sSQL += "INNER JOIN iPx_profile_client as c on c.businessid = a.pmsID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and Grp_Bud='" & grpBudget & "' "
            sSQL += "and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "' and b.revenuegroup='FB' "
            sSQL += "UNION "
            sSQL += "SELECT 3 as numb, a.*, (b.revenuegroup) as RevGrp, c.businessname, b.description FROM iPxAcctSales_Budget AS a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b on b.poscode=a.Code and a.pmsID=b.businessid "
            sSQL += "INNER JOIN iPx_profile_client as c on c.businessid = a.pmsID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and Grp_Bud='" & grpBudget & "' "
            sSQL += "and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "' and b.revenuegroup<>'RO' and b.revenuegroup<>'FB' and b.revenuegroup<>'SY' "
            sSQL += " order by numb, a.Code asc"
        ElseIf dlBudgetGrp.SelectedIndex = 1 Then
            grpBudget = "02"
            sSQL = "SELECT a.*, c.businessname, b.description FROM iPxAcctSales_Budget AS a "
            sSQL += "INNER JOIN iPxPMS_cfg_mktsegment as b on b.id=a.Code and a.pmsID=b.businessid "
            sSQL += "INNER JOIN iPx_profile_client as c on c.businessid = a.pmsID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and Grp_Bud='" & grpBudget & "' and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "' "
            If Session("sQueryTicket") = "" Then
                Session("sQueryTicket") = Session("sCondition")
                If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                    sSQL = sSQL & Session("sQueryTicket")
                    Session("sCondition") = ""
                Else
                    sSQL = sSQL & " "
                End If
            Else
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            End If
            sSQL += " order by a.date asc"
        ElseIf dlBudgetGrp.SelectedIndex = 2 Then
            grpBudget = "03"
            sSQL = "SELECT a.*, c.businessname, b.description FROM iPxAcctSales_Budget AS a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b on b.poscode=a.Code and a.pmsID=b.businessid "
            sSQL += "INNER JOIN iPx_profile_client as c on c.businessid = a.pmsID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and Grp_Bud='" & grpBudget & "' and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "' "
            If Session("sQueryTicket") = "" Then
                Session("sQueryTicket") = Session("sCondition")
                If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                    sSQL = sSQL & Session("sQueryTicket")
                    Session("sCondition") = ""
                Else
                    sSQL = sSQL & " "
                End If
            Else
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            End If
            sSQL += " order by a.date asc"
        ElseIf dlBudgetGrp.SelectedIndex = 3 Then
            grpBudget = "04"
            sSQL = "SELECT a.*, c.businessname, b.description FROM iPxAcctSales_Budget AS a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b on b.poscode=a.Code and a.pmsID=b.businessid "
            sSQL += "INNER JOIN iPx_profile_client as c on c.businessid = a.pmsID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and Grp_Bud='" & grpBudget & "' and pmsID = '" & dlFOLink.SelectedValue & "' and date ='" & forMonth & "-" & forYear & "' "
            If Session("sQueryTicket") = "" Then
                Session("sQueryTicket") = Session("sCondition")
                If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                    sSQL = sSQL & Session("sQueryTicket")
                    Session("sCondition") = ""
                Else
                    sSQL = sSQL & " "
                End If
            Else
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            End If
            sSQL += " order by a.date asc"
        End If
        
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvBudget.DataSource = dt
                    gvBudget.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvBudget.DataSource = dt
                    gvBudget.DataBind()
                    gvBudget.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    
    Sub FOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLink.DataSource = dt
                dlFOLink.DataTextField = "businessname"
                dlFOLink.DataValueField = "businessid"
                dlFOLink.DataBind()
            End Using
        End Using
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        Else
            If Not Me.IsPostBack Then
                If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Sales Configuration") <> True Then

                    Session("sMessage") = "Sorry, you dont have access in this module |"
                    Session("sMemberid") = ""
                    Session("sWarningID") = "0"
                    Session("sUrlOKONLY") = "home.aspx"
                    Session("sUrlYES") = "http://www.thepyxis.net"
                    Session("sUrlNO") = "http://www.thepyxis.net"
                    Response.Redirect("warningmsg.aspx")
                End If
                Session("sQueryTicket") = ""
                showdata_dropdownGrpBudget()
                dlBudgetGrp.Enabled = False
                FOLink()
                tbDate.Text = Format(Now, "MMMM yyyy")
                Dim dateBirthday As Date = tbDate.Text
                forMonth = dateBirthday.ToString("MM")
                forYear = dateBirthday.ToString("yyy")
                cekBudget()
            End If
            tbDate.Enabled = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
            UserAcces()
        End If
    End Sub
    Private tmpCategoryName As String = ""
    Private tmpHeaderName As String = ""
    Dim group As Integer = 0
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If drv("RevGrp").ToString() = "TAX" Or drv("RevGrp").ToString() = "SR" Then
            Else
                If tmpCategoryName <> drv("numb").ToString() Then
                    tmpCategoryName = drv("numb").ToString()
                    If drv("RevGrp").ToString() = "RO" Then
                        tmpHeaderName = "ROOM REVENUE"
                    ElseIf drv("RevGrp").ToString() = "FB" Then
                        tmpHeaderName = "FOOD AND BEVERAGE"
                    ElseIf drv("RevGrp").ToString() = "TAX" Then
                        tmpHeaderName = "TAX"
                    ElseIf drv("RevGrp").ToString() = "SET" Then
                        tmpHeaderName = "SETTLEMENT"
                    ElseIf drv("RevGrp").ToString() <> "RO" Or drv("RevGrp").ToString() <> "FB" Or drv("RevGrp").ToString() = "SY" Then
                        tmpHeaderName = "OTHER"
                    End If
                    Dim tbl As Table = TryCast(e.Row.Parent, Table)

                    If tbl IsNot Nothing Then
                        Dim row As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                        Dim cell As TableCell = New TableCell()
                        cell.ColumnSpan = Me.gvBudget.Columns.Count
                        cell.Width = Unit.Percentage(100)
                        cell.Style.Add("Font-weight", "bold")
                        cell.Style.Add("background-color", "#fffff")
                        cell.Style.Add("color", "black")
                        cell.Style.Add("text-transform", "uppercase")
                        Dim span As HtmlGenericControl = New HtmlGenericControl("span")

                        span.InnerHtml = tmpHeaderName
                        cell.Controls.Add(span)
                        row.Cells.Add(cell)
                        tbl.Rows.AddAt(tbl.Rows.Count - 1, row)
                        Group += 1
                    End If
                End If
            End If
        End If
    End Sub
    Public Function saveGrid(ByVal businessid As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvBudget.Rows
            Dim hdDate As HiddenField = row.FindControl("hdDate")
            Dim hdCode As HiddenField = row.FindControl("hdCode")
            Dim hdGroup As HiddenField = row.FindControl("hdGroup")
            Dim tbRevBud As TextBox = row.FindControl("txtRevBud")
            Dim tbPct As TextBox = row.FindControl("txtPct")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctSales_Budget  SET   PctOcc_Bud ='" & tbPct.Text & "', Revenue_Bud ='" & tbRevBud.Text & "' where businessid ='" & businessid & "' and date ='" & hdDate.Value.Trim & "' and Grp_Bud ='" & hdGroup.Value.Trim & "' and Code ='" & hdCode.Value.Trim & "' "
            If dlFOLink.SelectedValue <> "ALL HOTEL" Then
                sSQL += "and pmsID ='" & dlFOLink.SelectedValue & "' "
            End If
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

        Next
        oCnct.Close()
    End Function

    Protected Sub gvBudget_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBudget.PageIndexChanging
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        gvBudget.PageIndex = e.NewPageIndex
        ListBudget()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvBudget.PageIndex = e.NewPageIndex
        Me.ListBudget()
    End Sub

    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        cekBudget()
    End Sub

    Protected Sub btnSavegrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavegrid.Click
        saveGrid(Session("sBusinessID"))
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        ListBudget()
    End Sub

    Protected Sub dlBudgetGrp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlBudgetGrp.SelectedIndexChanged
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        cekBudget()
    End Sub

    Protected Sub dlFOLink_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlFOLink.SelectedIndexChanged
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        cekBudget()
    End Sub
End Class
