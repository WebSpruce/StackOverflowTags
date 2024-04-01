using System.Reflection;
using System.Transactions;
using Xunit.Sdk;

namespace StackOverflowTags.Tests
{
    public class Isolated : BeforeAfterTestAttribute
    {
        private TransactionScope transactionScope;
        public override void Before(MethodInfo methodUnderTest)
        {
            base.Before(methodUnderTest);
            transactionScope = new TransactionScope();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            base.After(methodUnderTest);
            transactionScope.Dispose();
        }
    }
}
