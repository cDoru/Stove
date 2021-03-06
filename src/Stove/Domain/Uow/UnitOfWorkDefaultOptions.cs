using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Stove.Domain.Uow
{
    internal class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
    {
        private readonly List<DataFilterConfiguration> _filters;

        public UnitOfWorkDefaultOptions()
        {
            _filters = new List<DataFilterConfiguration>();
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
        }

        public TransactionScopeOption Scope { get; set; }

        /// <inheritdoc />
        public bool IsTransactional { get; set; }

        /// <inheritdoc />
        public TimeSpan? Timeout { get; set; }

        /// <inheritdoc />
        public IsolationLevel? IsolationLevel { get; set; }

        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get { return _filters; }
        }

        public void RegisterFilter(string filterName, bool isEnabledByDefault)
        {
            if (_filters.Any(f => f.FilterName == filterName))
            {
                throw new StoveException("There is already a filter with name: " + filterName);
            }

            _filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }

        public void OverrideFilter(string filterName, bool isEnabledByDefault)
        {
            _filters.RemoveAll(f => f.FilterName == filterName);
            _filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }
    }
}
