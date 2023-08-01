using MochaCore.Navigation;
using Moq;
using Xunit.Abstractions;

namespace MochaTests
{
    public class NavigationStackTest
    {
        private readonly NavigationStack<IDisposable> _stack;
        private Mock<IDisposable> _baseItem = new Mock<IDisposable>();
        private Mock<IDisposable> _item1 = new Mock<IDisposable>();
        private Mock<IDisposable> _item2 = new Mock<IDisposable>();
        private Mock<IDisposable> _item3 = new Mock<IDisposable>();

        public NavigationStackTest()
        {
            _stack = new(new IDisposable[]
{
                _baseItem.Object,
                _item1.Object,
                _item2.Object,
                _item3.Object
});
            _stack.DisposeOnRemove = true;
        }

        [Fact]
        public void NavigationStackMainTest()
        {
            Mock<IDisposable> item1new = new Mock<IDisposable>();
            Mock<IDisposable> item2new = new Mock<IDisposable>();
            Mock<IDisposable> item3new = new Mock<IDisposable>();

            Assert.Equal(4, _stack.Count); // Inital
            Assert.Equal(_baseItem.Object, _stack.InternalCollection.First());
            Assert.Equal(_item3.Object, _stack.InternalCollection.Last());
            Assert.True(_stack.TryMoveBack(2)); // MoveBack(2)
            Assert.Equal(1, _stack.CurrentIndex);
            Assert.Equal(_item1.Object, _stack.CurrentItem);
            Assert.False(_stack.TryMoveBack(3));
            _stack.OverwriteCurrent(item1new.Object); // Overwrite current
            Assert.Equal(1, _stack.CurrentIndex);
            Assert.Equal(item1new.Object, _stack.CurrentItem);
            Assert.Equal(4, _stack.Count);
            _item1.Verify(i => i.Dispose(), Times.Once);
            _stack.Push(item2new.Object); // Push
            _item2.Verify(i => i.Dispose(), Times.Once);
            _item3.Verify(i => i.Dispose(), Times.Once);
            Assert.Equal(2, _stack.CurrentIndex);
            Assert.Equal(3, _stack.Count);
            Assert.Equal(item2new.Object, _stack.CurrentItem);
            _stack.DisposeOnRemove = false;
            _stack.Clear(); // Clear
            Assert.Equal(1, _stack.Count);
            Assert.Equal(_baseItem.Object, _stack.CurrentItem);
            item1new.Verify(i => i.Dispose(), Times.Never);
            item2new.Verify(i => i.Dispose(), Times.Never);
        }

        [Fact]
        public void NavivgationStackClearTest()
        {
            _stack.Clear();
            _baseItem.Verify(i => i.Dispose(), Times.Never);
            _item1.Verify(i => i.Dispose(), Times.Once);
            _item2.Verify(i => i.Dispose(), Times.Once);
            _item3.Verify(i => i.Dispose(), Times.Once);

        }
    }


}