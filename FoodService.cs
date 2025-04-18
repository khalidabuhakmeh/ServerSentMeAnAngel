using System.ComponentModel;
using System.Runtime.CompilerServices;
using Injectio.Attributes;

namespace ServerSentMeAnAngel;

[RegisterSingleton(Registration = RegistrationStrategy.Self)]
public class FoodService : INotifyPropertyChanged
{
    public FoodService()
    {
        Current = Foods[Random.Shared.Next(Foods.Length)];
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private static readonly string[] Foods = ["🍔", "🍟", "🥤", "🍤", "🍕", "🌮", "🥙"];

    private string Current
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public async IAsyncEnumerable<string> GetCurrent(
        [EnumeratorCancellation] CancellationToken ct)
    {
        while (ct is not { IsCancellationRequested: true })
        {
            yield return Current;
            var tcs = new TaskCompletionSource();
            PropertyChangedEventHandler handler = (_, _) => tcs.SetResult();
            PropertyChanged += handler;
            try
            {
                await tcs.Task.WaitAsync(ct);
            }
            finally
            {
                PropertyChanged -= handler;
            }
        }
    }

    public void Set()
    {
        Current = Foods[Random.Shared.Next(Foods.Length)];
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}