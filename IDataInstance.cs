namespace BindingRx
{
    /// <summary>
    ///     Entity which indicate and take access for data binding data source
    /// </summary>
    public interface IDataInstance
    {
        /// <summary>
        ///     Source of data binding
        /// </summary>
        object DataInstance { get; set; }
    }
}