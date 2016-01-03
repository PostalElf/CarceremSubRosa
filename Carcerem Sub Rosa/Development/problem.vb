Public Class problem
    Public Property problemType As problemType
    Public Property problemReporter As problemReporter
    Public Sub New(reporter As problemReporter, aProblemType As problemType)
        problemReporter = reporter
        problemType = aProblemType
    End Sub
End Class

Public Enum problemType
    Duplicate = 1
    ExceedCapacity
    NotFound
    NotSuitable
    Occupied
End Enum

Public Interface problemReporter
    Property name As String
End Interface