Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Partial Class iPxCrmUser_releaseversion
    Inherits System.Web.UI.Page
    Dim sSQL As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            'Me.BindRepeater()
            sSQL = "select distinct iPx_general_ver.version as verid, iPx_general_ver.id , iPx_general_verDTL.verid as VID from iPx_general_verDTL "
            sSQL = sSQL & "inner join iPx_general_ver on iPx_general_verDTL.verid=iPx_general_ver.id "
            'sSQL = sSQL & "inner join iPx_general_verMDL on iPx_general_verDTL.moduleid=iPx_general_verMDL.id "
            sSQL = sSQL & " where iPx_general_verDTL.active='Y' and iPx_general_ver.appid='5' order by verid desc"

            gvReleaseVersion.DataSource = GetData(sSQL)
            gvReleaseVersion.DataBind()
        End If
    End Sub
    Private Shared Function GetData(ByVal query As String) As DataTable
        Dim strConnString As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand()
                cmd.CommandText = query
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Using
    End Function
    'Private Sub BindRepeater()
    '    Dim constr As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
    '    Using con As New SqlConnection(constr)
    '        Using cmd As New SqlCommand()
    '            'sSQL = "select iPx_general_ver.version as verid, iPx_general_ver.description as Descversi, iPx_general_verMDL.description as moduleid ,iPx_general_verDTL.note ,iPx_general_verDTL.verid as VID from iPx_general_verDTL "
    '            'sSQL = sSQL & "inner join iPx_general_ver on iPx_general_verDTL.verid=iPx_general_ver.id "
    '            'sSQL = sSQL & "inner join iPx_general_verMDL on iPx_general_verDTL.moduleid=iPx_general_verMDL.id "
    '            'sSQL = sSQL & "  order by verid desc"
    '            cmd.CommandText = "select iPx_general_ver .version as verid, iPx_general_verMDL.description as moduleid, iPx_general_verDTL .note from iPx_general_verDTL inner join iPx_general_ver on iPx_general_verDTL .verid = iPx_general_ver .id inner join iPx_general_verMDL on iPx_general_verDTL.moduleid=iPx_general_verMDL.id  order by verid desc"
    '            cmd.Connection = con
    '            con.Open()
    '            rptAccordian.DataSource = cmd.ExecuteReader()
    '            rptAccordian.DataBind()
    '            con.Close()
    '        End Using
    '    End Using
    'End Sub
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim versiid As String = gvReleaseVersion.DataKeys(e.Row.RowIndex).Values("VID").ToString()
            Dim gvRelease As GridView = TryCast(e.Row.FindControl("gvRelease"), GridView)
            sSQL = "select iPx_general_ver .version , iPx_general_verMDL .description , iPx_general_verDTL .note from iPx_general_verDTL "
            sSQL += " inner join iPx_general_ver on iPx_general_ver .id = iPx_general_verDTL .verid "
            sSQL += " inner join iPx_general_verMDL on iPx_general_verDTL .moduleid = iPx_general_verMDL .id"
            sSQL += " where verid='" & versiid & "' and iPx_general_verDTL.active='Y'"
            sSQL += " order by iPx_general_ver .version  desc, iPx_general_verMDL .description asc"
            gvRelease.DataSource = GetData(String.Format(sSQL))
            gvRelease.DataBind()
        End If
    End Sub
End Class
