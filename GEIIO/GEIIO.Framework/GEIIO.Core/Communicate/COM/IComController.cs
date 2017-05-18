namespace GEIIO.Communicate.COM
{
    internal interface IComController : IController
    {
        IComSession ComChannel { get; set; }
    }
}
