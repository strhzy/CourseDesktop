using System.Collections.ObjectModel;

namespace DnDPartyManager.S;

public static class UnivHelper
{
    public static ObservableCollection<T> ListToObserv<T>(List<T> list)
    {
        ObservableCollection<T> obs = new ObservableCollection<T>();
        foreach (var el in list)
        {
            obs.Add(el);
        }
        return obs;
    }
}