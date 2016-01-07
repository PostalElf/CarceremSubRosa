Public Class disposition
    Friend Shared Function buildRandomDisposition() As disposition
        Dim disposition As New disposition
        With disposition
            Dim approaches As New List(Of Integer) From {11, 12, 13}
            Dim approachLike As choiceComponent = rng.Next(1, 4) + 10
            approaches.Remove(approachLike)
            Dim approachDislike As choiceComponent = approaches(rng.Next(2))

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
    Friend Function briefReport() As String
        Dim total As String = ""

        If getApproach(True) = choiceComponent.Supernatural Then
            If getApproach(False) = choiceComponent.Digital Then total &= "Wizardly"
            If getApproach(False) = choiceComponent.Practical Then total &= "Sorcerous"
        ElseIf getApproach(True) = choiceComponent.Practical Then
            If getApproach(False) = choiceComponent.Digital Then total &= "Old-Fashioned"
            If getApproach(False) = choiceComponent.Supernatural Then total &= "Superstitious"
        ElseIf getApproach(True) = choiceComponent.Digital Then
            If getApproach(False) = choiceComponent.Supernatural Then total &= "Professional"
            If getApproach(False) = choiceComponent.Practical Then total &= "Nerdy"
        End If

        total &= " "

        If getAction(True) = choiceComponent.Violence Then
            If getAction(False) = choiceComponent.Diplomacy Then total &= "Thug"
            If getAction(False) = choiceComponent.Guile Then total &= "Bruiser"
        ElseIf getAction(True) = choiceComponent.Diplomacy Then
            If getAction(False) = choiceComponent.Violence Then total &= "Faceman"
            If getAction(False) = choiceComponent.Guile Then total &= "Diplomat"
        ElseIf getAction(True) = choiceComponent.Guile Then
            If getAction(False) = choiceComponent.Diplomacy Then total &= "Spook"
            If getAction(False) = choiceComponent.Violence Then total &= "Rogue"
        End If

        Return total
    End Function
    Public Overrides Function ToString() As String
        Return briefReport()
        'Return fear.ToString & "/" & respect.ToString & " " & loathe.ToString & "/" & admire.ToString
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
    Friend Function getAction(isLike As Boolean) As choiceComponent
        If isLike = True Then
            If respect < 10 Then Return respect Else Return admire
        Else
            If fear < 10 Then Return fear Else Return loathe
        End If
    End Function
    Friend Function getApproach(isLike As Boolean) As choiceComponent
        If isLike = True Then
            If respect > 10 Then Return respect Else Return admire
        Else
            If fear > 10 Then Return fear Else Return loathe
        End If
    End Function
End Class
