namespace Sample.Consumer.Device.Infra.CrossCutting.Background
{
    public class Delegates
    {
        public delegate void ActionDelegate(int numberOfCurrentExection, ref bool stop);
    }
}
