Public Class citysite
    Public Sub New()
    End Sub
    Public Sub New(aPlayer As player, aCity As city)
        player = aPlayer
        aCity.addCitysite(Me)
    End Sub

    Friend Property city As city
    Friend Property player As player
    Friend Property holding As holding
End Class
