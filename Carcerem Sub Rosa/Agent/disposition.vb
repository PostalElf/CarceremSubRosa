Public Class disposition
    Friend Shared Function buildRandomDisposition() As disposition
        Dim disposition As New disposition
        With disposition
            Dim approachLike As choiceComponent
            If coinFlip() = True Then approachLike = choiceComponent.Supernatural Else approachLike = choiceComponent.Practical
            Dim approachDislike As choiceComponent
            If approachLike = choiceComponent.Supernatural Then approachDislike = choiceComponent.Practical Else approachDislike = choiceComponent.Supernatural

            Dim actions As New List(Of Integer) From {1, 2, 3}
            Dim actionLike As choiceComponent = rng.Next(1, 4)
            actions.Remove(actionLike)
            Dim actionDislike As choiceComponent = actions(rng.Next(2))

            If coinFlip() = True Then
                .respect = approachLike
                .fear = approachDislike
                .admire = actionLike
                .loathe = actionDislike
            Else
                .respect = actionLike
                .fear = actionDislike
                .admire = approachLike
                .loathe = approachDislike
            End If
        End With
        Return disposition
    End Function

    Friend Property respect As choiceComponent
    Friend Property fear As choiceComponent
    Friend Property admire As choiceComponent
    Friend Property loathe As choiceComponent

    Friend Function getReaction(choiceComponent As choiceComponent) As String
        If respect = choiceComponent Then
            Return "Respect"
        ElseIf admire = choiceComponent Then
            Return "Admire"
        ElseIf fear = choiceComponent Then
            Return "Fear"
        ElseIf loathe = choiceComponent Then
            Return "Loathe"
        Else
            Return Nothing
        End If
    End Function
End Class
