Public Class citysite
    Friend Shared Function buildCitysite(aPlayer As player, aCity As city) As citysite
        Dim citysite As New citysite
        citysite.name = getRandomName(aCity)
        aPlayer.addOpenCitysite(citysite)
        aCity.addCitysite(citysite)
        Return citysite
    End Function
    Friend Sub consoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)
        Console.WriteLine(ind & name & " in " & city.name & ", " & city.parseContinent(city.continent))
    End Sub

    Friend Property name As String
    Private Shared Function getRandomName(aCity As city)
        Return "Lot " & rng.Next(100)
    End Function

    Friend Property city As city
    Friend Property player As player
    Friend Property holding As holding
End Class
