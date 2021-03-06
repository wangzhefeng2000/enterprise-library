﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AExpense.Instrumentation
{
    [ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class SemanticLogCallHandler : ICallHandler
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "attributes")]
        public SemanticLogCallHandler(NameValueCollection attributes)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (getNext == null) throw new ArgumentNullException("getNext");

            AExpenseEvents.Log.LogCallHandlerPreInvoke(input.MethodBase.DeclaringType.FullName, input.MethodBase.Name);

            var sw = Stopwatch.StartNew();

            IMethodReturn result = getNext()(input, getNext);

            AExpenseEvents.Log.LogCallHandlerPostInvoke(input.MethodBase.DeclaringType.FullName, input.MethodBase.Name, sw.ElapsedMilliseconds);

            return result;
        }

        public int Order { get; set; }
    }
}