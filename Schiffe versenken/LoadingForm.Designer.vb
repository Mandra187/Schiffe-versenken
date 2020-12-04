<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadingForm
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
        Me.components = New System.ComponentModel.Container()
        Me.lblLoad = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblP = New System.Windows.Forms.Label()
        Me.btCopy = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblLoad
        '
        Me.lblLoad.AutoSize = True
        Me.lblLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoad.Location = New System.Drawing.Point(56, 56)
        Me.lblLoad.Name = "lblLoad"
        Me.lblLoad.Size = New System.Drawing.Size(503, 55)
        Me.lblLoad.TabIndex = 0
        Me.lblLoad.Text = "Waiting for Player 2..."
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 250
        '
        'lblP
        '
        Me.lblP.AutoSize = True
        Me.lblP.Location = New System.Drawing.Point(147, 24)
        Me.lblP.Name = "lblP"
        Me.lblP.Size = New System.Drawing.Size(39, 13)
        Me.lblP.TabIndex = 1
        Me.lblP.Text = "Label1"
        '
        'btCopy
        '
        Me.btCopy.Location = New System.Drawing.Point(66, 19)
        Me.btCopy.Name = "btCopy"
        Me.btCopy.Size = New System.Drawing.Size(75, 23)
        Me.btCopy.TabIndex = 2
        Me.btCopy.Text = "copy"
        Me.btCopy.UseVisualStyleBackColor = True
        '
        'LoadingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(599, 146)
        Me.Controls.Add(Me.btCopy)
        Me.Controls.Add(Me.lblP)
        Me.Controls.Add(Me.lblLoad)
        Me.Name = "LoadingForm"
        Me.Text = "Loading"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblLoad As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lblP As Label
    Friend WithEvents btCopy As Button
End Class
