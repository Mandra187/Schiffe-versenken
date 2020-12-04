Public Class LoadingForm

    Private Shared loadingFrom As LoadingForm

    Public Shared Function getForm() As LoadingForm
        If loadingFrom Is Nothing Then
            loadingFrom = New LoadingForm()
        End If
        Return loadingFrom
    End Function

    Private Sub New()
        InitializeComponent()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If lblLoad.Text = "Waiting for Player 2..." Then
            lblLoad.Text = "Waiting for Player 2"
            Exit Sub
        ElseIf lblLoad.Text = "Waiting for Player 2" Then
            lblLoad.Text = "Waiting for Player 2."
            Exit Sub
        ElseIf lblLoad.Text = "Waiting for Player 2." Then
            lblLoad.Text = "Waiting for Player 2.."
        ElseIf lblLoad.Text = "Waiting for Player 2.." Then
            lblLoad.Text = "Waiting for Player 2..."
            Exit Sub
        End If
    End Sub

    Private Sub btCopy_Click(sender As Object, e As EventArgs) Handles btCopy.Click
        Clipboard.SetText(lblP.Text)
    End Sub

    Public Sub HideForm()
        Hide()
        Timer1.Stop()
    End Sub
End Class