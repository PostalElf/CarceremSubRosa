﻿Public Class artefact
    Implements requirement

    Friend Property name As String Implements requirement.name
    Friend Property holding As holding Implements requirement.location
    Friend Property visibility As Integer Implements requirement.visibility

    Private Property _researchBonus As Integer
    Friend ReadOnly Property researchBonus As Integer
        Get
            Return _researchBonus
        End Get
    End Property
End Class
