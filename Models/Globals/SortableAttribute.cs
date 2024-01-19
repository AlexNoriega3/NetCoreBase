namespace Models.Globals
{
    public class SortableAttribute : Attribute
    {
        public string OrderBy { get; set; }
    }
}