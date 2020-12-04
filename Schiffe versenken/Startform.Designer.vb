<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Startform
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btStart = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btStart
        '
        Me.btStart.Location = New System.Drawing.Point(106, 142)
        Me.btStart.Name = "btStart"
        Me.btStart.Size = New System.Drawing.Size(523, 322)
        Me.btStart.TabIndex = 0
        Me.btStart.Text = "start game"
        Me.btStart.UseVisualStyleBackColor = True
        '
        'Startform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 643)
        Me.Controls.Add(Me.btStart)
        Me.Name = "Startform"
        Me.Text = "Startform"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btStart As Button
End Class
