namespace Paps
{
    public interface IValidation
    {
        bool IsValid();
    }

    public interface IValidation<in TParameter>
    {
        bool IsValid(TParameter parameter);
    }
}
