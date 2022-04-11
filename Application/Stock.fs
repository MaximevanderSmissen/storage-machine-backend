/// Provides functionality (use-cases) related to stock (mostly bins) stored in the Storage Machine.
module StorageMachine.Stock.Stock

open StorageMachine
open Common
open Bin

/// Defines errors that the application can throw
type StoreBinError =
    | BinAlreadyStored

/// Defines data access operations for stock functionality.
type IStockDataAccess =

    /// Retrieve all bins currently stored in the Storage Machine.
    abstract RetrieveAllBins : unit -> List<Bin>
    /// Store the given bin in the Storage Machine.
    abstract StoreBin : Bin -> Result<unit, StoreBinError>

/// An overview of all bins currently stored in the Storage Machine.
let binOverview (dataAccess : IStockDataAccess) : List<Bin> =
    // Trivially
    dataAccess.RetrieveAllBins ()

/// An overview of actual stock currently stored in the Storage Machine. Actual stock is defined as all non-empty bins.
let stockOverview (dataAccess : IStockDataAccess) : List<Bin> =
    // Perform I/O
    let allBins = dataAccess.RetrieveAllBins ()
    // Use the model which provides the definition of a bin being (non-)empty
    let actualStock = allBins |> List.filter Bin.isNotEmpty
    actualStock

/// Store a bin in the Storage Machine.
let storeBin (dataAccess : IStockDataAccess) (bin : Bin) : string =
    match dataAccess.StoreBin bin with
        | Ok () -> $"Bin with ID: {bin.Identifier} was stored."
        | Error BinAlreadyStored -> $"Error, did not store bin! Bin with ID: {bin.Identifier} is already stored in this Storage Machine."