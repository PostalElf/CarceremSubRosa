Public Class agent
    Implements problemReporter
    Public Sub New()
    End Sub

    Private Property _player As player

    Friend Property name As String Implements problemReporter.name
    Private Property _health As Integer = 10
    Private Property _sanity As Integer = 10
    Private Property _morale As Integer = 10

End Class
