Public Class city
    Implements problemReporter
    Public Sub New()
    End Sub
    Friend Shared Function buildCity(line As String()) As city
        Dim city As New city
        With city
            .name = line(0)
            .country = line(1)
            .continent = enumArrays.getEnumFromString(line(2), enumArrays.continentArray)
            ._coords = getCoordsFromString(line(3))
            ._standardOfLiving = CInt(line(4))
            For Each god In enumArrays.godArray
                ._corruption.Add(god, 0)
            Next
        End With
        Return city
    End Function
    Friend Sub consoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)

        Console.WriteLine(ind & name & ", " & country)
        Console.WriteLine(indd & "Continent: " & parseContinent(continent))
        Console.WriteLine(indd & "Coordinates: " & _coords.ToString)
        Console.WriteLine(indd & "Wealth: " & standardOfLiving.ToString)
    End Sub
    Friend Function briefReport() As String
        Return name & ", " & parseContinent(continent)
    End Function
    Public Overrides Function ToString() As String
        Return name & ", " & parseContinent(continent)
    End Function

    Friend Property world As world
    Friend Property name As String Implements problemReporter.name
    Friend Property country As String
    Friend Shared Function parseContinent(continent As continent) As String
        Select Case continent
            Case Carcerem_Sub_Rosa.continent.AmericaN : Return "North America"
            Case Carcerem_Sub_Rosa.continent.AmericaS : Return "South America"
            Case Carcerem_Sub_Rosa.continent.AsiaSE : Return "South East Asia"
            Case Carcerem_Sub_Rosa.continent.MiddleEast : Return "Middle East"
            Case Else : Return continent.ToString
        End Select
    End Function
    Friend Property continent As continent
    Private Property _coords As New xy
    Private Shared Function getCoordsFromString(str As String) As xy
        Dim xy As New xy
        With xy
            Dim raw As String() = str.Split(",")
            For n = 0 To 1
                raw(n) = raw(n).Replace("°", "")
                raw(n) = raw(n).Trim()

                Dim rawsub As String() = raw(n).Split(" ")
                Dim value As Integer = CInt(CDec(rawsub(0)) * 100)
                Dim direction As String = rawsub(1)

                Select Case direction
                    Case "N" : .y = value
                    Case "S" : .y = value * -1
                    Case "E" : .x = value
                    Case "W" : .x = value * -1
                End Select
            Next
        End With
        Return xy
    End Function
    Friend Function getDistanceTo(destination As city) As Integer
        Return world.getDistance(Me, destination)
    End Function

    Private Property _standardOfLiving As standardOfLiving
    Friend ReadOnly Property standardOfLiving As standardOfLiving
        Get
            Return _standardOfLiving
        End Get
    End Property
    Private Property _citysites As New List(Of citysite)
    Friend Function addCitysite(citysite As citysite) As problem
        If _citysites.Contains(citysite) Then Return New problem(Me, problemType.Duplicate)

        citysite.city = Me
        _citysites.Add(citysite)
        Return Nothing
    End Function
    Friend Function addHolding(holding As holding, citysite As citysite) As problem
        If _citysites.Contains(citysite) = False Then Return New problem(Me, problemType.NotFound)
        If citysite.holding Is Nothing = False Then Return New problem(Me, problemType.Occupied)
        If holding.citysite Is Nothing = False Then Return New problem(Me, problemType.Occupied)
        If TypeOf holding Is laundry Then Return New problem(Me, problemType.NotSuitable)
        If TypeOf holding Is safehouse AndAlso getHoldings("safehouse").Count > 0 Then Return New problem(Me, problemType.ExceedCapacity)
        If TypeOf holding Is tacsupport AndAlso getHoldings("tacsupport").Count > 0 Then Return New problem(Me, problemType.ExceedCapacity)

        citysite.holding = holding
        holding.citysite = citysite
        Return Nothing
    End Function
    Private Function getHoldings(type As String) As List(Of holding)
        Dim total As New List(Of holding)
        For Each citysite In _citysites
            If citysite.holding Is Nothing = False Then
                Dim holding As holding = citysite.holding
                Select Case type
                    Case "factory" : If TypeOf holding Is factory Then total.Add(holding)
                    Case "laundry" : If TypeOf holding Is laundry Then total.Add(holding)
                    Case "researchlab" : If TypeOf holding Is researchlab Then total.Add(holding)
                    Case "safehouse" : If TypeOf holding Is safehouse Then total.Add(holding)
                    Case "tacsupport" : If TypeOf holding Is tacsupport Then total.Add(holding)
                End Select
            End If
        Next
        Return total
    End Function

    Private Property _shellcompanies As New List(Of shellcompany)
    Friend ReadOnly Property shellcompanies As List(Of shellcompany)
        Get
            Return _shellcompanies
        End Get
    End Property
    Friend Function addShellcompany(shellcompany As shellcompany) As problem
        If _shellcompanies.Contains(shellcompany) Then Return New problem(Me, problemType.Duplicate)

        shellcompany.city = Me
        _shellcompanies.Add(shellcompany)
        Return Nothing
    End Function
    Private Property _importProducts As New List(Of product)
    Friend ReadOnly Property importProducts As List(Of product)
        Get
            Return _importProducts
        End Get
    End Property
    Friend Function addImportProduct(product As product, player As player) As problem
        If _importProducts.Contains(product) Then Return New problem(Me, problemType.Duplicate)
        If demand(player) - _importProducts.Count < 1 Then Return New problem(Me, problemType.ExceedCapacity)

        product.importer = Me
        For Each consequence In product.cityConsequences
            addConsequence(consequence)
        Next
        _importProducts.Add(product)
        Return Nothing
    End Function
    Friend Function removeImportProduct(product As product) As problem
        If _importProducts.Contains(product) = False Then Return New problem(Me, problemType.NotFound)

        product.importer = Nothing
        _importProducts.Remove(product)
        Return Nothing
    End Function
    Friend ReadOnly Property demand(player As player) As Integer
        Get
            If player.marketingCampaigns.ContainsKey(Me) Then Return player.marketingCampaigns(Me) + 1 Else Return 1
        End Get
    End Property
    Friend ReadOnly Property income As Integer
        Get
            Dim total As Integer = 0
            For Each product In _importProducts
                total += product.income
            Next
            Return total
        End Get
    End Property

    Friend Function addConsequence(consequence As String) As problem
        Dim rawstr As String() = consequence.Split(" ")
        If rawstr(0) <> "city" Then Return New problem(Me, problemType.NotSuitable)

        Select Case rawstr(1)
            Case "policeGoodwill"
                Dim value As Integer = CInt(rawstr(2))
                _policeGoodwill = constrain(_policeGoodwill + value, 0, 20)
            Case "mediaGoodwill"
                Dim value As Integer = CInt(rawstr(2))
                _mediaGoodwill = constrain(_mediaGoodwill + value, 0, 20)
            Case "tlaGoodwill"
                Dim value As Integer = CInt(rawstr(2))
                _tlaGoodwill = constrain(_tlaGoodwill + value, 0, 20)
            Case "corruption"
                Dim value As Integer = CInt(rawstr(3))
                Dim god As god = enumArrays.getEnumFromString(rawstr(2), enumArrays.godArray)
                _corruption(god) = constrain(_corruption(god) + value, 0, 20)
        End Select
        Debug.Print(name & " " & consequence)
        Return Nothing
    End Function
    Private Property _policeGoodwill As Integer
    Private Property _mediaGoodwill As Integer
    Private Property _tlaGoodwill As Integer
        Get
            Return world.tlaGoodwill(continent)
        End Get
        Set(value As Integer)
            world.tlaGoodwill(continent) = value
        End Set
    End Property
    Private Property _corruption As New Dictionary(Of god, Integer)
    Friend ReadOnly Property visibilityModifier As Integer
        Get
            Dim total As Integer = 0
            Select Case _mediaGoodwill
                Case Is >= 16 : total -= 2
                Case 13 To 15 : total -= 1
                Case 5 To 7 : total += 2
                Case 1 To 4 : total += 4
                Case 0 : total += 5
            End Select
            Return total
        End Get
    End Property
    Friend ReadOnly Property missionDifficultyModifier As Integer
        Get
            Dim total As Integer = 0
            Select Case _tlaGoodwill
                Case Is >= 16 : total -= 1
                Case 1 To 4 : total += 1
            End Select
            Select Case _policeGoodwill
                Case Is >= 16 : total -= 1
                Case 5 To 7 : total += 1
                Case 1 To 4 : total += 2
                Case 0 : total += 5
            End Select
            Return total
        End Get
    End Property
    Friend ReadOnly Property missionProgressModifier As Integer
        Get
            Dim total As Integer = 0
            Select Case _tlaGoodwill
                Case 1 To 7 : total -= 5
                Case Is >= 13 : total += 5
            End Select
            Select Case _policeGoodwill
                Case Is >= 13 : total += 5
            End Select
            Return total
        End Get
    End Property

    Private Property _squads As New List(Of squad)
    Friend ReadOnly Property squads As List(Of squad)
        Get
            Return _squads
        End Get
    End Property
    Friend Function addSquad(squad As squad) As problem
        If _squads.Contains(squad) Then Return New problem(Me, problemType.Duplicate)

        _squads.Add(squad)
        Return Nothing
    End Function
    Friend Function removeSquad(squad As squad) As problem
        If _squads.Contains(squad) = False Then Return New problem(Me, problemType.NotFound)

        _squads.Remove(squad)
        For Each mission In _missions
            If mission.squad.Equals(squad) Then mission.squad = Nothing
        Next
        Return Nothing
    End Function

    Private Property _missions As New List(Of mission)
    Friend ReadOnly Property missions As List(Of mission)
        Get
            Return _missions
        End Get
    End Property
    Friend Function addMission(mission As mission) As problem
        If _missions.Contains(mission) Then Return New problem(Me, problemType.Duplicate)

        mission.city = Me
        _missions.Add(mission)
        Return Nothing
    End Function
    Friend Function removeMission(mission As mission) As problem
        If _missions.Contains(mission) = False Then Return New problem(Me, problemType.NotFound)

        mission.city = Nothing
        _missions.Remove(mission)
        Return Nothing
    End Function
End Class