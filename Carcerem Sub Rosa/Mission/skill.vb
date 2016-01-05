Public Class skill
    Public Sub New(aAction As choiceComponent, aApproach As choiceComponent)
        action = aAction
        approach = aApproach
    End Sub
    Public Overrides Function ToString() As String
        Return _approach.ToString & " " & _action.ToString
    End Function

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

    Friend Shared Function getRandomAction() As choiceComponent
        Return rng.Next(1, 4)
    End Function
    Friend Shared Function getRandomApproach() As choiceComponent
        Return rng.Next(1, 4) + 10
    End Function
End Class
