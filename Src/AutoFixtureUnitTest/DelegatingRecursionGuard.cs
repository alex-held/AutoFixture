﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest
{
    public class DelegatingRecursionGuard : RecursionGuard
    {
        public DelegatingRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer, int recursionDepth)
            : base(builder, new DelegatingRecursionHandler(), comparer, recursionDepth)
        {
        }

        public DelegatingRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, new DelegatingRecursionHandler(), comparer, 1)
        {
        }

        public DelegatingRecursionGuard(ISpecimenBuilder builder, int recursionDepth)
            : base(builder, new DelegatingRecursionHandler(), recursionDepth)
        {
        }

        public DelegatingRecursionGuard(ISpecimenBuilder builder)
            : base(builder, new DelegatingRecursionHandler())
        {
        }

        [Obsolete]
        public override object HandleRecursiveRequest(object request)
        {
            return this.OnHandleRecursiveRequest(request);
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new DelegatingRecursionGuard(new CompositeSpecimenBuilder(builders));
        }

        internal IEnumerable<object> UnprotectedRecordedRequests
        {
            get { return this.RecordedRequests.Cast<object>(); }
        }

        internal Func<object, object> OnHandleRecursiveRequest { get; set; }
    }
}