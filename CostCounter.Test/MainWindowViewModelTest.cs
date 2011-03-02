using CostCounter.ViewModel;
using NUnit.Framework;

namespace CostCounter.Test
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        [Test]
        public void 最初は開始可能で停止不可()
        {
            var model = new MainWindowViewModel();
            Assert.That(model.StartCommand.CanExecute(null), Is.True);
            Assert.That(model.StopCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void 開始すると開始不可で停止可能()
        {
            var model = new MainWindowViewModel();

            model.StartCommand.Execute(null);
            Assert.That(model.StartCommand.CanExecute(null), Is.False);
            Assert.That(model.StopCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void 停止すると開始可能で停止不可()
        {
            var model = new MainWindowViewModel();

            model.StartCommand.Execute(null);
            model.StopCommand.Execute(null);

            Assert.That(model.StartCommand.CanExecute(null), Is.True);
            Assert.That(model.StopCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void 参加者追加()
        {
            var model = new MainWindowViewModel();

            string property = null;
            model.PropertyChanged += (s, e) => property = e.PropertyName;

            model.Name = "Test";
            Assert.That(property, Is.EqualTo("Name"));

            model.CostPerHour = 1000;
            Assert.That(property, Is.EqualTo("CostPerHour"));

            model.AddParticipantCommand.Execute(null);
            Assert.That(model.Participants.Count, Is.EqualTo(1));
            Assert.That(property, Is.EqualTo("Participants"));
        }

        [Test]
        public void 参加者追加したら名前はクリアし単金はクリアしない()
        {
            var model = new MainWindowViewModel { Name = "Test", CostPerHour = 1000 };

            model.AddParticipantCommand.Execute(null);

            Assert.That(model.Name, Is.EqualTo(""));
            Assert.That(model.CostPerHour, Is.EqualTo(1000));
        }

        [Test]
        public void インターバルをセット()
        {
            var model = new MainWindowViewModel();

            string property = null;
            model.PropertyChanged += (s, e) => property = e.PropertyName;

            model.Interval = 60;
            Assert.That(property, Is.EqualTo("Interval"));
            Assert.That(model.Interval, Is.EqualTo(60));
        }
    }
}
