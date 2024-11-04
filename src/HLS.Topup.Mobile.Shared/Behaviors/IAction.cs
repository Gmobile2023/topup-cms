using Xamarin.Forms.Internals;

namespace HLS.Topup.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}