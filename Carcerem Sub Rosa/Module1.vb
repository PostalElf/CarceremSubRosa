Module Module1

    Sub Main()
        Console.SetWindowSize(80, 70)

        Dim world As world = world.buildWorld
        Dim player As New player
        Dim researchProject As New researchProject("Top-Secret Project", 1000, Nothing, Nothing)
        player.devAddResearchProjectOpen(researchProject)
        player.changeResearchProject(researchProject)

        Dim city1 As city = world.cities(continent.Africa)(1)
        Dim city2 As city = world.cities(continent.China)(0)
        Dim citysite1 As New citysite(player, city1)
        Dim citysite2 As New citysite(player, city1)
        Dim citysite3 As New citysite(player, city1)
        Dim citysite4 As New citysite(player, city1)
        Dim shell As New shellcompany("", player, city1)
        Dim research As New researchlab("", citysite1, shell)
        Dim factory As New factory("", citysite2, shell)
        Dim safehouse As New safehouse("", citysite3, shell)
        Dim tacsupport As New tacsupport("", citysite4, shell)
        Dim laundry As New laundry("", shell)

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


        While True
            Console.Clear()
            player.consoleReport(1)
            Console.WriteLine(vbCrLf)

            shell.consoleReport(1)
            For Each holding In shell.holdings
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
