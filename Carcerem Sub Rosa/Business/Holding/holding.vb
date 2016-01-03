Public MustInherit Class holding
    Friend MustOverride Sub consoleReport(indent As Integer, Optional prefix As String = "")

    Friend Property shellcompany As shellcompany
    Friend Property citysite As citysite
    Friend ReadOnly Property city As city
        Get
            Return shellcompany.city
        End Get
    End Property
    Friend ReadOnly Property continent As continent
        Get
            Return shellcompany.continent
        End Get
    End Property
    Friend MustOverride ReadOnly Property visibility As Integer
    Friend MustOverride ReadOnly Property upkeep As Integer

    Private Shared _codenames As New List(Of String)
    Friend Shared Function getRandomCodename() As String
        If _codenames.Count = 0 Then _codenames = fileget("data/codenames.txt")
        Dim roll As Integer = rng.Next(_codenames.Count)
        getRandomCodename = _codenames(roll)
        _codenames.RemoveAt(roll)
    End Function
End Class
