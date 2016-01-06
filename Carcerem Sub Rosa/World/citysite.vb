Public Class citysite
    Friend Shared Function buildCitysite(aPlayer As player, aCity As city) As citysite
        Dim citysite As New citysite
        With citysite
            .player = aPlayer
            aCity.addCitysite(citysite)
        End With
        Return citysite
    End Function

    Friend Property city As city
    Friend Property player As player
    Friend Property holding As holding
End Class
