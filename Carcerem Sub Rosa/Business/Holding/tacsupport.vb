Public Class tacsupport
    Inherits holding
    Friend Shared Function buildTacsupport(aCitysite As citysite, aShellcompany As shellcompany)
        If aCitysite.city.Equals(aShellcompany.city) = False Then Return Nothing

        Dim tacsupport As New tacsupport
        With tacsupport
            .name = holding.getRandomCodename
            .citysite = aCitysite
            .city.addHolding(tacsupport, .citysite)
            aShellcompany.addHolding(tacsupport)
        End With
        Return tacsupport
    End Function
    Friend Overrides Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix
        Console.WriteLine(ind & """" & name & """ Tactical Support")
        Console.WriteLine(indd & fakeTab("Shell Company: ", 16) & shellcompany.name)
        Console.WriteLine(indd & fakeTab("City: ", 16) & city.name & ", " & city.parseContinent(continent))
        Console.WriteLine(indd & fakeTab("Visibility: ", 16) & withSign(visibility))
        Console.WriteLine(indd & fakeTab("Upkeep: ", 16) & withReverseSign(upkeep, "$"))
    End Sub
    Public Overrides Function ToString() As String
        Return name & " Tacsupport"
    End Function

    Friend Overrides ReadOnly Property upkeep As Integer
        Get
            Return 100
        End Get
    End Property
    Friend Overrides ReadOnly Property visibility As Integer
        Get
            Return 1
        End Get
    End Property
End Class
