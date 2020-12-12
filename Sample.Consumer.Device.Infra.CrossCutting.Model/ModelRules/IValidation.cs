namespace Sample.Consumer.Device.Infra.CrossCutting.Model.ModelRules
{
    public interface IValidation
    {
        string Attribute { get; }

        string Message { get; }
    }
}
