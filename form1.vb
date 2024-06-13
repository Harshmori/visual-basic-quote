Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Text.Json

Public Class Form1
    Private WithEvents Button1 As Button

    Public Sub New()
        InitializeComponent()
        Me.Hide()

        FetchAndDisplayQuoteAsync()
    End Sub

    Private Async Sub FetchAndDisplayQuoteAsync()
        Try
            Dim quote As String = Await GetQuoteAsync()
            ShowQuoteMessageBox(quote)
        Catch ex As Exception
            MsgBox("Failed to fetch quote: " & ex.Message)
        End Try
    End Sub

    Private Async Function GetQuoteAsync() As Task(Of String)
        Dim client As New HttpClient()
        Dim response As HttpResponseMessage = Await client.GetAsync("https://api.quotable.io/random")
        response.EnsureSuccessStatusCode()
        Dim responseBody As String = Await response.Content.ReadAsStringAsync()
        Dim quoteObject As Quote = System.Text.Json.JsonSerializer.Deserialize(Of Quote)(responseBody)
        Return $"{quoteObject.content} - {quoteObject.author}"
    End Function

    Private Class Quote
        Public Property content As String
        Public Property author As String
    End Class

    Private Sub ShowQuoteMessageBox(ByVal quote As String)
        Dim quoteMessageBox As New QuoteMessageBox(quote)
        quoteMessageBox.ShowDialog()
    End Sub

    Private Class QuoteMessageBox
        Inherits Form

        Private WithEvents Button1 As New Button()
        Private ReadOnly Label1 As New Label()

        Public Sub New(ByVal quote As String)
            Me.Text = "Quote"
            Me.AutoSize = True
            Me.AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
            Me.StartPosition = FormStartPosition.CenterScreen

            Label1.Text = quote
            Label1.AutoSize = True
            Label1.Location = New Point(10, 10)

            Button1.Text = "New Quote"
            Button1.Location = New Point(10, Label1.Bottom + 10)
            Button1.AutoSize = True

            Me.Controls.Add(Label1)
            Me.Controls.Add(Button1)

            AddHandler Button1.Click, Sub(sender, e) FetchAndDisplayQuoteAsync()
        End Sub

        Private Async Sub FetchAndDisplayQuoteAsync()
            Try
                Dim quote As String = Await GetQuoteAsync()
                Label1.Text = quote
            Catch ex As Exception
                MessageBox.Show("Failed to fetch quote: " & ex.Message)
            End Try
        End Sub

        Private Async Function GetQuoteAsync() As Task(Of String)
            Dim client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync("https://api.quotable.io/random")
            response.EnsureSuccessStatusCode()
            Dim responseBody As String = Await response.Content.ReadAsStringAsync()
            Dim quoteObject As Quote = System.Text.Json.JsonSerializer.Deserialize(Of Quote)(responseBody)
            Return $"{quoteObject.content} - {quoteObject.author}"
        End Function

        Private Class Quote
            Public Property content As String
            Public Property author As String
        End Class
    End Class
End Class
