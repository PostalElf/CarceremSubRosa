Public Class relationship
    Friend Property fearRespect As Integer
    Friend Property loathingAdmiration As Integer
    Friend ReadOnly Property likeDislike As Integer
        Get
            Return fearRespect + loathingAdmiration
        End Get
    End Property
End Class
