namespace ReferenceAPI.ErrorViewModel
{
    public class ResultViewModel<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; private set; } = new();
    }
}
