using System;
using System.Collections.Generic;

namespace KuroNeko.Utilities.DesignPattern
{
    public interface IObserver
    {
        public abstract void Update(Observer observer);
    }

    public interface ISubject
    {
        public abstract void Attach(IObserver observer);
        public abstract void Detach(IObserver observer);
        public abstract void Notify();
    }

    public class Observer : ISubject
    {
        protected List<IObserver> observers = new();

        public void Attach(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Detach(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}