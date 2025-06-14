namespace Diplom
{
    public class EmployeeObservable
    {
        private readonly List<EmployeeObserver> observers = new List<EmployeeObserver>();

        public void AddObserver(EmployeeObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(EmployeeObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(string action, object data)
        {
            foreach (var observer in observers)
            {
                observer.Update(action, data);
            }
        }
    }
}
