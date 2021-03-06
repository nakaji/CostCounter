﻿using System;
using CostCounter.Model;
using NUnit.Framework;

namespace CostCounter.Test
{
    [TestFixture]
    public class ParticipantTest
    {
        private Participant _participant;

        [SetUp]
        public void SetUp()
        {
            _participant = new Participant("name", 5000);
            Assert.That(_participant.Name, Is.EqualTo("name"));
            Assert.That(_participant.CostPerHour, Is.EqualTo(5000));
        }

        [Test]
        public void 会議を開始()
        {
            Assert.That(_participant.StartTime, Is.EqualTo(DateTime.MinValue));
            _participant.Start(new DateTime(2011, 2, 1, 21, 10, 5));
            Assert.That(_participant.StartTime, Is.EqualTo(DateTime.Parse("2011/2/1 21:10:5")));
        }

        [Test]
        public void 経過時間を計算する()
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Notify(DateTime.Parse("2011/2/1 01:02:03"));
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(1, 2, 3)));
        }

        [Test]
        public void 一時停止された場合も停止までの経過時間を計算する()
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Notify(DateTime.Parse("2011/2/1 01:00:00"));
            _participant.Stop(DateTime.Parse("2011/2/1 01:10:00"));
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(1, 10, 0)));
        }

        [Test]
        public void 途中で一時停止された場合はカウントをストップする()
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Notify(DateTime.Parse("2011/2/1 01:00:00"));
            _participant.Stop(DateTime.Parse("2011/2/1 01:10:00"));
            _participant.Notify(DateTime.Parse("2011/2/1 02:00:00"));
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(1, 10, 0)));
        }

        [Test]
        public void 再開された場合は一時中止時間を含まない()
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Stop(DateTime.Parse("2011/2/1 01:10:00"));

            _participant.Start(DateTime.Parse("2011/2/1 02:00:00"));
            _participant.Notify(DateTime.Parse("2011/2/1 02:30:00"));
            Assert.That(_participant.StartTime, Is.EqualTo(DateTime.Parse("2011/2/1 00:00:00")));
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(1, 40, 0)));
        }

        [Test]
        public void 二回目の停止は無視()
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Stop(DateTime.Parse("2011/2/1 01:10:00"));
            _participant.Stop(DateTime.Parse("2011/2/1 02:10:00"));     //無視
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(1, 10, 0)));
        }

        [Test]
        public void 二回目の開始は無視()
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Start(DateTime.Parse("2011/2/1 01:10:00"));    //無視
            _participant.Notify(DateTime.Parse("2011/2/1 02:10:00"));
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(2, 10, 0)));
        }

        [Test]
        public void 開始を忘れても通知があれば開始する()
        {
            _participant.Notify(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Notify(DateTime.Parse("2011/2/1 00:30:00"));
            Assert.That(_participant.Elaps, Is.EqualTo(new TimeSpan(0, 30, 0)));
        }

        [Test]
        [TestCase("2011/2/1 01:00:00", 5000)]
        [TestCase("2011/2/1 01:30:00", 7500)]
        public void コスト計算(string notifyedTime, int totalCost)
        {
            _participant.Start(DateTime.Parse("2011/2/1 00:00:00"));
            _participant.Notify(DateTime.Parse(notifyedTime));
            Assert.That(_participant.Cost, Is.EqualTo(totalCost));
        }
    }
}
