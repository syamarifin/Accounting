Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports CrystalDecisions.CrystalReports.Engine

Imports CrystalDecisions.Shared
Partial Class iPxAdmin_paymentmodeAdmin
    Inherits System.Web.UI.Page

    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Public reportdocument As New ReportDocument()
    Dim strCn As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim cn As SqlConnection = New SqlConnection(strCn)

   

    Protected Sub btnVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoucher.Click

        sSQL = "select status from iPx_general_voucher where voucherno='" & txtVoucher.Text.Trim & "' and status='A' "
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then
            Dim duration, package As String
            package = txtVoucher.Text.Substring(0, 2)
            duration = txtVoucher.Text.Substring(4, 2)

            Select Case duration
                Case "M1"
                    Session("cPyMode") = "M"
                Case "Y1"
                    Session("cPyMode") = "Y"
            End Select

            Select Case package
                Case "P1"
                    Session("sProgram") = "01"
                    Session("sRegisteredPackage") = "1"
                Case "P2"
                    Session("sProgram") = "02"
                    Session("sRegisteredPackage") = "2"
                Case "P3"
                    Session("sProgram") = "03"
                    Session("sRegisteredPackage") = "3"
            End Select

            Session("sVoucher") = txtVoucher.Text
            Response.Redirect("ordercfrm.aspx")
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Voucher # is not Valid or Registered !');document.getElementById('Buttonx').click()", True)
        End If
        oSQLReader.Close()
        oCnct.Close()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'btnCard.Text = btnCard.Text & " - " & String.Format("{0:#.00}", Convert.ToDecimal(Session("sPrice")))
            lblPayfor.Text = Session("sPayfor")
            lblAmount.Text = Session("nAmt")
            lblProgramName.Text = Session("sProgramName")
            lblPackage.Text = Session("sPackage")
            lblQty.Text = Session("sQty")
            ddsite.Text = Session("sUserName")
        End If
        invoice()
        'report()
    End Sub
    Sub invoice()
        lblItem.Text = Session("sPayfor") & " " & Session("sProgramName")
        lblPriceItem.Text = Session("nAmt")
        lblQtyItem.Text = Session("sQty")
        lblTotalItem.Text = Session("nAmt")
        lblSubtotal.Text = Session("nAmt")
        lblTotalAll.Text = Session("nAmt")
        ddSite.DataBind()
        lblShippedto.Text = ddSite.Text
        lblOrderDate.Text = Date.Today
    End Sub
    Sub report()
        Dim builder As New SqlConnectionStringBuilder(strCn)
        Dim dbName As String = builder.InitialCatalog
        Dim dbDataSource As String = builder.DataSource
        Dim userId As String = builder.UserID
        Dim pass As String = builder.Password
        reportdocument.Load(Server.MapPath("..\iPxReportFile\dck_ar_invoice.rpt"))
        reportdocument.SetDatabaseLogon(userId, pass, dbDataSource, dbName)
        'reportdocument.SetDatabaseLogon(ConfigurationManager.AppSettings("UID"), ConfigurationManager.AppSettings("PWD"), ConfigurationManager.AppSettings("SVR"), ConfigurationManager.AppSettings("DB"))

        reportdocument.SetParameterValue("pBusinessID", Session("sBusinessID"))
        reportdocument.SetParameterValue("pOprID", Session("sUserOperator"))
        reportdocument.SetParameterValue("P1", ddSite.Text)
        reportdocument.SetParameterValue("P2", Date.Today)
        reportdocument.SetParameterValue("P3", Session("sPayfor") & " " & Session("sProgramName"))
        reportdocument.SetParameterValue("P4", Session("nAmt"))
        reportdocument.SetParameterValue("P5", Session("sQty"))


        CrystalReportViewer1.ReportSource = reportdocument
        'reportdocument.PrintToPrinter(1, True, 0, 0)
        Dim myConnectionInfo As New ConnectionInfo()

        myConnectionInfo.UserID = userId
        myConnectionInfo.Password = pass

        setDBLOGONforREPORT(myConnectionInfo)
    End Sub
    Private Sub setDBLOGONforREPORT(ByVal myconnectioninfo As ConnectionInfo)
        Dim mytableloginfos As New TableLogOnInfos()
        mytableloginfos = CrystalReportViewer1.LogOnInfo
        For Each myTableLogOnInfo As TableLogOnInfo In mytableloginfos
            myTableLogOnInfo.ConnectionInfo = myconnectioninfo
        Next

    End Sub
End Class
