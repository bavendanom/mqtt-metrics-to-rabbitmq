﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace SendMessagesViaMqtt;

public class Program
{
    public static async Task Main(string[] args)
    {
        var tokenSource = new CancellationTokenSource();
        var keyboard = new KeyboardService();
        var randomizer = new Random(Seed: 125785);

        keyboard.AddKeyboardListener(forKey: ConsoleKey.Q, withMessage: "Press 'Q' key to stop running test scenarios", callbackFn: _ => {
            tokenSource.Cancel();
            var stopRunningKeyPressedEvents = true;
            return stopRunningKeyPressedEvents;
        });

        await Task.WhenAll(
            keyboard.RunKeyboardListeners(),

            RunTestingScenarios(whenFinished: () => keyboard.StopKeyboardListener(), randomizer, tokenSource.Token,
                new Scenario() {
                    Name = "3 devices sending 60 metrics each",
                    ClientId = "PLC001",
                    Devices = new Device[] {
                        new Device() { KeyToBind = ConsoleKey.A, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.S, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.D, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) }
                    },
                    MetricCountToSendPerDevice = 60,
                    StartingFromDate = DateTime.Now,
                    MillisecondsToWaitWhileSendingEachMetric = 1_000,
                    AddKeyboardListener = theParams => keyboard.AddKeyboardListener(forKey: theParams.forKey, callbackFn: theParams.callbackFn, withMessage: theParams.withMessage),
                    RemoveKeyboardListener = key => keyboard.RemoveKeyboardListener(forKey: key)
                },

                new Scenario() {
                    Name = "20 devices sending 500 metrics each",
                    ClientId = "PLC002",
                    Devices = new Device[] {
                        new Device() { KeyToBind = ConsoleKey.A, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.S, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.D, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.F, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.G, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.H, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.J, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.K, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.L, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.Z, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.X, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.V, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.D, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.B, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.N, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.M, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.W, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.E, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.R, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) },
                        new Device() { KeyToBind = ConsoleKey.T, Velocity = 600, WorkingForProductId = "002", GetApprovedCount = () => randomizer.Next(minValue: 1, maxValue: 10) }
                    },
                    MetricCountToSendPerDevice = 500,
                    StartingFromDate = DateTime.Now,
                    MillisecondsToWaitWhileSendingEachMetric = 1_000,
                    AddKeyboardListener = theParams => keyboard.AddKeyboardListener(forKey: theParams.forKey, callbackFn: theParams.callbackFn, withMessage: theParams.withMessage),
                    RemoveKeyboardListener = key => keyboard.RemoveKeyboardListener(forKey: key)
                }
            )
        );

        Console.WriteLine("\n ************* Finished runnig all scenarios *********************");
    }
    
    private static async Task RunTestingScenarios(Action whenFinished, Random withRandomizer, CancellationToken token, params Scenario[] scenarios)
    {
        foreach(var scenario in scenarios)
        {
            if(token.IsCancellationRequested)
                break;

            await scenario.RunAsync(withRandomizer, token);
        }

        whenFinished();
    }
}
