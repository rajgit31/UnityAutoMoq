using System;
using NUnit.Framework;
using Moq;
using NUnit.Framework.SyntaxHelpers;

namespace UnityAutoMoq.Test
{
    [TestFixture]
    public class UnityAutoMoqContainerFixture
    {
        private UnityAutoMoqContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new UnityAutoMoqContainer();
        }

        [Test]
        public void Can_get_instance_without_registering_it_first()
        {
            var mocked = container.Resolve<IService>();

            Assert.That(mocked, Is.Not.Null);
        }

        [Test]
        public void Can_get_mock()
        {
            Mock<IService> mock = container.GetMock<IService>();

            Assert.That(mock, Is.Not.Null);
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same()
        {
            Mock<IService> mock = container.GetMock<IService>();
            var mocked = container.Resolve<IService>();
            
            Assert.That(mock.Object, Is.SameAs(mocked));
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same_order_independent()
        {
            var mocked = container.Resolve<IService>();
            Mock<IService> mock = container.GetMock<IService>();

            Assert.That(mock.Object, Is.SameAs(mocked));
        }

        [Test]
        public void Should_apply_default_value_when_creating_mocks()
        {
            container.DefaultValue = DefaultValue.Mock;
            var mocked = container.GetMock<IService>();

            Assert.That(mocked.DefaultValue, Is.EqualTo(DefaultValue.Mock));
        }

        [Test]
        public void Can_resolve_concrete_type_with_dependency()
        {
            var concrete = container.Resolve<Service>();

            Assert.That(concrete, Is.Not.Null);
            Assert.That(concrete.AnotherService, Is.Not.Null);
        }

        [Test]
        public void Getting_mock_after_resolving_concrete_type_should_return_the_same_mock_as_passed_as_argument_to_the_concrete()
        {
            var concrete = container.Resolve<Service>();
            Mock<IAnotherService> mock = container.GetMock<IAnotherService>();

            Assert.That(concrete.AnotherService, Is.SameAs(mock.Object));
        }

        [Test]
        public void Can_add_one_interface_implementation_when_getting_mock()
        {
            Mock<IService> mock = container.As<IDisposable>().GetMock<IService>();

            Assert.That(mock.As<IDisposable>().Object, Is.Not.Null);
        }

        [Test]
        public void Can_add_several_interface_implementations_when_getting_mock()
        {
            Mock<IService> mock = container.As<IDisposable>().And<IAnotherService>().GetMock<IService>();

            Assert.That(mock.As<IDisposable>().Object, Is.Not.Null);
            Assert.That(mock.As<IAnotherService>().Object, Is.Not.Null);
        }
    }
}