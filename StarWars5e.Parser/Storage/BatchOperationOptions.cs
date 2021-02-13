namespace StarWars5e.Parser.Storage
{
    public class BatchOperationOptions
    {
        public BatchInsertMethod BatchInsertMethod { get; set; }
    }

    public enum BatchInsertMethod
    {
        Insert,
        InsertOrReplace,
        InsertOrMerge
    }
}
