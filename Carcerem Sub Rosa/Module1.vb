Module Module1

    Sub Main()
        Console.SetWindowSize(80, 70)

        Dim world As world = world.buildWorld
        Dim player As New player
        Dim researchProject As New researchProject("Top-Secret Project", 1000, Nothing, Nothing)
        player.devAddResearchProjectOpen(researchProject)
        player.changeResearchProject(researchProject)

        Dim city1 As city = world.cities(continent.Africa)(0)
        Dim city2 As city = world.cities(continent.China)(0)
        Dim citysite1 As New citysite(player, city1)
        Dim citysite2 As New citysite(player, city1)
        Dim citysite3 As New citysite(player, city1)
        Dim citysite4 As New citysite(player, city1)
        Dim shell1 As New shellcompany("", player, city1)
        Dim shell2 As New shellcompany("", player, city2)
        Dim research As New researchlab("", citysite1, shell2)
        Dim factory As New factory("", citysite2, shell1)
        Dim safehouse As New safehouse("", citysite3, shell1)
        Dim tacsupport As New tacsupport("", citysite4, shell1)
        Dim laundry As New laundry("", shell1)

        Dim blueprint As New product("Sheepskin")
        player.addBlueprint(blueprint)
        city2.addDemand(1)
        player.createTradeRoute(blueprint, factory, city2)

        Dim agent1 As agent = agent.buildRandomAgent(player)
        Dim agent2 As agent = agent.buildRandomAgent(player)
        Dim squad As squad = squad.buildRandomSquad(player, city1)
        squad.addAgent(agent1)
        squad.addAgent(agent2)
        agent1.changeRelationship(agent2, choiceComponent.Practical)
        squad.moveTo(city2)

        While True
            Console.Clear()
            player.consoleReport(1)
            Console.WriteLine(vbCrLf)

            shell1.consoleReport(1)
            For Each holding In shell1.holdings
                Console.WriteLine()
                holding.consoleReport(2)
            Next
            Console.WriteLine(vbCrLf)

            shell2.consoleReport(1)
            For Each holding In shell2.holdings
                Console.WriteLine()
                holding.consoleReport(2)
            Next
            Console.WriteLine(vbCrLf)

            squad.consoleReport(1)
            Console.ReadKey()

            player.tick()
        End While
    End Sub
End Module
