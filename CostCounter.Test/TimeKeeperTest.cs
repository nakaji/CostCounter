using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CostCounter.Model;

namespace CostCounter.Test
{
    class FakeClock : Clock
    {
        private DateTime _nowTime;

        #region Clock
        public override DateTime Now
        {
            get { return _nowTime; }
        }
        #endregion

        public void SetNow(DateTime value)
        {
            _nowTime = value;
        }

    }

    [TestFixture]
    public class TimeKeeperTest
    {
        private TimeKeeper _keeper;
        private FakeClock _clock;

        [SetUp]
        public void SetUp()
        {
            _clock = new FakeClock();
            _keeper = new TimeKeeper(_clock);
            Assert.That(_keeper.IsRunning, Is.False);
        }

        [Test]
        public void メンバの追加()
        {
            _keeper.AddMenber(new Participant("A", 1000));
            Assert.That(_keeper.Participants.Count, Is.EqualTo(1));
        }

        [Test]
        public void 開始()
        {
            var p1 = new Participant("A", 1000);
            _keeper.AddMenber(p1);

            _clock.SetNow(DateTime.Parse("2011/1/1 00:00:00"));
            _keeper.Start();

            Assert.That(p1.StartTime, Is.EqualTo(_clock.Now));
            Assert.That(_keeper.IsRunning, Is.True);
        }

        [Test]
        public void 時間経過()
        {
            var p1 = new Participant("A", 1000);
            _keeper.AddMenber(p1);

            _clock.SetNow(DateTime.Parse("2011/1/1 00:00:00"));
            _keeper.Start();
            _clock.SetNow(DateTime.Parse("2011/1/1 01:00:00"));
            _keeper.Notify();

            Assert.That(p1.CostPerHour, Is.EqualTo(1000));
            Assert.That(_keeper.TotalCost, Is.EqualTo(1000));
        }

        [Test]
        public void 停止()
        {
            _keeper.AddMenber(new Participant("A", 1000));
            _keeper.AddMenber(new Participant("B", 2000));

            _clock.SetNow(DateTime.Parse("2011/1/1 00:00:00"));
            _keeper.Start();
            _clock.SetNow(DateTime.Parse("2011/1/1 01:00:00"));
            _keeper.Stop();
            _clock.SetNow(DateTime.Parse("2011/1/1 01:30:00"));
            _keeper.Notify();

            Assert.That(_keeper.TotalCost, Is.EqualTo((1000 + 2000) * 1));
            Assert.That(_keeper.IsRunning, Is.False);
        }
    }
}
