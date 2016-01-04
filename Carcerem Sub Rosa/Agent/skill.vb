Public Class skill
    Public Sub New(aAction As choiceComponent, aApproach As choiceComponent)
        action = aAction
        approach = aApproach
    End Sub

    Private Property _action As choiceComponent
    Friend Property action As choiceComponent
        Get
            Return _action
        End Get
        Set(value As choiceComponent)
            If value > 0 AndAlso value < 10 Then _action = value
        End Set
    End Property
    Private Property _approach As choiceComponent
    Friend Property approach As choiceComponent
        Get
            Return _approach
        End Get
        Set(value As choiceComponent)
            If value > 10 AndAlso value < 20 Then _approach = value
        End Set
    End Property
End Class
