using System;
using System.Collections.Generic;
using System.Text;

namespace FlowSpline.Domain.Common
{
    public abstract class AggregateRoot
    {
        private readonly List<object> _domainEvents = new();

        public IReadOnlyCollection<object> DomainEvents => _domainEvents;

        protected void AddDomainEvent(object @event)
        {
            _domainEvents.Add(@event);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
