using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CostCounter.Model;

namespace CostCounter.Test
{
    class FakeClock : IClock
    {
        private DateTime _nowTime;

        #region IClock
        public DateTime Now
        {
            get { return _nowTime; }
            set { _nowTime = value; }
        }
        #endregion
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

            _clock.Now = DateTime.Parse("2011/1/1 00:00:00");
            _keeper.Start();
            
            Assert.That(p1.StartTime, Is.EqualTo(_clock.Now));
        }

        [Test]
        public void 時間経過()
        {
            var p1 = new Participant("A", 1000);
            _keeper.AddMenber(p1);

            _clock.Now = DateTime.Parse("2011/1/1 00:00:00");
            _keeper.Start();
            _clock.Now = DateTime.Parse("2011/1/1 01:00:00");
            _keeper.Notify();

            Assert.That(p1.CostPerHour, Is.EqualTo(1000));
            Assert.That(_keeper.TotalCost, Is.EqualTo(1000));
        }
    }
}
