Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
'Imports ExecuteQuery
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class iPxAdmin_iPxReportView
    Inherits System.Web.UI.Page
    'execute query
    '  Dim execute As New ExecuteQuery


    'definisi string koneksi dan buka koneksi 
    Dim Cmd As SqlCommand
    Dim Rd As SqlDataReader
    Dim dt As New DataTable
    Dim queryResult As Integer
    'definisi string koneksi dan buka koneksi 
    Dim strCn As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim cn As SqlConnection = New SqlConnection(strCn)



    Private Sub setDBLOGONforREPORT(ByVal myconnectioninfo As ConnectionInfo)
        Dim mytableloginfos As New TableLogOnInfos()
        mytableloginfos = CrystalReportViewer1.LogOnInfo
        For Each myTableLogOnInfo As TableLogOnInfo In mytableloginfos
            myTableLogOnInfo.ConnectionInfo = myconnectioninfo
        Next

    End Sub

    'for print


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then


            'Call hotelID()
            'Call propertyID()

        End If
        Try
            Dim ss As String = Session("filename")

            'file directory of report
            Dim cryRpt As New ReportDocument()
            cryRpt.Load(Server.MapPath("~/iPxReportFile/" & Session("filename")))
            CrystalReportViewer1.ReportSource = cryRpt
            CrystalReportViewer1.RefreshReport()



            Dim builder As New SqlConnectionStringBuilder(strCn)
            Dim dbName As String = builder.InitialCatalog
            Dim dbDataSource As String = builder.DataSource
            Dim userId As String = builder.UserID
            Dim pass As String = builder.Password

            '--- To display values on the page 
            Dim myConnectionInfo As New ConnectionInfo()

            myConnectionInfo.UserID = userId
            myConnectionInfo.Password = pass

            setDBLOGONforREPORT(myConnectionInfo)


            cn.Close()
            'business ID
            Dim ParamFields As ParameterFields = Me.CrystalReportViewer1.ParameterFieldInfo
            If Session("sBusinessID") = "" Then

            Else
                Dim p0 As New ParameterField
                p0.Name = "pBusinessID"

                Dim p0Value As New ParameterDiscreteValue
                p0Value.Value = Session("sBusinessID")
                p0.CurrentValues.Add(p0Value)

                ParamFields.Add(p0)

            End If

            'Operator ID
            If Session("sUserOperator") = "" Then

            Else
                Dim p1 As New ParameterField
                p1.Name = "pOprID"

                Dim p1Value As New ParameterDiscreteValue
                p1Value.Value = Session("sUserOperator")
                p1.CurrentValues.Add(p1Value)

                ParamFields.Add(p1)

            End If

            'parameter 1
            If Session("P1") = "" Then

            Else
                Dim p1 As New ParameterField
                p1.Name = "P1"

                Dim p1Value As New ParameterDiscreteValue
                p1Value.Value = Session("P1")
                p1.CurrentValues.Add(p1Value)

                ParamFields.Add(p1)

            End If

            'parameter 2
            If Session("P2") = "" Then

            Else
                Dim p2 As New ParameterField
                p2.Name = "P2"

                Dim p2Value As New ParameterDiscreteValue
                p2Value.Value = Session("P2")
                p2.CurrentValues.Add(p2Value)

                ParamFields.Add(p2)

            End If

            'parameter 3
            If Session("P3") = "" Then

            Else
                Dim p3 As New ParameterField
                p3.Name = "P3"

                Dim p3Value As New ParameterDiscreteValue
                p3Value.Value = Session("P3")
                p3.CurrentValues.Add(p3Value)

                ParamFields.Add(p3)

            End If

            'parameter 4
            If Session("P4") = "" Then

            Else
                Dim p4 As New ParameterField
                p4.Name = "P4"

                Dim p4Value As New ParameterDiscreteValue
                p4Value.Value = Session("P4")
                p4.CurrentValues.Add(p4Value)

                ParamFields.Add(p4)

            End If
        Catch ex As Exception

        End Try


    End Sub


End Class