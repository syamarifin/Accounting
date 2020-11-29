Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxSalesStatistic
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forDate, a, year, yearMonth As String
    Dim cIpx As New iPxClass
    Sub listStatisticSummary()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select 1 as numb,'INV' as groupStatic, 'Total Room' as Descrip, COUNT(*) as total from iPxPMS_cfg_roomno as a "
        sSQL += "INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid WHERE b.businessid='ABCDEF' UNION "
        sSQL += "select 2 as numb,'INV' as groupStatic, 'Out of Order' as Descrip, COUNT(*) as total from iPxPMS_cfg_roomno as a "
        sSQL += "INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid WHERE b.businessid='ABCDEF' and a.fostatus='ooo' UNION "
        sSQL += "select 3 as numb,'INV' as groupStatic, 'House used' as Descrip, COUNT(*) as total from iPxPMS_cfg_roomno as a "
        sSQL += "INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid WHERE b.businessid='ABCDEF' and (a.fostatus='OC' or a.fostatus='OD') UNION "
        sSQL += "select 4 as numb,'INV' as groupStatic, 'Total Available to Sell' as Descrip, COUNT(*) as total from iPxPMS_cfg_roomno as a "
        sSQL += "INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid WHERE b.businessid='ABCDEF' and a.fostatus<>'OOO' and a.fostatus<>'OC' and a.fostatus<>'OD' "

        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                    gvStatistic.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            tbDate.Text = Format(Now, "dd MMMM yyy")
            Dim dateBirthday As Date = tbDate.Text
            forDate = dateBirthday.ToString("yyy-MM-")
            a = dateBirthday.ToString("yyy-MM-dd")
            year = dateBirthday.ToString("yyy-")
            yearMonth = dateBirthday.ToString("yyy-MM-")
            listStatisticSummary()
        Else
            'Dim dateBirthday As Date = tbDate.Text
            'forDate = dateBirthday.ToString("yyy-MM-")
            'a = dateBirthday.ToString("yyy-MM-dd")
            'year = dateBirthday.ToString("yyy-")
            'yearMonth = dateBirthday.ToString("yyy-MM-")
            'listStatisticSummary()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthday As Date = tbDate.Text
        forDate = dateBirthday.ToString("yyy-MM-")
        a = dateBirthday.ToString("yyy-MM-dd")
        year = dateBirthday.ToString("yyy-")
        yearMonth = dateBirthday.ToString("yyy-MM-")
        listStatisticSummary()
    End Sub
    Private tmpCategoryName As String = ""
    Private tmpHeaderName As String = ""
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If tmpCategoryName <> drv("groupStatic").ToString() Then
                tmpCategoryName = drv("groupStatic").ToString()
                If drv("groupStatic").ToString() = "INV" Then
                    tmpHeaderName = "ROOM INVENTORY"
                End If
                Dim tbl As Table = TryCast(e.Row.Parent, Table)

                If tbl IsNot Nothing Then
                    Dim row As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim cell As TableCell = New TableCell()
                    cell.ColumnSpan = Me.gvStatistic.Columns.Count()
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
                End If
            End If
        End If
    End Sub
End Class
