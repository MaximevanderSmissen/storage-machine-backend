/// Data access implementation of the Stock component.
module StorageMachine.Stock.Stock

open StorageMachine
open Bin
open Stock
open StorageMachine.SimulatedDatabase

/// Data access operations of the Stock component implemented using the simulated in-memory DB.
let stockPersistence =
    { new IStockDataAccess with

        member this.RetrieveAllBins() =
            SimulatedDatabase.retrieveBins ()
            |> Set.map (fun binIdentifier ->
                { Identifier = binIdentifier
                  Content = SimulatedDatabase.retrieveStock () |> Map.tryFind binIdentifier })
            |> Set.toList

        member this.StoreBin bin =
            let storageResult = SimulatedDatabase.storeBin bin

            match storageResult with
            | Ok() -> Ok bin
            | Error BinAlreadyStored -> Error StockDataError.BinAlreadyStored }
