namespace TiaFileFormatWasm.Classes.Response
{
    public class ItemResult
    {
        public string Name { get; set; }

        public ItemType ItemType { get; set; }

        public byte[] Data { get; set; }

        public string StringData { get; set; }
    }
}
